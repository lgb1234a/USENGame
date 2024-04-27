using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using System.Linq;
using DG.Tweening;

public class HightAndLowGameView : AbstractView, IViewOperater
{
    string m_prefabPath = "HighAndLow/HighAndLowGamePanel";
    HighAndLowHistoryView m_historyView;
    Transform m_pokerStartTransform;
    Transform m_pokerShowTransform1;
    Transform m_pokerShowTransform2;
    Transform m_pokerTrashTransform;
    Text m_checkRestCountLabel;
    List<GameObject> m_checkedItemList = new();
    List<EPokers> m_pokerPool = new();
    List<EPokers> m_checkedPokers = new();
    GameObject m_timer;
    bool m_waitTrigger;
    Text m_timeLabel;
    Button m_historyBtn;
    Button m_terminalBtn;
    Button m_rouletteBtn;
    Button m_winnerBtn;
    GameObject m_pokerTemplate;
    GameObject m_resultLow;
    bool m_resultIsShowing;
    bool m_isGameFinished;
    GameObject m_pokersTile;
    Coroutine m_updateTimeCoroutine;

    HighAndLowRouletteView m_rouletteView;
    HighAndLowHomeView m_homeView;
    HighAndLowTerminalView m_terminalView;
    public void Build()
    {
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());

        m_pokerStartTransform = m_mainViewGameObject.transform.Find("PokerStart");
        m_pokerShowTransform1 = m_mainViewGameObject.transform.Find("PokerShow1");
        m_pokerShowTransform2 = m_mainViewGameObject.transform.Find("PokerShow2");
        m_pokerTrashTransform = m_mainViewGameObject.transform.Find("PokerShowTrash");

