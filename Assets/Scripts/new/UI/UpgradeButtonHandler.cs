using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeButtonHandler : MonoBehaviour {

  private Game game;
  private Button button;
  private Text buttonText;
  private string buttonOriginalText;
  private bool hasUpgradeTechnology;
  
  private GameObject lastBuilding;
  private CharacterStats characterStats;


  void Start() {
    game = Camera.main.GetComponent<Game>();
    button = GetComponent<Button>();
    button.interactable = false;
    buttonText = button.transform.GetChild(0).GetComponent<Text>();
    buttonOriginalText = buttonText.text;

    hasUpgradeTechnology = false;

    lastBuilding = null;
  }

  void Update() {

    if (!hasUpgradeTechnology) {
      hasUpgradeTechnology = game.HasTechnology(GameConstants.TechnologyID.UPGRADE);
      return;
    }

    GameObject building = game.SelectedBuilding;

    if (building != null && building != lastBuilding) {
      characterStats = building.GetComponent<CharacterStats>();
      lastBuilding = building;

      if (characterStats.NextLevel != null) {
        button.interactable = hasUpgradeTechnology;
        int nextLevelCost = characterStats.NextLevel.GetComponent<CharacterStats>().Cost;
        buttonText.text = buttonOriginalText + "(" + (nextLevelCost - characterStats.Cost) + ")";
      } else {
        button.interactable = false;
        buttonText.text = buttonOriginalText;
      }
    }

    if (building == null) {
      buttonText.text = buttonOriginalText;
      return;
    }

  }

}
