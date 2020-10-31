using System.Collections;
using UnityEngine;

/// <summary>
/// This class is for what happens when player rolls the dice
/// </summary>
public class RollTheDiceMultiplayer : MonoBehaviour
{

    private Sprite[] diceSides;
    private SpriteRenderer rend;
    private bool coroutineAllowed = true;

    /// <summary>
    /// This is for retrieving the sprites for each dice side on start
    /// </summary>
    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
        rend.sprite = diceSides[5];
    }
    /// <summary>
    /// Listens for mouse click down on the dice
    /// </summary>
    private void OnMouseDown()
    {
        // allow click on dice only when the game is not complete, player is no longer moving from tile to tile,
        // player is not encountering a question and it is the player's current turn
        if (!StrandedMultiplayerGameManager.levelComplete && coroutineAllowed
            && !StrandedMultiplayerGameManager.GetMoveAllowed() && !StrandedMultiplayerGameManager.qnEncountered
            && StrandedMultiplayerGameManager.currentTurn)
        {
            StartCoroutine("RollDice");
            GetComponent<AudioSource>().Play();
        }       
    }
    /// <summary>
    /// A coroutine for rolling the dice. 
    /// Waits for the dice to finish rolling before the player moves
    /// </summary>
    /// <returns>wait for seconds</returns>
    private IEnumerator RollDice()
    {
        coroutineAllowed = false;

        // imitates the animation of dice rolling
        int randomDiceSide = 0;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            rend.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.01f);
        }

        // alert Network Manager that the player is rolling the dice
        NetworkManager.instance.CommandRollDice(randomDiceSide + 1);
        yield return new WaitForSeconds(0.1f);

        StrandedMultiplayerGameManager.diceSideThrown = randomDiceSide + 1;
        //StrandedMultiplayerGameManager.diceSideThrown = 39;

        // once dice is rolled, allow player to move from tile to tile based on dice number rolled
        StrandedMultiplayerGameManager.MovePlayer();

        coroutineAllowed = true;

    }
}
