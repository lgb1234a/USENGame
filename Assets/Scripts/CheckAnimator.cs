using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

public class CheckAnimator : MonoBehaviour
{
    public TMPro.TextMeshProUGUI m_Text_0;
    public TMPro.TextMeshProUGUI m_Text_1;
    public TMPro.TextMeshProUGUI m_Text_2;
    bool m_isAnimate = false;
    int m_cellHeight = 600;
    public float m_speed = 5000;
    public float m_easeSpeed = 1000;
    bool m_easeMode = false;
    GameDataHandler m_gameData;
    void Start()
    {
        
    }

    void Update()
    {
        if (m_isAnimate)
        {
            var nums = m_gameData.GetRandomCellNumber();
            int i = 0;
            foreach (var item in nums)
            {
                if (i == 0) m_Text_0.text = item.ToString();
                if (i == 1) m_Text_1.text = item.ToString();
                if (i == 2) m_Text_2.text = item.ToString();
                i++;
            }
            var position = m_Text_0.gameObject.transform.localPosition;
            position.y -= m_speed * Time.deltaTime;
            if (position.y <= -m_cellHeight) position.y = m_cellHeight*2;
            m_Text_0.gameObject.transform.localPosition = position;

            m_gameData.GetRandomCellNumber().GetEnumerator();
            position = m_Text_1.gameObject.transform.localPosition;
            position.y -= m_speed * Time.deltaTime;
            if (position.y <= -m_cellHeight) position.y = m_cellHeight*2;
            m_Text_1.gameObject.transform.localPosition = position;

            m_gameData.GetRandomCellNumber().GetEnumerator();
            position = m_Text_2.gameObject.transform.localPosition;
            position.y -= m_speed * Time.deltaTime;
            if (position.y <= -m_cellHeight) position.y = m_cellHeight*2;
            m_Text_2.gameObject.transform.localPosition = position;
        }

        if (m_easeMode) {
            var transforms = new List<Transform> {m_Text_0.gameObject.transform, 
                                m_Text_1.gameObject.transform, 
                                m_Text_2.gameObject.transform};
            foreach (var t in transforms)
            {
                var position = t.localPosition;
                var tempY = position.y;
                position.y -= m_easeSpeed * Time.deltaTime;
                if (tempY >= 0 && position.y <= 0) {
                    position.y = 0;
                    m_easeMode = false;
                }
                if (tempY >= m_cellHeight &&  position.y <= m_cellHeight) {
                    position.y = m_cellHeight;
                }
                if (tempY >= -m_cellHeight && position.y <= -m_cellHeight) {
                    position.y = -m_cellHeight;
                }
                t.localPosition = position;
            }
        }
    }

    IEnumerator<WaitForSeconds> EaseToEndCheck() {
        m_speed = m_speed * 0.8f;
        yield return new WaitForSeconds(0.5f);
        m_speed = m_speed * 0.6f;
        yield return new WaitForSeconds(2f);
        m_speed = m_speed * 0.5f;
        yield return new WaitForSeconds(4f);
        m_easeMode = true;
        m_isAnimate = false;
    }

    public void Animate(GameDataHandler gameData) {
        m_gameData = gameData;
        if (m_isAnimate) {
            for(int i = 0; i < 4; i++) {
                StartCoroutine(EaseToEndCheck());
            }
        } else {
            m_isAnimate = true;
        }
    }
}
