using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneAssetProvider 
{
    public SceneAssetProvider()
    {
        Addressables.InitializeAsync();
    }
    public async UniTask<SceneInstance> LoadSceneAddressables(string sceneId, LoadSceneMode sceneMode)
    {
        var op = Addressables.LoadSceneAsync(sceneId, sceneMode);
        return await op.Task;
    }

    public async UniTask UnloadAddressablesScene(SceneInstance scene)
    {
        var op = Addressables.UnloadSceneAsync(scene);
        await op.Task;
    }
}
