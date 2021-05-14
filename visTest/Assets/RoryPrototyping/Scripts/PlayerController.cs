using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject throwLeft, throwRight;
    public GameObject thrownLeft, thrownRight;

    private float rightKey, leftKey, upKey, downKey; //jumpKey, crouchKey, action1Key;
    private float grabKey;

    public Transform cam, throwTrans;
    public GameObject dragPoint, swingPoint, rope;
    private Vector3 dragVec;

    public float jumpVel, moveVel, maxSpeed;
    public float sensitivity_x, sensitivity_y;

    //groundMargin is the range where you touch the ground
    public float groundMargin = 0.5f;

    //stepHeight is the max height for automatically stepping over objects
    public float stepHeight = 0.65f;

    //the time you can be airborne but still jump
    public float canJumpDelay = 0.4f;

    public float grappleRange = 30, ropeGrabForce = 20f, ropeReelSpeed = 5f, throwForce = 80f;
    public float duckHeight = 0.3f, crouchSmoothStepSpeed = 10;
    public float crouchColliderSizeY = 1.3f, crouchTriggerSizeY = 1.3f;
    public LayerMask ground_layer, fireLayers;

    private bool ground, canJump, isGrappling;
    [HideInInspector] public bool justJumped;

    private float velX, velZ;
    [HideInInspector] public float speed, distance;

    private RaycastHit groundHit, hit;
    private Vector3 groundNormal;

    public static Vector3 spawnPos;
    public static Quaternion spawnRot;

    private float mouseHorizontal;
    private Rigidbody rb;
    private BoxCollider bc;
    private BoxCollider bct;
    public float colliderSizeY, triggerSizeY;
    private MeshRenderer mesh;
    private Vector3 mScale, mPos;
    public CameraController cc;
    private float camHeight;
    private ConfigurableJoint cj;
    private SoftJointLimit limit;
    private GameObject ropeInstance;
    private LineRenderer lr;
    private GameObject swing;
    private Vector3 currentRotation;
    [HideInInspector] public Vector3 currentDirection, targetDirection;

    private Rigidbody grabbedObject;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponents<BoxCollider>()[0];
        colliderSizeY = bc.size.y;
        bct = GetComponents<BoxCollider>()[1];
        triggerSizeY = bct.size.y;
        mesh = GetComponentsInChildren<MeshRenderer>()[0];
        mScale = mesh.transform.localScale;
        mPos = mesh.transform.localPosition;
        camHeight = cc.cameraHeight;

        spawnPos = gameObject.transform.position;
        spawnRot = gameObject.transform.rotation;

        dragVec = dragPoint.transform.localPosition;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            NormalActions();
        }
    }

    void NormalActions()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            mesh.gameObject.transform.localScale = new Vector3(mesh.transform.localScale.x, (crouchColliderSizeY + 0.2f), mesh.transform.localScale.z);
            mesh.gameObject.transform.localPosition = new Vector3(mesh.transform.localPosition.x, -(colliderSizeY - (crouchColliderSizeY + 0.2f)) * 0.5f, mesh.transform.localPosition.z);
            bc.size = new Vector3(bc.size.x, crouchColliderSizeY, bc.size.z);
            bc.center = new Vector3(bc.center.x, -(colliderSizeY - crouchColliderSizeY) * 0.5f, bc.center.z);
            bct.size = new Vector3(bct.size.x, crouchColliderSizeY, bct.size.z);
            bc.material.dynamicFriction = 0f;
            cc.cameraHeight = Mathf.SmoothStep(cc.cameraHeight, duckHeight - 0.5f, Time.deltaTime * crouchSmoothStepSpeed);
        }
        else
        {
            mesh.gameObject.transform.localScale = mScale;
            mesh.gameObject.transform.localPosition = mPos;
            bc.size = new Vector3(bc.size.x, colliderSizeY, bc.size.z);
            bc.center = new Vector3(bc.center.x, 0, bc.center.z);
            bct.size = new Vector3(bct.size.x, triggerSizeY, bct.size.z);
            cc.cameraHeight = Mathf.SmoothStep(cc.cameraHeight, camHeight - 0.5f, Time.deltaTime * crouchSmoothStepSpeed);
        }

        UpdateCameraHorizontal();

        getGroundNormal();

        //store the current velocity direction
        currentDirection = rb.velocity.normalized;

        //calculate the current s p e e d
        speed = rb.velocity.magnitude;

        //check the values for all the buttons
        CheckInputs();

        if (Input.GetMouseButtonDown(1))
        {
            if (thrownRight && thrownRight.GetComponent<BulletHitActivator>().invokeOnRethrow)
            {
                thrownRight.GetComponent<BulletHitActivator>().hitEvent.Invoke();
            }
            else
            {
                Throw(throwRight, false);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (thrownLeft && thrownLeft.GetComponent<BulletHitActivator>().invokeOnRethrow)
            {
                thrownLeft.GetComponent<BulletHitActivator>().hitEvent.Invoke();
            }
            else
            {
                Throw(throwLeft, true);
            }
        }

        //set velocity from right and left keys
        velX = rightKey - leftKey;
        velZ = upKey - downKey;

        targetDirection = transform.TransformVector(velX, 0, velZ);
        targetDirection = Vector3.ProjectOnPlane(targetDirection, groundNormal);

        if (ground && Input.GetKeyDown(KeyCode.Space) || canJump && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (speed < maxSpeed)
        {
            //like 45 for move vel
            rb.AddForce(targetDirection.normalized * Time.deltaTime * moveVel, ForceMode.VelocityChange);

            //3000 for move vel
            //rb.AddForce(targetDirection.normalized * Time.deltaTime * moveVel, ForceMode.Force);
        }
        else
        {
            Vector3 current = rb.velocity.normalized;

            //like 45 for move vel
            rb.AddForce((targetDirection - current) * Time.deltaTime, ForceMode.VelocityChange);

            //3000 for move vel
            //rb.AddForce((targetDirection - current) * Time.deltaTime, ForceMode.Force);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            justJumped = false;
        }

        //if it is a moving ship or something add force to the player to move along here


        if (collision.gameObject.tag == "death")
        {
            Respawn(true);
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        //you can keep jumping after disconnecting from a wall (unintended)
        if (collision.gameObject.tag == "ground" && !justJumped || collision.gameObject.tag == "spear" && !justJumped)
        {
            StartCoroutine(CanJumpDelay());
        }
    }

    void getGroundNormal()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out groundHit, 1.4f, ground_layer))
        {
            groundNormal = groundHit.normal;
        }
        else
        {
            groundNormal = Vector3.up;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & ground_layer) != 0)
        {
            ground = true;
        }

        if (other.gameObject.tag == "death")
        {
            Respawn(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & ground_layer) != 0)
        {
            ground = true;
        }
        else
        {
            /*
            if (Input.GetMouseButtonDown(1) && other.isTrigger && other.GetComponent<BulletHitActivator>())
            {
                if (thrownRight)
                {
                    thrownRight.GetComponent<BulletHitActivator>().hitEvent.Invoke();
                }
            }*/
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & ground_layer) != 0)
        {
            ground = false;
        }
    }

    public void Respawn(bool death)
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        //Show Game over screen
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpVel, rb.velocity.z);
        canJump = false;
        justJumped = true;
        ground = false;
    }

    void UpdateCameraHorizontal()
    {
        //handle camera and player rotation on the y axis
        mouseHorizontal = Input.GetAxis("Mouse X");
        currentRotation = this.transform.rotation.eulerAngles;
        currentRotation.y += mouseHorizontal * Time.deltaTime * sensitivity_x;
        this.transform.rotation = Quaternion.Euler(currentRotation);
    }

    void Fire()
    {
        if (Physics.Raycast(cam.position, cam.forward, out hit, 100, fireLayers))
        {
            // Find the line from the gun to the point that was clicked.
            Vector3 incomingVec = hit.point - cam.position;

            // Use the point's normal to calculate the reflection vector.
            Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);

            // Draw lines to show the incoming "beam" and the reflection.
            Debug.DrawLine(cam.position, hit.point, Color.red);
            Debug.DrawRay(hit.point, reflectVec, Color.green);

            // ^^ I currently dont use the above reflection stuff

            if (hit.collider.gameObject.GetComponent<BulletHitActivator>())
            {
                hit.collider.gameObject.GetComponent<BulletHitActivator>().hitEvent.Invoke();
            }
            else if(hit.collider.gameObject.GetComponentInParent<BulletHitActivator>())
            {
                hit.collider.gameObject.GetComponentInParent<BulletHitActivator>().hitEvent.Invoke();
            }
            
            //spawn hit fx
            //Instantiate(fireFX, hit.point, Quaternion.identity);

            //when the shot object has a boid script, run their death function
            if (hit.collider.gameObject.GetComponentInParent<Boid>())
            {
                hit.collider.gameObject.GetComponentInParent<Boid>().Death();
            }
        }
    }

    void Throw(GameObject obj, bool left)
    {
        if (left)
        {
            thrownLeft = Instantiate(obj, throwTrans.position, throwTrans.rotation);
            thrownLeft.GetComponent<Rigidbody>().AddForce((throwTrans.position - transform.position).normalized * throwForce);
        }
        else
        {
            thrownRight = Instantiate(obj, throwTrans.position, throwTrans.rotation);
            thrownRight.GetComponent<Rigidbody>().AddForce((throwTrans.position - transform.position).normalized * throwForce);
        }
    }

    void StartGrapple()
    {
        if (Physics.Raycast(cam.position, cam.forward, out hit, grappleRange, ground_layer))
        {
            if (hit.collider.tag == "ground")
            {
                grabbedObject = hit.collider.attachedRigidbody;

                isGrappling = true;

                swing = Instantiate(swingPoint, hit.point, Quaternion.Euler(0, 0, 0));

                cj = swing.GetComponent<ConfigurableJoint>();
                cj.connectedBody = rb;

                limit = new SoftJointLimit();
                limit.limit = (gameObject.transform.position - hit.point).magnitude;
                cj.linearLimit = limit;

                CreateRope((cam.position - cam.up) + cam.forward, swing.transform.position);
            }
        }
    }

    void HoldGrapple()
    {
        if (!isGrappling) { return; }
        if (grabbedObject.tag == "ground")
        {
            
            //If you want to change the limit over time do that here
            limit.limit -= Time.deltaTime * ropeReelSpeed;
            cj.linearLimit = limit;

            if (limit.limit < 1f) //or if you touch ground after 0.6s of swinging (not implemented, but would be nice)
            {
                StopGrappling();
                return;
            }
           
        }
        DrawRope((cam.position - cam.up) + cam.forward, swing.transform.position);
    }

    void CreateRope(Vector3 start, Vector3 end)
    {
        if (!lr)
        {
            ropeInstance = Instantiate(rope);
            lr = ropeInstance.GetComponent<LineRenderer>();
        }
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    void DrawRope(Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    void StopGrappling()
    {
        if (isGrappling)
        {
            isGrappling = false;

            if (cj != null)
            {
                cj.connectedBody = null;
            }

            Destroy(ropeInstance);
            Destroy(swing);
            lr = null;
        } 
    }

    void CheckInputs()
    {
        if (Input.GetKeyUp("right") || Input.GetKeyUp(KeyCode.D))
        {
            rightKey = 0;
        }
        if (Input.GetKeyUp("left") || Input.GetKeyUp(KeyCode.A))
        {
            leftKey = 0;
        }
        if (Input.GetKeyUp("up") || Input.GetKeyUp(KeyCode.W))
        {
            upKey = 0;
        }
        if (Input.GetKeyUp("down") || Input.GetKeyUp(KeyCode.S))
        {
            downKey = 0;
        }

        if (Input.GetMouseButtonDown(1))
        {
            grabKey = 0f;
        }

        if (Input.GetKeyDown("right") || Input.GetKeyDown(KeyCode.D))
        {
            rightKey = 1;
        }
        if (Input.GetKeyDown("left") || Input.GetKeyDown(KeyCode.A))
        {
            leftKey = 1;
        }
        if (Input.GetKeyDown("up") || Input.GetKeyDown(KeyCode.W))
        {
            upKey = 1;
        }
        if (Input.GetKeyDown("down") || Input.GetKeyDown(KeyCode.S))
        {
            downKey = 1;
        }
 
        if (Input.GetMouseButtonDown(1))
        {
            grabKey = 1f;
        }
    }

    //a delay so you can jump canJumpDelay of time after losing contact with the ground
    IEnumerator CanJumpDelay()
    {
        float duration = canJumpDelay;
        float totalTime = 0;

        while (totalTime <= duration && !justJumped)
        {
            canJump = true;
            totalTime += Time.deltaTime ;
            yield return null;
        }

        if (duration <= totalTime || justJumped)
        {
            canJump = false;
            yield return null;
        }
    }
}