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
    characterStats.CurrentHP *= Mathf.Pow(1.1f, (float)wave);
    characterStats.MaxHP = characterStats.CurrentHP;
  }
}
