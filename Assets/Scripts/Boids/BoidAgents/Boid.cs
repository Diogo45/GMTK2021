using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;




[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Boid : MonoBehaviour
{

    public enum Team
    {
        NeutralCrowd,
        ForChangeRiot,
        StatusQuoRiot,
        Police
    }

    [field: SerializeField] public BoidParams boidParameters { get; protected set; }

    [SerializeField]
    protected int NeighboorsCount;


    public HashSet<Boid> Neighboors { get; protected set; }
   
    public HashSet<Collider> Obstacles { get; protected set; }

    protected SphereCollider radiusCollider;
    public Rigidbody Rigidbody { get; protected set; }


   

    protected virtual void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        radiusCollider = GetComponent<SphereCollider>();
        Neighboors = new HashSet<Boid>();
        Obstacles = new HashSet<Collider>();
    }

    protected virtual void Update()
    {
        radiusCollider.radius = boidParameters.LevelOfSeparation;
        NeighboorsCount = Neighboors.Count;
              
    }


    protected void FixedUpdate()
    {
        Rigidbody.velocity = Rigidbody.velocity + BoidMove() * Time.fixedDeltaTime;

        if (Rigidbody.velocity.sqrMagnitude > boidParameters.MaxVelocity * boidParameters.MaxVelocity)
        {
            Rigidbody.velocity *= (boidParameters.MaxVelocity * boidParameters.MaxVelocity) / Rigidbody.velocity.sqrMagnitude;
        }

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boid"))
        {
            var boid = other.GetComponent<Boid>();
            if (boid.boidParameters.Team == boidParameters.Team)
            {
                if (Neighboors.Count < 100)
                    Neighboors.Add(boid);
            }
           
        }

        if (other.CompareTag("Obstacle"))
            Obstacles.Add(other);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Boid"))
        {
            var boid = other.GetComponent<Boid>();
            Neighboors.Remove(boid);
        }

        if (other.CompareTag("Obstacle"))
            Obstacles.Remove(other);
    }





    protected virtual Vector3 BoidMove()
    {

        Vector2 separation = CalculateSeparation() * boidParameters.Separation;
        Vector2 cohesion_displacement = CalculateDisplacement() * boidParameters.Cohesion;
        Vector2 alignment = CalculateAlignment() * boidParameters.Alignment;
        Vector2 obst = AvoidObstacles();

       

        Vector2 velocity = separation + cohesion_displacement + alignment /* + obst * (boid_params.obstacles + follow.magnitude)*/;

        var v = new Vector3(velocity.x, 0f, velocity.y);

       

        return v;
      
    }


    protected virtual Vector2 CalculateSeparation()
    {
        Vector2 s = Vector2.zero;

        foreach (var neigh in Neighboors)
        {
            var t = Rigidbody.position - neigh.Rigidbody.position;

            t.y = 0f;

            var tm = t.magnitude > 0 ? t.magnitude : 1f;

            s += new Vector2(t.x, t.z).normalized * 1f / tm;

            //s -= new Vector2(t.x, t.z);
        }

        return s;
    }

    protected virtual Vector2 CalculateDisplacement()
    {

        Vector2 c = Vector2.zero;

        foreach (var neigh in Neighboors)
        {
            var t = neigh.Rigidbody.position;

            c += new Vector2(t.x, t.z);
        }
        var t2 = Rigidbody.position;
        return Neighboors.Count != 0 ? ((c / Neighboors.Count) - new Vector2(t2.x, t2.z)) : Vector2.zero;
    }

    protected virtual Vector2 CalculateAlignment()
    {
        var m = Vector2.zero;

        foreach (var neigh in Neighboors)
        {
            var t = new Vector2(neigh.Rigidbody.velocity.x, neigh.Rigidbody.velocity.z);
            m += t;
        }

        return Neighboors.Count != 0 ? (m / Neighboors.Count) : Vector2.zero;
    }

    protected virtual Vector2 AvoidObstacles()
    {
        Vector2 s = Vector2.zero;

        foreach (var obst in Obstacles)
        {
            Physics.Raycast(Rigidbody.position, obst.transform.position, out RaycastHit hit);
            var t = Rigidbody.position - hit.point;

            var tm = t.magnitude > 0 ? t.magnitude : 1f;

            s += new Vector2(t.x, t.z)/*.normalized * 1f / tm*/;
        }
        
        return s;
    }

}

