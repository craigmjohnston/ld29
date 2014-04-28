using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SpawnTrees : MonoBehaviour {
    public Tree treePrefab;
    public float minDelay;
    public float maxDelay;
    public float minDistance;
    public float maxDistance;
    public int maxTrees;
    public LayerMask groundMask;

    public List<Tree> trees = new List<Tree>();
    public List<Tree> targeted = new List<Tree>(); 
    private float delayTimer = 0;

    // Use this for initialization
    private void Start() {
        
    }

    // Update is called once per frame
    private void Update() {
        if (trees.Count < maxTrees) {
            delayTimer -= Time.deltaTime;
            if (delayTimer <= 0) {
                Vector3 position = transform.position + Quaternion.Euler(0, Random.Range(0.0f, 3600.0f), 0) * (Random.Range(minDistance, maxDistance) * Vector3.forward);
                RaycastHit hitInfo;
                if (Physics.Raycast(position + Vector3.up*10, Vector3.down, out hitInfo, 15f, groundMask)) {
                    position = new Vector3(position.x, hitInfo.point.y, position.z);
                }
                var tree = (Tree) Instantiate(treePrefab, position, Quaternion.identity);
                trees.Add(tree);
                tree.ChoppedDown += OnTreeChoppedDown;
                delayTimer = Random.Range(minDelay, maxDelay);
            }
        }
    }

    protected void OnTreeChoppedDown(Tree sender) {
        trees.Remove(sender);
        if (targeted.Contains(sender)) {
            targeted.Remove(sender);
        }
    }

    public void OnDestroy() {
        foreach (Tree tree in trees) {
            if (tree != null) {
                tree.Wither();
            }
        }
    }
}