using UnityEngine;
using System.Collections;

public class HarvestFromForest : MonoBehaviour {
    public int amountToCollect;
    public float timePerUnit;
    public Transform house;

    public float timeToUnload;

    private Forest forest;
    private int collected;
    private float unitTimer;
    private bool inForest = false;


    private float unloadTimer;
    private bool inHouse = false;
    private bool unloading = false;
    private Villager villager;

	void Start () {
        villager = GetComponent<Villager>();
        house = villager.house;
	    forest = FindObjectOfType<Forest>();
	}

    void Update() {
        if (villager.state != VillagerState.Working || villager.dead) return;
        // if not going home, do job
	    if (collected < amountToCollect && !unloading) {
	        if (inForest) {
	            // keep collecting from the forest
	            unitTimer -= Time.deltaTime;
	            if (unitTimer <= 0) {
	                unitTimer = timePerUnit;
	                collected += 1;
	            }
	        } else {
	            // move to the forest
	            Vector3 move = (forest.transform.position - transform.position).normalized*villager.moveSpeed;
	            transform.position += new Vector3(move.x, 0, move.z);
                transform.rotation = Quaternion.LookRotation(transform.position - forest.transform.position);
	        }
	    } else {
	        if (!inHouse) {
                transform.position += (house.collider.ClosestPointOnBounds(transform.position) - transform.position).normalized * villager.moveSpeed;
                transform.rotation = Quaternion.LookRotation(transform.position - house.transform.position);
	        } else {
	            unloadTimer -= Time.deltaTime;
	            if (unloadTimer <= 0) {
	                unloading = false;
	            }
	        }
	    }
	}

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject == forest.gameObject) {
            inForest = true;
            unitTimer = timePerUnit;
        } else if (house != null && other.gameObject == house.gameObject) {
            inHouse = true;
            if (collected > 0) {
                GameValues.Food += collected;
                collected = 0;
                unloadTimer = timeToUnload;
                unloading = true;
            }
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.gameObject == forest.gameObject) {
            inForest = false;
        } else if (house != null && other.gameObject == house.gameObject) {
            inHouse = false;
        }
    }
}
