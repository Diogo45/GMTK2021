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
        while ((Target - transform.position).magnitude > 0.02f)
            transform.position = transform.position + (Target - transform.position).normalized * MoveSpeed;
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator TriggerMovePolice()
    {
        while (true)
        {
            yield return new WaitForSeconds(MoveTimer);
            Vector3 v = Vector3.zero;
            foreach (var n in PlayerCursor.TeamNeighboors)
                v += n.transform.position;
            v /= PlayerCursor.TeamNeighboors.Count;
            v.y = 0.0f;
            Target = v;

            MovePolice();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TriggerMovePolice());
    }

}
