using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;

public class PlayerVoiceAbility : PlayerAbility
{
    public GameObject SpeakingIcon;
    [SerializeField] private PhotonVoiceView _voiceView;
    private Recorder _recorder;

    private void Start()
    {
        _recorder = FindAnyObjectByType<Recorder>();
    }
    
    private void Update()
    {
        var isSpeaking = false;

        if (_owner.PhotonView.IsMine)
        {
            isSpeaking = _recorder.IsCurrentlyTransmitting;
        }
        else
        {
            isSpeaking = _voiceView.IsSpeaking;
        }

        isSpeaking = _voiceView.IsSpeaking;
        SpeakingIcon.SetActive(isSpeaking);

        if (Input.GetKeyDown(KeyCode.M))
        {
            _recorder.TransmitEnabled = !_recorder.TransmitEnabled;
        }
    }
}
