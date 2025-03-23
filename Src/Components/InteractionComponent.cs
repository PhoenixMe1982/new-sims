using System.Collections.Generic;
using Godot;
using LooksLike.Ecs;

namespace Game.Components;

[GlobalClass]
public partial class InteractionComponent : EcsComponent
{
    public HashSet<EcsEntity> NewInteractionEntities = new();
    public Dictionary<ulong, EcsEntity> AllInteractionEntities = new();
}
