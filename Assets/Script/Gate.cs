using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dreamteck.Splines;
using UnityEngine.UI;

public class Gate : MonoBehaviour
{
    [SerializeField] bool move;
    [SerializeField] float SpeedForGate;
    [SerializeField]TextMeshPro gateSpeed;
    float timer;
    SplineFollower splineFollower;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        gateSpeed.text = SpeedForGate.ToString();
        if(move)
        {
            splineFollower = GetComponent<SplineFollower>();
        }
    }
    private void Update()
    {
        if(move)
        {
            MoveGate();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<SplineFollower>().followSpeed += SpeedForGate;
        }
    }

    void MoveGate()
    {
        timer += 1 * Time.deltaTime;
        var value = (Mathf.Sin(timer));
        var temp = value * 7.5f;
        //gameObject.transform.localScale = new Vector3(size + temp, size + temp, size + value);
        splineFollower.motion.offset = new Vector2(Mathf.Clamp(temp,-10,10),splineFollower.motion.offset.y);
    }
}
