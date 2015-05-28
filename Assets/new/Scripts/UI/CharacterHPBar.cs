using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterHPBar : MonoBehaviour {

  public GameObject character;
  
  public GameObject HPBarImage;
  //public GameObject HPBarText;

  private CharacterStats characterStats;

  private RectTransform HPBarRectTransform;

  private Vector2 originalSize;
  private Vector3 originalLocalPosition;
  private Quaternion originalRotation;

  //private Text text;

  void Start() {
    characterStats = character.GetComponent<CharacterStats>();
    HPBarRectTransform = HPBarImage.GetComponent<RectTransform>();
    originalSize = HPBarRectTransform.sizeDelta;
    originalLocalPosition = HPBarRectTransform.localPosition;
    originalRotation = transform.rotation;
    
    //text = HPBarText.GetComponent<Text>();
  }

  void Update() {
    float percentOfHP = (characterStats.CurrentHP / characterStats.MaxHP);
    HPBarRectTransform.localPosition = originalLocalPosition + new Vector3(-1 * originalSize.x * (1 - percentOfHP) * 0.5f, 0, 0);
    HPBarRectTransform.sizeDelta = Vector2.Scale(originalSize, new Vector2(percentOfHP, 1));
    transform.rotation = originalRotation;
    //text.text = (int)characterStats.CurrentHP + " / " + (int)characterStats.MaxHP;
  }
}
