using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Scoreboard : MonoBehaviour {

  public Text scoreboardText;

  private bool gotScore;
  
  void Start() {
    Time.timeScale = 1;
    StartCoroutine(ScoreboardManager.GetScores(GameConstants.GameMode.SURVIVAL_NORMAL));
  }

  void Update() {
    if (!gotScore) {
      gotScore = !ScoreboardManager.IsGettingScores;
      if (gotScore) {
        scoreboardText.text = ScoreboardManager.ScoreboardText;
      }
    }
  }

  public void OnBackToMainMenuButtonClick() {
    Application.LoadLevel("MainMenu");
  }

}
