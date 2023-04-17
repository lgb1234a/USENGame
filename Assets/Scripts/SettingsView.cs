using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsView : IViewOperater
{
    string m_prefabPath = "SettingsPanel";
    GameObject m_viewGameObject;
    Slider m_BGMSlider;
    Text m_BGMVolumeText;

    public SettingsView() {
        var obj = Resources.Load<GameObject>(m_prefabPath);
        m_viewGameObject = GameObject.Instantiate<GameObject>(obj, Vector3.zero, Quaternion.identity, ViewManager.Instance.GetRootTransform());
        var position = m_viewGameObject.transform.localPosition;
        position.z = 0;
        m_viewGameObject.transform.localPosition = position;

        m_BGMSlider = m_viewGameObject.transform.Find("Panel/CenterPanel/BGMSettings/Slider").GetComponent<Slider>();
        m_BGMSlider.onValueChanged.AddListener(OnBGMSliderValueChanged);
        m_BGMVolumeText = m_viewGameObject.transform.Find("Panel/CenterPanel/BGMSettings/VolumeText").GetComponent<Text>();
        EventSystem.current.SetSelectedGameObject(m_BGMSlider.gameObject);
    }


    public void Show() {
        m_viewGameObject.SetActive(true);
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
    }
}
