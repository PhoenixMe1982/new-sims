using Godot;
using LooksLike.Ecs;

namespace Game.Components;

[GlobalClass]
public partial class CharAnimationsComponent : EcsComponent
{
    [Export] public string CurentState = "";
}
