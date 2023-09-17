using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HomeView : IViewOperater
{
    string m_prefabPath = "HomePanel";
    private Button m_startButton;
    private Button m_reopenButton;
    private Button m_settingsButton;

    Transform m_resetPanel;
    Button m_resetBtn;
    Text m_resetBtnText;
    Button m_resetCancelBtn;
    Text m_resetCancelBtnText;

    PlayGameView m_playGameView;
    SettingsView m_settingsView;
    GameObject m_viewGameObject;
    
    // Start is called before the first frame update
    public HomeView(Transform parent)
    {
        Application.targetFrameRate = 30;
        AudioManager.InitVolume();
        var obj = Resources.Load<GameObject>(m_prefabPath);
        m_viewGameObject = GameObject.Instantiate<GameObject>(obj, Vector3.zero, Quaternion.identity, ViewManager.Instance.GetRootTransform());
        var position = m_viewGameObject.transform.localPosition;
        position.z = 0;
        m_viewGameObject.transform.localPosition = position;

        m_startButton = m_viewGameObject.transform.Find("StartPanel/StartButton").GetComponent<Button>();
        m_startButton.onClick.AddListener(OnClickStartButton);
        m_reopenButton = m_viewGameObject.transform.Find("StartPanel/ReopenButton").GetComponent<Button>();
        m_reopenButton.onClick.AddListener(OnClickReopenButton);
        m_settingsButton = m_viewGameObject.transform.Find("SettingsButton").GetComponent<Button>();
        m_settingsButton.onClick.AddListener(OnClickSettingsButton);

        m_resetPanel = m_viewGameObject.transform.Find("ResetPanel");
        m_resetBtn = m_viewGameObject.transform.Find("ResetPanel/ResetBtn").GetComponent<Button>();
        m_resetBtnText = m_viewGameObject.transform.Find("ResetPanel/ResetBtn/Text").GetComponent<Text>();
        m_resetBtn.onClick.AddListener(OnClickedResetBtn);
        m_resetCancelBtn = m_viewGameObject.transform.Find("ResetPanel/CancelBtn").GetComponent<Button>();
        m_resetCancelBtnText = m_viewGameObject.transform.Find("ResetPanel/CancelBtn/Text").GetComponent<Text>();
        m_resetCancelBtn.onClick.AddListener(OnClickedResetCancelBtn);

        var eventTrigger = m_resetCancelBtn.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener(OnCancelResetGameButtonSelected);
        eventTrigger.triggers.Add(entry);

        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        entry1.eventID = EventTriggerType.Deselect;
        entry1.callback.AddListener(OnCancelResetGameButtonDeselected);
        eventTrigger.triggers.Add(entry1);

        var isReopen = AppConfig.Instance.HasHistoryGameData();
        m_reopenButton.gameObject.SetActive(isReopen);
    }

    void OnClickStartButton() 
    {
        var isReopen = AppConfig.Instance.HasHistoryGameData();
        Debug.Log(isReopen);
        if (isReopen)
        {
            ShowResetToastPanel();
        }else
        {
            ResetToStartGame();
        }
        
    }

    void ResetToStartGame()
    {
        // PreferencesStorage.SaveString(AppConfig.__REOPEN_DATA__, null);
        AppConfig.Instance.ClearGameData();
        ShowPlayGameView(reset: true);
    }

    void OnClickReopenButton() {
        ShowPlayGameView();
    }

    void ShowResetToastPanel()
    {
        m_resetPanel.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(m_resetBtn.gameObject);
    }

    void HideResetToastPanel()
    {
        m_resetPanel.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(m_reopenButton.gameObject);
    }

    public void OnClickedResetBtn()
    {
        ResetToStartGame();

        HideResetToastPanel();
    }

    public void OnClickedResetCancelBtn()
    {
        HideResetToastPanel();
    }

    void OnResetGameButtonSelected(BaseEventData data)
    {
        m_resetBtnText.color = Color.white;
        m_resetCancelBtnText.color = new Color(0f, 147 / 255f, 1.0f, 1.0f);
    }

    void OnResetGameButtonDeselected(BaseEventData data)
    {
        m_resetBtnText.color = new Color(0f, 147 / 255f, 1.0f, 1.0f);
    }

    void OnCancelResetGameButtonSelected(BaseEventData data)
    {
        m_resetCancelBtnText.color = Color.white;
        m_resetBtnText.color = new Color(0f, 147 / 255f, 1.0f, 1.0f);
    }

    void OnCancelResetGameButtonDeselected(BaseEventData data)
    {
        m_resetCancelBtnText.color = new Color(0f, 147 / 255f, 1.0f, 1.0f);
    }

    void OnClickSettingsButton() {
        ShowSettingsView();
    }

    public void Show() {
        m_viewGameObject.SetActive(true);
        if (ViewManager.Instance.GetLastPopedView() == m_playGameView) 
            OnGamePlayViewHide();
        else if (ViewManager.Instance.GetLastPopedView() == m_settingsView) 
            OnSettingsViewHide();
        else
            OnGamePlayViewHide();
    }

    public void Hide() {
        m_viewGameObject.SetActive(false);
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (ToastView.Instance.IsToasting()) {
                Debug.Log("Quit app");
                Application.Quit();
                return;
            }
            ToastView.Instance.Show();
        }
    }

    public void OnAndroidKeyDown(string keyName) {
        
    }

    void ShowPlayGameView(bool reset = false) {
        if (m_playGameView == null) {
            m_playGameView = new PlayGameView();
        }
        if (reset)
            m_playGameView.ResetData();
        ViewManager.Instance.Push(m_playGameView);
    }

    void OnGamePlayViewHide() {
        var isReopen = AppConfig.Instance.HasHistoryGameData();
        if (isReopen) {
            m_reopenButton.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(m_reopenButton.gameObject);
        }else {
            EventSystem.current.SetSelectedGameObject(m_startButton.gameObject);
            m_reopenButton.gameObject.SetActive(false);
        }
    }

    void ShowSettingsView() {
        if (m_settingsView == null) {
            m_settingsView = new SettingsView();
        }
        ViewManager.Instance.Push(m_settingsView);
    }

    void OnSettingsViewHide() {
        EventSystem.current.SetSelectedGameObject(m_settingsButton.gameObject);
    }
}
