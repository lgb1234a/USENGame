using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;

public class HighAndLowHomeView : AbstractView, IViewOperater
{
    string m_prefabPath = "HighAndLow/HighAndLowHomePanel";
    Button m_startBtn;
    HightAndLowGameView m_gameView;
    public void Build()
    {
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());

        m_startBtn = m_mainViewGameObject.transform.Find("StartBtn").GetComponent<Button>();
        m_startBtn.onClick.AddListener(OnClickStartButton);
        AudioManager.Instance.PlayLowAndHighBGM();
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
        AudioManager.Instance.PlayKeyStartEffect();
        if (m_gameView == null) {
            m_gameView = new HightAndLowGameView();
        }
        ViewManager.Instance.Push(m_gameView);
    }
}