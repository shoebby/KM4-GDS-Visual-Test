using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float cameraHeight = 1.25f;
    public PlayerController script;
    public static bool overview;
    public static bool switchedView;

    public static Quaternion spawnRot;

    private float rotY;
    private float sens;

    public Vector3 overviewOffset = new Vector3(20, 15, 40);
    public static Vector3 overviewPos;
    public float clamp = 80;
    
    void Start()
    {
        sens = script.sensitivity_x;
        spawnRot = transform.rotation;
    }

    
    void Update()
    {
        sens = script.sensitivity_x;

        rotY -= Input.GetAxis("Mouse Y") * Time.deltaTime * sens;

        transform.localPosition = Vector3.up * (cameraHeight - 0.5f);
        this.transform.rotation = player.rotation;

        //clamp
        rotY += this.transform.rotation.eulerAngles.x;
        rotY = Mathf.Clamp(rotY, -80, 80);

        this.transform.rotation = Quaternion.Euler(rotY, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z);
    }
}
