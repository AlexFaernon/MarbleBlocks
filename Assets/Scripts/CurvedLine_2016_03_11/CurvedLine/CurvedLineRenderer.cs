using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

[RequireComponent( typeof(LineRenderer) )]
public class CurvedLineRenderer : MonoBehaviour 
{
	//PUBLIC
	[SerializeField] private bool isFilledByLevel;
	[SerializeField] private LineRenderer lineRenderer;
	public float lineSegmentSize = 0.15f;
	public float lineWidth = 0.1f;
	[Header("Gizmos")]
	public bool showGizmos = true;
	public float gizmoSize = 0.1f;
	public Color gizmoColor = new Color(1,0,0,0.5f);
	//PRIVATE
	private CurvedLinePoint[] linePoints = new CurvedLinePoint[0];
	private Vector3[] linePositions = new Vector3[0];
	private Vector3[] linePositionsOld = new Vector3[0];
	public List<float> pointsLength;

	// Update is called once per frame
	public void Update () 
	{
		GetPoints();
		SetPointsToLine();
		if (isFilledByLevel)
		{
			DrawCompleteLine();
		}
	}

	private void DrawCompleteLine()
	{
		var alphaTime = pointsLength[PlayerData.SingleLevelCompleted] / pointsLength.Last();
		var alphaKeys = lineRenderer.colorGradient.alphaKeys;
		alphaKeys[0].time = alphaTime;
		var gradient = new Gradient
		{
			mode = GradientMode.Fixed,
			alphaKeys = alphaKeys,
			colorKeys = lineRenderer.colorGradient.colorKeys
		};
		lineRenderer.colorGradient = gradient;
	}
	
	void GetPoints()
	{
		//find curved points in children
		linePoints = GetComponentsInChildren<CurvedLinePoint>();

		//add positions
		linePositions = new Vector3[linePoints.Length];
		for( int i = 0; i < linePoints.Length; i++ )
		{
			linePositions[i] = linePoints[i].transform.position;
		}
	}

	void SetPointsToLine()
	{
		//create old positions if they dont match
		if( linePositionsOld.Length != linePositions.Length )
		{
			linePositionsOld = new Vector3[linePositions.Length];
		}

		//check if line points have moved
		bool moved = false;
		for( int i = 0; i < linePositions.Length; i++ )
		{
			//compare
			if( linePositions[i] != linePositionsOld[i] )
			{
				moved = true;
			}
		}

		//update if moved
		if( moved == true )
		{

			//get smoothed values
			Vector3[] smoothedPoints = LineSmoother.SmoothLine( linePositions, lineSegmentSize, out pointsLength );

			//set line settings
			lineRenderer.positionCount = smoothedPoints.Length;
			lineRenderer.SetPositions(smoothedPoints);
			lineRenderer.startWidth = lineWidth;
			lineRenderer.endWidth = lineWidth;
		}
	}

	void OnDrawGizmosSelected()
	{
		Update();
	}

	void OnDrawGizmos()
	{
		if( linePoints.Length == 0 )
		{
			GetPoints();
		}

		//settings for gizmos
		foreach( CurvedLinePoint linePoint in linePoints )
		{
			linePoint.showGizmo = showGizmos;
			linePoint.gizmoSize = gizmoSize;
			linePoint.gizmoColor = gizmoColor;
		}
	}
}
