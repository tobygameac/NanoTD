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
    characterStats.HPModifier = wave - 1;

    for (int w = 2, hpModifierIndex = 0; hpModifierIndex < GameConstants.WAVE_THERSHOLD_FOR_THE_NEXT_HP_MODIFIER.Length; ++hpModifierIndex) {
      while (w <= GameConstants.WAVE_THERSHOLD_FOR_THE_NEXT_HP_MODIFIER[hpModifierIndex] && w <= wave) {
        characterStats.HPModifier += GameConstants.HP_MODIFIERS_FOR_WAVE[hpModifierIndex];
        ++w;
      }
    }
    
    characterStats.Cost = (int)(characterStats.Cost * (wave - 1) * GameConstants.COST_MODIFIER_FOR_EACH_WAVE);

    characterStats.MovingSpeedModifier += Random.Range(-1, 1) * GameConstants.ENEMY_MOVING_SPEED_FLOATING_MODIFIER;

    characterStats.MovingSpeedModifier += (wave - 1) * GameConstants.ENEMY_MOVING_SPEED_MODIFIER_FOR_EACH_WAVE; 
  }

  public static void AddRandomImprovement(GameObject targetCharacter) {
    float dice = Random.Range(0.0f, 1.0f);

    CharacterStats characterStats = targetCharacter.GetComponent<CharacterStats>();

    if (dice < GameConstants.PROBABILITY_OF_STRONGGER) {
      characterStats.Cost = (int)(characterStats.Cost * GameConstants.COST_SCALE_OF_STRONGGER);
      targetCharacter.AddComponent<Strongger>();
    }

    if (dice < GameConstants.PROBABILITY_OF_INSANE) {
      characterStats.Cost = (int)(characterStats.Cost * GameConstants.COST_SCALE_OF_INSANE);
      targetCharacter.AddComponent<Insane>();
    }

    if (dice < GameConstants.PROBABILITY_OF_SELF_HEALING) {
      characterStats.Cost = (int)(characterStats.Cost * GameConstants.COST_SCALE_OF_SELF_HEALING);
      targetCharacter.AddComponent<SelfHealing>();
    }

    if (dice < GameConstants.PROBABILITY_OF_CELL_DIVISION) {
      characterStats.Cost = (int)(characterStats.Cost * GameConstants.COST_SCALE_OF_CELL_DIVISION);
      targetCharacter.AddComponent<CellDivision>();
    }
  }
}
