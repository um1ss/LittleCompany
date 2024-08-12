using UnityEngine;

public class MenuUIView
{
    private MenuCanvas _menuCanvas;
    private GameContext _gameContext;
    public MenuUIView(MenuCanvas menuCanvas, GameContext gameContext)
    {
        _menuCanvas = menuCanvas;
        _gameContext = gameContext;

        _menuCanvas.StartButton.onClick.AddListener(OnGameplayButtonClicked);
        _menuCanvas.ExitButton.onClick.AddListener(CloseApp);
    }
    private void OnGameplayButtonClicked()
    {
        _gameContext.EventBase.Invoke(new LoadLobbyEvent());
    }
    private void CloseApp()
    {
        Application.Quit();
    }
}
