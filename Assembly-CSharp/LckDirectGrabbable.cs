using System;
using GorillaLocomotion.Gameplay;
using Liv.Lck.GorillaTag;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

// Token: 0x02000245 RID: 581
public class LckDirectGrabbable : MonoBehaviour, IGorillaGrabable
{
	// Token: 0x14000026 RID: 38
	// (add) Token: 0x06000D5E RID: 3422 RVA: 0x000A1B80 File Offset: 0x0009FD80
	// (remove) Token: 0x06000D5F RID: 3423 RVA: 0x000A1BB8 File Offset: 0x0009FDB8
	public event Action onGrabbed;

	// Token: 0x14000027 RID: 39
	// (add) Token: 0x06000D60 RID: 3424 RVA: 0x000A1BF0 File Offset: 0x0009FDF0
	// (remove) Token: 0x06000D61 RID: 3425 RVA: 0x000A1C28 File Offset: 0x0009FE28
	public event Action onReleased;

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x06000D62 RID: 3426 RVA: 0x000397AB File Offset: 0x000379AB
	public GorillaGrabber grabber
	{
		get
		{
			return this._grabber;
		}
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x06000D63 RID: 3427 RVA: 0x000397B3 File Offset: 0x000379B3
	public bool isGrabbed
	{
		get
		{
			return this._grabber != null;
		}
	}

	// Token: 0x06000D64 RID: 3428 RVA: 0x000397C1 File Offset: 0x000379C1
	public Vector3 GetLocalGrabbedPosition(GorillaGrabber grabber)
	{
		if (grabber == null)
		{
			return Vector3.zero;
		}
		return base.transform.InverseTransformPoint(grabber.transform.position);
	}

	// Token: 0x06000D65 RID: 3429 RVA: 0x000397E8 File Offset: 0x000379E8
	public bool CanBeGrabbed(GorillaGrabber grabber)
	{
		return this._grabber == null || grabber == this._grabber;
	}

	// Token: 0x06000D66 RID: 3430 RVA: 0x000A1C60 File Offset: 0x0009FE60
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

	// Token: 0x06000D67 RID: 3431 RVA: 0x000A1D60 File Offset: 0x0009FF60
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

	// Token: 0x06000D68 RID: 3432 RVA: 0x00039806 File Offset: 0x00037A06
	public void ForceGrab(GorillaGrabber grabber)
	{
		grabber.Inject(base.transform, this.GetLocalGrabbedPosition(grabber));
	}

	// Token: 0x06000D69 RID: 3433 RVA: 0x0003981B File Offset: 0x00037A1B
	public void ForceRelease()
	{
		if (this._grabber == null)
		{
			return;
		}
		this._grabber.Inject(null, Vector3.zero);
	}

	// Token: 0x06000D6A RID: 3434 RVA: 0x000A1DB4 File Offset: 0x0009FFB4
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

	// Token: 0x06000D6B RID: 3435 RVA: 0x0003983D File Offset: 0x00037A3D
	public void SetOriginalTargetParent(Transform parent)
	{
		this._originalTargetParent = parent;
	}

	// Token: 0x06000D6C RID: 3436 RVA: 0x00039846 File Offset: 0x00037A46
	public bool MomentaryGrabOnly()
	{
		return true;
	}

	// Token: 0x06000D6E RID: 3438 RVA: 0x0003261E File Offset: 0x0003081E
	string IGorillaGrabable.get_name()
	{
		return base.name;
	}

	// Token: 0x040010AC RID: 4268
	public UnityEvent OnTabletGrabbed = new UnityEvent();

	// Token: 0x040010AD RID: 4269
	public UnityEvent OnTabletReleased = new UnityEvent();

	// Token: 0x040010AE RID: 4270
	[SerializeField]
	private Transform _originalTargetParent;

	// Token: 0x040010AF RID: 4271
	public Transform target;

	// Token: 0x040010B0 RID: 4272
	[SerializeField]
	private bool _precise;

	// Token: 0x040010B1 RID: 4273
	private GorillaGrabber _grabber;
}
