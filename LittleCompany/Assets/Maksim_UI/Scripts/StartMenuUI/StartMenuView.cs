using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuView : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons;

    private int _index;


	private void Update()
	{
		InputButton();
	}
	public void OpenPanel(GameObject panel)
	{
		panel.SetActive(true);
	}

	public void ClosePanel(GameObject panel)
	{
		panel.SetActive(false);
	}

	private void OffImageButtons(int index)
    {
        foreach (var button in _buttons)
        {
            button.GetComponentInChildren<Image>().enabled = false;
        }

        _buttons[index].GetComponentInChildren<Image>().enabled = true;
    }

    private void InputButton()
    {
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			if (_index == _buttons.Count - 1)
			{
				_index = _buttons.Count - 1;
				return;
			}

			_index++;
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (_index == 0)
			{
				_index = 0;
				return;
			}

			_index--;
		}

		if (Input.GetKeyDown(KeyCode.Return))
		{
			_buttons[_index].onClick?.Invoke();
		}

		OffImageButtons(_index);
	}
}
