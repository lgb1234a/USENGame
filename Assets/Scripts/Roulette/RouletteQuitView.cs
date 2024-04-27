using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;

public class RouletteQuitView : AbstractView, IViewOperater
{
    string m_prefabPath = "Roulette/RouletteQuitPanel";
    Button Btn1;
    Button Btn2;
    HighAndLowRouletteView m_faqView;
    HightAndLowGameView m_gameView;
    public void Build()
    {
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());

        Btn1 = m_mainViewGameObject.transform.Find("Btn1").GetComponent<Button>();
        Btn1.onClick.AddListener(OnClickedBtn1);

        Btn2 = m_mainViewGameObject.transform.Find("Btn2").GetComponent<Button>();
        Btn2.onClick.AddListener(OnClickedBtn2);
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
                m_faqView = new HighAndLowRouletteView();
            }
            ViewManager.Instance.Push(m_faqView);
        }

        if (keyName == "yellow")
        {
            
        }
    }

    public void OnThemeTypeChanged()
    {
        
    }

    public void Show()
    {
        m_mainViewGameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(Btn1.gameObject);
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            USENSceneManager.Instance.LoadScene("GameEntries");
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            //测试
            if (m_faqView == null) {
                m_faqView = new HighAndLowRouletteView();
            }
            ViewManager.Instance.Push(m_faqView);
        }
    }

    public void OnClickedBtn1()
    {
        if (m_gameView == null) {
            m_gameView = new HightAndLowGameView();
        }
        ViewManager.Instance.Push(m_gameView);
    }

    public void OnClickedBtn2()
    {
        if (m_gameView == null) {
            m_gameView = new HightAndLowGameView();
        }
        ViewManager.Instance.Push(m_gameView);
    }
}