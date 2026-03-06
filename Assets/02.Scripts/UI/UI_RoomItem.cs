using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomItem : MonoBehaviour
{
    public TMP_InputField NameInputField;
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
        MaskNickNameTextUI.SetText(roomInfo.CustomProperties["MasterName"].ToString());
        PlayerCountTextUI.SetText($"{roomInfo.PlayerCount} / {roomInfo.MaxPlayers}");
    }

    private void EnterRoom()
    {
        if (_roomInfo == null) return;
        
        PhotonNetwork.NickName = NameInputField.text;
        PhotonNetwork.JoinRoom(_roomInfo.Name);
    }
}
