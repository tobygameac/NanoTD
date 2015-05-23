using UnityEngine;
using System.Collections;

public partial class Game : MonoBehaviour {

  // Audio
  public AudioClip buttonSound;
  public AudioClip buildSound;
  public AudioClip errorSound;
  public AudioClip deniedSound;
  public AudioClip sellSound;
  public AudioClip researchSound;

  // Tile
  public Material tileOriginalMaterial;
  public Material tileOnHoverMaterial;
  public Transform placementTilesRoot;
  public LayerMask placementLayerMask;
  private GameObject lastHoverTile;

  // Building
  public GameObject coreGameObject;
  public GameObject[] buildingList;
  private int buildingIndex;
  private int nowBuildingNumber;
  [SerializeField]
  private int maxBuildingNumber;
  public LayerMask buildingLayerMask;
  private GameObject lastHoverBuilding;
  private GameObject selectedBuilding;

  // Combination Building
  public GameObject fireCannon;
  public GameObject laserCannon;
  public GameObject superBurningTower;

  // Technology
  private static TechnologyManager technologyManager;
  private int technologyIndex;

  // Game Stats
  [SerializeField]
  private int basicMoney;
  private static int money;

  void Start() {
    Time.timeScale = 1;

    AudioManager.Volume = 0.5f;

    UpdateTilesMesh();
    
    buildingIndex = technologyIndex = -1;
    
    technologyManager = new TechnologyManager();
    technologyManager.Initiate();

    money += basicMoney;
  }

  void Update() {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit raycastHit;

    // Hover
    if (GameConstants.playerStatus == GameConstants.PlayerStatus.Building) {
      lastHoverBuilding = null;
      if (Physics.Raycast(ray, out raycastHit, 1000, placementLayerMask)) {
        if (lastHoverTile) {
          lastHoverTile.GetComponent<Renderer>().material = tileOriginalMaterial;
        }
        lastHoverTile = raycastHit.collider.gameObject;
        tileOriginalMaterial = lastHoverTile.GetComponent<Renderer>().material;
        lastHoverTile.GetComponent<Renderer>().material = tileOnHoverMaterial;
      } else {
        if (lastHoverTile) {
          lastHoverTile.GetComponent<Renderer>().material = tileOriginalMaterial;
          lastHoverTile = null;
        }
      }
    } else {
      if (Physics.Raycast(ray, out raycastHit, 1000, buildingLayerMask)) {
        if (raycastHit.collider.gameObject.transform.parent != null) {
          // Find a real building object
          do {
            lastHoverBuilding = raycastHit.collider.transform.parent.gameObject;
          } while (lastHoverBuilding.transform.parent != null);
        } else {
          lastHoverBuilding = raycastHit.collider.gameObject;
        }
      } else {
        lastHoverBuilding = null;
      }
    }

    GameObject newBuilding;

    // Left click
    if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0) {
      if (lastHoverBuilding != null) {
        if (GameConstants.playerStatus == GameConstants.PlayerStatus.Combinating) {
        }
        if (GameConstants.playerStatus == GameConstants.PlayerStatus.DoingNothing) {
          if (selectedBuilding) {
            selectedBuilding.GetComponent<RangeDisplayer>().enabled = false;
          }
          selectedBuilding = lastHoverBuilding;
          selectedBuilding.GetComponent<RangeDisplayer>().enabled = true;
        }
      }
      if (lastHoverBuilding == null) {
        if (selectedBuilding) {
          selectedBuilding.GetComponent<RangeDisplayer>().enabled = false;
        }
        lastHoverBuilding = selectedBuilding = null;
        if (GameConstants.playerStatus == GameConstants.PlayerStatus.Combinating) {
          AudioManager.PlayAudioClip(errorSound);
          MessageManager.AddMessage("請選擇正確的目標");
          GameConstants.playerStatus = GameConstants.PlayerStatus.DoingNothing;
        }
      }
      if (lastHoverTile != null) {
        if (lastHoverTile.tag == "PlacementTileAvailable" && buildingIndex >= 0) {
          CharacterStats toBuildBuildingStat = buildingList[buildingIndex].GetComponent<CharacterStats>();
          if (money >= toBuildBuildingStat.Cost && nowBuildingNumber < maxBuildingNumber) {
            newBuilding = Instantiate(buildingList[buildingIndex], lastHoverTile.transform.position + new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
            
            AudioManager.PlayAudioClip(buildSound);

            newBuilding.GetComponent<CharacterStats>().TileOccupied = lastHoverTile;
            lastHoverTile.tag = "PlacementTileOccupied";

            money -= toBuildBuildingStat.Cost;
            nowBuildingNumber++;

            lastHoverTile.GetComponent<Renderer>().enabled = false;
            MessageManager.AddMessage("建造完成 : " + newBuilding.tag);
          } else {
            if (money < toBuildBuildingStat.Cost) {
              AudioManager.PlayAudioClip(errorSound);
              MessageManager.AddMessage("需要更多金錢");
            }
            if (nowBuildingNumber >= maxBuildingNumber) {
              AudioManager.PlayAudioClip(errorSound);
              MessageManager.AddMessage("機械數量超過上限");
            }
          }
        }
      }
      if (lastHoverTile == null) {
      }
    }

    // Right click
    if (Input.GetMouseButtonDown(1) && GUIUtility.hotControl == 0) {
    }

    // Esc
    if (Input.GetKeyDown(KeyCode.Escape)) {
      if (GameConstants.playerStatus == GameConstants.PlayerStatus.Building) {
        GameConstants.playerStatus = GameConstants.PlayerStatus.DoingNothing;
        buildingIndex = -1;
        UpdateTilesMesh();
      } else if (selectedBuilding != null) {
        if (GameConstants.playerStatus == GameConstants.PlayerStatus.Combinating) {
          GameConstants.playerStatus = GameConstants.PlayerStatus.DoingNothing;
        }
        selectedBuilding.GetComponent<RangeDisplayer>().enabled = false;
        lastHoverBuilding = selectedBuilding = null;
      } else {
        GameConstants.gameStatus = GameConstants.GameStatus.Pausing;
      }
    }

    // Upgrade
    if (Input.GetKeyDown(KeyCode.U)) {
      if (selectedBuilding != null && HasTechnology(GameConstants.TechnologyID.Upgrade)) {
        if (selectedBuilding.GetComponent<CharacterStats>().NextLevel != null) {
          Upgrade();
        }
      }
    }
    
    if (Input.GetKeyDown(KeyCode.B)) {
      Build();
    }

    if (Input.GetKeyDown(KeyCode.C)) {
      // if (combinatable && selectingBuilding.GetComponent(Stats).combinatable*/) { // Player have to research & Building has combination
        Combinate();
    }

    if (Input.GetKeyDown(KeyCode.R)) {
      Research();
    }

    if (GameConstants.playerStatus == GameConstants.PlayerStatus.Building) {
      for (int i = 0; i < buildingList.Length; ++i) {
        if (Input.GetKeyDown(KeyCode.Keypad1 + i) || Input.GetKeyUp(KeyCode.Alpha1 + i)) {
          AudioManager.PlayAudioClip(buttonSound);
          if (buildingIndex != i) { // Prevent multiple click
            MessageManager.AddMessage("請選擇放置區域");
            buildingIndex = i;
          }
        }
      }
    } else if (GameConstants.playerStatus == GameConstants.PlayerStatus.Researching) {
      for (int i = 0; i < technologyManager.AvailableTechnology.Count; ++i) {
        if (Input.GetKeyDown(KeyCode.Keypad1 + i) || Input.GetKeyUp(KeyCode.Alpha1 + i)) {
          AudioManager.PlayAudioClip(buttonSound);
          technologyIndex = i;
        }
      }
    }

  }

