using Cysharp.Threading.Tasks;
using Extensions;
using UnityEngine.SceneManagement;

public class MenuLoadingOperation : ILoadingOperation
{
    private GameContext _gameContext;
    private LoadSceneMode _sceneMode;
    public MenuLoadingOperation(GameContext gameContext, LoadSceneMode sceneMode)
    {
        _gameContext = gameContext;
        _sceneMode = sceneMode;
    }
    public async UniTask Load()
    {
        var environment = await _gameContext.SceneAssetProvider.LoadSceneAddressables(AssetsConstants.MenuScene, _sceneMode);
        MainMenuMode menuMode = environment.Scene.GetRoot<MainMenuMode>();
        menuMode.Init(environment);
    }
}
