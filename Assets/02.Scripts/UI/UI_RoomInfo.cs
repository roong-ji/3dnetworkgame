using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roomNameTextUI;
    [SerializeField] private TextMeshProUGUI _roomCountTextUI;
    [SerializeField] private Button _roomExitButton;

    private void Start()
    {
        _roomExitButton.onClick.AddListener(ExitRoom);

        PhotonRoomManager.Instance.OnChanged += Refresh;
        
        Refresh();
    }

    private void OnDestroy()
    {
        if (PhotonRoomManager.Instance == null) return;
        PhotonRoomManager.Instance.OnChanged -= Refresh;
    }

    private void Refresh()
    {
        var room = PhotonRoomManager.Instance.Room;
        if (room == null) return;
        
        _roomNameTextUI.SetText(room.Name);
        _roomCountTextUI.SetText($"{room.PlayerCount} / {room.MaxPlayers}");
    }

    private void ExitRoom()
    {
        
    }
}
