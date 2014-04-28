using UnityEngine;
using System.Collections;

public class MainMenuOnClick : MonoBehaviour {
    public dfPanel winDialog;
    public dfPanel loseDialog;
    public dfPanel mainMenuDialog;

	void Update () {
        if (!mainMenuDialog.IsVisible && (winDialog.IsVisible || loseDialog.IsVisible) && Input.anyKeyDown) {
	        winDialog.Hide();
            loseDialog.Hide();
            mainMenuDialog.Show();
            Application.LoadLevel(0);
	    }
	}
}