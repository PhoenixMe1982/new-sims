using Godot;
using Godot.Collections;

[Tool]
[GlobalClass]
public partial class PlayerParts : Resource
{
    public enum HeadType
    {
        Default,
        One,
        Two,
        Three,
    }

    public enum HairType
    {
        Default,
        One,
        Two,
        Three,
    }


    [Export] public Dictionary<HeadType, PlayerPart> Heads = new();
    [Export] public Dictionary<HairType, PlayerPart> Hairs = new();
}
