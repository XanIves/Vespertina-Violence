using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private float speed = 20.0f;
    private float timer = 4.0f;
    private float startTime;
    private void Start()
    {
        startTime = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time > startTime + timer)
        {
            Destroy(gameObject);
        }
        
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
