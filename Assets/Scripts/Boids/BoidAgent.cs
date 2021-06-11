using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BoidTeam
{
    NeutralCrowd,
    ForChangeRiot,
    StatusQuoRiot,
    Police
}

[RequireComponent(typeof(Rigidbody))]
public class BoidAgent : MonoBehaviour
{
    public BoidParams boid_params;
    public List<BoidAgent> neighbours;

    //obstacle list
    public List<Collider> obstacles;

    public SphereCollider Radius_collider;
    public Rigidbody rb { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        neighbours = new List<BoidAgent>();

        var blah = Physics.OverlapSphere(rb.position, Radius_collider.radius, LayerMask.GetMask("Boid"));

        foreach (var neigh in blah)
            neighbours.Add(neigh.GetComponent<BoidAgent>());

        rb.velocity = Vector3.forward;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        BoidMove(Time.fixedDeltaTime);
    }

    // detected another boid entering neighbour radius
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Boid"))
            neighbours.Add(other.GetComponent<BoidAgent>());

        if (other.CompareTag("Obstacle"))
            obstacles.Add(other);
    }

    // detected another boid leaving neighbour radius
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Boid"))
            neighbours.Remove(other.GetComponent<BoidAgent>());

        if (other.CompareTag("Obstacle"))
            obstacles.Remove(other);
    }


    public virtual void BoidMove(float rate) 
    { 
        
        Vector2 separation = CalculateSeparation();
        Vector2 cohesion_displacement = CalculateDisplacement();
        Vector2 alignment = CalculateAlignment();

        Vector2 velocity = separation * boid_params.separations + cohesion_displacement * boid_params.cohesion + alignment * boid_params.alignment;

        var v = new Vector3(velocity.x, 0f, velocity.y);

        rb.velocity = rb.velocity +  v;


    }

    public virtual Vector2 CalculateSeparation()
    {
        Vector2 s = Vector2.zero;

        foreach(var neigh in neighbours)
        {
            var t = rb.position - neigh.rb.position;
            s += new Vector2(t.x, t.y);
        }

        return -s;
    }

    public virtual Vector2 CalculateDisplacement()
    {

        Vector2 c = Vector2.zero;

        foreach (var neigh in neighbours)
        {
            var t = neigh.rb.position;
            c += new Vector2(t.x, t.y);
        }
        var t2 = rb.position;
        return neighbours.Count != 0 ? (c / neighbours.Count) - new Vector2(t2.x, t2.y) : Vector2.zero;
    }

    public virtual Vector2 CalculateAlignment()
    {
        var m = Vector2.zero;

        foreach (var neigh in neighbours)
        {
            var t = new Vector2(neigh.rb.velocity.x, neigh.rb.velocity.y);
            m += t;
        }

        return neighbours.Count != 0 ? (m / neighbours.Count): Vector2.zero;
    }



    public void SetParams(BoidParams boidparams)
    {
        boid_params = boidparams;
    }

}

