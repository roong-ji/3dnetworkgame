using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomItem : MonoBehaviour
{
    public TextMeshProUGUI NameTextUI;
    public TextMeshProUGUI MaskNickNameTextUI;
    public TextMeshProUGUI PlayerCountTextUI;
    public Button RoomEnterButton;
    
    private RoomInfo _roomInfo;

    private void Awake()
    {
        RoomEnterButton.onClick.AddListener(EnterRoom);
    }

    public void Init(RoomInfo roomInfo)
    {
        _roomInfo = roomInfo;
        
        NameTextUI.SetText(roomInfo.Name);
        MaskNickNameTextUI.SetText("roongji");
        PlayerCountTextUI.SetText($"{roomInfo.PlayerCount} / {roomInfo.MaxPlayers}");
    }

    private void EnterRoom()
    {
        if (_roomInfo == null) return;
        
        PhotonNetwork.JoinRoom(_roomInfo.Name);
    }
}
