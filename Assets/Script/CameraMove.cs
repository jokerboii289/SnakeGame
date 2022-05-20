using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float damping;

    [HideInInspector] public bool followPlayer;
    private void Start()
    {
        followPlayer = true;
    }
    private void LateUpdate()
    {
        if (followPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime * damping);
           // transform.localRotation = Quaternion.Lerp(transform.rotation,player.rotation)
        }
    }
}
