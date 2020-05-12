using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PatrolScript : MonoBehaviour
{
	private GameObject[] GOwayPoints;
	public Transform[] wayPoints;
	public float wayPointReachedThreshold = 0.5f;
	private bool patrol_ = true;
	private int nextWayPoint_ = 0;
	private NavMeshAgent agent_;
	//public int ID;
	private int rand;


	void Start()
	{
	 GameObject GOtemp;
	 Transform TRANStemp;
	 int random;
		rand = Random.Range(4, 9);

		GOwayPoints = GameObject.FindGameObjectsWithTag("Waypoint");
		for(int i = 0; i < 12; i++)
        {
			GOtemp = GOwayPoints[i];
			wayPoints[i] = GOtemp.GetComponent<Transform>();
        }


		for(int i = 0; i < 12; i++)
        {
			random = Random.Range(0, wayPoints.Length);
			TRANStemp = wayPoints[random];
			wayPoints[random] = wayPoints[i];
			wayPoints[i] = TRANStemp;


        }

		agent_ = GetComponent<NavMeshAgent>();

		// I don't want my agent to stop or slowdown at waypoints
		agent_.autoBraking = false;

		gotoNextWaypoint_();
	}


	private void gotoNextWaypoint_()
	{
		// Returns if no points have been set up
		if (wayPoints.Length == 0)
			return;

		// Set the agent to go to the currently selected destination.
		agent_.destination = wayPoints[nextWayPoint_].position;

		// Choose the next point in the array as the destination,
		// cycling to the start if necessary.
		nextWayPoint_ = (nextWayPoint_ + 1) % rand;
	}


	void Update()
	{
		// Choose the next destination point when the agent gets
		// close to the current one.
		if (!agent_.pathPending &&
			agent_.remainingDistance < wayPointReachedThreshold)
		{
			gotoNextWaypoint_();
		}
	}
}
