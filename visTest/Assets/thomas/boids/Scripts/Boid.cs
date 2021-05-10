using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {

    BoidSettings settings;

    // State
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 forward;
    Vector3 velocity;

    // To update:
    Vector3 acceleration;
    [HideInInspector]
    public Vector3 avgFlockHeading;
    [HideInInspector]
    public Vector3 avgAvoidanceHeading;
    [HideInInspector]
    public Vector3 centreOfFlockmates;
    [HideInInspector]
    public int numPerceivedFlockmates;

    // Cached
    Material material;
    Transform cachedTransform;
    public GameObject targetObject;
    public Transform target;
    public Rigidbody rb;

    bool boidDead = false; //wordt true wanneer de Death() function wordt gecalled
    bool boidImmobilized = false;

    void Awake () {
        material = transform.GetComponentInChildren<MeshRenderer> ().material;
        targetObject = GameObject.FindGameObjectWithTag("Target"); //gebruik de Target tag voor de speler, kan ook anders maar dit is huidige oplossing
        target = targetObject.transform;
        cachedTransform = transform;
    }

    public void Initialize (BoidSettings settings, Transform target) {
        //this.target = target; //deze is weggecomment, als deze aanstaat doen de boids hun normale swarm behaviour
        this.settings = settings;

        position = cachedTransform.position;
        forward = cachedTransform.forward;

        float startSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
        velocity = transform.forward * startSpeed;
    }

    public void SetColour (Color col) { //hierin kunnen we spelen met de material, en kunnen we deze bijv veranderen on death etc
        if (material != null) {
            material.color = col;
        }
    }

    public void UpdateBoid () {
        
        if (boidImmobilized == false) //alle movement is nested in deze if, moet eigenlijk beter voor netheid en bruikbaarheid redenen
        {
            Vector3 acceleration = Vector3.zero;

            if (target != null) //in deze if statement sturen de boids naar de target, weight van deze sturing kan veranderd worden in de settings file
            {
                Vector3 offsetToTarget = (target.position - position);
                acceleration = SteerTowards(offsetToTarget) * settings.targetWeight;
            }

            if (numPerceivedFlockmates != 0)
            {
                centreOfFlockmates /= numPerceivedFlockmates;

                Vector3 offsetToFlockmatesCentre = (centreOfFlockmates - position);

                var alignmentForce = SteerTowards(avgFlockHeading) * settings.alignWeight;
                var cohesionForce = SteerTowards(offsetToFlockmatesCentre) * settings.cohesionWeight;
                var seperationForce = SteerTowards(avgAvoidanceHeading) * settings.seperateWeight;

                acceleration += alignmentForce;
                acceleration += cohesionForce;
                acceleration += seperationForce;
            }

            if (IsHeadingForCollision())
            {
                Vector3 collisionAvoidDir = ObstacleRays();
                Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * settings.avoidCollisionWeight;
                acceleration += collisionAvoidForce;
            }

            velocity += acceleration * Time.deltaTime;
            float speed = velocity.magnitude;
            Vector3 dir = velocity / speed;
            speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
            velocity = dir * speed;

            cachedTransform.position += velocity * Time.deltaTime;
            cachedTransform.forward = dir;
            position = cachedTransform.position;
            forward = dir;

            
        }

        if (Input.GetKeyDown(KeyCode.E) && !boidDead) //gewoon debug, klik op E om de boids 'dood' te maken
        {
            Death();
        } else if (Input.GetKeyDown(KeyCode.E) && boidDead)
        {
            Reanimate();
        }
    }

    bool IsHeadingForCollision () {
        RaycastHit hit;
        if (Physics.SphereCast (position, settings.boundsRadius, forward, out hit, settings.collisionAvoidDst, settings.obstacleMask)) {
            return true;
        } else { }
        return false;
    }

    Vector3 ObstacleRays () {
        Vector3[] rayDirections = BoidHelper.directions;

        for (int i = 0; i < rayDirections.Length; i++) {
            Vector3 dir = cachedTransform.TransformDirection (rayDirections[i]);
            Ray ray = new Ray (position, dir);
            if (!Physics.SphereCast (ray, settings.boundsRadius, settings.collisionAvoidDst, settings.obstacleMask)) {
                return dir;
            }
        }

        return forward;
    }

    Vector3 SteerTowards (Vector3 vector) {
        Vector3 v = vector.normalized * settings.maxSpeed - velocity;
        return Vector3.ClampMagnitude (v, settings.maxSteerForce);
    }

    public void Death() //function that turns the boid into an unmoving rigidbody object
    {
        rb.useGravity = true;
        boidImmobilized = true;
        boidDead = true;
        rb.AddForce(transform.forward * 5f, ForceMode.Impulse);
    }

    public void Reanimate()
    {
        rb.useGravity = false;
        boidDead = false;
    }

    public IEnumerator Immobilized(float duration)
    {
        if (boidDead == false)
        {
            rb.useGravity = true;
            boidImmobilized = true;

            yield return new WaitForSeconds(duration);
        }

        if (boidDead == false)
        {
            rb.useGravity = false;
            boidImmobilized = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "GravGrenade")
        {
            StartCoroutine(Immobilized(3f));
        }
    }
}