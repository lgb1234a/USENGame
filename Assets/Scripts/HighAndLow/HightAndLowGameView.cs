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
    List<int> m_pokerPool = new();
    List<int> m_checkedPokers = new();
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
    bool m_isWaitContinue;
    bool m_isWaitTimer;
    bool m_isShowTimer;
    int m_lastPoker = -1;
    public void Build()
    {
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());

        m_pokerStartTransform = m_mainViewGameObject.transform.Find("PokerStart");
        m_pokerShowTransform1 = m_mainViewGameObject.transform.Find("PokerShow1");
        m_pokerShowTransform2 = m_mainViewGameObject.transform.Find("PokerShow2");
        m_pokerTrashTransform = m_mainViewGameObject.transform.Find("PokerShowTrash");

        m_checkRestCountLabel = m_mainViewGameObject.transform.Find("PokerCheckList/RestCountLabel").GetComponent<Text>();
        var cachedPokerValues = AppConfig.Instance.CheckedPokers;
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 13; j++) {
                if (!cachedPokerValues.Contains(i*16+j))
                    m_pokerPool.Add(i*16+j);
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

        if (cachedPokerValues.Count() > 0) {
            for (int i = 0; i < cachedPokerValues.Count(); i++)
            {
                var value = cachedPokerValues[i];
                CheckedPoker(value, false);
                //创建缓存的牌
                var pokerGo = CreatePoker((EPokers)value);
                if (i == cachedPokerValues.Count() - 1) {
                    //最后一张
                    pokerGo.transform.SetParent(m_pokerShowTransform1);
                    (pokerGo.transform as RectTransform).anchoredPosition = Vector2.zero;
                    m_lastPoker = value;
                }else {
                    pokerGo.transform.SetParent(m_pokerTrashTransform);
                    (pokerGo.transform as RectTransform).anchoredPosition = Vector2.zero;
                    var angle = Random.Range(20, 70);
                    (pokerGo.transform as RectTransform).localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                }
            }

            m_isWaitContinue = true;
        }
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
                }
                else if (m_isWaitContinue) {
                    // 重连继续上一局
                    AudioManager.Instance.PlaySendPokerEffect();
                    var backFaceGO = CreatePoker(EPokers.BackFace);
                    (backFaceGO.transform as RectTransform).rotation = Quaternion.Euler(0f, 0f, 60f);
                    backFaceGO.transform.SetParent(m_pokerShowTransform2);
                    (backFaceGO.transform as RectTransform).DOAnchorPosX(0, 1).SetLink(backFaceGO);
                    var tween = (backFaceGO.transform as RectTransform).DOLocalRotate(Vector3.zero, 1).SetLink(backFaceGO);
                    tween.onComplete += WaitTimer;
                    m_isWaitContinue = false;
                }
                else if (m_isWaitTimer) {
                    ShowTimer();
                    m_isWaitTimer = false;
                }
                else if (m_isShowTimer) {
                    m_isShowTimer = false;
                }
                else if (m_resultIsShowing){
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
        AudioManager.Instance.PlayKeyBackEffect();
        if (m_historyView == null) {
            m_historyView = new HighAndLowHistoryView(m_checkedPokers);
        }
        ViewManager.Instance.Push(m_historyView);
    }

    void OnClickedTerminalBtn() {
        AudioManager.Instance.PlayKeyBackEffect();
        if (m_terminalView == null) {
            m_terminalView = new HighAndLowTerminalView();
            m_terminalView.Build(m_mainViewGameObject.transform);
        }
        // ViewManager.Instance.Push(m_terminalView);
        m_terminalView.Show();
    }

    void OnClickedRouletteBtn() {
        AudioManager.Instance.PlayKeyBackEffect();
        if (m_rouletteView == null) {
            m_rouletteView = new HighAndLowRouletteView();
        }
        ViewManager.Instance.Push(m_rouletteView);
    }

    void OnClickedWinnerBtn() {
        AudioManager.Instance.PlayKeyStartEffect();

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
        tween.onComplete += WaitTimer;
    }

    void PlayLoop() {
        AudioManager.Instance.PlaySendPokerEffect();
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
        tween.onComplete += WaitTimer;
    }

    void WaitTimer() {
        m_isWaitTimer = true;
    }

    void ShowTimer() {
        AudioManager.Instance.PlayTimerStartEffect();
        m_timer.SetActive(true);
        m_waitTrigger = true;

        ViewManager.Instance.StartCoroutine(UpdateTimeLabel());
    }

    IEnumerator<WaitForSeconds> UpdateTimeLabel() {
        var timer = AppConfig.Instance.CurrentHighAndLowTimer + 1;
        m_isShowTimer = true;
        while (timer-- > 0) 
        {
            if (!m_isShowTimer) {
                // 提前结束
                timer = 0;
                break;
            }
            AudioManager.Instance.PlayHighAndLowTimerEffect();
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

        var leftPoker = m_lastPoker;
        var poker = GetRandomPokerFromPool();
        var pokerSprite = GetSpriteWithPoker(poker);
        var backFaceGO = m_pokerShowTransform2.GetChild(0).gameObject;
        backFaceGO.GetComponent<Image>().overrideSprite = pokerSprite;
        backFaceGO.GetComponent<Image>().SetNativeSize();
        // 判断输赢
        m_resultLow.SetActive(true);
        if (EPokersHelper.GetPokerValue((EPokers)leftPoker) < EPokersHelper.GetPokerValue(poker)) {
            AudioManager.Instance.PlayHighEffect();
        }

        if (EPokersHelper.GetPokerValue((EPokers)leftPoker) > EPokersHelper.GetPokerValue(poker)) {
            AudioManager.Instance.PlayLowEffect();
        }
    }

    void HideTimer() {
        if (m_mainViewGameObject != null)
            m_timer.SetActive(false);
        m_waitTrigger = false;
    }

    EPokers GetRandomPokerFromPool() {
        var temp = new List<int>();
        if (m_lastPoker != -1) {
            var pokerValue = m_lastPoker;
            bool shoted = m_pokerPool.Remove(pokerValue%16);
            if (shoted) {
                temp.Add(pokerValue%16);
            }
            shoted = m_pokerPool.Remove(pokerValue%16 + 16);
            if (shoted) {
                temp.Add(pokerValue%16 + 16);
            }
            shoted = m_pokerPool.Remove(pokerValue%16 + 32);
            if (shoted) {
                temp.Add(pokerValue%16 + 32);
            }
            shoted = m_pokerPool.Remove(pokerValue%16 + 48);
            if (shoted) {
                temp.Add(pokerValue%16 + 48);
            }
        }
        var pokerIndex = Random.Range(0, m_pokerPool.Count - 1);

        var poker = m_pokerPool.ElementAt(pokerIndex);
        foreach (var item in temp)
        {
            m_pokerPool.Add(item);
        }
        CheckedPoker(poker);
        m_lastPoker = poker;
        return (EPokers)poker;
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

    void CheckedPoker(int poker, bool isNeedCache = true) {
        m_checkedPokers.Add(poker);
        var index = EPokersHelper.GetIndexOfPoker((EPokers)poker);
        m_checkedItemList.ElementAt(index).SetActive(true);
        m_pokerPool.Remove((int)poker);
        m_checkRestCountLabel.text = m_pokerPool.Count.ToString();
        m_isGameFinished = m_pokerPool.Count == 0;
        m_pokersTile.SetActive(m_pokerPool.Count > 0);
        // save
        if (isNeedCache) {
            var checkedPokerValues = m_checkedPokers.ConvertAll<int>((v)=>(int)v);
            AppConfig.Instance.CheckedPokers = checkedPokerValues;
        }
    }

    

    void UnCheckedAllPokers() {
        m_pokerPool.Clear();
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 13; j++) {
                m_checkedItemList.ElementAt(i*13 + j).SetActive(false);
                m_pokerPool.Add(i*16+j);
            }
        }
    }
}