

using System;
using UnityEngine;

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
        Debug.LogWarning("------------ " + spritePath);
        var sprite = Resources.Load<Sprite>(spritePath);
        Debug.LogWarning(sprite);
        return sprite;
    }
}