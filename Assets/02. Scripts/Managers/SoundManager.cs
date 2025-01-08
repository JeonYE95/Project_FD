using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonDontDestory<SoundManager>
{
    private AudioSource _bgmSource;                                     // 배경음
    private List<AudioSource> _sfxSource = new List<AudioSource>();     // 효과음
    private int maxSfxSource = 10;                                      // 최대 동시 재생 효과음

    private Dictionary<string, AudioClip> _bgmClip = new Dictionary<string, AudioClip>();   // BGM 클립
    private Dictionary<string, AudioClip> _sfxClip = new Dictionary<string, AudioClip>();   // SFX 클립

    [SerializeField] private float bgmVolume;
    [SerializeField] private float sfxVolume;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this) return;

        InitializeAudioSource();
        LoadAudioClips();

        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        SetBGMVolume(bgmVolume);
        SetSFXVolume(sfxVolume);
    }

    private void InitializeAudioSource()
    {
        _bgmSource = gameObject.AddComponent<AudioSource>();
        _bgmSource.loop = true;

        for (int i = 0; i < maxSfxSource; i++)
        {
            AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            _sfxSource.Add(sfxSource);
        }
    }

    private void LoadAudioClips()
    {
        // BGM 로드
        foreach (var clip in Resources.LoadAll<AudioClip>("Audio/BGM"))
        {
            _bgmClip[clip.name] = clip;
        }

        // SFX 로드
        string[] sfxFolder = new string[] { "Audio/SFX/Combine", "Audio/SFX/IngameUI" , "Audio/SFX/Battle" };

        foreach (string folder in sfxFolder)
        {
            foreach (var clip in Resources.LoadAll<AudioClip>(folder))
            {
                string key = $"{folder.Replace("Audio/SFX/", "")}/{clip.name}";
                _sfxClip[key] = clip;
            }
        }
    }

    // BGM 재생
    public void PlayBGM(string clipName, float volume = 1f)
    {
        if (_bgmClip.TryGetValue(clipName, out AudioClip clip))
        {
            if (_bgmSource.clip == clip && _bgmSource.isPlaying) return;

            _bgmSource.clip = clip;
            _bgmSource.volume = volume * bgmVolume;
            _bgmSource.Play();
        }
    }    

    // SFX 재생
    public void PlaySFX(string clipPath, float volume = 1f)
    {
        if (clipPath.Contains("Battle") && volume == 1)
        {
            volume = Defines.BattleSoundEffectVolume;
        }

        if (_sfxClip.TryGetValue(clipPath, out AudioClip clip))
        {
            AudioSource sfxSource = GetSfxSource();
            sfxSource.volume = volume * sfxVolume;
            sfxSource.PlayOneShot(clip);
        }
    }

    // 사용 가능한 SFX 불러오기
    private AudioSource GetSfxSource()
    {
        foreach (var source in _sfxSource)
        {
            if(!source.isPlaying)
            {
                return source;
            }
        }

        // 모두 사용 중일때 첫번째 사용
        return _sfxSource[0];
    }

    // BGM 정지
    public void StopBGM()
    {
        _bgmSource.Stop();
    }

    // SFX 정지
    public void StopSFX()
    {
        foreach (var source in _sfxSource)
        {
            source.Stop();
        }
    }

    // BGM 볼륨 설정
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        _bgmSource.volume = bgmVolume;

        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.Save();
    }

    // SFX 볼륨 설정
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        foreach (var source in _sfxSource)
        {
            source.volume = sfxVolume;
        }

        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    // 볼륨 가져오기
    public float GetBGMVolume() => bgmVolume;
    public float GetSFXVolume() => sfxVolume;
}