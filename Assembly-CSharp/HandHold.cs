using System;
using System.Collections.Generic;
using GorillaLocomotion.Gameplay;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020005B3 RID: 1459
[RequireComponent(typeof(Collider))]
public class HandHold : MonoBehaviour, IGorillaGrabable
{
	// Token: 0x1400004F RID: 79
	// (add) Token: 0x06002442 RID: 9282 RVA: 0x000B4CF0 File Offset: 0x000B2EF0
	// (remove) Token: 0x06002443 RID: 9283 RVA: 0x000B4D24 File Offset: 0x000B2F24
	public static event HandHold.HandHoldPositionEvent HandPositionRequestOverride;

	// Token: 0x14000050 RID: 80
	// (add) Token: 0x06002444 RID: 9284 RVA: 0x000B4D58 File Offset: 0x000B2F58
	// (remove) Token: 0x06002445 RID: 9285 RVA: 0x000B4D8C File Offset: 0x000B2F8C
	public static event HandHold.HandHoldEvent HandPositionReleaseOverride;

	// Token: 0x06002446 RID: 9286 RVA: 0x000B4DBF File Offset: 0x000B2FBF
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

	// Token: 0x06002447 RID: 9287 RVA: 0x000444E2 File Offset: 0x000426E2
	public virtual bool CanBeGrabbed(GorillaGrabber grabber)
	{
		return true;
	}

	// Token: 0x06002448 RID: 9288 RVA: 0x000B4DEC File Offset: 0x000B2FEC
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

	// Token: 0x06002449 RID: 9289 RVA: 0x000B4EB4 File Offset: 0x000B30B4
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

	// Token: 0x0600244A RID: 9290 RVA: 0x000B4F2C File Offset: 0x000B312C
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

	// Token: 0x0600244B RID: 9291 RVA: 0x000B506A File Offset: 0x000B326A
	public bool MomentaryGrabOnly()
	{
		return this.forceMomentary;
	}

	// Token: 0x0600244D RID: 9293 RVA: 0x0001227B File Offset: 0x0001047B
	string IGorillaGrabable.get_name()
	{
		return base.name;
	}

	// Token: 0x0400285C RID: 10332
	private Dictionary<Transform, Transform> attached = new Dictionary<Transform, Transform>();

	// Token: 0x0400285D RID: 10333
	[SerializeField]
	private HandHold.HandSnapMethod handSnapMethod;

	// Token: 0x0400285E RID: 10334
	[SerializeField]
	private bool rotatePlayerWhenHeld;

	// Token: 0x0400285F RID: 10335
	[SerializeField]
	private UnityEvent<Vector3> OnGrab;

	// Token: 0x04002860 RID: 10336
	[SerializeField]
	private UnityEvent<HandHold> OnGrabHandHold;

	// Token: 0x04002861 RID: 10337
	[SerializeField]
	private UnityEvent<bool> OnGrabHanded;

	// Token: 0x04002862 RID: 10338
	[SerializeField]
	private UnityEvent OnRelease;

	// Token: 0x04002863 RID: 10339
	[SerializeField]
	private UnityEvent<HandHold> OnReleaseHandHold;

	// Token: 0x04002864 RID: 10340
	private bool initialized;

	// Token: 0x04002865 RID: 10341
	private Collider myCollider;

	// Token: 0x04002866 RID: 10342
	private Tappable myTappable;

	// Token: 0x04002867 RID: 10343
	[Tooltip("Turning this on disables \"pregrabbing\". Use pregrabbing to allow players to catch a handhold even if they have squeezed the trigger too soon. Useful if you're anticipating jumping players needed to grab while airborne")]
	[SerializeField]
	private bool forceMomentary = true;

	// Token: 0x020005B4 RID: 1460
	private enum HandSnapMethod
	{
		// Token: 0x04002869 RID: 10345
		None,
		// Token: 0x0400286A RID: 10346
		SnapToCenterPoint,
		// Token: 0x0400286B RID: 10347
		SnapToNearestEdge,
		// Token: 0x0400286C RID: 10348
		SnapToXAxisPoint,
		// Token: 0x0400286D RID: 10349
		SnapToYAxisPoint,
		// Token: 0x0400286E RID: 10350
		SnapToZAxisPoint
	}

	// Token: 0x020005B5 RID: 1461
	// (Invoke) Token: 0x0600244F RID: 9295
	public delegate void HandHoldPositionEvent(HandHold hh, bool rh, Vector3 pos);

	// Token: 0x020005B6 RID: 1462
	// (Invoke) Token: 0x06002453 RID: 9299
	public delegate void HandHoldEvent(HandHold hh, bool rh);
}
