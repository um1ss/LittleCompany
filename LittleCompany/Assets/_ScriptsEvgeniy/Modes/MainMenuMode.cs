using Cysharp.Threading.Tasks;
using System;
using Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using System.Collections.Generic;

public class MainMenuMode : AbstractMode
{
    [Inject]
    private void Construct(GameContext gameContext)
    {
        _gameContext = gameContext;
    }
    private void OnEnable()
    {
        _gameContext.EventBase.Subscribe<LoadLobbyEvent>(LoadLobby);
    }
    private void OnDisable()
    {
        _gameContext.EventBase.Unsubscribe<LoadLobbyEvent>();
    }
    private void LoadLobby(LoadLobbyEvent loadGameEvent)
    {
        try
        {
            var loadingOperations = new Queue<ILoadingOperation>();
            loadingOperations.Enqueue(new LobbyLoadingOperation(_gameContext));
            loadingOperations.Enqueue(new CleanMenuOperation(this));
            _gameContext.LoadingScreenProvider.LoadAndDestroy(loadingOperations).Forget();
        }
        catch (Exception e)
        {
            Debug.LogError($"{nameof(MainMenuMode)} {nameof(LoadLobby)} exception: {e.Message}");
        }
    }
}
public class LobbyLoadingOperation : ILoadingOperation
{
    private GameContext _gameContext;
    public LobbyLoadingOperation(GameContext gameContext)
    {
        _gameContext = gameContext;
    }
    public async UniTask Load()
    {
        var environment = await _gameContext.SceneAssetProvider.LoadSceneAddressables(AssetsConstants.LobbyScene, LoadSceneMode.Additive);
        LobbyMode lobbyMode = environment.Scene.GetRoot<LobbyMode>();
        lobbyMode.Init(environment);
    }
}
public class CleanMenuOperation : ILoadingOperation
{
    private MainMenuMode _menuMode;
    public CleanMenuOperation(MainMenuMode menuMode)
    {
        _menuMode = menuMode;
    }
    public async UniTask Load()
    {
        await _menuMode.Cleanup();
    }
}
public class LoadLobbyEvent { }
