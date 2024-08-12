using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Globalnstaller : MonoInstaller
{
    // if (System.deviceType == DeviceType.Handheld)
    // {
    //     Container.Bind<IHandler>().To<MobileInput>().FromNew().AsSingle();
    // }
    // else
    // {
    //     Container.Bind<IHandler>().To<DesktopInput>().FromNew().AsSingle().NonLazy();
    // }
}
