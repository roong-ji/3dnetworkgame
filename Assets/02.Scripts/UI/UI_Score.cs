using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Score : MonoBehaviour
{
    [SerializeField] private List<UI_ScoreItem> _items;

    private void Start()
    {
        _items = GetComponentsInChildren<UI_ScoreItem>().ToList();
        ScoreManager.Instance.OnDataChanged += Refresh;
        Refresh();
    }

    private void OnDestroy()
    {
        ScoreManager.Instance.OnDataChanged -= Refresh;
    }

    private void Refresh()
    {
        var scores = ScoreManager.Instance.Scores;

        var scoresData = scores.Values.ToList();
        scoresData.Sort();
        
        for (int i = 0; i < _items.Count; ++i)
        {
            if (i > 2) return;
            var data = scoresData[i];
            _items[i].Set(data.Nickname, data.Score);
        }
    }
}
