using LooksLike.Utils;
using Game.Configs;
using Godot;

namespace Game.Services;

public interface ISceneService
{
    Location CurrentScene { get; }
    void ChangeScene(Location location);
}

public class SceneService : ServiceBase, ISceneService
{
    public Location CurrentScene => _currentLocation;

    private ScenesListConfig _sceneListConf = null!;
    private Node _sceneParent = null!;
    private Logger _logger = Logger.GetLogger("LocationService");
    private Location _currentLocation = Location.None;

    public SceneService(ScenesListConfig locationListConf, Node sceneParent)
    {
        _sceneListConf = locationListConf;
        _sceneParent = sceneParent;
    }

    public void ChangeScene(Location location)
    {
        var path = _sceneListConf.ScenePath[location];
        var scene = GD.Load<PackedScene>(path);
        if (scene == null)
        {
            _logger.Error($"Unknown location: {location}");
            return;
        }

        // if (_currentLocation == location)
        // {
        //     _logger.Info($"Already at location: {location}");
        //     return;
        // }

        _currentLocation = location;
        _sceneParent.RemoveChild(_sceneParent.GetChild(0));
        _sceneParent.AddChild(scene.Instantiate());
    }
}
