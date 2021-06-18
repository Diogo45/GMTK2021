using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protester : BoidFollow
{
    public HashSet<Boid> EnemyNeighboors { get; protected set; }

    [SerializeField]
    protected int EnemyNeighboorVisualization;

    protected override void Start()
    {
        base.Start();
        EnemyNeighboors = new HashSet<Boid>();

    }

    protected override void Update()
    {
        base.Update();
        EnemyNeighboorVisualization = EnemyNeighboors.Count;

        if (Leader && boidParameters.Team == Team.ForChangeRiot)
        {
            IsLeaderLost();

            HandleLeaderLost();
        }

    }

    protected override Vector3 BoidMove()
    {
        Vector2 avoidPolice = CalculateSeparationOtherTeams() * 15f;

        return base.BoidMove() + new Vector3(avoidPolice.x, 0f, avoidPolice.y);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("Boid"))
        {
            var boid = other.GetComponent<Boid>();

            if (boid.boidParameters.Team != boidParameters.Team)
            {
                EnemyNeighboors.Add(boid);
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.CompareTag("Boid"))
        {
            var boid = other.GetComponent<Boid>();

            if (boid.boidParameters.Team != boidParameters.Team)
            {
                EnemyNeighboors.Remove(boid);
            }
        }
    }

    protected virtual void IsLeaderLost()
    {

        if (EnemyNeighboors.Count > Neighboors.Count)
        {
            _isLeaderLost = true;
        }
        else
        {
            _isLeaderLost = false;
            _lostLeaderTimer = _lostLeaderTimerValue;
        }

    }

    protected virtual void HandleLeaderLost()
    {
        if (_isLeaderLost)
        {
            _lostLeaderTimer -= Time.deltaTime;

            if (_lostLeaderTimer <= 0f)
            {
                StartCoroutine(ResetLeaderLost());
                Leader = null;

            }
        }
    }

    protected virtual IEnumerator ResetLeaderLost()
    {
        yield return new WaitForSeconds(_lostLeaderBuffer);

        _lostLeaderTimer = _lostLeaderTimerValue;
        _isLeaderLost = false;

    }

    protected virtual Vector2 CalculateSeparationOtherTeams()
    {
        Vector2 s = Vector2.zero;

        foreach (var neigh in EnemyNeighboors)
        {

            var t = Rigidbody.position - neigh.Rigidbody.position;

            t.y = 0f;

            var tm = t.magnitude > 0 ? t.magnitude : 1f;

            s += new Vector2(t.x, t.z).normalized * 1f / tm;

        }

        return s;
    }
}
