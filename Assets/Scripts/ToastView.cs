using UnityEngine;
using System.Collections;

class ToastView : MonoBehaviourSingletonTemplate<ToastView>
{
    string m_prefabPath = "Toast";
    GameObject m_viewGameObject;

    void InstantiateGameObject() {
        var obj = Resources.Load<GameObject>(m_prefabPath);
        m_viewGameObject = GameObject.Instantiate<GameObject>(obj, Vector3.zero, Quaternion.identity, ViewManager.Instance.GetPopupTransform());
        var position = m_viewGameObject.transform.localPosition;
        position.z = 0;
        m_viewGameObject.transform.localPosition = position;
    }

    public void Show() {
        if (m_viewGameObject == null) {
            InstantiateGameObject();
        }

        if (m_viewGameObject.activeSelf) return;
        m_viewGameObject.SetActive(true);
        StartCoroutine(Hide());
    }

    IEnumerator Hide() {
        yield return new WaitForSeconds(2.0f);
        m_viewGameObject.SetActive(false);
    }

    public bool IsToasting() {
        if (m_viewGameObject == null) return false;
        return m_viewGameObject.activeSelf;
    }
}