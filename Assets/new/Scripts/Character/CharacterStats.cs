using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterStats : MonoBehaviour {

  public GameObject rangeDisplayer;

  public string description;

  [SerializeField]
  private GameConstants.BuildingID _buildingID;
  public GameConstants.BuildingID BuildingID {
    get {
      return _buildingID;
    }
  }

  [SerializeField]
  private GameConstants.EnemyID _enemyID;
  public GameConstants.EnemyID EnemyID {
    get {
      return _enemyID;
    }
  }

  // HP
  [SerializeField]
  private float _basicHP;
  private float BasicHP {
    get {
      return _basicHP;
    }
  }

  private float _currentHP;
  public float CurrentHP {
    get {
      return _currentHP;
    }
    set {
      _currentHP = value;
    }
  }

  private float _maxHP;
  public float MaxHP {
    get {
      return _maxHP;
    }
    set {
      _maxHP = value;
    }
  }

  [SerializeField]
  private float _hPModifier;
  public float HPModifier {
    get {
      return _hPModifier;
    }
    set {
      _hPModifier = value;
      if (MaxHP <= 0) {
        CurrentHP = MaxHP = BasicHP;
      } else {
        CurrentHP = (CurrentHP / MaxHP) * MaxHP * (1 + value);
        MaxHP = BasicHP * (1 + value);
      }
    }
  }

  // Damage
  [SerializeField]
  private float _basicDamage;
  public float BasicDamage {
    get {
      return _basicDamage;
    }
  }

  private float _damage;
  public float Damage {
    get {
      return _damage;
    }
    set {
      _damage = value;
    }
  }

  [SerializeField]
  private float _damageModifier;
  public float DamageModifier {
    get {
      return _damageModifier;
    }
    set {
      _damageModifier = value;
      Damage = BasicDamage * (1 + value);
    }
  }

  // MovingSpeed
  [SerializeField]
  private float _basicMovingSpeed;
  public float BasicMovingSpeed {
    get {
      return _basicMovingSpeed;
    }
  }

  private float _movingSpeed;
  public float MovingSpeed {
    get {
      return _movingSpeed;
    }
    set {
      _movingSpeed = value;
    }
  }

  [SerializeField]
  private float _movingSpeedModifier;
  public float MovingSpeedModifier {
    get {
      return _movingSpeedModifier;
    }
    set {
      _movingSpeedModifier = value;
      MovingSpeed = BasicMovingSpeed * (1 + value);
    }
  }

  //
  [SerializeField]
  private int _cost;
  public int Cost {
    get {
      return _cost;
    }
    set {
      _cost = value;
    }
  }

  [SerializeField]
  private int _attackingRange;
  public int AttackingRange {
    get {
      return _attackingRange;
    }
    set {
      _attackingRange = value;
    }
  }

  private int _unitKilled;
  public int UnitKilled {
    get {
      return _unitKilled;
    }
    set {
      _unitKilled = value;
    }
  }

  private GameObject _tileOccupied;
  public GameObject TileOccupied {
    get {
      return _tileOccupied;
    }
    set {
      _tileOccupied = value;
    }
  }

  [SerializeField]
  private GameObject _nextLevel;
  public GameObject NextLevel {
    get {
      return _nextLevel;
    }
    set {
      _nextLevel = value;
    }
  }

  [SerializeField]
  private List<Vector3> _path;
  public List<Vector3> Path {
    get {
      return _path;
    }
    set {
      _path = value;
    }
  }

  void Start() {
    CurrentHP = BasicHP * (1 + HPModifier);
    MaxHP = BasicHP * (1 + HPModifier);
    Damage = BasicDamage * (1 + DamageModifier);
    MovingSpeed = BasicMovingSpeed * (1 + MovingSpeedModifier);
  }

}
