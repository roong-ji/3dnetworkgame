using System;
using UnityEngine;
using Photon.Pun;

public class BearController : MonoBehaviour, IDamageable
{
    public readonly BearStat Stat = new();
    
    public event Action OnDeath;
    
    private PhotonView _photonView;
    private Animator _animator;
    private CharacterController _characterController;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();

        // 개발 편의를 위한 자동 관찰(Observe) 등록 방어코드
        // 유니티 인스펙터에서 리스트에 드래그를 깜빡하더라도 코드로 보정해줍니다.
        if (_photonView != null)
        {
            var transformView = GetComponent<PhotonTransformView>();
            var animatorView = GetComponent<PhotonAnimatorView>();

            if (transformView != null && !_photonView.ObservedComponents.Contains(transformView))
                _photonView.ObservedComponents.Add(transformView);
                
            if (animatorView != null && !_photonView.ObservedComponents.Contains(animatorView))
                _photonView.ObservedComponents.Add(animatorView);
        }

        Stat.Health = Stat.MaxHealth;
        Stat.IsDead = false;
    }

    public void TakeDamage(float damage, int attackActorNumber)
    {
        if (Stat.IsDead) return;

        _photonView.RPC(nameof(RpcTakeDamage), RpcTarget.All, damage, attackActorNumber);
    }

    [PunRPC]
    private void RpcTakeDamage(float damage, int attackActorNumber)
    {
        if (Stat.IsDead) return;

        Stat.Health -= damage;

        if (Stat.Health > 0f) return;
        
        Die();
        PhotonRoomManager.Instance.OnPlayerDeath(attackActorNumber, "Bear");
    }

    private void Die()
    {
        Stat.IsDead = true;
        Stat.Health = 0f;

        OnDeath?.Invoke();

        _characterController.enabled = false;

        _animator.SetBool("Death", true);

        if (!PhotonNetwork.IsMasterClient) return;
        Invoke(nameof(Destroy), 3f);
    }

    private void Destroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }
    
    public void MoveBody(Vector3 velocity)
    {
        if (Stat.IsDead || !_characterController.enabled) return;
        _characterController.Move(velocity * Time.deltaTime);
    }

    public void RotateBody(Quaternion targetRot, float rotSpeed)
    {
        if (Stat.IsDead) return;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
    }

    public void TriggerAnim(string paramName)
    {
        if (Stat.IsDead) return;
        _animator.SetTrigger(paramName);
    }
    
    public void SetAnimBool(string paramName, bool value)
    {
        if (Stat.IsDead) return;
        _animator.SetBool(paramName, value);
    }
}
