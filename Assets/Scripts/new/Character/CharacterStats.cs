using UnityEngine;
using System.Collections;

public class CharacterStats : MonoBehaviour {

  [SerializeField]
  private float _currentHP;
  public float CurrentHP {
    get {
      return _currentHP;
    }
    set {
      _currentHP = value;
    }
  }

  [SerializeField]
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
  private float _damage;
  public float Damage {
    get {
      return _damage;
    }
    set {
      _damage = value;
    }
  }

}
