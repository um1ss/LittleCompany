using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

public class PlayModel
{
    private GameContext _gameContext;

    public PlayModel(GameContext context)
    {
        _gameContext = context;
        _gameContext.Initialize();
    }

    public void Play(string name)
    {
        switch (name)
        {
            case "Single_game" :
                _gameContext.EventBase.Invoke(new LoadLobbyEvent());
                break;
        }
    }
}
