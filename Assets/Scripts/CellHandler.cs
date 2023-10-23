using UnityEngine;
using UnityEngine.UI;

public class CellHandler : MonoBehaviour
{
    public TMPro.TextMeshProUGUI m_NumberLabel;
    public Image m_UncheckedBg;
    public Image m_CheckedBg;
    private bool m_IsChecked = false;

    public void Init(int i) {
        gameObject.SetActive(true);
        m_NumberLabel.text = $"{i+1}";

        var position = transform.localPosition;
        position.z = 0;
        transform.localPosition = position;
        transform.localRotation = Quaternion.identity;
    }

    public void Checked() {
        if (m_IsChecked) return;
        m_IsChecked = true;
        m_CheckedBg.gameObject.SetActive(true);
        m_NumberLabel.color = ThemeResManager.Instance.GetNumberCheckedTextColor();
    }

    public void Uncheck() {
        if (!m_IsChecked) return;
        m_IsChecked = false;
        m_CheckedBg.gameObject.SetActive(false);
        m_NumberLabel.color = Color.white;
    }

    public void UpdateTheme() {
        m_UncheckedBg.sprite = ThemeResManager.Instance.GetCellNumberBgTexture();
        m_CheckedBg.sprite = ThemeResManager.Instance.GetCellNumberCheckBgTexture();
        if (m_IsChecked) {
            m_NumberLabel.color = ThemeResManager.Instance.GetNumberCheckedTextColor();
        }else {
            m_NumberLabel.color = Color.white;
        }
        
    }
}