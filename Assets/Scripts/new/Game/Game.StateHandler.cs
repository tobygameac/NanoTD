using UnityEngine;
using System.Collections;

public partial class Game : MonoBehaviour {

  private GameConstants.GameState _gameState;
  private GameConstants.GameState GameState {
    get {
      return _gameState;
    }
    set {
      _gameState = value;

      UpdateCanvas();
    }
  }

  private GameConstants.PlayerState _playerState;
  private GameConstants.PlayerState PlayerState {
    get {
      return _playerState;
    }
    set {
      _playerState = value;

      UpdateCanvas();
    }
  }
}
