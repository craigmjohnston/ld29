using System.Linq;
using UnityEngine;
using System.Collections;

public class HarvestTrees : MonoBehaviour {
    public int woodPerTree;
    public float timeToFellTree;
    public float timeToUnload;

    private int collected = 0;

    private float choppingTimer;

    private Transform house;
    private SpawnTrees treeSpawner;
    private bool droppingOffWood = false;
    private bool choppingDownTree = false;
    private bool inHouse = false;
    private Tree targetTree;

    private const float treeSearchDelay = 1f;
    private float treeSearchTimer;

    private float unloadTimer;
    private Villager villager;

	void Start () {
	    villager = GetComponent<Villager>();
        house = villager.house;
	    treeSpawner = house.GetComponent<SpawnTrees>();
	    treeSearchTimer = treeSearchDelay;
	}
	
	void Update () {
        if (villager.state != VillagerState.Working || villager.dead) return;
        // if not going home, do job
	    if (targetTree == null && !droppingOffWood) {
	        treeSearchTimer -= Time.deltaTime;
	        if (treeSearchTimer <= 0) {
	            Tree potentialTarget = treeSpawner.trees.Where(t => !treeSpawner.targeted.Contains(t)).FirstOrDefault(t => t.Grown);
                if (potentialTarget != null) {
                    targetTree = potentialTarget;
                    treeSpawner.targeted.Add(potentialTarget);
	            }
	            treeSearchTimer = treeSearchDelay;
	        }
	    } else {
	        if (collected == 0) {
	            if (!choppingDownTree) {
	                transform.position +=
	                    (targetTree.collider.ClosestPointOnBounds(transform.position) - transform.position).normalized*
	                    villager.moveSpeed;
                    transform.rotation = Quaternion.LookRotation(targetTree.transform.position - transform.position);
	            }
	            else {
	                choppingTimer -= Time.deltaTime;
	                if (choppingTimer <= 0) {
	                    targetTree.ChopDown();
	                    choppingDownTree = false;
	                    collected += woodPerTree;
	                    droppingOffWood = true;
	                }
	            }
	        } else {
	            if (!inHouse) {
	                transform.position +=
	                    (house.collider.ClosestPointOnBounds(transform.position) - transform.position).normalized*
                        villager.moveSpeed;
                    transform.rotation = Quaternion.LookRotation(house.transform.position - transform.position);
	            } else {
	                unloadTimer -= Time.deltaTime;
                    if (unloadTimer <= 0) {
                        GameValues.Wood += collected;
	                    collected = 0;
                        droppingOffWood = false;
                    }
	            }
	        }
	    }
	}

    public void OnTriggerEnter(Collider other) {
        if (house != null && other.gameObject == house.gameObject) {
            inHouse = true;
            if (collected != 0) {
                unloadTimer = timeToUnload;
            }
        } else if (targetTree != null && other.gameObject == targetTree.gameObject) {
            choppingDownTree = true;
            choppingTimer = timeToFellTree;
        }
    }

    public void OnTriggerExit(Collider other) {
        if (house != null && other.gameObject == house.gameObject) {
            inHouse = false;
        }
    }
}