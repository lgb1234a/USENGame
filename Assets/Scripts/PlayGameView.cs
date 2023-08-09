using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
using Spine.Unity;

public class PlayGameView : IViewOperater
{
    string m_prefabPath = "GamePanel";
    GameObject m_viewGameObject;
    Button m_stopButton;
    Text m_stopButtonText;
    Button m_exitButton;
    Text m_exitButtonText;
    Button m_backGameButton;
    Text m_backGameButtonText;
    Transform m_pausePanel;
    RectTransform m_numberPanel;
    GameObject m_numberPanelTitle;
    CheckAnimator m_checkAnimator;
    CanvasGroup m_maskCanvasGroup;

    Transform m_numberCellTemplate;
    List<Transform> m_numberCells;
    public GameDataHandler m_gameData;

    bool m_playRotationAnim = false;
    bool m_playRotationAnimBack = false;

    Button m_rotateTestButton;
    Button m_playButton;
    Text m_playButtonText;
    Button m_playbackButton;
    Button m_redButton;
    Button m_greenButton;
    CanvasGroup m_playbackCanvasGroup;
    SkeletonGraphic m_sg;
    SkeletonGraphic m_bgEffect;

    Sequence m_transformSequence = DOTween.Sequence();
    public PlayGameView() {
        var obj = Resources.Load<GameObject>(m_prefabPath);
        m_viewGameObject = GameObject.Instantiate<GameObject>(obj, Vector3.zero, Quaternion.identity, ViewManager.Instance.GetRootTransform());
        var position = m_viewGameObject.transform.localPosition;
        position.z = 0;
        m_viewGameObject.transform.localPosition = position;

        m_stopButton = m_viewGameObject.transform.Find("PlayPanel/PausePanel/StopButton").GetComponent<Button>();
        m_stopButtonText = m_viewGameObject.transform.Find("PlayPanel/PausePanel/StopButton/Text").GetComponent<Text>();
        m_stopButton.onClick.AddListener(OnClickStopButton);

        m_exitButton = m_viewGameObject.transform.Find("PlayPanel/PausePanel/ExitButton").GetComponent<Button>();
        m_exitButtonText = m_viewGameObject.transform.Find("PlayPanel/PausePanel/ExitButton/Text").GetComponent<Text>();
        m_exitButton.onClick.AddListener(OnClickExitButton);

        m_backGameButton = m_viewGameObject.transform.Find("PlayPanel/PausePanel/BackGameButton").GetComponent<Button>();
        m_backGameButtonText = m_viewGameObject.transform.Find("PlayPanel/PausePanel/BackGameButton/Text").GetComponent<Text>();
        m_backGameButton.onClick.AddListener(OnClickBackGameButton);

        m_pausePanel = m_viewGameObject.transform.Find("PlayPanel/PausePanel");
        var awardPanelTransform = m_viewGameObject.transform.Find("PlayPanel/Game/AwardPanel");
        m_checkAnimator = awardPanelTransform.GetComponent<CheckAnimator>();
        m_maskCanvasGroup = awardPanelTransform.GetComponent<CanvasGroup>();

        m_numberPanel = m_viewGameObject.transform.Find("PlayPanel/Game/NumberPanel") as RectTransform;
        m_numberPanelTitle = m_numberPanel.Find("Title").gameObject;
        m_numberCellTemplate = m_viewGameObject.transform.Find("PlayPanel/Game/NumberPanel/NumberCell");

        m_rotateTestButton = m_viewGameObject.transform.Find("PlayPanel/BottomPanel/Button5").GetComponent<Button>();
        m_rotateTestButton.onClick.AddListener(OnClickYellowButton);

        m_playButton = m_viewGameObject.transform.Find("PlayPanel/BottomPanel/Button2").GetComponent<Button>();
        m_playButton.onClick.AddListener(OnClickPlayButton);
        m_playButtonText = m_viewGameObject.transform.Find("PlayPanel/BottomPanel/Button2/Text").GetComponent<Text>();

        m_redButton = m_viewGameObject.transform.Find("PlayPanel/BottomPanel/Button3").GetComponent<Button>();
        m_redButton.onClick.AddListener(OnClickRedButton);
        m_greenButton = m_viewGameObject.transform.Find("PlayPanel/BottomPanel/Button4").GetComponent<Button>();
        m_greenButton.onClick.AddListener(OnClickGreenButton);

        m_playbackButton = m_viewGameObject.transform.Find("PlayPanel/Game/PlayBackButton").GetComponent<Button>();
        m_playbackCanvasGroup = m_playbackButton.GetComponent<CanvasGroup>();
        m_playbackButton.onClick.AddListener(OnClickPlayBackButton);

        m_sg = m_viewGameObject.transform.Find("PlayPanel/EffectPanel").GetComponent<SkeletonGraphic>();
        m_sg.AnimationState.Complete += OnPlayComplete;
        m_bgEffect = m_viewGameObject.transform.Find("PlayPanel/Game/AwardPanel/BgEffect").GetComponent<SkeletonGraphic>();
        HandleSelectedEventTriggers();
    }

