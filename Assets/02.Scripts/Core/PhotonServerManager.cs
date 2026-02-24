using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonServerManager : MonoBehaviourPunCallbacks
{
    private string _version = "0.0.1";
    private string _nickname = "roongji";

    private void Start()
    {
        _nickname += $"_{Random.Range(100, 999)}";

        PhotonNetwork.GameVersion = _version;
        PhotonNetwork.NickName = _nickname;
        
        PhotonNetwork.SendRate          = 30; // 얼마나 자주 데이터를 송수신할 것인가..  (실제 송수신)
        PhotonNetwork.SerializationRate = 30; // 얼마나 자주 데이터를 직렬화 할 것인지.  (송수신 준비)
        
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    // 포톤 서버에 접속 성공 시 호출되는 콜백 함수
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

    // 로비 입장 성공 시 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 완료!");
        Debug.Log(PhotonNetwork.InLobby);

        PhotonNetwork.JoinRandomRoom();
    }

    // 방 입장 성공 시 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        Debug.Log("룸 입장 완료!");
        
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        
        var roomPlayers = PhotonNetwork.CurrentRoom.Players;
        foreach (var player in roomPlayers)
        {
            Debug.Log($"{player.Value.NickName} : {player.Value.ActorNumber}");
        }

        // 리소스 폴더에서 프리팹을 찾아 생성, 다른 방법이 더 좋음
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"랜덤 방 입장에 실패했습니다: {returnCode} - {message}");
        
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.CreateRoom("test", roomOptions);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"방 입장에 실패했습니다: {returnCode} - {message}");
    }
}
