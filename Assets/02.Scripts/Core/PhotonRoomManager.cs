using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonRoomManager : MonoBehaviourPunCallbacks
{
    public static PhotonRoomManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public event Action OnChanged;
    public event Action<Player> OnPlayerEntered;
    public event Action<Player> OnPlayerLeft;
    public event Action<string, string> OnPlayerDied;
    
    private Room _room;
    public Room Room => _room;
    
    // 방 입장 성공 시 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        Debug.Log("룸 입장 완료!");
        
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        
        _room = PhotonNetwork.CurrentRoom;
        var roomPlayers = _room.Players;
        foreach (var player in roomPlayers)
        {
            Debug.Log($"{player.Value.NickName} : {player.Value.ActorNumber}");
        }

        // 리소스 폴더에서 프리팹을 찾아 생성, 다른 방법이 더 좋음
        var startPos = SpawnPoint.GetRandomPosition();
        PhotonNetwork.Instantiate("Player", startPos, Quaternion.identity);
        
        OnChanged?.Invoke();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnChanged?.Invoke();
        OnPlayerEntered?.Invoke(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        OnChanged?.Invoke();
        OnPlayerLeft?.Invoke(otherPlayer);
    }

    public void OnPlayerDeath(int attackerActorNumber)
    {
        var attackerName = _room.Players[attackerActorNumber].NickName;
        var victimNickname = PhotonNetwork.LocalPlayer.NickName;

        OnPlayerDied?.Invoke(attackerName, victimNickname);
    }
}
