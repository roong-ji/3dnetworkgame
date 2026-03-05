using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        _room = PhotonNetwork.CurrentRoom;

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("BattleScene");
        }
        
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

    public void OnPlayerDeath(int attackerActorNumber, string victimNickName)
    {
        var attackerName = _room.Players[attackerActorNumber].NickName;
        OnPlayerDied?.Invoke(attackerName, victimNickName);
    }
}
