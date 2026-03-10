using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotateAbility : PlayerAbility
{
    public Transform CameraRoot;

    private const float MinVerticalAngle = -80f;
    private const float MaxVerticalAngle = 80f;
    private const float InputSpikeThreshold = 30f;
    
    private float _mx;
    private float _my;
    private Vector2 _lookInput;

    private bool _inputMode = true;
    
    public void OnLook(InputValue value)
    {
        _lookInput = value.Get<Vector2>();
    }

    public void OnEsc(InputValue value)
    {
        if (!value.isPressed) return;
    
        _inputMode = !_inputMode;
        Cursor.lockState = _inputMode ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !_inputMode; 
    }

    private void Start()
    {
        if (!_owner.PhotonView.IsMine) return;

        Cursor.lockState = CursorLockMode.Locked;

        var vcam = GameObject.Find("FollowCamera").GetComponent<CinemachineCamera>();
        vcam.Follow = CameraRoot.transform;
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine || !_inputMode) return;
        
        var filteredX = Mathf.Abs(_lookInput.x) > InputSpikeThreshold ? 0f : _lookInput.x;
        var filteredY = Mathf.Abs(_lookInput.y) > InputSpikeThreshold ? 0f : _lookInput.y;

        _mx += filteredX * _owner.Stat.RotationSpeed;
        _my += filteredY * _owner.Stat.RotationSpeed;
        
        _my = Mathf.Clamp(_my, MinVerticalAngle, MaxVerticalAngle);
        
        transform.eulerAngles = new Vector3(0f, _mx, 0f);
        CameraRoot.localRotation = Quaternion.Euler(-_my, 0f, 0f);
    }
}