  void Build() {
    if (selectedBuilding != null) {
      selectedBuilding.GetComponent<RangeDisplayer>().enabled = false;
    }
    lastHoverBuilding = selectedBuilding = null;
    AudioManager.PlayAudioClip(buttonSound);
    buildingIndex = -1;
    if (GameConstants.playerStatus == GameConstants.PlayerStatus.Building) {
      GameConstants.playerStatus = GameConstants.PlayerStatus.DoingNothing;
    } else {
      GameConstants.playerStatus = GameConstants.PlayerStatus.Building;
    }

    UpdateTilesMesh();
  }

  void Combinate() {
    GameConstants.playerStatus = GameConstants.PlayerStatus.Combinating;
    AudioManager.PlayAudioClip(buttonSound);

    UpdateTilesMesh();
  }

  void Upgrade() {
    GameObject newBuilding = selectedBuilding.GetComponent<CharacterStats>().NextLevel;
    int upgradeCost = newBuilding.GetComponent<CharacterStats>().Cost - selectedBuilding.GetComponent<CharacterStats>().Cost;
    if (money >= upgradeCost) {
      MessageManager.AddMessage("升級完成 : " + selectedBuilding.tag);
      AudioManager.PlayAudioClip(researchSound);

      money -= upgradeCost;

      newBuilding.GetComponent<CharacterStats>().TileOccupied = selectedBuilding.GetComponent<CharacterStats>().TileOccupied;
      newBuilding.GetComponent<CharacterStats>().UnitKilled = selectedBuilding.GetComponent<CharacterStats>().UnitKilled;
      newBuilding = Instantiate(newBuilding, selectedBuilding.transform.position, selectedBuilding.transform.rotation) as GameObject;

      Destroy(selectedBuilding.gameObject);

      selectedBuilding = newBuilding;
      selectedBuilding.GetComponent<RangeDisplayer>().enabled = true;
    } else {
      AudioManager.PlayAudioClip(errorSound);
      MessageManager.AddMessage("需要更多金錢");
    }
  }

  void Research() {
    if (selectedBuilding != null) {
      selectedBuilding.GetComponent<RangeDisplayer>().enabled = false;
    }
    lastHoverBuilding = selectedBuilding = null;
    AudioManager.PlayAudioClip(buttonSound);

    technologyIndex = -1;

    if (GameConstants.playerStatus == GameConstants.PlayerStatus.Researching) {
      GameConstants.playerStatus = GameConstants.PlayerStatus.DoingNothing;
    } else {
      GameConstants.playerStatus = GameConstants.PlayerStatus.Researching;
    }

    UpdateTilesMesh();
  }

  public bool HasTechnology(GameConstants.TechnologyID technologyID) {
    return technologyManager.HasTechnology(technologyID);
  }

  void UpdateTilesMesh() {
    for (int i = 0; i < placementTilesRoot.childCount; ++i) {
      Transform tile = placementTilesRoot.GetChild(i);
      if (GameConstants.playerStatus == GameConstants.PlayerStatus.Building) {
        if (tile.tag == "PlacementTileAvailable") { // Only show the empty plane
          tile.gameObject.GetComponent<Renderer>().enabled = true;
        }
      } else {
        tile.gameObject.GetComponent<Renderer>().enabled = false;
      }
    }
  }

}
