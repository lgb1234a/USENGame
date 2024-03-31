using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class KeyBoardSelector : MonoBehaviour
{
    public GameObject m_selectedGO;
    public Text m_text;
    public Color m_selectedTextColor;
    Color m_textDefaultColor;
    EventTrigger m_eventTrigger;
    void Awake() {
        m_textDefaultColor = m_text.color;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_eventTrigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener(OnSelected);
        m_eventTrigger.triggers.Add(entry);

        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        entry1.eventID = EventTriggerType.Deselect;
        entry1.callback.AddListener(OnDeselected);
        m_eventTrigger.triggers.Add(entry1);
    }

    public void SetSelected() {
        OnSelected(null);
    }

    void OnSelected(BaseEventData data)
    {
        if (m_selectedGO != null)
            m_selectedGO.gameObject.SetActive(true);

        if (m_text != null)
            m_text.color = m_selectedTextColor;
    }

    void OnDeselected(BaseEventData data)
    {
        if (m_selectedGO != null)
            m_selectedGO.gameObject.SetActive(false);
        if (m_text != null)
            m_text.color = m_textDefaultColor;
    }
}
