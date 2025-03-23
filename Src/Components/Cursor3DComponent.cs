using LooksLike.Ecs;
using Godot;

namespace Game.Components;

[GlobalClass]
public partial class Cursor3DComponent : EcsComponent
{
    [Export] public Vector3 Position;
    [Export] public Node3D? Node;
}
