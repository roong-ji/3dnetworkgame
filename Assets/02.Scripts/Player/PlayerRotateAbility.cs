using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotateAbility : PlayerAbility
{
    public Transform CameraRoot;
    
    private float _mx;
    private float _my;
    
    private Vector2 _lookInput;
    
    public void OnLook(InputValue value)
    {
        _lookInput = value.Get<Vector2>();
    }

    private void Start()
    {
        Debug.Log(_owner.PhotonView.IsMine);
        
        if (!_owner.PhotonView.IsMine) return;

        Cursor.lockState = CursorLockMode.Locked;

        var vcam = GameObject.Find("FollowCamera").GetComponent<CinemachineCamera>();
        vcam.Follow = CameraRoot.transform;
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine) return;

        _mx += _lookInput.x * _owner.Stat.RotationSpeed * Time.deltaTime;
        _my += _lookInput.y * _owner.Stat.RotationSpeed * Time.deltaTime;
        
        _my = Mathf.Clamp(_my, -90f, 90f);
        
        transform.eulerAngles    = new Vector3(0f, _mx, 0f);
        CameraRoot.localRotation = Quaternion.Euler(-_my, 0f, 0f);
    }
}
