using Godot;
using LooksLike.Ecs;
using static Game.Systems.StartInteractionSystem;

namespace Game.Components;

[GlobalClass]
public partial class StartInteractionComponent : EcsComponent
{
    [Export] public Label3D? Node;
    [Export] public InteractionType Type;
    [Export] public bool Enabled = true;
}
