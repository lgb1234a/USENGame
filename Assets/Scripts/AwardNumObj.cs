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
            var firstNumberValue = int.Parse(m_text.Substring(0, 1));
            var firstSprite = ThemeResManager.Instance.GetRotateNumberTexture(firstNumberValue);
            // if (!first || first.name != firstSprite.name) {
                first = firstSprite;
                firstNumImage.sprite = first;
            // }
            

            if (value.Length == 2) {
                var lastNumberValue = int.Parse(m_text.Substring(1, 1));
                var lastSprite = ThemeResManager.Instance.GetRotateNumberTexture(lastNumberValue);
                // if (!last || last.name != lastSprite.name) {
                    last = lastSprite;
                    lastNumImage.sprite = last;
                // }
            }
        }
    }

    public void UpdateTextTheme() {
        text = m_text;
    }

    private Sprite first;
    private Sprite last;
    [SerializeField]
    private Image firstNumImage;

    [SerializeField]
    private Image lastNumImage;
}
