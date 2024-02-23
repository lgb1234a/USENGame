using UnityEngine;
using UnityEngine.UI;

public class AwardNumObj : MonoBehaviour
{
    private string m_text = "0";
    public string text
    {
        get
        {
            return m_text;
        }
        set
        {
            m_text = value;
            UpdateTextTheme();
            // lastNumImage.gameObject.SetActive(value.Length == 2);
            // var firstNumberValue = int.Parse(m_text.Substring(0, 1));
            // var firstSprite = await ThemeResManager.Instance.GetRotateNumberTexture(firstNumberValue);
            // // 切换主题时需要重设，因此注释掉
            // if (!first || first.name != firstSprite.name) {
            //     first = firstSprite;
            //     firstNumImage.sprite = first;
            // }
            

            // if (value.Length == 2) {
            //     var lastNumberValue = int.Parse(m_text.Substring(1, 1));
            //     var lastSprite = ThemeResManager.Instance.GetRotateNumberTexture(lastNumberValue);
            //     if (!last || last.name != lastSprite.name) {
            //         last = lastSprite;
            //         lastNumImage.sprite = last;
            //     }
            // }
        }
    }

    public async void UpdateTextTheme() {
        lastNumImage.gameObject.SetActive(m_text.Length == 2);
        var firstNumberValue = int.Parse(m_text.Substring(0, 1));
        var firstSprite = await ThemeResManager.Instance.GetRotateNumberTexture(firstNumberValue);
        first = firstSprite;
        firstNumImage.sprite = first;
        

        if (m_text.Length == 2) {
            var lastNumberValue = int.Parse(m_text.Substring(1, 1));
            var lastSprite = await ThemeResManager.Instance.GetRotateNumberTexture(lastNumberValue);
            last = lastSprite;
            lastNumImage.sprite = last;
        }
    }

    private Sprite first;
    private Sprite last;
    [SerializeField]
    private Image firstNumImage;

    [SerializeField]
    private Image lastNumImage;
}
