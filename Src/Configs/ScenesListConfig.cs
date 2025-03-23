using Godot;
using Godot.Collections;

namespace Game.Configs;

public enum Location
{
    None,
    Home,
    Customizer,
}


[GlobalClass]
public partial class ScenesListConfig : Resource
{
    // [Export] public Dictionary<Location, PackedScene> LocationScenes = new();
    [Export] public Dictionary<Location, string> ScenePath = new();
}
