using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRelativeToPortal : MonoBehaviour
{
    Transform otherPlayerTransform;
    GameObject otherPlayer;
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
        if (FindObjectsOfType<PlayerController>().Length > 1 && !otherPlayerTransform)
        {
            GameObject empty = new GameObject();

            foreach (PlayerController pc in FindObjectsOfType<PlayerController>())
            {
                if (pc.isSecondPlayer)
                {
                    empty.transform.position = pc.transform.position + FindObjectOfType<SceneTransitioner>().nextLevelPositionOffset;
                    empty.transform.rotation = pc.transform.rotation;
                    otherPlayer = pc.gameObject;
                }
            }
            otherPlayerTransform = empty.transform;

            otherPlayer.gameObject.SetActive(false);
        }

        if (!otherPlayerTransform) { return; }

        Matrix4x4 playerCamMatrix = FindObjectOfType<PlayerController>().GetComponentInChildren<Camera>().transform.localToWorldMatrix;
        Matrix4x4 portalMatrix = portal.worldToLocalMatrix;
        Matrix4x4 otherPlayerMatrix = otherPlayerTransform.localToWorldMatrix;

        Matrix4x4 m = otherPlayerMatrix * portalMatrix * playerCamMatrix;

        transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);
    }
}
