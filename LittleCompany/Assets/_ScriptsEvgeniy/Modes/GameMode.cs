using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceProviders;

public class GameMode : AbstractMode
{
    private LvlPrefab _lvlPrefab;
    private FirstPlayerController _player;

    [Inject]
    private void Construct(GameContext gameContext)
    {
        _gameContext = gameContext;
    }
    private void OnEnable()
    {
        _gameContext.EventBase.Subscribe<BackTomenuEvent>(GoToMainMenu);
    }
    private void OnDisable()
    {
        _gameContext.EventBase.Unsubscribe<BackTomenuEvent>();
    }
    public void Init(SceneInstance sceneInstance, LvlPrefab lvlPrefab)
    {
        base.Init(sceneInstance);
        _lvlPrefab = lvlPrefab;
        StartNewGame();
    }
    public void StartNewGame()
    {
        _gameContext.TryCreatePlayer(out _player, _lvlPrefab.PlayerSpawnPosition.position, Quaternion.identity);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    private void GoToMainMenu(BackTomenuEvent backTomenuEvent)
    {
        var loadingOperations = new Queue<ILoadingOperation>();
        loadingOperations.Enqueue(new MenuLoadingOperation(_gameContext, LoadSceneMode.Additive));
        loadingOperations.Enqueue(new ClearGameOperation(this));
        _gameContext.LoadingScreenProvider.LoadAndDestroy(loadingOperations).Forget();
    }
    public override UniTask Cleanup()
    {
        _gameContext.UnloadPlayer();
        return base.Cleanup();
    }
}
public class BackTomenuEvent { }
public sealed class ClearGameOperation : ILoadingOperation
{
    private readonly GameMode _gameMode;

    public ClearGameOperation(GameMode gameMode)
    {
        _gameMode = gameMode;
    }

    public async UniTask Load()
    {
        await _gameMode.Cleanup();

        var unloadOp = SceneManager.UnloadSceneAsync(Constants.GAME);
        while (unloadOp.isDone == false)
        {
            await UniTask.Yield();
        }
    }
}
