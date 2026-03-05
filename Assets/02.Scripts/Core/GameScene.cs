using Photon.Pun;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    private void Start()
    {
        // 리소스 폴더에서 프리팹을 찾아 생성, 다른 방법이 더 좋음
        var startPos = SpawnPoint.GetRandomPosition();
        PhotonNetwork.Instantiate("Player", startPos, Quaternion.identity);
    }
}
