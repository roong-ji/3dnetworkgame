using Photon.Pun;
using UnityEngine;

public class PlayerWeaponHitAbility : PlayerAbility
{
    private void OnTriggerEnter(Collider other)
    {
        if (_owner.PhotonView.IsMine) return;
        
        if (other.transform == _owner.transform) return;
        
        if (!other.TryGetComponent<IDamageable>(out var damageable)) return;
        
        var actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        
        var otherPlayer = other.GetComponent<PlayerController>();
        otherPlayer.PhotonView.RPC(nameof(damageable.TakeDamage), RpcTarget.All, _owner.Stat.AttackPower, actorNumber);

        _owner.GetAbility<PlayerWeaponColliderAbility>().DeActiveCollider();
    }
}
