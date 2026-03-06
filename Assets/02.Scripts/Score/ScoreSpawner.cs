using System.Collections;
using Photon.Pun;
using UnityEngine;

public class ScoreSpawner : MonoBehaviour
{
    public Vector2 SpawnRangeX = new (-40f, 30f);
    public Vector2 SpawnRangeZ = new (-35f, 5f);
    private const float SpawnInterval = 10f;
    private WaitForSeconds _spawnWait = new(SpawnInterval);

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Destroy(gameObject);
            return;
        }
        
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return _spawnWait;
        
            var randomX = Random.Range(SpawnRangeX.x, SpawnRangeX.y);
            var randomZ = Random.Range(SpawnRangeZ.x, SpawnRangeZ.y);
            var spawnPosition = new Vector3(randomX, 0, randomZ);
        
            ItemObjectFactory.Instance.RequestMakeScoreItems(spawnPosition);
        }
    }
}
