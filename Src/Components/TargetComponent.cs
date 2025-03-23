using Godot;
using LooksLike.Ecs;

namespace Game.Components;

[GlobalClass]
public partial class TargetComponent : EcsComponent
{
    [Export] public Node3D? Node;
    [Export] public Vector3 Position;
}