        m_checkRestCountLabel = m_mainViewGameObject.transform.Find("PokerCheckList/RestCountLabel").GetComponent<Text>();
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 13; j++) {
                m_pokerPool.Add((EPokers)(i*16+j));
                var itemPath = string.Format("PokerCheckList/CheckedList/{0}_{1}", i, j);
                m_checkedItemList.Add(m_mainViewGameObject.transform.Find(itemPath).gameObject);
            }
        }

        m_timer = m_mainViewGameObject.transform.Find("Timer").gameObject;
        m_timeLabel = m_mainViewGameObject.transform.Find("Timer/TimeLabel").GetComponent<Text>();

        m_historyBtn = m_mainViewGameObject.transform.Find("BottomPanel/HistoryBtn").GetComponent<Button>();
        m_historyBtn.onClick.AddListener(OnClickedHistoryButton);
        m_terminalBtn = m_mainViewGameObject.transform.Find("BottomPanel/StopBtn").GetComponent<Button>();
        m_terminalBtn.onClick.AddListener(OnClickedTerminalBtn);
        m_rouletteBtn = m_mainViewGameObject.transform.Find("BottomPanel/RouletteBtn").GetComponent<Button>();
        m_rouletteBtn.onClick.AddListener(OnClickedRouletteBtn);
        m_winnerBtn = m_mainViewGameObject.transform.Find("BottomPanel/WinnerBtn").GetComponent<Button>();
        m_winnerBtn.onClick.AddListener(OnClickedWinnerBtn);

        m_pokerTemplate = m_mainViewGameObject.transform.Find("PokerTemplate").gameObject;
        m_resultLow = m_mainViewGameObject.transform.Find("ResultLow").gameObject;
        m_pokersTile = m_mainViewGameObject.transform.Find("Bg/PokerTile").gameObject;
    }

    public override void OnDestroy() {
        if (m_updateTimeCoroutine != null)
            ViewManager.Instance.StopCoroutine(m_updateTimeCoroutine);
        m_timeLabel = null;
        m_mainViewGameObject = null;
        m_checkedItemList.Clear();
        m_pokerPool.Clear();
        m_checkedPokers.Clear();
    }

    public void Hide()
    {
        m_mainViewGameObject.SetActive(false);
    }

    public void OnAndroidKeyDown(string keyName)
    {
        if (keyName == "blue")
        {
            OnClickedHistoryButton();
        }
    }

    public void OnThemeTypeChanged()
    {
        
    }

    public void Show()
    {
        m_mainViewGameObject.SetActive(true);
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            if (m_homeView == null)
                m_homeView = new HighAndLowHomeView();
            ViewManager.Instance.Push(m_homeView);
        }

        if (m_waitTrigger) {
            if (Input.GetKeyDown(KeyCode.UpArrow)) {

                m_waitTrigger = false;
            }else if (Input.GetKeyDown(KeyCode.DownArrow)) {

                m_waitTrigger = false;
            }
        }

        if (!m_isGameFinished) {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit")) {
                if (m_pokerShowTransform1.childCount == 0) {
                    PlayFirst();
                }else if (m_resultIsShowing){
                    // 将牌丢弃
                    PlayLoop();
                    HideResultButtons();
                    m_resultIsShowing = false;
                }
            }
        }
    }

    public void OnClickStartButton()
    {

    }

    void OnClickedHistoryButton() {
        if (m_historyView == null) {
            m_historyView = new HighAndLowHistoryView(m_checkedPokers);
        }
        ViewManager.Instance.Push(m_historyView);
    }

    void OnClickedTerminalBtn() {
        if (m_terminalView == null) {
            m_terminalView = new HighAndLowTerminalView();
            m_terminalView.Build(m_mainViewGameObject.transform);
        }
        // ViewManager.Instance.Push(m_terminalView);
        m_terminalView.Show();
    }

    void OnClickedRouletteBtn() {
        if (m_rouletteView == null) {
            m_rouletteView = new HighAndLowRouletteView();
        }
        ViewManager.Instance.Push(m_rouletteView);
    }

    void OnClickedWinnerBtn() {

    }

    void ShowResultButtons() {
        m_rouletteBtn.gameObject.SetActive(true);
        m_winnerBtn.gameObject.SetActive(true);
    }

    void HideResultButtons() {
        m_rouletteBtn.gameObject.SetActive(false);
        m_winnerBtn.gameObject.SetActive(false);
    }

    void PlayFirst() {
        var pokerGO = CreateRandomPokerFromPool();
        (pokerGO.transform as RectTransform).rotation = Quaternion.Euler(0f, 0f, 60f);
        pokerGO.transform.SetParent(m_pokerShowTransform1);
        (pokerGO.transform as RectTransform).DOAnchorPosX(0, 1).SetLink(pokerGO);
        (pokerGO.transform as RectTransform).DOLocalRotate(Vector3.zero, 1).SetLink(pokerGO);

        var backFaceGO = CreatePoker(EPokers.BackFace);
        (backFaceGO.transform as RectTransform).rotation = Quaternion.Euler(0f, 0f, 60f);
        backFaceGO.transform.SetParent(m_pokerShowTransform2);
        (backFaceGO.transform as RectTransform).DOAnchorPosX(0, 1).SetDelay(2).SetLink(backFaceGO);
        var tween = (backFaceGO.transform as RectTransform).DOLocalRotate(Vector3.zero, 1).SetDelay(2).SetLink(backFaceGO);
        tween.onComplete += ShowTimer;
    }

    void PlayLoop() {
        m_resultLow.SetActive(false);
        var leftPokerGO = m_pokerShowTransform1.GetChild(0).gameObject;
        leftPokerGO.transform.SetParent(m_pokerTrashTransform);
        var angle = Random.Range(20, 70);
        (leftPokerGO.transform as RectTransform).DOAnchorPosX(0, 1).SetLink(leftPokerGO);
        (leftPokerGO.transform as RectTransform).DOLocalRotate(new Vector3(0,0,angle), 1).SetLink(leftPokerGO);


        var rightPokerGO = m_pokerShowTransform2.GetChild(0).gameObject;
        rightPokerGO.transform.SetParent(m_pokerShowTransform1);
        (rightPokerGO.transform as RectTransform).DOAnchorPosX(0, 1).SetLink(rightPokerGO);

        var backFaceGO = CreatePoker(EPokers.BackFace);
        (backFaceGO.transform as RectTransform).rotation = Quaternion.Euler(0f, 0f, 60f);
        backFaceGO.transform.SetParent(m_pokerShowTransform2);
        (backFaceGO.transform as RectTransform).DOAnchorPosX(0, 1).SetLink(backFaceGO);
        var tween = (backFaceGO.transform as RectTransform).DOLocalRotate(Vector3.zero, 1).SetLink(backFaceGO);
        tween.onComplete += ShowTimer;
    }

    void ShowTimer() {
        m_timer.SetActive(true);
        m_waitTrigger = true;

        ViewManager.Instance.StartCoroutine(UpdateTimeLabel());
    }

    IEnumerator<WaitForSeconds> UpdateTimeLabel() {
        var timer = AppConfig.Instance.CurrentHighAndLowTimer + 1;
        while (timer-- > 0) 
        {
            if (m_timeLabel != null)
                m_timeLabel.text = timer.ToString();
            yield return new WaitForSeconds(1);
        }
        if (m_mainViewGameObject != null)
            ShowResult();
    }

    void ShowResult() {
        HideTimer();
        m_resultIsShowing = true;
        ShowResultButtons();

        var poker = GetRandomPokerFromPool();
        var pokerSprite = GetSpriteWithPoker(poker);
        var backFaceGO = m_pokerShowTransform2.GetChild(0).gameObject;
        backFaceGO.GetComponent<Image>().overrideSprite = pokerSprite;
        backFaceGO.GetComponent<Image>().SetNativeSize();
        // 判断输赢
        m_resultLow.SetActive(true);
    }

    void HideTimer() {
        if (m_mainViewGameObject != null)
            m_timer.SetActive(false);
        m_waitTrigger = false;
    }

    EPokers GetRandomPokerFromPool() {
        var pokerIndex = Random.Range(0, m_pokerPool.Count - 1);
        var poker = m_pokerPool.ElementAt(pokerIndex);
        CheckedPoker(poker);
        return poker;
    }

    GameObject CreateRandomPokerFromPool() {
        var poker = GetRandomPokerFromPool();
        return CreatePoker(poker);
    }

    Sprite GetSpriteWithPoker(EPokers poker) {
        var path = "HighAndLow/Pokers/" + EPokersHelper.GetTextureNameFromPoker(poker);
        return Load<Sprite>(path);
    }

    GameObject CreatePoker(EPokers poker) {
        var pokerGO = GameObject.Instantiate(m_pokerTemplate, m_pokerStartTransform, false);
        pokerGO.SetActive(true);
        if (poker == EPokers.BackFace) {
            pokerGO.GetComponent<Image>().overrideSprite = Load<Sprite>("HighAndLow/Pokers/backface");
        }else {
            pokerGO.GetComponent<Image>().overrideSprite = GetSpriteWithPoker(poker);
        }
        pokerGO.GetComponent<Image>().SetNativeSize();
        return pokerGO;
    }

    void CheckedPoker(EPokers poker) {
        m_checkedPokers.Add(poker);
        var index = EPokersHelper.GetIndexOfPoker(poker);
        m_checkedItemList.ElementAt(index).SetActive(true);
        m_pokerPool.Remove(poker);
        m_checkRestCountLabel.text = m_pokerPool.Count.ToString();
        m_isGameFinished = m_pokerPool.Count == 0;
        m_pokersTile.SetActive(m_pokerPool.Count > 0);
    }

    

    void UnCheckedAllPokers() {
        m_pokerPool.Clear();
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 13; j++) {
                m_checkedItemList.ElementAt(i*13 + j).SetActive(false);
                m_pokerPool.Add((EPokers)(i*16+j));
            }
        }
    }
}