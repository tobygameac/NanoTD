using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Text))]
public class MoneyText : MonoBehaviour {

  private Game game;
  private Text text;
  
  void Start() {
    game = Camera.main.GetComponent<Game>();
    text = GetComponent<Text>();
  }

  void Update() {
    text.text = "金錢 : " + game.Money;
  }
}
