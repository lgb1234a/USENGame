using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CheckAnimator : MonoBehaviour
{
    const float __default_speed__ = 6000;
    public TMPro.TextMeshProUGUI m_Text_0;
    public TMPro.TextMeshProUGUI m_Text_1;
    public TMPro.TextMeshProUGUI m_Text_2;
    bool m_isAnimate = false;
    int m_cellHeight = 600;
    public float m_speed = __default_speed__;
    public float m_easeSpeed = 1000;
    private bool m_waitingBingo;
    private float m_delayInterval = 0;
    bool m_easeMode = false;
    GameDataHandler m_gameData;
    void Start()
    {
        
    }

    void Update()
    {
        if (m_isAnimate)
        {
            var position = m_Text_0.gameObject.transform.localPosition;
            position.y -= m_speed * Time.deltaTime;
            if (position.y <= -m_cellHeight) {
                position.y = m_cellHeight*2;
                m_Text_0.text = m_gameData.GetUncheckedNumber().ToString();
            }
            m_Text_0.gameObject.transform.localPosition = position;

            position = m_Text_1.gameObject.transform.localPosition;
            position.y -= m_speed * Time.deltaTime;
            if (position.y <= -m_cellHeight) {
                position.y = m_cellHeight*2;
                m_Text_1.text = m_gameData.GetUncheckedNumber().ToString();
            }
            m_Text_1.gameObject.transform.localPosition = position;

            position = m_Text_2.gameObject.transform.localPosition;
            position.y -= m_speed * Time.deltaTime;
            if (position.y <= -m_cellHeight) {
                position.y = m_cellHeight*2;
                m_Text_2.text = m_gameData.GetUncheckedNumber().ToString();
            }
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
                    var numChecked = t.GetComponent<TMPro.TextMeshProUGUI>().text;
                    m_gameData.SetCellChecked(int.Parse(numChecked) - 1);
                    
                    m_delayInterval += Time.deltaTime;
                    if (m_delayInterval > 2) {
                        m_easeMode = false;
                        m_waitingBingo = false;
                        m_delayInterval = 0;
                    }
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
        m_waitingBingo = true;
        m_speed = m_speed * 0.8f;
        yield return new WaitForSeconds(0.5f);
        m_speed = m_speed * 0.6f;
        yield return new WaitForSeconds(2f);
        m_speed = m_speed * 0.5f;
        yield return new WaitForSeconds(4f);
        m_isAnimate = false;
        m_easeMode = true;
    }

    public void Animate(GameDataHandler gameData) {
        m_gameData = gameData;
        if (!m_waitingBingo) {
            if (m_isAnimate) {
                StartCoroutine(EaseToEndCheck());
            } else {
                m_speed = __default_speed__;
                m_isAnimate = true;
            }
        }
    }
}
