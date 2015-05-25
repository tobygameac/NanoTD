using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeButtonHandler : MonoBehaviour {

  private Game game;
  private Button button;
  private bool hasUpgradeTechnology;
  
  private GameObject lastBuilding;
  private CharacterStats characterStats;


  void Start() {
    game = Camera.main.GetComponent<Game>();
    button = GetComponent<Button>();
    button.interactable = false;

    hasUpgradeTechnology = false;

    lastBuilding = null;
  }

  void Update() {

    if (!hasUpgradeTechnology) {
      hasUpgradeTechnology = game.HasTechnology(GameConstants.TechnologyID.UPGRADE);
    }

    GameObject building = game.SelectedBuilding;

    if (building != null && building != lastBuilding) {
      characterStats = building.GetComponent<CharacterStats>();
      lastBuilding = building;
    }

    if (building == null) {
      return;
    }

    if (characterStats.NextLevel != null) {
      button.interactable = hasUpgradeTechnology;
    } else {
      button.interactable = false;
    }
  }

}
