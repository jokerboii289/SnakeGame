using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Follower : MonoBehaviour
{
    public Vector3 followOffset;
    public Transform charToFollow;
    public float damping;

    private void LateUpdate()
    {
        if (charToFollow != null)
        {
            Vector3 smoothPos = Vector3.Lerp(transform.position, charToFollow.position + followOffset,Time.deltaTime * damping);
            transform.position = smoothPos;
            //transform.DOMove(smoothPos, 0.01f);
            transform.eulerAngles = charToFollow.eulerAngles;
            // transform.DORotate(charToFollow.eulerAngles, 0.1f);
        }
    }
}
