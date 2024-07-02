using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using DG.Tweening;
using Luna.UI;
using UnityEngine.Serialization;

public class GameSettingsView2 : Widget
{
    public ScrollRect scrollRect;
    
    public Slider bgmSegmentedButton;
    public ToggleGroup bgmToggleGroup;
    public List<Toggle> bgmToggles = new List<Toggle>();
    public Slider bgmVolumeSlider;
    public Slider effectVolumeSlider;
    public Text bgmVolumeText;
    public Text effectVolumeText;
    
    public Slider backgroundToggleSlider;
    public ToggleGroup backgroundToggleGroup;
    public List<Toggle> backgroundToggles = new List<Toggle>();
    
    public Button maxCellSettingButton;
    public Text maxCellSettingText; 
    
    public GameObject confirmPanel;
    public Button confirmSettingCellCountBtn;
    public Button cancelSettingCellCountBtn;
    
    public Button highAndLowTimerBtn;
    public Text highAndLowTimerText;

    public Button backButton;
    
    GameSelector gameSelector;
    
    float deltaTime = 0;
    
    bool isPresettingEffectVolume;
    bool isInited;
    
    int nextCellCountChange;


    public void Start() 
    {
        gameSelector = transform.parent.Find("Home").GetComponent<GameSelector>();
        
        bgmSegmentedButton.onValueChanged.AddListener(OnBgmSliderChanged);
        bgmSegmentedButton.value = AppConfig.Instance.BgmSelectedIdx;
        bgmVolumeSlider.onValueChanged.AddListener(OnBgmVolumeSliderValueChanged);
        bgmVolumeSlider.value = AppConfig.Instance.BGMVolume;
        
        effectVolumeSlider.onValueChanged.AddListener(OnVolumeEffectValueChanged);
        isPresettingEffectVolume = true;
        effectVolumeSlider.value = AppConfig.Instance.EffectVolume;
        isPresettingEffectVolume = false;
        
        backgroundToggleSlider.onValueChanged.AddListener(OnBackgroundSliderChanged);
        backgroundToggleSlider.value = AppConfig.Instance.ThemeSelectedIdx;
        maxCellSettingText.text = AppConfig.Instance.MaxCellCount.ToString();
        confirmSettingCellCountBtn.onClick.AddListener(OnClickConfirmSettingCellCountBtn);
        cancelSettingCellCountBtn.onClick.AddListener(OnClickCancelSettingCellCountBtn);
        highAndLowTimerText.text = AppConfig.Instance.CurrentHighAndLowTimer.ToString();
        backButton.onClick.AddListener(OnClickBackButton);

        EventSystem.current.SetSelectedGameObject(bgmSegmentedButton.gameObject);
        isInited = true;
    }
    
    private GameObject lastSelectedObject;
    
    
    public void Update() {
        if (Input.GetButtonDown("Cancel")) {
            OnClickBackButton();
        }

        if (Input.GetButtonUp("Horizontal")) {
            deltaTime = 0;
        }

        if (Input.GetButtonDown("Horizontal") && EventSystem.current.currentSelectedGameObject == maxCellSettingButton.gameObject) {
            if (Input.GetKey(KeyCode.LeftArrow)) {
                if (AppConfig.Instance.HasHistoryGameData()) {
                    confirmPanel.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(confirmSettingCellCountBtn.gameObject);
                    nextCellCountChange = -1;
                } else{
                    OnMaxCellSettingSliderValueChanged(-1);
                }
            }
            if (Input.GetKey(KeyCode.RightArrow)) {
                if (AppConfig.Instance.HasHistoryGameData()) {
                    confirmPanel.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(confirmSettingCellCountBtn.gameObject);
                    nextCellCountChange = +1;
                }else{
                    OnMaxCellSettingSliderValueChanged(+1);
                }
            }
        }

        if (Input.GetButtonDown("Horizontal") && EventSystem.current.currentSelectedGameObject == highAndLowTimerBtn.gameObject) {
            if (Input.GetKey(KeyCode.LeftArrow)) {
                OnHighAndLowTimerValueChanged(-10);
            }
            if (Input.GetKey(KeyCode.RightArrow)) {
                OnHighAndLowTimerValueChanged(+10);
            }
        }

        if (Input.GetButton("Horizontal") && EventSystem.current.currentSelectedGameObject == maxCellSettingButton.gameObject) {
            deltaTime += Time.deltaTime;
            if (Input.GetKey(KeyCode.LeftArrow) && deltaTime > 1) {
                OnMaxCellSettingSliderValueChanged(-1);
            }
            if (Input.GetKey(KeyCode.RightArrow) && deltaTime > 1) {
                OnMaxCellSettingSliderValueChanged(+1);
            }
        }
        
        GameObject currentSelectedObject = EventSystem.current.currentSelectedGameObject;
        if (currentSelectedObject != lastSelectedObject)
        {
            if (lastSelectedObject != null) 
                OnSelectedObjectChanged(lastSelectedObject, currentSelectedObject);
            lastSelectedObject = currentSelectedObject;
        }
    }

