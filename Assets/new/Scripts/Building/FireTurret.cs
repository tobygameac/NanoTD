﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterStats))]
[RequireComponent (typeof(SphereCollider))]
public class FireTurret : MonoBehaviour {

  //public AudioClip projectileSound;

  public GameObject[] FXObject;
  private ParticleSystem[] FXParticleSystem;

  public Transform[] muzzles;
  public Transform muzzleBase;

  public float turningSpeed;
  public float warmingUpTime;
  private float nextAttackTime;

  public float attackingAngleForEachMuzzle;

  private CharacterStats characterStats;

  private Transform target;

  private static Game game;

  void Start() {
    FXParticleSystem = new ParticleSystem[FXObject.Length];
    for (int i = 0; i < FXParticleSystem.Length; ++i) {
      FXParticleSystem[i] = FXObject[i].GetComponent<ParticleSystem>();
    }

    characterStats = GetComponent<CharacterStats>();
    GetComponent<SphereCollider>().radius = characterStats.AttackingRange;

    target = null;

    if (game == null) {
      game = Camera.main.GetComponent<Game>();
    }
  }

  void Update() {
    if (target != null) {
      for (int i = 0; i < FXParticleSystem.Length; ++i) {
        if (!FXParticleSystem[i].isPlaying) {
          nextAttackTime = Time.time + warmingUpTime;
          FXParticleSystem[i].Play();
        }
      }
      Quaternion desiredRotation = Quaternion.LookRotation(target.position - muzzleBase.position);
      desiredRotation.eulerAngles = new Vector3(muzzleBase.eulerAngles.x, desiredRotation.eulerAngles.y,muzzleBase.eulerAngles.z); // y-axis only
      muzzleBase.rotation = Quaternion.Slerp(muzzleBase.rotation, desiredRotation, Time.deltaTime * turningSpeed);
    } else {
      for (int i = 0; i < FXParticleSystem.Length; ++i) {
        FXParticleSystem[i].Stop();
      }
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
      bool isPlaying = false;
      for (int i = 0; !isPlaying && i < FXParticleSystem.Length; ++i) {
        isPlaying = FXParticleSystem[i].isPlaying;
      }
      if (!isPlaying) {
        return;
      }
      if (Time.time >= nextAttackTime) {
        for (int i = 0; i < muzzles.Length; ++i) {
          Quaternion desiredRotation = Quaternion.LookRotation(collider.transform.position - muzzleBase.position);
          float angleToEnemy = Quaternion.Angle(muzzles[i].rotation, desiredRotation);
          if (angleToEnemy <= attackingAngleForEachMuzzle / 2) {
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
