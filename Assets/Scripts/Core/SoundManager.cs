using Unity.VisualScripting;
using UnityEngine;

public enum SFX
{
    DisAbleSound,
    BackSound,
    OnClickButtonSound,
    
    PlaceCrewSound,
    TranqGunSound,
    TrapSound,
    ButterflySound,

    PlaceTileSound,
    DrawTileSound,
    DestroySound,

    GameClearSound,
    GameLoseSound,

    OnClickButtonInTitle,
    OnClickStartbuttonInTitle,
}

public enum BGM
{
    InGameSoundOneTime,
    InGameSoundLoop,
    TitleSceneBackGround,
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [Header("SFX")]
    public AudioClip[] SFXaudioClips;

    [Header("BGM")]
    public AudioClip[] BGMaudioClips;

    public AudioSource SFXaudioSource;
    public AudioSource BGMaudioSource;
    public static SoundManager Instance => GetInstance();
    private static SoundManager _Instance; 

    

    private static SoundManager GetInstance()
    {
        if(_Instance == null)
        {
            _Instance = GameObject.FindWithTag(TagIds.SoundManagerTag).GetComponent<SoundManager>();
        }

        return _Instance;
    }

    public void Awake()
    {
        if(_Instance != null)
        {
            Destroy(gameObject);
        }else
        {
            DontDestroyOnLoad(this);
        }
    }

    public void ChangeSFXSound(float value)
    {
        SFXaudioSource.volume = value;
    }

    public void ChangeBGMSound(float value)
    {
        BGMaudioSource.volume = value;
    }

    public void PlayOneShot(SFX id)
    {
        SFXaudioSource.PlayOneShot(SFXaudioClips[(int)id]);
    }

    public void PlayBackGround(BGM bgm)
    {
        BGMaudioSource.clip = BGMaudioClips[(int)bgm];
        BGMaudioSource.loop = true;
        BGMaudioSource.Play();
    }

    public float GetSFXValue()
    {
        return SFXaudioSource.volume;
    }

    public float GetBGMValue()
    {
        return BGMaudioSource.volume;
    }
}
