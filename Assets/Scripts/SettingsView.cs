using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SettingsView : IViewOperater
{
    string m_prefabPath = "SettingsPanel";
    GameObject m_viewGameObject;
    Slider m_BGMSlider;
    GameObject m_BGMSliderSelectedBg;
    Slider m_EffectVolumeSlider;
    GameObject m_EffectVolumeSliderSelectedBg;
    Text m_BGMVolumeText;
    Text m_EffectVolumeText;
    Button m_maxCellSettingButton;
    Text m_maxCellSettingText;
    ToggleGroup m_backgroundToggleGroup;
    Slider m_backgroundToggleSlider;
    GameObject m_backgroundSettingSelectedBg;
    List<Toggle> m_toggles = new List<Toggle>();
    bool m_isSelectedCellCountButton;
    Button m_resetSettingsButton;
    GameObject m_resetSettingsSelectedBg;
    Button m_aboutButton;
    GameObject m_aboutButtonSelectedBg;
    float m_deltaTime = 0;
    AboutView m_aboutView;
    GameObject m_maxCellSettingArrows;
    bool m_isPresettingEffectVolume;

    GameObject m_confirmPanel;
    Button m_confirmSettingCellCountBtn;
    Text m_confirmSettingCellCountText;
    Button m_cancelSettingCellCountBtn;
    Text m_cancelSettingCellCountText;
    int m_nextCellCountChange;

    public SettingsView() {
        var obj = Resources.Load<GameObject>(m_prefabPath);
        m_viewGameObject = GameObject.Instantiate<GameObject>(obj, Vector3.zero, Quaternion.identity, ViewManager.Instance.GetRootTransform());
        var position = m_viewGameObject.transform.localPosition;
        position.z = 0;
        m_viewGameObject.transform.localPosition = position;

        m_maxCellSettingButton = m_viewGameObject.transform.Find("Panel/CenterPanel/MaxCellSetting/MaxCellCount").GetComponent<Button>();
        m_maxCellSettingText = m_viewGameObject.transform.Find("Panel/CenterPanel/MaxCellSetting/MaxCellCount/Count").GetComponent<Text>();
        m_maxCellSettingArrows = m_viewGameObject.transform.Find("Panel/CenterPanel/MaxCellSetting/MaxCellCount/Arrows").gameObject;

        m_backgroundToggleGroup = m_viewGameObject.transform.Find("Panel/CenterPanel/BackgroundSettings/Settings").GetComponent<ToggleGroup>();
        m_backgroundToggleSlider = m_viewGameObject.transform.Find("Panel/CenterPanel/BackgroundSettings/Settings").GetComponent<Slider>();
        m_backgroundToggleSlider.onValueChanged.AddListener(OnBackgroundSliderChanged);
        m_backgroundSettingSelectedBg = m_viewGameObject.transform.Find("Panel/CenterPanel/BackgroundSettings/SelectedBg").gameObject;
        m_toggles.Clear();
        for (int i = 1; i <= 2; i++) {
            var toggle = m_viewGameObject.transform.Find("Panel/CenterPanel/BackgroundSettings/Settings/Backgound"+i).GetComponent<Toggle>();
            m_toggles.Add(toggle);
        }
        m_backgroundToggleSlider.value = AppConfig.Instance.ThemeSelectedIdx;

        m_BGMSlider = m_viewGameObject.transform.Find("Panel/CenterPanel/BGMSettings/Slider").GetComponent<Slider>();
        m_BGMVolumeText = m_viewGameObject.transform.Find("Panel/CenterPanel/BGMSettings/VolumeText").GetComponent<Text>();
        m_BGMSlider.onValueChanged.AddListener(OnBgmSliderValueChanged);
        m_BGMSlider.value = AppConfig.Instance.BGMVolume;

        m_EffectVolumeSlider = m_viewGameObject.transform.Find("Panel/CenterPanel/VolumeEffectSettings/Slider").GetComponent<Slider>();
        m_EffectVolumeText = m_viewGameObject.transform.Find("Panel/CenterPanel/VolumeEffectSettings/VolumeText").GetComponent<Text>();
        m_EffectVolumeSlider.onValueChanged.AddListener(OnVolumeEffectValueChanged);
        m_isPresettingEffectVolume = true;
        m_EffectVolumeSlider.value = AppConfig.Instance.EffectVolume;
        m_isPresettingEffectVolume = false;

        m_resetSettingsButton = m_viewGameObject.transform.Find("Panel/CenterPanel/ResetSettings").GetComponent<Button>();
        m_resetSettingsButton.onClick.AddListener(OnResetSettingsButtonClicked);
        m_aboutButton = m_viewGameObject.transform.Find("Panel/CenterPanel/About").GetComponent<Button>();
        m_aboutButton.onClick.AddListener(OnAboutButtonClicked);

        m_confirmPanel = m_viewGameObject.transform.Find("ConfirmPanel").gameObject;
        m_confirmSettingCellCountBtn = m_confirmPanel.transform.Find("ConfirmBtn").GetComponent<Button>();
        m_confirmSettingCellCountText = m_confirmPanel.transform.Find("ConfirmBtn/Text").GetComponent<Text>();
        m_confirmSettingCellCountBtn.onClick.AddListener(OnClickConfirmSettingCellCountBtn);

        m_cancelSettingCellCountBtn = m_confirmPanel.transform.Find("CancelBtn").GetComponent<Button>();
        m_cancelSettingCellCountText = m_confirmPanel.transform.Find("CancelBtn/Text").GetComponent<Text>();
        m_cancelSettingCellCountBtn.onClick.AddListener(OnClickCancelSettingCellCountBtn);

        HandleSelectedEventTriggers();
        EventSystem.current.SetSelectedGameObject(m_maxCellSettingButton.gameObject);
    }

    void HandleSelectedEventTriggers() {
        var eventTrigger = m_BGMSlider.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new ();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener(OnBgmSliderSelected);
        eventTrigger.triggers.Add(entry);

        EventTrigger.Entry entry_1 = new ();
        entry_1.eventID = EventTriggerType.Deselect;
        entry_1.callback.AddListener(OnBgmSliderUnSelected);
        eventTrigger.triggers.Add(entry_1);

        var eventTrigger_1 = m_EffectVolumeSlider.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry_2 = new ();
        entry_2.eventID = EventTriggerType.Select;
        entry_2.callback.AddListener(OnEffectVolumeSliderSelected);
        eventTrigger_1.triggers.Add(entry_2);

        EventTrigger.Entry entry_3 = new ();
        entry_3.eventID = EventTriggerType.Deselect;
        entry_3.callback.AddListener(OnEffectVolumeSliderUnSelected);
        eventTrigger_1.triggers.Add(entry_3);

        var eventTrigger_2 = m_resetSettingsButton.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry_4 = new ();
        entry_4.eventID = EventTriggerType.Select;
        entry_4.callback.AddListener(OnResetSettingsButtonSelected);
        eventTrigger_2.triggers.Add(entry_4);

        EventTrigger.Entry entry_5 = new ();
        entry_5.eventID = EventTriggerType.Deselect;
        entry_5.callback.AddListener(OnResetSettingsButtonUnSelected);
        eventTrigger_2.triggers.Add(entry_5);

        var eventTrigger_3 = m_aboutButton.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry_6 = new ();
        entry_6.eventID = EventTriggerType.Select;
        entry_6.callback.AddListener(OnAboutButtonSelected);
        eventTrigger_3.triggers.Add(entry_6);

        EventTrigger.Entry entry_7 = new ();
        entry_7.eventID = EventTriggerType.Deselect;
        entry_7.callback.AddListener(OnAboutButtonUnSelected);
        eventTrigger_3.triggers.Add(entry_7);

        var eventTrigger_4 = m_maxCellSettingButton.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry_8 = new ();
        entry_8.eventID = EventTriggerType.Select;
        entry_8.callback.AddListener(OnCellCountButtonSelected);
        eventTrigger_4.triggers.Add(entry_8);

        EventTrigger.Entry entry_9 = new ();
        entry_9.eventID = EventTriggerType.Deselect;
        entry_9.callback.AddListener(OnCellCountButtonUnSelected);
        eventTrigger_4.triggers.Add(entry_9);

        var eventTrigger_5 = m_backgroundToggleGroup.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry_10 = new();
        entry_10.eventID = EventTriggerType.Select;
        entry_10.callback.AddListener(OnTabSelected);
        eventTrigger_5.triggers.Add(entry_10);

        EventTrigger.Entry entry_11 = new();
        entry_11.eventID = EventTriggerType.Deselect;
        entry_11.callback.AddListener(OnTabUnSelected);
        eventTrigger_5.triggers.Add(entry_11);

        var eventTrigger_6 = m_confirmSettingCellCountBtn.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry_12 = new();
        entry_12.eventID = EventTriggerType.Select;
        entry_12.callback.AddListener(OnConfirmSettingCellCountBtnSelected);
        eventTrigger_6.triggers.Add(entry_12);

        EventTrigger.Entry entry_13 = new();
        entry_13.eventID = EventTriggerType.Deselect;
        entry_13.callback.AddListener(OnConfirmSettingCellCountBtnUnSelected);
        eventTrigger_6.triggers.Add(entry_13);


        var eventTrigger_7 = m_cancelSettingCellCountBtn.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry_14 = new();
        entry_14.eventID = EventTriggerType.Select;
        entry_14.callback.AddListener(OnCancelSettingCellCountBtnSelected);
        eventTrigger_7.triggers.Add(entry_14);

        EventTrigger.Entry entry_15 = new();
        entry_15.eventID = EventTriggerType.Deselect;
        entry_15.callback.AddListener(OnCancelSettingCellCountBtnUnSelected);
        eventTrigger_7.triggers.Add(entry_15);
    }


    public void Show() {
        m_viewGameObject.SetActive(true);
        m_maxCellSettingText.text = AppConfig.Instance.MaxCellCount.ToString();
        var lastView = ViewManager.Instance.GetLastPopedView();
        if (lastView != null && lastView == m_aboutView)
            EventSystem.current.SetSelectedGameObject(m_aboutButton.gameObject);
        else
            EventSystem.current.SetSelectedGameObject(m_maxCellSettingButton.gameObject);
    }

    public void Hide() {
        EventSystem.current.SetSelectedGameObject(null);
        m_viewGameObject.SetActive(false);
        AudioManager.Instance.StopNumberRotateEffect();
    }

    public void Update() {
        if (Input.GetButtonDown("Cancel")) {
            Hide();
            ViewManager.Instance.Hided(this);
        }

        if (Input.GetButtonUp("Horizontal")) {
            m_deltaTime = 0;
        }

        if (Input.GetButtonDown("Horizontal") && m_isSelectedCellCountButton) {
            if (Input.GetKey(KeyCode.LeftArrow)) {
                if (AppConfig.Instance.HasHistoryGameData()) {
                    m_confirmPanel.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(m_confirmSettingCellCountBtn.gameObject);
                    m_nextCellCountChange = -1;
                } else{
                    OnMaxCellSettingSliderValueChanged(-1);
                }
            }
            if (Input.GetKey(KeyCode.RightArrow)) {
                if (AppConfig.Instance.HasHistoryGameData()) {
                    m_confirmPanel.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(m_confirmSettingCellCountBtn.gameObject);
                    m_nextCellCountChange = +1;
                }else{
                    OnMaxCellSettingSliderValueChanged(+1);
                }
            }
        }

        if (Input.GetButton("Horizontal") && m_isSelectedCellCountButton) {
            m_deltaTime += Time.deltaTime;
            if (Input.GetKey(KeyCode.LeftArrow) && m_deltaTime > 1) {
                OnMaxCellSettingSliderValueChanged(-1);
            }
            if (Input.GetKey(KeyCode.RightArrow) && m_deltaTime > 1) {
                OnMaxCellSettingSliderValueChanged(+1);
            }
        }
    }

    public void OnAndroidKeyDown(string keyName) {
        
    }

    public void OnClickBackHomeButton() {
        Hide();
        ViewManager.Instance.Hided(this);
    }

    private void OnTabSelected(BaseEventData data) {
        m_backgroundSettingSelectedBg.SetActive(true);
    }

    private void OnTabUnSelected(BaseEventData data) {
        m_backgroundSettingSelectedBg.SetActive(false);
    }

    private void OnBackgroundSliderChanged(float value) {
        var intValue = Mathf.FloorToInt(value);
        AppConfig.Instance.ThemeSelectedIdx = intValue;
        m_toggles[AppConfig.Instance.ThemeSelectedIdx].isOn = true;
    }

    public void OnBgmSliderValueChanged(float value) {
        m_BGMVolumeText.text = Mathf.FloorToInt(value).ToString("+#;-#;0");
        AppConfig.Instance.BGMVolume = Mathf.FloorToInt(value);
        AudioManager.Instance.SetBgmVolume((int)value);
    }

    public void OnVolumeEffectValueChanged(float value) {
        m_EffectVolumeText.text = Mathf.FloorToInt(value).ToString("+#;-#;0");
        AppConfig.Instance.EffectVolume = Mathf.FloorToInt(value);
        AudioManager.Instance.SetEffectVolume((int)value);
        if (m_isPresettingEffectVolume) return;
        AudioManager.Instance.PlayNumberRotateEffectWithoutLoop();
        AudioManager.Instance.PlayNumberCheckEffect();
    }

    // 确认修改最大cell数
    void OnClickConfirmSettingCellCountBtn() {
        m_confirmPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(m_maxCellSettingButton.gameObject);
        OnMaxCellSettingSliderValueChanged(m_nextCellCountChange);
    }

    void OnConfirmSettingCellCountBtnSelected(BaseEventData data) {
        m_confirmSettingCellCountText.color = Color.white;
        m_cancelSettingCellCountText.color = new Color(0f, 147 / 255f, 1.0f, 1.0f);
    }

    void OnConfirmSettingCellCountBtnUnSelected(BaseEventData data) {
        m_confirmSettingCellCountText.color = new Color(0f, 147 / 255f, 1.0f, 1.0f);
        m_cancelSettingCellCountText.color = Color.white;
    }

    void OnClickCancelSettingCellCountBtn() {
        m_confirmPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(m_backgroundToggleSlider.gameObject);
    }

    void OnCancelSettingCellCountBtnSelected(BaseEventData data) {
        m_cancelSettingCellCountText.color = Color.white;
        m_confirmSettingCellCountText.color = new Color(0f, 147 / 255f, 1.0f, 1.0f);
    }

    void OnCancelSettingCellCountBtnUnSelected(BaseEventData data) {
        m_cancelSettingCellCountText.color = new Color(0f, 147 / 255f, 1.0f, 1.0f);
        m_confirmSettingCellCountText.color = Color.white;
    }

    void OnBgmSliderSelected(BaseEventData data) {
        if (!m_BGMSliderSelectedBg) {
            m_BGMSliderSelectedBg = m_BGMSlider.transform.parent.Find("SelectedBg").gameObject;
        }
        m_BGMSliderSelectedBg.SetActive(true);
    }

    void OnBgmSliderUnSelected(BaseEventData data) {
        if (!m_BGMSliderSelectedBg) {
            m_BGMSliderSelectedBg = m_BGMSlider.transform.parent.Find("SelectedBg").gameObject;
        }
        m_BGMSliderSelectedBg.SetActive(false);
    }

    void OnEffectVolumeSliderSelected(BaseEventData data) {
        if (!m_EffectVolumeSliderSelectedBg) {
            m_EffectVolumeSliderSelectedBg = m_EffectVolumeSlider.transform.parent.Find("SelectedBg").gameObject;
        }
        m_EffectVolumeSliderSelectedBg.SetActive(true);
    }

    void OnEffectVolumeSliderUnSelected(BaseEventData data) {
        if (!m_EffectVolumeSliderSelectedBg) {
            m_EffectVolumeSliderSelectedBg = m_EffectVolumeSlider.transform.parent.Find("SelectedBg").gameObject;
        }
        m_EffectVolumeSliderSelectedBg.SetActive(false);
    }

    void OnResetSettingsButtonSelected(BaseEventData data) {
        if (!m_resetSettingsSelectedBg) {
            m_resetSettingsSelectedBg = m_resetSettingsButton.transform.Find("SelectedBg").gameObject;
        }
        m_resetSettingsSelectedBg.SetActive(true);
    }

    void OnResetSettingsButtonUnSelected(BaseEventData data) {
        if (!m_resetSettingsSelectedBg) {
            m_resetSettingsSelectedBg = m_resetSettingsButton.transform.Find("SelectedBg").gameObject;
        }
        m_resetSettingsSelectedBg.SetActive(false);
    }

    void OnAboutButtonSelected(BaseEventData data) {
        if (!m_aboutButtonSelectedBg) {
            m_aboutButtonSelectedBg = m_aboutButton.transform.Find("SelectedBg").gameObject;
        }
        m_aboutButtonSelectedBg.SetActive(true);
    }

    void OnAboutButtonUnSelected(BaseEventData data) {
        if (!m_aboutButtonSelectedBg) {
            m_aboutButtonSelectedBg = m_aboutButton.transform.Find("SelectedBg").gameObject;
        }
        m_aboutButtonSelectedBg.SetActive(false);
    }

    void OnCellCountButtonSelected(BaseEventData data) {
        m_isSelectedCellCountButton = true;
        m_maxCellSettingArrows.SetActive(true);
    }

    void OnCellCountButtonUnSelected(BaseEventData data) {
        m_isSelectedCellCountButton = false;
        m_maxCellSettingArrows.SetActive(false);
    }

    void OnResetSettingsButtonClicked() {
        AppConfig.Instance.MaxCellCount = 75;
        m_maxCellSettingText.text = "75";
        m_toggles[0].isOn = true;
        AppConfig.Instance.ThemeSelectedIdx = 0;
        AppConfig.Instance.BGMVolume = 0;
        m_BGMSlider.value = 0;

        AppConfig.Instance.EffectVolume = 0;
        m_EffectVolumeSlider.value = 0;
    }

    void OnAboutButtonClicked() {
        if (m_aboutView == null) {
            m_aboutView = new AboutView();
        }
        ViewManager.Instance.Push(m_aboutView);
    }

    public void OnMaxCellSettingSliderValueChanged(int value) {
        if (value < 0 && AppConfig.Instance.MaxCellCount == 8) return;
        if (value > 0 && AppConfig.Instance.MaxCellCount == 75) return;

        AppConfig.Instance.MaxCellCount += value;
        m_maxCellSettingText.text = AppConfig.Instance.MaxCellCount.ToString();
        AppConfig.Instance.ClearGameData();
    }

    public void OnThemeTypeChanged() {

    }
}
