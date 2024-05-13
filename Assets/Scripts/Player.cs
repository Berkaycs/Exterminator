using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private JoyStick _moveStick;
    [SerializeField] private JoyStick _aimStick;

    [SerializeField] private CharacterController _characterController;

    private Camera _mainCamera;
    private CameraController _cameraController;

    private Vector2 _moveInput;
    private Vector2 _aimInput;

    private float _moveSpeed = 20f;
    private float _turnSpeed = 30f;

    private void Start()
    {
        _moveStick.OnStickValueChanged += MoveStick_OnStickValueChanged;
        _aimStick.OnStickValueChanged += AimStick_OnStickValueChanged;

        _mainCamera = Camera.main;
        _cameraController = FindAnyObjectByType<CameraController>();
    }

    private void Update()
    {
        PerformMoveAndAim();
        UpdateCamera();
    }

    private void PerformMoveAndAim()
    {
        Vector3 moveDir = StickInputToWorldDir(_moveInput);
        _characterController.Move(moveDir * Time.deltaTime * _moveSpeed);

        UpdateAim(moveDir);
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
