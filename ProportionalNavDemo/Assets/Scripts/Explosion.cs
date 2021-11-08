using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour{
    private Renderer rdr;
    private float a = 1f;

    private void Awake(){
        rdr = GetComponent<Renderer>();
    }

    private void Update(){
        rdr.material.color = SetAlpha(rdr.material.color, a);
        a = a - ((1f/0.2f)*Time.deltaTime);
        if(a <= 0f){
            Destroy(gameObject);
        }
    }

    private Color SetAlpha(Color set, float alpha){
        return new Color(set.r, set.g, set.b, alpha);
    }
}