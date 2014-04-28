using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class SpawnVillager : MonoBehaviour {
    public Transform villagerPrefab;
    public float minDelay;
    public float maxDelay;
    public Vector3 spawnOffset;
    public int maxVillagers;

    public List<Villager> villagers = new List<Villager>(); 
    private float spawnTimer;

	void Start () {
	    spawnTimer = Random.Range(minDelay, maxDelay);
	}
	
	void Update () {
	    if (villagers.Count < maxVillagers && GameValues.CurrentTimeOfDay != TimeOfDay.Night) {
	        spawnTimer -= Time.deltaTime;
	        if (spawnTimer <= 0) {
	            spawnTimer += Random.Range(minDelay, maxDelay);
	            // spawn a new villager
	            var villager =
	                ((Transform) Instantiate(villagerPrefab, transform.position + spawnOffset, Quaternion.identity))
	                    .GetComponent<Villager>();
	            villager.house = transform;
	            villager.Died += OnVillagerDied;
	            villagers.Add(villager);
                // TODO: this is lazy
                if (villagerPrefab.name == "Villager" || villagerPrefab.name == "Forester") {
	                GameValues.NumberOfVillagers += 1;
	            } else if (villagerPrefab.name == "Noble") {
                    GameValues.NumberOfNobles += 1;
	            }
	        }
	    }
        // dying from starvation
        if (GameValues.VillagersToDie > 0 && (villagerPrefab.name == "Villager" || villagerPrefab.name == "Forester")) {
	        if (villagers.Count > 0) {
	            Villager villager = villagers.First();
                villager.Kill();
                Destroy(villager.gameObject);
	            GameValues.VillagersToDie -= 1;
	        }
	    }
        if (GameValues.NoblesToDie > 0 && villagerPrefab.name == "Noble") {
            if (villagers.Count > 0) {
                Villager villager = villagers.First();
                villager.Kill();
                Destroy(villager.gameObject);
                GameValues.NoblesToDie -= 1;
            }
        }
	}

    protected void OnVillagerDied(Villager sender) {
        villagers.Remove(sender);
        if (villagerPrefab.name == "Villager" || villagerPrefab.name == "Forester") {
            GameValues.NumberOfVillagers -= 1;
        } else if (villagerPrefab.name == "Noble") {
            GameValues.NumberOfNobles -= 1;
        }
        if (villagers.Count == 0) {
            StartCoroutine(Sink());
        }
    }

    public void OnMouseEnter() {
        GameValues.InfoLabel.Text = "This building has " + (villagers.Count == 0 ? "no" : villagers.Count.ToString()) + " occupant" + (villagers.Count != 1 ? "s" : "") + ".";
    }

    public void OnMouseExit() {
        GameValues.InfoLabel.Text = "";
    }

    public IEnumerator Sink() {
        while (transform.position.y > -5) {
            transform.position += Vector3.down*0.5f*Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}