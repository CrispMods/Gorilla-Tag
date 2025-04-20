using System;
using ExitGames.Client.Photon;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x0200044D RID: 1101
public class PaperPlaneThrowable : TransferrableObject
{
	// Token: 0x06001B21 RID: 6945 RVA: 0x000D8BC8 File Offset: 0x000D6DC8
	private void OnLaunchRPC(int sender, int receiver, object[] args, PhotonMessageInfoWrapped info)
	{
		if (info.senderID != this.ownerRig.creator.ActorNumber)
		{
			return;
		}
		GorillaNot.IncrementRPCCall(info, "OnLaunchRPC");
		if (sender != receiver)
		{
			return;
		}
		if (!this)
		{
			return;
		}
		int num = PaperPlaneThrowable.FetchViewID(this);
		int num2 = (int)args[0];
		if (num == -1)
		{
			return;
		}
		if (num2 == -1)
		{
			return;
		}
		if (num != num2)
		{
			return;
		}
		int num3 = (int)args[1];
		int throwableId = this.GetThrowableId();
		if (num3 != throwableId)
		{
			return;
		}
		Vector3 launchPos = (Vector3)args[2];
		Quaternion launchRot = (Quaternion)args[3];
		Vector3 releaseVel = (Vector3)args[4];
		float num4 = 10000f;
		if (launchPos.IsValid(num4) && launchRot.IsValid())
		{
			float num5 = 10000f;
			if (releaseVel.IsValid(num5) && !this._renderer.forceRenderingOff)
			{
				this.LaunchProjectileLocal(launchPos, launchRot, releaseVel);
				return;
			}
		}
	}

	// Token: 0x06001B22 RID: 6946 RVA: 0x000426B3 File Offset: 0x000408B3
	internal override void OnEnable()
	{
		PhotonNetwork.NetworkingClient.EventReceived += this.OnPhotonEvent;
		this._lastWorldPos = base.transform.position;
		this._renderer.forceRenderingOff = false;
		base.OnEnable();
	}

	// Token: 0x06001B23 RID: 6947 RVA: 0x000426EE File Offset: 0x000408EE
	internal override void OnDisable()
	{
		PhotonNetwork.NetworkingClient.EventReceived -= this.OnPhotonEvent;
		base.OnDisable();
	}

	// Token: 0x06001B24 RID: 6948 RVA: 0x000D8CA0 File Offset: 0x000D6EA0
	private void OnPhotonEvent(EventData evData)
	{
		if (evData.Code != 176)
		{
			return;
		}
		object[] array = (object[])evData.CustomData;
		object obj = array[0];
		if (!(obj is int))
		{
			return;
		}
		int num = (int)obj;
		if (num != PaperPlaneThrowable.kProjectileEvent)
		{
			return;
		}
		NetPlayer player = NetworkSystem.Instance.GetPlayer(evData.Sender);
		NetPlayer netPlayer = base.OwningPlayer();
		if (player != netPlayer)
		{
			return;
		}
		GorillaNot.IncrementRPCCall(new PhotonMessageInfo(netPlayer.GetPlayerRef(), PhotonNetwork.ServerTimestamp, null), "OnPhotonEvent");
		if (!this.m_spamCheck.CheckCallTime(Time.unscaledTime))
		{
			return;
		}
		TransferrableObject.PositionState positionState = (TransferrableObject.PositionState)array[1];
		Vector3 vector = (Vector3)array[2];
		Quaternion launchRot = (Quaternion)array[3];
		Vector3 releaseVel = (Vector3)array[4];
		TransferrableObject.PositionState positionState2 = positionState;
		if (positionState2 != TransferrableObject.PositionState.InLeftHand)
		{
			if (positionState2 != TransferrableObject.PositionState.InRightHand)
			{
				goto IL_CE;
			}
			if (base.InRightHand())
			{
				goto IL_CE;
			}
		}
		else if (base.InLeftHand())
		{
			goto IL_CE;
		}
		return;
		IL_CE:
		float num2 = 10000f;
		if (vector.IsValid(num2) && launchRot.IsValid())
		{
			float num3 = 10000f;
			if (releaseVel.IsValid(num3) && !this._renderer.forceRenderingOff && !base.myOnlineRig.IsNull() && base.myOnlineRig.IsPositionInRange(vector, 4f))
			{
				this.LaunchProjectileLocal(vector, launchRot, releaseVel);
				return;
			}
		}
	}

