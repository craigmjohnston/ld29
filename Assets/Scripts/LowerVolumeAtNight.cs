using UnityEngine;
using System.Collections;

public class LowerVolumeAtNight : MonoBehaviour {
    private bool changingForNight = false;
    private bool changingForDay = false;

    private float defaultVolume;

	void Start () {
	    defaultVolume = audio.volume;
	}

	void Update () {
	    if (GameValues.CurrentTimeOfDay == TimeOfDay.Night && !changingForNight) {
	        changingForDay = false;
	        changingForNight = true;
	        StartCoroutine(ChangeVolume(4, 0));
        } else if (GameValues.CurrentTimeOfDay == TimeOfDay.Dawn && !changingForDay) {
            changingForNight = false;
            changingForDay = true;
            StartCoroutine(ChangeVolume(4, defaultVolume));
	    }
	}

    public IEnumerator ChangeVolume(float time, float target) {
        float start = audio.volume;
        float progress = 0;
        while (progress < time) {
            progress += Time.deltaTime;
            audio.volume = Mathfx.Hermite(start, target, progress/time);
            yield return new WaitForEndOfFrame();
        }
    }
}
