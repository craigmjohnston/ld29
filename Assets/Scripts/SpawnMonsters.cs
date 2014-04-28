using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class SpawnMonsters : MonoBehaviour {
    public TimeOfDay SpawnTime;
    public float monsterIncrease;
    public int maxMonsters;
    public float minDelay;
    public float maxDelay;
    public Monster monsterPrefab;

    private float delayTimer;
    private List<Monster> monsters = new List<Monster>();

    public List<SpawnVillager> buildingsByDistance = new List<SpawnVillager>();
    public List<SpawnVillager> targeted = new List<SpawnVillager>(); 
    public bool spawning = false;

	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {
	    if (GameValues.CurrentTimeOfDay == SpawnTime && monsters.Count < maxMonsters) {
	        if (!spawning) {
                buildingsByDistance = FindObjectsOfType<SpawnVillager>()
	                .OrderBy(b => (b.transform.position - transform.position).magnitude)
	                .ToList();
                spawning = true;
	        }
	        delayTimer -= Time.deltaTime;
	        if (delayTimer <= 0) {
	            delayTimer += Random.Range(minDelay, maxDelay);
	            var monster = (Monster) Instantiate(monsterPrefab, transform.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f)), Quaternion.identity);
	            monsters.Add(monster);
	            monster.spawner = this;
	        }
	    } else if (GameValues.CurrentTimeOfDay != SpawnTime) {
	        if (monsters.Count > 0) {
	            foreach (Monster monster in monsters) {
	                if (monster != null) {
	                    Destroy(monster.gameObject);
	                }
	            }
                monsters.Clear();
	            maxMonsters = Mathf.Clamp((int)Mathf.Floor(maxMonsters * monsterIncrease), 1, 32);
	        }
	        spawning = false;
	    }
	}
}