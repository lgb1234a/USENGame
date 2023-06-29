using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CheckAnimator : MonoBehaviour
{
    const float __default_speed__ = 5;
    const int __cell_height__ = 500;
    public AwardNumObj[] m_Texts;
    public float m_speed = __default_speed__;
    bool m_isAnimate = false;
    bool m_isEaseToStop;
    private bool m_waitingBingo;
    GameDataHandler m_gameData;
    float[] m_progress = new[] {0f, 1f, 2f, 3f};
    Vector3[] m_animPositions = new[] {Vector3.up*__cell_height__*2, Vector3.up*__cell_height__, Vector3.zero, Vector3.down*__cell_height__};
    void Start()
    {
        
    }

    void Update()
    {
        if (!m_isAnimate)
        {
            return;
        }

        float t = Time.deltaTime * m_speed;
        for (int i = 0; i < m_Texts.Length; i++)
        {
            m_progress[i] += t;
            // Debug.Log(MoveNextPosition(i));
            m_Texts[i].gameObject.transform.localPosition = MoveNextPosition(i);
        }

    }

    Vector3 MoveNextPosition(int i) {
        int index = Mathf.FloorToInt(m_progress[i]);
        if (index > m_animPositions.Length - 2) {
            m_progress[i] -= index;
            index = 0;
            if (i == 2 && m_isEaseToStop) {
                m_isAnimate = false;
                StartCoroutine(WaitToTriggerNextRound());
                m_gameData.SetCellChecked(int.Parse(m_Texts[1].text)-1);
            }
            m_Texts[i].text = m_gameData.GetUncheckedNumber().ToString();
            return m_animPositions[index];
        }else {
            return Vector3.Lerp(m_animPositions[index], m_animPositions[index + 1], m_progress[i]-index);
        }
    }

    IEnumerator<WaitForSeconds> EaseAnimSpeed() {
        m_waitingBingo = true;
        m_speed = m_speed * 0.8f;
        yield return new WaitForSeconds(1f);
        m_speed = m_speed * 0.6f;
        yield return new WaitForSeconds(2f);
        m_speed = m_speed * 0.5f;
        yield return new WaitForSeconds(2f);
        m_isEaseToStop = true;
    }

    IEnumerator<WaitForSeconds> WaitToTriggerNextRound() {
        yield return new WaitForSeconds(1);
        m_waitingBingo = false;
    }

    public void Animate(GameDataHandler gameData) {
        m_gameData = gameData;
        if (!m_waitingBingo) {
            if (m_isAnimate) {
                StartCoroutine(EaseAnimSpeed());
            } else {
                m_speed = __default_speed__;
                m_isAnimate = true;
                m_isEaseToStop = false;
            }
        }
    }

    public void ResetCheckTexts() {
        foreach (var t in m_Texts) {
            t.text = "0";
        }
    }

    public bool isAnimteFinished() {
        return m_isEaseToStop;
    }
}
