using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameDataHandler
{
    public int m_cellCount = 70;
    public List<bool> m_cellCheckedList;
    public List<int> m_checkedNumbers = new List<int>(75);
    public int[] m_playNumberSequence;

    public GameDataHandler(int cellCount) {
        m_cellCount = cellCount;
        m_cellCheckedList = new List<bool>(cellCount){};
        for(int i = 0; i < cellCount; i++) {
            m_cellCheckedList.Add(false);
        }
    }

    public void SetCellChecked(int index) {
        if (index < 0) return;
        m_cellCheckedList[index] = true;
        m_checkedNumbers.Add(index);
    }

    public bool IsCellChecked(int index) {
        return m_cellCheckedList[index];
    }

    public void ResetAllCellChecked() {
        for (int i = 0; i < m_cellCheckedList.Count; i++) {
            m_cellCheckedList[i] = false;
        }
    }

    public int GetUncheckedNumber() {
        var validNumbers = new List<int>();
        for (int i = 0; i < m_cellCount; i++)
            if (!m_checkedNumbers.Contains(i))
                validNumbers.Add(i+1);
                
        var index = UnityEngine.Random.Range(0, validNumbers.Count-1);
        if (index >= validNumbers.Count)
            return 0;
        return validNumbers[index];
    }
}
