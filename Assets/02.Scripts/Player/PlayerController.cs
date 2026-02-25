using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPunObservable, IDamageable
{
    public PhotonView PhotonView;
    public PlayerStat Stat;

    private Animator _animator;
    private CharacterController _characterController;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!PhotonView.IsMine) return;
        
        if (!Stat.IsDead && transform.position.y < -10f)
        {
            PhotonView.RPC(nameof(RpcDieFromFall), RpcTarget.All);
        }
    }

    [PunRPC]
    private void RpcDieFromFall()
    {
        if (Stat.IsDead) return;
        Stat.Health = 0f;
        Die();
    }

    private void Die()
    {
        Stat.IsDead = true;
        _animator.SetTrigger("Die");
        
        if (PhotonView.IsMine)
        {
            Invoke(nameof(Respawn), 5f);
        }
    }

    private void Respawn()
    {
        PhotonView.RPC(nameof(RpcRespawn), RpcTarget.All, SpawnPoint.GetRandomPosition());
    }

    [PunRPC]
    private void RpcRespawn(Vector3 respawnPosition)
    {
        Stat.IsDead = false;
        Stat.Health = Stat.MaxHealth;
        Stat.Stamina = Stat.MaxStamina;

        if (_animator != null) _animator.Play("Idle");

        if (_characterController != null)
        {
            // CharacterController가 활성화되어 있으면 transform.position 변경이 무시될 수 있으므로 잠시 껐다 켭니다.
            _characterController.enabled = false;
            transform.position = respawnPosition;
            _characterController.enabled = true;
        }
        else
        {
            transform.position = respawnPosition;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            Debug.Log("데이터 전송 중...");
            stream.SendNext(Stat.Health);
            stream.SendNext(Stat.Stamina);
        }
        else if (stream.IsReading)
        {
            Debug.Log("데이터 수신 중...");
            Stat.Health = (float)stream.ReceiveNext();
            Stat.Stamina = (float)stream.ReceiveNext();
        }
    }
    
    private Dictionary<Type, PlayerAbility> _abilitiesCache = new();
    
    public T GetAbility<T>() where T : PlayerAbility
    {
        var type = typeof(T);

        if (_abilitiesCache.TryGetValue(type, out var ability))
        {
            return ability as T;
        }

        // 게으른 초기화/로딩 -> 처음에 곧바로 초기화/로딩을 하는게 아니라
        //                    필요할때만 하는.. 뒤로 미루는 기법
        ability = GetComponent<T>();

        if (ability == null)
        {
            throw new Exception($"어빌리티 {type.Name}을 {gameObject.name}에서 찾을 수 없습니다.");
        }
        
        _abilitiesCache[ability.GetType()] = ability;

        return ability as T;
    }

    [PunRPC]
    public void TakeDamage(float damage, int attackActorNubmer)
    {
        if (Stat.IsDead) return;

        Stat.Health -= damage;

        if (Stat.Health > 0f) return;
        
        PhotonRoomManager.Instance.OnPlayerDeath(attackActorNubmer);
        Die();
    }
}
