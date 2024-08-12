using System;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText = default;

    private void OnEnable()
    {
        FirstPlayerController.OnDamage += UpdateHealth;
        FirstPlayerController.OnHeal += UpdateHealth;
    }

    private void OnDisable()
    {
        FirstPlayerController.OnDamage -= UpdateHealth;
        FirstPlayerController.OnHeal -= UpdateHealth;
    }

    private void Start()
    {
        UpdateHealth(100);
    }

    private void UpdateHealth(float currentHealth)
    {
        _healthText.text = currentHealth.ToString("00");
    }
}
