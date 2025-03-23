using System.Collections.Generic;
using LooksLike.DependencyInjection;
using LooksLike.Ecs;
using LooksLike.Utils;
using LooksLike.Framework.Components;
using Game.Components;
using Godot;

namespace Game.Systems;

public class CharAnimationSystem : SystemBase, IEntitiesUpdate
{
    [Inject] private EcsWorld _world = null!;

    public CharAnimationSystem() : base(new EcsFilter()
        .With<Transform3DComponent>()
        // .With<MoveComponent>()
        .With<AnimationTreeComponent>()
        .With<CharAnimationsComponent>()
        .With<CharacterBodyComponent>())
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
            var animationTree = entity.GetComponent<AnimationTreeComponent>()!.Node;
            var charAnimations = entity.GetComponent<CharAnimationsComponent>()!;
            var transform = trComponent.Transform3D;
            var move = entity.GetComponent<MoveComponent>();

            if (animationTree == null || body == null)
                continue;

            var isWalking = move != null;

            var curState = charAnimations.CurentState;
            var newState = isWalking ? "walk" : "idle";

            if (curState != newState)
            {
                charAnimations.CurentState = newState;
                animationTree.Set("parameters/movement/transition_request", newState);
            }
        }
    }
}
