using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    private GameObject selectedPlayer;

    public Vector3 offset;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        selectedPlayer = GameObject.FindGameObjectWithTag("SelectedPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = cam.WorldToScreenPoint(selectedPlayer.transform.position + offset);

        if (transform.position != pos)
            transform.position = pos;
    }
}
