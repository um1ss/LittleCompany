using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private Color _spotColor = Color.white;

    private int _health = 3;
    private SpriteRenderer _spotRend;

    private void Awake()
    {
        _spotRend = GetComponentInChildren<SpriteRenderer>();
        _health = _maxHealth;
    }
    private void Start()
    {
        SetColor(_spotColor);
    }
    public void SetColor(Color color)
    {
        _spotRend.color = color;
    }
    public Color Clean()
    {
        _health--;
        float a = (float)_health / _maxHealth;
        Color color = _spotRend.color;
        color.a = a;
        _spotRend.color = color;
        if (_health <= 0)
        {
            Destroy(gameObject, 0.1f);
            return _spotColor;
        }
        return _spotColor;
    }
}
