using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Manager that connects to Unity Login Scene UI objects and triggers function calls on events
/// </summary>
public class LoginManager : MonoBehaviour
{
    //Singleton
    public static LoginManager instance = null;
    private string url = "https://smart-mario-backend-1.herokuapp.com/api/";
    private SceneController scene;
    public GameObject loginButton;
    public GameObject cancelButton;
    public GameObject loadingText;
    public InputField usernameInputField;
    public InputField passwordInputField;
    public Toggle teacherToggle;
    public Text msg;
    
    /// <summary>
    /// Get instances of SceneController once LoginScreen starts
    /// </summary>
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        scene = SceneController.GetSceneController();
        loadingText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Changes scene to Start Menu
    /// </summary>
    public void ReturnToStartMenu()
    {
        scene.ToStartMenu();
    }
    /// <summary>
    /// Takes username and password details and sends them for Teacher/Student validation
    /// </summary>
    public void CheckInput()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (teacherToggle.isOn)
        {
            ValidateTeacherLogin(username, password, msg);
        }
        else
        {
           ValidateStudentLogin(username, password, msg);
        }
        loginButton.SetActive(false);
        cancelButton.SetActive(false);
        loadingText.SetActive(true);
    }
    /// <summary>
    /// Displays error message on screen for failed login attempts
    /// </summary>
    /// <param name="str"></param>
    /// <param name="message"></param>
    public void DisplayMessage(string str)
    {
        msg.text = str;
        loginButton.SetActive(true);
        cancelButton.SetActive(true);
        loadingText.SetActive(false);
    }

    /// <summary>
    /// Changes scene to Main Menu
    /// </summary>
    public void StudentLoginSuccess()
    {
        scene = SceneController.GetSceneController();
        scene.ToMainMenu();
    }
    /// <summary>
    /// Changes scene to Teacher Menu
    /// </summary>
    public void TeacherLoginSuccess()
    {
        scene = SceneController.GetSceneController();
        scene.ToTeacherMenu();
    }

    /// <summary>
    /// Check the Student login details are valid
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="msg"></param>
    public void ValidateStudentLogin(string username, string password, Text msg)
    {
        APICall apiCall = APICall.getAPICall();
        Student student = new Student(username, "", password, "");
        string bodyJsonString = apiCall.saveToJSONString(student);
        StartCoroutine(apiCall.StudentLoginPostRequest(url + "students/authenticate", bodyJsonString));
    }

    /// <summary>
    /// Check the Teacher login details are valid
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="msg"></param>
    public void ValidateTeacherLogin(string username, string password, Text msg)
    {
        APICall apiCall = APICall.getAPICall();
        Teacher teacher = new Teacher(username, "", password, "");
        string bodyJsonString = apiCall.saveToJSONString(teacher);
        StartCoroutine(apiCall.TeacherLoginPostRequest(url + "teachers/authenticate", bodyJsonString));
    }

    /// <summary>
    /// Get the Questions from the database
    /// </summary>
    public void GetQuestions()
    {
        APICall apiCall = APICall.getAPICall();
        StartCoroutine(apiCall.QuestionsListGetRequest("mcqtheory"));
    }

    /// <summary>
    /// This method is to store the Mcq Theory questions once they are retrieved from the database
    /// </summary>
    /// <param name="convertedStr"></param>
    public void McqTheoryQuestionsRetrieved(string convertedStr)
    {
        QuestionList.ClearMcqTheoryQuestionListEasy();
        QuestionList.ClearMcqTheoryQuestionListMedium();
        QuestionList.ClearMcqTheoryQuestionListHard();

        var data = (JObject)JsonConvert.DeserializeObject(convertedStr);
        JArray data2 = data["Question"].Value<JArray>();
        //int counter = 0;
        foreach (JObject questionObject in data2)
        {
            Question question = questionObject.ToObject<Question>();
            if (question.difficulty == 1)
                QuestionList.AddMcqTheoryQuestionEasy(question);
            else if (question.difficulty == 2)
                QuestionList.AddMcqTheoryQuestionMedium(question);
            else
                QuestionList.AddMcqTheoryQuestionHard(question);
        }

        APICall apiCall = APICall.getAPICall();
        StartCoroutine(apiCall.QuestionsListGetRequest("mcqcode"));
    }

    /// <summary>
    /// This method is to store the Mcq Code questions once they are retrieved from the database
    /// </summary>
    /// <param name="convertedStr"></param>
    public void McqCodeQuestionsRetrieved(string convertedStr)
    {
        QuestionList.ClearMcqCodeQuestionListEasy();
        QuestionList.ClearMcqCodeQuestionListMedium();
        QuestionList.ClearMcqCodeQuestionListHard();

        var data = (JObject)JsonConvert.DeserializeObject(convertedStr);
        JArray data2 = data["Question"].Value<JArray>();
        //int counter = 0;
        foreach (JObject questionObject in data2)
        {
            Question question = questionObject.ToObject<Question>();
            if (question.difficulty == 1)
                QuestionList.AddMcqCodeQuestionEasy(question);
            else if (question.difficulty == 2)
                QuestionList.AddMcqCodeQuestionMedium(question);
            else
                QuestionList.AddMcqCodeQuestionHard(question);
        }

        APICall apiCall = APICall.getAPICall();
        StartCoroutine(apiCall.QuestionsListGetRequest("shorttheory"));
    }

    /// <summary>
    /// This method is to store the Short Theory questions once they are retrieved from the database
    /// </summary>
    /// <param name="convertedStr"></param>
    public void ShortTheoryQuestionsRetrieved(string convertedStr)
    {
        QuestionList.ClearShortTheoryQuestionListEasy();
        QuestionList.ClearShortTheoryQuestionListMedium();
        QuestionList.ClearShortTheoryQuestionListHard();

        var data = (JObject)JsonConvert.DeserializeObject(convertedStr);
        JArray data2 = data["Question"].Value<JArray>();
        //int counter = 0;
        foreach (JObject questionObject in data2)
        {
            Question question = questionObject.ToObject<Question>();
            if (question.difficulty == 1)
                QuestionList.AddShortTheoryQuestionEasy(question);
            else if (question.difficulty == 2)
                QuestionList.AddShortTheoryQuestionMedium(question);
            else
                QuestionList.AddShortTheoryQuestionHard(question);
        }

        APICall apiCall = APICall.getAPICall();
        StartCoroutine(apiCall.QuestionsListGetRequest("shortcode"));
    }

    /// <summary>
    /// This method is to store the Short Code questions once they are retrieved from the database
    /// </summary>
    /// <param name="convertedStr"></param>
    public void ShortCodeQuestionsRetrieved(string convertedStr)
    {
        QuestionList.ClearShortCodeQuestionListEasy();
        QuestionList.ClearShortCodeQuestionListMedium();
        QuestionList.ClearShortCodeQuestionListHard();

        var data = (JObject)JsonConvert.DeserializeObject(convertedStr);
        JArray data2 = data["Question"].Value<JArray>();
        //int counter = 0;
        foreach (JObject questionObject in data2)
        {
            Question question = questionObject.ToObject<Question>();
            if (question.difficulty == 1)
                QuestionList.AddShortCodeQuestionEasy(question);
            else if (question.difficulty == 2)
                QuestionList.AddShortCodeQuestionMedium(question);
            else
                QuestionList.AddShortCodeQuestionHard(question);
        }

        StudentLoginSuccess();
    }
}
