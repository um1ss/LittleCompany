using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class LoadingScreenProvider : LocalAssetLoader
{
    public async UniTask LoadAndDestroy(ILoadingOperation loadingOperation)
    {
        var operations = new Queue<ILoadingOperation>();
        operations.Enqueue(loadingOperation);
        await LoadAndDestroy(operations);
    }

    public async UniTask LoadAndDestroy(Queue<ILoadingOperation> loadingOperations)
    {
        var loadingScreen = await Load<LoadScreen>(AssetsConstants.LoadingScreen);
        await loadingScreen.Load(loadingOperations);
        Unload();
    }
}
