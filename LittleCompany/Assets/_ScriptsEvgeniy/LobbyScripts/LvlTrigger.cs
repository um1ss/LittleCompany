using System;
using UnityEngine;

public class LvlTrigger : MonoBehaviour
{
    public Action<string> OnPlayerEnter;

    [SerializeField] private string _lvlName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnPlayerEnter?.Invoke(_lvlName);
        }
    }
}

