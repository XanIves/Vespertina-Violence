using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePatrol : MonoBehaviour
{
    public GameObject Melee;
    public GameObject Shooter;
    private Transform temp;

    // Start is called before the first frame update
    void Start()
    {
        int random = Random.Range(0, 2);
        temp = this.GetComponent<Transform>();

        if (random == 0)
        {
            Instantiate( Melee, temp.localPosition , Quaternion.identity);
        }
        else
        {
            Instantiate( Shooter, temp.localPosition , Quaternion.identity);
        }
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        


    }
}
