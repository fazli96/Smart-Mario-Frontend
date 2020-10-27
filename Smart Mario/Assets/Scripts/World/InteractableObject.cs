using UnityEngine;

/// <summary>
/// This is an abstract class for interactable objects in the world map
/// </summary>
public abstract class InteractableObject: MonoBehaviour
{
    /// <summary>
    /// This is an abstract method to set text on screen 
    /// for interactable objects to implement
    /// </summary>
    /// <returns></returns>
    public abstract string DisplayText();

    /// <summary>
    /// This is an abstract method for interactable 
    /// objects to implement to do some action
    /// </summary>
    /// <returns></returns>
    public abstract void Action();
}
