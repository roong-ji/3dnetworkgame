using TMPro;
using UnityEngine;

public class UI_ScoreItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nicknameTextUI;
    [SerializeField] private TextMeshProUGUI _scoreTextUI;

    public void Set(string nickname, int score)
    {
        _nicknameTextUI.SetText(nickname);
        _scoreTextUI.text = $"{score:N0}";
    }
}
