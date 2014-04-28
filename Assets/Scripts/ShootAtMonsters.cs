using System.Linq;
using UnityEngine;

public class ShootAtMonsters : MonoBehaviour {
    public LayerMask targetLayerMask;
    public float targetRadius;
    public float shotDelay;
    public FlyAtTarget arrowPrefab;
    public Vector3 firingOffset;

    private const float searchDelay = 1f;
    private float searchDelayTimer;

    private float shotDelayTimer;

    private Monster targetMonster;


	void Start () {
	
	}
	
	void Update () {
	    if (targetMonster == null) {
	        searchDelayTimer -= Time.deltaTime;
	        if (searchDelayTimer <= 0) {
	            searchDelayTimer += searchDelay;
	            Collider[] colliders = Physics.OverlapSphere(transform.position, targetRadius, targetLayerMask);
	            Collider potentialTarget = colliders.OrderBy(c => (c.transform.position - transform.position).magnitude)
	                .FirstOrDefault();
	            if (potentialTarget != null) {
	                targetMonster = potentialTarget.GetComponent<Monster>();
	                targetMonster.Died += OnTargetDied;
	            }
	        }
	    } else {
	        shotDelayTimer -= Time.deltaTime;
	        if (shotDelayTimer <= 0) {
	            shotDelayTimer += shotDelay;
                var arrow = (FlyAtTarget)Instantiate(arrowPrefab, transform.position + firingOffset, Quaternion.identity);
	            arrow.target = targetMonster;
                Debug.Log("shot");
	        }
	    }
	}

    protected void OnTargetDied(Monster sender) {
        targetMonster = null;
    }
}
