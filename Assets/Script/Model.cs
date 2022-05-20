using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    [SerializeField] float speed;

    private void Start()
    {
        StartCoroutine(Delay());
    }
    private void Update()
    {
        speed += .5f * Time.deltaTime;
        if(transform.localScale.x>=0)
         transform.localScale = new Vector3(transform.localScale.x-speed, transform.localScale.x - speed, transform.localScale.x - speed);
    }
    
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
