using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

  public AudioClip buttonSound;

  public void OnStartButtonClick() {
    AudioManager.PlayAudioClip(buttonSound);
    Time.timeScale = 1;
    Application.LoadLevel("SurvivalNormal");
  }

  public void OnScoreboardButtonClick() {
    AudioManager.PlayAudioClip(buttonSound);
    Application.LoadLevel("Scoreboard");
  }

  public void OnExitButtonClick() {
    AudioManager.PlayAudioClip(buttonSound);
    Application.Quit();
  }

  void Start() {
    AudioManager.Volume = 0.5f;
  }
}
