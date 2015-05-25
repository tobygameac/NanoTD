using System;

public static class GameConstants {

  public enum GameMode {
    STORY,
    SURVIVAL_NORMAL,
    SURVIVAL_BOSS,
  }

  public enum GameState {
    PLAYING,
    PAUSE_MENU,
    AUDIO_MENU,
    FINISHED,
    LOSED
  }

  public enum PlayerState {
    IDLE,
    COMBINATING_BUILDINGS,
    VIEWING_BUILDING_LIST,
    VIEWING_TECHNOLOGY_LIST,
    EXITING
  }

  public enum BuildingID {
    TURRET_CANNON,
    SLOW_AURA,
    BURNING_DEVICE,
    LASER_DEVICE,
    LASER_CANNON,
    FIRE_CANNON,
    SUPER_BURNING_DEVICE
  }

  private static string[] _nameOfBuildingID;
  public static string[] NameOfBuildingID {
    get {
      if (_nameOfBuildingID == null) {
        _nameOfBuildingID = new string[Enum.GetNames(typeof(GameConstants.BuildingID)).Length];
        _nameOfBuildingID[(int)BuildingID.TURRET_CANNON] = "基礎砲塔";
        _nameOfBuildingID[(int)BuildingID.SLOW_AURA] = "緩速光環";
        _nameOfBuildingID[(int)BuildingID.BURNING_DEVICE] = "燃燒裝置";
        _nameOfBuildingID[(int)BuildingID.LASER_DEVICE] = "雷射裝置";
        _nameOfBuildingID[(int)BuildingID.LASER_CANNON] = "雷射加農砲";
        _nameOfBuildingID[(int)BuildingID.FIRE_CANNON] = "火焰加農砲";
        _nameOfBuildingID[(int)BuildingID.SUPER_BURNING_DEVICE] = "超級燃燒塔";
      }
      return _nameOfBuildingID;
    }
  }

  public enum EnemyID {
    ENEMY1,
    ENEMY2,
  }

  public enum TechnologyID {
    UPGRADE,
    COMBINATE,
    SELF_LEARNING,
    SELF_HEALING,
    ADDITIONAL_BUILDING_NUMBER
  }

  private static string[] _nameOfTechnologyID;
  public static string[] NameOfTechnologyID {
    get {
      if (_nameOfTechnologyID == null) {
        _nameOfTechnologyID = new string[Enum.GetNames(typeof(GameConstants.TechnologyID)).Length];
        _nameOfTechnologyID[(int)TechnologyID.UPGRADE] = "升級技術";
        _nameOfTechnologyID[(int)TechnologyID.COMBINATE] = "組合技術";
        _nameOfTechnologyID[(int)TechnologyID.SELF_LEARNING] = "自我學習";
        _nameOfTechnologyID[(int)TechnologyID.SELF_HEALING] = "自癒";
        _nameOfTechnologyID[(int)TechnologyID.ADDITIONAL_BUILDING_NUMBER] = "額外機械數量";
      }
      return _nameOfTechnologyID;
    }
  }

}
