using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Game))]
public class GameManager : MonoBehaviour {

  private Game game;

  public List<Component> componentsToAdd;

  [SerializeField]
  private List<Transform> spawningPoints;

  [SerializeField]
  private List<Vector3> enemyPath;

  [SerializeField]
  private List<Vector3> corePositions;

  [SerializeField]
  private List<GameObject> enemyPrefabs;

  [SerializeField]
  private int _currentWave;
  private int currentWave {
    get {
      return _currentWave;
    }
    set {
      _currentWave = value;
    }
  }
  public int CurrentWave {
    get {
      return _currentWave;
    }
  }


  [SerializeField]
  private int _maxWave;
  private int maxWave {
    get {
      return _maxWave;
    }
    set {
      _maxWave = value;
    }
  }
  public int MaxWave {
    get {
      return _maxWave;
    }
  }

  [SerializeField]
  private float restingTimeBetweenWaves;

  private float restedTime;

  private GameConstants.GameState _gameState;
  private GameConstants.GameState gameState {
    get {
      return _gameState;
    }
    set {
      _gameState = value;
      if (value == GameConstants.GameState.WAIT_FOR_THE_NEXT_WAVE) {
        restedTime = 0;
        MessageManager.AddMessage("第 " + (currentWave + 1) + " 波病菌將於 " + (int)(restingTimeBetweenWaves) + " 秒後入侵");
      }
      if (value == GameConstants.GameState.MIDDLE_OF_THE_WAVE) {
        generateEnemyTime = Time.time;
      }
    }
  }

  [SerializeField]
  private float timeBetweenGenerateEnemy;
  private float generateEnemyTime;

  private int numberOfEnemiesToGenerate;
  public int NumberOfEnemiesToGenerate {
    get {
      return numberOfEnemiesToGenerate;
    }
  }

  private int numberOfEnemiesOnMap;
  public int NumberOfEnemiesOnMap {
    get {
      return numberOfEnemiesOnMap;
    }
  }

  public void KillEnemyWithCost(int cost) {
    game.AddMoney(cost);
    numberOfEnemiesOnMap--;
  }

  void Start() {
    game = GetComponent<Game>();
    gameState = GameConstants.GameState.WAIT_FOR_THE_NEXT_WAVE;
  }

  void Update() {
    if (gameState == GameConstants.GameState.FINISHED || gameState == GameConstants.GameState.LOSED) {
      return;
    }

    if (gameState == GameConstants.GameState.WAIT_FOR_THE_NEXT_WAVE) {
      restedTime += Time.deltaTime;
      if (restedTime >= restingTimeBetweenWaves) {
        NextWave();
        gameState = GameConstants.GameState.MIDDLE_OF_THE_WAVE;
      }
      return;
    }

    if (gameState == GameConstants.GameState.MIDDLE_OF_THE_WAVE) {
      if (numberOfEnemiesToGenerate > 0) {
        if (Time.time >= generateEnemyTime) {
          GenerateEnemies();
        }
      } else if (numberOfEnemiesOnMap > 0) {
        return;
      } else {
        if (numberOfEnemiesToGenerate <= 0 && numberOfEnemiesOnMap <= 0) {
          if (currentWave < maxWave || (game.GameMode == GameConstants.GameMode.SURVIVAL_NORMAL) || (game.GameMode == GameConstants.GameMode.SURVIVAL_BOSS)) {
            gameState = GameConstants.GameState.WAIT_FOR_THE_NEXT_WAVE;
          } else {
            gameState = GameConstants.GameState.FINISHED;
          }
        }
      }
      return;
    }
  }

  private void GenerateEnemies() {
    for (int i = 0; i < spawningPoints.Count && numberOfEnemiesToGenerate > 0; ++i) {
      GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
      GameObject newEnemy = CharacterGenerator.GenerateCharacter(enemyPrefab, spawningPoints[i].position, enemyPath);
      EnemyStatsModifier.ModifyStatsWithWave(newEnemy.GetComponent<CharacterStats>(), currentWave);
      --numberOfEnemiesToGenerate;
      ++numberOfEnemiesOnMap;
    }
    generateEnemyTime = Time.time + timeBetweenGenerateEnemy;
  }

  private void NextWave() {
    ++currentWave;
    numberOfEnemiesToGenerate = currentWave * 10;
  }

}
