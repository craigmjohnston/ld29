using UnityEngine;
using System.Collections;

public class BuildingChooser : MonoBehaviour {
    public Building housePrefab;
    public Building forestersLodgePrefab;
    public Building guardTowerPrefab;
    public Building villaPrefab;

    public void OnHouseClick(dfControl control, dfMouseEventArgs mouseEvent) {
        GameValues.CurrentBuilding = housePrefab;
        audio.Play();
    }

    public void OnForestersLodgeClick(dfControl control, dfMouseEventArgs mouseEvent) {
        GameValues.CurrentBuilding = forestersLodgePrefab;
        audio.Play();
    }

    public void OnGuardTowerClick(dfControl control, dfMouseEventArgs mouseEvent) {
        GameValues.CurrentBuilding = guardTowerPrefab;
        audio.Play();
    }

    public void OnVillaClick(dfControl control, dfMouseEventArgs mouseEvent) {
        GameValues.CurrentBuilding = villaPrefab;
        audio.Play();
    }
}