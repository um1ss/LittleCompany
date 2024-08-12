using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Mop : PickUpItem
{
    public Action<bool> OnColliderSwitch;

    [SerializeField] private int _maxHealth = 4;
    [SerializeField] private Transform _washPosition;
    [SerializeField] private float _washRadius = 3;
    [SerializeField] private float _moveTime = 0.5f;
    [SerializeField] private DecalProjector _projector;
    [SerializeField] private Material _mopSpotMat;
    [SerializeField] private Spot _spotPrefab;

    private Color _dirtColor = Color.black;
    private LayerMask _spotMask;
    private int _health;

    private Tween _moveTween;
    private Tween _rotateTween;
    private Sequence _moveSequence;

    private bool _isAnim;

    public Color DirtColor
    {
        get { return _dirtColor; }
    }
    public int Health
    {
        get { return _health; }
        private set
        {
            _health = value;
            if (_health < 0)
            {
                _health = 0;
            }
            _projector.fadeFactor = (_maxHealth - _health) / (float)_maxHealth;
        }
    }

    private void Start()
    {
        _isAnim = false;
        _spotMask = LayerMask.GetMask("Spot");
        _health = _maxHealth;
    }
    private void TakeDirt(Color color)
    {
        Health--;
        _dirtColor += color;
        _dirtColor /= 2;
        _mopSpotMat.color = _dirtColor;
    }
    public override void OnPickUp(Transform container)
    {
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
        _collider.enabled = false;

        transform.SetParent(container);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        _player = GetComponentInParent<FirstPlayerController>();
    }
    public override void OnDrop(Transform container)
    {
        base.OnDrop(container);
        _collider.enabled = true;
        _rigidbody.useGravity = true;
    }
    private void WashSpot()
    {
        Collider[] hitColliders = Physics.OverlapSphere(_washPosition.position, _washRadius, _spotMask);
        foreach (var collider in hitColliders)
        {
            if (collider.gameObject.TryGetComponent(out Spot spot))
            {
                if (_health > 0)
                {
                    TakeDirt(spot.Clean());
                }
            }
        }
        _rigidbody.isKinematic = true;
        _rotateTween.Kill();
        _rotateTween = transform.DOLocalRotate(Vector3.zero, _moveTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bucket bucket))
        {
            CleanMop(bucket);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (Health <= 0 && collision.gameObject.CompareTag("Wall"))
        {
            var point = collision.GetContact(0);
            var spot = Instantiate(_spotPrefab, point.point, Quaternion.LookRotation(point.normal));
            spot.SetColor(DirtColor);
        }
    }
    private void CleanMop(Bucket bucket)
    {
        Color bucketColor = bucket.TakeDirt(_maxHealth - Health, _dirtColor);

        if (bucketColor != Color.black)
        {
            _dirtColor = bucketColor;
            _mopSpotMat.color = _dirtColor;
            Health = 0;
        }
        else
        {
            Health = _maxHealth;
            _dirtColor = Color.black;
            _mopSpotMat.color = _dirtColor;
            _projector.fadeFactor = 0;
        }
    }
    public override void OnUse()
    {
        if (_isAnim) return;
        EnableMop();
        _moveTween.Kill();
        _moveSequence.Kill();
        _moveTween = transform.DOLocalMoveY(0.7f, _moveTime).OnComplete(WashSpot);
        _moveSequence = DOTween.Sequence();
        _moveSequence.Append(_moveTween);
        _moveSequence.Append(transform.DOLocalMove(Vector3.zero, _moveTime)).OnComplete(DisableMop);
    }
    private void EnableMop()
    {
        _isAnim = true;
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
    }
    private void DisableMop()
    {
        _isAnim = false;
        _collider.enabled = false;
    }
}
