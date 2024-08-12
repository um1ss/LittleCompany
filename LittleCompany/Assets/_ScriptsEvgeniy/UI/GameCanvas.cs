using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] private Button _backToMenuButton;

    public Button MenuButton { get { return _backToMenuButton; } }
}
