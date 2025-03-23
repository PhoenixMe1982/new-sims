using System.Collections.Generic;
using System.Linq;
using LooksLike.Ecs;
using Game.Components;
using LooksLike.Utils;
using LooksLike.Framework.Components;
using Godot;
using LooksLike.DependencyInjection;

namespace Game.Systems;

public class CameraSystem : SystemBase, IEntitiesAdded, IEntitiesUpdate
{
    [Inject] private EcsWorld _world = null!;
    private Logger _logger = Logger.GetLogger("CameraSystem", "#909Aff");
    private bool isTouching = false;

    public static CameraNodeComponent? GetCameraComponent(EcsWorld world)
    {
        var cameraFilter = new EcsFilter().With<CameraNodeComponent>();
        var cameraEnitites = world.GetFilteredEntities(cameraFilter);
        var firstCamera = cameraEnitites.FirstOrDefault();
        return firstCamera.Value.GetComponent<CameraNodeComponent>();
    }

    public CameraSystem() : base(new EcsFilter().With<Transform3DComponent>().With<CameraNodeComponent>()) { }

    public void EntitiesAdded(Dictionary<ulong, EcsEntity> entities)
    {
        foreach (var (_, entity) in entities)
        {
            var cameraComponent = entity.GetComponent<CameraNodeComponent>()!;
            var transform = entity.GetComponent<Transform3DComponent>()!.LocalTransform3D;
            cameraComponent.Offset = transform.Origin;
        }
    }

    public void EntitiesUpdate(Dictionary<ulong, EcsEntity> entities, float deltaTime)
    {
        foreach (var (_, entity) in entities)
        {
            var cameraComponent = entity.GetComponent<CameraNodeComponent>()!;
            var transform = entity.GetComponent<Transform3DComponent>()!.Transform3D;

            var camera = cameraComponent.Node;
            var target = cameraComponent.Target;

            if (target == null || camera == null)
                continue;

            var targetTransformComponent = target.GetComponent<Transform3DComponent>();
            if (targetTransformComponent == null)
                continue;

            var targetPosition = targetTransformComponent.Transform3D.Origin;
            if (isTouching)
            {
                var mouseDelta = Input.GetLastMouseVelocity();
                if (mouseDelta.LengthSquared() > 0.0f)
                {
                    var sensitivity = 0.005f;
                    var movement = new Vector3(-mouseDelta.X, 0, -mouseDelta.Y) * cameraComponent.Offset.Y * sensitivity * deltaTime;
                    targetTransformComponent.Transform3D = targetTransformComponent.Transform3D.Translated(movement);
                }
            }

            // interpolation
            camera.GlobalPosition = camera.GlobalPosition.Lerp(targetPosition + cameraComponent.Offset, cameraComponent.SmoothSpeed * deltaTime);


            if (Input.IsActionJustPressed("ZoomIn"))
            {
                cameraComponent.Offset = cameraComponent.Offset * cameraComponent.ZoomFactor;
                if (cameraComponent.Offset.Length() < cameraComponent.MinZoomDistance)
                    cameraComponent.Offset = cameraComponent.Offset.Normalized() * cameraComponent.MinZoomDistance;
            }
            else if (Input.IsActionJustPressed("ZoomOut"))
            {
                cameraComponent.Offset = cameraComponent.Offset / cameraComponent.ZoomFactor;
                if (cameraComponent.Offset.Length() > cameraComponent.MaxZoomDistance)
                    cameraComponent.Offset = cameraComponent.Offset.Normalized() * cameraComponent.MaxZoomDistance;
            }
            else if (Input.IsActionJustPressed("Centered"))
            {
                var filter = new EcsFilter().With<Transform3DComponent>().With<NavAgentComponent>();
                var _entities = _world.GetFilteredEntities(filter);
                var firstEntity = _entities.FirstOrDefault().Value;
                var position = firstEntity.GetComponent<Transform3DComponent>()!.Transform3D.Origin;

                targetTransformComponent.Transform3D = new Transform3D(
                    targetTransformComponent.Transform3D.Basis,
                    new Vector3(position.X, targetTransformComponent.Transform3D.Origin.Y, position.Z)
                );
            }
            else if (Input.IsActionJustPressed("Touch"))
                isTouching = true;
            else if (Input.IsActionJustReleased("Touch"))
                isTouching = false;
        }
    }
}
