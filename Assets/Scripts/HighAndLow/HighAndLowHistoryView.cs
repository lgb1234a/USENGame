using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;
using System.Collections.Generic;
using System.Linq;

public class HighAndLowHistoryView : AbstractView, IViewOperater
{
    string m_prefabPath = "HighAndLow/HighAndLowHistoryPanel";
    Text m_restCountLabel;
    Button m_backButton;
    List<GameObject> m_itemList = new();
    List<EPokers> m_checkedPokers;

    public HighAndLowHistoryView(List<EPokers> checkedPokers) {
        m_checkedPokers = checkedPokers;
    }

    public void Build()
    {
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());

        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 13; j++) {
                var itemPath = string.Format("CheckList/{0}_{1}", i, j);
                m_itemList.Add(m_mainViewGameObject.transform.Find(itemPath).gameObject);
            }
        }

        foreach (var poker in m_checkedPokers)
        {
            var index = EPokersHelper.GetIndexOfPoker(poker);
            m_itemList.ElementAt(index).GetComponent<Image>().color = Color.white;
        }

        m_restCountLabel = m_mainViewGameObject.transform.Find("RestCountLabel").GetComponent<Text>();
        m_restCountLabel.text = string.Format("{0}", 52 - m_checkedPokers.Count());
        m_backButton = m_mainViewGameObject.transform.Find("BottomPanel/BackBtn").GetComponent<Button>();
        m_backButton.onClick.AddListener(OnClickedBackButton);
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
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            Hide();
            ViewManager.Instance.Hided(this);
        }
    }

    void OnClickedBackButton() {
        Hide();
        ViewManager.Instance.Hided(this);
    }
}