    void HandleSelectedEventTriggers() {
        var eventTrigger = m_stopButton.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener(OnStopButtonSelected);
        eventTrigger.triggers.Add(entry);

        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        entry1.eventID = EventTriggerType.Deselect;
        entry1.callback.AddListener(OnStopButtonDeselected);
        eventTrigger.triggers.Add(entry1);

        var eventTrigger1 = m_exitButton.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.Select;
        entry2.callback.AddListener(OnExitButtonSelected);
        eventTrigger1.triggers.Add(entry2);

        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.Deselect;
        entry3.callback.AddListener(OnExitButtonDeselected);
        eventTrigger1.triggers.Add(entry3);

        var eventTrigger2 = m_backGameButton.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry4 = new EventTrigger.Entry();
        entry4.eventID = EventTriggerType.Select;
        entry4.callback.AddListener(OnBackGameButtonSelected);
        eventTrigger2.triggers.Add(entry4);

        EventTrigger.Entry entry5 = new EventTrigger.Entry();
        entry5.eventID = EventTriggerType.Deselect;
        entry5.callback.AddListener(OnBackGameButtonDeselected);
        eventTrigger2.triggers.Add(entry5);
    }

    public void Show() {
        m_viewGameObject.SetActive(true);

        if (AppConfig.Instance.GameData != null) {
            m_gameData = AppConfig.Instance.GameData;
            if (m_gameData.m_cellCount != AppConfig.Instance.MaxCellCount) {
                m_checkAnimator.ResetCheckTexts();
                m_gameData = new GameDataHandler(AppConfig.Instance.MaxCellCount);
                AppConfig.Instance.GameData = m_gameData;

                if (m_numberCells != null) {
                    foreach (var cell in m_numberCells)
                    {
                        UnityEngine.Object.Destroy(cell.gameObject);
                    }
                    m_numberCells = null;
                }
            }
        }else {
            m_checkAnimator.ResetCheckTexts();
            m_gameData = new GameDataHandler(AppConfig.Instance.MaxCellCount);
            AppConfig.Instance.GameData = m_gameData;
        }


        if (m_numberCells == null) {
            m_numberCells = new List<Transform>();
            int i = 0;
            foreach (var cell in GenerateNumberCells())
            {
                m_numberCells.Add(cell);
                cell.Find("CheckBg").gameObject.SetActive(m_gameData.IsCellChecked(i));
                i++;
            }
        }
    }

    public void Hide() {
        m_viewGameObject.SetActive(false);
        m_pausePanel.gameObject.SetActive(false);
    }

