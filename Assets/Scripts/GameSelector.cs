using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameSelector : MonoBehaviour
{
    public GameEntry[] m_gameEntries;
    public int m_selectIndex = 2;
    int m_betweenSelectedSpace = 600;
    int m_otherSpace = 200;
    float m_keyDownTimeInterval = 0.5f;
    float m_timeInterval = 0;

    void Start()
    {
        for (int i = 0; i < m_gameEntries.Length; i++)
            m_gameEntries[i].m_index = i;
        m_selectIndex = AppConfig.Instance.SelectedGameIndex;
        m_gameEntries[m_selectIndex].SetEventSystemSelected();
        UpdateSelectedIndex(m_selectIndex);
        m_gameEntries[m_selectIndex].m_selectedBg.alpha = 1;
    }

    void Update()
    {
        m_timeInterval += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftArrow) && m_timeInterval > m_keyDownTimeInterval) {
            m_timeInterval = 0;
            if (m_selectIndex > 0) 
                UpdateSelectedIndex(--m_selectIndex);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && m_timeInterval > m_keyDownTimeInterval) {
            m_timeInterval = 0;
            if (m_selectIndex < 4)
                UpdateSelectedIndex(++m_selectIndex);
        }
    }

    void UpdateSelectedIndex(int index) {
        m_selectIndex = index;
        var selectedPositionX = 0;
        for (int i = 0; i < m_gameEntries.Length; i++) {
            m_gameEntries[i].SetPositionX((i-2)*300);
            if (i == m_selectIndex) {
                m_gameEntries[i].ResetRotate();
                m_gameEntries[i].SetPositionX(selectedPositionX);
                m_gameEntries[i].SetEventSystemSelected();
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
