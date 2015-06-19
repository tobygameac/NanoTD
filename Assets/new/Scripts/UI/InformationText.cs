using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Text))]
public class InformationText : MonoBehaviour {

  private Game game;
  private GameManager gameManager;

  private Text text;

  public float remainingTimeAlertTime = 10.0f;
  public float alertSpeed = 10.0f;
  
  void Start() {
    game = Camera.main.GetComponent<Game>();
    gameManager = Camera.main.GetComponent<GameManager>();
    text = GetComponent<Text>();
  }

  void Update() {
    text.text = "金錢 : <color=yellow>" + game.Money + "</color>\n";
    if (game.CurrentBuildingNumber == game.MaxBuildingNumber) {
      text.text += "裝置數量 : <color=red>" + game.CurrentBuildingNumber + " / " + game.MaxBuildingNumber + "</color>\n";
    } else {
      text.text += "裝置數量 : " + game.CurrentBuildingNumber + " / " + game.MaxBuildingNumber + "\n";
    }
    if (game.GameMode == GameConstants.GameMode.STORY) {
      text.text += "波數 : " + gameManager.CurrentWave + " / " + gameManager.MaxWave + "\n";
    } else if ((game.GameMode == GameConstants.GameMode.SURVIVAL_NORMAL) || (game.GameMode == GameConstants.GameMode.SURVIVAL_BOSS)) {
      text.text += "波數 : " + gameManager.CurrentWave + "\n";
    }
    
    text.text += "\n場上病菌數量 : " + gameManager.NumberOfEnemiesOnMap + "\n";
    text.text += "本波數剩餘病菌數量 : " + gameManager.NumberOfEnemiesToGenerate + "\n\n";

    if ((game.GameMode == GameConstants.GameMode.SURVIVAL_NORMAL) || (game.GameMode == GameConstants.GameMode.SURVIVAL_BOSS)) {
      if (gameManager.GameState == GameConstants.GameState.WAIT_FOR_THE_NEXT_WAVE) {
        text.text += "病菌將於" + (int)(gameManager.RestingTimeBetweenWaves - gameManager.RestedTime) + "秒後進行下一波攻勢\n\n";
      } else if (gameManager.GameState == GameConstants.GameState.MIDDLE_OF_THE_WAVE) {
        int remainingTime = (int)gameManager.RemainingTimeOfCurrentWave;
        if (remainingTime >= remainingTimeAlertTime) {
          text.text += "您必須在<color=red>" + remainingTime + "</color>秒內擊退所有病菌\n\n";
        } else {
          int alpha = (int)((Mathf.Sin(Time.time * alertSpeed) + 1) / 2 * 255);
          text.text += "您必須在<color=#ff0000" + alpha.ToString("X2") + ">" + remainingTime + "</color>秒內擊退所有病菌\n\n";
        }
      } else if ((gameManager.GameState == GameConstants.GameState.FINISHED) || (gameManager.GameState == GameConstants.GameState.LOSED)) {
        text.text += "遊戲結束\n";
      }
      text.text += "分數 : " + gameManager.Score + "\n";
    }
  }
}
