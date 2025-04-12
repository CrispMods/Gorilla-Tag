using System;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200014B RID: 331
public class DeployableObject : TransferrableObject
{
	// Token: 0x06000872 RID: 2162 RVA: 0x0003506B File Offset: 0x0003326B
	protected override void Awake()
	{
		this._deploySignal.OnSignal += this.DeployRPC;
		base.Awake();
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x0008CBFC File Offset: 0x0008ADFC
	internal override void OnEnable()
	{
		this._deploySignal.Enable();
		VRRig componentInParent = base.GetComponentInParent<VRRig>();
		for (int i = 0; i < this._rigAwareObjects.Length; i++)
		{
			IRigAware rigAware = this._rigAwareObjects[i] as IRigAware;
			if (rigAware != null)
			{
				rigAware.SetRig(componentInParent);
			}
		}
		this.m_VRRig = componentInParent;
		base.OnEnable();
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.itemState &= (TransferrableObject.ItemStates)(-2);
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x0003508A File Offset: 0x0003328A
	internal override void OnDisable()
	{
		this.m_VRRig = null;
		this._deploySignal.Disable();
		if (this._objectToDeploy.activeSelf)
		{
			this.ReturnChild();
		}
		base.OnDisable();
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x000350B7 File Offset: 0x000332B7
	protected override void OnDestroy()
	{
		this._deploySignal.Dispose();
		base.OnDestroy();
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x0008CC70 File Offset: 0x0008AE70
	protected override void LateUpdateReplicated()
	{
		base.LateUpdateReplicated();
		if (this.itemState.HasFlag(TransferrableObject.ItemStates.State0))
		{
			if (!this._objectToDeploy.activeSelf)
			{
				this.DeployChild();
				return;
			}
		}
		else if (this._objectToDeploy.activeSelf)
		{
			this.ReturnChild();
		}
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x0008CCC4 File Offset: 0x0008AEC4
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		if (!base.OnRelease(zoneReleased, releasingHand))
		{
			return false;
		}
		if (VRRig.LocalRig != this.ownerRig)
		{
			return false;
		}
		bool isRightHand = releasingHand == EquipmentInteractor.instance.rightHand;
		GorillaVelocityTracker interactPointVelocityTracker = GTPlayer.Instance.GetInteractPointVelocityTracker(isRightHand);
		Transform transform = base.transform;
		Vector3 vector = transform.TransformPoint(Vector3.zero);
		Quaternion rotation = transform.rotation;
		Vector3 averageVelocity = interactPointVelocityTracker.GetAverageVelocity(true, 0.15f, false);
		this.DeployLocal(vector, rotation, averageVelocity, false);
		this._deploySignal.Raise(ReceiverGroup.Others, BitPackUtils.PackWorldPosForNetwork(vector), BitPackUtils.PackQuaternionForNetwork(rotation), BitPackUtils.PackWorldPosForNetwork(averageVelocity * 100f));
		return true;
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x000350CA File Offset: 0x000332CA
	protected virtual void DeployLocal(Vector3 launchPos, Quaternion launchRot, Vector3 releaseVel, bool isRemote = false)
	{
		this.DisableWhileDeployed(true);
		this._child.Deploy(this, launchPos, launchRot, releaseVel, isRemote);
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x0008CD68 File Offset: 0x0008AF68
	private void DeployRPC(long packedPos, int packedRot, long packedVel, PhotonSignalInfo info)
	{
		if (info.sender != base.OwningPlayer())
		{
			return;
		}
		GorillaNot.IncrementRPCCall(info, "DeployRPC");
		Vector3 vector = BitPackUtils.UnpackWorldPosFromNetwork(packedPos);
		Quaternion launchRot = BitPackUtils.UnpackQuaternionFromNetwork(packedRot);
		Vector3 inVel = BitPackUtils.UnpackWorldPosFromNetwork(packedVel) / 100f;
		float num = 10000f;
		if (!vector.IsValid(num) || !launchRot.IsValid() || !this.m_VRRig.IsPositionInRange(vector, this._maxDeployDistance))
		{
			return;
		}
		this.DeployLocal(vector, launchRot, this.m_VRRig.ClampVelocityRelativeToPlayerSafe(inVel, this._maxThrowVelocity), true);
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x0008CE00 File Offset: 0x0008B000
	private void DisableWhileDeployed(bool active)
	{
		if (this._disabledWhileDeployed.IsNullOrEmpty<GameObject>())
		{
			return;
		}
		for (int i = 0; i < this._disabledWhileDeployed.Length; i++)
		{
			this._disabledWhileDeployed[i].SetActive(!active);
		}
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x000350E4 File Offset: 0x000332E4
	public void DeployChild()
	{
		this.itemState |= TransferrableObject.ItemStates.State0;
		this._objectToDeploy.SetActive(true);
		this.DisableWhileDeployed(true);
		UnityEvent onDeploy = this._onDeploy;
		if (onDeploy == null)
		{
			return;
		}
		onDeploy.Invoke();
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x00035117 File Offset: 0x00033317
	public void ReturnChild()
	{
		this.itemState &= (TransferrableObject.ItemStates)(-2);
		this._objectToDeploy.SetActive(false);
		this.DisableWhileDeployed(false);
		UnityEvent onReturn = this._onReturn;
		if (onReturn == null)
		{
			return;
		}
		onReturn.Invoke();
	}

	// Token: 0x04000A12 RID: 2578
	[SerializeField]
	private GameObject _objectToDeploy;

	// Token: 0x04000A13 RID: 2579
	[SerializeField]
	private DeployedChild _child;

	// Token: 0x04000A14 RID: 2580
	[SerializeField]
	private GameObject[] _disabledWhileDeployed = new GameObject[0];

	// Token: 0x04000A15 RID: 2581
	[SerializeField]
	private SoundBankPlayer deploySound;

	// Token: 0x04000A16 RID: 2582
	[SerializeField]
	private PhotonSignal<long, int, long> _deploySignal = "_deploySignal";

	// Token: 0x04000A17 RID: 2583
	[SerializeField]
	private float _maxDeployDistance = 4f;

	// Token: 0x04000A18 RID: 2584
	[SerializeField]
	private float _maxThrowVelocity = 50f;

	// Token: 0x04000A19 RID: 2585
	[SerializeField]
	private UnityEvent _onDeploy;

	// Token: 0x04000A1A RID: 2586
	[SerializeField]
	private UnityEvent _onReturn;

	// Token: 0x04000A1B RID: 2587
	[SerializeField]
	private Component[] _rigAwareObjects = new Component[0];

	// Token: 0x04000A1C RID: 2588
	private VRRig m_VRRig;
}
