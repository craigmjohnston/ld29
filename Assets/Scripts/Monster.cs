using System.Linq;
using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {
    public SpawnMonsters spawner;
    public float moveSpeed;
    public float timeToCapture;
    public int shotsToKill;
    public LayerMask groundMask;

    private float captureTimer;
    private SpawnVillager targetBuilding;
    private const float searchDelay = 2f;
    private float searchDelayTimer;
    private Villager captive;
    private bool inBuilding = false;
    private int timesShot = 0;

    public delegate void MonsterEventHandler(Monster sender);
    public event MonsterEventHandler Died;

	void Start () {

	}
	
	void Update () {
	    if (spawner == null) return;
	    if (targetBuilding == null && captive == null) {
	        if (searchDelayTimer <= 0) {
	            searchDelayTimer += searchDelay;
	            SpawnVillager potentialTarget = spawner.buildingsByDistance
                    .Where(b => spawner.targeted.Count(e => e == b) < b.villagers.Count)
                    .FirstOrDefault(b => b.villagers.Count > 0);
	            if (potentialTarget != null) {
	                targetBuilding = potentialTarget;
                    spawner.targeted.Add(potentialTarget);
	            }
	        }
	    } else if (captive == null) {
	        if (!inBuilding) {
	            // move to target
	            transform.position += (targetBuilding.transform.position - transform.position).normalized*moveSpeed;
                transform.rotation = Quaternion.LookRotation(transform.position - targetBuilding.transform.position);
	        } else {
	            captureTimer -= Time.deltaTime;
	            if (captureTimer <= 0) {
	                captive = targetBuilding.villagers.First();
	                captive.Kill();
	                targetBuilding = null;
	            }
	        }
	    } else {
            transform.position += (spawner.transform.position - transform.position).normalized * moveSpeed;
	        if (transform.position != spawner.transform.position) {
	            transform.rotation = Quaternion.LookRotation(transform.position - spawner.transform.position);
	        }
	        captive.transform.position = transform.position;
	    }
	}

    public void LateUpdate() {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + Vector3.up * 10f, Vector3.down, out hitInfo, 15f, groundMask)) {
            transform.position = new Vector3(transform.position.x, hitInfo.point.y + 0.025f, transform.position.z);
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (targetBuilding != null && other.gameObject == targetBuilding.gameObject) {
            inBuilding = true;
            captureTimer = timeToCapture;
        }
    }

    public void OnTriggerExit(Collider other) {
        if (targetBuilding != null && other.gameObject == targetBuilding.gameObject) {
            inBuilding = false;
            if (captive != null) {
                spawner.targeted.Remove(targetBuilding);
                targetBuilding = null;
            }
        }
    }

    public void Shoot() {
        timesShot += 1;
        if (timesShot >= shotsToKill) {
            Die();
        }
    }

    protected void Die() {
        if (Died != null) Died(this);
        Destroy(gameObject);
    }

    void OnDestroy() {
        if (captive != null) {
            Destroy(captive.gameObject);
        }
    }
}