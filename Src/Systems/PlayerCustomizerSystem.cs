using System.Collections.Generic;
using LooksLike.DependencyInjection;
using LooksLike.Ecs;
using LooksLike.Utils;
using LooksLike.Framework.Components;
using Game.Components;
using Godot;
using Game.Services;
using Game.Configs;

namespace Game.Systems;

public class PlayerCustomizerSystem : SystemBase, IEntitiesAdded, IEntitiesUpdate
{
    [Inject] private EcsWorld _world = null!;
    [Inject] private ISceneService _sceneService = null!;

    public PlayerCustomizerSystem() : base(new EcsFilter()
        .With<PlayerCustomizerComponent>())
    {
    }

    public void EntitiesAdded(Dictionary<ulong, EcsEntity> entities)
    {
        foreach (var (_, entity) in entities)
        {
            GD.Print("Player customizer added");
            var playerCustomizer = entity.GetComponent<PlayerCustomizerComponent>()!;

            playerCustomizer.BodyButton.Pressed += () => _OnBodyButtonPressed(playerCustomizer);
            playerCustomizer.HeadButton.Pressed += () => _OnHeadButtonPressed(playerCustomizer);
            playerCustomizer.HairButton.Pressed += () => _OnHairButtonPressed(playerCustomizer);
            playerCustomizer.ConfirmButton.Pressed += () => _OnConfirmButtonPressed();

            var playerParts = PlayerPartsSystem.GetPlayerParts(_world);
            var heads = playerParts?.Parts?.Heads;

            if (heads != null)
            {
                foreach (var part in heads)
                {
                }
            }
        }
    }

    public void EntitiesUpdate(Dictionary<ulong, EcsEntity> entities, float deltaTime)
    {
        foreach (var (_, entity) in entities)
        {
            var playerCustomizer = entity.GetComponent<PlayerCustomizerComponent>()!;
        }
    }

    private void _OnBodyButtonPressed(PlayerCustomizerComponent playerCustomizer)
    {
        RemoveVisibleChildren(playerCustomizer);
    }

    private void _OnHeadButtonPressed(PlayerCustomizerComponent playerCustomizer)
    {
        var playerParts = PlayerPartsSystem.GetPlayerParts(_world);
        var itemPref = playerCustomizer.ItemButton;
        var itemPrefParent = itemPref?.GetParent() as Control;
        var heads = playerParts?.Parts?.Heads;

        RemoveVisibleChildren(playerCustomizer);
        if (heads == null || itemPref == null || itemPrefParent == null)
            return;


        foreach (var part in heads)
        {
            var newItemButton = itemPref?.Duplicate() as Button;
            if (newItemButton == null)
                continue;

            newItemButton.Visible = true;
            itemPrefParent.AddChild(newItemButton);
            newItemButton.Pressed += () =>
            {
                playerParts.HeadType = part.Key;
                playerParts.NeedsUpdate = true;
            };
        }
    }

    private void _OnHairButtonPressed(PlayerCustomizerComponent playerCustomizer)
    {
        RemoveVisibleChildren(playerCustomizer);

        var playerParts = PlayerPartsSystem.GetPlayerParts(_world);
        var itemPref = playerCustomizer.ItemButton;
        var itemPrefParent = itemPref?.GetParent() as Control;
        var hairs = playerParts?.Parts?.Hairs;

        if (hairs == null || itemPref == null || itemPrefParent == null)
            return;

        foreach (var part in hairs)
        {
            var newItemButton = itemPref?.Duplicate() as Button;
            if (newItemButton == null)
                continue;

            newItemButton.Visible = true;
            itemPrefParent.AddChild(newItemButton);
            newItemButton.Pressed += () =>
            {
                playerParts.HairType = part.Key;
                playerParts.NeedsUpdate = true;
            };
        }
    }

    private void _OnConfirmButtonPressed()
    {
        _sceneService.ChangeScene(Location.Home);
    }

    private void RemoveVisibleChildren(PlayerCustomizerComponent playerCustomizer)
    {
        var itemPref = playerCustomizer.ItemButton;
        var itemPrefParent = itemPref?.GetParent() as Control;

        if (itemPref == null || itemPrefParent == null)
            return;
        foreach (var child in itemPrefParent.GetChildren())
        {
            var n = child as Control;
            if (n == null || !n.Visible)
                continue;
            n.QueueFree();
        }
    }
}
