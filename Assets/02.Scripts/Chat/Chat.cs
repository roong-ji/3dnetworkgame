using System;
using UnityEngine;

public enum ChatType
{
    Mine,
    Other,
    System,
}

public class Chat
{
    public readonly ChatType Type;
    public readonly string Nickname;
    public readonly string Message;
    public readonly string Time;
    
    public Chat(ChatType type, string nickname, string message)
    {
        if(string.IsNullOrEmpty(nickname)) throw new Exception("Nickname cannot be null or empty");
        if(string.IsNullOrEmpty(message))  throw new Exception("Message cannot be null or empty");

        Type = type;
        Nickname = nickname;
        Message = message;
        Time = DateTime.Now.ToString("tt h:mm");
    }
}
