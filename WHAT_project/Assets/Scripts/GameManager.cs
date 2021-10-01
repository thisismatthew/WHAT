using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


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
    public GameObject ShootPos, Hayley, WorkImages, WorkUI, WinUI;
    public List<DrawTask> Tasks;
    public DrawTask currentGestureTarget;
    public GestureRecognitionManager _grm;
    public TextMeshProUGUI TaskTitleText, TaskDescriptionText, TaskRemainingText, SubjectText;
    public SpriteRenderer traceImage;
    public Slider uploadSlider;
    private Camera cam;
    public int probabilityOfRandomFail = 3;
    public List<GameObject> GestureLines;
    public SpriteRenderer daleSpriteRenderer;
    public List<Sprite> spriteState;
    private float turntimer = 1.5f;
    public Animator ScreenAnim;
    private bool ScreenOpened = false;

    private void Start()
    {
        GestureLines = new List<GameObject>();
        foreach (DrawTask t in Tasks)
            t.TaskFail = false;
        
        State = GameState.Work;
       
        currentGestureTarget = Tasks[0];
        cam = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        if (State == GameState.Start)
        {
            Hayley.GetComponent<Hayley>().loved += Time.deltaTime;
        }
        if (State == GameState.Love)
        {
            Cursor.visible = true;
            turntimer -= Time.deltaTime;
            if (turntimer < 0)
            {
                if (Input.GetMouseButton(0))
                {
                    daleSpriteRenderer.sprite = spriteState[2];
                }
                else
                {
                    daleSpriteRenderer.sprite = spriteState[1];
                }
               
            }
           
            GestureLines.Clear();
            foreach(GameObject line in GameObject.FindGameObjectsWithTag("Gesture"))
            {
                GestureLines.Add(line);
                line.GetComponent<LineRenderer>().enabled = false;
            }
            if (ScreenOpened)
            {
                FindObjectOfType<AudioManager>().Play("HayleyRampage");
                FindObjectOfType<AudioManager>().Stop("MainMusic");
                ScreenAnim.Play("Shrink");
                ScreenOpened = false;
                _grm.enabled = false;
            }
            LoveUpdate();
        }
        if (State == GameState.Work)
        {
            

            turntimer = 1.5f;
            daleSpriteRenderer.sprite = spriteState[0];
            if (GestureLines.Count > 0)
            {
                foreach (GameObject line in GestureLines)
                    line.GetComponent<LineRenderer>().enabled = true;
            }
            if (!ScreenOpened)
            {
                FindObjectOfType<AudioManager>().Stop("HayleyRampage");
                FindObjectOfType<AudioManager>().Play("MainMusic");
                ScreenAnim.Play("Expand");
                _grm.enabled = true;
                ScreenOpened = true;
            }
            WorkUpdate();
            
        }
        if (State == GameState.End)
        {
            FindObjectOfType<AudioManager>().StopAll();
            FindObjectOfType<AudioManager>().Play("HappyBirthday");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }

    public void LoveUpdate()
    {
        
        //if the mouse gets pressed fire a heart at the mouse position
        if (Input.GetMouseButtonDown(0))
        {
            FindObjectOfType<AudioManager>().Play("Kiss");
            Debug.Log("HeartTriggered");
            GameObject newHeart = Instantiate(HeartProjectile, ShootPos.transform);
            // find direction to click
            Vector2 dir = (cam.ScreenToWorldPoint(Input.mousePosition) - newHeart.transform.position).normalized;
           
            newHeart.GetComponent<Rigidbody2D>().velocity = new Vector2(dir.x *ProjectileSpeed, dir.y *ProjectileSpeed);
            
        }
        Debug.DrawLine(ShootPos.transform.position, cam.ScreenToWorldPoint(Input.mousePosition), Color.red);
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
            //lets have a small possibility that this gets flipped. 
            /*int random = Random.RandomRange(0, probabilityOfRandomFail);
            if (random == 0)
            {
                Debug.Log("Random Failure Triggered");
                resultSuccess = false;

            }
            else
            {

            }*/
        }
        if (!resultSuccess)
        {
            Debug.Log("Fail");
            previousTask.TaskFail = true;
            Tasks.Add(previousTask);
        }
    }

    public void StartUpload()
    {
        GestureLines.Clear();
        uploadSlider.gameObject.GetComponent<Animator>().Play("UploadSlider");
    }

    public void WorkUpdate()
    {
        SubjectText.text = currentGestureTarget.Sender;
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
        TaskRemainingText.text = Tasks.Count.ToString();

        if(Tasks.Count == 0)
        {
            State = GameState.End;
        }

    }

    public void StartGame()
    {
        State = GameState.Work;
    }

   
}
