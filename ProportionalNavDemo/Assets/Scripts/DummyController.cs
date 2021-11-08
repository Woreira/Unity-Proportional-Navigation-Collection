using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the dummy along a list of waypoints
public class DummyController : MonoBehaviour{
    
    public float turnRate, speed, minWaypointDist;
    public List<Transform> waypoints = new List<Transform>();
    
    private int currentWaypointIndex;
    private Rigidbody rb;

    private void OnDrawGizmos(){
        if(waypoints.Count == 0) return;
        for(int i = 0; i < waypoints.Count; i++){
            Gizmos.DrawWireSphere(waypoints[i].position, 1f);
            if(i != 0){
                Gizmos.DrawLine(waypoints[i].position, waypoints[i-1].position);
            }else{
                Gizmos.DrawLine(transform.position, waypoints[i].position);
            }
        }
    }

    private void Awake(){
        rb = GetComponent<Rigidbody>();
    }

    private void Start(){
        currentWaypointIndex = 0;
    }

    private void Update(){
        Steer();
        CheckIfCloseToWaypoint();
    }

    private void Steer(){
        var target_rotation = Quaternion.LookRotation(waypoints[currentWaypointIndex].position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target_rotation, turnRate*Time.deltaTime); 
        rb.velocity = transform.forward * speed;
    }

    private void CheckIfCloseToWaypoint(){
        if(Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < minWaypointDist){
            currentWaypointIndex++;
            if(currentWaypointIndex >= waypoints.Count){
                currentWaypointIndex = 0;
            }
        }
    }


}