    IEnumerable<Transform> GenerateNumberCells() {
        var cells = new List<Transform>(m_gameData.m_cellCount);
        for(int i = 0; i < m_gameData.m_cellCount; i++) {
            var cell = GameObject.Instantiate<GameObject>(
                m_numberCellTemplate.gameObject, 
                Vector3.zero,
                Quaternion.identity,
                m_numberCellTemplate.parent);
            cell.gameObject.SetActive(true);
            cell.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = $"{i+1}";
            var position = cell.transform.localPosition;
            position.z = 0;
            cell.transform.localPosition = position;
            cell.transform.localRotation = Quaternion.identity;
            yield return cell.transform;
        }
    }

    public void Update() {

        if (m_numberCells != null) {
            for (int i = 0; i < m_numberCells.Count; i++)
            {
                var cell = m_numberCells[i];
                cell.Find("CheckBg").gameObject.SetActive(m_gameData.IsCellChecked(i));
            }
        }

        if (Input.GetButtonDown("Cancel")) {
            if (IsShowHistory()) {
                m_playRotationAnimBack = true;
                HideNumberPanelTitle();
                Show();
            }else {
                m_checkAnimator.ForceStop();
                OnClickStopButton();
            }
        }

        if (Input.GetKeyDown(KeyCode.Menu) || Input.GetKeyDown(KeyCode.A)) {
            m_pausePanel.gameObject.SetActive(!IsShowPausePanel());
            EventSystem.current.SetSelectedGameObject(m_backGameButton.gameObject);
        }

        if (m_playRotationAnim) {
            var color = m_bgEffect.color;
            color.a = 0;
            m_transformSequence.Join(m_numberPanel.DOAnchorPosX(-410, 1f)).Join(m_numberPanel.DOLocalRotateQuaternion(Quaternion.identity, 1f)).Join(m_maskCanvasGroup.DOFade(0, 1f)).Join(m_playbackCanvasGroup.DOFade(1, 1f)).Join(m_bgEffect.DOColor(color, 1f));
            m_playRotationAnim = false;
            EventSystem.current.SetSelectedGameObject(m_playbackButton.gameObject);
        }

        if (m_playRotationAnimBack) {
            var color = m_bgEffect.color;
            color.a = 1;
            m_transformSequence.Join(m_numberPanel.DOAnchorPosX(0, 1f)).Join(m_numberPanel.DOLocalRotateQuaternion(Quaternion.Euler(0, 30, 0), 1f)).Join(m_maskCanvasGroup.DOFade(1, 1f)).Join(m_playbackCanvasGroup.DOFade(0, 1f)).Join(m_bgEffect.DOColor(color, 1f));
            m_playRotationAnimBack = false;
        }

        // green bingo anim  KEYCODE_PROG_GREEN  399
        // red  398
        // yellow 400
        // blue
        // if (Input.GetKeyDown(KeyCode.Return)) {
        //     m_checkAnimator.Animate(m_gameData);
        // }

        UpdatePlayButtonText();
    }

    public void OnAndroidKeyDown(string keyName) {
        if (IsShowPausePanel()) return;

        if (keyName == "blue") {
            if (IsShowHistory()) return;
            if (!m_playButton.gameObject.activeSelf) return;
            OnClickPlayButton();
        } else if (keyName == "green") {
            if (IsShowHistory()) return;
            OnClickGreenButton();
        } else if (keyName == "red") {
            if (IsShowHistory()) return;
            OnClickRedButton();
        } else if (keyName == "yellow") {
            OnClickYellowButton();
        }
    }

    public void OnClickPlayButton() {
        m_checkAnimator.Animate(m_gameData);
        m_playButtonText.text = "ストップ";
    }

    public void UpdatePlayButtonText() {
        if (m_checkAnimator.isAnimteFinished() && m_playButtonText.text != "シャッフル")
            m_playButtonText.text = "シャッフル";
    }

    public void OnClickExitButton() {
        ResetData();
        Hide();
        ViewManager.Instance.Hided(this);
    }

    public void ResetData() {
        PreferencesStorage.SaveString(AppConfig.__REOPEN_DATA__, null);
        m_bgEffect.AnimationState.SetAnimation(0, "panel_blue", true);
        AudioManager.Instance.PlayDefaultBgm();
    }

