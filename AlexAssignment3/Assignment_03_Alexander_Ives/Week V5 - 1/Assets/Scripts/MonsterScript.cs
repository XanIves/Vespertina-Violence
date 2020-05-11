using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.ThirdPerson;
using TMPro;

public class MonsterScript : MonoBehaviour
{
    public bool useGoalObject;
    public Transform goal;
    public GameObject pointer;
    public int numberOfTotalPickups;
    public ThirdPersonCharacter character;
    public TextMeshProUGUI textMesh;
    public GameObject endScreen;

    public static bool gameRunning = true;

    public NavMeshAgent agent_;
    private int playerScore = 0;

    


    // Start is called before the first frame update
    void Start()
    {
        agent_.updateRotation = false;
        if (useGoalObject)
            agent_.SetDestination(goal.localPosition);
    }


    // Update is called once per frame
    void Update()
    {
        if (!useGoalObject && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    agent_.SetDestination(hit.point);
                    pointer.transform.position = hit.point;
                }
            }
        }
        if(agent_.remainingDistance > agent_.stoppingDistance + 0.5f)
        {
            character.Move(agent_.desiredVelocity, false, false);
        }
        else
        {
            character.Move(Vector3.zero, false, false);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("pickup")){
            GameObject parent = collision.gameObject.transform.parent.gameObject;
            Destroy(collision.gameObject);
            Destroy(parent);
            playerScore += 1;
        }

        if (playerScore >= numberOfTotalPickups)
        {
            EndGame(0);
        }

        if (collision.gameObject.CompareTag("chaser"))
        {
            EndGame(1);
        }

        if (collision.gameObject.CompareTag("bullet"))
        {
            EndGame(2);
        }


    }
    private void EndGame(int result)
    {
        Debug.Log("Finished Game!");
        gameRunning = false;
        agent_.isStopped = true;

        endScreen.SetActive(true);

        if(result == 0)
        {
            textMesh.text = "You Win!";
        }
        else if (result == 1)
        {
            textMesh.text = "You Were Caught!";
        }
        else if(result == 2)
        {
            textMesh.text = "You Died!";
        }

    }
}
