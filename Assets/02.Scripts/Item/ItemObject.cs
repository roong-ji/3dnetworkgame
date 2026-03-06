using Photon.Pun;
using UnityEngine;

public class ItemObject : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        var pv = other.GetComponent<PhotonView>();
        if (!pv.IsMine) return;
        
        ScoreManager.Instance.AddScore(100);
            
        ItemObjectFactory.Instance.RequestDelete(base.photonView.ViewID);
    }
}
