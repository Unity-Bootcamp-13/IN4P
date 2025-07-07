using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum SOUND_TYPE
{
    BGM, SFX
}

public enum BGM
{
    Title, InGame, Boss
}
public enum SFX
{
    Bullet
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
    [Header("ȭ�� ��ġ")]
    public Image volumeImage;
    public Button volumeButton;
    public Button closeButton;

    public void Start()
    {
        volumeImage.gameObject.SetActive(false);

        volumeButton.onClick.AddListener(VolumeOnClick);
        closeButton.onClick.AddListener(CloseOnClick);
    }

    void VolumeOnClick()
    {
        volumeImage.gameObject.SetActive(true);
    }

    void CloseOnClick()
    {
        volumeImage.gameObject.SetActive(false);
    }

    [Header("����� �ͼ�")]
    public AudioMixer audioMixer;
    public string bgmParameter = "BGM";
    public string sfxParameter = "SFX";

    [Header("����� �ҽ�")]
    public AudioSource bgm;
    public AudioSource sfx;

    [Header("����� Ŭ��")]
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
            DontDestroyOnLoad(gameObject);

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