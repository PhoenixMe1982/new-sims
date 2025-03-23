using Godot;

[Tool]
[GlobalClass]
public partial class PlayerPart : Resource
{
    [Export] public Mesh? Mesh;
    [Export] public Resource? Skin;
    [Export] public Texture2D? Icon;
}
