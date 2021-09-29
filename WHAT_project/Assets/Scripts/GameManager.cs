using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum GameState
{
    Start, 
    Work,
    Love,
    End
}



public class GameManager : MonoBehaviour
{
    public GameState State = GameState.Start;
    public GameObject HeartProjectile;
    public float ProjectileSpeed;
    public GameObject Dale, Hayley, WorkImages, WorkUI;
    public List<DrawTask> Tasks;
    public DrawTask currentGestureTarget;
    private GestureRecognitionManager _grm;
    public TextMeshProUGUI TaskTitleText, TaskDescriptionText, TaskRemainingText;
    public SpriteRenderer traceImage;
    public Slider uploadSlider;
    private Camera cam;
    public int probabilityOfRandomFail = 3;
    

    private void Start()
    {
        foreach(DrawTask t in Tasks)
            t.TaskFail = false;
        
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
            WorkImages.active = false;
            WorkUI.active = false;
            LoveUpdate();
        }
        if (State == GameState.Work)
        {
            WorkImages.active = true;
            WorkUI.active = true;
            WorkUpdate();
            
        }
        if (State == GameState.End)
        {
            Debug.Log("DONE");
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
            
            //lets have a small possibility that this gets flipped. 
            int random = Random.RandomRange(0, probabilityOfRandomFail);
            if (random == 0)
            {
                Debug.Log("Random Failure Triggered");
                resultSuccess = false;
                
            }
            else
            {
                Debug.Log("Success");
            }
        }
        if (!resultSuccess)
        {
            previousTask.TaskFail = true;
            Tasks.Add(previousTask);
        }
    }

    public void StartUpload()
    {
        uploadSlider.gameObject.GetComponent<Animator>().Play("UploadSlider");
    }

    public void WorkUpdate()
    {
        if (!currentGestureTarget.TaskFail)
        {
            TaskTitleText.text = currentGestureTarget.TaskName;
            TaskDescriptionText.text = currentGestureTarget.TaskDesc;
        }
        else
        {
            TaskTitleText.text = currentGestureTarget.taskFailName;
            TaskDescriptionText.text = currentGestureTarget.taskFailDesc;
        }
        
        traceImage.sprite = currentGestureTarget.traceImage;
        TaskRemainingText.text = "Remaining Tasks: " + Tasks.Count;

        if(Tasks.Count == 0)
        {
            State = GameState.End;
        }

    }

    
}
