using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    [SerializeField] GameObject tapToHelp;
    // Start is called before the first frame update
    void Start()
    {
        tapToHelp.SetActive(true);
    }

}
