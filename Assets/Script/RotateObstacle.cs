using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObstacle : MonoBehaviour
{
 
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.up, 5f * Time.deltaTime);
    }
}
