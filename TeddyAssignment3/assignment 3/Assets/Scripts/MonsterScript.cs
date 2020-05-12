using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    public bool useGoalObject;
    public Transform goal;
    private NavMeshAgent agent_;

    // Start is called before the first frame update
    void Start()
    {
        agent_ = GetComponent<NavMeshAgent>();
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
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    agent_.SetDestination(hit.point);
                }
            }
        }

    }
}
