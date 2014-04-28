using System.Linq;
using UnityEngine;
using System.Collections;

public class ShowBuildingCosts : MonoBehaviour {
    public void OnForestersLodgeButtonMouseEnter(dfControl control, dfMouseEventArgs mouseEvent) {
        GameValues.InfoLabel.Text = "The Forester's Lodge costs " + CostString(BuildingType.ForestersLodge) + " and generates wood.";
    }

    public void OnHouseButtonMouseEnter(dfControl control, dfMouseEventArgs mouseEvent) {
        GameValues.InfoLabel.Text = "The House costs " + CostString(BuildingType.House) + " and generates food.";
    }

    public void OnGuardTowerButtonMouseEnter(dfControl control, dfMouseEventArgs mouseEvent) {
        GameValues.InfoLabel.Text = "The Guard Tower costs " + CostString(BuildingType.GuardTower) + " and protects against the night.";
    }

    public void OnVillaButtonMouseEnter(dfControl control, dfMouseEventArgs mouseEvent) {
        GameValues.InfoLabel.Text = "The Villa costs " + CostString(BuildingType.Villa) + " and generates lots of gold.";
    }

    public void OnMouseLeave(dfControl control, dfMouseEventArgs mouseEvent) {
        GameValues.InfoLabel.Text = "";
    }

    protected string CostString(BuildingType buildingType) {
        BuildingCost cost = GameValues.BuildingCosts.First(c => c.buildingType == buildingType);
        string output = "";
        int items = 0;
        if (cost.food > 0) {
            output += cost.food + " food";
            items += 1;
        }
        if (cost.wood > 0) {
            output += (cost.food > 0 ? (cost.gold > 0 ? ", " : " and ") : "") + cost.wood + " wood";
            items += 1;
        }
        if (cost.gold > 0) {
            output += (items == 1 ? " and " : (items == 2 ? ", and " : "")) + cost.gold + " gold";
        }
        return output;
    }
}