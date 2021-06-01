using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRelativeToPortal : MonoBehaviour
{
    Transform otherPlayer;
    public Transform portal;

    void Start()
    {
        // if we already have an active MoveRelativeToPortal disable this one
        if (FindObjectsOfType<MoveRelativeToPortal>().Length > 1)
        {
            Destroy(gameObject);
        }

        //portal = transform.parent;
        transform.parent = null;
    }

    void Update()
    {
        if (FindObjectsOfType<PlayerController>().Length > 1 && !otherPlayer)
        {
            GameObject empty = new GameObject();
            empty.transform.position = FindObjectsOfType<PlayerController>()[0].transform.position + FindObjectOfType<SceneTransitioner>().nextLevelPositionOffset;
            empty.transform.rotation = FindObjectsOfType<PlayerController>()[0].transform.rotation;
            Debug.Log(empty.transform.position);
            otherPlayer = empty.transform;

            FindObjectsOfType<PlayerController>()[0].gameObject.SetActive(false);
        }

        if (!otherPlayer) { return; }

        Matrix4x4 playerCamMatrix = FindObjectOfType<PlayerController>().GetComponentInChildren<Camera>().transform.localToWorldMatrix;
        Matrix4x4 portalMatrix = portal.worldToLocalMatrix;
        Matrix4x4 otherPlayerMatrix = otherPlayer.localToWorldMatrix;

        Matrix4x4 m = otherPlayerMatrix * portalMatrix * playerCamMatrix;

        transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);
    }
}
