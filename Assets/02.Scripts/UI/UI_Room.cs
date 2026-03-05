using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class UI_Room : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<UI_RoomItem> _roomItems;
    private Dictionary<string, RoomInfo> _rooms = new();
    
    private void Awake()
    {
        _roomItems = GetComponentsInChildren<UI_RoomItem>().ToList();
        
        HideAllRoomUI();
    }

    private void HideAllRoomUI()
    {
        foreach (var item in _roomItems)
        {
            item.gameObject.SetActive(false);
        }
    }
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        HideAllRoomUI();

        foreach (var room in roomList)
        {
            if (room.RemovedFromList)
            {
                _rooms.Remove(room.Name);
            }
            else
            {
                _rooms[room.Name] = room;
            }
        }

        var rooms = _rooms.Values.ToList();
        for (var i = 0; i < _rooms.Count; ++i)
        {
            _roomItems[i].Init(rooms[i]);
            _roomItems[i].gameObject.SetActive(true);
        }
    }
}
