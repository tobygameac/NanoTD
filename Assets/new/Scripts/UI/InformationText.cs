using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Text))]
public class InformationText : MonoBehaviour {

  private Game game;
  private GameManager gameManager;

  private Text text;
  
  void Start() {
    game = Camera.main.GetComponent<Game>();
    gameManager = Camera.main.GetComponent<GameManager>();
    text = GetComponent<Text>();
  }

  void Update() {
    text.text = "金錢 : " + game.Money + "\n";
    text.text += "建造數量 : " + game.CurrentBuildingNumber + " / " + game.MaxBuildingNumber + "\n";
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
        text.text += "您必須在" + (int)gameManager.RemainingTimeOfCurrentWave + "秒內擊退所有病菌\n\n";
      } else if ((gameManager.GameState == GameConstants.GameState.FINISHED) || (gameManager.GameState == GameConstants.GameState.LOSED)) {
        text.text += "遊戲結束\n";
      }
      text.text += "分數 : " + gameManager.Score + "\n";
    }
  }
}
