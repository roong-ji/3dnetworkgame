using TMPro;
using UnityEngine;

public class UI_ScoreItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nicknameTextUI;
    [SerializeField] private TextMeshProUGUI _scoreTextUI;
    [SerializeField] private CanvasGroup _canvasGroup;
    
    public void Set(string nickname, int score)
    {
        _canvasGroup.alpha = 1;
        _nicknameTextUI.SetText(nickname);
        _scoreTextUI.text = $"{score:N0}";
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0;
    }
}
