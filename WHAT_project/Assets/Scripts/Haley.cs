using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haley : MonoBehaviour
{

    public float moveSpeed = 0.2f;
    public float discoverRadius = 0.1f;
    private float timer;
    public int currentNode;
    private static Vector2 startPosition;
    public List<GameObject> nodes = new List<GameObject>();

    private void Start()
    {
        startPosition = nodes[1].transform.position;
        currentNode = 0;
    }

    // Update is called once per frame
    void Update()
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

}
