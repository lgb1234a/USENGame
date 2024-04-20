using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class GameSettingsView : MonoBehaviour
{
    Slider m_backgroundToggleSlider;
    ToggleGroup m_backgroundToggleGroup;
    List<Toggle> m_backgroundToggles = new List<Toggle>();
    Slider m_bgmSlider;
    ToggleGroup m_bgmToggleGroup;
    List<Toggle> m_bgmToggles = new List<Toggle>();
    Slider m_BGMVolumeSlider;
    Slider m_EffectVolumeSlider;
    Text m_BGMVolumeText;
    Text m_EffectVolumeText;
    bool m_isPresettingEffectVolume;
    Slider m_winnerSettingSlider;
    ToggleGroup m_winnerToggleGroup;
    List<Toggle> m_winnerToggles = new List<Toggle>();
    Button m_gameSettingsBtn1;
    Button m_gameSettingsBtn2;
    Button m_gameSettingsBtn3;
    Button m_gameSettingsBtn4;
    Button m_gameSettingsBtn5;
    GameSelector m_gameSelector;
    bool m_isInited;

    void Awake() {
        m_gameSelector = transform.parent.Find("Home").GetComponent<GameSelector>();
    }

    public void Start() {
        m_backgroundToggleGroup = transform.Find("Panel/ViewPort/CenterPanel/BackgroundSettings/Settings").GetComponent<ToggleGroup>();
        m_backgroundToggleSlider = transform.Find("Panel/ViewPort/CenterPanel/BackgroundSettings/Settings").GetComponent<Slider>();
        m_backgroundToggleSlider.onValueChanged.AddListener(OnBackgroundSliderChanged);
        m_backgroundToggles.Clear();
        for (int i = 1; i <= 2; i++) {
            var toggle = transform.Find("Panel/ViewPort/CenterPanel/BackgroundSettings/Settings/Backgound"+i).GetComponent<Toggle>();
            m_backgroundToggles.Add(toggle);
        }
        m_backgroundToggleSlider.value = AppConfig.Instance.ThemeSelectedIdx;


        m_bgmToggleGroup = transform.Find("Panel/ViewPort/CenterPanel/BgmSettings/Settings").GetComponent<ToggleGroup>();
        m_bgmSlider = transform.Find("Panel/ViewPort/CenterPanel/BgmSettings/Settings").GetComponent<Slider>();
        m_bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged);
        m_bgmToggles.Clear();
        for (int i = 1; i <= 2; i++) {
            var toggle = transform.Find("Panel/ViewPort/CenterPanel/BgmSettings/Settings/BGM"+i).GetComponent<Toggle>();
            m_bgmToggles.Add(toggle);
        }
        m_bgmSlider.value = AppConfig.Instance.BgmSelectedIdx;


        m_BGMVolumeSlider = transform.Find("Panel/ViewPort/CenterPanel/BGMVolumeSettings/Slider").GetComponent<Slider>();
        m_BGMVolumeText = transform.Find("Panel/ViewPort/CenterPanel/BGMVolumeSettings/VolumeText").GetComponent<Text>();
        m_BGMVolumeSlider.onValueChanged.AddListener(OnBgmVolumeSliderValueChanged);
        m_BGMVolumeSlider.value = AppConfig.Instance.BGMVolume;

        m_EffectVolumeSlider = transform.Find("Panel/ViewPort/CenterPanel/VolumeEffectSettings/Slider").GetComponent<Slider>();
        m_EffectVolumeText = transform.Find("Panel/ViewPort/CenterPanel/VolumeEffectSettings/VolumeText").GetComponent<Text>();
        m_EffectVolumeSlider.onValueChanged.AddListener(OnVolumeEffectValueChanged);
        m_isPresettingEffectVolume = true;
        m_EffectVolumeSlider.value = AppConfig.Instance.EffectVolume;
        m_isPresettingEffectVolume = false;

        m_winnerToggleGroup = transform.Find("Panel/ViewPort/CenterPanel/WinSettings/Settings").GetComponent<ToggleGroup>();
        m_winnerSettingSlider = transform.Find("Panel/ViewPort/CenterPanel/WinSettings/Settings").GetComponent<Slider>();
        m_winnerSettingSlider.onValueChanged.AddListener(OnWinnerSliderValueChanged);
        m_winnerToggles.Clear();
        for (int i = 1; i <= 2; i++) {
            var toggle = transform.Find("Panel/ViewPort/CenterPanel/WinSettings/Settings/Backgound"+i).GetComponent<Toggle>();
            m_winnerToggles.Add(toggle);
        }
        m_winnerSettingSlider.value = AppConfig.Instance.WinnerSelectedIdx;

        m_gameSettingsBtn1 = transform.Find("Panel/ViewPort/CenterPanel/GamesSettings/Game1").GetComponent<Button>();
        m_gameSettingsBtn1.onClick.AddListener(OnGameSettingsBtnClick1);
        m_gameSettingsBtn2 = transform.Find("Panel/ViewPort/CenterPanel/GamesSettings/Game2").GetComponent<Button>();
        m_gameSettingsBtn2.onClick.AddListener(OnGameSettingsBtnClick2);
        m_gameSettingsBtn3 = transform.Find("Panel/ViewPort/CenterPanel/GamesSettings/Game3").GetComponent<Button>();
        m_gameSettingsBtn3.onClick.AddListener(OnGameSettingsBtnClick3);
        m_gameSettingsBtn4 = transform.Find("Panel/ViewPort/CenterPanel/GamesSettings/Game4").GetComponent<Button>();
        m_gameSettingsBtn4.onClick.AddListener(OnGameSettingsBtnClick4);
        m_gameSettingsBtn5 = transform.Find("Panel/ViewPort/CenterPanel/GamesSettings/Game5").GetComponent<Button>();
        m_gameSettingsBtn5.onClick.AddListener(OnGameSettingsBtnClick5);

        EventSystem.current.SetSelectedGameObject(m_backgroundToggleSlider.gameObject);
        m_isInited = true;
    }

    void OnEnable() {
        if (m_isInited) {
            EventSystem.current.SetSelectedGameObject(m_backgroundToggleSlider.gameObject);
        }
    }

    public void Hide() {
        gameObject.SetActive(false);
        m_gameSelector.Show();
    }

    public void Update() {
        if (Input.GetButtonDown("Cancel")) {
            Hide();
        }
    }

    private void OnBackgroundSliderChanged(float value) {
        var intValue = Mathf.FloorToInt(value);
        AppConfig.Instance.ThemeSelectedIdx = intValue;
        m_backgroundToggles[AppConfig.Instance.ThemeSelectedIdx].isOn = true;
    }

    private void OnBgmSliderChanged(float value) {
        var intValue = Mathf.FloorToInt(value);
        AppConfig.Instance.BgmSelectedIdx = intValue;
        m_bgmToggles[AppConfig.Instance.BgmSelectedIdx].isOn = true;
    }

    public void OnBgmVolumeSliderValueChanged(float value) {
        m_BGMVolumeText.text = Mathf.FloorToInt(value).ToString("+#;-#;0");
        AppConfig.Instance.BGMVolume = Mathf.FloorToInt(value);
        AudioManager.Instance.SetBgmVolume((int)value);
    }

    public void OnWinnerSliderValueChanged(float value) {
        var intValue = Mathf.FloorToInt(value);
        AppConfig.Instance.WinnerSelectedIdx = intValue;
        m_winnerToggles[AppConfig.Instance.WinnerSelectedIdx].isOn = true;
    }

    public void OnVolumeEffectValueChanged(float value) {
        m_EffectVolumeText.text = Mathf.FloorToInt(value).ToString("+#;-#;0");
        AppConfig.Instance.EffectVolume = Mathf.FloorToInt(value);
        AudioManager.Instance.SetEffectVolume((int)value);
        if (m_isPresettingEffectVolume) return;
        AudioManager.Instance.PlayNumberRotateEffectWithoutLoop();
        AudioManager.Instance.PlayNumberCheckEffect();
    }

    void OnGameSettingsBtnClick1() {
        AppConfig.Instance.HomeSceneRootViewType = RootViewType.BingoSettings;
        USENSceneManager.Instance.LoadScene("Home");
    }

    void OnGameSettingsBtnClick2() {
        AppConfig.Instance.HomeSceneRootViewType = RootViewType.HighAndLowSettings;
        USENSceneManager.Instance.LoadScene("Home");
    }

    void OnGameSettingsBtnClick3() {
        AppConfig.Instance.HomeSceneRootViewType = RootViewType.RouletteSettings;
        USENSceneManager.Instance.LoadScene("Home");
    }

    void OnGameSettingsBtnClick4() {
        USENSceneManager.Instance.LoadScene("Home");
    }

    void OnGameSettingsBtnClick5() {
        USENSceneManager.Instance.LoadScene("Home");
    }
}
