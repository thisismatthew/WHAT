using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HayleyStates
{
    Start, 
    Rampage, 
    Retreat,
    Dormant
}


public class Hayley : MonoBehaviour
{

    public float moveSpeed = 0.2f;
    public float discoverRadius = 0.1f;
    private float timer;
    public int currentNode;
    private static Vector2 HayleyStartPosition;
    private GameManager gm;
    private bool SliderEntered = false, SliderLeft = false;
    private Animator sliderAnim;

    public List<GameObject> nodes = new List<GameObject>();
    public float loved =5f;
    public Slider loveSlider;
    public Transform LeaveSpot, EnterSpot;
    public HayleyStates State;
    

    private void Start()
    {
        sliderAnim = loveSlider.gameObject.GetComponent<Animator>();
        gm = FindObjectOfType<GameManager>();
        HayleyStartPosition = nodes[1].transform.position;
        currentNode = 0;
    }

    // Update is called once per frame
    void Update()
    {
        loved -= Time.deltaTime;
        Mathf.Clamp(loved, 0, 110f);
        loveSlider.value = loved;

        if (State == HayleyStates.Start)
        {
            gm.State = GameState.Love;
            if (!SliderEntered)
            {
                SliderEntered = true;
                sliderAnim.Play("LoveSliderAnimation");
            }
            EnterScreen();
        }
        if (State == HayleyStates.Rampage)
        {

            MoveAlongNodes();
            if (loved >= 100f)
            {
                loved = 100f;
                State = HayleyStates.Retreat;
            }

        }
        if(State == HayleyStates.Retreat)
        {
            if (!SliderLeft)
            {
                SliderLeft = true;
                sliderAnim.Play("LoveLeaveAnimation");
            }
            LeaveScreen();
        }
        if(State == HayleyStates.Dormant)
        {
            SliderLeft = false;
            SliderEntered = false;
            if (loved <= 0)
            {
                loved = 0;
                State = HayleyStates.Start;
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Heart")
        {
            loved += 5;
            Mathf.Clamp(loved, 0, 100f);
            Destroy(collision.gameObject);
        }
    }

    private void MoveAlongNodes()
    {
        timer = Time.deltaTime * moveSpeed;
        //if the Haley is not close to the target
        if (Vector2.Distance(transform.position, nodes[currentNode].transform.position) > discoverRadius)
        {
            //lerp to it
            this.GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, nodes[currentNode].transform.position, timer));
        }
        else
        {
            timer = 0;
            if (currentNode == nodes.Count - 1)
            {
                nodes.Reverse();
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
    }


    private void LeaveScreen()
    {
        timer = Time.deltaTime * moveSpeed; 

        if (Vector2.Distance(transform.position, LeaveSpot.position) > discoverRadius)
        {
            //lerp to it
            this.GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, LeaveSpot.position, timer));
        }
        else
        {
            State = HayleyStates.Dormant;
            gm.State = GameState.Work;
        }
    }

    private void EnterScreen()
    {
        timer = Time.deltaTime * moveSpeed;
        
        if (Vector2.Distance(transform.position, EnterSpot.position) > discoverRadius)
        {
            //lerp to it
            Debug.Log("Lerping to start spot");
            this.GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, EnterSpot.position, timer));
        }
        else
        {
            State = HayleyStates.Rampage;
        }
        
    }
}
