using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RouletteGameQuitView : AbstractView, IViewOperater
{
    string m_prefabPath = "RouletteGameQuitPanel";
    Button m_btn1;
    Button m_btn2;
    RouletteHomeView m_homeView;

    public void Build()
    {
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());
        m_btn1 = m_mainViewGameObject.transform.Find("Btn1").GetComponent<Button>();
        m_btn1.onClick.AddListener(ShowRouletteHomeView);
        m_btn2 = m_mainViewGameObject.transform.Find("Btn2").GetComponent<Button>();
        m_btn2.onClick.AddListener(showGameEntriesView);
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
        EventSystem.current.SetSelectedGameObject(m_btn1.gameObject);
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            USENSceneManager.Instance.LoadScene("GameEntries");
        }
    }

    public void ShowRouletteHomeView() {
        if (m_homeView == null) {
            m_homeView = new RouletteHomeView();
        }
        ViewManager.Instance.Push(m_homeView);
    }

    public void showGameEntriesView() {
        USENSceneManager.Instance.LoadScene("GameEntries");
    }
}