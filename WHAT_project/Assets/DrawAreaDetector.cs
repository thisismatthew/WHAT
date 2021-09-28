using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawAreaDetector : MonoBehaviour
{
	public int Width, Height, X, Y;
	private LineRenderer border;
	private Vector2 bottomLeft, topLeft, bottomRight, topRight;
	private Camera cam;

	private void Start()
    {
		cam = Camera.main;
		border = GetComponent<LineRenderer>();
		border.SetVertexCount(5);
		border.sortingOrder = 1;
		border.material = new Material(Shader.Find("Sprites/Default"));
		border.material.color = Color.red;
		border.startWidth = 0.1f;
		border.endWidth = 0.1f;
		DrawBorders();
	}

    public void DrawBorders()
	{

		bottomLeft.x = X;
		bottomLeft.y = Y;
		topLeft.x = X;
		topLeft.y = Y + Height;
		bottomRight.x = X + Width;
		bottomRight.y = Y;
		topRight.x = X + Width;
		topRight.y = Y + Height;

        
		border.SetPosition(0, bottomLeft);
		border.SetPosition(1, topLeft);
		border.SetPosition(2, topRight);
		border.SetPosition(3, bottomRight);
		border.SetPosition(4, bottomLeft);
	}

    public bool PosInArea(Vector2 pos)
    {
		if (pos.x < X)
			return false;
		if (pos.x > X + Width)
			return false;
		if (pos.y < Y)
			return false;
		if (pos.y > Y + Height)
			return false;
		return true;
	}

    private void Update()
    {
		DrawBorders();
    }
}
