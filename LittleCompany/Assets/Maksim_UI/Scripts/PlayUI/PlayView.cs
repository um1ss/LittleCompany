using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayView : MonoBehaviour
{
	[SerializeField] private List<ButtonClickName> _buttons;

	private int _index;
	private PlayModel _playModel;

	[Inject]
	public void Construct(GameContext gameContext)
	{
		_playModel = new PlayModel(gameContext);

		gameObject.SetActive(false);
	}
	private void OnEnable()
	{
		_buttons.ForEach(button =>
		{
			button.Click += Play;

		});
		
	}

	private void OnDisable()
	{
		_buttons.ForEach(button =>
		{
			button.Click -= Play;

		});
	}
	private void Play(string buttonName)
	{
		_playModel.Play(buttonName);
	}

	private void Update()
	{
		InputButton();
		
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
			_buttons[_index].GetComponent<Button>().onClick?.Invoke();
			
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			foreach(var button in GetComponentsInChildren<Button>())
			{
				if(button.name == "EscButton")
					button.onClick?.Invoke();
			}
		}

		OffImageButtons(_index);

		
	}
}
