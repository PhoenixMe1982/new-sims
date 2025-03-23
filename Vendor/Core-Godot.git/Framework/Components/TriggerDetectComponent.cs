using System.Collections.Generic;
using LooksLike.Ecs;
using Godot;

namespace LooksLike.Framework.Components;

[GlobalClass]
public partial class TriggerDetectComponent : EcsComponent
{
    [Export] public Area3D? Node;
    [Export] public float Timeout = 0;

    public HashSet<EcsEntity> DetectedEntities { get; set; } = new();
    public Node3D? DetectedBodyEnter { get; set; }
    public Node3D? DetectedBodyExit { get; set; }
    public List<ulong> ExcludedEntities { get; set; } = new();

    public override void _Ready()
    {
        if (Node == null)
        {
            GD.PrintErr("Collision Detect Node is null");
            return;
        }

        Node.BodyEntered += OnBodyEntered;
        Node.BodyExited += OnBodyExited;
        Node.AreaEntered += OnAreaEntered;
        Node.AreaExited += OnAreaExited;
    }

    public void OnBodyEntered(Node3D body)
    {
        OnNodeEntered(body);
    }
    public void OnBodyExited(Node3D body)
    {
        OnNodeExited(body);
    }

    public void OnAreaEntered(Area3D area)
    {
        OnNodeEntered(area);
    }
    public void OnAreaExited(Node3D area)
    {
        OnNodeExited(area);
    }

    private void OnNodeEntered(Node3D node)
    {
        if (Timeout > 0)
            return;

        if (node?.Owner is EcsEntity otherEntity)
        {
            if (otherEntity.Id == Owner.GetInstanceId())
                return;

            if (ExcludedEntities.Contains(otherEntity.Id))
                return;
        }

        DetectedBodyEnter = node;
    }

    private void OnNodeExited(Node3D node)
    {
        DetectedBodyExit = node;
        if (node == DetectedBodyEnter)
            DetectedBodyEnter = null;
    }
}
