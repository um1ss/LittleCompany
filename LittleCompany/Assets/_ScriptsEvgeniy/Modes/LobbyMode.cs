using Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;
using Zenject;
using UnityEngine.ResourceManagement.ResourceProviders;

public class LobbyMode : AbstractMode
{
    [SerializeField] private AssetReferenceMaterial _assetMaterial;
    [SerializeField] private Transform _playerSpawnPosition;
    [SerializeField] private List<LvlTrigger> _triggers;

    private FirstPlayerController _player;

    [Inject]
    private void Construct(GameContext gameContext)
    {
        _gameContext = gameContext;
    }

    private void OnEnable()
    {
        foreach (var trigger in _triggers)
        {
            trigger.OnPlayerEnter += LoadGame;
        }
    }
    private void OnDisable()
    {
        foreach (var trigger in _triggers)
        {
            trigger.OnPlayerEnter -= LoadGame;
        }
    }
    private void Start()
    {
        //_assetMaterial.LoadAssetAsync<Material>().Completed += (assyncOerationHandlep) =>
        //{
        //    if (assyncOerationHandlep.Status == AsyncOperationStatus.Succeeded)
        //    {
        //        foreach (var render in _renderers)
        //        {
        //            render.material = assyncOerationHandlep.Result;
        //        }
        //    }
        //};
    }
    public override void Init(SceneInstance sceneInstance)
    {
        base.Init(sceneInstance);
        if (_gameContext.TryCreatePlayer(out _player, _playerSpawnPosition.position, Quaternion.identity))
        {
            DontDestroyOnLoad(_player);
        }
    }
    private void LoadGame(string lvlName)
    {
        try
        {
            var loadingOperations = new Queue<ILoadingOperation>();
            loadingOperations.Enqueue(new GameLoadingOperation(_gameContext, lvlName));
            loadingOperations.Enqueue(new CleanLobbiOperation(this));
            _gameContext.LoadingScreenProvider.LoadAndDestroy(loadingOperations).Forget();
        }
        catch (Exception e)
        {
            Debug.LogError($"{nameof(LobbyMode)} {nameof(LoadGame)} exception: {e.Message}");
        }
    }
}
public class GameLoadingOperation : ILoadingOperation
{
    private GameContext _gameContext;
    private string _lvlName;
    public GameLoadingOperation(GameContext gameContext, string lvlName)
    {
        _lvlName = lvlName;
        _gameContext = gameContext;
    }
    public async UniTask Load()
    {
        var loadOp = SceneManager.LoadSceneAsync(Constants.GAME, LoadSceneMode.Additive);
        while (loadOp.isDone == false)
        {
            await UniTask.Yield();
        }
        var scene = SceneManager.GetSceneByName(Constants.GAME);
        var environmentScene = await _gameContext.SceneAssetProvider.LoadSceneAddressables(_lvlName, LoadSceneMode.Additive);
        LvlPrefab lvlPrefab = environmentScene.Scene.GetRoot<LvlPrefab>();
        GameMode gameMode = scene.GetRoot<GameMode>();
        gameMode.Init(environmentScene, lvlPrefab);
    }
}
public class CleanLobbiOperation : ILoadingOperation
{
    private LobbyMode _lobbyMode;
    public CleanLobbiOperation(LobbyMode lobbyMode)
    {
        _lobbyMode = lobbyMode;
    }
    public async UniTask Load()
    {
        await _lobbyMode.Cleanup();
    }
}
