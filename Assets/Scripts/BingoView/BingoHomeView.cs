using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;

public class BingoHomeView : AbstractView, IViewOperater
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
    Transform m_effectParentPanel;
    SkeletonGraphic m_homeSpineSkeletonGraphic;

    public void Build() {
        Application.targetFrameRate = 30;
        AudioManager.InitVolume();
        // var obj = Resources.Load<GameObject>(m_prefabPath);
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());
        // m_mainViewGameObject = GameObject.Instantiate<GameObject>(obj, Vector3.zero, Quaternion.identity, ViewManager.Instance.GetRootTransform());
        var position = m_mainViewGameObject.transform.localPosition;
        position.z = 0;
        m_mainViewGameObject.transform.localPosition = position;

        // 改成固定动画，不随主题变动而更新
        m_effectParentPanel = m_mainViewGameObject.transform.Find("EffectPanel");
        // var gameObject = Resources.Load<GameObject>("DefaultTheme/Effects/Home/EffectPanel");
        m_homeSpineSkeletonGraphic = LoadViewGameObject("DefaultTheme/Effects/Home/EffectPanel", m_effectParentPanel).GetComponent<SkeletonGraphic>();
        m_homeSpineSkeletonGraphic.AnimationState.SetAnimation(0, "titlle", true);

        m_startButton = m_mainViewGameObject.transform.Find("StartPanel/StartButton").GetComponent<Button>();
        m_startButton.onClick.AddListener(OnClickStartButton);
        m_reopenButton = m_mainViewGameObject.transform.Find("StartPanel/ReopenButton").GetComponent<Button>();
        m_reopenButton.onClick.AddListener(OnClickReopenButton);
        m_settingsButton = m_mainViewGameObject.transform.Find("SettingsButton").GetComponent<Button>();
        m_settingsButton.onClick.AddListener(OnClickSettingsButton);

        m_resetPanel = m_mainViewGameObject.transform.Find("ResetPanel");
        m_resetBtn = m_mainViewGameObject.transform.Find("ResetPanel/ResetBtn").GetComponent<Button>();
        m_resetBtnText = m_mainViewGameObject.transform.Find("ResetPanel/ResetBtn/Text").GetComponent<Text>();
        m_resetBtn.onClick.AddListener(OnClickedResetBtn);
        m_resetCancelBtn = m_mainViewGameObject.transform.Find("ResetPanel/CancelBtn").GetComponent<Button>();
        m_resetCancelBtnText = m_mainViewGameObject.transform.Find("ResetPanel/CancelBtn/Text").GetComponent<Text>();
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

        var eventTrigger2 = m_resetBtn.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.Select;
        entry2.callback.AddListener(OnResetGameButtonSelected);
        eventTrigger2.triggers.Add(entry2);

        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.Deselect;
        entry3.callback.AddListener(OnResetGameButtonDeselected);
        eventTrigger2.triggers.Add(entry3);

        var isReopen = AppConfig.Instance.HasHistoryGameData();
        m_reopenButton.gameObject.SetActive(isReopen);
    }

    void OnClickStartButton() 
    {
        var isReopen = AppConfig.Instance.HasHistoryGameData();
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
        HideResetToastPanel();
        ResetToStartGame();
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
        m_mainViewGameObject.SetActive(true);
        if (ViewManager.Instance.GetLastPopedView() == m_playGameView) 
            OnGamePlayViewHide();
        else if (ViewManager.Instance.GetLastPopedView() == m_settingsView) 
            OnSettingsViewHide();
        else
            OnGamePlayViewHide();
    }

    public void Hide() {
        m_mainViewGameObject.SetActive(false);

        Destroy();
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (ToastView.Instance.IsToasting()) {
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
            ViewManager.Instance.ShowLoading();
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

    public void OnThemeTypeChanged() {
        // var childCount = m_effectParentPanel.childCount;
        // for (int i = 0; i < childCount; i++) {
        //     var go = m_effectParentPanel.GetChild(i);
        //     GameObject.Destroy(go);
        // }
        // m_homeSpineSkeletonGraphic = ThemeResManager.Instance.InstantiateHomeSpineGameObject(m_effectParentPanel).GetComponent<SkeletonGraphic>();
        // m_homeSpineSkeletonGraphic.AnimationState.SetAnimation(0, "titlle", true);
    }
}
