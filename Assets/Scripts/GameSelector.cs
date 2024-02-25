using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameSelector : MonoBehaviour
{
    public GameEntry[] m_gameEntries;
    public int m_selectIndex = 2;
    int[] m_selectedPositionX = { -600, -200, 0, 200, 600 };
    int m_betweenSelectedSpace = 700;
    int m_otherSpace = 200;

    void Start()
    {
        foreach (var entry in m_gameEntries)
        {
            entry.AddSelectListener(OnGameEntrySelected);
        }
        m_gameEntries[m_selectIndex].SetEventSystemSelected();
        UpdateSelectedIndex(m_selectIndex);
    }

    void Update()
    {
        
    }

    void OnGameEntrySelected(GameEntry selectedEntry) 
    {
        var selectedIndex = m_selectIndex;
        for (int i = 0; i < m_gameEntries.Length; i++)
        {
            var entry = m_gameEntries[i];
            if (entry == selectedEntry)
                selectedIndex = i;
        }
        if (selectedIndex != m_selectIndex)
            UpdateSelectedIndex(selectedIndex);
    }

    void UpdateSelectedIndex(int index) {
        m_selectIndex = index;
        var selectedPositionX = m_selectedPositionX[m_selectIndex];
        for (int i = 0; i < m_gameEntries.Length; i++) {
            m_gameEntries[i].SetPositionX((i-2)*300);
            if (i == m_selectIndex) {
                m_gameEntries[i].ResetRotate();
                m_gameEntries[i].SetPositionX(selectedPositionX);
            }else
            {
                if (i-m_selectIndex < 0) {
                    m_gameEntries[i].SetPivot(new Vector2(0, 0.5f));
                }else
                {
                    m_gameEntries[i].SetPivot(new Vector2(1, 0.5f));
                }
                if (i < m_selectIndex)
                {
                    m_gameEntries[i].SetRotate(-40);
                }else {
                    m_gameEntries[i].SetRotate(40);
                }
                if (i < m_selectIndex) {
                    m_gameEntries[i].SetPositionX(selectedPositionX-m_betweenSelectedSpace-(m_selectIndex-i-1)*m_otherSpace);
                    m_gameEntries[i].SetSiblingIndex(i);
                }else {
                    m_gameEntries[i].SetPositionX(selectedPositionX+m_betweenSelectedSpace+(i-m_selectIndex-1)*m_otherSpace);
                    m_gameEntries[i].SetSiblingIndex(m_gameEntries.Length - i);
                }
            }
        }
    }
}
