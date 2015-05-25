using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingStatsDisplayer : MonoBehaviour {

  private Game game;
  private Text text;
  
  private GameObject lastBuilding;
  private CharacterStats characterStats;

  void Start() {
    game = Camera.main.GetComponent<Game>();
    text = GetComponent<Text>();

    lastBuilding = null;
  }

  void Update() {
    GameObject building = game.SelectedBuilding;

    if (building != null && building != lastBuilding) {
      characterStats = building.GetComponent<CharacterStats>();
      lastBuilding = building;
    }

    if (building == null) {
      return;
    }

    text.text = GameConstants.NameOfBuildingID[(int)characterStats.BuildingID] + "\n";
    text.text += "價值 : " + characterStats.Cost + "\n\n";
    text.text += "傷害 : " + characterStats.Damage + "\n";
    text.text += "攻擊範圍 : " + characterStats.AttackingRange + "\n";
    text.text += "擊殺數 : " + characterStats.Cost + "\n";
  }
}
