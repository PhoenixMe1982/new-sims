using Game.Configs;
using Game.Services;
using Game.Systems;
using Godot;
using LooksLike.DependencyInjection;
using LooksLike.Ecs;
using LooksLike.Framework.Services;
using LooksLike.Framework.Systems;
using LooksLike.Services;
using LooksLike.Utils;

public partial class Main : Node
{
    [Export] private Node3D? _entitiesRoot;
    [Export] private ScenesListConfig? _scenesListConf;
    [Export] private Node? _sceneParent;

    private EcsWorld? _world;
    private DIContainer _container = new DIContainer();
    private Logger _logger = Logger.GetLogger("Main");

    public override void _Ready()
    {
        if (_entitiesRoot == null || _scenesListConf == null || _sceneParent == null)
        {
            _logger.Error("Entities root or scenes list is null");
            return;
        }

        _world = EcsWorld.Instance!;
        _container.Register(() => _world);
        _container.Register(() => this);


        var serviceBase = new ServiceBase();
        serviceBase.Register<IAudioService>(new AudioService(_entitiesRoot));
        serviceBase.Register<ISceneService>(new SceneService(_scenesListConf, _sceneParent));


        _world.RegisterSystem(new AttachedToNodeSystem(_entitiesRoot));
        _world.RegisterSystem(new LifeTimeSystem());
        _world.RegisterSystem(new MoveSystem());
        _world.RegisterSystem(new CharacterBodyMoveSystem());
        _world.RegisterSystem(new CharAnimationSystem());
        _world.RegisterSystem(new CameraSystem());
        _world.RegisterSystem(new Cursor3DSystem());
        _world.RegisterSystem(new NavAgentSystem());
        _world.RegisterSystem(new PlayerPartsSystem());
        _world.RegisterSystem(new PlayerCustomizerSystem());
        _world.RegisterSystem(new InteractionSystem());
        _world.RegisterSystem(new StartInteractionSystem());


        _world.RegisterSystem(new TriggerDetectSystem());
        _world.RegisterSystem(new HitShapeDetectSystem());
        _world.RegisterSystem(new HitRayDetectSystem());


        ServiceBase.Initialize(_container);
        SystemBase.Initialize(_container);
        _world.Initialize();
    }

    public override void _Process(double delta)
    {
        var dt = (float)delta;

        _world?.Tick(dt);
        Service.Tick(dt);
    }

    public override void _PhysicsProcess(double delta)
    {
        _world?.PhysicsTick((float)delta);
    }

    public override void _ExitTree()
    {
        ServiceBase.UnInitialize();
        SystemBase.UnInitialize();
        _world?.UnInitialize();
    }
}
