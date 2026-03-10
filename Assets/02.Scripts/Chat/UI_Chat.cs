using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class UI_Chat : MonoBehaviour
{
    [Header("프리팹")] 
    public UI_ChatMessage MinePrefab;
    public UI_ChatMessage OtherPrefab;
    public UI_ChatMessage SystemPrefab;
    
    public TMP_InputField InputField;
    public TextMeshProUGUI MemberCountText;
    
    private List<UI_ChatMessage> _chatMessageUI = new();

    public Transform ContentTransform;
    public ScrollRect ScrollRect;
    
    private void Start()
    {
        ChatManager.Instance.OnDataChanged += Refresh;
        ChatManager.Instance.OnUserCountChanged += UpdateUserCountUI;
        InputField.onSubmit.AddListener(OnSendMessage);
    }

    private void OnDestroy()
    {
        ChatManager.Instance.OnDataChanged -= Refresh;
        ChatManager.Instance.OnUserCountChanged -= UpdateUserCountUI;
        InputField.onSubmit.AddListener(OnSendMessage);
    }

    public void Refresh()
    {
        var chats = ChatManager.Instance.Chats;

        // ui를 다 지운다.
        foreach (var ui in _chatMessageUI)
        {
            Destroy(ui.gameObject);
        }
        _chatMessageUI.Clear();
        
        foreach (var chat in chats)
        {
            UI_ChatMessage chatMessage = null;
            switch (chat.Type)
            {
                case ChatType.Mine:
                    chatMessage = Instantiate(MinePrefab, ContentTransform);
                    break;
                case ChatType.Other:
                    chatMessage = Instantiate(OtherPrefab, ContentTransform);
                    break;
                case ChatType.System:
                    chatMessage = Instantiate(SystemPrefab, ContentTransform);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            chatMessage.Set(chat);
            _chatMessageUI.Add(chatMessage);
        }
        
        ScrollRect.verticalNormalizedPosition = 0f;
    }

    public void OnSendMessage(string text)
    {
        if (string.IsNullOrEmpty(text)) return;
        ChatManager.Instance.SendChatMessage(text);
        InputField.text = string.Empty;
    }
    
    public void OnClickSendButton()
    {
        var text = InputField.text;
        OnSendMessage(text);
    }
    
    private void UpdateUserCountUI(int count)
    {
        MemberCountText.text = count.ToString();
    }
}
