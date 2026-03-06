using System;
using Cysharp.Threading.Tasks;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviour
{
    public TMP_InputField NameInputField;
    public TMP_InputField RoomInputField;
    public Button CreateRoomButton;
    public Button MaleButton;
    public Button FemaleButton;
    
    public AssetBundleManager BundleManager;
    public Transform CharacterAnchor;
    private GameObject CurrentCharacter;

    private const string BundleName = "character";
    
    private void Awake()
    {
        CreateRoomButton.onClick.AddListener(MakeRoom);
        MaleButton.onClick.AddListener(OnClickMale);
        FemaleButton.onClick.AddListener(OnClickFemale);
        
        BundleManager.DownloadBundleAsync(BundleName).Forget();
    }

    private void OnDestroy()
    {
        BundleManager.UnloadBundle(BundleName);
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

        var customProps = new Hashtable();
        customProps.Add("MasterName", nickname);
        roomOptions.CustomRoomProperties = customProps;
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "MasterName" };
        
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }
    
    private void OnClickMale() => OnClickCharacterButton(BundleName, "Male").Forget();
    private void OnClickFemale() => OnClickCharacterButton(BundleName, "Female").Forget();

    private async UniTask OnClickCharacterButton(string bundleName, string assetName)
    {
        var character = await BundleManager.LoadAssetAsync(bundleName, assetName, CharacterAnchor);
        
        if (CurrentCharacter != null)
        {
            Destroy(CurrentCharacter);
        }
        
        CurrentCharacter = character;
        CurrentCharacter.transform.localPosition = Vector3.zero;
        CurrentCharacter.transform.localRotation = Quaternion.identity;
    }
}

public enum CharacterType
{
    None,
    Male,
    Female
}
