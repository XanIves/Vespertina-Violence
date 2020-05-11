using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject cameraPivot;
    private bool rotating = false;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && rotating != true)
        {
            rotating = true;
            StartCoroutine(Rotate(Vector3.down * 90, 0.34f));
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && rotating != true)
        {
            rotating = true;
            StartCoroutine(Rotate(Vector3.up * 90, 0.34f));
        }

    }

    IEnumerator Rotate(Vector3 byAngles, float time)
    {
        var fromAngle = cameraPivot.transform.rotation;
        var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
        for (var t = 0f; t <= 1f; t += Time.deltaTime / time)
        {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }
        transform.rotation = toAngle;
        rotating = false;
    }
}
