using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public partial class Game : MonoBehaviour {
  
  public GameObject HUDCanvas;
  public GameObject basicButtonCanvas;
  public GameObject buildingListCanvas;
  public GameObject techonologyListCanvas;
  public GameObject pauseMenuCanvas;
  public GameObject audioMenuCanvas;

  public GameObject buildingDetailCanvas;
  public GameObject technologyDetailCanvas;

  public GameObject buildingStatsCanvas;

  public GameObject buttonTemplate;

  private List<GameObject> buildingButtons;
  private List<GameObject> technologyButtons;

  public void OnViewBuildingListButtonClick() {
    AudioManager.PlayAudioClip(buttonSound);
    ViewBuildingList();
  }

  public void OnViewTechnologyListButtonClick() {
    AudioManager.PlayAudioClip(buttonSound);
    ViewTechnologyList();
  }

  public void OnResearchButtonClick() {
    int technologyCost = technologyManager.AvailableTechnology[viewingTechnologyIndex].Cost;
    if (money >= technologyCost) {
      AudioManager.PlayAudioClip(researchSound);

      money -= technologyCost;
      
      if (technologyManager.AvailableTechnology[viewingTechnologyIndex].ID == GameConstants.TechnologyID.ADDITIONAL_BUILDING_NUMBER) {
        ++maxBuildingNumber;
      }

      MessageManager.AddMessage("研發完成 : " + technologyManager.AvailableTechnology[viewingTechnologyIndex].Name);
      technologyManager.ResearchTechnology(viewingTechnologyIndex);
      for (int i = 0; i < technologyManager.NewTechnology.Count; ++i) {
        MessageManager.AddMessage("獲得科技 : " + technologyManager.NewTechnology[i].Name);
      }

      viewingTechnologyIndex = -1;

      InstantiateTechnologyButton();
    } else {
      AudioManager.PlayAudioClip(errorSound);
      MessageManager.AddMessage("需要更多金錢");
    }
  }

  public void OnUpgradeButtonClick() {
    AudioManager.PlayAudioClip(researchSound);
    Upgrade();
  }

  public void OnSellButtonClick() {
    AudioManager.PlayAudioClip(sellSound);
    Sell();
  }

  public void OnPauseButtonClick() {
    AudioManager.PlayAudioClip(buttonSound);
    Pause();
  }

  public void OnBackToGameButtonClick() {
    AudioManager.PlayAudioClip(buttonSound);
    BackToGame();
  }

  public void OnBuildingListButtonClick(int i) {
    AudioManager.PlayAudioClip(buttonSound);
    if (viewingBuildingIndex != i) { // Message spamming
      MessageManager.AddMessage("請選擇放置區域");
    }
    viewingBuildingIndex = i;
  }

  public void OnTechnologyListButtonClick(int i) {
    AudioManager.PlayAudioClip(buttonSound);
    viewingTechnologyIndex = i;
  }

  private void InstantiateBuildingButton() {

    if (buildingButtons == null) {
      buildingButtons = new List<GameObject>();
    } else {
      for (int i = 0; i < buildingButtons.Count; ++i) {
        Destroy(buildingButtons[i]);
      }
      buildingButtons.Clear();
    }

    for (int i = 0; i < buildingList.Count; ++i) {
      GameObject button = Instantiate(buttonTemplate) as GameObject;
      button.transform.SetParent(buildingListCanvas.transform);
      
      button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
      button.GetComponent<RectTransform>().localPosition = new Vector3(-380 + button.GetComponent<RectTransform>().sizeDelta.x * i * 1.1f, -160, 0);
      
      int buttonIndex = i; // Delegate is capturing a reference to the variable i
      button.GetComponent<Button>().onClick.AddListener(delegate{OnBuildingListButtonClick(buttonIndex);});

      string buildingName = GameConstants.NameOfBuildingID[(int)buildingList[i].GetComponent<CharacterStats>().BuildingID];
      button.transform.GetChild(0).GetComponent<Text>().text = buildingName + "(" + (i + 1) + ")";

      buildingButtons.Add(button);
    }

  }

  private void InstantiateTechnologyButton() {

    if (technologyButtons == null) {
      technologyButtons = new List<GameObject>();
    } else {
      for (int i = 0; i < technologyButtons.Count; ++i) {
        Destroy(technologyButtons[i]);
      }
      technologyButtons.Clear();
    }

    for (int i = 0; i < technologyManager.AvailableTechnology.Count; ++i) {
      GameObject button = Instantiate(buttonTemplate) as GameObject;
      button.transform.SetParent(techonologyListCanvas.transform);
      
      button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
      button.GetComponent<RectTransform>().localPosition = new Vector3(-380 + button.GetComponent<RectTransform>().sizeDelta.x * i * 1.1f, -160, 0);
      
      int buttonIndex = i; // Delegate is capturing a reference to the variable i
      button.GetComponent<Button>().onClick.AddListener(delegate{OnTechnologyListButtonClick(buttonIndex);});

      string technologyName = technologyManager.AvailableTechnology[i].Name;
      button.transform.GetChild(0).GetComponent<Text>().text = technologyName + "(" + (i + 1) + ")";

      technologyButtons.Add(button);
    }
  }

  private void UpdateCanvas() {
    basicButtonCanvas.SetActive((playerState == GameConstants.PlayerState.IDLE 
                              || playerState == GameConstants.PlayerState.VIEWING_BUILDING_LIST
                              || playerState == GameConstants.PlayerState.VIEWING_TECHNOLOGY_LIST)
                              && systemState == GameConstants.SystemState.PLAYING);
    buildingListCanvas.SetActive(playerState == GameConstants.PlayerState.VIEWING_BUILDING_LIST && systemState == GameConstants.SystemState.PLAYING);
    techonologyListCanvas.SetActive(playerState == GameConstants.PlayerState.VIEWING_TECHNOLOGY_LIST && systemState == GameConstants.SystemState.PLAYING);
    pauseMenuCanvas.SetActive(systemState == GameConstants.SystemState.PAUSE_MENU);
    audioMenuCanvas.SetActive(systemState == GameConstants.SystemState.AUDIO_MENU);

    if (_viewingTechnologyIndex >= 0 && _viewingTechnologyIndex < technologyManager.AvailableTechnology.Count) {
      technologyDetailCanvas.SetActive(systemState == GameConstants.SystemState.PLAYING);
    }
  }

  private void InitializeUI() {
    InstantiateBuildingButton();
    InstantiateTechnologyButton();
  }

  /*
  public GUISkin gameGUISkin;
  public Texture2D gameGUITexture;

  public Texture2D highlightTexture;
  public Texture2D towerFrameTexture;
  public Texture2D statsHUD;
  public Texture2D buildingHUD;
  public Texture2D maskTexture;
  public Texture2D nurseCheer;

  public Texture2D researchTexture;

  void OnGUI() {
    GUI.skin = gameGUISkin;
    GUI.depth = 1;
    
    if (PlayerState != GameConstants.PlayerState.IDLE || SystemState != GameConstants.SystemState.PLAYING) {
      GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), maskTexture, ScaleMode.StretchToFill, true, 10);
    }
    if (SystemState == GameConstants.SystemState.FINISHED || SystemState == GameConstants.SystemState.LOSED) {
      GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), nurseCheer, ScaleMode.StretchToFill, true, 10);
      return;
    }
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), statsHUD, ScaleMode.StretchToFill, true, 10);
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), buildingHUD, ScaleMode.StretchToFill, true, 10);
    if (SystemState == GameConstants.SystemState.PAUSE_MENU) {
      Time.timeScale = 0;
      // Menu
      GUI.color = Color.white;
      float menuWidth = Screen.width / 4.8f;
      float menuHeight = menuWidth;
      GUILayout.BeginArea(new Rect((Screen.width - menuWidth) / 2, (Screen.height - menuHeight) / 2, menuWidth, menuHeight), towerFrameTexture);
      GUI.color = Color.black;
      float labelWidth = menuWidth / 2;
      float labelHeight = labelWidth / 4;
      if (PlayerState == GameConstants.PlayerState.EXITING) {
        GUI.Label(new Rect(labelWidth / 2, labelHeight * 1, labelWidth, labelHeight), "確定離開?");
        if (GUI.Button(new Rect((menuWidth - labelWidth) / 2 - labelWidth * 0.5f, labelHeight * 2, labelWidth, labelHeight), "是")) {
          AudioManager.PlayAudioClip(buttonSound);
          Application.LoadLevel("MainMenu");
        }
        if (GUI.Button(new Rect((menuWidth - labelWidth) / 2 + labelWidth * 0.5f, labelHeight * 2, labelWidth, labelHeight), "否")) {
          AudioManager.PlayAudioClip(buttonSound);
          PlayerState = GameConstants.PlayerState.IDLE;
        }
      } else if (PlayerState == GameConstants.PlayerState.VIEWING_AUDIO_MENU) {
        AudioManager.Volume = GUI.HorizontalScrollbar(new Rect((menuWidth - labelWidth) / 2, labelHeight * 1, labelWidth, labelHeight), AudioManager.Volume, 0.01f, 0, 1);
        if (GUI.Button(new Rect((menuWidth - labelWidth) / 2, labelHeight * 2, labelWidth, labelHeight), "返回")) {
          AudioManager.PlayAudioClip(buttonSound);
          PlayerState = GameConstants.PlayerState.IDLE;
        }
      } else {
        if (GUI.Button(new Rect((menuWidth - labelWidth) / 2, labelHeight * 1, labelWidth, labelHeight), "音量調整")) {
          AudioManager.PlayAudioClip(buttonSound);
          PlayerState = GameConstants.PlayerState.VIEWING_AUDIO_MENU;
        }
        if (GUI.Button(new Rect((menuWidth - labelWidth) / 2, labelHeight * 2, labelWidth, labelHeight), "返回")) {
          AudioManager.PlayAudioClip(buttonSound);
          Time.timeScale = 1;
          SystemState = GameConstants.SystemState.PLAYING;
        }
        if (GUI.Button(new Rect((menuWidth - labelWidth) / 2, labelHeight * 3, labelWidth, labelHeight), "離開")) {
          AudioManager.PlayAudioClip(buttonSound);
          PlayerState = GameConstants.PlayerState.EXITING;
        }
      }
      GUILayout.EndArea();
      return;
    }

    // Stats
    float statsWidth = Screen.width / 4.8f;
    float statsHeight = statsWidth;
    GUILayout.BeginArea(new Rect(statsWidth / 5, statsHeight / 6, statsWidth, statsHeight));

    // Health bar
    float healthBarWidth = Screen.width / 9.6f;
    float healthBarHeight = healthBarWidth / 4;
    GUI.color = Color.red;
    GUI.DrawTexture(new Rect(0, 0, healthBarWidth, healthBarHeight), gameGUITexture, ScaleMode.StretchToFill, true, 10);
    GUI.color = Color.green;
    float healthPercent = 1;
    GUI.DrawTexture(new Rect(0, 0, healthBarWidth * healthPercent, healthBarHeight), gameGUITexture, ScaleMode.StretchToFill, true, 10);
    GUI.color = Color.black;

    GUI.Label(new Rect(healthBarWidth / 4, 0, healthBarWidth, healthBarHeight), "TBD" + " / " + "TBD");

    float informationLabelWidth = Screen.width / 6.4f;
    float informationLabelHeight = informationLabelWidth / 6;
    GUI.color = Color.green;
    GUI.Label(new Rect(0, informationLabelHeight * 0.8f * 1, informationLabelWidth, informationLabelHeight), "金錢" + money);
    GUI.Label(new Rect(0, informationLabelHeight * 0.8f * 2, informationLabelWidth, informationLabelHeight), "機械數量 : " + nowBuildingNumber + " / " + maxBuildingNumber);
    if (gameMode == GameConstants.GameMode.STORY) {
      if (GetComponent<LevelManager>().nowWave == GetComponent<LevelManager>().maxWave) {
        GUI.color = Color.red;
      }
      GUI.Label(new Rect(0, informationLabelHeight * 0.8f * 3, informationLabelWidth, informationLabelHeight), "波數 : ");
      if (GetComponent(WaveHandler).nowWave == 1 && GetComponent(WaveHandler).restTime > 0) {
        GUI.Label(new Rect(0, informationLabelHeight * 0.8f * 3, informationLabelWidth, informationLabelHeight), "準備時間 : ");
      } else if (GetComponent(MaxLoading)) {
        GUI.Label(new Rect(0, informationLabelHeight * 0.8f * 3, informationLabelWidth, informationLabelHeight), "負載 : ");
      }
    } else if (gameMode == GameConstants.GameMode.SURVIVAL_NORMAL || gameMode == GameConstants.GameMode.SURVIVAL_BOSS) {
      GUI.Label(new Rect(0, informationLabelHeight * 0.8f * 3, informationLabelWidth, informationLabelHeight), "分數 : ");
    }

    GUILayout.EndArea();

    GUI.color = Color.black;
    if (GUI.Button(new Rect(Screen.width / 1.35f, Screen.height / 1.15f, Screen.width / 9.6f, Screen.height * 0.05f), "選單(ESC)")) {
      Pause();
    }

    // Operation menu
    float operationMenuWidth = Screen.width / 9.6f;
    float operationMenuHeight = operationMenuWidth * 2;
    GUILayout.BeginArea(new Rect(Screen.width / 38.4f, Screen.height / 1.15f, operationMenuWidth, operationMenuHeight));
    float operationButtonWidth = operationMenuWidth;
    float operationButtonHeight = operationMenuWidth * 0.3f;
    if (GUI.Button(new Rect(0, operationButtonHeight * 0, operationButtonWidth, operationButtonHeight), "研究(R)")) {
      ViewTechnologyList();
    }
    if (GUI.Button(new Rect(0, operationButtonHeight * 1.15f, operationButtonWidth, operationButtonHeight), "建造(B)")) {
      ViewBuildingList();
    }
    GUILayout.EndArea();

    if (PlayerState == GameConstants.PlayerState.VIEWING_TECHNOLOGY_LIST) {
      if (viewingTechnologyIndex == -1) {
        float technologyButtonWidth = Screen.width / 6.4f;
        float technologyButtonHeight = Screen.height / 20;
        GUILayout.BeginArea(new Rect(Screen.width / 9.6f, Screen.height / 1.1f, Screen.width, Screen.height));
        for (int i = 0; i < technologyManager.AvailableTechnology.Count; ++i) {
          Technology technology = technologyManager.AvailableTechnology[i];
          if (GUI.Button(new Rect(technologyButtonWidth * (0.5f + i * 1.1f), 0, technologyButtonWidth, technologyButtonHeight), technology.Name + "(" + (i + 1) + ")")) {
            viewingTechnologyIndex = i;
          }
        }
        GUILayout.EndArea();
      } else if (viewingTechnologyIndex != -1) {
        float researchButtonWidth = Screen.width / 9.6f;
        float researchButtonHeight = researchButtonWidth / 4;
        int technologyCost = technologyManager.AvailableTechnology[viewingTechnologyIndex].Cost;
        if (GUI.Button(new Rect(Screen.width / 1.6f, Screen.height / 7.5f, researchButtonWidth, researchButtonHeight), "研究")) {
          if (money >= technologyCost) {
            AudioManager.PlayAudioClip(researchSound);

            money -= technologyCost;
            MessageManager.AddMessage("研發完成 : " + technologyManager.AvailableTechnology[viewingTechnologyIndex].Name);
            technologyManager.ResearchTechnology(viewingTechnologyIndex);
            for (int i = 0; i < technologyManager.NewTechnology.Count; ++i) {
              MessageManager.AddMessage("獲得科技 : " + technologyManager.NewTechnology[i].Name);
            }


            viewingTechnologyIndex = -1;
          } else {
            AudioManager.PlayAudioClip(errorSound);
            MessageManager.AddMessage("需要更多金錢");
          }
        }
      }
      return;
    }

    if (PlayerState == GameConstants.PlayerState.VIEWING_BUILDING_LIST) {
      GUILayout.BeginArea(new Rect(Screen.width / 9.6f, Screen.height / 1.1f, Screen.width, Screen.height));

      for (int i = 0; i < buildingList.Length; ++i) {
        float buildingButtonWidth = Screen.width / 6.4f;
        float buildingButtonHeight = Screen.height / 20;
        string buildingName = GameConstants.NameOfBuildingID[(int)buildingList[i].GetComponent<CharacterStats>().BuildingID];
        if (GUI.Button(new Rect(buildingButtonWidth * (0.5f + i * 1.1f), 0, buildingButtonWidth, buildingButtonHeight), buildingName + "(" + (i + 1) + ")")) {
          AudioManager.PlayAudioClip(buttonSound);
          if (i != buildingIndex) { // Prevent multiple click
            MessageManager.AddMessage("請選擇放置區域");
            buildingIndex = i;
          }
        }
      }

      GUILayout.EndArea();
      return;
    }

    if (selectedBuilding != null) {

      
    }

  }

  */
}
