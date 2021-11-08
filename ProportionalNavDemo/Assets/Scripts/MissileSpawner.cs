using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour{
    public GameObject missilePrefab;
    public Transform dummyTransform;
    public Rigidbody dummyRb;

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            var missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            missile.GetComponent<Missile>().SetTarget(dummyTransform, dummyRb);
        }
    }
}