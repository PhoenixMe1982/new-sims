using System.Collections.Generic;
using System.Linq;
using LooksLike.DependencyInjection;
using LooksLike.Ecs;
using Game.Components;
using Godot;
using LooksLike.Utils;
using LooksLike.Framework.Components;

namespace Game.Systems;

public class Cursor3DSystem : SystemBase, IEntitiesPhysicsUpdate
{
    [Inject] private EcsWorld _world = null!;

    // TODO: move to sercive
    public static Cursor3DComponent? GetCursorComponent(EcsWorld world)
    {
        var filter = new EcsFilter().With<Cursor3DComponent>();
        var entities = world.GetFilteredEntities(filter);
        var firstEntity = entities.FirstOrDefault();
        return firstEntity.Value.GetComponent<Cursor3DComponent>();
    }

    public Cursor3DSystem() : base(new EcsFilter().With<Cursor3DComponent>()) { }

    public void EntitiesPhysicsUpdate(Dictionary<ulong, EcsEntity> entities, float delta)
    {
        foreach (var (_, entity) in entities)
        {
            var cursor = entity.GetComponent<Cursor3DComponent>()!;
            var worldPosition = GetCursorWorldPosition();

            if (cursor.Node == null)
                continue;

            if (worldPosition != null && cursor.Node != null)
            {
                cursor.Position = (Vector3)worldPosition;
                cursor.Node.GlobalPosition = cursor.Position;
            }

            if (Input.IsActionJustPressed("Click"))
            {
                var filter = new EcsFilter().With<Transform3DComponent>().With<NavAgentComponent>();
                var _entities = _world.GetFilteredEntities(filter);
                var firstEntity = _entities.FirstOrDefault();
                var agent = firstEntity.Value.GetComponent<NavAgentComponent>()!;
                agent.TargetPosition = cursor.Position;
                firstEntity.Value.AddComponent(new MoveComponent());
            }
        }
    }

    private Vector3? GetCursorWorldPosition()
    {
        var camera = CameraSystem.GetCameraComponent(_world)?.Node;
        if (camera == null)
            return null;

        var mousePos = camera.GetViewport().GetMousePosition();
        var rayOrigin = camera.ProjectRayOrigin(mousePos);
        var rayDirection = camera.ProjectRayNormal(mousePos);

        var rayLength = 100.0f; // Adjust as needed
        var rayEnd = rayOrigin + rayDirection * rayLength;

        var spaceState = camera.GetWorld3D().DirectSpaceState;
        var query = new PhysicsRayQueryParameters3D
        {
            From = rayOrigin,
            To = rayEnd,
            CollideWithAreas = false
        };

        var result = spaceState.IntersectRay(query);
        if (result.Count > 0)
        {
            var hitPosition = (Vector3)result["position"];
            return hitPosition;
        }
        return null;
    }
}
