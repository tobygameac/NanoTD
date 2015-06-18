using UnityEngine;
using System.Collections;

public class EnemyStatsModifier : MonoBehaviour {
  
  private static EnemyStatsModifier instance;

  public static EnemyStatsModifier GetInstance() {
    return instance;
  }

  void Awake() {
    if (instance != null && instance != this) {
      Destroy(this.gameObject);
      return;
    }
    instance = this;
  }

  public static void ModifyStatsWithWave(CharacterStats characterStats, int wave) {
    characterStats.HPModifier = ((wave - 1) + Mathf.Pow(1.3f, (float)(wave - 1)));
    characterStats.Cost += (wave - 1) * 25;

    // Random buff
    if (wave >= 5) {
    }
  }
}
