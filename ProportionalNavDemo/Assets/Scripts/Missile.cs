using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//implement acceleration
public class Missile : MonoBehaviour{

    public GameObject explosionPrefab;
    public float speed, turnRate;

    public Transform targetTransform;
    public Rigidbody targetRb;
    private Rigidbody rb;

    private float lifetime = 0f;
    

    private void Start(){
        rb = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos(){
       
    }

    private void Update(){
        lifetime += Time.deltaTime;
        CheckLock();
        Fly();
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.yellow);
      
    }

    private void CheckLock(){

        //allow the missile to adjust itself to target on spawn
        if(lifetime < 2f || targetTransform == null){
            return;
        }

        //check if the target is insede the view cone (in this case, a 30 degree cone)
        if(Vector3.Dot(transform.forward, (targetTransform.position - transform.position).normalized) >= Mathf.Sin(30f * Mathf.Rad2Deg)){
            return;
        }

        Debug.Log("Missile missed the target!");
        targetTransform = null;
    }

    private void Fly(){

        Vector3 direction;
        Quaternion target_rotation;

        if(targetTransform == null) return;

        if(GetInterceptDirection(transform.position, targetRb.gameObject.transform.position, speed, targetRb.velocity, out direction)){
            target_rotation = Quaternion.LookRotation(direction);
        }else{
            //if the proNav intercept fails, fallback to simple pursuit
            target_rotation = Quaternion.LookRotation(targetRb.gameObject.transform.position - transform.position);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, target_rotation, turnRate*Time.deltaTime);      
        rb.velocity = transform.forward * speed;
    }

    public static bool GetInterceptDirection(Vector3 origin, Vector3 targetPosition, float bulletSpeed, Vector3 targetVelocity, out Vector3 result){

        var targetingVector = origin - targetPosition;
        var distance = targetingVector.magnitude;
        var alpha = Vector3.Angle(targetingVector, targetVelocity) * Mathf.Deg2Rad;
        var vt = targetVelocity.magnitude;
        var vRatio = vt/bulletSpeed;

        //solve the triangle, using cossine law
        if(SolveQuadratic(1-(vRatio*vRatio), 2*vRatio*distance*Mathf.Cos(alpha), -distance*distance, out var root1, out var root2) == 0){
            //no possible intercept
            result = Vector3.zero;
            return false;
        }

        var expectedPositionSize = Mathf.Max(root1, root2);
        var time = expectedPositionSize/bulletSpeed;
        var estimatedPos = targetPosition + targetVelocity*time;
        result = (estimatedPos - origin).normalized;

        #if UNITY_EDITOR
            Debug.DrawRay(origin, -targetingVector, Color.red);
            Debug.DrawRay(origin, estimatedPos-origin, Color.blue);
            Debug.DrawRay(targetPosition, targetVelocity*time, Color.white);
        #endif

        return true;
    }

    public static int SolveQuadratic(float a, float b, float c, out float root1, out float root2){

        var discriminant = b*b - 4*a*c;

        if(discriminant < 0){
            root1 = Mathf.Infinity;
            root2 = -root1;
            return 0;
        }

        root1 = (-b + Mathf.Sqrt(discriminant))/(2*a);
        root2 = (-b - Mathf.Sqrt(discriminant))/(2*a);
        
        return discriminant > 0 ? 2 : 1;
    }

    public void SetTarget(Transform targetT, Rigidbody targetRb){
        targetTransform = targetT;
        this.targetRb = targetRb;
    }

    private void OnTriggerEnter(Collider c){
        if(c.gameObject.CompareTag("Dummy")){
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

   
}
