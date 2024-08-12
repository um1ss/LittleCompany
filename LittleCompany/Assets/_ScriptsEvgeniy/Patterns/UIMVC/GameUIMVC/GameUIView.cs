using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIView
{
    private GameCanvas _gameCanvas;
    private GameContext _gameContext;
    public GameUIView(GameCanvas gameCanvas, GameContext gameContext)
    {
        _gameCanvas = gameCanvas;
        _gameContext = gameContext;

        _gameCanvas.MenuButton.onClick.AddListener(() => _gameContext.EventBase.Invoke(new BackTomenuEvent()));
    }
}
