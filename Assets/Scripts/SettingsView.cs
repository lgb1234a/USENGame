using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    GameObject m_backgroundSettingSelectedBg;
    Toggle[] m_toggles = new Toggle[4];
    bool m_isSelectedCellCountButton;
    Button m_resetSettingsButton;
    GameObject m_resetSettingsSelectedBg;
    Button m_aboutButton;
    GameObject m_aboutButtonSelectedBg;
    float m_deltaTime = 0;
    AboutView m_aboutView;
    GameObject m_maxCellSettingArrows;

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
        m_backgroundSettingSelectedBg = m_viewGameObject.transform.Find("Panel/CenterPanel/BackgroundSettings/SelectedBg").gameObject;
        for (int i = 1; i <= 4; i++) {
            var toggle = m_viewGameObject.transform.Find("Panel/CenterPanel/BackgroundSettings/Settings/Backgound"+i).GetComponent<Toggle>();
            m_toggles[i - 1] = toggle;

            var eventTrigger = toggle.gameObject.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Select;
            entry.callback.AddListener(OnTabSelected);
            eventTrigger.triggers.Add(entry);

            EventTrigger.Entry entry_1 = new EventTrigger.Entry();
            entry_1.eventID = EventTriggerType.Deselect;
            entry_1.callback.AddListener(OnTabUnSelected);
            eventTrigger.triggers.Add(entry_1);
        }

        m_BGMSlider = m_viewGameObject.transform.Find("Panel/CenterPanel/BGMSettings/Slider").GetComponent<Slider>();
        m_BGMVolumeText = m_viewGameObject.transform.Find("Panel/CenterPanel/BGMSettings/VolumeText").GetComponent<Text>();
        m_BGMSlider.onValueChanged.AddListener(OnBgmSliderValueChanged);
        m_BGMSlider.value = AppConfig.Instance.BGMVolume;

        m_EffectVolumeSlider = m_viewGameObject.transform.Find("Panel/CenterPanel/VolumeEffectSettings/Slider").GetComponent<Slider>();
        m_EffectVolumeText = m_viewGameObject.transform.Find("Panel/CenterPanel/VolumeEffectSettings/VolumeText").GetComponent<Text>();
        m_EffectVolumeSlider.onValueChanged.AddListener(OnVolumeEffectValueChanged);
        m_EffectVolumeSlider.value = AppConfig.Instance.EffectVolume;

        m_resetSettingsButton = m_viewGameObject.transform.Find("Panel/CenterPanel/ResetSettings").GetComponent<Button>();
        m_resetSettingsButton.onClick.AddListener(OnResetSettingsButtonClicked);
        m_aboutButton = m_viewGameObject.transform.Find("Panel/CenterPanel/About").GetComponent<Button>();
        m_aboutButton.onClick.AddListener(onAboutButtonClicked);

        HandleSelectedEventTriggers();
        EventSystem.current.SetSelectedGameObject(m_maxCellSettingButton.gameObject);
    }

    void HandleSelectedEventTriggers() {
        var eventTrigger = m_BGMSlider.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener(OnBgmSliderSelected);
        eventTrigger.triggers.Add(entry);

        EventTrigger.Entry entry_1 = new EventTrigger.Entry();
        entry_1.eventID = EventTriggerType.Deselect;
        entry_1.callback.AddListener(OnBgmSliderUnSelected);
        eventTrigger.triggers.Add(entry_1);

        var eventTrigger_1 = m_EffectVolumeSlider.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry_2 = new EventTrigger.Entry();
        entry_2.eventID = EventTriggerType.Select;
        entry_2.callback.AddListener(OnEffectVolumeSliderSelected);
        eventTrigger_1.triggers.Add(entry_2);

        EventTrigger.Entry entry_3 = new EventTrigger.Entry();
        entry_3.eventID = EventTriggerType.Deselect;
        entry_3.callback.AddListener(OnEffectVolumeSliderUnSelected);
        eventTrigger_1.triggers.Add(entry_3);

        var eventTrigger_2 = m_resetSettingsButton.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry_4 = new EventTrigger.Entry();
        entry_4.eventID = EventTriggerType.Select;
        entry_4.callback.AddListener(OnResetSettingsButtonSelected);
        eventTrigger_2.triggers.Add(entry_4);

        EventTrigger.Entry entry_5 = new EventTrigger.Entry();
        entry_5.eventID = EventTriggerType.Deselect;
        entry_5.callback.AddListener(OnResetSettingsButtonUnSelected);
        eventTrigger_2.triggers.Add(entry_5);

        var eventTrigger_3 = m_aboutButton.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry_6 = new EventTrigger.Entry();
        entry_6.eventID = EventTriggerType.Select;
        entry_6.callback.AddListener(OnAboutButtonSelected);
        eventTrigger_3.triggers.Add(entry_6);

        EventTrigger.Entry entry_7 = new EventTrigger.Entry();
        entry_7.eventID = EventTriggerType.Deselect;
        entry_7.callback.AddListener(OnAboutButtonUnSelected);
        eventTrigger_3.triggers.Add(entry_7);

        var eventTrigger_4 = m_maxCellSettingButton.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry_8 = new EventTrigger.Entry();
        entry_8.eventID = EventTriggerType.Select;
        entry_8.callback.AddListener(OnCellCountButtonSelected);
        eventTrigger_4.triggers.Add(entry_8);

        EventTrigger.Entry entry_9 = new EventTrigger.Entry();
        entry_9.eventID = EventTriggerType.Deselect;
        entry_9.callback.AddListener(OnCellCountButtonUnSelected);
        eventTrigger_4.triggers.Add(entry_9);
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
            if (Input.GetKey(KeyCode.LeftArrow))
                OnMaxCellSettingSliderValueChanged(-1);
            if (Input.GetKey(KeyCode.RightArrow)) 
                OnMaxCellSettingSliderValueChanged(+1);
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
        var toggle = data.selectedObject.GetComponent<Toggle>();
        toggle.isOn = true;
        m_backgroundSettingSelectedBg.SetActive(true);
    }

    private void OnTabUnSelected(BaseEventData data) {
        m_backgroundSettingSelectedBg.SetActive(false);
    }

    public void OnBgmSliderValueChanged(float value) {
        m_BGMVolumeText.text = Mathf.FloorToInt(value).ToString("+#;-#;0");
        AppConfig.Instance.BGMVolume = Mathf.FloorToInt(value);
        AudioManager.Instance.SetBgmVolume(value / 10f + 0.5f);
    }

    public void OnVolumeEffectValueChanged(float value) {
        m_EffectVolumeText.text = Mathf.FloorToInt(value).ToString("+#;-#;0");
        AppConfig.Instance.EffectVolume = Mathf.FloorToInt(value);
        AudioManager.Instance.SetEffectVolume(value / 10f + 0.5f);
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

        AppConfig.Instance.BGMVolume = 0;
        m_BGMSlider.value = 0;

        AppConfig.Instance.EffectVolume = 0;
        m_EffectVolumeSlider.value = 0;
    }

    void onAboutButtonClicked() {
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
    }
}
