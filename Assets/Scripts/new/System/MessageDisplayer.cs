using UnityEngine;
using System.Collections;

public class MessageDisplayer : MonoBehaviour {

  public Texture backgroundTexture;

  public int maxDisplayMessageNumber = 10;

  void OnGUI() {
    string messageToDisplay = "";
    for (int i = MessageManager.Messages.Count - 1; i >= Mathf.Max(0, MessageManager.Messages.Count - maxDisplayMessageNumber); --i) {
      messageToDisplay += MessageManager.Messages[i] + "\n";
    }
    
    GUI.depth = 0;

    float backgroundWidth = Screen.width / 6;
    float backgroundHeight = Screen.height / 6;
    Rect backgroundArea = new Rect(Screen.width - backgroundWidth * 1.5f, backgroundHeight * 0.3f, backgroundWidth, backgroundHeight);
    GUI.DrawTexture(backgroundArea, backgroundTexture);
    GUI.color = Color.red;
    float messageWidth = backgroundWidth * 0.8f;
    float messageHeight = backgroundHeight * 0.8f;
    Rect messageArea = new Rect(backgroundArea.x + (backgroundWidth - messageWidth) / 2, backgroundArea.y + (backgroundHeight - messageHeight) / 2, messageWidth, messageHeight);
    GUI.Label(messageArea, messageToDisplay);
  }
}
