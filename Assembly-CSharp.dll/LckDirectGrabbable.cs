using System;
using GorillaLocomotion.Gameplay;
using Liv.Lck.GorillaTag;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

// Token: 0x0200023A RID: 570
public class LckDirectGrabbable : MonoBehaviour, IGorillaGrabable
{
	// Token: 0x14000025 RID: 37
	// (add) Token: 0x06000D15 RID: 3349 RVA: 0x0009F2F4 File Offset: 0x0009D4F4
	// (remove) Token: 0x06000D16 RID: 3350 RVA: 0x0009F32C File Offset: 0x0009D52C
	public event Action onGrabbed;

	// Token: 0x14000026 RID: 38
	// (add) Token: 0x06000D17 RID: 3351 RVA: 0x0009F364 File Offset: 0x0009D564
	// (remove) Token: 0x06000D18 RID: 3352 RVA: 0x0009F39C File Offset: 0x0009D59C
	public event Action onReleased;

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06000D19 RID: 3353 RVA: 0x000384EB File Offset: 0x000366EB
	public GorillaGrabber grabber
	{
		get
		{
			return this._grabber;
		}
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x06000D1A RID: 3354 RVA: 0x000384F3 File Offset: 0x000366F3
	public bool isGrabbed
	{
		get
		{
			return this._grabber != null;
		}
	}

	// Token: 0x06000D1B RID: 3355 RVA: 0x00038501 File Offset: 0x00036701
	public Vector3 GetLocalGrabbedPosition(GorillaGrabber grabber)
	{
		if (grabber == null)
		{
			return Vector3.zero;
		}
		return base.transform.InverseTransformPoint(grabber.transform.position);
	}

	// Token: 0x06000D1C RID: 3356 RVA: 0x00038528 File Offset: 0x00036728
	public bool CanBeGrabbed(GorillaGrabber grabber)
	{
		return this._grabber == null || grabber == this._grabber;
	}

	// Token: 0x06000D1D RID: 3357 RVA: 0x0009F3D4 File Offset: 0x0009D5D4
	public void OnGrabbed(GorillaGrabber grabber, out Transform grabbedTransform, out Vector3 localGrabbedPosition)
	{
		if (!base.isActiveAndEnabled)
		{
			this._grabber = null;
			grabbedTransform = grabber.transform;
			localGrabbedPosition = Vector3.zero;
			return;
		}
		if (this._grabber != null && this._grabber != grabber)
		{
			this.ForceRelease();
		}
		bool flag;
		bool flag2;
		if (this._precise && this.IsSlingshotHeldInHand(out flag, out flag2) && ((grabber.XrNode == XRNode.LeftHand && flag) || (grabber.XrNode == XRNode.RightHand && flag2)))
		{
			this._grabber = null;
			grabbedTransform = grabber.transform;
			localGrabbedPosition = Vector3.zero;
			return;
		}
		this._grabber = grabber;
		GtColliderTriggerProcessor.CurrentGrabbedHand = grabber.XrNode;
		GtColliderTriggerProcessor.IsGrabbingTablet = true;
		grabbedTransform = base.transform;
		localGrabbedPosition = this.GetLocalGrabbedPosition(this._grabber);
		this.target.SetParent(grabber.transform, true);
		Action action = this.onGrabbed;
		if (action != null)
		{
			action();
		}
		UnityEvent onTabletGrabbed = this.OnTabletGrabbed;
		if (onTabletGrabbed == null)
		{
			return;
		}
		onTabletGrabbed.Invoke();
	}

	// Token: 0x06000D1E RID: 3358 RVA: 0x0009F4D4 File Offset: 0x0009D6D4
	public void OnGrabReleased(GorillaGrabber grabber)
	{
		this.target.transform.SetParent(this._originalTargetParent, true);
		this._grabber = null;
		GtColliderTriggerProcessor.IsGrabbingTablet = false;
		Action action = this.onReleased;
		if (action != null)
		{
			action();
		}
		UnityEvent onTabletReleased = this.OnTabletReleased;
		if (onTabletReleased == null)
		{
			return;
		}
		onTabletReleased.Invoke();
	}

	// Token: 0x06000D1F RID: 3359 RVA: 0x00038546 File Offset: 0x00036746
	public void ForceGrab(GorillaGrabber grabber)
	{
		grabber.Inject(base.transform, this.GetLocalGrabbedPosition(grabber));
	}

	// Token: 0x06000D20 RID: 3360 RVA: 0x0003855B File Offset: 0x0003675B
	public void ForceRelease()
	{
		if (this._grabber == null)
		{
			return;
		}
		this._grabber.Inject(null, Vector3.zero);
	}

	// Token: 0x06000D21 RID: 3361 RVA: 0x0009F528 File Offset: 0x0009D728
	private bool IsSlingshotHeldInHand(out bool leftHand, out bool rightHand)
	{
		VRRig rig = VRRigCache.Instance.localRig.Rig;
		if (rig == null || rig.projectileWeapon == null)
		{
			leftHand = false;
			rightHand = false;
			return false;
		}
		leftHand = rig.projectileWeapon.InLeftHand();
		rightHand = rig.projectileWeapon.InRightHand();
		return rig.projectileWeapon.InHand();
	}

	// Token: 0x06000D22 RID: 3362 RVA: 0x0003857D File Offset: 0x0003677D
	public void SetOriginalTargetParent(Transform parent)
	{
		this._originalTargetParent = parent;
	}

	// Token: 0x06000D23 RID: 3363 RVA: 0x00038586 File Offset: 0x00036786
	public bool MomentaryGrabOnly()
	{
		return true;
	}

	// Token: 0x06000D25 RID: 3365 RVA: 0x000314EB File Offset: 0x0002F6EB
	string IGorillaGrabable.get_name()
	{
		return base.name;
	}

	// Token: 0x04001067 RID: 4199
	public UnityEvent OnTabletGrabbed = new UnityEvent();

	// Token: 0x04001068 RID: 4200
	public UnityEvent OnTabletReleased = new UnityEvent();

	// Token: 0x04001069 RID: 4201
	[SerializeField]
	private Transform _originalTargetParent;

	// Token: 0x0400106A RID: 4202
	public Transform target;

	// Token: 0x0400106B RID: 4203
	[SerializeField]
	private bool _precise;

	// Token: 0x0400106C RID: 4204
	private GorillaGrabber _grabber;
}
