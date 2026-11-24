using StageManager.Native.Interop;
using StageManager.Native.PInvoke;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StageManager.Native;

/// <summary>Suppress only the native restore animation; preserve each application's previous DWM setting.</summary>
internal static class WindowTransitionGuard
{
    private sealed record Entry(int ProcessId, bool Original, long Generation);
    private static readonly Dictionary<IntPtr, Entry> Entries = new();
    private static long _generation;

    public static void Suppress(IntPtr handle)
    {
        Win32.GetWindowThreadProcessId(handle, out var pid);
        if (pid == 0) return;
        if (!Entries.TryGetValue(handle, out var prior) || prior.ProcessId != (int)pid)
        {
            if (NativeMethods.DwmGetWindowAttribute(handle, (DWMWINDOWATTRIBUTE)3, out var original, sizeof(int)) != 0)
                return;
            // The scene flight overlay (or the application itself) already owns suppression.
            if (original) return;
            prior = new Entry((int)pid, original, 0);
        }
        var entry = prior with { Generation = ++_generation };
        if (!NativeMethods.SetTransitionsDisabled(handle, true)) return;
        Entries[handle] = entry;
        RestoreLater(handle, entry.Generation);
    }

    private static async void RestoreLater(IntPtr handle, long generation)
    {
        try
        {
            await Task.Delay(350);
            if (Entries.TryGetValue(handle, out var entry) && entry.Generation == generation)
                Restore(handle, entry);
        }
        catch (Exception ex) { Log.Error("Restoring native window animation failed", ex); }
    }

    private static void Restore(IntPtr handle, Entry entry)
    {
        Win32.GetWindowThreadProcessId(handle, out var pid);
        if (Win32.IsWindow(handle) && pid == entry.ProcessId)
            NativeMethods.SetTransitionsDisabled(handle, entry.Original);
        Entries.Remove(handle);
    }

    public static void RestoreAll()
    {
        foreach (var pair in new List<KeyValuePair<IntPtr, Entry>>(Entries))
            Restore(pair.Key, pair.Value);
    }
}
