using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{

    [SerializeField] private AudioMixerSnapshot mainAudio;
    [SerializeField] private AudioMixerSnapshot pauseAudio;
    [SerializeField] private AudioMixerSnapshot muteMainAudio;
    [SerializeField] private AudioMixerSnapshot mutePauseAudio;
    [SerializeField] private Toggle muteUI;

    private static bool isMute = false;

    public void SetPlayerTimeScale(float timescale)
    {
        GameTimeManager.GetTimeTag("CustomerTime").tagTimeScale = timescale;
    }

    private void Start()
    {
        muteUI.isOn = isMute;
    }

    public void SetUnPause(float transition)
    {
        switch (isMute)
        {
            case false:
                mainAudio.TransitionTo(transition);
                break;
            case true:
                muteMainAudio.TransitionTo(transition);
                break;
        }
    }

    public void SetPause(float transition)
    {
        switch (isMute)
        {
            case false:
                pauseAudio.TransitionTo(transition);
                break;
            case true:
                mutePauseAudio.TransitionTo(transition);
                break;
        }
    }

    public void SetMute(bool isMute)
    {
        SoundMixerManager.isMute = isMute;
        switch (isMute)
        {
            case false:
                pauseAudio.TransitionTo(0.1f);
                break;
            case true:
                mutePauseAudio.TransitionTo(0.1f);
                break;
        }
    }

    public void ToggleMute()
    {
        isMute = !isMute;
        SetMute(isMute);
    }


}
