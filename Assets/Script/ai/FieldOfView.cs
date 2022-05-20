using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] bool splineUsed;

    public static FieldOfView instance;
    [SerializeField] GameObject nodeOne;
    //movement of ai snake towards food;
    [SerializeField] float speed;

    float positionTomoveX;
    bool canDetect;

    SplineFollower splineFollower;

    float timer;
    float xOffset;
    // Start is called before the first frame update
    void Start()
    {    
        instance = this;
        canDetect = true;
        splineFollower = nodeOne.GetComponent<SplineFollower>();
        xOffset = splineFollower.motion.offset.x; ;
    }

    private void Update()
    {
        if (!canDetect)
        {
            MoveTowardsFoods();
        }
        else
            SnakeLikeEffect();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canDetect)
        {
            if (other.gameObject.CompareTag("rededibles") || other.gameObject.CompareTag("greenedibles") || other.gameObject.CompareTag("blueedibles") || other.gameObject.CompareTag("yellowedibles"))
            {
                if(!splineUsed)
                    positionTomoveX = other.transform.position.x; // spline not used;
                else
                {
                   positionTomoveX= other.gameObject.GetComponent<SplineFollower>().motion.offset.x;
                }
                StartCoroutine(Delay());
                canDetect = false;
            }
        }     
    }

    public void EnableDetect()
    {
        canDetect = true;
    }

    void MoveTowardsFoods()
    {  
        splineFollower.motion.offset = new Vector2(Mathf.Lerp(splineFollower.motion.offset.x,positionTomoveX,speed*Time.deltaTime),splineFollower.motion.offset.y);
        
    }

    void SnakeLikeEffect()
    {
        splineFollower.motion.offset = new Vector2(Mathf.Clamp( splineFollower.motion.offset.x + SpawnRandom(),-12,12), splineFollower.motion.offset.y);
    }

    float SpawnRandom()
    {
       // print("snake");
        timer += 3f * Time.deltaTime;
        var value =(Mathf.Sin(timer));
        var temp = value * .2f;
     
        return temp;
    }

    //void CheckTheDistance()
    //{
    //    var distance = (transform.position - new Vector3(positionTomove.x, transform.position.y, positionTomove.z)).magnitude;
    //    if(distance<.1f)
    //    {
    //        canDetect = true;
    //    }
    //}

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3f);
        EnableDetect();
    }
}
