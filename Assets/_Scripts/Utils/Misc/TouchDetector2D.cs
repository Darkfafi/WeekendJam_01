// Created by Ramses Di Perna

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// A component that tracks hits on different sides with the given 2D collider 
/// This information can be accessed by Variables, Full info lists and Events
/// The Events are called on start and finish of the hit, and also in the middle (A loop for as long a hit is active)
/// </summary>
public class TouchDetector2D : MonoBehaviour
{
	#region members
	public delegate void OnHitHandler(Vector2 side, Collider2D collider);

	public event OnHitHandler HitEvent;
	public event OnHitHandler InHitEvent;
	public event OnHitHandler HitEndEvent;

	public bool IsRunning { get { return (this.coll2D != null); } }

	[Range(3, 50)]
	public int ChecksPerSide = 3;
	[Range(0.01f, 20)]
	public float DistanceCheck = 0.01f;
	[Range(0.02f, 10)]
	public float StartDistance = 0.02f;
	[Range(0, 1)]
	public float MarginBordersHorizontal = 0;
	[Range(0, 1)]
	public float MarginBordersVertical = 0;

	public Collider2D HitLeft { get; private set; }
	public Collider2D HitRight { get; private set; }
	public Collider2D HitUp { get; private set; }
	public Collider2D HitDown { get; private set; }

	private Collider2D coll2D = null;
	private Vector2[] sides = { Vector2.left, Vector2.right, Vector2.up, Vector2.down };
	private Vector2 checkCoordinates = new Vector2();

	private LayerMask layerMaskLeft = -1;
	private LayerMask layerMaskRight = -1;
	private LayerMask layerMaskUp = -1;
	private LayerMask layerMaskDown = -1;
	#endregion


	#region public methods

	public void RunOn(Collider2D collider)
	{
		this.coll2D = collider;
    }

	public void RunOn(Collider2D collider,int allSidesLayerMask)
	{
		SetMaskLayers(allSidesLayerMask);
		RunOn(collider);
    }

	public void RunOn(Collider2D collider, int leftLayerMask, int rightLayerMask, int upLayerMask, int downLayerMask)
	{
		SetMaskLayers(leftLayerMask, rightLayerMask, upLayerMask, downLayerMask);
		RunOn(collider);
	}
	public void SetMaskLayers(int allSidesLayerMask)
	{
		SetMaskLayers(allSidesLayerMask, allSidesLayerMask, allSidesLayerMask, allSidesLayerMask);
	}

	public void SetMaskLayers(int leftLayerMask, int rightLayerMask, int upLayerMask, int downLayerMask)
	{
		layerMaskLeft = leftLayerMask;
		layerMaskRight = rightLayerMask;
		layerMaskUp = upLayerMask;
		layerMaskDown = downLayerMask;
	}
	public void Stop()
	{
		this.coll2D = null;
	}

	public Collider2D GetHitInfoFromSide(Vector2 side)
	{
		if (side == Vector2.left)
		{
			return HitLeft;
		}
		else if (side == Vector2.right)
		{
			return HitRight;
		}
		else if (side == Vector2.up)
		{
			return HitUp;
		}
		else if (side == Vector2.down)
		{
			return HitDown;
		}
		return null;
	}

	public KeyValuePair<Vector2, Collider2D>[] AllSidesHitInfo()
	{

		KeyValuePair<Vector2, Collider2D> leftInfo = new KeyValuePair<Vector2, Collider2D>(Vector2.left, HitLeft);
		KeyValuePair<Vector2, Collider2D> upInfo = new KeyValuePair<Vector2, Collider2D>(Vector2.up, HitUp);
		KeyValuePair<Vector2, Collider2D> downInfo = new KeyValuePair<Vector2, Collider2D>(Vector2.down, HitDown);
		KeyValuePair<Vector2, Collider2D> rightInfo = new KeyValuePair<Vector2, Collider2D>(Vector2.right, HitRight);

		KeyValuePair<Vector2, Collider2D>[] sidesInfo = { leftInfo, upInfo, rightInfo, downInfo };
		return sidesInfo;
	}

	#endregion

	#region unity methods

	protected void Update()
	{
		if (IsRunning)
		{
			foreach (Vector2 side in sides)
			{
				Collider2D beforeFillData = null;
				Collider2D afterFillData = null;
				RaycastHit2D hit2D = new RaycastHit2D();
				for (int i = -(ChecksPerSide / 2); i < ChecksPerSide - (ChecksPerSide / 2); i++)
				{
					checkCoordinates = coll2D.transform.position;

					if (side.x != 0)
					{
						checkCoordinates = new Vector2(checkCoordinates.x + ((coll2D.bounds.size.x / 2) * side.x) + (StartDistance * side.x),
							(checkCoordinates.y + (coll2D.bounds.size.y / 2) + ((coll2D.bounds.size.y - (coll2D.bounds.size.y * MarginBordersVertical)) * ((float)i / (float)(ChecksPerSide - 1)))));
					}
					else
					{
						checkCoordinates = new Vector2((checkCoordinates.x + ((coll2D.bounds.size.x - (coll2D.bounds.size.x * MarginBordersHorizontal)) * ((float)i / (float)(ChecksPerSide - 1)))),
							checkCoordinates.y + (coll2D.bounds.size.y / 2) + ((coll2D.bounds.size.y / 2) * side.y) + (StartDistance * side.y));
					}

					if (hit2D.collider == null)
					{
						LayerMask maskToCheck = new LayerMask();
						if (side == Vector2.left)
						{
							maskToCheck = layerMaskLeft;
						}
						else if (side == Vector2.right)
						{
							maskToCheck = layerMaskRight;
						}
						else if (side == Vector2.up)
						{
							maskToCheck = layerMaskUp;
						}
						else if (side == Vector2.down)
						{
							maskToCheck = layerMaskDown;
						}
						hit2D = Physics2D.Raycast(checkCoordinates, side, DistanceCheck, maskToCheck);
					}
#if UNITY_EDITOR
					if (hit2D.collider != null)
					{
						Debug.DrawRay(checkCoordinates, side * ((hit2D.collider.transform.position - coll2D.transform.position).magnitude - StartDistance), Color.red);
					}
					else
					{
						Color color = Color.blue;
						if (side.x != 0)
						{
							color = Color.cyan;
                        }

						Debug.DrawRay(checkCoordinates, side * DistanceCheck, color);
					}
#endif
				}

				if (side == Vector2.left)
				{
					beforeFillData = HitLeft;
					HitLeft = hit2D.collider;
					afterFillData = HitLeft;
				}
				else if (side == Vector2.right)
				{
					beforeFillData = HitRight;
					HitRight = hit2D.collider;
					afterFillData = HitRight;
				}
				else if (side == Vector2.up)
				{
					beforeFillData = HitUp;
					HitUp = hit2D.collider;
					afterFillData = HitUp;
				}
				else if (side == Vector2.down)
				{
					beforeFillData = HitDown;
					HitDown = hit2D.collider;
					afterFillData = HitDown;
				}
				if(HitEvent != null && beforeFillData == null && afterFillData != null)
				{
					HitEvent(side, afterFillData);
                }
				if (HitEndEvent != null && afterFillData != beforeFillData && beforeFillData != null)
				{
					HitEndEvent(side, beforeFillData);
				}
				if(InHitEvent != null && afterFillData != null)
				{
					InHitEvent(side, afterFillData);
                }
			}
		}
	}

#endregion
}
