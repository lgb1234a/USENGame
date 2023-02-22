using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayGameView : IViewOperater
{
    public static string __IS_REOPEN__ = "__IS_REOPEN__";
    string m_prefabPath = "GamePanel";
    GameObject m_viewGameObject;
    Button m_stopButton;
    Button m_exitButton;
    Transform m_pausePanel;

    public PlayGameView() {
        var obj = Resources.Load<GameObject>(m_prefabPath);
        m_viewGameObject = GameObject.Instantiate<GameObject>(obj, Vector3.zero, Quaternion.identity, ViewManager.Instance.GetRootTransform());

        m_stopButton = m_viewGameObject.transform.Find("PlayPanel/PausePanel/StopButton").GetComponent<Button>();
        m_stopButton.onClick.AddListener(OnClickStopButton);

        m_exitButton = m_viewGameObject.transform.Find("PlayPanel/PausePanel/ExitButton").GetComponent<Button>();
        m_exitButton.onClick.AddListener(OnClickExitButton);

        m_pausePanel = m_viewGameObject.transform.Find("PlayPanel/PausePanel");
    }


    public void Show() {
        m_viewGameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(m_stopButton.gameObject);
    }

    public void Hide() {
        m_viewGameObject.SetActive(false);
        m_pausePanel.gameObject.SetActive(false);
        ViewManager.Instance.Pop();
    }

    public void Update() {
        if (Input.GetButtonDown("Cancel")) {
            m_pausePanel.gameObject.SetActive(!m_pausePanel.gameObject.activeSelf);
        }
    }

    public void OnClickExitButton() {
        PreferencesStorage.SaveBoolean(__IS_REOPEN__, false);
        Hide();
    }

    public void OnClickStopButton() {
        PreferencesStorage.SaveBoolean(__IS_REOPEN__, true);
        Hide();
    }
}
