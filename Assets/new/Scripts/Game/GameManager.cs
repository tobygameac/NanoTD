﻿using UnityEngine;
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
  public GameObject pathArrow;
  public float distancePerArrow = 5.0f;

  [SerializeField]
  private List<Vector3> corePositions;

  [SerializeField]
  private List<GameObject> enemyPrefabs;

  [SerializeField]
  private int waveThresholdForTheNextTypeOfEnemy = 5;

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

  private int _score;
  private int score {
    get {
      return _score;
    }
    set {
      if (gameState == GameConstants.GameState.FINISHED || gameState == GameConstants.GameState.LOSED) {
        return;
      }
      _score = value;
    }
  }
  public int Score {
    get {
      return _score;
    }
  }

  [SerializeField]
  private float restingTimeBetweenWaves;
  public float RestingTimeBetweenWaves {
    get {
      return restingTimeBetweenWaves;
    }
  }

  private float restedTime;
  public float RestedTime {
    get {
      return restedTime;
    }
  }

  private GameConstants.GameState _gameState;
  private GameConstants.GameState gameState {
    get {
      return _gameState;
    }
    set {
      _gameState = value;
      game.gameState = value;
      if (value == GameConstants.GameState.WAIT_FOR_THE_NEXT_WAVE) {
        restedTime = 0;
        //MessageManager.AddMessage("第 " + (currentWave + 1) + " 波病菌將於 " + (int)(restingTimeBetweenWaves) + " 秒後入侵");
        if ((game.GameMode == GameConstants.GameMode.SURVIVAL_NORMAL) || (game.GameMode == GameConstants.GameMode.SURVIVAL_BOSS)) {
          if (currentWave > 0) {
            int speedBonus = (int)remainingTimeOfCurrentWave * currentWave * currentWave;
            score += speedBonus;
            MessageManager.AddMessage("成功擊退第" + currentWave + "波病菌\n剩餘秒數 : " + (int)remainingTimeOfCurrentWave + "\n快速擊退加分 : " + speedBonus);
          }
        }
      }
      if (value == GameConstants.GameState.MIDDLE_OF_THE_WAVE) {
        nextGenerateEnemyTime = Time.time;
      }
    }
  }

  public GameConstants.GameState GameState {
    get {
      return _gameState;
    }
  }

  [SerializeField]
  private float maxRemainingTimeForEachWave = 90.0f;

  private float remainingTimeOfCurrentWave;
  public float RemainingTimeOfCurrentWave {
    get {
      return remainingTimeOfCurrentWave;
    }
  }

  private float timeBetweenGenerateEnemy;
  private float nextGenerateEnemyTime;

  private int numberOfEnemiesToGenerate;
  public int NumberOfEnemiesToGenerate {
    get {
      return numberOfEnemiesToGenerate;
    }
    set {
      numberOfEnemiesToGenerate = value;
    }
  }

  private int numberOfEnemiesOnMap;
  public int NumberOfEnemiesOnMap {
    get {
      return numberOfEnemiesOnMap;
    }
    set {
      numberOfEnemiesOnMap = value;
    }
  }

  public void KillEnemyWithCost(int cost) {
    score += cost;
    game.AddMoney(cost);
    --numberOfEnemiesOnMap;
  }

  public void OnSubmitScoreButtonClick() {
    game.SubmitScore(score);
  }

  void Start() {
    game = GetComponent<Game>();
    gameState = GameConstants.GameState.WAIT_FOR_THE_NEXT_WAVE;
    GeneratePathArrows(enemyPath, distancePerArrow);

    MessageManager.AddMessage("請建造攻擊裝置抵擋即將入侵的病菌");
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
      if (currentWave < maxWave || (game.GameMode == GameConstants.GameMode.SURVIVAL_NORMAL) || (game.GameMode == GameConstants.GameMode.SURVIVAL_BOSS)) {
        remainingTimeOfCurrentWave -= Time.deltaTime;
        if (remainingTimeOfCurrentWave < 0) {
          gameState = GameConstants.GameState.LOSED;
          MessageManager.AddMessage("遊戲結束，請輸入您的名稱將分數登入排行榜");
          return;
        }
      }
      if (numberOfEnemiesToGenerate > 0) {
        if (Time.time >= nextGenerateEnemyTime) {
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

  private void GeneratePathArrows(List<Vector3> path, float distancePerArrow) {
    if (path.Count == 0) {
      return;
    }
    if (distancePerArrow <= 1e-5) {
      return;
    }
    for (int i = 0; i < path.Count; ++i) {
      Vector3 arrowDirection = (path[i] - path[(i + path.Count - 1) % path.Count]).normalized;
      Quaternion arrowAngle = Quaternion.FromToRotation(Vector3.forward, arrowDirection);
      float distanceBetweenPoints = Vector3.Distance(path[i], path[(i + path.Count - 1) % path.Count]);
      float gapT = distancePerArrow / distanceBetweenPoints;
      if (distanceBetweenPoints <= 1e-5) {
        continue;
      }
      for (float t = gapT; t < 1; t += gapT) {
        Vector3 arrowPosition = Vector3.Lerp(path[(i + path.Count - 1) % path.Count], path[i], t) + new Vector3(0, 0.005f, 0);
        Instantiate(pathArrow, arrowPosition, arrowAngle);
      }
    }
  }

  private void GenerateEnemies() {
    int indexRangeOfEnemyToGenerate = CurrentWave;

    if (waveThresholdForTheNextTypeOfEnemy > 0) {
      indexRangeOfEnemyToGenerate /= waveThresholdForTheNextTypeOfEnemy;
      ++indexRangeOfEnemyToGenerate;
    }
    if (indexRangeOfEnemyToGenerate > enemyPrefabs.Count) {
      indexRangeOfEnemyToGenerate = enemyPrefabs.Count;
    }

    GameObject enemyPrefab = enemyPrefabs[Random.Range(0, indexRangeOfEnemyToGenerate)];

    int spwaningPointIndex = Random.Range(0, spawningPoints.Count);
    Vector3 spawningPosition = spawningPoints[spwaningPointIndex].position;

    GameObject newEnemy = CharacterGenerator.GenerateCharacter(enemyPrefab, spawningPosition, enemyPath);

    EnemyStatsModifier.AddRandomImprovementWithWave(newEnemy, currentWave);
    EnemyStatsModifier.ModifyStatsWithWave(newEnemy.GetComponent<CharacterStats>(), currentWave);

    --numberOfEnemiesToGenerate;
    ++numberOfEnemiesOnMap;

    nextGenerateEnemyTime = Time.time + timeBetweenGenerateEnemy;
  }

  private void NextWave() {
    ++currentWave;
      /* temp */
      /* temp */
      /* temp */
    numberOfEnemiesToGenerate = 10 + (currentWave - 1) * 5 * (int)Mathf.Pow(1.1f, currentWave);
    if ((game.GameMode == GameConstants.GameMode.SURVIVAL_NORMAL) || (game.GameMode == GameConstants.GameMode.SURVIVAL_BOSS)) {
      remainingTimeOfCurrentWave = 45 + ((currentWave - 1) * 5);
      /* temp */
      /* temp */
      /* temp */
      if (remainingTimeOfCurrentWave > maxRemainingTimeForEachWave) {
        if (maxRemainingTimeForEachWave > 0) {
          remainingTimeOfCurrentWave = maxRemainingTimeForEachWave;
        }
      }
    }
    
    timeBetweenGenerateEnemy = (remainingTimeOfCurrentWave / numberOfEnemiesToGenerate) / 3.0f;
  }

}
