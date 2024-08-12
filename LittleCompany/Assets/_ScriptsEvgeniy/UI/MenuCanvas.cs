using UnityEngine;
using UnityEngine.UI;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;

    public Button StartButton { get { return _startButton; } }
    public Button ExitButton { get { return _exitButton; } }
}
