using System.Collections.Generic;
using LooksLike.DependencyInjection;
using LooksLike.Ecs;
using LooksLike.Utils;
using LooksLike.Framework.Components;
using Game.Components;

namespace Game.Systems;

public class InteractionSystem : SystemBase, IEntitiesUpdate
{
    [Inject] private EcsWorld _world = null!;

    public InteractionSystem() : base(new EcsFilter()
        .With<InteractionComponent>()
        .With<TriggerDetectComponent>())
    {
    }

    public void EntitiesUpdate(Dictionary<ulong, EcsEntity> entities, float deltaTime)
    {
        foreach (var (_, entity) in entities)
        {
            var interaction = entity.GetComponent<InteractionComponent>()!;
            var trigger = entity.GetComponent<TriggerDetectComponent>()!;

            var detected = trigger.DetectedEntities;
            var allInteracted = interaction.AllInteractionEntities;

            interaction.NewInteractionEntities.Clear();

            foreach (var detectedEntity in detected)
            {
                if (allInteracted.ContainsKey(detectedEntity.Id))
                    continue;

                allInteracted.Add(detectedEntity.Id, detectedEntity);
                interaction.NewInteractionEntities.Add(detectedEntity);

                var startInteraction = detectedEntity.GetComponent<StartInteractionComponent>();
                if (startInteraction != null)
                    startInteraction.Enabled = true;
            }

            foreach (var interactedEntity in allInteracted)
            {
                if (!detected.Contains(interactedEntity.Value))
                {
                    var startInteraction = interactedEntity.Value.GetComponent<StartInteractionComponent>();
                    if (startInteraction != null)
                        startInteraction.Enabled = false;

                    allInteracted.Remove(interactedEntity.Key);
                }
            }
        }
    }
}
