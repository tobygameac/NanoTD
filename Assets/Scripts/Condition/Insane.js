#pragma strict

function Start () {
  GetComponent(Monster).dx *= 1.2;
  GetComponent(Monster).dz *= 1.2;
  renderer.material.color = Color.red;
}

function Update () {

}
