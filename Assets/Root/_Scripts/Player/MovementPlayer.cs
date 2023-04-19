using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class MovementPlayer : MonoBehaviour
{
    [Header("Movement player")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _gravity = -9.81f;

    [Header("Dash")]
    [SerializeField] private float _dashPower;
    [SerializeField] public float _DashCooldown;
    [SerializeField] private float _dashDuration;
    public bool _IsDashing;


    private Vector3 _playerVelocity;

    private Vector3 _movementInput;
    private Vector3 _aimInput;

    [Header("Controller settings")]
    [SerializeField] private float _controllerDeadzone = 0.1f;
    [SerializeField] private float _gamepadRotationSmoothing = 300f;

    private bool _isGamepad;

    [Header("References")]
    private CharacterController _characterController;
    private PlayerControls _playerControls;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    void Update()
    {
        HandleInput();
        HandleMovement();
        if (!_IsDashing && _playerControls.Controls.Dash.ReadValue<float>() > 0.1f)
            StartCoroutine(Dash());

        if (Time.timeScale == 1f)
            HandleRotation();
    }

    /// <summary>
    /// Gets the input from the new input manager
    /// </summary>
    void HandleInput()
    {
        _movementInput = _playerControls.Controls.Movement.ReadValue<Vector2>();
        _aimInput = _playerControls.Controls.Aim.ReadValue<Vector2>();
    }

    /// <summary>
    /// Adds forces to the character controller
    /// </summary>
    void HandleMovement()
    {
        Vector3 move = new Vector3(_movementInput.x, 0, _movementInput.y);
        _characterController.Move(move * Time.deltaTime * _moveSpeed);

        // Apply gravity
        _playerVelocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);
    }

    /// <summary>
    /// Handles the rotation of the player
    /// </summary>
    void HandleRotation()
    {
        if (_isGamepad)
        {
            // rotate towards controller
            if (Mathf.Abs(_aimInput.x) > _controllerDeadzone || Mathf.Abs(_aimInput.y) > _controllerDeadzone)
            {
                Vector3 playerDirection = Vector3.right * _aimInput.x + Vector3.forward * _aimInput.y;

                if (playerDirection.sqrMagnitude > 0.0f)
                {
                    Quaternion newrotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, _gamepadRotationSmoothing * Time.deltaTime);
                }
            }
        }

        else
        {
            //// Rotate towards mouse
            //Ray ray = Camera.main.ScreenPointToRay(_aimInput);
            //Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            //float rayDistance;

            //if (groundPlane.Raycast(ray, out rayDistance))
            //{
            //    Vector3 point = ray.GetPoint(rayDistance);
            //    LookAt(point);

            //    Quaternion targetRotation = Quaternion.LookRotation(point - transform.position, Vector3.up);
            //    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _gamepadRotationSmoothing * Time.deltaTime);
            //}

            // rotate towards mouse
            Ray ray = Camera.main.ScreenPointToRay(_aimInput);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                LookAt(point);
            }
        }
    }

    /// <summary>
    /// Sets the player to look at the mouse with the players y position
    /// </summary>
    /// <param name="lookPoint"></param>
    private void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }


    /// <summary>
    /// This method is called when the player changes the input device (gets called in using a unity event in the _playerControls
    /// </summary>
    /// <param name="playerInput"></param>
    public void OnDeviceChange(PlayerInput playerInput)
    {
        _isGamepad = playerInput.currentControlScheme.Equals("Controller") ? true : false;
    }

    private IEnumerator Dash()
    {
        Vector3 dashDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        float startTime = Time.time;
        //Puts is dashing to true
        _IsDashing = true;

        //While the startTime is lower then Time.time + dashDuration
        while (Time.time < startTime + _dashDuration)
        {
            //Move the player from the _movementInput variable * dashPower
            _characterController.Move(dashDirection * _dashPower * Time.deltaTime);
            yield return null;
        }
        //Wait for the cooldown
        yield return new WaitForSeconds(_DashCooldown);
        //You are no longer dashing
        _IsDashing = false;
    }
}
