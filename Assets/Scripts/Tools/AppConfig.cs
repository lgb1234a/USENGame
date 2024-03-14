// AppConfig singleton

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppConfig
{
    public static string __REOPEN_DATA__ = "__REOPEN_GAME_DATA__";
    private static AppConfig _instance;
    public static AppConfig Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AppConfig();
            }
            return _instance;
        }
    }

    private int _cellCount = 0;

    public int MaxCellCount {
        set
        {
            _cellCount = value;
            PreferencesStorage.SaveInt("__MAX_CELL_COUNT__", _cellCount);
        }
        get 
        {
            if (_cellCount == 0) {
                _cellCount = PreferencesStorage.ReadInt("__MAX_CELL_COUNT__", 75);
            }
            return _cellCount;
        }
    }

    private int _bgmVolume = 0;

    public int BGMVolume {
        set
        {
            _bgmVolume = value;
            PreferencesStorage.SaveInt("__BGM_VOLUME__", value);
        }
        get
        {
            _bgmVolume = PreferencesStorage.ReadInt("__BGM_VOLUME__", 0);
            return _bgmVolume;
        }
    }

    private int _effectVolume = 0;
    public int EffectVolume {
        set
        {
            _effectVolume = value;
            PreferencesStorage.SaveInt("__EFFECT_VOLUME__", value);
        }
        get
        {
            _effectVolume = PreferencesStorage.ReadInt("__EFFECT_VOLUME__", 0);
            return _effectVolume;
        }
    }

    private int _themeSelectedIdx = 0;
    public int ThemeSelectedIdx {
        set
        {
            _themeSelectedIdx = value;
            ThemeResManager.Instance.SetThemeType((EThemeTypes)value);
            PreferencesStorage.SaveInt("__THEME_SELECTED__", value);
        }
        get
        {
            _themeSelectedIdx = PreferencesStorage.ReadInt("__THEME_SELECTED__", 0);
            return _themeSelectedIdx;
        }
    }

    private GameDataHandler _gameData;

    public GameDataHandler GameData {
        set
        {
            _gameData = value;
            PreferencesStorage.SaveString(__REOPEN_DATA__, JsonSerializablity.Serialize(value));
        }
        get
        {
            if (_gameData == null) {
                var gameDataStr = PreferencesStorage.ReadString(__REOPEN_DATA__, null);
                if (gameDataStr != null && gameDataStr.Length > 0)
                    _gameData = JsonSerializablity.Deserialize<GameDataHandler>(gameDataStr);
            }
            return _gameData;
        }
    }

    public void ClearGameData() {
        AppConfig.Instance.GameData = null;
    }

    public bool HasHistoryGameData()
    {
        var saveData = PreferencesStorage.ReadString(AppConfig.__REOPEN_DATA__, null);
        return saveData != null && saveData.Length > 0;
    }

    public float rotateEaseExtraTime = 0.0f;

    private int _selectedGameIndex = 2;

    public int SelectedGameIndex {
        set
        {
            _selectedGameIndex = value;
            PreferencesStorage.SaveInt("__SELECT_GAME_INDEX__", value);
        }
        get
        {
            _selectedGameIndex = PreferencesStorage.ReadInt("__SELECT_GAME_INDEX__", 2);
            return _selectedGameIndex;
        }
    }

    public IViewOperater GetSceneRootViewType() {
        if (_selectedGameIndex == 1) {
            return new HighAndLowHomeView();
        }else if (_selectedGameIndex == 2) {
            return new BingoHomeView();
        }
        return null;
    }
}