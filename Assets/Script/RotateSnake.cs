using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Dreamteck.Splines;

public class RotateSnake : MonoBehaviour
{

    [SerializeField] GameObject snakeHead;
    [SerializeField] GameObject snakeBody;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0; 
    }
    private void Update()
    {
        timer += .2f*Time.deltaTime; 
        snakeHead.GetComponent<SplineFollower>().SetClipRange(1-timer,1);
        snakeBody.GetComponent<TubeGenerator>().SetClipRange(1 - timer, 1);
    }

}
