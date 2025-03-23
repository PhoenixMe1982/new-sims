using Godot;
using LooksLike.Ecs;

namespace Game.Components;

[GlobalClass]
public partial class InteractableComponent : EcsComponent
{
    [Export] public Area3D? Node;
}
