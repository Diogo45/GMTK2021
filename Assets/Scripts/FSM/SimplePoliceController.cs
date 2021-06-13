using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePoliceController : MonoBehaviour
{

    public BoidLeader PlayerCursor;
    public BoidLeader PoliceLeader;

    public float MoveTimer;
    public float MoveSpeed;

    public Vector3 Target;

    public IEnumerator MovePolice()
    {
        //while()
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator TriggerMovePolice()
    {
        while (true)
        {
            yield return new WaitForSeconds(MoveTimer);
            MovePolice();
        }
    }

    // Start is called before the first frame update
    void Start()
    {



    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
