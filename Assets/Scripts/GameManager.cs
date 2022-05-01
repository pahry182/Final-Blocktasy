using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Data data = new Data();
    public Audio audio = new Audio();
    public Setting setting = new Setting();
    public GameObject[] playerCharacters, VFXs;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
            PlayerPrefs.DeleteAll();
            SetupAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject GetVFX(string name)
    {
        foreach (var item in VFXs)
        {
            if (item.name == name) return item;
        }

        return null;
    }

    public void PlaySfx(string name)
    {
        Sound sfx = Array.Find(audio.soundEffects, sound => sound.name == name);
        if (sfx == null)
        {
            print("Audio " + name + " not found!!");
            return;
        }

        sfx.audioSource.Play();
    }

    public void PlayBgm(string name)
    {
        Sound bgm = Array.Find(audio.backgroundMusics, sound => sound.name == name);
        if (bgm.name == name && bgm.audioSource.isPlaying) return;
        if (bgm == null)
        {
            print("Audio " + name + " not found!!");
            return;
        }

        for (int i = 0; i < audio.backgroundMusics.Length; i++)
        {
            audio.backgroundMusics[i].audioSource.Stop();
        }

        bgm.audioSource.Play();
    }

    public void StopBgm()
    {
        for (int i = 0; i < audio.backgroundMusics.Length; i++)
        {
            audio.backgroundMusics[i].audioSource.Stop();
        }
    }

    public void UpdateVolume()
    {

        for (int i = 0; i < audio.activeSfx.Count; i++)
        {
            audio.activeSfx[i].volume = setting.BgmVolume;
        }

        for (int i = 0; i < audio.activeBgm.Count; i++)
        {
            audio.activeBgm[i].volume = setting.SfxVolume;
        }
    }

    private void SetupAudio()
    {
        for (int i = 0; i < audio.soundEffects.Length; i++)
        {
            audio.soundEffects[i].audioSource = gameObject.AddComponent<AudioSource>();
            audio.soundEffects[i].audioSource.clip = audio.soundEffects[i].clip;
            audio.soundEffects[i].audioSource.volume = audio.soundEffects[i].volume;
            audio.soundEffects[i].audioSource.pitch = audio.soundEffects[i].pitch;
            audio.soundEffects[i].audioSource.loop = audio.soundEffects[i].loop;
            audio.activeSfx.Add(audio.soundEffects[i].audioSource);
        }

        for (int i = 0; i < audio.backgroundMusics.Length; i++)
        {
            audio.backgroundMusics[i].audioSource = gameObject.AddComponent<AudioSource>();
            audio.backgroundMusics[i].audioSource.clip = audio.backgroundMusics[i].clip;
            audio.backgroundMusics[i].audioSource.volume = audio.backgroundMusics[i].volume;
            audio.backgroundMusics[i].audioSource.pitch = audio.backgroundMusics[i].pitch;
            audio.backgroundMusics[i].audioSource.loop = audio.backgroundMusics[i].loop;
            audio.activeBgm.Add(audio.backgroundMusics[i].audioSource);
        }
    }

    public void ToggleMusic(bool value)
    {
        for (int i = 0; i < audio.backgroundMusics.Length; i++)
        {
            audio.backgroundMusics[i].audioSource.mute = value;
        }
    }

    public void ToggleEffects(bool value)
    {
        for (int i = 0; i < audio.soundEffects.Length; i++)
        {
            audio.soundEffects[i].audioSource.mute = value;
        }
    }

    [Serializable]
    public class Audio
    {
        public Sound[] soundEffects;
        public Sound[] backgroundMusics;

        [HideInInspector] public List<AudioSource> activeSfx = new List<AudioSource>();
        [HideInInspector] public List<AudioSource> activeBgm = new List<AudioSource>();
    }

    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        public bool loop = true;
        [HideInInspector] public AudioSource audioSource;
    }

    public class Setting
    {
        public float SfxVolume
        {
            get { return PlayerPrefs.GetFloat("sfxVolume"); }
            set { PlayerPrefs.SetFloat("sfxVolume", value); }
        }

        public float BgmVolume
        {
            get { return PlayerPrefs.GetFloat("bgmVolume"); }
            set { PlayerPrefs.SetFloat("bgmVolume", value); }
        }

        public float SfxStatus
        {
            get { return PlayerPrefs.GetInt("sfxStatus"); }
            set { PlayerPrefs.SetInt("sfxStatus", (int)value); }
        }

        public float BgmStatus
        {
            get { return PlayerPrefs.GetInt("bgmStatus"); }
            set { PlayerPrefs.SetInt("bgmStatus", (int)value); }
        }
    }

    public class Data
    {
        public Vector2 GetStageProgress()
        {
            return new Vector2(PlayerPrefs.GetFloat("EXPLORE_X"), PlayerPrefs.GetFloat("EXPLORE_Y"));
        }

        public void SetStageProgress(Vector2 pos)
        {
            PlayerPrefs.SetFloat("EXPLORE_X", pos.x);
            PlayerPrefs.SetFloat("EXPLORE_Y", pos.y);
        }

        public void SetCharacterProgress(UnitBase _ub)
        {
            PlayerPrefs.SetFloat(_ub.unitName + "_LEVEL", _ub.unitLevel);
            PlayerPrefs.SetFloat(_ub.unitName + "_CURRENT_XP", _ub.currentXp);
            PlayerPrefs.SetFloat(_ub.unitName + "_CURRENT_PERCENT_HP", _ub.currentHp / _ub.maxHp);
            PlayerPrefs.SetFloat(_ub.unitName + "_CURRENT_PERCENT_MP", _ub.currentMp / _ub.maxMp);
        }

        public float[] GetCharacterProgress(UnitBase _ub)
        {
            float[] data = new float[4];
            data[0] = PlayerPrefs.GetFloat(_ub.unitName + "_LEVEL");
            data[1] = PlayerPrefs.GetFloat(_ub.unitName + "_CURRENT_XP");
            data[2] = PlayerPrefs.GetFloat(_ub.unitName + "_CURRENT_PERCENT_HP");
            data[3] = PlayerPrefs.GetFloat(_ub.unitName + "_CURRENT_PERCENT_MP");
            return data;
        }
    }
}
