using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    public static ScoreManager Instance;
    
    private int _score;
    
    private Dictionary<int, ScoreData> _scores = new();
    public IReadOnlyDictionary<int, ScoreData> Scores => _scores;
    
    public event Action OnDataChanged;
    
    private void Awake()
    {
        Instance = this;
    }

    public void AddScore(int score)
    {
        _score += score;
        Refresh();
    }

    private void Refresh()
    {
        var hashtable = new Hashtable();
        hashtable.Add("score", _score);
        
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }

    public override void OnJoinedRoom()
    {
        Refresh();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!changedProps.ContainsKey("score")) return;
        var scoreData = new ScoreData()
        {
            Nickname = targetPlayer.NickName,
            Score = (int)changedProps["score"]
        };
        
        _scores[targetPlayer.ActorNumber] = scoreData;

        OnDataChanged?.Invoke();
    }
}