	// Token: 0x06001B25 RID: 6949 RVA: 0x0004270C File Offset: 0x0004090C
	protected override void Start()
	{
		base.Start();
		if (PaperPlaneThrowable._playerView == null)
		{
			PaperPlaneThrowable._playerView = Camera.main;
		}
	}

	// Token: 0x06001B26 RID: 6950 RVA: 0x0004272B File Offset: 0x0004092B
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		if (this._renderer.forceRenderingOff)
		{
			return;
		}
		base.OnGrab(pointGrabbed, grabbingHand);
	}

	// Token: 0x06001B27 RID: 6951 RVA: 0x000D8DE4 File Offset: 0x000D6FE4
	private static int FetchViewID(PaperPlaneThrowable ppt)
	{
		NetPlayer netPlayer = (ppt.myOnlineRig != null) ? ppt.myOnlineRig.creator : ((ppt.myRig != null) ? ((ppt.myRig.creator != null) ? ppt.myRig.creator : NetworkSystem.Instance.LocalPlayer) : null);
		if (netPlayer == null)
		{
			return -1;
		}
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(netPlayer, out rigContainer))
		{
			return -1;
		}
		if (rigContainer.Rig.netView == null)
		{
			return -1;
		}
		return rigContainer.Rig.netView.ViewID;
	}

	// Token: 0x06001B28 RID: 6952 RVA: 0x000D8E80 File Offset: 0x000D7080
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		TransferrableObject.PositionState currentState = this.currentState;
		if (!base.OnRelease(zoneReleased, releasingHand))
		{
			return false;
		}
		if (VRRigCache.Instance.localRig.Rig != this.ownerRig)
		{
			return false;
		}
		if (this._renderer.forceRenderingOff)
		{
			return false;
		}
		bool isRightHand = releasingHand == EquipmentInteractor.instance.rightHand;
		GorillaVelocityTracker interactPointVelocityTracker = GTPlayer.Instance.GetInteractPointVelocityTracker(isRightHand);
		Vector3 vector = base.transform.TransformPoint(Vector3.zero);
		Quaternion rotation = base.transform.rotation;
		Vector3 averageVelocity = interactPointVelocityTracker.GetAverageVelocity(true, 0.15f, false);
		PaperPlaneThrowable.FetchViewID(this);
		this.GetThrowableId();
		this.LaunchProjectileLocal(vector, rotation, averageVelocity);
		if (PaperPlaneThrowable.gRaiseOpts == null)
		{
			PaperPlaneThrowable.gRaiseOpts = RaiseEventOptions.Default;
			PaperPlaneThrowable.gRaiseOpts.Receivers = ReceiverGroup.Others;
		}
		PaperPlaneThrowable.gEventArgs[0] = PaperPlaneThrowable.kProjectileEvent;
		PaperPlaneThrowable.gEventArgs[1] = currentState;
		PaperPlaneThrowable.gEventArgs[2] = vector;
		PaperPlaneThrowable.gEventArgs[3] = rotation;
		PaperPlaneThrowable.gEventArgs[4] = averageVelocity;
		PhotonNetwork.RaiseEvent(176, PaperPlaneThrowable.gEventArgs, PaperPlaneThrowable.gRaiseOpts, SendOptions.SendReliable);
		return true;
	}

	// Token: 0x06001B29 RID: 6953 RVA: 0x000D8FAC File Offset: 0x000D71AC
	private int GetThrowableId()
	{
		int num = this._throwableIdHash.GetValueOrDefault();
		if (this._throwableIdHash == null)
		{
			num = StaticHash.Compute(this._throwableID);
			this._throwableIdHash = new int?(num);
			return num;
		}
		return num;
	}

	// Token: 0x06001B2A RID: 6954 RVA: 0x000D8FF0 File Offset: 0x000D71F0
	private void LaunchProjectileLocal(Vector3 launchPos, Quaternion launchRot, Vector3 releaseVel)
	{
		if (releaseVel.sqrMagnitude <= this.minThrowSpeed * base.transform.lossyScale.z * base.transform.lossyScale.z)
		{
			return;
		}
		GameObject gameObject = ObjectPools.instance.Instantiate(this._projectilePrefab.gameObject, launchPos);
		gameObject.transform.localScale = base.transform.lossyScale;
		PaperPlaneProjectile component = gameObject.GetComponent<PaperPlaneProjectile>();
		component.OnHit += this.OnProjectileHit;
		component.ResetProjectile();
		component.SetVRRig(base.myRig);
		component.Launch(launchPos, launchRot, releaseVel);
		this._renderer.forceRenderingOff = true;
	}

	// Token: 0x06001B2B RID: 6955 RVA: 0x00042743 File Offset: 0x00040943
	private void OnProjectileHit(Vector3 endPoint)
	{
		this._renderer.forceRenderingOff = false;
	}

	// Token: 0x06001B2C RID: 6956 RVA: 0x000D9098 File Offset: 0x000D7298
	protected override void LateUpdateLocal()
	{
		base.LateUpdateLocal();
		Transform transform = base.transform;
		Vector3 position = transform.position;
		this._itemWorldVel = (position - this._lastWorldPos) / Time.deltaTime;
		Quaternion localRotation = transform.localRotation;
		this._itemWorldAngVel = PaperPlaneThrowable.CalcAngularVelocity(this._lastWorldRot, localRotation, Time.deltaTime);
		this._lastWorldRot = localRotation;
		this._lastWorldPos = position;
	}

	// Token: 0x06001B2D RID: 6957 RVA: 0x000D9100 File Offset: 0x000D7300
	private static Vector3 CalcAngularVelocity(Quaternion from, Quaternion to, float dt)
	{
		Vector3 vector = (to * Quaternion.Inverse(from)).eulerAngles;
		if (vector.x > 180f)
		{
			vector.x -= 360f;
		}
		if (vector.y > 180f)
		{
			vector.y -= 360f;
		}
		if (vector.z > 180f)
		{
			vector.z -= 360f;
		}
		vector *= 0.017453292f / dt;
		return vector;
	}

	// Token: 0x06001B2E RID: 6958 RVA: 0x00042751 File Offset: 0x00040951
	public override void DropItem()
	{
		base.DropItem();
	}

	// Token: 0x04001DFA RID: 7674
	[SerializeField]
	private Renderer _renderer;

	// Token: 0x04001DFB RID: 7675
	[SerializeField]
	private GameObject _projectilePrefab;

	// Token: 0x04001DFC RID: 7676
	[SerializeField]
	private float minThrowSpeed;

	// Token: 0x04001DFD RID: 7677
	private static Camera _playerView;

	// Token: 0x04001DFE RID: 7678
	private static PhotonEvent gLaunchRPC;

	// Token: 0x04001DFF RID: 7679
	private CallLimiterWithCooldown m_spamCheck = new CallLimiterWithCooldown(5f, 4, 1f);

	// Token: 0x04001E00 RID: 7680
	private static readonly int kProjectileEvent = StaticHash.Compute("PaperPlaneThrowable".GetStaticHash(), "LaunchProjectileLocal".GetStaticHash());

	// Token: 0x04001E01 RID: 7681
	private static object[] gEventArgs = new object[5];

	// Token: 0x04001E02 RID: 7682
	private static RaiseEventOptions gRaiseOpts;

	// Token: 0x04001E03 RID: 7683
	[SerializeField]
	private string _throwableID;

	// Token: 0x04001E04 RID: 7684
	private int? _throwableIdHash;

	// Token: 0x04001E05 RID: 7685
	[Space]
	private Vector3 _lastWorldPos;

	// Token: 0x04001E06 RID: 7686
	private Quaternion _lastWorldRot;

	// Token: 0x04001E07 RID: 7687
	[Space]
	private Vector3 _itemWorldVel;

	// Token: 0x04001E08 RID: 7688
	private Vector3 _itemWorldAngVel;
}
