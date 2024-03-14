using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEntry : MonoBehaviour
{
    [SerializeField]
    private Button m_entryButton;
    private Action<GameEntry> m_gameEntryDelegate;
    public CanvasGroup m_selectedBg;
    bool m_isWillQuitApplication;
    public int m_index;
    void Start()
    {
        var eventTrigger = m_entryButton.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener(OnGameEntrySelected);
        eventTrigger.triggers.Add(entry);

        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        entry1.eventID = EventTriggerType.Deselect;
        entry1.callback.AddListener(OnGameEntryDeselected);
        eventTrigger.triggers.Add(entry1);

        m_entryButton.onClick.AddListener(OnGameEntryClicked);
    }

    void Update()
    {
        
    }

    void OnGUI()
    {

    }

    void OnGameEntrySelected(BaseEventData data)
    {
        // m_gameEntryDelegate(this);
        m_selectedBg.DOFade(1f, 0.2f).SetEase(Ease.InOutSine).SetLink(m_selectedBg.gameObject);
    }

    void OnGameEntryDeselected(BaseEventData data)
    {
        m_selectedBg.DOFade(0f, 0.2f).SetEase(Ease.InOutSine).SetLink(m_selectedBg.gameObject);
    }

    void OnGameEntryClicked()
    {
        if (m_index == 2 || m_index == 1) 
        {
            USENSceneManager.Instance.LoadScene("Home");
            AppConfig.Instance.SelectedGameIndex = m_index;
        } 
    }

    public void AddSelectListener(Action<GameEntry> d) 
    {
        m_gameEntryDelegate = d;
    }

    public void SetEventSystemSelected()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void ResetRotate() {
        (transform as RectTransform).pivot = new Vector2(0.5f, 0.5f);
        (transform as RectTransform).rotation = Quaternion.identity;
    }

    public void SetPositionX(float x) {
        (transform as RectTransform).localPosition = new Vector3(x, 0, 0);
    }

    public void SetPivot(Vector2 pivot) {
        (transform as RectTransform).pivot = pivot;
    }

    public void SetRotate(float rotateY) {
        (transform as RectTransform).DOLocalRotate(new Vector3(0, rotateY, 0), 0.5f).SetLink(gameObject);
    }

    public void SetSiblingIndex(int index) {
        transform.SetSiblingIndex(index);
    }
}
