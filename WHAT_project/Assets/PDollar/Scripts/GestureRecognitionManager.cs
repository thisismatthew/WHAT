using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using PDollarGestureRecognizer;


public class GestureRecognitionManager : MonoBehaviour {
	private Camera cam;
	public Transform gestureOnScreenPrefab;
	public int DrawAreaWidth, DrawAreaHeight, DrawAreaX, DrawAreaY;



	private List<Gesture> trainingSet = new List<Gesture>();

	private List<Point> points = new List<Point>();
	private int strokeId = -1;

	private Vector3 virtualKeyPosition = Vector2.zero;
	public DrawAreaDetector drawArea;

	private RuntimePlatform platform;
	private int vertexCount = 0;

	private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
	private LineRenderer currentGestureLineRenderer;

	//GUI
	private string message;
	private bool recognized;
	public InputField GestureNameInput, GestureNameTest;

	void Start () {
		cam = Camera.main;
		platform = Application.platform;		
		//Load pre-made gestures
		TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
		foreach (TextAsset gestureXml in gesturesXml)
        {
			Debug.Log("added");
			trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
		}
		//Load user custom gestures
		string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
		foreach (string filePath in filePaths)
        {
			trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));
			Debug.Log(filePath);
		}
	}

	void Update () {

		if (Input.GetMouseButton(0))
		{
			virtualKeyPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		}


		if (drawArea.PosInArea(cam.ScreenToWorldPoint(virtualKeyPosition))) {

			if (Input.GetMouseButtonDown(0)) {

				if (recognized) {

					recognized = false;
					strokeId = -1;

					points.Clear();

					foreach (LineRenderer lineRenderer in gestureLinesRenderer) {

						lineRenderer.SetVertexCount(0);
						Destroy(lineRenderer.gameObject);
					}

					gestureLinesRenderer.Clear();
				}

				++strokeId;
				
				Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
				currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();
				
				gestureLinesRenderer.Add(currentGestureLineRenderer);
				
				vertexCount = 0;
			}
			
			if (Input.GetMouseButton(0) && currentGestureLineRenderer != null) {
				points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

				currentGestureLineRenderer.SetVertexCount(++vertexCount);
				currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
			}
		}
	}

	public void TestRecognise()
    {
		Recognize(GestureNameTest.text);
    }

	public bool Recognize(string expected)
    {
		recognized = true;

		Gesture candidate = new Gesture(points.ToArray());
		Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

		message = gestureResult.GestureClass + " " + gestureResult.Score;
		Debug.Log(message);

		//clear off the gesture prefabs
		GameObject[] lines = GameObject.FindGameObjectsWithTag("Gesture");
		for (int i = 0; i < lines.Length; i++)
			Destroy(lines[i]);


		if (gestureResult.GestureClass == expected)
        {
			Debug.Log("PASS");
			return true;
        }
		if (gestureResult.GestureClass != expected)
        {
			Debug.Log("FAIL");
			return false;
        }

		Debug.Log("ERROR: Gesture did not return true or false.");
		return false;

		
	}

	

	public void AddNew()
    {

		if (points.Count > 0)
        {
			
			string _name = GestureNameInput.text;
            string fileName = String.Format("{0}/{1}-{2}.xml", Application.persistentDataPath, _name, DateTime.Now.ToFileTime());

#if !UNITY_WEBPLAYER
			GestureIO.WriteGesture(points.ToArray(), _name, fileName);
#endif

			trainingSet.Add(new Gesture(points.ToArray(), _name));

			Debug.Log("adding " + _name + " to gesture list");
		}

	}


}
