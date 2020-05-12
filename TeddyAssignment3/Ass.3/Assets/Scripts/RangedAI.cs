using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class RangedAI : MonoBehaviour
{
	private GameObject[] GOwayPoints;
	public Transform[] wayPoints;
	public float wayPointReachedThreshold = 1f;
	public bool patrol_ = true;
	private int nextWayPoint_ = 0;
	private NavMeshAgent agent_;
	private int rand;
	private GameObject player;
	private Transform playerLocation;
	private Vector3 playerPosition;
	private Transform RangedLocation;
	private Vector3 RangedPosition;
	public float distance;
	public float timer;
	public Rigidbody projectile;


	void Start()
	{

		// Delete Global and make instantiated here depending on use in detect function
		timer = 1.0f;
		player = GameObject.FindWithTag("Player");

		GameObject GOtemp;
		Transform TRANStemp;
		int random;
		rand = Random.Range(4, 9);

		GOwayPoints = GameObject.FindGameObjectsWithTag("Waypoint");
		for (int i = 0; i < 12; i++)
		{
			GOtemp = GOwayPoints[i];
			wayPoints[i] = GOtemp.GetComponent<Transform>();
		}


		for (int i = 0; i < 12; i++)
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
		if (wayPoints.Length == 0 || patrol_ == false)
			return;

		// Set the agent to go to the currently selected destination.
		agent_.destination = wayPoints[nextWayPoint_].position;

		// Choose the next point in the array as the destination,
		// cycling to the start if necessary.
		nextWayPoint_ = (nextWayPoint_ + 1) % rand;
	}

	private void detect()
	{

		playerLocation = player.GetComponent<Transform>();
		playerPosition = playerLocation.position;
		RangedLocation = this.GetComponent<Transform>();
		RangedPosition = RangedLocation.position;

		Vector3 Direction = playerPosition - RangedPosition;

		float relativeAngle = (Vector3.Angle(Direction, transform.forward));

		distance = Vector3.Distance(playerPosition, RangedPosition);




		if (!(Physics.Linecast(playerPosition, RangedPosition, ~8)))
		{
			if ((distance < 30) && (relativeAngle >= -45 && relativeAngle <= 45))
			{
				patrol_ = false;
				agent_.destination = RangedPosition;
			}
			if ((patrol_ == false) && (timer > 0))
			{
				timer -= Time.deltaTime;

			}
			else if ((patrol_ == false) && (timer < 0))
			{
				Rigidbody clone;
				clone = (Rigidbody)Instantiate(projectile, RangedPosition, projectile.rotation);
				clone.velocity = playerPosition - RangedPosition;
				timer = 1;

			}
		}
		else
		{
			patrol_ = true;
		}
	}





	void Update()
	{
		// Choose the next destination point when the agent gets
		// close to the current one.

		detect();



		if (!agent_.pathPending && agent_.remainingDistance < wayPointReachedThreshold && patrol_ == true)
		{

			gotoNextWaypoint_();

		}
	}

}