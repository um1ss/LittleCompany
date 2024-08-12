using UnityEngine;
using Zenject;

public class MenuInstaller : MonoInstaller
{
    [SerializeField] private MenuCanvas _menuCanvasPrefab;
    public override void InstallBindings()
    {
        CanvasBinding();
        MVCBinding();

    }
    private void CanvasBinding()
    {
        Container
            .Bind<MenuCanvas>()
            .FromComponentInNewPrefab(_menuCanvasPrefab)
            .AsSingle()
            .NonLazy();
    }
    private void MVCBinding()
    {
        Container
            .Bind<MenuUIController>()
            .FromNew()
            .AsSingle()
            .NonLazy();
    }
}