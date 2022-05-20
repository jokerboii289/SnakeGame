using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    private void OnEnable()
    {
        transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
    }
    private void Update()
    {       
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        transform.SetParent(null);
        ObjectPooling.instance.AddToPool(gameObject);
    }

}
