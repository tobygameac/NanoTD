using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

  public void OnStartButtonClick() {
    Time.timeScale = 1;
    Application.LoadLevel("SurvivalNormal");
  }

  public void OnScoreboardButtonClick() {
    Application.LoadLevel("Scoreboard");
  }

  public void OnExitButtonClick() {
    Application.Quit();
  }
}
