using Animancer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private JoyStick _moveStick;
    [SerializeField] private JoyStick _aimStick;

    [SerializeField] private CharacterController _characterController;

    [SerializeField] private AnimancerComponent _animancer;

    [SerializeField] private AvatarMask _avatarMask;
    public ClipTransition Idle;
    public ClipTransition Attack;
    [SerializeField] private ClipTransition _weaponPutAway;
    [SerializeField] private ClipTransition _moveForward;
    [SerializeField] private ClipTransition _moveBackward;
    [SerializeField] private ClipTransition _moveLeft;
    [SerializeField] private ClipTransition _moveRight;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _turnSpeed = 8f;

    [Header("Inventory")]
    [SerializeField] private InventoryComponent _inventory;

    private AnimancerLayer _baseLayer;
    private AnimancerLayer _actionLayer;

    private Vector2 _moveInput;
    private Vector2 _aimInput;

    private Camera _mainCamera;
    private CameraController _cameraController;

    private void Awake()
    {
        _baseLayer = _animancer.Layers[0];
        _actionLayer = _animancer.Layers[1];
        _actionLayer.SetMask(_avatarMask);
    }

    private void Start()
    {
        _moveStick.OnStickValueChanged += MoveStick_OnStickValueChanged;
        _aimStick.OnStickValueChanged += AimStick_OnStickValueChanged;
        _aimStick.onStickTaped += SwitchWeapon_onStickTaped;

        _mainCamera = Camera.main;
        _cameraController = FindAnyObjectByType<CameraController>();
    }

    private void AttackPoint()
    {
        _inventory.GetActiveWeapon().Attack();
    }

    private void SwitchWeapon_onStickTaped()
    {
        if (_aimInput.magnitude > 0)
        {
            _baseLayer.Play(_weaponPutAway).Events.OnEnd = SwitchWeapon;
        }
    }

    private void SwitchWeapon()
    {
        _inventory.NextWeapon();
    }

    private void Update()
    {
        PerformMoveAndAim();
        UpdateAnimation();
        UpdateCamera();
    }

    private void UpdateAnimation()
    {
        // Check if the player is not moving or firing
        if (_moveInput.magnitude == 0 && _aimInput.magnitude == 0)
        {
            // Play idle animation
            _baseLayer.Play(Idle);
        }
    }

    private void PerformMoveAndAim()
    {
        Vector3 moveDir = StickInputToWorldDir(_moveInput);

        float angle = Vector3.SignedAngle(Vector3.forward, moveDir, Vector3.up);

        // Play appropriate movement animation based on movement direction
        if (angle >= 70f && angle < 130f && _moveInput.magnitude != 0)
        {
            _actionLayer.Play(Idle);
            _baseLayer.Play(_moveForward);
        }
        else if (angle >= 130f && angle < 180f && _moveInput.magnitude != 0)
        {
            _actionLayer.Play(Idle);
            _baseLayer.Play(_moveRight); 
        }
        else if (angle >= 20f && angle < 70f && _moveInput.magnitude != 0)
        {
            _actionLayer.Play(Idle);
            _baseLayer.Play(_moveLeft);
        }
        else
        {
            if(_moveInput.magnitude != 0)
            {
                _actionLayer.Play(Idle);
                _baseLayer.Play(_moveBackward);
            }
        }

        _characterController.Move(moveDir * Time.deltaTime * _moveSpeed);

        UpdateAim(moveDir);

        _characterController.Move(Vector3.down * Time.deltaTime * 10f);
    }

    private void UpdateAim(Vector3 currentMoveDir)
    {
        Vector3 aimDir = currentMoveDir; // at the start it is equal to moveDir

        if (_aimInput.magnitude != 0)
        {
            aimDir = StickInputToWorldDir(_aimInput);
        }

        RotateTowards(aimDir);
    }

    private void UpdateCamera()
    {
        //if player is moving but not aiming, and camera controller exists
        if (_moveInput.magnitude != 0 && _aimInput.magnitude == 0 && _cameraController != null)
        {
            _cameraController.AddYawInput(_moveInput.x);
        }
    }

    private void RotateTowards(Vector3 aimDir)
    {
        if (aimDir.magnitude != 0)
        {
            float turnLerpAlpha = _turnSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(aimDir, Vector3.up), turnLerpAlpha);
        }
    }

    private void AimStick_OnStickValueChanged(Vector2 inputValue)
    {
        _aimInput = inputValue;
        if (_aimInput.magnitude > 0 && Attack != null)
        {
            var state = _actionLayer.Play(Attack);
            state.Events.Add(0.18f, AttackPoint);
        }
    }

    private void MoveStick_OnStickValueChanged(Vector2 inputValue)
    {
        _moveInput = inputValue;
    }

    private Vector3 StickInputToWorldDir(Vector2 inputValue)
    {
        Vector3 rightDir = _mainCamera.transform.right;
        Vector3 upDir = Vector3.Cross(rightDir, Vector3.up);

        Vector3 characterDir = rightDir * inputValue.x + upDir * inputValue.y;

        return characterDir;
    }
}
