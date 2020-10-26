using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is to manage all the scenes in the game
/// </summary>
public class SceneController : MonoBehaviour
{
    private static SceneController instance = null;

    /// <summary>
    /// This is to implement the singleton class principle
    /// This creates a gameObject that contains an instance of this class
    /// </summary>
    /// <returns></returns>
    public static SceneController GetSceneController()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<SceneController>();
        }
        return instance;
    }

    /// <summary>
    /// This is to load the start menu and display it on the screen
    /// </summary>
    public void ToStartMenu()
    {
        SceneManager.LoadScene("Start");
    }

    /// <summary>
    /// This is to load World 1 map and display it on the screen
    /// </summary>
    public void PlayWorld1()
    {
        SceneManager.LoadSceneAsync("World1");
    }
    /// <summary>
    /// This is to load World 2 map and display it on the screen
    /// </summary>
    public void PlayWorld2()
    {
        SceneManager.LoadSceneAsync("World2");
    }

    /// <summary>
    /// This is to load World Selection page and display it on the screen
    /// </summary>
    public void ToWorldSelection()
    {
        SceneManager.LoadScene("WorldSelection");
    }

    /// <summary>
    /// This is to load Customize Character page and display it on the screen
    /// </summary>
    public void ToCustomizeCharacter()
    {
        SceneManager.LoadScene("CustomizeCharacter");
    }

    /// <summary>
    /// This is to load Manage Tasks page and display it on the screen
    /// </summary>
    public void ToStudentManageTasks()
    {
        SceneManager.LoadScene("StudentManageTasks");
    }

    /// <summary>
    /// This is to load the main menu and display it on the screen
    /// </summary>
    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    /// <summary>
    /// This is to load the statistics page and display it on the screen
    /// </summary>
    public void ToStatistics()
    {
        SceneManager.LoadScene("Statistics");
    }

    /// <summary>
    /// This is to load the leaderboard page and display it on the screen
    /// </summary>
    public void ToLeaderboard()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    /// <summary>
    /// This is to load register page and display it on the screen
    /// </summary>
    public void ToCreateAccount()
    {
        SceneManager.LoadScene("CreateAccount");
    }

    /// <summary>
    /// This is to load the login page and display it on the screen
    /// </summary>
    public void ToLogin()
    {
        SceneManager.LoadScene("Login");
    }

    /// <summary>
    /// This is to quit the game
    /// </summary>
    public void ToQuit()
    {
        Application.Quit();
    }

    /// <summary>
    /// This is to load the difficulty selection page of selected minigame and display it on the screen
    /// </summary>
    public void ToDifficultySelection()
    {
        SceneManager.LoadScene("SelectDifficulty");
    }
    /// <summary>
    /// This is to load the level selection page of selected minigame and display it on the screen
    /// </summary>
    public void ToLevelSelection()
    {
        SceneManager.LoadScene("SelectLevel");
    }

    #region Teachers

    /// <summary>
    /// This is to load the Teacher's Main Menu
    /// </summary>
    public void ToTeacherMenu()
    {
        SceneManager.LoadScene("TeacherMenu");
    }

    /// <summary>
    /// This is to load the student selection page to view their performance
    /// </summary>
    public void ToSelectStudentPerformance()
    {
        SceneManager.LoadScene("SelectStudentPerformance");
    }

    /// <summary>
    /// This is to load the screen for selecting the task to view for the teacher
    /// </summary>
    public void ToTeacherSelectTaskScreen()
    {
        SceneManager.LoadScene("TeacherSelectTask");
    }

    /// <summary>
    /// This is to load the page for the Teacher to assign task(s) to the students
    /// </summary>
    public void ToAssignTasksScreen()
    {
        SceneManager.LoadScene("TeacherAssignTasks");
    }

    /// <summary>
    /// This is to load the page for all the tasks the teacher has assigned to the students
    /// </summary>
    public void ToViewAssignedTasksScreen()
    {
        SceneManager.LoadScene("ViewAssignedTasks");
    }

    #endregion

    #region Minigame 1 Levels

    /// <summary>
    /// This is to load World 1 Stranded game and display it on the screen
    /// </summary>
    public void ToWorld1Stranded()
    {
        SceneManager.LoadScene("World1Stranded");
    }

    /// <summary>
    /// This is to load World 2 Stranded game and display it on the screen
    /// </summary>
    public void ToWorld2Stranded()
    {
        SceneManager.LoadScene("World2Stranded");
    }

    #endregion

    #region Minigame 2 Levels

    /// <summary>
    /// This is to load World 1 Matching Cards Level 1 game and display it on the screen
    /// </summary>
    public void ToWorld1Minigame2()
    {
        SceneManager.LoadScene("World1_Minigame2");
    }
    /// <summary>
    /// This is to load World 2 Stranded Level 1 game and display it on the screen
    /// </summary>
    public void ToWorld2Minigame2()
    {
        SceneManager.LoadScene("World2_Minigame2");
    }

    #endregion

    #region Multiplayer

    /// <summary>
    /// This is to load the multiplayer lobby page and display it on the screen
    /// </summary>
    public void ToMultiplayerLobby()
    {
        SceneManager.LoadScene("MultiplayerLobby");
    }

    /// <summary>
    /// This is to load the multiplayer challenge room and display it on the screen
    /// </summary>
    public void ToMultiplayerRoom()
    {
        SceneManager.LoadScene("MultiplayerRoom");
    }

    /// <summary>
    /// This is to load the multiplayer World 1 Stranded minigame and display it on the screen
    /// </summary>
    public void ToWorld1StrandedMultiplayer()
    {
        SceneManager.LoadScene("World1StrandedMultiplayer");
    }

    /// <summary>
    /// This is to load the multiplayer World 1 Stranded minigame and display it on the screen
    /// </summary>
    public void ToWorld1MatchingMultiplayer()
    {
        SceneManager.LoadScene("World1MatchingMultiplayer");
    }

    /// <summary>
    /// This is to load the multiplayer World 2 Stranded minigame and display it on the screen
    /// </summary>
    public void ToWorld2StrandedMultiplayer()
    {
        SceneManager.LoadScene("World2StrandedMultiplayer");
    }
    #endregion
}
