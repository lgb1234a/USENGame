using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;

public class RouletteEditView : AbstractView, IViewOperater
{
    string m_prefabPath = "Roulette/RouletteEditPanel";

    InputField m_titleInputField;
    InputField m_itemInputField1;
    InputField m_itemInputField2;
    InputField m_itemInputField3;
    InputField m_itemInputField4;
    Button m_startAndSaveBtn;
    Button m_saveAndQuitBtn;
    Button m_deleteBtn;
    HighAndLowRouletteView m_faqView;
    HightAndLowGameView m_gameView;
    bool m_canDelete;

    public RouletteEditView(bool canDelete = false) {
        m_canDelete = canDelete;
    }

    public void Build()
    {
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());

        m_titleInputField = m_mainViewGameObject.transform.Find("TitleInputField").GetComponent<InputField>();
        m_itemInputField1 = m_mainViewGameObject.transform.Find("Item1/ItemContentInput").GetComponent<InputField>();
        m_itemInputField2 = m_mainViewGameObject.transform.Find("Item2/ItemContentInput").GetComponent<InputField>();
        m_itemInputField3 = m_mainViewGameObject.transform.Find("Item3/ItemContentInput").GetComponent<InputField>();
        m_itemInputField4 = m_mainViewGameObject.transform.Find("Item4/ItemContentInput").GetComponent<InputField>();

        m_startAndSaveBtn = m_mainViewGameObject.transform.Find("BottomPanel/SaveAndStartBtn").GetComponent<Button>();
        m_startAndSaveBtn.onClick.AddListener(OnClickedStartAndSaveBtn);
        m_saveAndQuitBtn = m_mainViewGameObject.transform.Find("BottomPanel/SaveAndQuitBtn").GetComponent<Button>();
        m_saveAndQuitBtn.onClick.AddListener(OnClickedSaveAndQuitBtn);
        m_deleteBtn = m_mainViewGameObject.transform.Find("BottomPanel/DeleteBtn").GetComponent<Button>();
        m_deleteBtn.onClick.AddListener(OnClickedDeleteBtn);
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
    }

    public void OnThemeTypeChanged()
    {
        
    }

    public void Show()
    {
        m_mainViewGameObject.SetActive(true);
        // EventSystem.current.SetSelectedGameObject(m_btn1.gameObject);
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



    public void OnClickedStartAndSaveBtn() {

    }

    public void OnClickedSaveAndQuitBtn() {

    }

    public void OnClickedDeleteBtn() {

    }
}