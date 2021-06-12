using System;
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
    public HashSet<BoidAgent> neighbours;
    public int neighVis; 

    //obstacle list
    public List<Collider> obstacles;

    public SphereCollider Radius_collider;
    public Rigidbody rb { get; private set; }

    public float vMax;

    public int Id { get; private set; }

    // Start is called before the first frame update
    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        neighbours = new HashSet<BoidAgent>();
        

        Id = Guid.NewGuid().GetHashCode();

        //var blah = Physics.OverlapSphere(rb.position, Radius_collider.radius, LayerMask.GetMask("Boid"));

        //foreach (var neigh in blah)
        //    neighbours.Add(neigh.GetComponent<BoidAgent>());

        //rb.velocity = Vector3.forward;
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        neighVis = neighbours.Count;
        BoidMove(Time.fixedDeltaTime);
    }

    // detected another boid entering neighbour radius
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boid"))
        {
            var boid = other.GetComponent<BoidAgent>();
            if (neighbours.Count < 100)
                neighbours.Add(boid);
            
        }

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

        Vector2 separation = CalculateSeparation() * boid_params.separations;
        Vector2 cohesion_displacement = CalculateDisplacement() * boid_params.cohesion;
        Vector2 alignment = CalculateAlignment() * boid_params.alignment;

        Vector2 follow = boid_params.Leader ? FollowLeader() * boid_params.FollowLeader : Vector2.zero;
        //Vector2 obst = AvoidObstacles();

        Vector2 velocity = separation + cohesion_displacement + alignment + follow /*+ obst * boid_params.obstacles*/;

        var v = new Vector3(velocity.x, 0f, velocity.y);

        if (rb.velocity.sqrMagnitude > vMax * vMax)
        {
            rb.velocity *= (vMax * vMax) / rb.velocity.sqrMagnitude;
        }

        rb.velocity = rb.velocity + v * rate;
        //rb.AddForce(rb.velocity + v/* * rate*/);


    }

    public virtual Vector2 CalculateSeparation()
    {
        Vector2 s = Vector2.zero;

        foreach (var neigh in neighbours)
        {
            var t = rb.position - neigh.rb.position;

            var tm = t.magnitude;



            s += new Vector2(t.x, t.z).normalized * 1f / tm;

            //s -= new Vector2(t.x, t.z);
        }

        return s;
    }

    public virtual Vector2 CalculateDisplacement()
    {

        Vector2 c = Vector2.zero;

        foreach (var neigh in neighbours)
        {
            var t = neigh.rb.position;

            c += new Vector2(t.x, t.z);
        }
        var t2 = rb.position;
        return neighbours.Count != 0 ? ((c / neighbours.Count) - new Vector2(t2.x, t2.z)) : Vector2.zero;
    }

    public virtual Vector2 CalculateAlignment()
    {
        var m = Vector2.zero;

        foreach (var neigh in neighbours)
        {
            var t = new Vector2(neigh.rb.velocity.x, neigh.rb.velocity.z);
            m += t;
        }

        return neighbours.Count != 0 ? (m / neighbours.Count) : Vector2.zero;
    }

    public virtual Vector2 FollowLeader()
    {
        var tv = boid_params.Leader.rb.position - rb.position;
        var behind = tv * -1f;
        behind = behind.normalized;

        behind *= boid_params.DistanceBehindLeader;

        return new Vector2(tv.x, tv.z) - new Vector2(behind.x, behind.z);

    }

    public virtual Vector2 AvoidObstacles()
    {
        Vector2 s = Vector2.zero;

        foreach (var obst in obstacles)
        {
            Physics.Raycast(rb.position, obst.transform.position, out RaycastHit hit);
            var t = rb.position - hit.point;

            var tm = t.magnitude;

            s += new Vector2(t.x, t.z).normalized * 1f / tm;
        }

        //var x = s.x > 0 ? 1 / s.x : 0f;
        //var y = s.y > 0 ? 1 / s.y : 0f;

        return s;
    }


    public void SetParams(BoidParams boidparams)
    {
        boid_params = boidparams;
    }

}

