using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidVisualization : MonoBehaviour
{

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.GetComponentInParent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
