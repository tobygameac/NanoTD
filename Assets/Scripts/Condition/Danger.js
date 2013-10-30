#pragma strict

var timeRemain : float;
var originalColor : Color;

function Start () {
  originalColor = transform.light.color;
  transform.light.color = Color.red;
  timeRemain = 0.2;
}

function Update () {
  timeRemain -= Time.deltaTime;
  if (timeRemain <= 0) {
    transform.light.color = originalColor;
    Destroy(transform.GetComponent(Danger));
  }
}
