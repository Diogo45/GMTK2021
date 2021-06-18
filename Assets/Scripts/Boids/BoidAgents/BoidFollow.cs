using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidFollow : Boid
{

    protected BoidLeader Leader;

    protected const float _lostLeaderTimerValue = 1f;
    protected const float _lostLeaderBuffer = 10f;
    protected float _lostLeaderTimer = _lostLeaderTimerValue;
    protected bool _isLeaderLost = false;


    protected virtual Vector2 FollowLeader()
    {
        var tv = Leader.Rigidbody.position - Rigidbody.position;
        var behind = tv * -1f;
        behind = behind.normalized;
        behind *= boidParameters.DistanceBehindLeader;
        return new Vector2(tv.x, tv.z) - new Vector2(behind.x, behind.z);
    }


    protected override Vector3 BoidMove()
    {
        Vector2 follow = Leader && boidParameters.FollowLeader > 0f ? FollowLeader() : Vector2.zero;

        return base.BoidMove() + new Vector3(follow.x, 0f, follow.y);
    }
 

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("Leader"))
        {
            if (!Leader)
            {
                var boid = other.GetComponent<Boid>();

                if (boid.boidParameters.Team != boidParameters.Team)
                    return;

                if (!_isLeaderLost)
                {
                    Leader = other.GetComponent<BoidLeader>();
                    boidParameters = Leader.boidParameters;
                }
            }
        }
    }
}
