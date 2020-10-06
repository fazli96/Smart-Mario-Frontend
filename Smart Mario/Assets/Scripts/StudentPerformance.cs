using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentPerformance : MonoBehaviour
{
    private SceneController scene;
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void World1Game1()
    {
        #DisplayStatistics(1, 1)
    }

    public void World1Game2()
    {
        #DisplayStatistics(1, 2)
    }
    
    public void World2Game1()
    {
        #DisplayStatistics(2, 1)
    }
    
    public void World2Game2()
    {
        #DisplayStatistics(2, 2)
    }

    public void exportCSV()
    {
        # lol wtf?
    }

    public void BackToTeacherMenu()
    {
        scene = SceneController.ToTeacherMenu();
    }
}
