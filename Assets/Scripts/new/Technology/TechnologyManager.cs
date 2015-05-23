using System;
using System.Collections.Generic;

public class TechnologyManager {

  private List<Technology> technologyList;

  private List<Technology> availableTechnology;
  public List<Technology> AvailableTechnology {
    get {
      return availableTechnology;
    }
  }

  private List<Technology> newTechnology;
  public List<Technology> NewTechnology {
    get {
      return newTechnology;
    }
  }

  private bool[] addedToAvailableTechnology;
  private bool[] researchedTechnology;

  public TechnologyManager() {
    technologyList = new List<Technology>();
    availableTechnology = new List<Technology>();
    newTechnology = new List<Technology>();
    addedToAvailableTechnology = new bool[Enum.GetNames(typeof(GameConstants.TechnologyID)).Length];
    researchedTechnology = new bool[Enum.GetNames(typeof(GameConstants.TechnologyID)).Length];
  }

  public void Initiate() {
    technologyList.Add(new Technology(GameConstants.TechnologyID.Upgrade, "升級技術", 300));
    technologyList.Add(new Technology(GameConstants.TechnologyID.SelfLearning, "自我學習", 500));
    technologyList.Add(new Technology(GameConstants.TechnologyID.Combinate, "組合技術", 2000,
        new GameConstants.TechnologyID[]{GameConstants.TechnologyID.Upgrade
      }
    ));
    technologyList.Add(new Technology(GameConstants.TechnologyID.SelfHealing, "自癒", 1500,
      new GameConstants.TechnologyID[]{
        GameConstants.TechnologyID.SelfLearning
      }
    ));
    technologyList.Add(new Technology(GameConstants.TechnologyID.AdditionalBuildingNumber, "額外機械數量", 5000, true,
      new GameConstants.TechnologyID[]{
        GameConstants.TechnologyID.Combinate,
        GameConstants.TechnologyID.SelfHealing
      }
    ));

    UpdateAvailableTechnologyList();
  }

  public void InitiateWithCustomTechnologyList(Technology[] technologyList) {
  }

  public void ResearchTechnology(int technologyIndex) {
    if (technologyIndex < 0 || technologyIndex >= availableTechnology.Count) {
      return;
    }

    researchedTechnology[(int)availableTechnology[technologyIndex].ID] = true;

    if (!availableTechnology[technologyIndex].Repeatable) {
      availableTechnology.RemoveAt(technologyIndex);
    }

    UpdateAvailableTechnologyList();
  }

  public bool HasTechnology(GameConstants.TechnologyID technologyID) {
    return researchedTechnology[(int)technologyID];
  }

  private void UpdateAvailableTechnologyList() {
    newTechnology.Clear();
    for (int i = 0; i < technologyList.Count; ++i) {
      if (!researchedTechnology[(int)technologyList[i].ID] && !addedToAvailableTechnology[(int)technologyList[i].ID] && !StillRequireOtherTechnology(technologyList[i])) {
        addedToAvailableTechnology[(int)technologyList[i].ID] = true;
        availableTechnology.Add(technologyList[i]);
        newTechnology.Add(technologyList[i]);
      }
    }
  }

  private bool StillRequireOtherTechnology(Technology technology) {
    if (technology.RequiredTechnology == null) {
      return false;
    }
    for (int i = 0; i < technology.RequiredTechnology.Length; ++i) {
      if (!researchedTechnology[(int)technology.RequiredTechnology[i]]) {
        return true;
      }
    }
    return false;
  }
}
