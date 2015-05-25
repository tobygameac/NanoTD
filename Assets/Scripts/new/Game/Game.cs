using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public partial class Game : MonoBehaviour {

  [SerializeField]
  private GameConstants.GameMode gameMode;

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
  public List<GameObject> buildingList;
  private int nowBuildingNumber;
  [SerializeField]
  private int maxBuildingNumber;
  public LayerMask buildingLayerMask;
  private GameObject lastHoverBuilding;
  private GameObject _selectedBuilding;
  private GameObject selectedBuilding {
    get {
      return _selectedBuilding;
    }
    set {
      _selectedBuilding = value;
      buildingStatsCanvas.SetActive(value != null);
    }
  }
  public GameObject SelectedBuilding {
    get {
      return _selectedBuilding;
    }
  }

  private int _viewingBuildingIndex;
  private int viewingBuildingIndex {
    get {
      return _viewingBuildingIndex;
    }
    set {
      _viewingBuildingIndex = value;
      buildingDetailCanvas.SetActive(value >= 0 && value < buildingList.Count);
    }
  }
  public int ViewingBuildingIndex {
    get {
      return _viewingBuildingIndex;
    }
  }

  // Technology
  private static TechnologyManager technologyManager;
  private int _viewingTechnologyIndex;
  private int viewingTechnologyIndex {
    get {
      return _viewingTechnologyIndex;
    }
    set {
      _viewingTechnologyIndex = value;
      technologyDetailCanvas.SetActive(value >= 0 && value < technologyManager.AvailableTechnology.Count);
    }
  }
  public int ViewingTechnologyIndex {
    get {
      return _viewingTechnologyIndex;
    }
  }

  // Game Stats
  [SerializeField]
  private int basicmoney;
  private static int money;
  public int Money {
    get {
      return money;
    }
  }

  void Start() {
    InitializeGame();
  }

  void Update() {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit raycastHit;

    // Hover
    if (PlayerState == GameConstants.PlayerState.VIEWING_BUILDING_LIST) {
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
    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
      if (lastHoverBuilding != null) {
        if (PlayerState == GameConstants.PlayerState.COMBINATING_BUILDINGS) {
        }
        if (PlayerState == GameConstants.PlayerState.IDLE) {
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
        if (PlayerState == GameConstants.PlayerState.COMBINATING_BUILDINGS) {
          AudioManager.PlayAudioClip(errorSound);
          MessageManager.AddMessage("請選擇正確的目標");
          PlayerState = GameConstants.PlayerState.IDLE;
        }
      }
      if (lastHoverTile != null) {
        if (lastHoverTile.tag == "PlacementTileAvailable" && viewingBuildingIndex >= 0) {
          CharacterStats toBuildBuildingStat = buildingList[viewingBuildingIndex].GetComponent<CharacterStats>();
          if (money >= toBuildBuildingStat.Cost && nowBuildingNumber < maxBuildingNumber) {
            newBuilding = Instantiate(buildingList[viewingBuildingIndex], lastHoverTile.transform.position + new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
            
            AudioManager.PlayAudioClip(buildSound);

            newBuilding.GetComponent<CharacterStats>().TileOccupied = lastHoverTile;
            lastHoverTile.tag = "PlacementTileOccupied";

            money -= toBuildBuildingStat.Cost;
            nowBuildingNumber++;

            lastHoverTile.GetComponent<Renderer>().enabled = false;
            GameConstants.BuildingID newBuildingID = newBuilding.GetComponent<CharacterStats>().BuildingID;
            MessageManager.AddMessage("建造完成 : " + GameConstants.NameOfBuildingID[(int)newBuildingID]);
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

    // Esc
    if (Input.GetKeyDown(KeyCode.Escape)) {
      if (PlayerState == GameConstants.PlayerState.VIEWING_BUILDING_LIST) {
        PlayerState = GameConstants.PlayerState.IDLE;
        viewingBuildingIndex = -1;
        UpdateTilesMesh();
      } else if (PlayerState == GameConstants.PlayerState.VIEWING_TECHNOLOGY_LIST) {
        PlayerState = GameConstants.PlayerState.IDLE;
        viewingTechnologyIndex = -1;
      } else if (selectedBuilding != null) {
        if (PlayerState == GameConstants.PlayerState.COMBINATING_BUILDINGS) {
          PlayerState = GameConstants.PlayerState.IDLE;
        }
        selectedBuilding.GetComponent<RangeDisplayer>().enabled = false;
        lastHoverBuilding = selectedBuilding = null;
      } else {
        OnPauseButtonClick();
      }
    }

    // Upgrade
    if (Input.GetKeyDown(KeyCode.U)) {
      if (selectedBuilding != null && HasTechnology(GameConstants.TechnologyID.UPGRADE)) {
        if (selectedBuilding.GetComponent<CharacterStats>().NextLevel != null) {
          OnUpgradeButtonClick();
        }
      }
    }
    
    if (Input.GetKeyDown(KeyCode.B)) {
      if (PlayerState == GameConstants.PlayerState.IDLE || PlayerState == GameConstants.PlayerState.VIEWING_BUILDING_LIST) {
        OnViewBuildingListButtonClick();
      }
    }

    if (Input.GetKeyDown(KeyCode.R)) {
      if (PlayerState == GameConstants.PlayerState.IDLE || PlayerState == GameConstants.PlayerState.VIEWING_TECHNOLOGY_LIST) {
        OnViewTechnologyListButtonClick();
      }
    }

    if (Input.GetKeyDown(KeyCode.C)) {
      // if (combinatable && selectingBuilding.GetComponent(Stats).combinatable*/) { // Player have to research & Building has combination
      Combinate();
    }

    if (PlayerState == GameConstants.PlayerState.VIEWING_BUILDING_LIST) {
      for (int i = 0; i < buildingList.Count; ++i) {
        if (Input.GetKeyDown(KeyCode.Keypad1 + i) || Input.GetKeyUp(KeyCode.Alpha1 + i)) {
          OnBuildingListButtonClick(i);
        }
      }
    } else if (PlayerState == GameConstants.PlayerState.VIEWING_TECHNOLOGY_LIST) {
      for (int i = 0; i < technologyManager.AvailableTechnology.Count; ++i) {
        if (Input.GetKeyDown(KeyCode.Keypad1 + i) || Input.GetKeyUp(KeyCode.Alpha1 + i)) {
          OnTechnologyListButtonClick(i);
        }
      }
    }

  }

  void ViewBuildingList() {
    if (selectedBuilding != null) {
      selectedBuilding.GetComponent<RangeDisplayer>().enabled = false;
    }
    
    lastHoverBuilding = selectedBuilding = null;
    viewingBuildingIndex = viewingTechnologyIndex = -1;
    
    if (PlayerState == GameConstants.PlayerState.VIEWING_BUILDING_LIST) {
      PlayerState = GameConstants.PlayerState.IDLE;
    } else {
      PlayerState = GameConstants.PlayerState.VIEWING_BUILDING_LIST;
    }

    UpdateTilesMesh();
  }

  void ViewTechnologyList() {
    if (selectedBuilding != null) {
      selectedBuilding.GetComponent<RangeDisplayer>().enabled = false;
    }

    lastHoverBuilding = selectedBuilding = null;
    viewingBuildingIndex = viewingTechnologyIndex = -1;

    if (PlayerState == GameConstants.PlayerState.VIEWING_TECHNOLOGY_LIST) {
      PlayerState = GameConstants.PlayerState.IDLE;
    } else {
      PlayerState = GameConstants.PlayerState.VIEWING_TECHNOLOGY_LIST;
    }

    UpdateTilesMesh();
  }

  void Combinate() {
    PlayerState = GameConstants.PlayerState.COMBINATING_BUILDINGS;

    UpdateTilesMesh();
  }

  void Upgrade() {

    if (selectedBuilding.GetComponent<CharacterStats>().NextLevel == null) {
      return;
    }

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

  void Pause() {
    if (GameState == GameConstants.GameState.PAUSE_MENU) {
      GameState = GameConstants.GameState.PLAYING;
      Time.timeScale = 1;
    } else {
      GameState = GameConstants.GameState.PAUSE_MENU;
      Time.timeScale = 0;
    }
  }

  void BackToGame() {
    GameState = GameConstants.GameState.PLAYING;
    Time.timeScale = 1;
  }

  public bool HasTechnology(GameConstants.TechnologyID technologyID) {
    return technologyManager.HasTechnology(technologyID);
  }

  void UpdateTilesMesh() {
    for (int i = 0; i < placementTilesRoot.childCount; ++i) {
      Transform tile = placementTilesRoot.GetChild(i);
      if (PlayerState == GameConstants.PlayerState.VIEWING_BUILDING_LIST) {
        if (tile.tag == "PlacementTileAvailable") { // Only show the empty plane
          tile.gameObject.GetComponent<Renderer>().enabled = true;
        }
      } else {
        tile.gameObject.GetComponent<Renderer>().enabled = false;
      }
    }
  }

  void InitializeGame() {
    Time.timeScale = 0;

    AudioManager.Volume = 0.5f;

    UpdateTilesMesh();
    
    viewingBuildingIndex = viewingTechnologyIndex = -1;
    
    technologyManager = new TechnologyManager();
    technologyManager.Initiate();

    money += basicmoney;

    InitializeUI();

    Time.timeScale = 1;
  }
}
