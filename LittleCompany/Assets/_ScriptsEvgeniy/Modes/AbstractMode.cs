using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceProviders;

public abstract class AbstractMode : MonoBehaviour
{   
    protected SceneInstance _environment;
    protected GameContext _gameContext;
    public virtual void Init(SceneInstance sceneInstance)
    {
        _environment = sceneInstance;
    }
    public virtual async UniTask Cleanup()
    {
        await _gameContext.SceneAssetProvider.UnloadAddressablesScene(_environment);
    }
}
