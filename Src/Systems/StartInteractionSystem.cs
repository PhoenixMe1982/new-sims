using System.Collections.Generic;
using LooksLike.DependencyInjection;
using LooksLike.Ecs;
using LooksLike.Utils;
using Game.Components;
using Godot;
using Game.Services;
using Game.Configs;

namespace Game.Systems;

public class StartInteractionSystem : SystemBase, IEntitiesAdded, IEntitiesUpdate
{
    [Inject] private EcsWorld _world = null!;
    [Inject] private ISceneService _sceneService = null!;

    public enum InteractionType
    {
        None,
        Customize,
    }

    public StartInteractionSystem() : base(new EcsFilter()
        // .With<InteractionComponent>()
        .With<StartInteractionComponent>())
    {
    }

    public void EntitiesAdded(Dictionary<ulong, EcsEntity> entities)
    {
        foreach (var (_, entity) in entities)
        {
            var startInteraction = entity.GetComponent<StartInteractionComponent>()!;
            startInteraction.Enabled = false;
        }
    }

    public void EntitiesUpdate(Dictionary<ulong, EcsEntity> entities, float deltaTime)
    {
        foreach (var (_, entity) in entities)
        {
            var startInteraction = entity.GetComponent<StartInteractionComponent>()!;

            var label = startInteraction.Node;
            if (label == null)
                continue;

            if (label.Visible != startInteraction.Enabled)
                label.Visible = startInteraction.Enabled;

            if (!startInteraction.Enabled)
                continue;

            if (Input.IsActionJustPressed("Interact"))
            {
                startInteraction.Enabled = false;

                switch (startInteraction.Type)
                {
                    case InteractionType.Customize:
                        _sceneService.ChangeScene(Location.Customizer);
                        break;
                }
            }
        }
    }
}
