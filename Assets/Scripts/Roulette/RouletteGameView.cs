using UnityEngine;
using UnityEngine.UI;

public class RouletteGameView : AbstractView, IViewOperater
{
    string m_prefabPath = "RouletteGamePanel";
    string m_title;
    Button m_contentEditBtn;
    Button m_changeRouletteBtn;
    Button m_backBtn;
    RouletteHomeView m_homeView;
    RouletteGameQuitView m_gameQuitView;

    public RouletteGameView(string title) {
        m_title = title;
    }

    public void Build()
    {
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());

        m_contentEditBtn = m_mainViewGameObject.transform.Find("BottomPanel/ContentEditBtn").GetComponent<Button>();
        m_contentEditBtn.onClick.AddListener(OnClickedContentEditBtn);
        m_changeRouletteBtn = m_mainViewGameObject.transform.Find("BottomPanel/ChangeRouletteBtn").GetComponent<Button>();
        m_changeRouletteBtn.onClick.AddListener(OnClickedChangeRouletteBtn);
        m_backBtn = m_mainViewGameObject.transform.Find("BottomPanel/BackBtn").GetComponent<Button>();
        m_backBtn.onClick.AddListener(OnClickedBackBtn);
    }

    public void Hide()
    {
        m_mainViewGameObject.SetActive(false);
    }

    public void OnAndroidKeyDown(string keyName)
    {
        if (keyName == "red")
        {
            
        }

        if (keyName == "blue")
        {
            if (m_homeView == null) {
                m_homeView = new RouletteHomeView();
            }
            ViewManager.Instance.Push(m_homeView);
        }
    }

    public void OnThemeTypeChanged()
    {
        
    }

    public void Show()
    {
        m_mainViewGameObject.SetActive(true);
        // EventSystem.current.SetSelectedGameObject(Btn1.gameObject);
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            OnClickedBackBtn();
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            //测试
        }

        if (Input.GetKeyDown(KeyCode.B)) {
            //测试
            if (m_homeView == null) {
                m_homeView = new RouletteHomeView();
            }
            ViewManager.Instance.Push(m_homeView);
        }
    }

    public void OnClickedContentEditBtn() {

    }

    public void OnClickedChangeRouletteBtn() {
        if (m_homeView == null) {
            m_homeView = new RouletteHomeView();
        }
        ViewManager.Instance.Push(m_homeView);
    }

    public void OnClickedBackBtn() {
        if (m_gameQuitView == null) {
            m_gameQuitView = new RouletteGameQuitView();
        }
        ViewManager.Instance.Push(m_gameQuitView);
    }
}