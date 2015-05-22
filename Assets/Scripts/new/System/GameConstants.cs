public static class GameConstants {

  public enum GameMode {
    Story,
    SurvivalNormal,
    SurvivalBoss
  }

  public enum GameStatus {
    Playing,
    Pausing,
    Finished,
    Losed
  }

  public enum PlayerStatus {
    DoingNothing,
    Building,
    Researching,
    Combinating,
    AdjustingVolume,
    Exiting
  }

  public enum TechnologyID {
    Upgrade,
    Combinate,
    SelfLearning,
    SelfHealing,
    AdditionalBuildingNumber
  }

  public static GameMode gameMode;
  public static GameStatus gameStatus;
  public static PlayerStatus playerStatus;

}
