using Photon.Pun;
using UnityEngine;

public class PlayerWeaponAbility : PlayerAbility
{
    private void Start()
    {
        if (!TryGetComponent(out PhotonView photonView) || !photonView.IsMine) return;
        
        ScoreManager.Instance.OnDataChanged += Refresh;
        Refresh();
    }

    private void OnDestroy()
    {
        if (!TryGetComponent(out PhotonView photonView) || !photonView.IsMine) return;
        ScoreManager.Instance.OnDataChanged -= Refresh;
    }

    private void Refresh()
    {
        var score = ScoreManager.Instance.Score;
        var factor = 1 + score / 2000f;
        transform.localScale = new Vector3(factor, factor, factor);
    }
}
