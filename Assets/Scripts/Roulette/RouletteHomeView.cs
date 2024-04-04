using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;

public class RouletteHomeView : AbstractView, IViewOperater
{
    string m_prefabPath = "RouletteHomePanel";
    Button Btn1;
    Button Btn2;
    Button Btn3;
    Button Btn4;
    Button Btn5;
    RouletteChooseView m_chooseView;
    RouletteOriginView m_originView;
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
        EventSystem.current.SetSelectedGameObject(Btn1.gameObject);
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            USENSceneManager.Instance.LoadScene("GameEntries");
        }

    }

    public void OnClickedBtn1()
    {
        ShowChooseView("罰ゲーム");
    }

    public void OnClickedBtn2()
    {
        ShowChooseView("トークテーマ（プライベート①）");
    }

    public void OnClickedBtn3()
    {
        ShowChooseView("トークテーマ（仕事系）");
    }

    public void OnClickedBtn4()
    {
        ShowChooseView("トークテーマ（プライベート②）");
    }

    void ShowChooseView(string title) {
        if (m_chooseView == null) {
            m_chooseView = new RouletteChooseView(title);
        }
        ViewManager.Instance.Push(m_chooseView);
    }

    public void OnClickedBtn5()
    {
        if (m_originView == null) {
            m_originView = new RouletteOriginView();
        }
        ViewManager.Instance.Push(m_originView);
    }
}