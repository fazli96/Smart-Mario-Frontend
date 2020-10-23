using System.Collections;
using UnityEngine;

/// <summary>
/// This class is for what happens when player rolls the dice
/// </summary>
public class RollTheDice : MonoBehaviour
{

    private Sprite[] diceSides;
    private SpriteRenderer rend;
    private bool coroutineAllowed = true;

    /// <summary>
    /// This is for initialization
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
        if (!StrandedGameManager.levelComplete && coroutineAllowed 
            && !StrandedGameManager.GetMoveAllowed() && !StrandedGameManager.qnEncountered)
            StartCoroutine("RollDice");
            //GetComponent<AudioSource>().Play();
    }
    /// <summary>
    /// A coroutine for rolling the dice. 
    /// Waits for the dice to finish rolling before the player moves
    /// </summary>
    /// <returns>wait for seconds</returns>
    private IEnumerator RollDice()
    {
        coroutineAllowed = false;
        int randomDiceSide = 0;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            rend.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.1f);

        //StrandedGameManager.diceSideThrown = randomDiceSide + 1;
        // for testing
        StrandedGameManager.diceSideThrown = 30;
        //randomDiceSide = 5 for six;

        StrandedGameManager.MovePlayer();

        coroutineAllowed = true;

    }
}
