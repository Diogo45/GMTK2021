using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidVisualization : MonoBehaviour
{

    private Rigidbody rb;

    [SerializeField] private float _rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = rb.velocity.normalized;

        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            transform.rotation = targetRotation;
        }
    }
}
