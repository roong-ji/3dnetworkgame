using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotateAbility : PlayerAbility
{
    public Transform CameraRoot;
    
    private float _mx;
    private float _my;

    private void Start()
    {
        if (!_owner.PhotonView.IsMine) return;

        Cursor.lockState = CursorLockMode.Locked;

        CinemachineCamera vcam = GameObject.Find("FollowCamera").GetComponent<CinemachineCamera>();
        vcam.Follow = CameraRoot.transform;
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine) return;

        _mx += Mouse.current.delta.x.ReadValue() * _owner.Stat.RotationSpeed * Time.deltaTime;
        _my += Mouse.current.delta.y.ReadValue() * _owner.Stat.RotationSpeed * Time.deltaTime;
        
        _my = Mathf.Clamp(_my, -90f, 90f);
        
        transform.eulerAngles    = new Vector3(0f, _mx, 0f);
        CameraRoot.localRotation = Quaternion.Euler(-_my, 0f, 0f);
    }
}
