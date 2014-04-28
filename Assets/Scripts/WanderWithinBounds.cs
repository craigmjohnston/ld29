using UnityEngine;
using System.Collections;

public class WanderWithinBounds : MonoBehaviour {
    public float minimumDistance;
    public Rect relativeBounds;

    private Rect bounds;
    private Vector3 targetPosition;
    private Villager villager;

	// Use this for initialization
	void Start () {
        villager = GetComponent<Villager>();
        bounds = new Rect(
            relativeBounds.x + transform.position.x, 
            relativeBounds.y + transform.position.z, 
            relativeBounds.width, relativeBounds.height);
        GenerateTarget();
	}
	
	// Update is called once per frame
	void Update () {
	    if (villager.state != VillagerState.Working) return;
	    if (transform.position != targetPosition) {
	        transform.position += (targetPosition - transform.position).normalized*villager.moveSpeed*Time.deltaTime;
	        transform.rotation = Quaternion.LookRotation(targetPosition - transform.position);
	        if ((targetPosition - transform.position).magnitude < minimumDistance) {
	            GenerateTarget();
	        }
	    }
	}

    protected void GenerateTarget() {
        targetPosition = new Vector3(
            Random.Range(bounds.xMin, bounds.xMax), 
            transform.position.y, 
            Random.Range(bounds.yMin, bounds.yMax));
    }
}
