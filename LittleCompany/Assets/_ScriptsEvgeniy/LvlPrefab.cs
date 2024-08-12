using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlPrefab : MonoBehaviour
{
    [SerializeField] Transform _playerSpawnPosition;

    public Transform PlayerSpawnPosition {  get { return _playerSpawnPosition; } }
}
