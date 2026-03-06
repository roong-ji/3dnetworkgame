using UnityEngine;
using Photon.Pun;

public class ItemObjectFactory : MonoBehaviour
{
    public static ItemObjectFactory Instance { get; private set; }
    private PhotonView _photonView;

    private void Awake()
    {
        Instance = this;
        
        _photonView = GetComponent<PhotonView>();
    }

    public void RequestMakeScoreItems(Vector3 makePosition)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 방장이라면 그냥 함수 호출
            MakeScoreItems(makePosition);
        }
        else
        {
            // 아니라면 방장의 함수를 호출
            _photonView.RPC(nameof(MakeScoreItems), RpcTarget.MasterClient, makePosition);
        }
    }
    
    [PunRPC]
    private void MakeScoreItems(Vector3 makePosition)
    {
        int randomCount = Random.Range(3, 5);
        
        for (int i = 0; i < randomCount; i++)
        {
            var randomOffset = Random.insideUnitSphere;
            randomOffset.y = 0;
            PhotonNetwork.InstantiateRoomObject("ScoreItem", makePosition + randomOffset, Quaternion.identity);
        }
    }

    public void RequestDelete(int viewId)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Delete(viewId);
        }
        else
        {
            _photonView.RPC(nameof(Delete), RpcTarget.MasterClient, viewId);
        }
    }
    
    [PunRPC]
    private void Delete(int viewId)
    {
        var objectToDelete = PhotonView.Find(viewId)?.gameObject;
        if (objectToDelete == null) return;
        
        PhotonNetwork.Destroy(objectToDelete);
    }
}