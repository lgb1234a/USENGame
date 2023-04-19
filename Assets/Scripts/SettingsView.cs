using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsView : IViewOperater
{
    string m_prefabPath = "SettingsPanel";
    GameObject m_viewGameObject;
    Slider m_BGMSlider;
    Text m_BGMVolumeText;
    Slider m_maxCellSettingSlider;
    Text m_maxCellSettingText;

    public SettingsView() {
        var obj = Resources.Load<GameObject>(m_prefabPath);
        m_viewGameObject = GameObject.Instantiate<GameObject>(obj, Vector3.zero, Quaternion.identity, ViewManager.Instance.GetRootTransform());
        var position = m_viewGameObject.transform.localPosition;
        position.z = 0;
        m_viewGameObject.transform.localPosition = position;

        m_maxCellSettingSlider = m_viewGameObject.transform.Find("Panel/CenterPanel/MaxCellSetting/Slider").GetComponent<Slider>();
        m_maxCellSettingText = m_viewGameObject.transform.Find("Panel/CenterPanel/MaxCellSetting/MaxCellCount").GetComponent<Text>();
        m_maxCellSettingSlider.onValueChanged.AddListener(OnMaxCellSettingSliderValueChanged);
        m_maxCellSettingSlider.value = AppConfig.Instance.MaxCellCount;

        m_BGMSlider = m_viewGameObject.transform.Find("Panel/CenterPanel/BGMSettings/Slider").GetComponent<Slider>();
        m_BGMVolumeText = m_viewGameObject.transform.Find("Panel/CenterPanel/BGMSettings/VolumeText").GetComponent<Text>();
        m_BGMSlider.onValueChanged.AddListener(OnBGMSliderValueChanged);
        m_BGMSlider.value = AppConfig.Instance.BGMVolume;
        EventSystem.current.SetSelectedGameObject(m_maxCellSettingSlider.gameObject);
    }


    public void Show() {
        m_viewGameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(m_maxCellSettingSlider.gameObject);
    }

    public void Hide() {
        m_viewGameObject.SetActive(false);
        ViewManager.Instance.Pop();
    }

    public void Update() {
        if (Input.GetButtonDown("Cancel")) {
            Hide();
        }
    }

    public void OnAndroidKeyDown(string keyName) {
        
    }

    public void OnClickBackHomeButton() {
        Hide();
    }

    public void OnBGMSliderValueChanged(float value) {
        m_BGMVolumeText.text = Mathf.FloorToInt(value).ToString();
        AppConfig.Instance.BGMVolume = Mathf.FloorToInt(value);
    }

    public void OnMaxCellSettingSliderValueChanged(float value) {
        m_maxCellSettingText.text = Mathf.FloorToInt(value).ToString();
        AppConfig.Instance.MaxCellCount = Mathf.FloorToInt(value);
    }
}
