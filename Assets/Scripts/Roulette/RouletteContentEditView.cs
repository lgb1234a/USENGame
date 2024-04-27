using UnityEngine;

public class RouletteContentEditView : AbstractView, IViewOperater
{
    string m_prefabPath = "Roulette/RouletteContentEditPanel";
    HighAndLowRouletteView m_faqView;
    public void Build()
    {
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());

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
        // EventSystem.current.SetSelectedGameObject(Btn1.gameObject);
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
}