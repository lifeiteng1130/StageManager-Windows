using StageManager.Core;

namespace StageManager.Tests;

public class FrameStabilityTests
{
    [Fact]
    public void Resizing_or_missing_windows_delay_handoff()
    {
        var tracker = new FrameStability();
        var normal = new Dictionary<long, Area> { [1] = new(0, 0, 800, 600) };
        var maximized = new Dictionary<long, Area> { [1] = new(0, 0, 1920, 1080) };
        Assert.False(tracker.Observe(normal, 1));
        Assert.False(tracker.Observe(maximized, 1));
        Assert.True(tracker.Observe(maximized, 1));
        Assert.False(tracker.Observe(maximized, 2));
        Assert.False(tracker.Observe(new Dictionary<long, Area>(), 1));
        Assert.False(tracker.Observe(maximized, 1));
    }
}
