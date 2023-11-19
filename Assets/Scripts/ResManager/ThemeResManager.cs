

using System;
using UnityEngine;
using Spine.Unity;
using UnityEditor;
using System.Collections.Generic;
using System.Threading.Tasks;

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

    public EThemeTypes GetCurrentThemeType() {
        return m_currentType;
    }

    public IThemeRes GetCurrentResConfig() {
        if (m_currentResConfig == null) {
            SetResConfig(new DefaultThemeResConfig());
        }
        return m_currentResConfig;
    }

    public Task<Sprite> GetThemeBgTexture() {
        var sprite = ResourceLoader.LoadResourcesAsync<Sprite>(GetCurrentResConfig().GetThemeBgTexturePath());
        return sprite;
    }

    public Task<Sprite> GetThemeHomeBgDecorateTexture() {
        var sprite = ResourceLoader.LoadResourcesAsync<Sprite>(GetCurrentResConfig().GetThemeBgDecorateTexturePath());
        return sprite;
    }

    public Task<Sprite> GetThemePlayViewDecorateTexture() {
        var sprite = ResourceLoader.LoadResourcesAsync<Sprite>(GetCurrentResConfig().GetThemeGameViewTopDecorateTexturePaht());
        return sprite;
    }

    public Task<Sprite> GetCellNumberBgTexture() {
        var sprite = ResourceLoader.LoadResourcesAsync<Sprite>(GetCurrentResConfig().GetNumberCellBgDefaultTexturePath());
        return sprite;
    }

    public Task<Sprite> GetCellNumberCheckBgTexture() {
        var sprite = ResourceLoader.LoadResourcesAsync<Sprite>(GetCurrentResConfig().GetNumberCellBgHighlightTexturePath());
        return sprite;
    }

    public Task<Sprite> GetRotateNumberTexture(int index) {
        var spritePath = String.Format(GetCurrentResConfig().GetNumberTexturePath(), index);
        var sprite = ResourceLoader.LoadResourcesAsync<Sprite>(spritePath);
        return sprite;
    }

    public async Task<GameObject> InstantiateHomeSpineGameObject(Transform parent) {
        var prefabPath = GetCurrentResConfig().GetHomeSpinePrefabPath();
        var gameObject = await ResourceLoader.LoadResourcesAsync<GameObject>(prefabPath);
        return GameObject.Instantiate(gameObject, parent, false);
    }

    public async Task<GameObject> InstantiateBingoSpineGameObject(Transform parent) {
        var prefabPath = GetCurrentResConfig().GetBingoSpinePrefabPath();
        var gameObject = await ResourceLoader.LoadResourcesAsync<GameObject>(prefabPath);
        return GameObject.Instantiate(gameObject, parent, false);
    }

    public async Task<GameObject> InstantiateRotateBgSpineGameObject(Transform parent) {
        var prefabPath = GetCurrentResConfig().GetRotateBgSpinePrefabPath();
        var gameObject = await ResourceLoader.LoadResourcesAsync<GameObject>(prefabPath);
        return GameObject.Instantiate(gameObject, parent, false);
    }

    public async Task<AudioClip> GetBgmAudioClip() {
        var audioPath = GetCurrentResConfig().GetNormalBgmPath();
        var audioClip = await ResourceLoader.LoadResourcesAsync<AudioClip>(audioPath);
        return audioClip;
    }

    public async Task<AudioClip> GetWillReachBgmAudioClip() {
        var audioPath = GetCurrentResConfig().GetWillReachBgmPath();
        var audioClip = await ResourceLoader.LoadResourcesAsync<AudioClip>(audioPath);
        return audioClip;
    }

    public async Task<AudioClip> GetNumberCheckDownAudioClip() {
        var audioPath = GetCurrentResConfig().GetNumberCheckDownAudioPath();
        var audioClip = await ResourceLoader.LoadResourcesAsync<AudioClip>(audioPath);
        return audioClip;
    }

    public async Task<AudioClip> GetNumberRotateAudioClip() {
        var audioPath = GetCurrentResConfig().GetNumberRotateAudioPath();
        var audioClip = await ResourceLoader.LoadResourcesAsync<AudioClip>(audioPath);
        return audioClip;
    }

    public async Task<AudioClip> GetBingoAudioClip() {
        var audioPath = GetCurrentResConfig().GetBingoAudioPath();
        var audioClip = await ResourceLoader.LoadResourcesAsync<AudioClip>(audioPath);
        return audioClip;
    }

     public async Task<AudioClip> GetReachAudioClip() {
        var audioPath = GetCurrentResConfig().GetReachAudioPath();
        var audioClip = await ResourceLoader.LoadResourcesAsync<AudioClip>(audioPath);
        return audioClip;
    }

    public Color GetNumberCheckedTextColor() {
        var color = GetCurrentResConfig().GetNumberCheckedTextColor();
        return color;
    }
}