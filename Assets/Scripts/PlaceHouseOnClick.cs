using System.Linq;
using UnityEngine;
using System.Collections;

public class PlaceHouseOnClick : MonoBehaviour {
    public LayerMask layerMask;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnMouseDown() {
        if (dfInputManager.ControlUnderMouse) return;
        if (!HaveResourcesForBuilding()) return;
        if (GameValues.CurrentTimeOfDay == TimeOfDay.Night) return;
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, 100f, layerMask)) {
            Instantiate(GameValues.CurrentBuilding, hitInfo.point, Quaternion.identity);
            SpendResourcesForBuilding();
            audio.Play();
        }
    }

    protected bool HaveResourcesForBuilding() {
        BuildingCost cost = GameValues.BuildingCosts.First(c => c.buildingType == GameValues.CurrentBuilding.type);
        if (cost.food > GameValues.Food) return false;
        if (cost.wood > GameValues.Wood) return false;
        if (cost.gold > GameValues.Gold) return false;
        return true;
    }

    protected void SpendResourcesForBuilding() {
        BuildingCost cost = GameValues.BuildingCosts.First(c => c.buildingType == GameValues.CurrentBuilding.type);
        GameValues.Food -= cost.food;
        GameValues.Wood -= cost.wood;
        GameValues.Gold -= cost.gold;
    }
}