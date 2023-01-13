using UnityEngine;

public enum Guidance {LineOfSight, Simplified, Quadratic};

public class Missile : MonoBehaviour{

    public GameObject explosionPrefab;
    public Guidance guidanceChoosen;

    public Transform target;
    public float speed;
    public float turnRate;
    public float pValue;

    private Rigidbody rb;
    private Rigidbody targetRb;

    private System.Action GuidanceLogic;

    void Start(){
        rb = GetComponent<Rigidbody>();

        //doing like this to spare the switch call on the fixed update
        switch(guidanceChoosen){
            case Guidance.LineOfSight:
            GuidanceLogic = () => LOSPN();
            break;
            case Guidance.Simplified:
            GuidanceLogic = () => SimplifiedPN();
            break;
            case Guidance.Quadratic:
            GuidanceLogic = () => QuadraticPN();
            break;
        }
    }

    void FixedUpdate(){
       GuidanceLogic();
    }

    void LOSPN(){
        
        Vector3 lineOfSight = (target.position + (targetRb.velocity * Time.fixedDeltaTime)) - transform.position;

        float angle = Vector3.Angle(rb.velocity, lineOfSight);

        Vector3 adjustment = pValue * angle * lineOfSight.normalized;

        rb.velocity = rb.velocity.normalized * speed;
        var target_rotation = Quaternion.LookRotation(adjustment);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target_rotation, turnRate); 

        rb.velocity = transform.forward * speed;
    }

    void SimplifiedPN(){
        Vector3 distanceVector = target.position - transform.position;
        Vector3 targetRelativeVelocity = targetRb.velocity - rb.velocity;

        float navigationTime = distanceVector.magnitude / speed;
        Vector3 targetRelativeInterceptPosition = distanceVector + (targetRelativeVelocity * navigationTime);

        Vector3 desiredHeading = targetRelativeInterceptPosition.normalized;

        targetRelativeInterceptPosition *= pValue;   //multiply the relative intercept pos so the missile will lead a bit more

        var target_rotation = Quaternion.LookRotation((target.position + targetRelativeInterceptPosition) - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target_rotation, turnRate); 

        rb.velocity = transform.forward * speed;
    }

    void QuadraticPN(){
        Vector3 direction;
        Quaternion target_rotation = Quaternion.identity;


        if(GetInterceptDirection(transform.position, targetRb.gameObject.transform.position, speed, targetRb.velocity, out direction)){
            target_rotation = Quaternion.LookRotation(direction);
        }else{
            //well, I guess we cant intercept then
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, target_rotation, turnRate*Time.deltaTime);      
        rb.velocity = transform.forward * speed;
    }

    static bool GetInterceptDirection(Vector3 origin, Vector3 targetPosition, float missileSpeed, Vector3 targetVelocity, out Vector3 result){

        var targetingVector = origin - targetPosition;
        var distance = targetingVector.magnitude;
        var alpha = Vector3.Angle(targetingVector, targetVelocity) * Mathf.Deg2Rad;
        var vt = targetVelocity.magnitude;
        var vRatio = vt/missileSpeed;

        //solve the triangle, using cossine law
        if(SolveQuadratic(1-(vRatio*vRatio), 2*vRatio*distance*Mathf.Cos(alpha), -distance*distance, out var root1, out var root2) == 0){
            result = Vector3.zero;
            return false;   //no intercept solution possible!
        }

        var interceptVectorMagnitude = Mathf.Max(root1, root2);
        var time = interceptVectorMagnitude/missileSpeed;
        var estimatedPos = targetPosition + targetVelocity*time;
        result = (estimatedPos - origin).normalized;

        return true;
    }

    static int SolveQuadratic(float a, float b, float c, out float root1, out float root2){

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
        target = targetT;
        this.targetRb = targetRb;
    }

    void OnTriggerEnter(Collider c){
        if(c.gameObject.CompareTag("Dummy")){
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
