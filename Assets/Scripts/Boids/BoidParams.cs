using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidParams : MonoBehaviour
{
    [Range(0f, 5f)] public float Separation;
    [Range(0f, 1f)] public float Alignment;
    [Range(0f, 1f)] public float Cohesion;
    [Range(0f, 1f)] public float ObstacleAvoidance;
    [Range(5f, 15f)] public float LevelOfSeparation;

    [Range(0f, 1f)] public float FollowLeader;
    [Range(0f, 1f)] public float DistanceBehindLeader;

    [Range(1, 25f)] public float MaxVelocity;

    public Boid.Team Team;
}
