using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonServerManager : MonoBehaviourPunCallbacks
{
    private string _version = "0.0.1";
    private string _nickname = "roong-ji";

    private void Start()
    {
        PhotonNetwork.GameVersion = _version;
        PhotonNetwork.NickName = _nickname;
        
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        Debug.Log("네임서버 접속 완료");
        
        Debug.Log(PhotonNetwork.CloudRegion);
    }

    public override void OnConnectedToMaster()
    {
        //var lobby = new TypedLobby("3channel", LobbyType.Default);
        
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 완료!");
        Debug.Log(PhotonNetwork.InLobby);

        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("룸 입장 완료!");
        
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"랜덤 방 입장에 실패했습니다: {returnCode} - {message}");
        
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        roomOptions.IsVisible = true;
        
        
    }
}
