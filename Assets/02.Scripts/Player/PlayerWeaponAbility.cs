using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerWeaponAbility : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        if (!photonView.Owner.CustomProperties.TryGetValue("score", out var scoreObj)) return;
        UpdateScale((int)scoreObj);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!changedProps.ContainsKey("score") || targetPlayer.ActorNumber != photonView.OwnerActorNr) return;
        UpdateScale((int)changedProps["score"]);
    }

    private void UpdateScale(int score)
    {
        var factor = 1 + score / 2000f;
        transform.localScale = new Vector3(factor, factor, factor);
    }
}
