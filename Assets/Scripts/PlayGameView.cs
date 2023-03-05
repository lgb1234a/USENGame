﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayGameView : IViewOperater
{
    public static string __REOPEN_DATA__ = "__REOPEN_GAME_DATA__";
    string m_prefabPath = "GamePanel";
    GameObject m_viewGameObject;
    Button m_stopButton;
    Button m_exitButton;
    Transform m_pausePanel;
    CheckAnimator m_doTweenAnimator;

    Transform m_numberCellTemplate;
    List<Transform> m_numberCells;
    public GameDataHandler m_gameData;

    public PlayGameView() {
        var obj = Resources.Load<GameObject>(m_prefabPath);
        m_viewGameObject = GameObject.Instantiate<GameObject>(obj, Vector3.zero, Quaternion.identity, ViewManager.Instance.GetRootTransform());
        var position = m_viewGameObject.transform.localPosition;
        position.z = 0;
        m_viewGameObject.transform.localPosition = position;

        m_stopButton = m_viewGameObject.transform.Find("PlayPanel/PausePanel/StopButton").GetComponent<Button>();
        m_stopButton.onClick.AddListener(OnClickStopButton);

        m_exitButton = m_viewGameObject.transform.Find("PlayPanel/PausePanel/ExitButton").GetComponent<Button>();
        m_exitButton.onClick.AddListener(OnClickExitButton);

        m_pausePanel = m_viewGameObject.transform.Find("PlayPanel/PausePanel");
        m_doTweenAnimator = m_viewGameObject.transform.Find("PlayPanel/Game/Mask").GetComponent<CheckAnimator>();

        m_numberCellTemplate = m_viewGameObject.transform.Find("PlayPanel/Game/NumberPanel/NumberCell");
    }


    public void Show() {
        m_viewGameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(m_stopButton.gameObject);

        var gameDataStr = PreferencesStorage.ReadString(__REOPEN_DATA__, null);
        if (gameDataStr != null && gameDataStr.Length > 0) {
            m_gameData = JsonSerializablity.Deserialize<GameDataHandler>(gameDataStr);
        }else {
            m_gameData = new GameDataHandler(75);
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
        ViewManager.Instance.Pop();
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
            m_pausePanel.gameObject.SetActive(!m_pausePanel.gameObject.activeSelf);
        }

        // green bingo anim  KEYCODE_PROG_GREEN  399


        // red  398

        // yellow 400
    }

    public void OnAndroidKeyDown(string keyName) {
        if (keyName == "blue") {
            m_doTweenAnimator.Animate(m_gameData);
        }
    }

    public void OnClickExitButton() {
        PreferencesStorage.SaveString(__REOPEN_DATA__, null);
        Hide();
    }

    public void OnClickStopButton() {
        var gameData = JsonSerializablity.Serialize(m_gameData);
        PreferencesStorage.SaveString(__REOPEN_DATA__, gameData);
        Hide();
    }
}
