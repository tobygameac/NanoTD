using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour {

  bool done;
  
  void Start() {
    StartCoroutine(ScoreboardManager.GetScores(GameConstants.GameMode.SURVIVAL_NORMAL));
  }

  void Update() {
    if (!done) {
      done = !ScoreboardManager.IsGettingScores;
      if (done) {
        Debug.Log(ScoreboardManager.ScoreboardText);
      }
    }
  }

}
