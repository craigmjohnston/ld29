using UnityEngine;
using System.Collections;

public class FlyAtTarget : MonoBehaviour {
    public Monster target;
    public float moveSpeed;

    private Vector3 targetLastPosition;
	
	void Update () {
	    if (target != null) {
            targetLastPosition = target.transform.position;
	    }
        transform.position += (targetLastPosition - transform.position).normalized * moveSpeed * Time.deltaTime;
	    transform.rotation = Quaternion.LookRotation(targetLastPosition - transform.position);
	}

    public void OnTriggerEnter(Collider other) {
        if (target != null && other.gameObject == target.gameObject) {
            target.Shoot();
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision) {
        if (target == null) {
            if (collision.gameObject.name == "Ground") {
                Destroy(gameObject);
            }
        }
    }
}
