using Godot;
using LooksLike.Ecs;

namespace Game.Components;

[GlobalClass]
public partial class PlayerCustomizerComponent : EcsComponent
{
    [Export] public Button? BodyButton;
    [Export] public Button? HairButton;
    [Export] public Button? HeadButton;
    [Export] public Button? ItemButton;
    [Export] public Button? ConfirmButton;
}
