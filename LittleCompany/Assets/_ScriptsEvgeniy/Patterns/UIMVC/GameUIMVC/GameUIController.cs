using UnityEngine;
using Zenject;

public class GameUIController
{
    private GameUIModel _model;
    private GameUIView _view;
    private GameContext _gameContext;

    [Inject]
    private void Construct(GameCanvas gameCanvas, GameContext gameContext)
    {
        _gameContext = gameContext;
        _view = new GameUIView(gameCanvas, gameContext);

        _model = new GameUIModel(_view);
    }
}
