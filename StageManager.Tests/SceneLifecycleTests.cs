using StageManager.Core;

namespace StageManager.Tests;

public class SceneLifecycleTests
{
	[Fact]
	public void Explicit_click_bypasses_foreground_bounce_guard()
	{
		var registry = new SceneRegistry();
		var groups = registry.Initialize(new[] { (1L, "IDEA"), (2L, "Chrome") });
		Assert.True(registry.TrySwitch(groups[0].Id));
		Assert.True(registry.TrySwitch(groups[1].Id));
		Assert.False(registry.TrySwitch(groups[0].Id));
		Assert.True(registry.TrySwitch(groups[0].Id, userInitiated: true));
	}
    [Fact]
    public void Multiple_apps_stay_together_when_switching_and_receiving_new_windows()
    {
        var registry = new SceneRegistry(reentrancyWindow: TimeSpan.Zero);
        registry.Initialize(new[] { (1L, "IDEA"), (2L, "Chrome"), (3L, "DBeaver") });
        var development = registry.FindGroupId(1)!.Value;
        registry.Merge(registry.FindGroupId(2)!.Value, development);
        var database = registry.FindGroupId(3)!.Value;
        Assert.True(registry.TrySwitch(development));
        Assert.Equal(development, registry.Place(4, "Chrome").GroupId);
        Assert.True(registry.TrySwitch(database));
        Assert.True(registry.TrySwitch(development));
        Assert.Equal(new long[] { 1, 2, 4 }, registry.WindowsOf(development));
        registry.Remove(2);
        Assert.Equal(development, registry.FindGroupId(4));
    }

    [Fact]
    public void A_queued_switch_cannot_select_a_scene_after_shutdown()
    {
        var registry = new SceneRegistry();
        var groups = registry.Initialize(new[] { (1L, "IDEA"), (2L, "Chrome") });
        registry.TrySwitch(groups[0].Id);
        registry.Stop();
        Assert.False(registry.TrySwitch(groups[1].Id));
        Assert.Null(registry.CurrentId);
    }

    [Fact]
    public void Initialize_allows_a_new_session_after_shutdown()
    {
        var registry = new SceneRegistry();
        registry.Stop();
        var group = Assert.Single(registry.Initialize(new[] { (1L, "IDEA") }));
        Assert.True(registry.TrySwitch(group.Id));
    }
}
