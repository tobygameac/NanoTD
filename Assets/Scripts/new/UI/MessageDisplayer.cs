using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageDisplayer : MonoBehaviour {

  public int maxDisplayMessageNumber = 5;

  private Text text;

  void Start() {
    text = GetComponent<Text>();
  }

  void Update() {
    string messageToDisplay = "";
    for (int i = MessageManager.Messages.Count - 1; i >= Mathf.Max(0, MessageManager.Messages.Count - maxDisplayMessageNumber); --i) {
      messageToDisplay += MessageManager.Messages[i] + "\n";
    }
    text.text = messageToDisplay;
  }
}
