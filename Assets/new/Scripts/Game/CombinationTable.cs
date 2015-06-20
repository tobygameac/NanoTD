using UnityEngine;
using System;
using System.Collections;

public class CombinationTable : MonoBehaviour {

  public GameObject fireTurret;
  public GameObject superFireTurret;
  public GameObject laserCannon;
  public GameObject laserBurningDevice;
  public GameObject superBurningDevice;
  public GameObject fireStormDevice;

  private static GameObject[][] combinationTable;

  void Start() {
    int buildingCount = Enum.GetNames(typeof(GameConstants.BuildingID)).Length;
    combinationTable = new GameObject[buildingCount][];

    for (int i = 0; i < buildingCount; ++i) {
      combinationTable[i] = new GameObject[buildingCount];
      for (int j = 0; j < buildingCount; ++j) {
        combinationTable[i][j] = null;
      }
    }

    combinationTable[(int)GameConstants.BuildingID.TURRET][(int)GameConstants.BuildingID.BURNING_DEVICE] = fireTurret;
    combinationTable[(int)GameConstants.BuildingID.FIRE_TURRET][(int)GameConstants.BuildingID.FIRE_TURRET] = superFireTurret;
    combinationTable[(int)GameConstants.BuildingID.TURRET][(int)GameConstants.BuildingID.LASER_DEVICE] = laserCannon;
    combinationTable[(int)GameConstants.BuildingID.BURNING_DEVICE][(int)GameConstants.BuildingID.LASER_DEVICE] = laserBurningDevice;
    combinationTable[(int)GameConstants.BuildingID.BURNING_DEVICE][(int)GameConstants.BuildingID.BURNING_DEVICE] = superBurningDevice;
    combinationTable[(int)GameConstants.BuildingID.BURNING_DEVICE][(int)GameConstants.BuildingID.SLOWING_DEVICE] = fireStormDevice;

    for (int i = 0; i < buildingCount; ++i) {
      for (int j = 0; j < buildingCount; ++j) {
        if (combinationTable[j][i] != null) {
          combinationTable[i][j] = combinationTable[j][i];
        }
      }
    }
  }

  public static GameObject GetCombinationObject(GameConstants.BuildingID buildingID1, GameConstants.BuildingID buildingID2) {
    return combinationTable[(int)buildingID1][(int)buildingID2];
  }
}
