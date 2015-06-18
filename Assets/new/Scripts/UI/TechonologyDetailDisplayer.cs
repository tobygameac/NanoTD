using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TechonologyDetailDisplayer : MonoBehaviour {

  public Sprite[] iconSprites;

  /*
  public GameObject TechnologyIcon;
  private Image TechnologyIconImage;
  */

  public GameObject TechnologyDetail;
  private Text TechnologyDetailText;

  private Game game;

  private Technology previousViewingTechnology;

  void Start() {
    //TechnologyIconImage = TechnologyIcon.GetComponent<Image>();
    TechnologyDetailText = TechnologyDetail.GetComponent<Text>();

    game = Camera.main.GetComponent<Game>();

    previousViewingTechnology = null;
  }

  void Update() {
    if (previousViewingTechnology != game.ViewingTechnology) {
      UpdateTechnologyDetail();
      previousViewingTechnology = game.ViewingTechnology;
    }
  }

  void UpdateTechnologyDetail() {
    if (game.ViewingTechnology != null) {
      //TechnologyIconImage.sprite = iconSprites[(int)game.ViewingTechnology.ID];

      TechnologyDetailText.text = "需要金錢 : " + game.ViewingTechnology.Cost + "\n";
      TechnologyDetailText.text += GameConstants.DetailOfTechnologyID[(int)game.ViewingTechnology.ID];

    } else {
      //TechnologyIconImage.sprite = null;
      TechnologyDetailText.text = "";
    }
  }
}
