using System.Collections.Generic;
using LooksLike.DependencyInjection;
using LooksLike.Ecs;
using LooksLike.Utils;
using LooksLike.Framework.Components;
using Game.Components;
using Godot;
using System.Linq;

namespace Game.Systems;

public class PlayerPartsSystem : SystemBase, IEntitiesAdded, IEntitiesUpdate
{
    [Inject] private EcsWorld _world = null!;

    public PlayerPartsSystem() : base(new EcsFilter()
        .With<Transform3DComponent>()
        .With<PlayerPartsComponent>())
    {
    }

    public static PlayerPartsComponent? GetPlayerParts(EcsWorld world)
    {
        var filter = new EcsFilter().With<PlayerPartsComponent>();
        var entities = world.GetFilteredEntities(filter);
        var firstEntity = entities.FirstOrDefault();
        return firstEntity.Value.GetComponent<PlayerPartsComponent>();
    }

    public void EntitiesAdded(Dictionary<ulong, EcsEntity> entities)
    {
        foreach (var (_, entity) in entities)
        {
            var trComponent = entity.GetComponent<Transform3DComponent>()!;
            var playerParts = entity.GetComponent<PlayerPartsComponent>()!;
            var skeleton = playerParts.Skeleton;

            if (playerParts == null || skeleton == null)
                continue;

            PartsUpdate(playerParts, skeleton);
        }
    }

    public void EntitiesUpdate(Dictionary<ulong, EcsEntity> entities, float deltaTime)
    {
        foreach (var (_, entity) in entities)
        {
            var trComponent = entity.GetComponent<Transform3DComponent>()!;
            var playerParts = entity.GetComponent<PlayerPartsComponent>()!;
            var skeleton = playerParts.Skeleton;

            if (playerParts == null || !playerParts.NeedsUpdate || skeleton == null)
                continue;

            PartsUpdate(playerParts, skeleton);
        }
    }

    public void PartsUpdate(PlayerPartsComponent playerParts, Skeleton3D skeleton)
    {

        var head = playerParts.HeadNode;
        var hair = playerParts.HairNode;

        playerParts.DefaultHeadNode.Visible = head == null;
        playerParts.DefaultHairNode.Visible = hair == null;

        if (head != null)
            head.QueueFree();
        if (hair != null)
            hair.QueueFree();


        if (playerParts.HeadType != PlayerParts.HeadType.Default)
        {
            var headPart = playerParts.Parts?.Heads[playerParts.HeadType];
            var headMesh = headPart?.Mesh;
            var headSkin = headPart?.Skin as Skin;

            if (headMesh != null && headSkin != null)
            {
                var headNode = new MeshInstance3D();
                headNode.Mesh = headMesh;
                headNode.Skin = headSkin;
                skeleton.AddChild(headNode);
                headNode.Visible = true;
                playerParts.HeadNode = headNode;
            }
        }

        if (playerParts.HairType != PlayerParts.HairType.Default)
        {
            var hairPart = playerParts.Parts?.Hairs[playerParts.HairType];
            var hairMesh = hairPart?.Mesh;
            var hairSkin = hairPart?.Skin as Skin;

            if (hairMesh != null && hairSkin != null)
            {
                var hairNode = new MeshInstance3D();
                hairNode.Mesh = hairMesh;
                hairNode.Skin = hairSkin;
                skeleton.AddChild(hairNode);
                hairNode.Visible = true;
                playerParts.HairNode = hairNode;
            }
        }

        playerParts.NeedsUpdate = false;
    }
}
