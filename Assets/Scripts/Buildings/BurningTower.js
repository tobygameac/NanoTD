#pragma strict

private var damage : float;
var target : GameObject;
var burningEffect : Transform;

var targetLosingTime : float;

function Start () {
  damage = GetComponent(Stats).damage;
  burningEffect = transform.Find("burning");
  targetLosingTime = Time.time - 5;
}

function Update () {
  if (Time.time - targetLosingTime <= 3) {
    if (!audio.loop) {
      audio.Play();
      audio.loop = true;
    }
    burningEffect.renderer.enabled = true;
  } else {
    audio.Stop();
    audio.loop = false;
    burningEffect.renderer.enabled = false;
  }
  if (target) {
    target = null;
    targetLosingTime = Time.time;
  }
}

function OnTriggerEnter (other : Collider) {
  if (other.gameObject.tag == "Enemy") {
    target = other.gameObject;
    if (!other.gameObject.GetComponent(Burning)) {
      other.gameObject.AddComponent(Burning);
      other.gameObject.GetComponent(Burning).damage = damage;
      other.gameObject.GetComponent(Burning).bonusDamage = ((GetComponent(Stats).kill / 10.0f) / 100.0f) * damage;
      other.gameObject.GetComponent(Burning).giver = gameObject;
    } else {
      other.gameObject.GetComponent(Burning).damage += damage;
      other.gameObject.GetComponent(Burning).bonusDamage += ((GetComponent(Stats).kill / 10.0f) / 100.0f) * damage;
      other.gameObject.GetComponent(Burning).giver = gameObject;
    }
  }
}

function OnTriggerStay (other : Collider) {
  if (other.gameObject.tag == "Enemy") {
    target = other.gameObject;
  }
}

function OnTriggerExit (other : Collider) {
  if (other.gameObject.tag == "Enemy") {
    if (other.gameObject.GetComponent(Burning)) {
      other.gameObject.GetComponent(Burning).damage -= damage;
      other.gameObject.GetComponent(Burning).bonusDamage -= GetComponent(Stats).kill / 10;
      if (other.gameObject.GetComponent(Burning).damage < 0 || other.gameObject.GetComponent(Burning).bonusDamage < 0) {
        other.gameObject.GetComponent(Burning).damage = other.gameObject.GetComponent(Burning).bonusDamage = 0;
      }
    }
  }
}
