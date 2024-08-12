using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameContext : MonoBehaviour
{
    public LoadingScreenProvider LoadingScreenProvider { get; private set; }
    public SceneAssetProvider SceneAssetProvider { get; private set; }
    public EventBase EventBase { get; private set; }

    private FirstPlayerController _player;

    private FirstPlayerController _playerPrefab;

    public void Initialize()
    {
        if (EventBase != null)
            return;
        LoadingScreenProvider = new LoadingScreenProvider();
        SceneAssetProvider = new SceneAssetProvider();
        EventBase = new EventBase();
    }
    public void SetPlayerPrefab(FirstPlayerController player)
    {
        _playerPrefab = player;
    }
    public bool TryCreatePlayer(out FirstPlayerController controller, Vector3 playerPos, Quaternion playerRotation)
    {
        if (_playerPrefab != null)
        {
            if (_player != null)
            {
                _player.transform.position = playerPos;
                _player.transform.rotation = playerRotation;
                controller = _player;
                return true;
            }
            _player = Instantiate(_playerPrefab, playerPos, playerRotation);
            controller = _player;
            return true;
        }
        controller = null;
        return false;
    }
    public void UnloadPlayer()
    {
        Destroy(_player.gameObject);
        //Addressables.ReleaseInstance(_playerPrefab.gameObject);
    }
}
