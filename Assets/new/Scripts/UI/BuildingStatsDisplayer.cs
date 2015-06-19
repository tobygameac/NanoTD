﻿using UnityEngine;
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

    if ((int)characterStats.BuildingID >= 0 && (int)characterStats.BuildingID < iconSprites.Length) {
      buildingIconImage.enabled = true;
      buildingIconImage.sprite = iconSprites[(int)characterStats.BuildingID];
    } else {
      buildingIconImage.enabled = false;
    }

    buildingStatsText.text = "<color=#0f0f0fff>" + GameConstants.NameOfBuildingID[(int)characterStats.BuildingID] + "</color>\n";
    buildingStatsText.text += "<color=red>價值 : </color><color=yellow>" + characterStats.Cost + "</color>\n\n";
    if (characterStats.BuildingID == GameConstants.BuildingID.SLOWING_DEVICE) {
      buildingStatsText.text += "<color=brown>減緩移動速度 : </color><color=blue>" + (characterStats.Damage * 100).ToString("0.00") + "%\n";
    } else if (characterStats.BuildingID == GameConstants.BuildingID.FIRE_STORM_DEVICE) {
      float damageScale = building.GetComponent<FireStormDevice>().DamageScale;
      buildingStatsText.text += "<color=brown>減緩移動速度 : </color><color=blue>" + (characterStats.Damage * 100).ToString("0.00") + "%</color>\n";
      if (game.HasTechnology(GameConstants.TechnologyID.SELF_LEARNING)) {
        buildingStatsText.text += "<color=red>(+" + (characterStats.DamageModifier * characterStats.BasicDamage).ToString("0.0") + ")</color>";
      }
      buildingStatsText.text += "<color=brown>傷害 : </color><color=blue>" + (characterStats.Damage * damageScale).ToString("0.0");
    } else {
      buildingStatsText.text += "<color=brown>傷害 : </color><color=blue>" + (characterStats.Damage).ToString("0.0");
    }
    buildingStatsText.text += "</color>";
    if (game.HasTechnology(GameConstants.TechnologyID.SELF_LEARNING)) {
      if (characterStats.BuildingID == GameConstants.BuildingID.SLOWING_DEVICE) {
      } else if (characterStats.BuildingID == GameConstants.BuildingID.FIRE_STORM_DEVICE) {
        float damageScale = building.GetComponent<FireStormDevice>().DamageScale;
        buildingStatsText.text += "<color=red>(+" + (characterStats.DamageModifier * characterStats.BasicDamage * damageScale).ToString("0.0") + ")</color>";
      } else {
        buildingStatsText.text += "<color=red>(+" + (characterStats.DamageModifier * characterStats.BasicDamage).ToString("0.0") + ")</color>";
      }
    }
    buildingStatsText.text += "\n";
    buildingStatsText.text += "<color=brown>攻擊範圍 : </color><color=blue>" + characterStats.AttackingRange + "</color>\n";
    buildingStatsText.text += "<color=brown>擊殺數 : </color><color=blue>" + characterStats.UnitKilled + "</color>\n";
  }
}
