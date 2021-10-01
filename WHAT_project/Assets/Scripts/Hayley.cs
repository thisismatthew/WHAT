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
    public float invisibleDistance = 1f;
    private float leaveRadius = 1.3f;
    private float timer;
    public int currentNode;
    private static Vector2 HayleyStartPosition;
    private GameManager gm;
    private bool SliderEntered = false, SliderLeft = false, ThreatMade = false;
    private Animator sliderAnim;
    public Animator HayleyAnim;
    public List<SpriteRenderer> BodySprites;
    private CamerShake shaker;
    

    public List<GameObject> nodes = new List<GameObject>();
    public float loved =5f;
    public Slider loveSlider;
    public Transform LeaveSpot, EnterSpot;
    public HayleyStates State;
    

    private void Start()
    {
        shaker = FindObjectOfType<CamerShake>();
        sliderAnim = loveSlider.gameObject.GetComponent<Animator>();
        gm = FindObjectOfType<GameManager>();
        HayleyStartPosition = nodes[1].transform.position;
        currentNode = 0;
    }

    // Update is called once per frame
    void Update()
    {

        loveSlider.value = loved;

        if (State == HayleyStates.Start)
        {
            HayleyAnim.SetBool("Rampage", true);
            gm.State = GameState.Love;
            if (!SliderEntered)
            {
                FindObjectOfType<AudioManager>().Play("What");
                SliderEntered = true;
                sliderAnim.Play("LoveSliderAnimation");
            }
            EnterScreen();
        }
        if (State == HayleyStates.Rampage)
        {
            HayleyAnim.SetBool("Rampage", true);
            
            MoveAlongNodes();
            if (loved >= 30f)
            {
                FindObjectOfType<AudioManager>().Play("Thank");
                loved = 30f;
                State = HayleyStates.Retreat;
            }

        }
        if(State == HayleyStates.Retreat)
        {
            ThreatMade = false;
            HayleyAnim.SetBool("Rampage", false);
            if (!SliderLeft)
            {
                FindObjectOfType<AudioManager>().Stop("What");
                SliderLeft = true;
                sliderAnim.Play("LoveLeaveAnimation");
            }
            LeaveScreen();
        }
        if(State == HayleyStates.Dormant)
        {
            loved -= Time.deltaTime;
            Mathf.Clamp(loved, 0, 40f);

            if (loved < 7f && !ThreatMade)
            {
                FindObjectOfType<AudioManager>().Play("Yell");
                shaker.shake = 7;
                ThreatMade = true;
            }

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
            loved += 1f;
            Mathf.Clamp(loved, 0, 50f);
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
        
        if (Vector2.Distance(transform.position, LeaveSpot.position) > invisibleDistance)
        {
            foreach(SpriteRenderer sr in BodySprites)
            {
                Color newColor = sr.color;
                newColor.a = Mathf.Lerp(newColor.a, 0, Time.deltaTime);

                if (sr.color.a <= 0.5)
                    newColor.a = 0f;

                sr.color = newColor;
            }
        }

        if (Vector2.Distance(transform.position, LeaveSpot.position) > discoverRadius)
        {
            //lerp to it
            this.GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, LeaveSpot.position, timer));
        }
        else
        {
            State = HayleyStates.Dormant;
        }
        if (Vector2.Distance(transform.position, LeaveSpot.position) < leaveRadius)
        {
            //lerp to it
            gm.State = GameState.Work;
        }
       
    }

    private void EnterScreen()
    {
        timer = Time.deltaTime * moveSpeed;
        
        if (Vector2.Distance(transform.position, EnterSpot.position) > invisibleDistance)
        {
            foreach (SpriteRenderer sr in BodySprites)
            {
                Color newColor = sr.color;
                newColor.a = Mathf.Lerp(newColor.a, 1, Time.deltaTime);
                if (sr.color.a >= 0.5f)
                    newColor.a = 1f;

                sr.color = newColor;
            }
            
        }

        if (Vector2.Distance(transform.position, EnterSpot.position) > discoverRadius)
        {
            //lerp to it
            //Debug.Log("Lerping to start spot");
            this.GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, EnterSpot.position, timer));
        }
        else
        {
            State = HayleyStates.Rampage;
        }
        
    }
}
