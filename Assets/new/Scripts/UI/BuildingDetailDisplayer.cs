using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingDetailDisplayer : MonoBehaviour {

  public Sprite[] iconSprites;

  public GameObject buildingIcon;
  private Image buildingIconImage;

  public GameObject buildingDetail;
  private Text buildingDetailText;

  private Game game;

  private int previousViewingBuildingIndex;

  void Start() {
    buildingIconImage = buildingIcon.GetComponent<Image>();
    buildingDetailText = buildingDetail.GetComponent<Text>();

    game = Camera.main.GetComponent<Game>();

    previousViewingBuildingIndex = -1;
  }

  void Update() {
    if (previousViewingBuildingIndex != game.ViewingBuildingIndex) {
      UpdateBuildingDetail();
      previousViewingBuildingIndex = game.ViewingBuildingIndex;
    }
  }

  void UpdateBuildingDetail() {
    if (game.ViewingBuildingIndex >= 0) {
      CharacterStats characterStats = game.BuildingList[game.ViewingBuildingIndex].GetComponent<CharacterStats>();
      buildingIconImage.sprite = iconSprites[(int)characterStats.BuildingID];

      buildingDetailText.text = GameConstants.NameOfBuildingID[(int)characterStats.BuildingID] + "\n\n";

      buildingDetailText.text += characterStats.description + "\n\n";

      buildingDetailText.text += "價值 : " + characterStats.Cost + "\n\n";
      buildingDetailText.text += "傷害 : " + characterStats.Damage + "\n";
      buildingDetailText.text += "攻擊範圍 : " + characterStats.AttackingRange + "\n";
    } else {
      buildingIconImage.sprite = null;
      buildingDetailText.text = "";
    }
  }
}
