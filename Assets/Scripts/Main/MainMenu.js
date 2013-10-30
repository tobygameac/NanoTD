#pragma strict

var myGUISkin : GUISkin;
var background : Texture;
var teamPicture : Texture;

// State
var isAdjusting : boolean;
var isSelectingMode : boolean;
var isSelectingSurvivalMode : boolean;
var isCheckAbout : boolean;

// Sound
var buttonSound : AudioClip;

function Start () {
  AudioListener.volume = 0.5;
}

function Update () {
  if (Input.GetKeyUp(KeyCode.F12)) {
    Application.LoadLevel("Demo");
  }
}

function OnGUI () {
  GUI.skin = myGUISkin;
  if (isCheckAbout) { // Team Picture
    GUI.DrawTexture(Rect(0, 0, Screen.width, Screen.height), teamPicture, ScaleMode.StretchToFill, true, 10.0f);
  } else { // Game Picture
    GUI.DrawTexture(Rect(0, 0, Screen.width, Screen.height), background, ScaleMode.StretchToFill, true, 10.0f);
  }
  GUI.color = Color.black;
  // GUI.Label(Rect(0, 0, 100, 100), "Last Update : " + "6/15 23.00");
  GUI.Label(Rect(0, 0, 100, 100), "1.0");
  GUILayout.BeginArea(Rect(Screen.width / 2 - 50, 200, 500, 500));
  if (isAdjusting) {
    AudioListener.volume = GUI.HorizontalScrollbar(Rect(0, 0, 100, 50), AudioListener.volume, 0.01, 0.0, 1.0);
    if (GUI.Button(Rect(0, 75, 100, 50), "返回")) {
      audio.PlayOneShot(buttonSound);
      isAdjusting = false;
    }
  } else if (isSelectingSurvivalMode) {
    if (GUI.Button(Rect(0, 0, 100, 50), "普通病菌")) {
      audio.PlayOneShot(buttonSound);
      Time.timeScale = 1;
      Application.LoadLevel("SurvivalNormal");
    }
    if (GUI.Button(Rect(0, 75, 100, 50), "超級病菌")) {
      audio.PlayOneShot(buttonSound);
      Time.timeScale = 1;
      Application.LoadLevel("SurvivalBoss");
    }
    if (GUI.Button(Rect(0, 150, 100, 50), "返回")) {
      audio.PlayOneShot(buttonSound);
      isSelectingSurvivalMode = false;
    }
  } else if (isSelectingMode) {
    if (GUI.Button(Rect(0, 0, 100, 50), "劇情模式")) {
      audio.PlayOneShot(buttonSound);
      Time.timeScale = 1;
      Application.LoadLevel("Level1");
    }
    if (GUI.Button(Rect(0, 75, 100, 50), "生存模式")) {
      audio.PlayOneShot(buttonSound);
      isSelectingSurvivalMode = true;
    }
    if (GUI.Button(Rect(0, 150, 100, 50), "返回")) {
      audio.PlayOneShot(buttonSound);
      isSelectingMode = false;
    }
  } else if (isCheckAbout) { // About
    if (GUI.Button(Rect(0, 150, 100, 50), "返回")) {
      audio.PlayOneShot(buttonSound);
      isCheckAbout = false;
    }
  } else {
    if (GUI.Button(Rect(0, 0, 100, 50), "開始遊戲")) {
      audio.PlayOneShot(buttonSound);
      isSelectingMode = true;
    }
    if (GUI.Button(Rect(0, 75, 100, 50), "新手教學")) {
      audio.PlayOneShot(buttonSound);
      Application.LoadLevel("Tutorial");
    }
    if (GUI.Button(Rect(0, 150, 100, 50), "音量調整")) {
      audio.PlayOneShot(buttonSound);
      isAdjusting = true;
    }
    if (GUI.Button(Rect(0, 225, 100, 50), "關於")) {
      audio.PlayOneShot(buttonSound);
      isCheckAbout = true;
    }
    if (GUI.Button(Rect(0, 300, 100, 50), "離開")) {
      audio.PlayOneShot(buttonSound);
      Application.Quit();
    }
  }
  GUILayout.EndArea();
}
