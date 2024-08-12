using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private GameContext _gameContext;
    public override void InstallBindings()
    {
        ContextBinding();
    }
    private void ContextBinding()
    {
        Container
            .Bind<GameContext>()
            .FromComponentInNewPrefab(_gameContext)
            .AsSingle()
            .NonLazy();
    }
}