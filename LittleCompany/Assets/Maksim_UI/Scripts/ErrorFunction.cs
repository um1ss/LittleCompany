using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorFunction : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _massage;

    private Vector3 _startPosition = new(960, 200, 0);
    private Color _colorA = new Color(1, 1, 1, 1);
    private Color _colorB = new Color(1, 1, 1, 0);
    public void ErrorMessage()
    {
        _massage.gameObject.SetActive(true);
        _massage.color = _colorA;
        _massage.transform.position = _startPosition;

		_massage.DOColor(_colorB, 3.5f);
        _massage.transform.DOMoveY(560, 3).OnComplete(() =>
        {
            _massage.gameObject.SetActive(false);
            
        }); 
		
	}
}
