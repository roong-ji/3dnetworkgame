using Photon.Pun;
using UnityEngine;

public class BearSpawner : MonoBehaviour
{
    [SerializeField] private string bearPrefabName = "Bear";
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnBear();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SpawnBear()
    {
        PhotonNetwork.InstantiateRoomObject(
            bearPrefabName,
            spawnPoint.position,
            spawnPoint.rotation
        );
    }
}
