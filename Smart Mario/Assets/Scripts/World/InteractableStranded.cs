using UnityEngine;

/// <summary>
/// This class implements methods defined in InteractableObject interface for Stranded Interactable object
/// </summary>
public class InteractableStranded : InteractableObject
{
    /// <summary>
    /// This method is to set the display text on screen for Stranded minigame
    /// It overrides the abstract method defined in the interface
    /// </summary>
    /// <returns></returns>
    public override string DisplayText()
    {
        return "play Stranded Minigame";
    }

    /// <summary>
    /// This method is to allow player to navigate to difficulty selection screen for Stranded minigame 
    /// It overrides the abstract method defined in the interface
    /// </summary>
    /// <returns></returns>
    public override void Action()
    {
        WorldManager.instance.ToMinigame1DifficultySelection();
    }
}
