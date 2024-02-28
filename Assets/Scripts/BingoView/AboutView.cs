using UnityEngine;

public class AboutView : AbstractView, IViewOperater
{
    string m_prefabPath = "AboutPanel";

    public void Build() {
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());
        // m_mainViewGameObject = GameObject.Instantiate<GameObject>(obj, Vector3.zero, Quaternion.identity, ViewManager.Instance.GetRootTransform());
        var position = m_mainViewGameObject.transform.localPosition;
        position.z = 0;
        m_mainViewGameObject.transform.localPosition = position;
    }

    public void Show() {
        m_mainViewGameObject.SetActive(true);
    }

    public void Hide() {
        m_mainViewGameObject.SetActive(false);
    }

    public void Update() {
        if (Input.GetButtonDown("Cancel")) {
            Hide();
            ViewManager.Instance.Hided(this);
        }
    }

    public void OnAndroidKeyDown(string keyName) {
        
    }

    public void OnClickBackHomeButton() {
        Hide();
        ViewManager.Instance.Hided(this);
    }

    public void OnThemeTypeChanged() {

    }
}
