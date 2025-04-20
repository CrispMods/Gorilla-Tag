using System;
using System.Collections.Generic;
using GorillaExtensions;
using GorillaLocomotion.Gameplay;
using GT_CustomMapSupportRuntime;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020005C1 RID: 1473
[RequireComponent(typeof(Collider))]
public class HandHold : MonoBehaviour, IGorillaGrabable
{
	// Token: 0x14000050 RID: 80
	// (add) Token: 0x060024A2 RID: 9378 RVA: 0x00103630 File Offset: 0x00101830
	// (remove) Token: 0x060024A3 RID: 9379 RVA: 0x00103664 File Offset: 0x00101864
	public static event HandHold.HandHoldPositionEvent HandPositionRequestOverride;

	// Token: 0x14000051 RID: 81
	// (add) Token: 0x060024A4 RID: 9380 RVA: 0x00103698 File Offset: 0x00101898
	// (remove) Token: 0x060024A5 RID: 9381 RVA: 0x001036CC File Offset: 0x001018CC
	public static event HandHold.HandHoldEvent HandPositionReleaseOverride;

	// Token: 0x060024A6 RID: 9382 RVA: 0x00103700 File Offset: 0x00101900
	public void OnDisable()
	{
		for (int i = 0; i < this.currentGrabbers.Count; i++)
		{
			if (this.currentGrabbers[i].IsNotNull())
			{
				this.currentGrabbers[i].Ungrab(this);
			}
		}
	}

	// Token: 0x060024A7 RID: 9383 RVA: 0x00048E07 File Offset: 0x00047007
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

	// Token: 0x060024A8 RID: 9384 RVA: 0x00039846 File Offset: 0x00037A46
	public virtual bool CanBeGrabbed(GorillaGrabber grabber)
	{
		return true;
	}

	// Token: 0x060024A9 RID: 9385 RVA: 0x00103748 File Offset: 0x00101948
	void IGorillaGrabable.OnGrabbed(GorillaGrabber g, out Transform grabbedTransform, out Vector3 localGrabbedPosition)
	{
		this.Initialize();
		grabbedTransform = base.transform;
		Vector3 position = g.transform.position;
		localGrabbedPosition = base.transform.InverseTransformPoint(position);
		Vector3 arg;
		g.Player.AddHandHold(base.transform, localGrabbedPosition, g, g.IsRightHand, this.rotatePlayerWhenHeld, out arg);
		this.currentGrabbers.AddIfNew(g);
		if (this.handSnapMethod != HandHold.HandSnapMethod.None && HandHold.HandPositionRequestOverride != null)
		{
			HandHold.HandPositionRequestOverride(this, g.IsRightHand, this.CalculateOffset(position));
		}
		UnityEvent<Vector3> onGrab = this.OnGrab;
		if (onGrab != null)
		{
			onGrab.Invoke(arg);
		}
		UnityEvent<HandHold> onGrabHandHold = this.OnGrabHandHold;
		if (onGrabHandHold != null)
		{
			onGrabHandHold.Invoke(this);
		}
		UnityEvent<bool> onGrabHanded = this.OnGrabHanded;
		if (onGrabHanded != null)
		{
			onGrabHanded.Invoke(g.IsRightHand);
		}
		if (this.myTappable != null)
		{
			this.myTappable.OnGrab();
		}
	}

	// Token: 0x060024AA RID: 9386 RVA: 0x00103830 File Offset: 0x00101A30
	void IGorillaGrabable.OnGrabReleased(GorillaGrabber g)
	{
		this.Initialize();
		g.Player.RemoveHandHold(g, g.IsRightHand);
		this.currentGrabbers.Remove(g);
		if (this.handSnapMethod != HandHold.HandSnapMethod.None && HandHold.HandPositionReleaseOverride != null)
		{
			HandHold.HandPositionReleaseOverride(this, g.IsRightHand);
		}
		UnityEvent onRelease = this.OnRelease;
		if (onRelease != null)
		{
			onRelease.Invoke();
		}
		UnityEvent<HandHold> onReleaseHandHold = this.OnReleaseHandHold;
		if (onReleaseHandHold != null)
		{
			onReleaseHandHold.Invoke(this);
		}
		if (this.myTappable != null)
		{
			this.myTappable.OnRelease();
		}
	}

