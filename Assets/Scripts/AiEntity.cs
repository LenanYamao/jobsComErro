using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

public class AiEntity : MonoBehaviour
{
    public GameManager gm;
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public bool canFly = false;

    public Transform sprite;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;
    Rigidbody2D rb;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        //InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (target == null) return;
        //if(seeker.IsDone())
        //    seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    //private void OnPathComplete(Path p)
    //{
    //    if (p.error) return;

    //    path = p;
    //    currentWaypoint = 0;
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (path == null) return;

        //if (currentWaypoint >= path.vectorPath.Count)
        //{
        //    reachedEndOfPath = true;
        //    return;
        //}
        //else
        //{
        //    reachedEndOfPath = false;
        //}

        Vector2 force = new Vector2();
        if (!gm.useJob)
        {
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;
            force = direction * speed * Time.deltaTime;  // force that will move the enemy in the desired direction
            rb.AddForce(force);
        }
        else
        {
            var position = new NativeArray<Vector2>(1, Allocator.Persistent);
            var targetArray = new NativeArray<Vector2>(1, Allocator.Persistent);
            var forceArray = new NativeArray<Vector2>(1, Allocator.Persistent);

            position[0] = rb.position;
            targetArray[0] = (Vector2)target.position;

            // Initialize the job data
            var job = new VelocityJob()
            {
                deltaTime = Time.deltaTime,
                position = position,
                target = targetArray,
                force = forceArray,
                speed = speed
            };

            JobHandle jobHandle = job.Schedule(position.Length, 32);

            // Ensure the job has completed.
            // It is not recommended to Complete a job immediately,
            // since that reduces the chance of having other jobs run in parallel with this one.
            // You optimally want to schedule a job early in a frame and then wait for it later in the frame.
            jobHandle.Complete();

            force = job.force[0];
            rb.AddForce(force);

            position.Dispose();
            targetArray.Dispose();
            forceArray.Dispose();
        }


        //float distance = Vector2.Distance(rb.position, target.position);

        //if (distance < nextWaypointDistance)
        //{
        //    currentWaypoint++;
        //}

        if (force.x >= 0.01f)
        {
            sprite.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (force.x <= -0.01f)
        {
            sprite.localScale = new Vector3(-1f, 1f, 1f);

        }
    }
}

[BurstCompile]
public struct VelocityJob : IJobParallelFor
{
    // Jobs declare all data that will be accessed in the job
    // By declaring it as read only, multiple jobs are allowed to access the data in parallel
    public NativeArray<Vector2> position;

    // By default containers are assumed to be read & write
    public NativeArray<Vector2> target;

    public NativeArray<Vector2> force;

    // Delta time must be copied to the job since jobs generally don't have concept of a frame.
    // The main thread waits for the job same frame or next frame, but the job should do work deterministically
    // independent on when the job happens to run on the worker threads.
    public float deltaTime;
    public float speed;

    // The code actually running on the job
    public void Execute(int i)
    {
        // Move the positions based on delta time and velocity
        var dir = (target[i] - position[i]).normalized;
        force[i] = dir * speed * deltaTime;
    }
}