    public void OnClickStopButton() {
        AppConfig.Instance.GameData = m_gameData;
        Hide();
        ViewManager.Instance.Hided(this);
    }

    public void OnClickBackGameButton() {
        m_pausePanel.gameObject.SetActive(false);
    }

    void OnClickRotateButton() {
        m_playRotationAnim = true;
        EventSystem.current.SetSelectedGameObject(m_playbackButton.gameObject);
        ShowNumberPanelTitle();
    }

    void OnClickPlayBackButton() {
        m_playRotationAnimBack = true;
        HideNumberPanelTitle();
        // reset
        AppConfig.Instance.ClearGameData();
        Show();
    }

    void OnClickRedButton() {
        AudioManager.Instance.PlayReachClickEffect();
        AudioManager.Instance.PlayWillReachBgm();
        m_sg.gameObject.SetActive(true);
        m_sg.AnimationState.SetAnimation(0, "reach", false);
        m_sg.AnimationState.AddEmptyAnimation(0, 0, 0);
        m_bgEffect.AnimationState.SetAnimation(0, "carcle_puple", true);
    }

    void OnClickGreenButton() {
        AudioManager.Instance.PlayBingoEffect();
        AudioManager.Instance.PlayDefaultBgm();
        m_sg.gameObject.SetActive(true);
        m_sg.AnimationState.SetAnimation(0, "bingo", false);
        m_sg.AnimationState.AddEmptyAnimation(0, 0, 0);
        m_bgEffect.AnimationState.SetAnimation(0, "panel_blue", true);
    }

    void OnClickYellowButton() 
    {
        if (m_checkAnimator.isAnimating()) return;
        
        if (m_numberPanel.localRotation == Quaternion.identity)
        {
            // back but not reset
            m_playRotationAnimBack = true;
            HideNumberPanelTitle();
            OnClickPlayBackButton();
        }
        else if (m_numberPanel.localRotation == Quaternion.Euler(0, 30, 0))
        {
            OnClickRotateButton();
        }
    }

    private void OnPlayComplete(Spine.TrackEntry entry)
    {
        m_sg.AnimationState.ClearTracks();
    }

    void ShowNumberPanelTitle() {
        m_numberPanelTitle.SetActive(true);

        m_redButton.gameObject.SetActive(false);
        m_greenButton.gameObject.SetActive(false);
        m_playButton.gameObject.SetActive(false);
        // m_rotateTestButton.gameObject.SetActive(false);
    }

    void HideNumberPanelTitle() {
        m_numberPanelTitle.SetActive(false);

        m_redButton.gameObject.SetActive(true);
        m_greenButton.gameObject.SetActive(true);
        m_playButton.gameObject.SetActive(true);
        // m_rotateTestButton.gameObject.SetActive(true);
    }

    bool IsShowHistory() {
        return m_numberPanel.localRotation != Quaternion.Euler(0, 30, 0);
    }

    bool IsShowPausePanel() {
        return m_pausePanel.gameObject.activeSelf;
    }

    void OnStopButtonSelected(BaseEventData data) {
        m_stopButtonText.color = Color.white;
    }

    void OnStopButtonDeselected(BaseEventData data) {
        m_stopButtonText.color = new Color(0f, 147/255f, 1.0f, 1.0f);
    }

    void OnExitButtonSelected(BaseEventData data) {
        m_exitButtonText.color = Color.white;
    }

    void OnExitButtonDeselected(BaseEventData data) {
        m_exitButtonText.color = new Color(0f, 147/255f, 1.0f, 1.0f);
    }

    void OnBackGameButtonSelected(BaseEventData data) {
        m_backGameButtonText.color = Color.white;
        m_exitButtonText.color = new Color(0f, 147/255f, 1.0f, 1.0f);
        m_stopButtonText.color = new Color(0f, 147/255f, 1.0f, 1.0f);
    }

    void OnBackGameButtonDeselected(BaseEventData data) {
        m_backGameButtonText.color = new Color(0f, 147/255f, 1.0f, 1.0f);
    }
}
