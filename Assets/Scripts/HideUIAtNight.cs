using UnityEngine;
using System.Collections;

public class HideUIAtNight : MonoBehaviour {
    public dfControl[] toHide;
    private bool hidden = false;
	// Update is called once per frame
	void Update () {
	    if (GameValues.CurrentTimeOfDay == TimeOfDay.Night && !hidden) {
	        hidden = true;
	        foreach (dfControl control in toHide) {
	            control.Hide();
	        }
        } else if (GameValues.CurrentTimeOfDay != TimeOfDay.Night && hidden) {
            hidden = false;
            foreach (dfControl control in toHide) {
                control.Show();
            }
        }
	}
}