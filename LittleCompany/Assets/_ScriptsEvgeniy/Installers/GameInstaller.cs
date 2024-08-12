using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameCanvas _gameCanvasPrefab;
    public override void InstallBindings()
    {
        CanvasBinding();
        MVCBinding();

    }
    private void CanvasBinding()
    {
        Container
            .Bind<GameCanvas>()
            .FromComponentInNewPrefab(_gameCanvasPrefab)
            .AsSingle()
            .NonLazy();
    }
    private void MVCBinding()
    {
        Container
            .Bind<GameUIController>()
            .FromNew()
            .AsSingle()
            .NonLazy();
    }
}