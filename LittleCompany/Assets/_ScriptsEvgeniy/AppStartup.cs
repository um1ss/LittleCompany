using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using Extensions;
using UnityEngine.AddressableAssets;

public class AppStartup : MonoBehaviour
{
    private GameContext _gameContext;

    [Inject]
    private void Construct(GameContext gameContext)
    {
        _gameContext = gameContext;
    }
    private void Awake()
    {
        _gameContext.Initialize();
    }
    private async void Start()
    {
        var loadingOperations = new Queue<ILoadingOperation>();
        loadingOperations.Enqueue(new LoadPlayerOperation(_gameContext));
        loadingOperations.Enqueue(new MenuLoadingOperation(_gameContext, LoadSceneMode.Single));
        await _gameContext.LoadingScreenProvider.LoadAndDestroy(loadingOperations);
    }
}
public class LoadPlayerOperation : ILoadingOperation
{
    private GameContext _gameContext;
    public LoadPlayerOperation(GameContext gameContext)
    {
        _gameContext = gameContext;
    }
    public async UniTask Load()
    {
        var playerPrefab = await Addressables.LoadAssetAsync<GameObject>(AssetsConstants.PlayerLink);
        _gameContext.SetPlayerPrefab(playerPrefab.GetComponent<FirstPlayerController>());
    }
}
