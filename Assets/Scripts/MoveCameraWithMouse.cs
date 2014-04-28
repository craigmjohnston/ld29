using UnityEngine;
using System.Collections;

public class MoveCameraWithMouse : MonoBehaviour {
    public float moveSpeed;
    public float acceleration;
    public float drag;
    public Vector3 minPosition;
    public Vector3 maxPosition;

    private Vector3 speed = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    Vector3 moveVector = Vector3.zero;
	    if (Input.GetKey(KeyCode.W)) {
	        moveVector += Vector3.forward;
	    }
        if (Input.GetKey(KeyCode.A)) {
            moveVector += Vector3.left;
        }
        if (Input.GetKey(KeyCode.S)) {
            moveVector += Vector3.back;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveVector += Vector3.right;
        }
	    speed += moveVector*acceleration*Time.deltaTime;
	    if (moveVector.x == 0 && speed.x != 0) {
	        bool positive = speed.x > 0;
	        speed -= Mathf.Sign(speed.x) * Vector3.right * drag*Time.deltaTime;
	        if ((positive && speed.x < 0) || (!positive && speed.x > 0)) {
	            speed = new Vector3(0, speed.y, speed.z);
	        }
	    } else if (moveVector.z == 0 && speed.z != 0) {
            bool positive = speed.z > 0;
            speed -= Mathf.Sign(speed.z) * Vector3.forward * drag * Time.deltaTime;
            if ((positive && speed.z < 0) || (!positive && speed.z > 0)) {
                speed = new Vector3(speed.x, speed.y, 0);
            }
	    }
	    Vector3.ClampMagnitude(speed, moveSpeed);
	    float rot = transform.rotation.eulerAngles.x;
	    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        transform.Translate(speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(rot, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
	    LimitPosition();
	}

    protected void LimitPosition() {
        /*transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minPosition.x, maxPosition.x),
            Mathf.Clamp(transform.position.y, minPosition.y, maxPosition.y),
            Mathf.Clamp(transform.position.z, minPosition.z, maxPosition.z)
        );*/
    }
}