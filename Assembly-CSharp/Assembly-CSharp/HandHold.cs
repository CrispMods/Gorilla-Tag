using System;
using System.Collections.Generic;
using GorillaLocomotion.Gameplay;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020005B4 RID: 1460
[RequireComponent(typeof(Collider))]
public class HandHold : MonoBehaviour, IGorillaGrabable
{
	// Token: 0x1400004F RID: 79
	// (add) Token: 0x0600244A RID: 9290 RVA: 0x000B5170 File Offset: 0x000B3370
	// (remove) Token: 0x0600244B RID: 9291 RVA: 0x000B51A4 File Offset: 0x000B33A4
	public static event HandHold.HandHoldPositionEvent HandPositionRequestOverride;

	// Token: 0x14000050 RID: 80
	// (add) Token: 0x0600244C RID: 9292 RVA: 0x000B51D8 File Offset: 0x000B33D8
	// (remove) Token: 0x0600244D RID: 9293 RVA: 0x000B520C File Offset: 0x000B340C
	public static event HandHold.HandHoldEvent HandPositionReleaseOverride;

	// Token: 0x0600244E RID: 9294 RVA: 0x000B523F File Offset: 0x000B343F
	private void Initialize()
	{
		if (this.initialized)
		{
			return;
		}
		this.myTappable = base.GetComponent<Tappable>();
		this.myCollider = base.GetComponent<Collider>();
		this.initialized = true;
	}

	// Token: 0x0600244F RID: 9295 RVA: 0x00044826 File Offset: 0x00042A26
	public virtual bool CanBeGrabbed(GorillaGrabber grabber)
	{
		return true;
	}

	// Token: 0x06002450 RID: 9296 RVA: 0x000B526C File Offset: 0x000B346C
	void IGorillaGrabable.OnGrabbed(GorillaGrabber g, out Transform grabbedTransform, out Vector3 localGrabbedPosition)
	{
		this.Initialize();
		grabbedTransform = base.transform;
		Vector3 position = g.transform.position;
		localGrabbedPosition = base.transform.InverseTransformPoint(position);
		Vector3 arg;
		g.Player.AddHandHold(base.transform, localGrabbedPosition, g, g.IsRightHand, this.rotatePlayerWhenHeld, out arg);
		if (this.handSnapMethod != HandHold.HandSnapMethod.None && HandHold.HandPositionRequestOverride != null)
		{
			HandHold.HandPositionRequestOverride(this, g.IsRightHand, this.CalculateOffset(position));
		}
		this.OnGrab.Invoke(arg);
		this.OnGrabHandHold.Invoke(this);
		this.OnGrabHanded.Invoke(g.IsRightHand);
		if (this.myTappable != null)
		{
			this.myTappable.OnGrab();
		}
	}

	// Token: 0x06002451 RID: 9297 RVA: 0x000B5334 File Offset: 0x000B3534
	void IGorillaGrabable.OnGrabReleased(GorillaGrabber g)
	{
		this.Initialize();
		g.Player.RemoveHandHold(g, g.IsRightHand);
		if (this.handSnapMethod != HandHold.HandSnapMethod.None && HandHold.HandPositionReleaseOverride != null)
		{
			HandHold.HandPositionReleaseOverride(this, g.IsRightHand);
		}
		this.OnRelease.Invoke();
		this.OnReleaseHandHold.Invoke(this);
		if (this.myTappable != null)
		{
			this.myTappable.OnRelease();
		}
	}

	// Token: 0x06002452 RID: 9298 RVA: 0x000B53AC File Offset: 0x000B35AC
	private Vector3 CalculateOffset(Vector3 position)
	{
		switch (this.handSnapMethod)
		{
		case HandHold.HandSnapMethod.SnapToNearestEdge:
			if (this.myCollider == null)
			{
				this.myCollider = base.GetComponent<Collider>();
				if (this.myCollider is MeshCollider && !(this.myCollider as MeshCollider).convex)
				{
					this.handSnapMethod = HandHold.HandSnapMethod.None;
					return Vector3.zero;
				}
			}
			return base.transform.position - this.myCollider.ClosestPoint(position);
		case HandHold.HandSnapMethod.SnapToXAxisPoint:
			return base.transform.position - base.transform.TransformPoint(Vector3.right * base.transform.InverseTransformPoint(position).x);
		case HandHold.HandSnapMethod.SnapToYAxisPoint:
			return base.transform.position - base.transform.TransformPoint(Vector3.up * base.transform.InverseTransformPoint(position).y);
		case HandHold.HandSnapMethod.SnapToZAxisPoint:
			return base.transform.position - base.transform.TransformPoint(Vector3.forward * base.transform.InverseTransformPoint(position).z);
		default:
			return Vector3.zero;
		}
	}

	// Token: 0x06002453 RID: 9299 RVA: 0x000B54EA File Offset: 0x000B36EA
	public bool MomentaryGrabOnly()
	{
		return this.forceMomentary;
	}

	// Token: 0x06002455 RID: 9301 RVA: 0x0001259F File Offset: 0x0001079F
	string IGorillaGrabable.get_name()
	{
		return base.name;
	}

	// Token: 0x04002862 RID: 10338
	private Dictionary<Transform, Transform> attached = new Dictionary<Transform, Transform>();

	// Token: 0x04002863 RID: 10339
	[SerializeField]
	private HandHold.HandSnapMethod handSnapMethod;

	// Token: 0x04002864 RID: 10340
	[SerializeField]
	private bool rotatePlayerWhenHeld;

	// Token: 0x04002865 RID: 10341
	[SerializeField]
	private UnityEvent<Vector3> OnGrab;

	// Token: 0x04002866 RID: 10342
	[SerializeField]
	private UnityEvent<HandHold> OnGrabHandHold;

	// Token: 0x04002867 RID: 10343
	[SerializeField]
	private UnityEvent<bool> OnGrabHanded;

	// Token: 0x04002868 RID: 10344
	[SerializeField]
	private UnityEvent OnRelease;

	// Token: 0x04002869 RID: 10345
	[SerializeField]
	private UnityEvent<HandHold> OnReleaseHandHold;

	// Token: 0x0400286A RID: 10346
	private bool initialized;

	// Token: 0x0400286B RID: 10347
	private Collider myCollider;

	// Token: 0x0400286C RID: 10348
	private Tappable myTappable;

	// Token: 0x0400286D RID: 10349
	[Tooltip("Turning this on disables \"pregrabbing\". Use pregrabbing to allow players to catch a handhold even if they have squeezed the trigger too soon. Useful if you're anticipating jumping players needed to grab while airborne")]
	[SerializeField]
	private bool forceMomentary = true;

	// Token: 0x020005B5 RID: 1461
	private enum HandSnapMethod
	{
		// Token: 0x0400286F RID: 10351
		None,
		// Token: 0x04002870 RID: 10352
		SnapToCenterPoint,
		// Token: 0x04002871 RID: 10353
		SnapToNearestEdge,
		// Token: 0x04002872 RID: 10354
		SnapToXAxisPoint,
		// Token: 0x04002873 RID: 10355
		SnapToYAxisPoint,
		// Token: 0x04002874 RID: 10356
		SnapToZAxisPoint
	}

	// Token: 0x020005B6 RID: 1462
	// (Invoke) Token: 0x06002457 RID: 9303
	public delegate void HandHoldPositionEvent(HandHold hh, bool rh, Vector3 pos);

	// Token: 0x020005B7 RID: 1463
	// (Invoke) Token: 0x0600245B RID: 9307
	public delegate void HandHoldEvent(HandHold hh, bool rh);
}
