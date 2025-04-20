using System;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000155 RID: 341
public class DeployableObject : TransferrableObject
{
	// Token: 0x060008B4 RID: 2228 RVA: 0x000362E1 File Offset: 0x000344E1
	protected override void Awake()
	{
		this._deploySignal.OnSignal += this.DeployRPC;
		base.Awake();
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x0008F584 File Offset: 0x0008D784
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

	// Token: 0x060008B6 RID: 2230 RVA: 0x00036300 File Offset: 0x00034500
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

	// Token: 0x060008B7 RID: 2231 RVA: 0x0003632D File Offset: 0x0003452D
	protected override void OnDestroy()
	{
		this._deploySignal.Dispose();
		base.OnDestroy();
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x0008F5F8 File Offset: 0x0008D7F8
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

	// Token: 0x060008B9 RID: 2233 RVA: 0x0008F64C File Offset: 0x0008D84C
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

	// Token: 0x060008BA RID: 2234 RVA: 0x00036340 File Offset: 0x00034540
	protected virtual void DeployLocal(Vector3 launchPos, Quaternion launchRot, Vector3 releaseVel, bool isRemote = false)
	{
		this.DisableWhileDeployed(true);
		this._child.Deploy(this, launchPos, launchRot, releaseVel, isRemote);
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x0008F6F0 File Offset: 0x0008D8F0
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

	// Token: 0x060008BC RID: 2236 RVA: 0x0008F788 File Offset: 0x0008D988
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

	// Token: 0x060008BD RID: 2237 RVA: 0x0003635A File Offset: 0x0003455A
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

	// Token: 0x060008BE RID: 2238 RVA: 0x0003638D File Offset: 0x0003458D
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

	// Token: 0x04000A54 RID: 2644
	[SerializeField]
	private GameObject _objectToDeploy;

	// Token: 0x04000A55 RID: 2645
	[SerializeField]
	private DeployedChild _child;

	// Token: 0x04000A56 RID: 2646
	[SerializeField]
	private GameObject[] _disabledWhileDeployed = new GameObject[0];

	// Token: 0x04000A57 RID: 2647
	[SerializeField]
	private SoundBankPlayer deploySound;

	// Token: 0x04000A58 RID: 2648
	[SerializeField]
	private PhotonSignal<long, int, long> _deploySignal = "_deploySignal";

	// Token: 0x04000A59 RID: 2649
	[SerializeField]
	private float _maxDeployDistance = 4f;

	// Token: 0x04000A5A RID: 2650
	[SerializeField]
	private float _maxThrowVelocity = 50f;

	// Token: 0x04000A5B RID: 2651
	[SerializeField]
	private UnityEvent _onDeploy;

	// Token: 0x04000A5C RID: 2652
	[SerializeField]
	private UnityEvent _onReturn;

	// Token: 0x04000A5D RID: 2653
	[SerializeField]
	private Component[] _rigAwareObjects = new Component[0];

	// Token: 0x04000A5E RID: 2654
	private VRRig m_VRRig;
}
