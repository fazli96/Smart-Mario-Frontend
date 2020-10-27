using UnityEngine;

/// <summary>
/// This class implements methods defined in InteractableObject interface Matching Cards Interactable object
/// </summary>
public class InteractableMatchingCards : InteractableObject
{
    /// <summary>
    /// This method is to set the display text on screen 
    /// for Matching Cards minigame.
    /// It overrides the abstract method defined in the abstract class
    /// </summary>
    /// <returns></returns>
    public override string DisplayText()
    {
        return "play Matching Cards Minigame";
    }

    /// <summary>
    /// This method is to allow player to navigate to 
    /// difficulty selection screen for Matching Cards minigame 
    /// It overrides the abstract method defined in the abstract class
    /// </summary>
    /// <returns></returns>
    public override void Action()
    {
        WorldManager.instance.ToMinigame2DifficultySelection();
    }
}
