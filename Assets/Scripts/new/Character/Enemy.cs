using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterStats))]
public class Enemy : MonoBehaviour {

  private GameManager gameManager;
  private CharacterStats characterStats;

  private float previousHP;

  private int currentTargetIndex;

  [SerializeField]
  private float minStoppingDistance = 0.1f;

  void Start() {
    gameManager = Camera.main.GetComponent<GameManager>();

    characterStats = GetComponent<CharacterStats>();

    previousHP = characterStats.MaxHP;

    currentTargetIndex = 0;
  }

  void Update() {
    transform.position = Vector3.MoveTowards(transform.position, characterStats.Path[currentTargetIndex], characterStats.MovingSpeed * Time.deltaTime);

    if (characterStats.CurrentHP <= 0) {
      if (previousHP == characterStats.MaxHP) { // One shot kill
        gameManager.KillEnemyWithCost(characterStats.Cost * 2);
        MessageManager.AddMessage("瞬間擊殺! 獎賞加倍");
      } else {
        gameManager.KillEnemyWithCost(characterStats.Cost);
      }
      Destroy(gameObject);
    }

    previousHP = characterStats.CurrentHP;

    if (CloseEnoughToCurrentTarget()) {
      currentTargetIndex = (currentTargetIndex + 1) % characterStats.Path.Count;
    }
  }

  private bool CloseEnoughToCurrentTarget() {
    return (Vector3.Distance(transform.position, characterStats.Path[currentTargetIndex]) <= minStoppingDistance);
  }
}
