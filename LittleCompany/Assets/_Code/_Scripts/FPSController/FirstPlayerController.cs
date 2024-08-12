using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPlayerController : MonoBehaviour
{
    public bool CanMove { get; private set; } = true;
    public bool IsSprinting => Input.GetKey(_sprintKey) && _canSprint;
    public bool ShouldJump => Input.GetKeyDown(_jumpKey) && _characterController.isGrounded;

    public bool ShouldCrouch =>
        Input.GetKeyDown(_crouchKey) && !_duringCrouchAnimation && _characterController.isGrounded;

    public Action OnJump;
    public Action OnLanding;

    [Header("Function Options")] [SerializeField]
    private bool _canSprint = true;
    [SerializeField] private bool _canJump = true;
    [SerializeField] private bool _canCrouch = true;
    [SerializeField] private bool _canUseHeadbob = true;
    [SerializeField] private bool _canWillSlidenOnSlopes = true;

    [Header("Controls")] [SerializeField] private KeyCode _sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _crouchKey = KeyCode.LeftControl;

    [Header("Movement Parameters")] [SerializeField]
    public /* Тестовый вариант влияния на персонажа */ float _walkSpeed = 3.0f;

    [SerializeField] private float _sprintSpeed = 6.0f;
    [SerializeField] private float _crouchSpeed = 1.5f;
    [SerializeField] private float _slopeSpeed = 8.0f;

    [Header("Look Parameters")] [SerializeField, Range(1, 10)]
    private float _lookSpeedX = 2.0f;

    [SerializeField, Range(1, 10)] private float _lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float _upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float _lowerLookLimit = 80.0f;

    [Header("Jumping Parameters")] [SerializeField]
    private float _jumpForce = 8.0f;
    [SerializeField] private float _gravity = 30.0f;

    [Header("Crouching Parameters")] [SerializeField]
    private float _crouchHeight = 0.5f;

    [SerializeField] private float _standingHeight = 2f;
    [SerializeField] private float _timeToCrouch = 0.25f;
    [SerializeField] private Vector3 _crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 _standingCenter = new Vector3(0, 0, 0);
    private bool _isCrouching;
    private bool _duringCrouchAnimation;

    [Header("Headbob Parameters")] [SerializeField]
    private float _walkBobSpeed = 14f;

    [SerializeField] private float _walkBobAmount = 0.05f;
    [SerializeField] private float _sprintBobSpeed = 18f;
    [SerializeField] private float _sprintBobAmount = 0.11f;
    [SerializeField] private float _crouchBobSpeed = 8f;
    [SerializeField] private float _crouchBobAmount = 0.025f;
    private float _defaultYPos = 0;
    private float _timer;

    [Header("Footsteps Parameters")] [SerializeField]
    private float _baseStepSpeed = 0.5f;

    [SerializeField] private float _crouchStepMultipler = 1.5f;

    [SerializeField] private float _sprintStepMultipler = 0.6f;
    public float _footstepTimer = 0; //теcт FMODPlugin

    [Header("Health Parameters")] [SerializeField]
    private float _maxHealth = 100;

    [SerializeField] private float _timeBeforeRegenStarts = 3;
    [SerializeField] private float _healthValueIncrement = 1;
    [SerializeField] private float _healthTimeIncrement = 0.1f;
    private float _currentHealth;
    private Coroutine _regeneratingHealth;
    public static Action<float> OnTakeDamage;
    public static Action<float> OnDamage;
    public static Action<float> OnHeal;

    public float GetCurrentOffset => _isCrouching ? _baseStepSpeed * _crouchStepMultipler :
        IsSprinting ? _baseStepSpeed * _sprintStepMultipler : _baseStepSpeed;  //теcт FMODPlugin

    #region Sliding Parameters

    private Vector3 _hitPointNormal;

    private bool IsSliding
    {
        get
        {
            if (_characterController.isGrounded &&
                Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
            {
                _hitPointNormal = slopeHit.normal;
                return Vector3.Angle(_hitPointNormal, Vector3.up) > _characterController.slopeLimit;
            }
            else
            {
                return false;
            }
        }
    }

    #endregion

    public Camera _playerCamera; //теcт FMODPlugin
    private CameraPlane _cameraPlane;
    public CharacterController _characterController; //теcт FMODPlugin

    private Vector3 _moveDirection;
    public Vector2 _currentInput; //теcт FMODPlugin
    private float _rotationX = 0f;
    
    private void OnEnable()
    {
        OnTakeDamage += ApplyDamage;
    }

    private void OnDisable()
    {
        OnTakeDamage -= ApplyDamage;
    }

    void Awake()
    {
        _playerCamera = GetComponentInChildren<Camera>();
        _characterController = GetComponent<CharacterController>();
        _defaultYPos = _playerCamera.transform.localPosition.y;
        _currentHealth = _maxHealth;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        if (CanMove)
        {
            HandleMovementInput();
            HandleMouseInput();

            if (_canJump)
            {
                HandleJump();
            }

            if (_canCrouch)
            {
                HandleCrouch();
            }

            if (_canUseHeadbob)
            {
                HandleHeadBob();
            }

            ApplyFinalMovements();
        }
    }

    private void HandleMovementInput()
    {
        _currentInput = new Vector2(
            (_isCrouching ? _crouchSpeed : IsSprinting ? _sprintSpeed : _walkSpeed) * Input.GetAxis("Vertical"),
            (_isCrouching ? _crouchSpeed : IsSprinting ? _sprintSpeed : _walkSpeed) * Input.GetAxis("Horizontal"));

        var moveDirectionY = _moveDirection.y;
        _moveDirection = (transform.TransformDirection(Vector3.forward) * _currentInput.x) +
                         (transform.TransformDirection(Vector3.right) * _currentInput.y);
        _moveDirection.y = moveDirectionY;
    }
    
    private void HandleMouseInput()
    {
        _rotationX -= Input.GetAxis("Mouse Y") * _lookSpeedY;
        _rotationX = Mathf.Clamp(_rotationX, -_upperLookLimit, _lowerLookLimit);
        _playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _lookSpeedX, 0);
    }
    
    private void ApplyFinalMovements()
    {
        if (!_characterController.isGrounded)
        {
            _moveDirection.y -= _gravity * Time.deltaTime;
        }

        if (_canWillSlidenOnSlopes && IsSliding)
        {
            _moveDirection += new Vector3(_hitPointNormal.x, -_hitPointNormal.y, _hitPointNormal.z) * _slopeSpeed;
        }

        _characterController.Move(_moveDirection * Time.deltaTime);
    }
    
    private void HandleJump()
    {
        if (ShouldJump)
        {
            OnJump?.Invoke();
            _moveDirection.y = _jumpForce;
        }
    }
    
    private void HandleCrouch()
    {
        if (ShouldCrouch)
        {
            StartCoroutine(CrouchStand());
        }
    }

    private void ApplyDamage(float damage)
    {
        _currentHealth -= damage;
        OnDamage?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
        {
            KillPlayer();
        }
        else if (_regeneratingHealth != null)
        {
            StopCoroutine(_regeneratingHealth);
        }

        _regeneratingHealth = StartCoroutine(RegenerateHealth());
    }

    private void KillPlayer()
    {
        _currentHealth = 0;

        if (_regeneratingHealth != null)
        {
            StopCoroutine(_regeneratingHealth);
        }

        print("dead");
    }

    private void HandleHeadBob()
    {
        if (!_characterController.isGrounded)
        {
            return;
        }

        if (Mathf.Abs(_moveDirection.x) > 0.1f || Mathf.Abs(_moveDirection.z) > 0.1f)
        {
            _timer += Time.deltaTime * (_isCrouching ? _crouchBobSpeed : IsSprinting ? _sprintBobSpeed : _walkBobSpeed);
            _playerCamera.transform.localPosition = new Vector3(_playerCamera.transform.localPosition.x,
                _defaultYPos + Mathf.Sin(_timer) *
                (_isCrouching ? _crouchBobAmount : IsSprinting ? _sprintBobAmount : _walkBobAmount),
                _playerCamera.transform.localPosition.z);
        }
    }

    private void HandleLanding()
    {
        OnLanding?.Invoke();
    }

    private IEnumerator CrouchStand()
    {
        if (_isCrouching && Physics.Raycast(_playerCamera.transform.position, Vector3.up, 1.5f))
        {
            yield break;
        }

        _duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = _isCrouching ? _standingHeight : _crouchHeight;
        float currentHeight = _characterController.height;
        Vector3 targetCenter = _isCrouching ? _standingCenter : _crouchingCenter;
        Vector3 currentCenter = _characterController.center;

        while (timeElapsed < _timeToCrouch)
        {
            _characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / _timeToCrouch);
            _characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / _timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _characterController.height = targetHeight;
        _characterController.center = targetCenter;

        _isCrouching = !_isCrouching;

        _duringCrouchAnimation = false;
    }
    
    private IEnumerator RegenerateHealth()
    {
        yield return new WaitForSeconds(_timeBeforeRegenStarts);
        WaitForSeconds timeToWait = new WaitForSeconds(_healthTimeIncrement);

        while (_currentHealth < _maxHealth)
        {
            _currentHealth += _healthValueIncrement;

            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }

            OnHeal?.Invoke(_currentHealth);
            yield return timeToWait;
        }

        _regeneratingHealth = null;
    }
}