using UnityEngine;
using UnityEngine.UI;

public class AwardNumObj : MonoBehaviour
{
    private string m_text;
    public string text
    {
        get
        {
            return m_text;
        }
        set
        {
            m_text = value;
            lastNumImage.gameObject.SetActive(value.Length == 2);
            if (first != NumberTextureTool.Instance.GetAwardNumSprite(int.Parse(m_text.Substring(0, 1)))) {
                first = NumberTextureTool.Instance.GetAwardNumSprite(int.Parse(m_text.Substring(0, 1)));
            }
            firstNumImage.overrideSprite = first;
            if (value.Length == 2) {
                if (last != NumberTextureTool.Instance.GetAwardNumSprite(int.Parse(m_text.Substring(1, 1)))) {
                    last = NumberTextureTool.Instance.GetAwardNumSprite(int.Parse(m_text.Substring(1, 1)));
                    lastNumImage.overrideSprite = last;
                }
            }
        }
    }

    private Sprite first;
    private Sprite last;
    [SerializeField]
    private Image firstNumImage;

    [SerializeField]
    private Image lastNumImage;
}
