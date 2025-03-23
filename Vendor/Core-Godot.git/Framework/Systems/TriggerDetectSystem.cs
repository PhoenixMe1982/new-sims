using System.Collections.Generic;
using LooksLike.DependencyInjection;
using LooksLike.Ecs;
using LooksLike.Utils;
using LooksLike.Framework.Components;

namespace LooksLike.Framework.Systems;

public class TriggerDetectSystem : SystemBase, IEntitiesPhysicsUpdate
{
    [Inject] private EcsWorld? _world;

    public TriggerDetectSystem() : base(new EcsFilter()
        .With<TriggerDetectComponent>())
    {
    }

    public void EntitiesPhysicsUpdate(Dictionary<ulong, EcsEntity> entities, float deltaTime)
    {
        foreach (var (_, entity) in entities)
        {
            var collisionDetect = entity.GetComponent<TriggerDetectComponent>()!;

            if (collisionDetect.DetectedBodyExit != null)
            {
                var otherBody = collisionDetect.DetectedBodyExit;
                if (otherBody?.Owner is EcsEntity otherEntity)
                    collisionDetect.DetectedEntities.Remove(otherEntity);

                collisionDetect.DetectedBodyExit = null;
            }

            if (collisionDetect.Timeout > 0)
            {
                collisionDetect.Timeout -= deltaTime;
                continue;
            }

            if (collisionDetect.DetectedBodyEnter != null)
            {
                var otherBody = collisionDetect.DetectedBodyEnter;
                if (otherBody?.Owner is EcsEntity otherEntity)
                    collisionDetect.DetectedEntities.Add(otherEntity);

                collisionDetect.DetectedBodyEnter = null;
            }
        }
    }
}
