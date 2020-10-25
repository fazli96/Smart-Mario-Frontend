using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to ensure the camera follows the player on the map while playing
/// </summary>
public class CameraFollowPlayer : MonoBehaviour
{

    private GameObject target;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float minX, maxX, minY, maxY;

    /// <summary>
    /// This method is to set the target, for the camera to follow, to the player
    /// </summary>
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");//MARKER dont forget to tag player as tag
    }

    /// <summary>
    /// For every frame, make the camera position follow the player position
    /// </summary>
    private void Update()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            // this is to add a decelerating effect whenever player comes to a stop
            transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z), smoothSpeed * Time.deltaTime);

            // set camera position to the target position
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX),
                                             Mathf.Clamp(transform.position.y, minY, maxY),
                                             transform.position.z);
        }
        
    }

}