    void OnEnable() {
        if (isInited) {
            EventSystem.current.SetSelectedGameObject(bgmSegmentedButton.gameObject);
        }
    }
    
    void OnDisable() {
        lastSelectedObject = null;
        scrollRect.content.anchoredPosition = Vector2.zero;
    }

    private void OnBecameVisible()
    {
        scrollRect.content.anchoredPosition = Vector2.zero;
    }

    private void OnSelectedObjectChanged(GameObject oldObject, GameObject newObject)
    {
        SnapTo(newObject.GetComponent<RectTransform>());
    }

    public void Hide() {
        gameObject.SetActive(false);
        gameSelector.Show();
    }

    private void OnBackgroundSliderChanged(float value) {
        var intValue = Mathf.FloorToInt(value);
        AppConfig.Instance.ThemeSelectedIdx = intValue;
        backgroundToggles[AppConfig.Instance.ThemeSelectedIdx].isOn = true;
    }

    private void OnBgmSliderChanged(float value) {
        var intValue = Mathf.FloorToInt(value);
        AppConfig.Instance.BgmSelectedIdx = intValue;
        bgmToggles[AppConfig.Instance.BgmSelectedIdx].isOn = true;
    }

    public void OnBgmVolumeSliderValueChanged(float value) {
        bgmVolumeText.text = string.Format("{0}%", value * 10);
        AppConfig.Instance.BGMVolume = Mathf.FloorToInt(value);
        AudioManager.Instance.SetBgmVolume((int)value);
    }

    public void OnVolumeEffectValueChanged(float value) {
        effectVolumeText.text = string.Format("{0}%", value * 10);
        AppConfig.Instance.EffectVolume = Mathf.FloorToInt(value);
        AudioManager.Instance.SetEffectVolume((int)value);
        if (isPresettingEffectVolume) return;
        AudioManager.Instance.PlayNumberRotateEffectWithoutLoop();
        AudioManager.Instance.PlayNumberCheckEffect();
    }

    
    // 确认修改最大cell数
    void OnClickConfirmSettingCellCountBtn() {
        confirmPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(maxCellSettingButton.gameObject);
        OnMaxCellSettingSliderValueChanged(nextCellCountChange);
    }

    void OnClickCancelSettingCellCountBtn() {
        confirmPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(maxCellSettingButton.gameObject);
    }

    public void OnMaxCellSettingSliderValueChanged(int value) {
        if (value < 0 && AppConfig.Instance.MaxCellCount == 8) return;
        if (value > 0 && AppConfig.Instance.MaxCellCount == 75) return;

        AppConfig.Instance.MaxCellCount += value;
        maxCellSettingText.text = AppConfig.Instance.MaxCellCount.ToString();
        AppConfig.Instance.ClearGameData();
    }

    void OnHighAndLowTimerValueChanged(int value) {
        if (value < 0 && AppConfig.Instance.CurrentHighAndLowTimer == 10) return;
        if (value > 0 && AppConfig.Instance.CurrentHighAndLowTimer == 30) return;

        AppConfig.Instance.CurrentHighAndLowTimer += value;
        highAndLowTimerText.text = AppConfig.Instance.CurrentHighAndLowTimer.ToString();
    }

    void OnClickBackButton() {
        EventSystem.current.SetSelectedGameObject(null);
        AudioManager.Instance.PlayKeyBackEffect();
        Hide();
    }
    
    public void SnapTo(RectTransform target)
    {
        var newPos =
            (Vector2)scrollRect.transform.InverseTransformPoint(scrollRect.content.position)
            - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
        var y = Mathf.Clamp(newPos.y - 80f, 0, scrollRect.content.sizeDelta.y);
        var pos = new Vector2(scrollRect.content.anchoredPosition.x, y);
        Debug.Log("SnapTo: " + pos);
        DOTween.To(() => scrollRect.content.anchoredPosition, v => scrollRect.content.anchoredPosition = v, pos, 0.5f);
    }
}
