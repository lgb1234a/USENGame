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

    PlayGameView m_playGameView;
    SettingsView m_settingsView;
    GameObject m_viewGameObject;
    // Start is called before the first frame update
    public HomeView(Transform parent)
    {
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

        var isReopen = PreferencesStorage.ReadBoolean(PlayGameView.__REOPEN_DATA__, false);
        m_reopenButton.gameObject.SetActive(isReopen);
    }

    void OnClickStartButton() {
        PreferencesStorage.SaveString(PlayGameView.__REOPEN_DATA__, null);
        ShowPlayGameView();
    }

    void OnClickReopenButton() {
        ShowPlayGameView();
    }

    void OnClickSettingsButton() {
        ShowSettingsView();
    }

    public void Show() {
        if (ViewManager.Instance.GetLastPopedView() == m_playGameView) 
            OnGamePlayViewHide();
        else if (ViewManager.Instance.GetLastPopedView() == m_settingsView) 
            OnSettingsViewHide();
        else
            OnGamePlayViewHide();
    }

    public void Hide() {

    }

    public void Update() {
    
    }

    void ShowPlayGameView() {
        if (m_playGameView == null) {
            m_playGameView = new PlayGameView();
        }
        ViewManager.Instance.Push(m_playGameView);
    }

    void OnGamePlayViewHide() {
        var saveData = PreferencesStorage.ReadString(PlayGameView.__REOPEN_DATA__, null);
        if (saveData != null && saveData.Length > 0) {
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
