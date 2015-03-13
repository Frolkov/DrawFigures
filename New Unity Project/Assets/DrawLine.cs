using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class DrawLine : MonoBehaviour  
{    
	public LineRenderer line;
	private bool isMousePressed;    
	private List<Vector3> pointsList;    
	private Vector3 mousePos;
	private bool StartGame;
	public Button StartButton,RestartButton;
	public Text	NameOfFig,CurrLevel,TimeText;
	public CanvasGroup G1,G2;
	private int kindOfFig;//0 - треугольник; 1 - круг
	private int level;
	private float timecurr,timeofstart;




	  
	struct myLine
	{
		public Vector3 StartPoint;         
		public Vector3 EndPoint;    
	};    
	//    -----------------------------------        

	void Start()    
	{        
		/*line = gameObject.AddComponent<LineRenderer>();  
		line.material =  new Material(Shader.Find("Particles/Additive"));   
		line.SetVertexCount(0);        
		line.SetWidth(0.1f,0.1f);         
		line.SetColors(Color.green, Color.green);        
		line.useWorldSpace = true; 
		*/
		isMousePressed = false;        
		pointsList = new List<Vector3>();
		StartGame = false;
		kindOfFig = 0;
		level = 1;



	





	}    
	//    -----------------------------------        
	void Update ()      
	{        
	
			if ((StartGame)&&(timecurr>0)) {
			timecurr = 10-(Time.time - timeofstart);
			TimeText.text = timecurr.ToString("0.00");
			if (kindOfFig==0)
			{
				NameOfFig.text="Triangle";	
			}
			else
			{
				NameOfFig.text="Circle";
			}

			if (Input.GetMouseButtonDown (0)) {            
				isMousePressed = true;            
				line.SetVertexCount (0);             
				pointsList.RemoveRange (0, pointsList.Count);             
				line.SetColors (Color.green, Color.green);        
			} else if (Input.GetMouseButtonUp (0)) {            
				isMousePressed = false;        
			}        
			      
			if (isMousePressed) {            
				mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);             
				mousePos.z = 0;            
				if (!pointsList.Contains (mousePos)) {                
					pointsList.Add (mousePos);                 
					line.SetVertexCount (pointsList.Count);                 
					line.SetPosition (pointsList.Count - 1, (Vector3)pointsList [pointsList.Count - 1]);                 
				}
			if (((kindOfFig==0)&&(isTriangle())) || ((kindOfFig==1)&&(isCircle())))
				    {
				level++;
				CurrLevel.text="Level:"+level;
				if (kindOfFig==0)
					{
						kindOfFig=1;
					}
					else
					{
						kindOfFig=0;
					}
				}
						
			}
		}
		if (timecurr < 0) {
			RestartButton.active=true;
			StartGame=false;
		}
		

	}    
	    
	public void ButtonStartClick()
	{
		StartGame = true;
		StartButton.active = false;
		NameOfFig.text="Triangle";
		G1.alpha = 1;
		G2.alpha = 1;
		TimeText.text = "60";
		timecurr = 60.0f;
		timeofstart = Time.time;

	}
	public void ButtonRestartClick()
	{
		level = 1;
		timecurr = 60.0f;
		StartGame = true;
		RestartButton.active = false;
		timeofstart = Time.time;
		CurrLevel.text="Level:"+level;
	}
	private bool isLineCollide()    
	{        
		if (pointsList.Count < 2)            
			return false;        
		int TotalLines = pointsList.Count - 1;        
		myLine[] lines = new myLine[TotalLines];        
		if (TotalLines > 1)          
		{            
			for (int i=0; i<TotalLines; i++)              
			{                
				lines [i].StartPoint = (Vector3)pointsList [i];                 
				lines [i].EndPoint = (Vector3)pointsList [i + 1];             }        
		}        
		for (int i=0; i<TotalLines-1; i++)          
		{            
			myLine currentLine;            
			currentLine.StartPoint = (Vector3)pointsList [pointsList.Count - 2];            
			currentLine.EndPoint = (Vector3)pointsList [pointsList.Count - 1];            
			if (isLinesIntersect (lines [i], currentLine))                  return true;        
		}        
		return false;    
	}    
	       

	     
	private bool checkPoints (Vector3 pointA, Vector3 pointB)     {        
		return (pointA.x == pointB.x && pointA.y == pointB.y);     }    

	private bool isLinesIntersect (myLine L1, myLine L2)    
	{        
		if (checkPoints (L1.StartPoint, L2.StartPoint) ||  checkPoints (L1.StartPoint, L2.EndPoint) || checkPoints (L1.EndPoint, L2.StartPoint) || checkPoints (L1.EndPoint, L2.EndPoint))            
			return false;        
		return((Mathf.Max (L1.StartPoint.x, L1.EndPoint.x) >= Mathf.Min (L2.StartPoint.x, L2.EndPoint.x)) &&(Mathf.Max (L2.StartPoint.x, L2.EndPoint.x) >= Mathf.Min (L1.StartPoint.x, L1.EndPoint.x)) && (Mathf.Max (L1.StartPoint.y, L1.EndPoint.y) >= Mathf.Min (L2.StartPoint.y, L2.EndPoint.y)) &&(Mathf.Max (L2.StartPoint.y, L2.EndPoint.y) >= Mathf.Min (L1.StartPoint.y, L1.EndPoint.y)));    
	}

	private bool isTriangle()
	{

		Vector3 top = pointsList [0];
		Vector3 left = pointsList [0];
		Vector3 right = pointsList [0];
		float Xmax=pointsList[0].x;
		float Ymax=pointsList[0].y;
		float Xmin=pointsList[0].x;
		float Ymin=pointsList[0].y;
		List<float> distancesLeft=new List<float>();
		List<float> distancesBott=new List<float>();
		List<float> distancesRight=new List<float>();
		float MaxDLeft = 0;
		float MaxDBott = 0;
		float MaxDRight = 0;
		bool WasLeft=false;
		bool WasRight=false;
		bool res = false;



		for (int i=0; i<pointsList.Count; i++) 
		{
			if (pointsList [i].x > Xmax) {
				Xmax = pointsList [i].x;
				right=pointsList [i];
			}
			if (pointsList [i].y > Ymax) {
				Ymax = pointsList [i].y;
				top=pointsList [i];
			}
			if (pointsList [i].x < Xmin) {
				Xmin = pointsList [i].x;
				left=pointsList [i];
			}
			if (pointsList [i].y < Ymin) {
				Ymin = pointsList [i].y;
			}
		}
		if (isLineCollide ()) 
		{

			for (int i=0; i<pointsList.Count;i++)
			{
				if (pointsList[i].x==Xmin)
				{
					WasLeft=true;
				}
				if (pointsList[i].x==Xmax)
				{
					WasRight=true;
				}
				if (!WasLeft)
				{
					distancesLeft.Add(D (top,left,pointsList[i]));
				}
				else
				{
					if (!WasRight)
					{
						distancesBott.Add (D (left,right,pointsList[i]));
					}
					else
							{
								distancesRight.Add (D (right,top,pointsList[i]));
							}
				}
			}
			MaxDLeft=Max(distancesLeft);
			MaxDRight=Max(distancesRight);
			MaxDBott=Max(distancesBott);
		
			if((MaxDBott!=0)&&(MaxDLeft!=0)&&(MaxDRight!=0))
			{
				if ((MaxDBott<=0.5f)&&(MaxDLeft<=0.5f)&&(MaxDRight<=0.7f))
				{
					line.SetColors(Color.yellow, Color.yellow);
					res=true;
				}
				else
				{
					line.SetColors(Color.red, Color.red);
					res= false;
				}
			}
			else
			{
				line.SetColors(Color.red, Color.red);
				res= false;
			}

			isMousePressed = false;


		}
		return res;
	
	
	}
	private float D(Vector3 t0, Vector3 t1, Vector3 t2)
	{
					float A=t1.y-t0.y;
					float B=t0.x-t1.x;
					float C = t0.x * (-A) - t0.y * (B);
					return Mathf.Abs(A*t2.x+B*t2.y+C)/Mathf.Sqrt(Mathf.Pow(A,2)+Mathf.Pow(B,2));			
	}
	private float D(Vector3 t1,Vector3 t2)
	{
		return Mathf.Sqrt(Mathf.Pow(t2.x-t1.x,2)+Mathf.Pow (t2.y-t1.y,2));
	}

	private float Max(List<float> a)
	{
				float M=0;
				for (int i=0;i<a.Count;i++)
				{
					if (a[i]>M)
					{
						M=a[i];
					}
				}
				return M;
	}
	private Vector3 Center(Vector3 top,Vector3 bott,Vector3 left,Vector3 right)
	{
		float A1 = top.y - bott.y;
		float B1 = bott.x - top.x;
		float C1 = bott.x * (bott.y - top.y) - bott.y * (bott.x - top.x);
		float A2 = left.y - right.y;
		float B2 = right.x - left.x;
		float C2 = right.x * (right.y - left.y) - right.y * (right.x - left.x);
		Vector3 P;
		P.y = (A2 * C1 - C2 * A1) / (B2 * A1 - A2 * B1);
		P.x = (-C1 - B1 * P.y) / A1;
		P.z = 0;
		return P;

	}

	private bool isCircle()
	{
		Vector3 top = pointsList [0];
		Vector3 left = pointsList [0];
		Vector3 right = pointsList [0];
		Vector3 bott = pointsList [0];
		Vector3 CC = pointsList [0];
		float Xmax = pointsList [0].x;
		float Ymax = pointsList [0].y;
		float Xmin = pointsList [0].x;
		float Ymin = pointsList [0].y;
		int z = 0;
		bool res = false;

		
		
		
		for (int i=0; i<pointsList.Count; i++) {
			if (pointsList [i].x > Xmax) {
				Xmax = pointsList [i].x;
				right = pointsList [i];
			}
			if (pointsList [i].y > Ymax) {
				Ymax = pointsList [i].y;
				top = pointsList [i];
			}
			if (pointsList [i].x < Xmin) {
				Xmin = pointsList [i].x;
				left = pointsList [i];
			}
			if (pointsList [i].y < Ymin) {
				Ymin = pointsList [i].y;
				bott = pointsList [i];
			}
		}


		if (isLineCollide ()) {
			CC = Center (top, bott, left, right);
			for (int i=0; i<pointsList.Count-1; i++) {
				float R = D (CC, pointsList [i]);
				for (int j=i+1; j<pointsList.Count; j++) {
					float tmpR = D (CC, pointsList [j]);
					if (Mathf.Abs (R - tmpR) >0.8f) {
						z++;

					}
				}
				if (z <4) {
					line.SetColors (Color.yellow, Color.yellow);
					res=true;
				} else {
					line.SetColors (Color.red, Color.red);
					res=false;
				}


				isMousePressed = false;

			}
		}
		return res;
	}	
}
