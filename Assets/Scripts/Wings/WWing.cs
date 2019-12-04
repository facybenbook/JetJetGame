﻿//
// Copyright (c) Brian Hernandez. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using UnityEngine;

public class WWing : MonoBehaviour
{
	[Tooltip("Size of the wing. The bigger the wing, the more lift it provides.")]
	public Vector2 dimensions = new Vector2(7.0f, 2.0f);

	[Tooltip("When true, wing forces will be applied only at the center of mass.")]
	public bool applyForcesToCenter = false;

	[Tooltip("Lift coefficient curve.")]
	public WCurve wing;
	[Tooltip("The higher the value, the more lift the wing applie at a given angle of attack.")]
	public float liftMultiplier = 1.0f;
	[Tooltip("The higher the value, the more drag the wing incurs at a given angle of attack.")]
	public float dragMultiplier = 1.0f;

	private Rigidbody rigid;

	private float liftCoefficient = 0.0f;
	private float dragCoefficient = 0.0f;
	private float liftForce = 0.0f;
	private float dragForce = 0.0f;
	private float angleOfAttack = 0.0f;

	private bool WingDeactivated = false;

	public float AngleOfAttack
	{
		get
		{
			if (rigid != null)
			{
				Vector3 localVelocity = transform.InverseTransformDirection(rigid.velocity);
				return angleOfAttack * -Mathf.Sign(localVelocity.y);
			}
			else
			{
				return 0.0f;
			}
		}
	}

	public float WingArea
	{
		get { return dimensions.x * dimensions.y; }
	}

	public float LiftCoefficient { get { return liftCoefficient; } }
	public float LiftForce { get { return liftForce; } }
	public float DragCoefficient { get { return dragCoefficient; } }
	public float DragForce { get { return dragForce; } }

	public Rigidbody Rigidbody
	{
		set { rigid = value; }
	}

	private void Awake()
	{
		// Wings affect the center of mass of the overall rigidbody
		rigid = GetComponentInParent<Rigidbody>();
	}

	private void Start()
	{
		if (rigid == null)
		{
			Debug.LogError(name + ": WWing has no rigidbody on self or parent!");
		}

		if (wing == null)
		{
			Debug.LogError(name + ": WWing has no defined wing curves!");
		}
	}

	private void Update()
	{
		// Prevent division by zero
		if (dimensions.x <= 0.0f)
			dimensions.x = 0.01f;
		if (dimensions.y <= 0.0f)
			dimensions.y = 0.01f;

		if (rigid != null)
		{
			Debug.DrawRay(transform.position, transform.up * liftForce * 0.0001f, new Color(52f, 152f, 219f, 0.4f) );
			Debug.DrawRay(transform.position, -rigid.velocity.normalized * dragForce * 0.0001f, new Color(52f, 152f, 219f, 0.1f) );
		}
	}

	private void FixedUpdate()
	{
		if (rigid != null && wing != null && !rigid.isKinematic && !WingDeactivated)
			DoWingForces();
	}

	public void WingOn()
	{
		WingDeactivated = false;
	}

	public void WingOff()
	{
		Debug.Log("WingOff");
		WingDeactivated = true;
	}

	public void DoWingForces()
	{
		Vector3 forceApplyPos = (applyForcesToCenter) ? rigid.transform.TransformPoint(rigid.centerOfMass) : transform.position;

		Vector3 localVelocity = transform.InverseTransformDirection(rigid.GetPointVelocity(transform.position));
		localVelocity.x = 0.0f;

		// Angle of attack is used as the look up for the lift and drag curves
		angleOfAttack = Vector3.Angle(Vector3.forward, localVelocity);
		liftCoefficient = wing.GetLiftAtAaoA(angleOfAttack);
		dragCoefficient = wing.GetDragAtAaoA(angleOfAttack);

		// Calculate lift/drag
		liftForce = localVelocity.sqrMagnitude * liftCoefficient * WingArea * liftMultiplier;
		dragForce = localVelocity.sqrMagnitude * dragCoefficient * WingArea * dragMultiplier;

		// Vector3.Angle always returns a positive value, so add the sign back in
		liftForce *= -Mathf.Sign(localVelocity.y);

		// Lift is always perpendicular to air flow
		Vector3 liftDirection = Vector3.Cross(rigid.velocity, transform.right).normalized;
		rigid.AddForceAtPosition(liftDirection * liftForce, forceApplyPos, ForceMode.Force);

		// Drag is always opposite of the velocity
		rigid.AddForceAtPosition(-rigid.velocity.normalized * dragForce, forceApplyPos, ForceMode.Force);

	}

	private void OnDrawGizmosSelected()
	{
		Matrix4x4 oldMatrix = Gizmos.matrix;

		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
		Gizmos.DrawWireCube(Vector3.zero, new Vector3(dimensions.x, 0.0f, dimensions.y));

		Gizmos.matrix = oldMatrix;
	}
}
