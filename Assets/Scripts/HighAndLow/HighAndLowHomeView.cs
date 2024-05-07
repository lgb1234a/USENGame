using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;
using System.Collections.Generic;

public class HighAndLowHomeView : AbstractView, IViewOperater
{
    string m_prefabPath = "HighAndLow/HighAndLowHomePanel";
    Button m_startBtn;
    HightAndLowGameView m_gameView;

    GameObject m_resetPanel;
    Button m_resetBtn;
    Button m_cancelBtn;
    public void Build()
    {
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());

        m_startBtn = m_mainViewGameObject.transform.Find("StartBtn").GetComponent<Button>();
        m_startBtn.onClick.AddListener(OnClickStartButton);
        AudioManager.Instance.PlayLowAndHighBGM();

        m_resetPanel = m_mainViewGameObject.transform.Find("ResetPanel").gameObject;
        m_resetBtn = m_mainViewGameObject.transform.Find("ResetPanel/ResetBtn").GetComponent<Button>();
        m_resetBtn.onClick.AddListener(OnClickResetButton);
        m_cancelBtn = m_mainViewGameObject.transform.Find("ResetPanel/CancelBtn").GetComponent<Button>();
        m_cancelBtn.onClick.AddListener(OnClickCancelButton);
    }

    public void Hide()
    {
        m_mainViewGameObject.SetActive(false);
    }

    public void OnAndroidKeyDown(string keyName)
    {
        
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
    }

    public void OnClickStartButton()
    {
        if (AppConfig.Instance.CheckedPokers.Count > 0) {
            ShowResetAlertView();
            return;
        }

        AudioManager.Instance.PlayKeyStartEffect();
        if (m_gameView == null) {
            m_gameView = new HightAndLowGameView();
        }
        ViewManager.Instance.Push(m_gameView);
    }

    void ShowResetAlertView() {
        m_resetPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(m_resetBtn.gameObject);
    }

    void HideResetAlertView() {
        m_resetPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(m_startBtn.gameObject);
    }

    void OnClickResetButton() {
        HideResetAlertView();
        AppConfig.Instance.CheckedPokers = new List<int>() { };
        OnClickStartButton();
    }

    void OnClickCancelButton() {
        HideResetAlertView();
        
        AudioManager.Instance.PlayKeyStartEffect();
        if (m_gameView == null) {
            m_gameView = new HightAndLowGameView();
        }
        ViewManager.Instance.Push(m_gameView);
    }
}