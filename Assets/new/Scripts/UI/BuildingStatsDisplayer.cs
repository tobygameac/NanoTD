using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingStatsDisplayer : MonoBehaviour {

  public Sprite[] iconSprites;

  public GameObject buildingIcon;
  private Image buildingIconImage;

  public GameObject buildingStats;
  private Text buildingStatsText;
  
  private Game game;

  private GameObject lastBuilding;
  private CharacterStats characterStats;

  void Start() {
    buildingIconImage = buildingIcon.GetComponent<Image>();
    buildingStatsText = buildingStats.GetComponent<Text>();

    game = Camera.main.GetComponent<Game>();

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

    buildingIconImage.sprite = iconSprites[(int)characterStats.BuildingID];

    buildingStatsText.text = GameConstants.NameOfBuildingID[(int)characterStats.BuildingID] + "\n";
    buildingStatsText.text += "價值 : " + characterStats.Cost + "\n\n";
    buildingStatsText.text += "傷害 : " + (characterStats.Damage).ToString("0.0");
    if (game.HasTechnology(GameConstants.TechnologyID.SELF_LEARNING)) {
      buildingStatsText.text += "(+" + (characterStats.DamageModifier * characterStats.BasicDamage).ToString("0.0") + ")";
    }
    buildingStatsText.text += "\n";
    buildingStatsText.text += "攻擊範圍 : " + characterStats.AttackingRange + "\n";
    buildingStatsText.text += "擊殺數 : " + characterStats.UnitKilled + "\n";
  }
}
