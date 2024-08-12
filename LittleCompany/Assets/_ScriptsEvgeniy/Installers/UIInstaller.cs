using UnityEngine;
using Zenject;

public class UIInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
		//PlayModelBinding();
    }

	private void PlayModelBinding()
	{
		Container
			.Bind<PlayModel>()
			.FromNew()
			.AsSingle()
			.NonLazy();
	}
}