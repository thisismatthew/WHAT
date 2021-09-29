using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum GameState
{
    Start, 
    Work,
    Love
}



public class GameManager : MonoBehaviour
{
    public GameState State = GameState.Start;
    public GameObject HeartProjectile;
    public float ProjectileSpeed;
    public GameObject Dale, Hayley;
    public List<DrawTask> Tasks;
    public DrawTask currentGestureTarget;
    private GestureRecognitionManager _grm;
    public TextMeshProUGUI TaskTitleText, TaskDescriptionText, TaskRemainingText;
    public SpriteRenderer traceImage;

    private Camera cam;

    private void Start()
    {
        State = GameState.Work;
        _grm = FindObjectOfType<GestureRecognitionManager>();
        currentGestureTarget = Tasks[0];
        cam = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        if (State == GameState.Start)
        {
            State = GameState.Love;
        }
        if (State == GameState.Love)
        {
            LoveUpdate();
        }
        if (State == GameState.Work)
        {
            WorkUpdate();
            //enable the work screen
        }
    }

    public void LoveUpdate()
    {
        //if the mouse gets pressed fire a heart at the mouse position
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("HeartTriggered");
            GameObject newHeart = Instantiate(HeartProjectile, Dale.transform);
            // find direction to click
            Vector2 dir = (cam.ScreenToWorldPoint(Input.mousePosition) - newHeart.transform.position).normalized;
           
            newHeart.GetComponent<Rigidbody2D>().velocity = new Vector2(dir.x *ProjectileSpeed, dir.y *ProjectileSpeed);
            
        }
        Debug.DrawLine(Dale.transform.position, cam.ScreenToWorldPoint(Input.mousePosition), Color.red);
    }

    public void SubmitTask()
    {
        //send the current gesture drawing to the request check using the current target drawing as the arg
        bool resultSuccess = _grm.Recognize(currentGestureTarget.GestureName);
        DrawTask previousTask = currentGestureTarget;
        Tasks.Remove(previousTask);
        currentGestureTarget = Tasks[0];

        if (resultSuccess)
        {
            Debug.Log("Success");
        }
        if (!resultSuccess)
        {
            previousTask.TaskName += " AGAIN";
            previousTask.TaskDesc = "I was really unhappy with how you drew the " + previousTask.GestureName + " last time... please redo it, and GET IT RIGHT!";
            Tasks.Add(previousTask);
        }
    }

    public void WorkUpdate()
    {
        TaskTitleText.text = currentGestureTarget.TaskName;
        TaskDescriptionText.text = currentGestureTarget.TaskDesc;
        traceImage.sprite = currentGestureTarget.traceImage;
        TaskRemainingText.text = "Remaining Tasks: " + Tasks.Count;

    }
}
