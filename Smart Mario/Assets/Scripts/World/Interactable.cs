using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// This class is to allow players to access interactable objects on the map
/// while player is moving around the world map
/// </summary>
public class Interactable : MonoBehaviour
{

    public Text interactableText;
    public GameObject interactableTextPanel;
    public bool isInRange;
    public KeyCode interactKey;

    /// <summary>
    /// This is to initialize the interactable text panel which will only show
    /// when players approach any interactable objects on the map
    /// </summary>
    void Start()
    {
        interactableTextPanel.SetActive(false);
        interactableText.text = "";
    }

    /// <summary>
    /// When player is near the interactable object on the map,
    /// allow player to do some action by pressing the E key 
    /// </summary>
    void Update()
    {
        if (isInRange) //if player is in range
        {
            // Liskov substitution principle
            InteractableObject interactableObject = GetComponent<InteractableObject>();

            interactableText.text = "Press E to " + interactableObject.DisplayText();
            if (Input.GetKeyDown(interactKey)) //and presses the interact key
            {
                interactableObject.Action(); //do the action associated with the object
            }
        }
    }

    /// <summary>
    /// Detect player when player enters the radius of any interactable object
    /// </summary>
    /// <param name="collision player character on map"></param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            interactableText.enabled = true;
            interactableTextPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Detect player when player leaves the radius of any interactable object
    /// </summary>
    /// <param name="collision playerCharacter on map"></param>
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            interactableText.enabled = false;
            interactableTextPanel.SetActive(false);
        }
    }
}
