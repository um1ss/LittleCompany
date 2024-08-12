using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingsView : MonoBehaviour
{
	[SerializeField] private List<Button> _buttons;
	[SerializeField] private List<GameObject> _panels;

	private SettingsPresenter _presenter;
	private int _index;

	private void Start()
	{
		_buttons.ForEach(button =>
		{
			button.GetComponent<ButtonClickName>().Click += FindButton;
		});
	}

	private void Update()
    {
		InputButton();
    }

	private void OffImageButtons(int index)
	{
		var buttons = _buttons[index].GetComponentsInChildren<Image>();

		foreach (var image in buttons)
		{
			image.enabled = !image.enabled;
		}

		
	}

	private void FindButton(string name)
	{
		var index = _index;
		for (int i = 0; i < _buttons.Count; i++)
		{
			if (_buttons[i].name == name)
			{
				_index = i;
				break;
			}
		}

		Insert(index);
	}

	private void InputButton()
	{
		int index = _index;
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			if (_index == _buttons.Count - 1)
			{
				_index = _buttons.Count - 1;
				return;
			}

			_index++;
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
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
			// ?? _buttons[_index].onClick.RemoveAllListeners();
		}

		Insert(index);
	}

	private void Insert(int index)
	{
		OffImageButtons(index);
		OffImageButtons(_index);

		_panels[index].SetActive(false);
		_panels[_index].SetActive(true);
	}
}
