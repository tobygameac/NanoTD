using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterStats))]
[RequireComponent (typeof(SphereCollider))]
public class FireTurret : MonoBehaviour {

  //public AudioClip projectileSound;

  public GameObject FXObject;
  private ParticleSystem FXParticleSystem;

  public Transform turretBall;

  public float turningSpeed;
  public float warmingUpTime = 1.0f;
  private float nextAttackTime;

  public float attackingAngle;

  private float attackingRange;

  private Transform target;

  private CharacterStats characterStats;

  private static Game game;

  void Start() {
    FXParticleSystem = FXObject.GetComponent<ParticleSystem>();

    characterStats = GetComponent<CharacterStats>();
    GetComponent<SphereCollider>().radius = characterStats.AttackingRange;

    target = null;

    if (game == null) {
      game = Camera.main.GetComponent<Game>();
    }
  }

  void Update() {
    if (target != null) {
      if (!FXParticleSystem.isPlaying) {
        nextAttackTime = Time.time + warmingUpTime;
        FXParticleSystem.Play();
      }
      Quaternion desiredRotation = Quaternion.LookRotation(target.position - turretBall.position);
      desiredRotation.eulerAngles = new Vector3(turretBall.eulerAngles.x, desiredRotation.eulerAngles.y,turretBall.eulerAngles.z); // y-axis only
      turretBall.rotation = Quaternion.Slerp(turretBall.rotation, desiredRotation, Time.deltaTime * turningSpeed);
    } else {
      FXParticleSystem.Stop();
    }
  }

  void OnTriggerEnter(Collider collider) {
    if (target == null && collider.gameObject.tag == "Enemy") {
      target = collider.gameObject.transform;
    }
  }

  void OnTriggerStay(Collider collider) {
    if (collider.gameObject.tag == "Enemy") {
      if (target == null) {
        target = collider.gameObject.transform;
      }
      if (!FXParticleSystem.isPlaying) {
        return;
      }
      if (Time.time >= nextAttackTime) {
        Quaternion desiredRotation = Quaternion.LookRotation(collider.transform.position - turretBall.position);
        float angleToEnemy = Quaternion.Angle(turretBall.rotation, desiredRotation);
        if (angleToEnemy <= attackingAngle / 2) {
          CharacterStats targetCharacterStats = collider.GetComponent<CharacterStats>();
          targetCharacterStats.CurrentHP -= characterStats.Damage * Time.deltaTime;
          if (targetCharacterStats.CurrentHP <= 0) {
            ++characterStats.UnitKilled;
            if (game.HasTechnology(GameConstants.TechnologyID.SELF_LEARNING)) {
              characterStats.DamageModifier += GameConstants.SELF_LEARNING_IMPROVEMENT_PERCENT_PER_KILL;
            } else {
            }
          }
        }
      }
    }
  }

  void OnTriggerExit(Collider collider) {
    if (target == collider.transform) {
      target = null;
    }
  }

  public void DealDamage(GameObject enemyGameObject) {
    CharacterStats enemyCharacterStats = enemyGameObject.GetComponent<CharacterStats>();
    enemyCharacterStats.CurrentHP -= characterStats.Damage;
    if (enemyCharacterStats.CurrentHP <= 0) {
      ++characterStats.UnitKilled;
      if (game.HasTechnology(GameConstants.TechnologyID.SELF_LEARNING)) {
        characterStats.DamageModifier += GameConstants.SELF_LEARNING_IMPROVEMENT_PERCENT_PER_KILL;
      } else {
      }
    }
  }

}
