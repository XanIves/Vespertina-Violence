using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System;

public class PatrolScript : MonoBehaviour
{
	public Transform[] wayPoints;
	public float wayPointReachedThreshold = 0.5f;
	public Transform playerTransform;
	public int AIDetectionRange; // Uses world units, defines radius of detection.
	public GameObject Shooter;
	public GameObject Chaser;
	public Material monsterMaterial;
	public GameObject bullet;

	private int nextWayPoint_ = 0;
	private NavMeshAgent agent_;
	private bool isShooter;
	private bool playerSpotted = false;
	private float nextActionTime = 2.0f;
	public float period = 1.0f;


	void Start()
	{
		agent_ = GetComponent<NavMeshAgent>();

		// I don't want my agent to stop or slowdown at waypoints
		agent_.autoBraking = false;

		// Gotta cache the random number, almost messed up by having this in the for loop conditional :-/
		int randomNumber = UnityEngine.Random.Range(4, 9);

		// Initialize our new list
		Transform[] newWaypoints = new Transform[randomNumber];

		// Now shuffle the old list, so we can then just choose the first n = randcomNumber elements of the list
		Shuffle(wayPoints);

		for (int i = 0; i < randomNumber; i++)
		{
			newWaypoints[i] = wayPoints[i];
		}

		wayPoints = newWaypoints;

		// Now choose whether the patroller is a chaser or a shooter
		var random = new System.Random(gameObject.GetInstanceID() + Convert.ToInt32(System.DateTime.Now.Millisecond));
		var number = random.Next(0, 100);
		isShooter = number <= 50 ? true : false;

		if (isShooter)
		{
			Shooter.SetActive(true);
		}
		else
		{
			Chaser.SetActive(true);
		}

		GotoNextWaypoint_();
	}


	private void GotoNextWaypoint_()
	{
		if (!playerSpotted || !MonsterScript.gameRunning)
		{
			// Returns if no points have been set up
			if (wayPoints.Length == 0)
				return;

			// Set the agent to go to the currently selected destination.
			agent_.destination = wayPoints[nextWayPoint_].position;

			// Choose the next point in the array as the destination,
			// cycling to the start if necessary.
			nextWayPoint_ = (nextWayPoint_ + 1) % wayPoints.Length;
		}
		else
		{
			agent_.destination = playerTransform.position;
		}
	}


	void Update()
	{
		
		// Check for player within a radius of AIDetection of the patroller
		if (Vector3.Distance(playerTransform.position, transform.position) < AIDetectionRange
			&& MonsterScript.gameRunning
			&& Vector3.Angle(playerTransform.position - transform.position, transform.forward) < 45.0f)
		{
			var direction = playerTransform.position - transform.position;
			Debug.DrawRay(transform.position, direction, Color.red);
			// Send out a raycast to determine if the patroller can see the player
			if (Physics.Raycast(transform.position, direction, out _, Mathf.Infinity))
			{
				Debug.DrawRay(transform.position, direction, Color.yellow);
				playerSpotted = true;
				transform.gameObject.GetComponent<Renderer>().material.color = Color.red;
			}
		}

		if (playerSpotted)
		{
			if (MonsterScript.gameRunning)
			{
				if (isShooter)
				{
					transform.LookAt(playerTransform.position);
					if (Time.time > nextActionTime)
					{
						nextActionTime = Time.time + period;
						ShootBullet();
					}
				}
				else    // Case for being a chaser
				{
					agent_.destination = playerTransform.position;
				}
			}
			else
			{
				transform.gameObject.GetComponent<Renderer>().material = monsterMaterial;
			}
		}

		
		else
		{
			nextActionTime = Time.time + period;
			// Choose the next destination point when the agent gets
			// close to the current one.
			if (!agent_.pathPending &&
				agent_.remainingDistance < wayPointReachedThreshold)
			{
				GotoNextWaypoint_();

			}
		}


	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 9)
		{
			Physics.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider>());
		}
	}

	void ShootBullet()
	{
		var moddedPosition = transform.position + transform.TransformDirection(new Vector3(0.5f, 0, +2.5f));
		var direction = playerTransform.position - transform.position;
		direction.y = 0;
		Instantiate(bullet, moddedPosition, Quaternion.LookRotation(direction));
	}

	void Shuffle(Transform[] transArray)
	{
		for (int t = 0; t < transArray.Length; t++)
		{
			Transform tmp = transArray[t];
			int r = UnityEngine.Random.Range(t, transArray.Length);
			transArray[t] = transArray[r];
			transArray[r] = tmp;
		}
	}


}
