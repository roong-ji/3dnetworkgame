using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackAbility : PlayerAbility
{
    private Animator _animator;

    [SerializeField] private EAnimationSequenceType _animationSequenceType;

    private int _prevAnimationNumber = 0;
    private float _attackTimer = 0f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnAttack(InputValue value)
    {
        if (!_owner.PhotonView.IsMine || !value.isPressed || _owner.Stat.IsDead) return;
        Attack();
    }
    
    private void Update()
    {
        if (!_owner.PhotonView.IsMine) return;
        _attackTimer += Time.deltaTime;
    }

    private void Attack()
    {
        if (_attackTimer < _owner.Stat.AttackSpeed) return;

        _attackTimer = 0f;

        int animationNumber = 0;
        switch (_animationSequenceType)
        {
            case EAnimationSequenceType.Sequence:
                animationNumber = 1 + (_prevAnimationNumber++) % 3;
                break;
                
            case EAnimationSequenceType.Random:
                animationNumber = Random.Range(1, 4);
                break;
        }
        
        PlayAttackAnimation(animationNumber);
        
        _owner.PhotonView.RPC(nameof(PlayAttackAnimation), RpcTarget.Others, animationNumber);
    }

    [PunRPC]
    private void PlayAttackAnimation(int animationNumber)
    {
        _animator.SetTrigger($"Attack{animationNumber}");
    }
}

public enum EAnimationSequenceType
{
    Sequence,
    Random,
}
