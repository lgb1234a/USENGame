using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    Transform m_resetPanel;
    Button m_resetBtn;
    Text m_resetBtnText;
    Button m_resetCancelBtn;
    Text m_resetCancelBtnText;
    RectTransform m_numberPanel;
    GameObject m_bottomBackButton;
    GameObject m_numberPanelTitle;
    CheckAnimator m_checkAnimator;
    CanvasGroup m_maskCanvasGroup;

    Transform m_numberCellTemplate;
    List<CellHandler> m_numberCells;
    public GameDataHandler m_gameData;

    bool m_playRotationAnim = false;
    bool m_playRotationAnimBack = false;

    Button m_rotateTestButton;
    Text m_rotateTestButtonText;
    GameObject m_rotateBackGO;
    Button m_playButton;
    Text m_playButtonText;
    Button m_playbackButton;
    Button m_redButton;
    Button m_greenButton;
    CanvasGroup m_playbackCanvasGroup;
    Image m_topDecorate;
    CellHandler m_numberCellHandler;
    Transform m_qiqiuEffectPanel;
    SkeletonGraphic m_qiqiuSpineSkeletonGraphic;
    Transform m_bingoEffectPanel;
    SkeletonGraphic m_bingoSpineSkeletonGraphic;
    Transform m_rotateBgEffectPanel;
    SkeletonGraphic m_rotateBgEffect;

    int m_reachCount = 0;
    int m_bingoCount = 0;
    bool m_canPlayBingoAnim = true;
    double m_playBingoInterval = 0;

    private string bgSkeletonName = "bg";
    private string bgRirekiSkeletonName = "bg_rireki";
    private double m_LoadingInterval = 0;

    Sequence m_transformSequence = DOTween.Sequence();
    public PlayGameView() {
        var obj = Resources.Load<GameObject>(m_prefabPath);
        m_viewGameObject = GameObject.Instantiate<GameObject>(obj, Vector3.zero, Quaternion.identity, ViewManager.Instance.GetRootTransform());
        var position = m_viewGameObject.transform.localPosition;
        position.z = 0;
        m_viewGameObject.transform.localPosition = position;

        m_qiqiuEffectPanel = m_viewGameObject.transform.Find("QiqiuEffect");
        InitMqiqiuSpineSkeletonGraphic();

        m_bingoEffectPanel = m_viewGameObject.transform.Find("PlayPanel/BingoEffectPanel");
        m_bingoSpineSkeletonGraphic = ThemeResManager.Instance.InstantiateBingoSpineGameObject(m_bingoEffectPanel).GetComponent<SkeletonGraphic>();
        m_bingoSpineSkeletonGraphic.AnimationState.Complete += OnPlayComplete;

        m_rotateBgEffectPanel = m_viewGameObject.transform.Find("PlayPanel/Game/AwardPanel/BgEffect");
        m_rotateBgEffect = ThemeResManager.Instance.InstantiateRotateBgSpineGameObject(m_rotateBgEffectPanel).GetComponent<SkeletonGraphic>();
        m_rotateBgEffect.AnimationState.SetAnimation(0, "panel_blue", true);

        m_topDecorate = m_viewGameObject.transform.Find("TopDecorate").GetComponent<Image>();
        m_topDecorate.sprite = ThemeResManager.Instance.GetThemePlayViewDecorateTexture();

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


        m_resetPanel = m_viewGameObject.transform.Find("PlayPanel/ResetPanel");
        m_resetBtn = m_viewGameObject.transform.Find("PlayPanel/ResetPanel/ResetBtn").GetComponent<Button>();
        m_resetBtnText = m_viewGameObject.transform.Find("PlayPanel/ResetPanel/ResetBtn/Text").GetComponent<Text>();
        m_resetBtn.onClick.AddListener(OnClickedResetBtn);
        m_resetCancelBtn = m_viewGameObject.transform.Find("PlayPanel/ResetPanel/CancelBtn").GetComponent<Button>();
        m_resetCancelBtnText = m_viewGameObject.transform.Find("PlayPanel/ResetPanel/CancelBtn/Text").GetComponent<Text>();
        m_resetCancelBtn.onClick.AddListener(OnClickedResetCancelBtn);

        var awardPanelTransform = m_viewGameObject.transform.Find("PlayPanel/Game/AwardPanel");
        m_checkAnimator = awardPanelTransform.GetComponent<CheckAnimator>();
        m_maskCanvasGroup = awardPanelTransform.GetComponent<CanvasGroup>();

        m_numberPanel = m_viewGameObject.transform.Find("PlayPanel/Game/NumberPanel") as RectTransform;
        m_numberPanelTitle = m_numberPanel.Find("Title").gameObject;
        m_numberCellTemplate = m_viewGameObject.transform.Find("PlayPanel/Game/NumberPanel/NumberCell");
        m_numberCellHandler = m_numberCellTemplate.GetComponent<CellHandler>();
        m_numberCellHandler.UpdateTheme();

        m_bottomBackButton = m_viewGameObject.transform.Find("PlayPanel/BottomPanel/Button1").gameObject;

        m_rotateTestButton = m_viewGameObject.transform.Find("PlayPanel/BottomPanel/Button5").GetComponent<Button>();
        m_rotateTestButtonText = m_viewGameObject.transform.Find("PlayPanel/BottomPanel/Button5/Text").GetComponent<Text>();
        m_rotateTestButton.onClick.AddListener(OnClickYellowButton);
        m_rotateBackGO = m_viewGameObject.transform.Find("PlayPanel/BottomPanel/Button5/BackImg").gameObject;

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

        HandleSelectedEventTriggers();
    }

    private void InitMqiqiuSpineSkeletonGraphic() {
        m_qiqiuSpineSkeletonGraphic = ThemeResManager.Instance.InstantiateHomeSpineGameObject(m_qiqiuEffectPanel).GetComponent<SkeletonGraphic>();
        m_qiqiuSpineSkeletonGraphic.AnimationState.SetAnimation(0, bgSkeletonName, true);
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

        var eventTrigger3 = m_resetBtn.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry6 = new EventTrigger.Entry();
        entry6.eventID = EventTriggerType.Select;
        entry6.callback.AddListener(OnResetGameButtonSelected);
        eventTrigger3.triggers.Add(entry6);

        EventTrigger.Entry entry7 = new EventTrigger.Entry();
        entry7.eventID = EventTriggerType.Deselect;
        entry7.callback.AddListener(OnResetGameButtonDeselected);
        eventTrigger3.triggers.Add(entry7);

        var eventTrigger4 = m_resetCancelBtn.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry8 = new EventTrigger.Entry();
        entry8.eventID = EventTriggerType.Select;
        entry8.callback.AddListener(OnCancelResetGameButtonSelected);
        eventTrigger4.triggers.Add(entry8);

        EventTrigger.Entry entry9 = new EventTrigger.Entry();
        entry9.eventID = EventTriggerType.Deselect;
        entry9.callback.AddListener(OnCancelResetGameButtonDeselected);
        eventTrigger4.triggers.Add(entry9);
    }

    public void Show() {
        if (!ViewManager.Instance.IsLoadingShow()) {
            m_viewGameObject.SetActive(true);
        }
        AppConfig.Instance.rotateEaseExtraTime = 0.0f;
        m_numberCellHandler.UpdateTheme();
        m_checkAnimator.ResetCheckTexts();

        if (AppConfig.Instance.GameData != null) {
            m_gameData = AppConfig.Instance.GameData;
            if (m_gameData.m_cellCount != AppConfig.Instance.MaxCellCount) {
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


        if (m_numberCells == null) {
            m_numberCells = new List<CellHandler>();
            int i = 0;
            foreach (var cell in GenerateNumberCells())
            {
                m_numberCells.Add(cell);
                if (m_gameData.IsCellChecked(i)) {
                    cell.Checked();
                }else
                {
                    cell.Uncheck();
                }
                i++;
            }
        }
    }

    public void Hide() {
        m_viewGameObject.SetActive(false);
        m_pausePanel.gameObject.SetActive(false);
        AudioManager.Instance.StopNumberRotateEffect();
    }

    IEnumerable<CellHandler> GenerateNumberCells() {
        for(int i = 0; i < m_gameData.m_cellCount; i++) {
            var cellHandler = GameObject.Instantiate<GameObject>(
                m_numberCellTemplate.gameObject, 
                Vector3.zero,
                Quaternion.identity,
                m_numberCellTemplate.parent).GetComponent<CellHandler>();
            cellHandler.Init(i);
            yield return cellHandler;
        }
    }

    public void Update() {
        if (ViewManager.Instance.IsLoadingShow()) {
            m_LoadingInterval += Time.deltaTime;
        }
        if (m_LoadingInterval > 1) {
            m_viewGameObject.SetActive(true);
        }
        if (m_LoadingInterval > 2) {
            ViewManager.Instance.HideLoading();
            m_LoadingInterval = 0;
        }
        if (!m_canPlayBingoAnim) {
            m_playBingoInterval += Time.deltaTime;
            if (m_playBingoInterval > 3.3) {
                m_canPlayBingoAnim = true;
                m_playBingoInterval = 0;
            }
        }

        if (m_numberCells != null) {
            for (int i = 0; i < m_numberCells.Count; i++)
            {
                var cell = m_numberCells[i];
                if (m_gameData.IsCellChecked(i)) {
                    cell.Checked();
                }else
                {
                    cell.Uncheck();
                }
            }
        }

        if (Input.GetButtonDown("Cancel")) {
            if (IsShowHistory()) {
                m_playRotationAnimBack = true;
                HideNumberPanelTitle();
                Show();

                m_rotateBackGO.SetActive(false);
                m_rotateTestButtonText.text = "履歴";
            }
            else {
                m_checkAnimator.ForceStop();
                OnClickStopButton();
            }
        }

        if (Input.GetKeyDown(KeyCode.Menu) || Input.GetKeyDown(KeyCode.A)) {
            m_pausePanel.gameObject.SetActive(!IsShowPausePanel());
            EventSystem.current.SetSelectedGameObject(m_backGameButton.gameObject);
        }

        if (m_playRotationAnim) {
            var color = m_rotateBgEffect.color;
            color.a = 0;
            m_transformSequence.Join(m_numberPanel.DOAnchorPosX(-410, 1f)).Join(m_numberPanel.DOLocalRotateQuaternion(Quaternion.identity, 1f)).Join(m_maskCanvasGroup.DOFade(0, 1f)).Join(m_playbackCanvasGroup.DOFade(1, 1f)).Join(m_rotateBgEffect.DOColor(color, 1f));
            m_playRotationAnim = false;
            EventSystem.current.SetSelectedGameObject(m_playbackButton.gameObject);
        }

        if (m_playRotationAnimBack) {
            var color = m_rotateBgEffect.color;
            color.a = 1;
            m_transformSequence.Join(m_numberPanel.DOAnchorPosX(0, 1f)).Join(m_numberPanel.DOLocalRotateQuaternion(Quaternion.Euler(0, 30, 0), 1f)).Join(m_maskCanvasGroup.DOFade(1, 1f)).Join(m_playbackCanvasGroup.DOFade(0, 1f)).Join(m_rotateBgEffect.DOColor(color, 1f));
            m_playRotationAnimBack = false;
            EventSystem.current.SetSelectedGameObject(null);
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
        if (!m_canPlayBingoAnim) return;
        if (m_playButtonText.text == "ストップ")
        {
            m_playButton.transform.GetComponent<CanvasGroup>().alpha = 0.5f;
        }

        if (m_gameData.IsAllChecked()) {
            // 显示弹窗
            ShowResetPanel();
        }else {
            m_checkAnimator.Animate(m_gameData);
            m_playButtonText.text = "ストップ";
        }
    }

    public void UpdatePlayButtonText() {
        if (m_checkAnimator.isAnimteFinished() && m_playButtonText.text != "シャッフル")
        {
            m_playButtonText.text = "シャッフル";
            m_playButton.transform.GetComponent<CanvasGroup>().alpha = 1.0f;
        }
    }

    public void OnClickExitButton() {
        ResetData();
        Hide();
        ViewManager.Instance.Hided(this);
    }

    public void ResetData() {
        PreferencesStorage.SaveString(AppConfig.__REOPEN_DATA__, null);
        m_rotateBgEffect.AnimationState.SetAnimation(0, "panel_blue", true);
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

    public void ShowResetPanel() {
        m_resetPanel.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(m_resetBtn.gameObject);
    }

    public void HideResetPanel() {
        m_resetPanel.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(m_playButton.gameObject);
    }

    public void OnClickedResetBtn() {
        ResetData();
        AppConfig.Instance.ClearGameData();
        Show();
        HideResetPanel();
    }

    public void OnClickedResetCancelBtn() {
        HideResetPanel();
    }

    void OnClickRotateButton() {
        m_playRotationAnim = true;
        ShowNumberPanelTitle();
    }

    void OnClickPlayBackButton() {
        m_playRotationAnimBack = true;
        HideNumberPanelTitle();
        // reset
        AppConfig.Instance.ClearGameData();
        Show();

        m_rotateTestButtonText.text = "履歴";
        m_rotateBackGO.SetActive(false);
        // reset music & effect
        AudioManager.Instance.PlayDefaultBgm();
        m_rotateBgEffect.AnimationState.SetAnimation(0, "panel_blue", true);
    }

    void OnClickRedButton() {
        if (m_checkAnimator.isAnimating()) return;
        if (!m_canPlayBingoAnim) return;
        AudioManager.Instance.PlayReachClickEffect();
        AudioManager.Instance.PlayWillReachBgm();
        m_bingoSpineSkeletonGraphic.AnimationState.SetAnimation(0, "reach", false);
        m_rotateBgEffect.AnimationState.SetAnimation(0, "carcle_puple", true);
        m_reachCount++;

        m_canPlayBingoAnim = false;
        AppConfig.Instance.rotateEaseExtraTime = 3.0f;
    }

    void OnClickGreenButton() {
        if (m_checkAnimator.isAnimating()) return;
        if (!m_canPlayBingoAnim) return;
        AudioManager.Instance.PlayBingoEffect();

        if (m_reachCount != 0) {
            m_bingoCount++;
        }
        
        if (m_bingoCount == m_reachCount) {
            AudioManager.Instance.PlayDefaultBgm(1f);
            m_rotateBgEffect.AnimationState.SetAnimation(0, "panel_blue", true);
        }
        
        m_bingoSpineSkeletonGraphic.AnimationState.SetAnimation(0, "bingo", false);

        m_canPlayBingoAnim = false;
        AppConfig.Instance.rotateEaseExtraTime = 0.0f;
    }

    void OnClickYellowButton() 
    {
        if (m_checkAnimator.isAnimating()) return;

        if (m_numberPanel.localRotation == Quaternion.identity)
        {
            // back but not reset
            m_playRotationAnimBack = true;
            HideNumberPanelTitle();
            m_rotateBackGO.SetActive(false);
            m_rotateTestButtonText.text = "履歴";
            m_bottomBackButton.SetActive(true);
        }
        else if (m_numberPanel.localRotation == Quaternion.Euler(0, 30, 0))
        {
            OnClickRotateButton();
            m_rotateBackGO.SetActive(true);
            m_rotateTestButtonText.text = "戻る";
            m_bottomBackButton.SetActive(false);
        }
    }

    private void OnPlayComplete(Spine.TrackEntry entry)
    {
        m_bingoSpineSkeletonGraphic.AnimationState.AddEmptyAnimation(0, 0, 0);
    }

    void ShowNumberPanelTitle() {
        m_numberPanelTitle.SetActive(true);

        m_redButton.gameObject.SetActive(false);
        m_greenButton.gameObject.SetActive(false);
        m_playButton.gameObject.SetActive(false);

        m_qiqiuSpineSkeletonGraphic.AnimationState.SetAnimation(0, bgRirekiSkeletonName, true);
    }

    void HideNumberPanelTitle() {
        m_numberPanelTitle.SetActive(false);

        m_redButton.gameObject.SetActive(true);
        m_greenButton.gameObject.SetActive(true);
        m_playButton.gameObject.SetActive(true);

        m_qiqiuSpineSkeletonGraphic.AnimationState.SetAnimation(0, bgSkeletonName, true);
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

    void OnResetGameButtonSelected(BaseEventData data)
    {
        m_resetBtnText.color = Color.white;
        m_resetCancelBtnText.color = new Color(0f, 147/255f, 1.0f, 1.0f);
    }

    void OnResetGameButtonDeselected(BaseEventData data)
    {
        m_resetBtnText.color = new Color(0f, 147/255f, 1.0f, 1.0f);
    }

    void OnCancelResetGameButtonSelected(BaseEventData data)
    {
        m_resetCancelBtnText.color = Color.white;
        m_resetBtnText.color = new Color(0f, 147/255f, 1.0f, 1.0f);
    }

    void OnCancelResetGameButtonDeselected(BaseEventData data)
    {
        m_resetCancelBtnText.color = new Color(0f, 147/255f, 1.0f, 1.0f);
    }

    void OnBackGameButtonDeselected(BaseEventData data) {
        m_backGameButtonText.color = new Color(0f, 147/255f, 1.0f, 1.0f);
    }

    public void OnThemeTypeChanged() {
        m_topDecorate.sprite = ThemeResManager.Instance.GetThemePlayViewDecorateTexture();
        m_numberCellHandler.UpdateTheme();
        // 左边转转转数字更新
        m_checkAnimator.UpdateAwardNumTheme();
        // 转转转下面的底盘更新
        GameObject.Destroy(m_qiqiuSpineSkeletonGraphic);
        InitMqiqiuSpineSkeletonGraphic();

        for(int i = 0; i < m_numberCells.Count; i++) {
            var cell = m_numberCells[i];
            var numberImage = cell.GetComponent<CellHandler>();
            numberImage.UpdateTheme();
        }

        GameObject.Destroy(m_rotateBgEffect.gameObject);
        m_rotateBgEffectPanel.DetachChildren();
        m_rotateBgEffect = ThemeResManager.Instance.InstantiateRotateBgSpineGameObject(m_rotateBgEffectPanel).GetComponent<SkeletonGraphic>();
        if (AudioManager.Instance.m_isWillReach) {
            m_rotateBgEffect.AnimationState.SetAnimation(0, "carcle_puple", true);
        }else {
            m_rotateBgEffect.AnimationState.SetAnimation(0, "panel_blue", true);
        }

        GameObject.Destroy(m_bingoSpineSkeletonGraphic.gameObject);
        m_bingoEffectPanel.DetachChildren();
        m_bingoSpineSkeletonGraphic = ThemeResManager.Instance.InstantiateBingoSpineGameObject(m_bingoEffectPanel).GetComponent<SkeletonGraphic>();
        m_bingoSpineSkeletonGraphic.AnimationState.Complete += OnPlayComplete;
    }
}
