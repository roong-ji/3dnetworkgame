using System;
using UnityEngine;

public class BearController : MonoBehaviour, IDamageable
{
    public BearStat Stat = new BearStat();
    
    public event Action OnDeath;
    
    private Animator _animator;
    private CharacterController _characterController;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();

        Stat.Health = Stat.MaxHealth;
        Stat.IsDead = false;
    }

    public void TakeDamage(float damage, int attackActorNumber)
    {
        if (Stat.IsDead) return;

        Stat.Health -= damage;

        if (Stat.Health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Stat.IsDead = true;
        Stat.Health = 0f;

        OnDeath?.Invoke();

        _characterController.enabled = false;

        _animator.SetTrigger("Die");

        Destroy(gameObject, 3f);
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
    
    public void RebindAnim()
    {
        if (Stat.IsDead) return;
        _animator.Rebind();
    }
}
