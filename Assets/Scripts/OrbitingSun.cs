using UnityEngine;
using System.Collections;

public class OrbitingSun : MonoBehaviour {
    public float dayStart;
    public float dayEnd;
    public float startRotation;
    public float endRotation;

    private Light light;
    private float intensity;

	// Use this for initialization
	void Start () {
	    light = transform.GetComponentInChildren<Light>();
	    intensity = light.intensity;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameValues.CurrentTime < dayEnd && GameValues.CurrentTime > dayStart) {
	        light.intensity = Mathfx.Hermite(intensity, 0, ((GameValues.CurrentTime - dayStart)/(dayEnd - dayStart)));
	    } else if (GameValues.CurrentTime > 0) {
            light.intensity = Mathfx.Hermite(0, intensity, GameValues.CurrentTime / 0.3f);
	    }
	    transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x, 
            transform.rotation.eulerAngles.y,
            startRotation + (endRotation - startRotation) * ((GameValues.CurrentTime - dayStart) / (dayEnd - dayStart)));
	}
}