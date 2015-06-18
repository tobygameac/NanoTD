using System;

public static class GameConstants {

  public enum GameMode {
    STORY,
    SURVIVAL_NORMAL,
    SURVIVAL_BOSS,
  }

  public enum SystemState {
    PLAYING,
    PAUSE_MENU,
    AUDIO_MENU
  }

  public enum PlayerState {
    IDLE,
    COMBINATING_BUILDINGS,
    VIEWING_BUILDING_LIST,
    VIEWING_TECHNOLOGY_LIST,
    EXITING
  }

  public enum GameState {
    MIDDLE_OF_THE_WAVE,
    WAIT_FOR_THE_NEXT_WAVE,
    FINISHED,
    LOSED
  }

  public enum BuildingID {
    TURRET,
    SLOWING_DEVICE,
    BURNING_DEVICE,
    LASER_DEVICE,
    LASER_CANNON,
    FIRE_CANNON,
    SUPER_BURNING_DEVICE,
    LASER_BURNING_DEVICE,
  }

  private static string[] _nameOfBuildingID;
  public static string[] NameOfBuildingID {
    get {
      if (_nameOfBuildingID == null) {
        _nameOfBuildingID = new string[Enum.GetNames(typeof(GameConstants.BuildingID)).Length];
        _nameOfBuildingID[(int)BuildingID.TURRET] = "基礎砲塔";
        _nameOfBuildingID[(int)BuildingID.SLOWING_DEVICE] = "緩速裝置";
        _nameOfBuildingID[(int)BuildingID.BURNING_DEVICE] = "燃燒裝置";
        _nameOfBuildingID[(int)BuildingID.LASER_DEVICE] = "雷射裝置";
        _nameOfBuildingID[(int)BuildingID.LASER_CANNON] = "雷射加農砲";
        _nameOfBuildingID[(int)BuildingID.FIRE_CANNON] = "火焰加農砲";
        _nameOfBuildingID[(int)BuildingID.SUPER_BURNING_DEVICE] = "超級燃燒塔";
        _nameOfBuildingID[(int)BuildingID.LASER_BURNING_DEVICE] = "雷與火之歌";
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
    ADDITIONAL_BUILDING_NUMBER,
    FREEZING_LEVEL1,
    FREEZING_LEVEL2,
    FREEZING_LEVEL3
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
        _nameOfTechnologyID[(int)TechnologyID.FREEZING_LEVEL1] = "初級冷凍技術";
        _nameOfTechnologyID[(int)TechnologyID.FREEZING_LEVEL2] = "中級冷凍技術";
        _nameOfTechnologyID[(int)TechnologyID.FREEZING_LEVEL3] = "高級冷凍技術";
      }
      return _nameOfTechnologyID;
    }
  }


  private static string[] _detailOfTechnologyID;
  public static string[] DetailOfTechnologyID {
    get {
      if (_detailOfTechnologyID == null) {
        _detailOfTechnologyID = new string[Enum.GetNames(typeof(GameConstants.TechnologyID)).Length];
        _detailOfTechnologyID[(int)TechnologyID.UPGRADE] = "裝置將可以進行升級";
        _detailOfTechnologyID[(int)TechnologyID.COMBINATE] = "可以將兩個裝置進行組合";
        
        _detailOfTechnologyID[(int)TechnologyID.SELF_LEARNING] = "裝置將根據擊殺數來增強攻擊力，每個擊殺數增加 ";
        _detailOfTechnologyID[(int)TechnologyID.SELF_LEARNING] += (SELF_LEARNING_IMPROVEMENT_PERCENT_PER_KILL * 100).ToString("0.00") + "% 傷害";

        _detailOfTechnologyID[(int)TechnologyID.SELF_HEALING] = "核心將隨時間恢復生命值";
        _detailOfTechnologyID[(int)TechnologyID.ADDITIONAL_BUILDING_NUMBER] = "增加 " + ADDITIONAL_BUILDING_NUMBER_PER_RESEARCH + " 個最大可建裝置數量";

        _detailOfTechnologyID[(int)TechnologyID.FREEZING_LEVEL1] = "場上病菌減慢 ";
        _detailOfTechnologyID[(int)TechnologyID.FREEZING_LEVEL1] += (FREEZING_LEVEL1_MOVING_SPEED_MODIFIER * -100).ToString("0.00") + "% 移動速度";

        _detailOfTechnologyID[(int)TechnologyID.FREEZING_LEVEL2] = "場上病菌減慢 ";
        _detailOfTechnologyID[(int)TechnologyID.FREEZING_LEVEL2] += (FREEZING_LEVEL2_MOVING_SPEED_MODIFIER * -100).ToString("0.00") + "% 移動速度";

        _detailOfTechnologyID[(int)TechnologyID.FREEZING_LEVEL3] = "場上病菌減慢 ";
        _detailOfTechnologyID[(int)TechnologyID.FREEZING_LEVEL3] += (FREEZING_LEVEL3_MOVING_SPEED_MODIFIER * -100).ToString("0.00") + "% 移動速度";

      }
      return _detailOfTechnologyID;
    }
  }

  public static float GLOBAL_ENEMY_SPEED_MODIFIER = 0.0f;

  public static readonly int ADDITIONAL_BUILDING_NUMBER_PER_RESEARCH = 1;

  public static readonly float ONE_SHOT_KILL_BONUS_MODIFIER = 0.25f;
  public static readonly float SELF_LEARNING_IMPROVEMENT_PERCENT_PER_KILL = 0.001f;

  public static readonly float FREEZING_LEVEL1_MOVING_SPEED_MODIFIER = -0.15f;
  public static readonly float FREEZING_LEVEL2_MOVING_SPEED_MODIFIER = -0.25f;
  public static readonly float FREEZING_LEVEL3_MOVING_SPEED_MODIFIER = -0.4f;

  public static void ResetModifier() {
    GLOBAL_ENEMY_SPEED_MODIFIER = 0.0f;
  }

}
