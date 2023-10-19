

using System;
using UnityEngine;
using Spine.Unity;
using UnityEditor;

public class ThemeResManager
{
    private static ThemeResManager m_instance;

    public static ThemeResManager Instance
    {
        get
        {
            if (m_instance == null) {
                m_instance = new ThemeResManager();
            }
            return m_instance;
        }
        set
        {
            m_instance = value;
        }
    }

    private IThemeRes m_currentResConfig;
    public void SetResConfig(IThemeRes config) {
        m_currentResConfig = config;
    }

    private EThemeTypes m_currentType = EThemeTypes.Default;
    public void SetThemeType(EThemeTypes type) {
        if (m_currentType == type) {
            return;
        }

        m_currentType = type;
        if (type == EThemeTypes.Default) {
            SetResConfig(new DefaultThemeResConfig());
        }

        if (type == EThemeTypes.Christmas) {
            SetResConfig(new ChristmasThemeResConfig());
        }
        ViewManager.Instance.SendChangeThemeTypeEvent();
        AudioManager.Instance.SendChangeThemeTypeEvent();
    }

    private IThemeRes GetCurrentResConfig() {
        if (m_currentResConfig == null) {
            SetResConfig(new DefaultThemeResConfig());
        }
        return m_currentResConfig;
    }

    public Sprite GetThemeBgTexture() {
        var sprite = Resources.Load<Sprite>(GetCurrentResConfig().GetThemeBgTexturePath());
        return sprite;
    }

    public Sprite GetThemeHomeBgDecorateTexture() {
        var sprite = Resources.Load<Sprite>(GetCurrentResConfig().GetThemeBgDecorateTexturePath());
        return sprite;
    }

    public Sprite GetThemePlayViewDecorateTexture() {
        var sprite = Resources.Load<Sprite>(GetCurrentResConfig().GetThemeGameViewTopDecorateTexturePaht());
        return sprite;
    }

    public Sprite GetCellNumberBgTexture() {
        var sprite = Resources.Load<Sprite>(GetCurrentResConfig().GetNumberCellBgDefaultTexturePath());
        return sprite;
    }

    public Sprite GetCellNumberCheckBgTexture() {
        var sprite = Resources.Load<Sprite>(GetCurrentResConfig().GetNumberCellBgHighlightTexturePath());
        return sprite;
    }

    public Sprite GetRotateNumberTexture(int index) {
        var spritePath = String.Format(GetCurrentResConfig().GetNumberTexturePath(), index);
        var sprite = Resources.Load<Sprite>(spritePath);
        return sprite;
    }

    public Sprite GetBingoDefaultNumberTexture(int index) {
        var spritePath = String.Format(GetCurrentResConfig().GetBingoDefaultNumberTexturePath(), index);
        var sprite = Resources.Load<Sprite>(spritePath);
        return sprite;
    }

    public Sprite GetBingoSelectedNumberTexture(int index) {
        var spritePath = String.Format(GetCurrentResConfig().GetBingoSelectedNumberTexturePath(), index);
        var sprite = Resources.Load<Sprite>(spritePath);
        return sprite;
    }

    public GameObject InstantiateHomeSpineGameObject(Transform parent) {
        var prefabPath = GetCurrentResConfig().GetHomeSpinePrefabPath();
        var gameObject = Resources.Load<GameObject>(prefabPath);
        return GameObject.Instantiate(gameObject, parent, false);
    }

    public GameObject InstantiateBingoSpineGameObject(Transform parent) {
        var prefabPath = GetCurrentResConfig().GetBingoSpinePrefabPath();
        var gameObject = Resources.Load<GameObject>(prefabPath);
        return GameObject.Instantiate(gameObject, parent, false);
    }

    public GameObject InstantiateRotateBgSpineGameObject(Transform parent) {
        var prefabPath = GetCurrentResConfig().GetRotateBgSpinePrefabPath();
        var gameObject = Resources.Load<GameObject>(prefabPath);
        return GameObject.Instantiate(gameObject, parent, false);
    }

    public AudioClip GetBgmAudioClip() {
        var audioPath = GetCurrentResConfig().GetNormalBgmPath();
        var audioClip = Resources.Load<AudioClip>(audioPath);
        return audioClip;
    }

    public AudioClip GetWillReachBgmAudioClip() {
        var audioPath = GetCurrentResConfig().GetWillReachBgmPath();
        var audioClip = Resources.Load<AudioClip>(audioPath);
        return audioClip;
    }

    public AudioClip GetNumberCheckDownAudioClip() {
        var audioPath = GetCurrentResConfig().GetNumberCheckDownAudioPath();
        var audioClip = Resources.Load<AudioClip>(audioPath);
        return audioClip;
    }

    public AudioClip GetNumberRotateAudioClip() {
        var audioPath = GetCurrentResConfig().GetNumberRotateAudioPath();
        var audioClip = Resources.Load<AudioClip>(audioPath);
        return audioClip;
    }

    public AudioClip GetBingoAudioClip() {
        var audioPath = GetCurrentResConfig().GetBingoAudioPath();
        var audioClip = Resources.Load<AudioClip>(audioPath);
        return audioClip;
    }

     public AudioClip GetReachAudioClip() {
        var audioPath = GetCurrentResConfig().GetReachAudioPath();
        var audioClip = Resources.Load<AudioClip>(audioPath);
        return audioClip;
    }
}