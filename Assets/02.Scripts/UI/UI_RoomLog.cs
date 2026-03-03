using System;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class UI_RoomLog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _logText;

    private void Start()
    {
        _logText.text = "You Enter the room.";
        
        PhotonRoomManager.Instance.OnPlayerEntered += OnPlayerEnter;
        PhotonRoomManager.Instance.OnPlayerLeft += OnPlayerLeft;
        PhotonRoomManager.Instance.OnPlayerDied += PlayerDeathLog;
    }

    private void OnDestroy()
    {
        if (PhotonRoomManager.Instance == null) return;
        PhotonRoomManager.Instance.OnPlayerEntered -= OnPlayerEnter;
        PhotonRoomManager.Instance.OnPlayerLeft -= OnPlayerLeft;
        PhotonRoomManager.Instance.OnPlayerDied -= PlayerDeathLog;
    }

    private void OnPlayerEnter(Player player)
    {
        _logText.text += $"{player.NickName} Enter the room.\n";
    }

    private void OnPlayerLeft(Player player)
    {
        _logText.text += $"{player.NickName} Exit the room.\n";
    }

    private void PlayerDeathLog(string attackerNickName, string victimNickName)
    {
        _logText.text += $"{attackerNickName} Killed {victimNickName}";
    }
}
