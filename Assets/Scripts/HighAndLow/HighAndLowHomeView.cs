using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;

public class HighAndLowHomeView : AbstractView, IViewOperater
{
    string m_prefabPath = "HighAndLow/HighAndLowHomePanel";
    Button m_startBtn;
    HighAndLowFAQView m_faqView;
    HightAndLowGameView m_gameView;
    HighAndLowSettingsView m_settingsView;
    public void Build()
    {
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());

        m_startBtn = m_mainViewGameObject.transform.Find("StartBtn").GetComponent<Button>();
        m_startBtn.onClick.AddListener(OnClickStartButton);
    }

    public void Hide()
    {
        m_mainViewGameObject.SetActive(false);
    }

    public void OnAndroidKeyDown(string keyName)
    {
        if (keyName == "blue")
        {
            if (m_faqView == null) {
                m_faqView = new HighAndLowFAQView();
            }
            ViewManager.Instance.Push(m_faqView);
        }

        if (keyName == "yellow")
        {
            if (m_settingsView == null) {
                m_settingsView = new HighAndLowSettingsView();
            }
            ViewManager.Instance.Push(m_settingsView);
        }
    }

    public void OnThemeTypeChanged()
    {
        
    }

    public void Show()
    {
        m_mainViewGameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(m_startBtn.gameObject);
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            USENSceneManager.Instance.LoadScene("GameEntries");
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            //测试
            if (m_faqView == null) {
                m_faqView = new HighAndLowFAQView();
            }
            ViewManager.Instance.Push(m_faqView);
        }

        if (Input.GetKeyDown(KeyCode.B)) {
            //测试
            if (m_settingsView == null) {
                m_settingsView = new HighAndLowSettingsView();
            }
            ViewManager.Instance.Push(m_settingsView);
        }
    }

    public void OnClickStartButton()
    {
        if (m_gameView == null) {
            m_gameView = new HightAndLowGameView();
        }
        ViewManager.Instance.Push(m_gameView);
    }
}