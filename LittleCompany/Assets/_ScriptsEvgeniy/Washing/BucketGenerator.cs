using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketGenerator : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Bucket _bucketPrefab;

    private void OnMouseDown()
    {
        Instantiate(_bucketPrefab, _spawnPoint.position, Quaternion.identity);
    }
}
