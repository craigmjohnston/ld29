using System;
using UnityEngine;
using System.Collections;

public class BuildingChoiceIndicator : MonoBehaviour {
    public dfButton houseButton;
    public dfButton forestersLodgeButton;
    public dfButton guardTowerButton;
    public dfButton villaButton;

    private dfControl control;

    private BuildingType target;

	// Use this for initialization
	void Start () {
	    control = GetComponent<dfControl>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (target == null || target != GameValues.CurrentBuilding.type) {
	        target = GameValues.CurrentBuilding.type;
	        switch (target) {
	            case BuildingType.House:
                    control.Position = new Vector3(houseButton.Position.x + houseButton.Size.x / 2 - 5, control.Position.y, control.Position.z);
	                break;
	            case BuildingType.ForestersLodge:
                    control.Position = new Vector3(forestersLodgeButton.Position.x + forestersLodgeButton.Size.x / 2 - 5, control.Position.y, control.Position.z);
	                break;
	            case BuildingType.GuardTower:
                    control.Position = new Vector3(guardTowerButton.Position.x + guardTowerButton.Size.x / 2 - 5, control.Position.y, control.Position.z);
	                break;
	            case BuildingType.Villa:
                    control.Position = new Vector3(villaButton.Position.x + villaButton.Size.x / 2 - 5, control.Position.y, control.Position.z);
	                break;
	            default:
	                throw new ArgumentOutOfRangeException();
	        }
	    }
	}
}