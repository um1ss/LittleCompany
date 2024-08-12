using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("EntityFields")]
    [SerializeField] protected float _moveSpeed = 1;
    [SerializeField] protected float _rotationSpeed;

    public float MoveSpeed { get { return _moveSpeed; } }
    public float RotationSpeed { get { return _rotationSpeed; } }
}
