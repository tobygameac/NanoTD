using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterStats))]
[RequireComponent (typeof(SphereCollider))]
public class SuperBurningDevice : MonoBehaviour {

  public GameObject explosion;

  public float additionalBurningFXTime = 3.0f;

  public GameObject muzzleBase;
  private Rotating muzzleBaseRotating;

  private float attackingRange;

  private CharacterStats characterStats;

  private float lastAttackingTime;

  public float FXduration = 2.5f;
  private float nextFXTime;

  private static Game game;

  void Start() {
    characterStats = GetComponent<CharacterStats>();

    attackingRange = characterStats.AttackingRange;

    GetComponent<SphereCollider>().radius = attackingRange;

    muzzleBaseRotating = muzzleBase.GetComponent<Rotating>();

    if (game == null) {
      game = Camera.main.GetComponent<Game>();
    }

    lastAttackingTime = Time.time - additionalBurningFXTime - 1;
    nextFXTime = Time.time;
  }

  void Update() {
    if (Time.time - lastAttackingTime <= additionalBurningFXTime) {
      muzzleBaseRotating.enabled = true;
    } else {
      muzzleBaseRotating.enabled = false;
    }
  }

  void OnTriggerStay(Collider collider) {
    if (collider.gameObject.tag == "Enemy") {
      if (Time.time >= nextFXTime) {
        GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
        Destroy(newExplosion, FXduration);
        nextFXTime = Time.time + FXduration;
      }
      lastAttackingTime = Time.time;

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
