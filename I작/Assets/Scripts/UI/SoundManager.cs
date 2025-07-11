using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SOUND_TYPE
{
    BGM, SFX
}

public enum BGM
{
    Title, InGame, Boss_Intro, Boss_Win, Boss_Fight
}

public enum SFX
{
    Bomb,
    ChestOpen,
    DoorOpen,
    PickKey,
    PassiveItem,
    Tear,
    Damage,
    Dead,
    Boss_Fall,
    Boss_Tear,
    Boss_Die,
    Monster_Fly,
    Monster_Jombie,
    Monster_Die,
    TearFire
}

[Serializable]
public class BGMClip
{
    public BGM type;
    public AudioClip clip;
}

[Serializable]
public class SFXClip
{
    public SFX type;
    public AudioClip clip;
}


public class SoundManager : MonoBehaviour
{
    [Header("오디오 믹서")]
    public AudioMixer audioMixer;
    public string bgmParameter = "BGM";
    public string sfxParameter = "SFX";

    [Header("오디오 소스")]
    public AudioSource bgm;
    public AudioSource sfx;

    [Header("오디오 클립")]
    public List<BGMClip> bgm_list;
    public List<SFXClip> sfx_list;

    private Dictionary<BGM, AudioClip> bgm_dict;
    private Dictionary<SFX, AudioClip> sfx_dict; 

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            bgm_dict = new Dictionary<BGM, AudioClip>();
            sfx_dict = new Dictionary<SFX, AudioClip>();

            foreach (var bgm in bgm_list)
            {
                bgm_dict[bgm.type] = bgm.clip;
            }

            foreach (var sfx in sfx_list)
            {
                sfx_dict[sfx.type] = sfx.clip;
            }
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBGM(BGM bgm_type)
    {
        if (bgm_dict.TryGetValue(bgm_type, out var clip))
        {
            if (bgm.clip == clip)
            {
                return;
            }
            bgm.clip = clip;
            bgm.loop = true;
            bgm.Play();
        }
    }

    public void PlaySFX(SFX sfx_type)
    {
        if (sfx_dict.TryGetValue(sfx_type, out var clip))
        {
            sfx.PlayOneShot(clip);
        }
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat(bgmParameter, Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat(sfxParameter, Mathf.Log10(volume) * 20);
    }

    public void MuteBGM(bool mute)
    {
        audioMixer.SetFloat(bgmParameter, mute ? -80.0f : 0f);
    }

    public void MuteSFX(bool mute)
    {
        audioMixer.SetFloat(sfxParameter, mute ? -80.0f : 0f);
    }

}