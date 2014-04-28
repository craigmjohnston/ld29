using System.Linq;
using UnityEngine;
using System.Collections;

public enum VillagerState {
    Sleeping, Working, GoingHome, Captured
}

public class Villager : MonoBehaviour {
    public float moveSpeed;
    public Transform house;
    public LayerMask groundMask;
    public Vector3 positionAdjust;
    public bool disableFlooring;

    private static readonly TimeOfDay[] SleepTimes = {TimeOfDay.Dusk, TimeOfDay.Night};

    public delegate void VillagerEventHandler(Villager sender);
    public event VillagerEventHandler Died;

    public VillagerState state = VillagerState.Working;

    public bool dead = false;

    public void Start() {
        
    }

    public void Update() {
        if (dead) return;
        if (state != VillagerState.Sleeping && SleepTimes.Contains(GameValues.CurrentTimeOfDay)) {
            state = VillagerState.GoingHome;
            transform.position += (house.transform.position - transform.position).normalized * moveSpeed;
            if ((house.transform.position - transform.position).magnitude > 0.5f) {
                transform.rotation = Quaternion.LookRotation(house.transform.position - transform.position);
            } else {
                state = VillagerState.Sleeping;
                renderer.enabled = false;
            }
        } else if (state == VillagerState.Sleeping && !SleepTimes.Contains(GameValues.CurrentTimeOfDay)) {
            state = VillagerState.Working;
            renderer.enabled = true;
        }
    }

    public void LateUpdate() {
        if (state != VillagerState.Sleeping && !disableFlooring) {
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position + Vector3.up * 10f, Vector3.down, out hitInfo, 15f, groundMask)) {
                transform.position = new Vector3(transform.position.x, hitInfo.point.y + 0.025f, transform.position.z) + positionAdjust;
            }
        }
    }

    public void Kill() {
        if (Died != null) Died(this);
        dead = true;
    }
}