using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private GameObject target;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float minX, maxX, minY, maxY;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");//MARKER dont forget to tag player as tag
    }

    private void Update()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z), smoothSpeed * Time.deltaTime);

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX),
                                             Mathf.Clamp(transform.position.y, minY, maxY),
                                             transform.position.z);
        }
        
    }

}
