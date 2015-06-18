using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterStats))]
[RequireComponent (typeof(SphereCollider))]
public class SlowingDevice : MonoBehaviour {

  public GameObject slowingFXObject;
  private ParticleSystem slowingFXParticleSystem;

  public float additionalslowingFXTime = 3.0f;

  public GameObject deviceBase;
  private Rotating deviceBaseRotating;

  private float attackingRange;

  private CharacterStats characterStats;

  private float lastAttackingTime;

  private static Game game;

  void Start() {
    characterStats = GetComponent<CharacterStats>();

    attackingRange = characterStats.AttackingRange;

    GetComponent<SphereCollider>().radius = attackingRange;
    slowingFXParticleSystem = slowingFXObject.GetComponent<ParticleSystem>();
    slowingFXParticleSystem.startSize *= attackingRange;

    deviceBaseRotating = deviceBase.GetComponent<Rotating>();

    if (game == null) {
      game = Camera.main.GetComponent<Game>();
    }

    lastAttackingTime = Time.time - additionalslowingFXTime - 1;
  }

  void Update() {
    if (Time.time - lastAttackingTime <= additionalslowingFXTime) {
      slowingFXParticleSystem.loop = true;
      if (slowingFXParticleSystem.isStopped) {
        slowingFXParticleSystem.Play();
      }
      deviceBaseRotating.enabled = true;
    } else {
      slowingFXParticleSystem.loop = false;
      deviceBaseRotating.enabled = false;
    }
  }

  void OnTriggerEnter(Collider collider) {
    if (collider.gameObject.tag == "Enemy") {
      lastAttackingTime = Time.time;

      CharacterStats targetCharacterStats = collider.GetComponent<CharacterStats>();
      targetCharacterStats.MovingSpeedModifier -= characterStats.Damage;
    }
  }

  void OnTriggerExit(Collider collider) {
    if (collider.gameObject.tag == "Enemy") {
      CharacterStats targetCharacterStats = collider.GetComponent<CharacterStats>();
      targetCharacterStats.MovingSpeedModifier += characterStats.Damage;
    }
  }
}
