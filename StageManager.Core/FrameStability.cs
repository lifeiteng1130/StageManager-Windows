namespace StageManager.Core;

/// <summary>Two consecutive visible geometry samples are required before removing a flight overlay.</summary>
public sealed class FrameStability
{
    private readonly Dictionary<long, Area> _previous = new();

    public bool Observe(IReadOnlyDictionary<long, Area> frames, int expectedCount)
    {
        var stable = frames.Count == expectedCount && expectedCount > 0
            && frames.All(pair => _previous.TryGetValue(pair.Key, out var prior) && prior == pair.Value);
        _previous.Clear();
        foreach (var pair in frames) _previous[pair.Key] = pair.Value;
        return stable;
    }
}
