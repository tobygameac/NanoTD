#pragma strict

var cursorImage : Texture;
 
function Start () {
  Screen.showCursor = false;
}
 
function OnGUI () {
  GUI.depth = 0;
  var mousePos : Vector3 = Input.mousePosition;
  GUI.DrawTexture(Rect(mousePos.x, Screen.height - mousePos.y, 25, 25), cursorImage);
}
