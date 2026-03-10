using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Chat;
using UnityEngine;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    public static ChatManager Instance { get; private set; }

    private const string ChannelSkku   = "skku2";
    private const string ChannelNotice = "notice";
    
    private ChatClient _chatClient;

    public event Action OnDataChanged;

    private readonly List<Chat> _chats = new();
    public IReadOnlyList<Chat> Chats => _chats;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _chatClient = new(this);
        _chatClient.DebugOut = DebugLevel.ALL;
        _chatClient.ChatRegion = "ASIA";
        var auth = new AuthenticationValues("민규");
        _chatClient.Connect("8952ee8f-6b2e-45c9-9cdb-b60a2f906585", "1.0", auth);
    }

    private void Update()
    {
        _chatClient.Service();
    }

    // IChatClientListener는 11개의 콜백으로 채팅 이벤트를 처리 (이게 포톤챗의 끝)

    // 1. 연결 상태 변화
    public void OnConnected()
    {
        Debug.Log("[Photon Chat] 서버에 연결됐습니다.");

        var channelOption = new ChannelCreationOptions { PublishSubscribers = true };

        int messagesFromHistory = 20;
        
        _chatClient.Subscribe(new string[] {ChannelSkku, ChannelNotice}, messagesFromHistory);
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log($"[Photon Chat] 상태 변경 ▶ {state}");
    }

    public void OnDisconnected()
    {
        Debug.Log("[Photon Chat] 서버에 연결 해제됐습니다.");
    }


    // 2. 채널 입장/퇴장
    public void OnSubscribed(string[] channels, bool[] results)
    {
        for (int i = 0; i < results.Length; i++)
        {
            Debug.Log($"[Photon Chat] 채널 {channels[i]} 구독 ({results[i]})");
        }

        foreach (var channel in _chatClient.PublicChannels)
        {
            // 여기서 내가 구동중인 채널 목록을 알 수 있다.
        }
    }

    public void OnUnsubscribed(string[] channels)
    {
        foreach (string channel in channels)
        {
            Debug.Log($"[Photon Chat] 채널 {channel} 구독 해지");
        }
    }


    // 3. 다른 유저의 온라인 상태
    public void OnUserSubscribed(string channel, string user)
    {
        Debug.Log($"[Photon Chat] 채널 {channel}에 {user} 입장!");
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        Debug.Log($"[Photon Chat] 채널 {channel}에 {user} 퇴장!");
    }
    
    // 친구/팔로우 리스트 중 특정 유저가 상태 변경 시
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    // 4. 메시지 수신
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < messages.Length; i++)
        {
            Debug.Log($"[Photon Chat] [{channelName}] {senders[i]}: {messages[i]}");

            var type = senders[i] == "민규" ? ChatType.Mine : ChatType.Other;
            
            _chats.Add(new Chat(type, senders[i], messages[i].ToString()));
            
            OnDataChanged?.Invoke();
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }
    

    // 포톤챗 내부에서 디버그 로그가 발생할 때 호출된다.
    // level에서 지정한 심각도 이상만 들어오며, 개발 단계에서 로그 확인용이다.
    public void DebugReturn(DebugLevel level, string message)
    {
        switch (level)
        {
            case DebugLevel.ERROR:
                Debug.LogError("[Photon Error] " + message);
                break;
            
            case DebugLevel.WARNING:
                Debug.LogWarning("[Photon Warning] " + message);
                break;
            
            default:
                Debug.Log("[Photon Info] " + message);
                break;
        }
    }


    public void SendChatMessage(string message)
    {
        if (_chatClient == null) return;
        if (!_chatClient.CanChat) return;

        _chatClient.PublishMessage(ChannelSkku, message);
    }
}
