using Godot;
using LooksLike.Ecs;

namespace Game.Components;

[GlobalClass]
public partial class NavAgentComponent : EcsComponent
{
    [Export] public NavigationAgent3D? Node;
    [Export] public Vector3 TargetPosition;
}
