using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;

public class RouletteOriginView : AbstractView, IViewOperater
{
    string m_prefabPath = "Roulette/RouletteOriginPanel";
    string m_title;
    Button Btn1;
    Button Btn2;
    Button Btn3;
    Button Btn4;
    Button Btn5;
    Button Btn6;
    RouletteEditView m_editView;
    RouletteGameView m_gameView;

    public RouletteOriginView(string title) {
        m_title = title;
    }

    public void Build()
    {
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());

        Btn1 = m_mainViewGameObject.transform.Find("Btn1").GetComponent<Button>();
        Btn1.onClick.AddListener(OnClickedBtn1);

        Btn2 = m_mainViewGameObject.transform.Find("Btn2").GetComponent<Button>();
        Btn2.onClick.AddListener(OnClickedBtn2);

        Btn3 = m_mainViewGameObject.transform.Find("Btn3").GetComponent<Button>();
        Btn3.onClick.AddListener(OnClickedBtn3);

        Btn4 = m_mainViewGameObject.transform.Find("Btn4").GetComponent<Button>();
        Btn4.onClick.AddListener(OnClickedBtn4);

        Btn5 = m_mainViewGameObject.transform.Find("Btn5").GetComponent<Button>();
        Btn5.onClick.AddListener(OnClickedBtn5);

        Btn6 = m_mainViewGameObject.transform.Find("Btn6").GetComponent<Button>();
        Btn6.onClick.AddListener(OnClickedBtn6);
    }

    public void Hide()
    {
        m_mainViewGameObject.SetActive(false);
    }

    public void OnAndroidKeyDown(string keyName)
    {
        if (keyName == "red")
        {
            if (m_editView == null) {
                m_editView = new RouletteEditView();
            }
            ViewManager.Instance.Push(m_editView);
        }

        if (keyName == "blue")
        {
            if (m_editView == null) {
                m_editView = new RouletteEditView(true);
            }
            ViewManager.Instance.Push(m_editView);
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
            if (m_editView == null) {
                m_editView = new RouletteEditView();
            }
            ViewManager.Instance.Push(m_editView);
        }

        if (Input.GetKeyDown(KeyCode.B)) {
            //测试
            if (m_editView == null) {
                m_editView = new RouletteEditView(true);
            }
            ViewManager.Instance.Push(m_editView);
        }
    }

    public void OnClickedBtn1()
    {
        ShowGameView();
    }

    public void OnClickedBtn2()
    {
        ShowGameView();
    }

    public void OnClickedBtn3()
    {
        ShowGameView();
    }

    public void OnClickedBtn4()
    {
        ShowGameView();
    }

    public void OnClickedBtn5()
    {
        ShowGameView();
    }

    public void OnClickedBtn6()
    {
        ShowGameView();
    }

    void ShowGameView() {
        if (m_gameView == null) {
            m_gameView = new RouletteGameView(m_title);
        }
        ViewManager.Instance.Push(m_gameView);
    }
}