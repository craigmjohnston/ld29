using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour {
    public float growingScale;
    public float grownScale;

    public float timeToGrow;

    public bool Grown {
        get { return grown; }
    }

    private float growingTimer;
    private bool grown = false;

    private Material _material;
    private bool growing = false;

    public delegate void TreeEventHandler(Tree sender);
    public event TreeEventHandler ChoppedDown;

    private bool withering = false;
    private float witherTime;
    private float witherTimer = 0;
    private float startScale;

	// Use this for initialization
	void Start () {
	    _material = new Material(renderer.material);
	    renderer.material = _material;
	    growingTimer = timeToGrow;
	    transform.localScale = Vector3.one * growingScale;
	}
	
	// Update is called once per frame
	void Update () {
	    if (withering) {
	        witherTimer += Time.deltaTime;
            if (witherTimer > witherTime) {
                Destroy(gameObject);
                return;
            }
	        transform.localScale = Vector3.one*Mathfx.Hermite(startScale, 0, witherTimer/witherTime);
	    } else {
	        if (!grown && !growing) {
	            growingTimer -= Time.deltaTime;
	            if (growingTimer <= 0) {
	                StartCoroutine(Grow());
	            }
	        }
	    }
	}

    public IEnumerator Grow() {
        growing = true;
        float time = 1f;
        float progress = 0;
        while (transform.localScale.x < grownScale) {
            progress += Time.deltaTime;
            transform.localScale = Vector3.one*Mathfx.Berp(growingScale, grownScale, progress/time);
            yield return new WaitForEndOfFrame();
        }
        grown = true;
    }

    public void ChopDown() {
        if (ChoppedDown != null) ChoppedDown(this);
        Destroy(gameObject);
    }

    public void Wither() {
        withering = true;
        witherTime = Random.Range(0.6f, 2f);
        startScale = transform.localScale.x;
    }
}