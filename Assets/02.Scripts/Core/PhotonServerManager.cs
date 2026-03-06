using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonServerManager : MonoBehaviourPunCallbacks
{
    private string _version = "0.0.1";

    private void Start()
    {
        PhotonNetwork.GameVersion = _version;
        
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
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"랜덤 방 입장에 실패했습니다: {returnCode} - {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"방 입장에 실패했습니다: {returnCode} - {message}");
    }
}
