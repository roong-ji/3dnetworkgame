using UnityEngine;

public class PlayerWeaponColliderAbility : PlayerAbility
{
    [SerializeField] private Collider _collider;

    private void ActiveCollider()
    {
        _collider.enabled = true;
    }

    private void DeActiveCollider()
    {
        _collider.enabled = false;
    }
}
