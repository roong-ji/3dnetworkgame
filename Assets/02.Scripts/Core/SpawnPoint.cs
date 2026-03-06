using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private static readonly List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();

    private void Awake()
    {
        _spawnPoints.Add(this);
    }

    private void OnDestroy()
    {
        _spawnPoints.Remove(this);
    }

    public static Vector3 GetRandomPosition()
    {
        if (_spawnPoints.Count == 0)
        {
            Debug.LogWarning("SpawnPoint가 씬에 존재하지 않습니다. 기본 위치(0,0,0)를 반환합니다.");
            return Vector3.zero;
        }

        int index = Random.Range(0, _spawnPoints.Count);
        return _spawnPoints[index].transform.position;
    }
}
