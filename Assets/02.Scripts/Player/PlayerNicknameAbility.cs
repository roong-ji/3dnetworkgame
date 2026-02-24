using TMPro;
using UnityEngine;

public class PlayerNicknameAbility : PlayerAbility
{
    [SerializeField] private TextMeshProUGUI _nicknameTextUI;
    private Camera _camera;
    
    private void Start()
    {
        _camera = Camera.main;
        
        _nicknameTextUI.text = _owner.PhotonView.Owner.NickName;
        
        if (_owner.PhotonView.IsMine)
        {
            _nicknameTextUI.color = Color.green;
        }
        else
        {
            _nicknameTextUI.color = Color.red;
        }
    }

    private void Update()
    {
        transform.forward = _camera.transform.forward;
    }
}
