using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviour
{
    public GameObject MalePrefab;
    public GameObject FemalePrefab;

    public TMP_InputField NameInputField;
    public TMP_InputField RoomInputField;
    public Button CreateRoomButton;
    
    private CharacterType _characterType;

    private void Awake()
    {
        CreateRoomButton.onClick.AddListener(MakeRoom);
    }

    private void MakeRoom()
    {
        var nickname = NameInputField.text;
        var roomName = RoomInputField.text;

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(roomName)) return;
        
        PhotonNetwork.NickName = nickname;
        
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }
    
    public void OnClickMale() => OnClickCharacterButton(CharacterType.Male);
    public void OnClickFemale() => OnClickCharacterButton(CharacterType.Female);

    private void OnClickCharacterButton(CharacterType characterType)
    {
        _characterType = characterType;
        
        MalePrefab.SetActive(_characterType == CharacterType.Male);
        FemalePrefab.SetActive(_characterType == CharacterType.Female);
    }
}

public enum CharacterType
{
    None,
    Male,
    Female
}
