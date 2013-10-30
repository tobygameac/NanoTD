#pragma strict
#pragma downcast

var background : Texture;
var myGUISkin : GUISkin;

var knowledge = new Array("病毒", "發炎", "細胞分裂", "腸病毒", "球菌", "奈米科技" , "殺菌" , "消毒" , "手術" , "皮膚" , "血管" , "血小板" , "白血球" , "紅血球" , "微血管" , "血壓" , "小腸" , "大腦" , "小腦" , "抗生素" , "藥物" , "過敏" , "免疫系統" , "安慰劑" , "心臟" , "靜脈" , "動脈" , "肌肉" , "骨骼" , "脾臟" , "腎臟" , "肝臟" , "胰臟" , "代謝" , "疫苗" , "眼睛" , "鼻子" , "口腔" , "牙齒" );

function Start () {

}

function Update () {
}

function OnGUI () {
  GUI.skin = myGUISkin;
  GUI.depth = 0;
  GUILayout.BeginArea(Rect(Screen.width - 350, Screen.height - 90, 250, 75), background);
  var choose : int = ((System.DateTime.Now.Second + Time.time) / 10) % knowledge.length;
  var knowledgeName : String = knowledge[choose];
  GUI.color = Color.black;
  if (GUI.Button(Rect(20, 15, 75, 30), knowledgeName)) {
    var link : String = "https://zh.wikipedia.org/zh-tw/" + knowledgeName;
    if (Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.OSXWebPlayer) { // WebPlayer
      link = "window.open('" + link + "' ,'_new')";
      Application.ExternalEval(link);
    } else {
      Application.OpenURL(link);
    }
  }
  GUILayout.EndArea();
}