	// Token: 0x060024AB RID: 9387 RVA: 0x001038C0 File Offset: 0x00101AC0
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

	// Token: 0x060024AC RID: 9388 RVA: 0x00048E31 File Offset: 0x00047031
	public bool MomentaryGrabOnly()
	{
		return this.forceMomentary;
	}

	// Token: 0x060024AD RID: 9389 RVA: 0x00048E39 File Offset: 0x00047039
	public void CopyProperties(HandHoldSettings handHoldSettings)
	{
		this.handSnapMethod = (HandHold.HandSnapMethod)handHoldSettings.handSnapMethod;
		this.rotatePlayerWhenHeld = handHoldSettings.rotatePlayerWhenHeld;
		this.forceMomentary = !handHoldSettings.allowPreGrab;
	}

	// Token: 0x060024AF RID: 9391 RVA: 0x0003261E File Offset: 0x0003081E
	string IGorillaGrabable.get_name()
	{
		return base.name;
	}

	// Token: 0x040028B8 RID: 10424
	private Dictionary<Transform, Transform> attached = new Dictionary<Transform, Transform>();

	// Token: 0x040028B9 RID: 10425
	[SerializeField]
	private HandHold.HandSnapMethod handSnapMethod;

	// Token: 0x040028BA RID: 10426
	[SerializeField]
	private bool rotatePlayerWhenHeld;

	// Token: 0x040028BB RID: 10427
	[SerializeField]
	private UnityEvent<Vector3> OnGrab;

	// Token: 0x040028BC RID: 10428
	[SerializeField]
	private UnityEvent<HandHold> OnGrabHandHold;

	// Token: 0x040028BD RID: 10429
	[SerializeField]
	private UnityEvent<bool> OnGrabHanded;

	// Token: 0x040028BE RID: 10430
	[SerializeField]
	private UnityEvent OnRelease;

	// Token: 0x040028BF RID: 10431
	[SerializeField]
	private UnityEvent<HandHold> OnReleaseHandHold;

	// Token: 0x040028C0 RID: 10432
	private bool initialized;

	// Token: 0x040028C1 RID: 10433
	private Collider myCollider;

	// Token: 0x040028C2 RID: 10434
	private Tappable myTappable;

	// Token: 0x040028C3 RID: 10435
	[Tooltip("Turning this on disables \"pregrabbing\". Use pregrabbing to allow players to catch a handhold even if they have squeezed the trigger too soon. Useful if you're anticipating jumping players needed to grab while airborne")]
	[SerializeField]
	private bool forceMomentary = true;

	// Token: 0x040028C4 RID: 10436
	private List<GorillaGrabber> currentGrabbers = new List<GorillaGrabber>();

	// Token: 0x020005C2 RID: 1474
	private enum HandSnapMethod
	{
		// Token: 0x040028C6 RID: 10438
		None,
		// Token: 0x040028C7 RID: 10439
		SnapToCenterPoint,
		// Token: 0x040028C8 RID: 10440
		SnapToNearestEdge,
		// Token: 0x040028C9 RID: 10441
		SnapToXAxisPoint,
		// Token: 0x040028CA RID: 10442
		SnapToYAxisPoint,
		// Token: 0x040028CB RID: 10443
		SnapToZAxisPoint
	}

	// Token: 0x020005C3 RID: 1475
	// (Invoke) Token: 0x060024B1 RID: 9393
	public delegate void HandHoldPositionEvent(HandHold hh, bool rh, Vector3 pos);

	// Token: 0x020005C4 RID: 1476
	// (Invoke) Token: 0x060024B5 RID: 9397
	public delegate void HandHoldEvent(HandHold hh, bool rh);
}
