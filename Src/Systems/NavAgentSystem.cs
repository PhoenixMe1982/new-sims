using System.Collections.Generic;
using LooksLike.DependencyInjection;
using LooksLike.Ecs;
using LooksLike.Utils;
using LooksLike.Framework.Components;
using Game.Components;
using Godot;

namespace Game.Systems;

public class NavAgentSystem : SystemBase, IEntitiesUpdate
{
    [Inject] private EcsWorld _world = null!;

    public NavAgentSystem() : base(new EcsFilter()
        .With<Transform3DComponent>()
        .With<MoveComponent>()
        .With<CharacterBodyComponent>()
        .With<NavAgentComponent>())
    {
    }

    public void EntitiesUpdate(Dictionary<ulong, EcsEntity> entities, float deltaTime)
    {
        foreach (var (_, entity) in entities)
        {
            var agentComponent = entity.GetComponent<NavAgentComponent>()!;
            var trComponent = entity.GetComponent<Transform3DComponent>()!;
            var body = entity.GetComponent<CharacterBodyComponent>()!.Node;
            var visual = entity.GetComponent<CharacterBodyComponent>()?.Visual;
            var move = entity.GetComponent<MoveComponent>()!;
            var transform = trComponent.Transform3D;
            var agent = agentComponent.Node;

            if (agent == null || body == null)
                continue;

            agent.TargetPosition = agentComponent.TargetPosition;

            var destination = agent.GetNextPathPosition();
            var direction = transform.Origin - destination;
            move.Direction = direction.Normalized();

            if (direction.Length() < 0.6f)
                entity.RemoveComponent<MoveComponent>();

            if (visual != null)
            {
                float angle = Mathf.Atan2(move.Direction.X, move.Direction.Z);
                visual.Rotation = new Vector3(0, angle + Mathf.Pi, 0);
            }
        }
    }
}
