using UnityEngine;
using UnityEngine.Localization.Settings;
using Zenject;

public class MenuUIController
{
    private MenuUIModel _model;
    private MenuUIView _view;
    private GameContext _gameContext;

    [Inject]
    private void Construct(MenuCanvas menuCanvas, GameContext gameContext)
    {
        _gameContext = gameContext;
        _view = new MenuUIView(menuCanvas, gameContext);

        _model = new MenuUIModel(_view);
    }
}

