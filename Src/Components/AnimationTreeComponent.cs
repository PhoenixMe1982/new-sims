using Godot;
using LooksLike.Ecs;

namespace Game.Components;

[GlobalClass]
public partial class AnimationTreeComponent : EcsComponent
{
    [Export] public AnimationTree? Node;
}
