using TMPro;
using UnityEngine;

public class UI_ChatMessage : MonoBehaviour
{
    public ChatType Type;

    public TextMeshProUGUI NicknameTextUI;
    public TextMeshProUGUI MessageTextUI;
    public TextMeshProUGUI DataTimeTextUI;

    public void Set(Chat chat)
    {
        if (NicknameTextUI != null) NicknameTextUI.text = chat.Nickname;
        MessageTextUI.text = chat.Message;
        if (DataTimeTextUI != null) DataTimeTextUI.text = chat.Time;
    }
}
