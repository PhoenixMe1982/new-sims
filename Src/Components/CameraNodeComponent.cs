using LooksLike.Ecs;
using Godot;

namespace Game.Components;

[GlobalClass]
public partial class CameraNodeComponent : EcsComponent
{
    [Export] public Camera3D? Node;
    [Export] public EcsEntity? Target;
    [Export] public Vector3 Offset;
    [Export] public int SmoothSpeed = 5;
    [Export] public float ZoomFactor = 0.9f; // Множитель масштабирования (значение меньше 1 для приближения)
    [Export] public float MinZoomDistance = 2.0f; // Минимальное расстояние смещения
    [Export] public float MaxZoomDistance = 20.0f; // Максимальное расстояние смещения

    public override void _Ready()
    {
        if (Node == null)
            GD.PrintErr("Camera Node is null");
    }
}
