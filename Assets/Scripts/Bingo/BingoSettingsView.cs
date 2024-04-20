using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BingoSettingsView : AbstractView, IViewOperater
{
    string m_prefabPath = "Bingo/BingoSettingsPanel";
    Button m_maxCellSettingButton;
    Text m_maxCellSettingText;
    bool m_isSelectedCellCountButton;
    Button m_aboutButton;
    GameObject m_aboutButtonSelectedBg;
    float m_deltaTime = 0;
    BingoAboutView m_aboutView;
    GameObject m_maxCellSettingArrows;

    GameObject m_confirmPanel;
    Button m_confirmSettingCellCountBtn;
    Text m_confirmSettingCellCountText;
    Button m_cancelSettingCellCountBtn;
    Text m_cancelSettingCellCountText;
    int m_nextCellCountChange;

    public void Build() {
        // var obj = Resources.Load<GameObject>(m_prefabPath);
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());
        // m_mainViewGameObject = GameObject.Instantiate<GameObject>(obj, Vector3.zero, Quaternion.identity, ViewManager.Instance.GetRootTransform());
        var position = m_mainViewGameObject.transform.localPosition;
        position.z = 0;
        m_mainViewGameObject.transform.localPosition = position;

        m_maxCellSettingButton = m_mainViewGameObject.transform.Find("Panel/CenterPanel/MaxCellSetting/MaxCellCount").GetComponent<Button>();
        m_maxCellSettingText = m_mainViewGameObject.transform.Find("Panel/CenterPanel/MaxCellSetting/MaxCellCount/Count").GetComponent<Text>();
        m_maxCellSettingArrows = m_mainViewGameObject.transform.Find("Panel/CenterPanel/MaxCellSetting/MaxCellCount/Arrows").gameObject;

        m_aboutButton = m_mainViewGameObject.transform.Find("Panel/CenterPanel/About").GetComponent<Button>();
        m_aboutButton.onClick.AddListener(OnAboutButtonClicked);

        m_confirmPanel = m_mainViewGameObject.transform.Find("ConfirmPanel").gameObject;
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
        m_mainViewGameObject.SetActive(true);
        m_maxCellSettingText.text = AppConfig.Instance.MaxCellCount.ToString();
        var lastView = ViewManager.Instance.GetLastPopedView();
        if (lastView != null && lastView == m_aboutView)
            EventSystem.current.SetSelectedGameObject(m_aboutButton.gameObject);
        else
            EventSystem.current.SetSelectedGameObject(m_maxCellSettingButton.gameObject);
    }

    public void Hide() {
        EventSystem.current.SetSelectedGameObject(null);
        m_mainViewGameObject.SetActive(false);
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
        EventSystem.current.SetSelectedGameObject(m_aboutButton.gameObject);
    }

    void OnCancelSettingCellCountBtnSelected(BaseEventData data) {
        m_cancelSettingCellCountText.color = Color.white;
        m_confirmSettingCellCountText.color = new Color(0f, 147 / 255f, 1.0f, 1.0f);
    }

    void OnCancelSettingCellCountBtnUnSelected(BaseEventData data) {
        m_cancelSettingCellCountText.color = new Color(0f, 147 / 255f, 1.0f, 1.0f);
        m_confirmSettingCellCountText.color = Color.white;
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

    void OnAboutButtonClicked() {
        if (m_aboutView == null) {
            m_aboutView = new BingoAboutView();
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
