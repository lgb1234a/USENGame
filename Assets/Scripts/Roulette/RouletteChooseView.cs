using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;

public class RouletteChooseView : AbstractView, IViewOperater
{
    string m_prefabPath = "RouletteChoosePanel";
    Text m_titleLabel;
    Button Btn1;
    Text BtnLabel1;
    Button Btn2;
    Text BtnLabel2;
    Button Btn3;
    Text BtnLabel3;
    Button Btn4;
    Text BtnLabel4;
    Button Btn5;
    Text BtnLabel5;
    Button Btn6;
    Text BtnLabel6;
    RouletteGameView m_gameView;
    RouletteQuitView m_quitView;

    string m_title;
    public RouletteChooseView(string title) {
        m_title = title;
    }

    public void Build()
    {
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());

        m_titleLabel = m_mainViewGameObject.transform.Find("Title").GetComponent<Text>();
        m_titleLabel.text = m_title;

        Btn1 = m_mainViewGameObject.transform.Find("Btn1").GetComponent<Button>();
        BtnLabel1 = m_mainViewGameObject.transform.Find("Btn1/Text").GetComponent<Text>();
        BtnLabel1.text = m_title + "1";
        Btn1.onClick.AddListener(OnClickedBtn1);

        Btn2 = m_mainViewGameObject.transform.Find("Btn2").GetComponent<Button>();
        BtnLabel2 = m_mainViewGameObject.transform.Find("Btn2/Text").GetComponent<Text>();
        BtnLabel2.text = m_title + "2";
        Btn2.onClick.AddListener(OnClickedBtn2);

        Btn3 = m_mainViewGameObject.transform.Find("Btn3").GetComponent<Button>();
        BtnLabel3 = m_mainViewGameObject.transform.Find("Btn3/Text").GetComponent<Text>();
        BtnLabel3.text = m_title + "3";
        Btn3.onClick.AddListener(OnClickedBtn3);

        Btn4 = m_mainViewGameObject.transform.Find("Btn4").GetComponent<Button>();
        BtnLabel4 = m_mainViewGameObject.transform.Find("Btn4/Text").GetComponent<Text>();
        BtnLabel4.text = m_title + "4";
        Btn4.onClick.AddListener(OnClickedBtn4);

        Btn5 = m_mainViewGameObject.transform.Find("Btn5").GetComponent<Button>();
        BtnLabel5 = m_mainViewGameObject.transform.Find("Btn5/Text").GetComponent<Text>();
        BtnLabel5.text = m_title + "5";
        Btn5.onClick.AddListener(OnClickedBtn5);

        Btn6 = m_mainViewGameObject.transform.Find("Btn6").GetComponent<Button>();
        BtnLabel6 = m_mainViewGameObject.transform.Find("Btn6/Text").GetComponent<Text>();
        BtnLabel6.text = m_title + "6";
        Btn6.onClick.AddListener(OnClickedBtn6);
    }

    public void Hide()
    {
        m_mainViewGameObject.SetActive(false);
    }

    public void OnAndroidKeyDown(string keyName)
    {
        if (keyName == "blue")
        {
            if (m_quitView == null) {
                m_quitView = new RouletteQuitView();
            }
            ViewManager.Instance.Push(m_quitView);
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

        // if (Input.GetKeyDown(KeyCode.A)) {
        //     //测试
        //     if (m_faqView == null) {
        //         m_faqView = new HighAndLowFAQView();
        //     }
        //     ViewManager.Instance.Push(m_faqView);
        // }
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