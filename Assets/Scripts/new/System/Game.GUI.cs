using UnityEngine;
using System.Collections;

public partial class Game : MonoBehaviour {
  
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
    if (GameConstants.playerStatus != GameConstants.PlayerStatus.DoingNothing || GameConstants.gameStatus != GameConstants.GameStatus.Playing) {
      GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), maskTexture, ScaleMode.StretchToFill, true, 10);
    }
    if (GameConstants.gameStatus == GameConstants.GameStatus.Finished || GameConstants.gameStatus == GameConstants.GameStatus.Losed) {
      GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), nurseCheer, ScaleMode.StretchToFill, true, 10);
      return;
    }
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), statsHUD, ScaleMode.StretchToFill, true, 10);
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), buildingHUD, ScaleMode.StretchToFill, true, 10);
    if (GameConstants.gameStatus == GameConstants.GameStatus.Pausing) {
      Time.timeScale = 0;
      // Menu
      GUI.color = Color.white;
      float menuWidth = Screen.width / 4.8f;
      float menuHeight = menuWidth;
      GUILayout.BeginArea(new Rect((Screen.width - menuWidth) / 2, (Screen.height - menuHeight) / 2, menuWidth, menuHeight), towerFrameTexture);
      GUI.color = Color.black;
      float labelWidth = menuWidth / 2;
      float labelHeight = labelWidth / 4;
      if (GameConstants.playerStatus == GameConstants.PlayerStatus.Exiting) {
        GUI.Label(new Rect(labelWidth / 2, labelHeight * 1, labelWidth, labelHeight), "確定離開?");
        if (GUI.Button(new Rect((menuWidth - labelWidth) / 2 - labelWidth * 0.5f, labelHeight * 2, labelWidth, labelHeight), "是")) {
          AudioManager.PlayAudioClip(buttonSound);
          Application.LoadLevel("MainMenu");
        }
        if (GUI.Button(new Rect((menuWidth - labelWidth) / 2 + labelWidth * 0.5f, labelHeight * 2, labelWidth, labelHeight), "否")) {
          AudioManager.PlayAudioClip(buttonSound);
          GameConstants.playerStatus = GameConstants.PlayerStatus.DoingNothing;
        }
      } else if (GameConstants.playerStatus == GameConstants.PlayerStatus.AdjustingVolume) {
        AudioManager.Volume = GUI.HorizontalScrollbar(new Rect((menuWidth - labelWidth) / 2, labelHeight * 1, labelWidth, labelHeight), AudioManager.Volume, 0.01f, 0, 1);
        if (GUI.Button(new Rect((menuWidth - labelWidth) / 2, labelHeight * 2, labelWidth, labelHeight), "返回")) {
          AudioManager.PlayAudioClip(buttonSound);
          GameConstants.playerStatus = GameConstants.PlayerStatus.DoingNothing;
        }
      } else {
        if (GUI.Button(new Rect((menuWidth - labelWidth) / 2, labelHeight * 1, labelWidth, labelHeight), "音量調整")) {
          AudioManager.PlayAudioClip(buttonSound);
          GameConstants.playerStatus = GameConstants.PlayerStatus.AdjustingVolume;
        }
        if (GUI.Button(new Rect((menuWidth - labelWidth) / 2, labelHeight * 2, labelWidth, labelHeight), "返回")) {
          AudioManager.PlayAudioClip(buttonSound);
          Time.timeScale = 1;
          GameConstants.gameStatus = GameConstants.GameStatus.Playing;
        }
        if (GUI.Button(new Rect((menuWidth - labelWidth) / 2, labelHeight * 3, labelWidth, labelHeight), "離開")) {
          AudioManager.PlayAudioClip(buttonSound);
          GameConstants.playerStatus = GameConstants.PlayerStatus.Exiting;
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
    float healthPercent = 1/*************TBD***/;
    GUI.DrawTexture(new Rect(0, 0, healthBarWidth * healthPercent, healthBarHeight), gameGUITexture, ScaleMode.StretchToFill, true, 10);
    GUI.color = Color.black;

    GUI.Label(new Rect(healthBarWidth / 4, 0, healthBarWidth, healthBarHeight), "TBD" + " / " + "TBD");

    float informationLabelWidth = Screen.width / 6.4f;
    float informationLabelHeight = informationLabelWidth / 6;
    GUI.color = Color.green;
    GUI.Label(new Rect(0, informationLabelHeight * 0.8f * 1, informationLabelWidth, informationLabelHeight), "金錢" + money);
    GUI.Label(new Rect(0, informationLabelHeight * 0.8f * 2, informationLabelWidth, informationLabelHeight), "機械數量 : " + nowBuildingNumber + " / " + maxBuildingNumber);
    if (GameConstants.gameMode == GameConstants.GameMode.Story) {
      /*
      if (GetComponent<LevelManager>().nowWave == GetComponent<LevelManager>().maxWave) {
        GUI.color = Color.red;
      }
      */
      GUI.Label(new Rect(0, informationLabelHeight * 0.8f * 3, informationLabelWidth, informationLabelHeight), "波數 : ");
      /*
      if (GetComponent(WaveHandler).nowWave == 1 && GetComponent(WaveHandler).restTime > 0) {
        GUI.Label(new Rect(0, informationLabelHeight * 0.8f * 3, informationLabelWidth, informationLabelHeight), "準備時間 : ");
      } else if (GetComponent(MaxLoading)) {
        GUI.Label(new Rect(0, informationLabelHeight * 0.8f * 3, informationLabelWidth, informationLabelHeight), "負載 : ");
      }
      */
    } else if (GameConstants.gameMode == GameConstants.GameMode.SurvivalNormal || GameConstants.gameMode == GameConstants.GameMode.SurvivalBoss) {
      GUI.Label(new Rect(0, informationLabelHeight * 0.8f * 3, informationLabelWidth, informationLabelHeight), "分數 : ");
    }

    GUILayout.EndArea();

    GUI.color = Color.black;
    if (GUI.Button(new Rect(Screen.width / 1.35f, Screen.height / 1.15f, Screen.width / 9.6f, Screen.height * 0.05f), "選單(ESC)")) {
      AudioManager.PlayAudioClip(buttonSound);
      GameConstants.gameStatus = GameConstants.GameStatus.Pausing;
    }

    // Operation menu
    float operationMenuWidth = Screen.width / 9.6f;
    float operationMenuHeight = operationMenuWidth * 2;
    GUILayout.BeginArea(new Rect(Screen.width / 38.4f, Screen.height / 1.15f, operationMenuWidth, operationMenuHeight));
    float operationButtonWidth = operationMenuWidth;
    float operationButtonHeight = operationMenuWidth * 0.3f;
    if (GUI.Button(new Rect(0, operationButtonHeight * 0, operationButtonWidth, operationButtonHeight), "研究(R)")) {
      Research();
    }
    if (GUI.Button(new Rect(0, operationButtonHeight * 1.15f, operationButtonWidth, operationButtonHeight), "建造(B)")) {
      Build();
    }
    GUILayout.EndArea();

    if (GameConstants.playerStatus == GameConstants.PlayerStatus.Researching) {
      if (technologyIndex == -1) {
        float technologyButtonWidth = Screen.width / 6.4f;
        float technologyButtonHeight = Screen.height / 20;
        GUILayout.BeginArea(new Rect(Screen.width / 9.6f, Screen.height / 1.1f, Screen.width, Screen.height));
        for (int i = 0; i < technologyManager.AvailableTechnology.Count; ++i) {
          Technology technology = technologyManager.AvailableTechnology[i];
          if (GUI.Button(new Rect(technologyButtonWidth * (0.5f + i * 1.1f), 0, technologyButtonWidth, technologyButtonHeight), technology.Name + "(" + (i + 1) + ")")) {
            technologyIndex = i;
          }
        }
        GUILayout.EndArea();
      } else if (technologyIndex != -1) {
        float researchButtonWidth = Screen.width / 9.6f;
        float researchButtonHeight = researchButtonWidth / 4;
        int technologyCost = technologyManager.AvailableTechnology[technologyIndex].Cost;
        if (GUI.Button(new Rect(Screen.width / 1.6f, Screen.height / 7.5f, researchButtonWidth, researchButtonHeight), "研究")) {
          if (money >= technologyCost) {
            AudioManager.PlayAudioClip(researchSound);

            money -= technologyCost;
            MessageManager.AddMessage("研發完成 : " + technologyManager.AvailableTechnology[technologyIndex].Name);
            technologyManager.ResearchTechnology(technologyIndex);
            for (int i = 0; i < technologyManager.NewTechnology.Count; ++i) {
              MessageManager.AddMessage("獲得科技 : " + technologyManager.NewTechnology[i].Name);
            }


            technologyIndex = -1;
          } else {
            AudioManager.PlayAudioClip(errorSound);
            MessageManager.AddMessage("需要更多金錢");
          }
        }
      }
      return;
    }

    if (GameConstants.playerStatus == GameConstants.PlayerStatus.Building) {
      GUILayout.BeginArea(new Rect(Screen.width / 9.6f, Screen.height / 1.1f, Screen.width, Screen.height));

      for (int i = 0; i < buildingList.Length; ++i) {
        float buildingButtonWidth = Screen.width / 6.4f;
        float buildingButtonHeight = Screen.height / 20;
        if (GUI.Button(new Rect(buildingButtonWidth * (0.5f + i * 1.1f), 0, buildingButtonWidth, buildingButtonHeight), "name" + "(" + (i + 1) + ")")) {
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
}
