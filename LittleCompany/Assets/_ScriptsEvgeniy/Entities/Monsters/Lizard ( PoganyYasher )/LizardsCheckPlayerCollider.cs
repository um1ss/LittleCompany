using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LizardsCheckPlayerCollider : MonoBehaviour
{
    public Action<Transform> OnPlayerEnter;
    public Action OnAllPlayerExit;

    [SerializeField] private bool _isActiveCollider = true;

    private List<Transform> _enterTargets = new List<Transform>();
    public bool IsEnterTargets {  get { return _isActiveCollider; } }
    public List<Transform> EnterTarget { get { return _enterTargets; } }

    private void OnTriggerEnter(Collider other)
    {
        var tag = other.tag;

        if (tag.Equals("Lizard"))
        {
            _isActiveCollider = true;
            return;
        }

        if (tag.Equals("Player"))
        {
            _enterTargets.Add(other.transform);
            if (!_isActiveCollider) return;
            OnPlayerEnter?.Invoke(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var tag = other.tag;

        if (tag.Equals("Lizard"))
        {
            _isActiveCollider = false;
            return;
        }
        if (tag.Equals("Player"))
        {
            _enterTargets.Remove(other.transform);
            if (_enterTargets.Count == 0)
            {
                OnAllPlayerExit?.Invoke();
            }
        }
    }
}
