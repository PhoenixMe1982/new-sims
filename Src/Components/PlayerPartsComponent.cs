using Godot;
using LooksLike.Ecs;

namespace Game.Components;

[GlobalClass]
public partial class PlayerPartsComponent : EcsComponent
{
    [Export] public Skeleton3D? Skeleton;
    [Export] public BoneAttachment3D? HeadAttachment;
    [Export] public PlayerParts? Parts;
    [Export] public PlayerParts.HeadType HeadType = PlayerParts.HeadType.Default;
    [Export] public PlayerParts.HairType HairType = PlayerParts.HairType.Default;
    [Export] public Node3D? DefaultHeadNode;
    [Export] public Node3D? DefaultHairNode;

    public bool NeedsUpdate = false;
    public MeshInstance3D? HeadNode;
    public MeshInstance3D? HairNode;
}
