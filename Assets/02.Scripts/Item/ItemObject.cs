using Photon.Pun;
using UnityEngine;

public class ItemObject : MonoBehaviourPun
{
    [Header("Drop Settings")]
    [SerializeField] private float minForce = 3f;
    [SerializeField] private float maxForce = 6f;
    [SerializeField] private float upForce = 5f;
    [SerializeField] private float torqueForce = 10f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        ApplyDropForce();
    }

    private void ApplyDropForce()
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized;
        Vector3 dropDirection = new Vector3(randomCircle.x, 0, randomCircle.y);

        float randomForwardForce = Random.Range(minForce, maxForce);
        Vector3 finalForce = (dropDirection * randomForwardForce) + (Vector3.up * upForce);

        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.AddForce(finalForce, ForceMode.Impulse);

        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f), 
            Random.Range(-1f, 1f), 
            Random.Range(-1f, 1f)
        ) * torqueForce;
        
        _rigidbody.AddTorque(randomTorque, ForceMode.Impulse);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("아이템 충돌!");
            
            other.GetComponent<PlayerStat>().Score += 100;
            
            ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
        }
    }
}
