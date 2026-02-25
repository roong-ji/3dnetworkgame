using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveAbility : PlayerAbility
{
    
    private const float Gravity = 9.8f;
    private float _yVelocity = 0f;

    private CharacterController _characterController;
    private Animator _animator;
    private Camera _camera;
    
    private Vector2 _moveInput;
    private bool _isDashPressed;
    
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _camera = Camera.main;
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }
    
    public void OnJump(InputValue value)
    {
        if (_owner.Stat.IsDead) return;
        if (!value.isPressed || !_characterController.isGrounded) return;
        _yVelocity = _owner.Stat.JumpPower;
    }

    public void OnDash(InputValue value)
    {
        _isDashPressed = value.isPressed;
    }
    
    private void Update()
    {
        if (!_owner.PhotonView.IsMine || _owner.Stat.IsDead) return;

        var h = _moveInput.x;
        var v = _moveInput.y;
        
        var direction = new Vector3(h, 0, v);
        direction.Normalize();
        
        direction = _camera.transform.TransformDirection(direction);

        _animator.SetFloat("Move", direction.magnitude);

        _yVelocity -= Gravity * Time.deltaTime;
        
        direction.y = _yVelocity;

        var currentSpeed = _owner.Stat.MoveSpeed;
        
        if (_isDashPressed && _owner.Stat.Stamina > 0f)
        {
            currentSpeed = _owner.Stat.RunSpeed;
            _owner.Stat.Stamina -= Time.deltaTime * 5f;
        }
        else
        {
            _owner.Stat.Stamina += Time.deltaTime * 2f;
        }

        _owner.Stat.Stamina = Mathf.Clamp(_owner.Stat.Stamina, 0f, _owner.Stat.MaxStamina);
        
        _characterController.Move(direction * Time.deltaTime * currentSpeed);
    }
}
