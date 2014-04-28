using UnityEngine;
using System.Collections;

public class StartGameFromMainMenu : MonoBehaviour {
    public dfPanel gameUIPanel;
    public dfPanel titleScreenPanel;

	void Update () {
	    if (Input.GetKeyDown(KeyCode.Space) && titleScreenPanel.IsVisible) {
	        StartCoroutine(GameValues.TimeLerp(GameValues.GameStartTime, 3));
            // TODO: show the game ui
            gameUIPanel.Show();
            // hide the menu UI
            titleScreenPanel.Hide();
	    }
	}
}
