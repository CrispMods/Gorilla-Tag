using System;
using System.Collections.Generic;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x02000033 RID: 51
public class CrittersActor : MonoBehaviour
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x060000C2 RID: 194 RVA: 0x0006A930 File Offset: 0x00068B30
	// (remove) Token: 0x060000C3 RID: 195 RVA: 0x0006A968 File Offset: 0x00068B68
	public event Action<CrittersActor> OnGrabbedChild;

	// Token: 0x060000C4 RID: 196 RVA: 0x0006A9A0 File Offset: 0x00068BA0
	public virtual void UpdateAverageSpeed()
	{
		this.averageSpeed[this.averageSpeedIndex] = (base.transform.position - this.lastPosition).magnitude;
		this.averageSpeedIndex++;
		this.averageSpeedIndex %= 6;
		this.lastPosition = base.transform.position;
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x060000C5 RID: 197 RVA: 0x00030D1A File Offset: 0x0002EF1A
	public float GetAverageSpeed
	{
		get
		{
			return (this.averageSpeed[0] + this.averageSpeed[1] + this.averageSpeed[2] + this.averageSpeed[3] + this.averageSpeed[4] + this.averageSpeed[5]) / 6f;
		}
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00030D57 File Offset: 0x0002EF57
	protected virtual void Awake()
	{
		this._isOnPlayerDefault = this.isOnPlayer;
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x0006AA08 File Offset: 0x00068C08
	public virtual void Initialize()
	{
		if (this.defaultParentTransform == null)
		{
			this.SetDefaultParent(base.transform.parent);
		}
		if (this.rb == null)
		{
			this.rb = base.GetComponent<Rigidbody>();
		}
		if (this.rb == null)
		{
			Debug.LogError("I should have a rigidbody, but I don't!", base.gameObject);
		}
		this.wasEnabled = false;
		this.isEnabled = true;
		this.TogglePhysics(this.usesRB);
		if (!this.rb.isKinematic)
		{
			this.rb.velocity = Vector3.zero;
			this.rb.angularVelocity = Vector3.zero;
		}
		if (this.resetPhysicsOnSpawn)
		{
			this.rb.velocity = Vector3.zero;
			this.rb.angularVelocity = Vector3.zero;
			this.lastImpulseVelocity = Vector3.zero;
		}
		if (this.subObjectIndex >= 0 && this.subObjectIndex < this.subObjects.Length)
		{
			for (int i = 0; i < this.subObjects.Length; i++)
			{
				this.subObjects[i].SetActive(i == this.subObjectIndex);
			}
		}
		this.colliders = new Collider[50];
		if (this.preventDespawnUntilGrabbed)
		{
			this.isDespawnBlocked = true;
			this.despawnTime = 0.0;
		}
		else
		{
			this.isDespawnBlocked = false;
			this.despawnTime = (double)this.despawnDelay + (PhotonNetwork.InRoom ? PhotonNetwork.Time : ((double)Time.time));
		}
		this.rb.includeLayers = 0;
		this.rb.excludeLayers = CrittersManager.instance.containerLayer;
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00030D65 File Offset: 0x0002EF65
	public virtual void OnEnable()
	{
		CrittersManager.RegisterActor(this);
		this.Initialize();
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00030D73 File Offset: 0x0002EF73
	public virtual void OnDisable()
	{
		this.CleanupActor();
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00030D7B File Offset: 0x0002EF7B
	public virtual string GetActorSubtype()
	{
		if (this.subObjectIndex >= 0 && this.subObjectIndex < this.subObjects.Length)
		{
			return this.subObjects[this.subObjectIndex].name;
		}
		return base.name;
	}

	// Token: 0x060000CB RID: 203 RVA: 0x0006ABA8 File Offset: 0x00068DA8
	protected virtual void CleanupActor()
	{
		CrittersManager.DeregisterActor(this);
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
		for (int i = 0; i < this.subObjects.Length; i++)
		{
			if (this.subObjects[i].activeSelf)
			{
				this.subObjects[i].transform.localRotation = Quaternion.identity;
				this.subObjects[i].transform.localPosition = Vector3.zero;
				this.subObjects[i].SetActive(false);
			}
		}
		this.ReleasedEvent.Invoke(this);
		this.ReleasedEvent.RemoveAllListeners();
		this.isEnabled = false;
		this.wasEnabled = true;
		this.isOnPlayer = this._isOnPlayerDefault;
		this.rigPlayerId = -1;
		this.rigIndex = -1;
		this.despawnTime = 0.0;
		this.isDespawnBlocked = false;
		this.rb.isKinematic = false;
		if (this.parentActorId >= 0)
		{
			this.AttemptRemoveStoredObjectCollider(this.parentActorId, false);
		}
		this.parentActorId = -1;
		this.parentActor = null;
		this.lastParentActorId = -1;
		this.isGrabDisabled = false;
		this.lastGrabbedPlayer = -1;
		this.lastImpulsePosition = Vector3.zero;
		this.lastImpulseVelocity = Vector3.zero;
		this.lastImpulseQuaternion = Quaternion.identity;
		this.lastImpulseTime = -1.0;
		this.localLastImpulse = -1.0;
		this.updatedSinceLastFrame = false;
		this.localCanStore = false;
	}

	// Token: 0x060000CC RID: 204 RVA: 0x0006AD1C File Offset: 0x00068F1C
	public virtual bool ProcessLocal()
	{
		this.updatedSinceLastFrame |= (this.isEnabled != this.wasEnabled || this.parentActorId != this.lastParentActorId);
		this.lastParentActorId = this.parentActorId;
		this.wasEnabled = this.isEnabled;
		return this.updatedSinceLastFrame;
	}

	// Token: 0x060000CD RID: 205 RVA: 0x0006AD78 File Offset: 0x00068F78
	public virtual void ProcessRemote()
	{
		bool flag = this.forceUpdate;
		this.forceUpdate = false;
		if (base.gameObject.activeSelf != this.isEnabled)
		{
			base.gameObject.SetActive(this.isEnabled);
		}
		if (!this.isEnabled)
		{
			return;
		}
		bool flag2 = this.lastParentActorId == this.parentActorId || this.isOnPlayer || this.isSceneActor;
		bool flag3 = this.lastImpulseTime == this.localLastImpulse;
		if (flag2 && flag3 && !flag)
		{
			return;
		}
		if (!flag2)
		{
			if (this.lastParentActorId >= 0)
			{
				this.AttemptRemoveStoredObjectCollider(this.lastParentActorId, true);
			}
			this.lastParentActorId = this.parentActorId;
			if (this.parentActorId >= 0)
			{
				CrittersActor crittersActor;
				if (!CrittersManager.instance.actorById.TryGetValue(this.parentActorId, out crittersActor))
				{
					return;
				}
				this.parentActor = crittersActor.transform;
				base.transform.SetParent(this.parentActor, true);
				this.SetImpulse();
				if (crittersActor is CrittersBag)
				{
					((CrittersBag)crittersActor).AddStoredObjectCollider(this);
				}
				if (crittersActor.isOnPlayer)
				{
					this.lastGrabbedPlayer = crittersActor.rigPlayerId;
				}
				crittersActor.RemoteGrabbed(this);
				return;
			}
			else if (this.parentActorId == -1)
			{
				this.parentActor = null;
				this.SetTransformToDefaultParent(false);
				this.HandleRemoteReleased();
				this.SetImpulse();
				return;
			}
		}
		else
		{
			this.SetImpulse();
		}
	}

	// Token: 0x060000CE RID: 206 RVA: 0x0006AEC4 File Offset: 0x000690C4
	public virtual void SetImpulse()
	{
		if (this.isOnPlayer || this.isSceneActor)
		{
			return;
		}
		this.localLastImpulse = this.lastImpulseTime;
		this.MoveActor(this.lastImpulsePosition, this.lastImpulseQuaternion, this.parentActorId >= 0, false, true);
		this.TogglePhysics(this.usesRB && this.parentActorId == -1);
		if (!this.rb.isKinematic)
		{
			this.rb.velocity = this.lastImpulseVelocity;
			this.rb.angularVelocity = this.lastImpulseAngularVelocity;
		}
	}

	// Token: 0x060000CF RID: 207 RVA: 0x0006AF58 File Offset: 0x00069158
	public virtual void TogglePhysics(bool enable)
	{
		if (enable)
		{
			this.rb.isKinematic = false;
			this.rb.interpolation = RigidbodyInterpolation.Interpolate;
			this.rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
			return;
		}
		this.rb.isKinematic = true;
		this.rb.interpolation = RigidbodyInterpolation.None;
		this.rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x0006AFB4 File Offset: 0x000691B4
	public void AddPlayerCrittersActorDataToList(ref List<object> objList)
	{
		objList.Add(this.actorId);
		objList.Add(this.isOnPlayer);
		objList.Add(this.rigPlayerId);
		objList.Add(this.rigIndex);
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x0006B00C File Offset: 0x0006920C
	public virtual int AddActorDataToList(ref List<object> objList)
	{
		objList.Add(this.actorId);
		objList.Add(this.lastImpulseTime);
		objList.Add(this.lastImpulsePosition);
		objList.Add(this.lastImpulseVelocity);
		objList.Add(this.lastImpulseAngularVelocity);
		objList.Add(this.lastImpulseQuaternion);
		objList.Add(this.parentActorId);
		objList.Add(this.isEnabled);
		objList.Add(this.subObjectIndex);
		return this.BaseActorDataLength();
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00030DAF File Offset: 0x0002EFAF
	public int BaseActorDataLength()
	{
		return 9;
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00030DAF File Offset: 0x0002EFAF
	public virtual int TotalActorDataLength()
	{
		return 9;
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x0006B0C4 File Offset: 0x000692C4
	public virtual int UpdateFromRPC(object[] data, int startingIndex)
	{
		double value;
		if (!CrittersManager.ValidateDataType<double>(data[startingIndex + 1], out value))
		{
			return this.BaseActorDataLength();
		}
		Vector3 vector;
		if (!CrittersManager.ValidateDataType<Vector3>(data[startingIndex + 2], out vector))
		{
			return this.BaseActorDataLength();
		}
		Vector3 vector2;
		if (!CrittersManager.ValidateDataType<Vector3>(data[startingIndex + 3], out vector2))
		{
			return this.BaseActorDataLength();
		}
		Vector3 vector3;
		if (!CrittersManager.ValidateDataType<Vector3>(data[startingIndex + 4], out vector3))
		{
			return this.BaseActorDataLength();
		}
		Quaternion quaternion;
		if (!CrittersManager.ValidateDataType<Quaternion>(data[startingIndex + 5], out quaternion))
		{
			return this.BaseActorDataLength();
		}
		int num;
		if (!CrittersManager.ValidateDataType<int>(data[startingIndex + 6], out num))
		{
			return this.BaseActorDataLength();
		}
		bool flag;
		if (!CrittersManager.ValidateDataType<bool>(data[startingIndex + 7], out flag))
		{
			return this.BaseActorDataLength();
		}
		int num2;
		if (!CrittersManager.ValidateDataType<int>(data[startingIndex + 8], out num2))
		{
			return this.BaseActorDataLength();
		}
		this.lastImpulseTime = value.GetFinite();
		ref this.lastImpulsePosition.SetValueSafe(vector);
		ref this.lastImpulseVelocity.SetValueSafe(vector2);
		ref this.lastImpulseAngularVelocity.SetValueSafe(vector3);
		ref this.lastImpulseQuaternion.SetValueSafe(quaternion);
		this.parentActorId = num;
		this.isEnabled = flag;
		this.subObjectIndex = num2;
		this.forceUpdate = true;
		if (this.isEnabled)
		{
			base.gameObject.SetActive(true);
		}
		for (int i = 0; i < this.subObjects.Length; i++)
		{
			this.subObjects[i].SetActive(i == this.subObjectIndex);
		}
		return this.BaseActorDataLength();
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x0006B224 File Offset: 0x00069424
	public int UpdatePlayerCrittersActorFromRPC(object[] data, int startingIndex)
	{
		bool flag;
		if (!CrittersManager.ValidateDataType<bool>(data[startingIndex + 1], out flag))
		{
			return 4;
		}
		int num;
		if (!CrittersManager.ValidateDataType<int>(data[startingIndex + 2], out num))
		{
			return 4;
		}
		int num2;
		if (!CrittersManager.ValidateDataType<int>(data[startingIndex + 3], out num2))
		{
			return 4;
		}
		this.isOnPlayer = flag;
		this.rigPlayerId = num;
		this.rigIndex = num2;
		if (this.rigPlayerId == -1 && CrittersManager.instance.guard.currentOwner != null)
		{
			this.rigPlayerId = CrittersManager.instance.guard.currentOwner.ActorNumber;
		}
		this.PlacePlayerCrittersActor();
		return 4;
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x0006B2B8 File Offset: 0x000694B8
	public virtual bool UpdateSpecificActor(PhotonStream stream)
	{
		double num;
		Vector3 vector;
		Vector3 vector2;
		Vector3 vector3;
		Quaternion quaternion;
		int num2;
		bool flag;
		int num3;
		if (!(CrittersManager.ValidateDataType<double>(stream.ReceiveNext(), out num) & CrittersManager.ValidateDataType<Vector3>(stream.ReceiveNext(), out vector) & CrittersManager.ValidateDataType<Vector3>(stream.ReceiveNext(), out vector2) & CrittersManager.ValidateDataType<Vector3>(stream.ReceiveNext(), out vector3) & CrittersManager.ValidateDataType<Quaternion>(stream.ReceiveNext(), out quaternion) & CrittersManager.ValidateDataType<int>(stream.ReceiveNext(), out num2) & CrittersManager.ValidateDataType<bool>(stream.ReceiveNext(), out flag) & CrittersManager.ValidateDataType<int>(stream.ReceiveNext(), out num3)))
		{
			return false;
		}
		float num4 = 10000f;
		if (vector.IsValid(num4))
		{
			ref this.lastImpulsePosition.SetValueSafe(vector);
		}
		num4 = 10000f;
		if (vector2.IsValid(num4))
		{
			ref this.lastImpulseVelocity.SetValueSafe(vector2);
		}
		if (quaternion.IsValid())
		{
			ref this.lastImpulseQuaternion.SetValueSafe(quaternion);
		}
		num4 = 10000f;
		if (vector3.IsValid(num4))
		{
			ref this.lastImpulseAngularVelocity.SetValueSafe(vector3);
		}
		if (num2 >= -1 && num2 < CrittersManager.instance.universalActorId)
		{
			this.parentActorId = num2;
		}
		if (num3 < this.subObjects.Length)
		{
			this.subObjectIndex = num3;
		}
		this.isEnabled = flag;
		this.lastImpulseTime = num;
		if (this.isEnabled != base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(this.isEnabled);
		}
		if (this.isEnabled && this.subObjectIndex >= 0)
		{
			this.subObjects[this.subObjectIndex].SetActive(true);
		}
		else if (!this.isEnabled && this.subObjectIndex >= 0)
		{
			this.subObjects[this.subObjectIndex].SetActive(false);
		}
		return true;
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x0006B45C File Offset: 0x0006965C
	public virtual void SendDataByCrittersActorType(PhotonStream stream)
	{
		stream.SendNext(this.actorId);
		stream.SendNext(this.lastImpulseTime);
		stream.SendNext(this.lastImpulsePosition);
		stream.SendNext(this.lastImpulseVelocity);
		stream.SendNext(this.lastImpulseAngularVelocity);
		stream.SendNext(this.lastImpulseQuaternion);
		stream.SendNext(this.parentActorId);
		stream.SendNext(this.isEnabled);
		stream.SendNext(this.subObjectIndex);
		this.updatedSinceLastFrame = false;
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x00030DB3 File Offset: 0x0002EFB3
	public virtual void OnHover(bool isLeft)
	{
		GorillaTagger.Instance.StartVibration(isLeft, GorillaTagger.Instance.tapHapticStrength / 8f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x00030DE0 File Offset: 0x0002EFE0
	public virtual bool CanBeGrabbed(CrittersActor grabbedBy)
	{
		return !this.isGrabDisabled && this.grabbable;
	}

	// Token: 0x060000DA RID: 218 RVA: 0x0006B50C File Offset: 0x0006970C
	public static CrittersActor GetRootActor(int actorId)
	{
		CrittersActor crittersActor;
		if (!CrittersManager.instance.actorById.TryGetValue(actorId, out crittersActor))
		{
			return null;
		}
		if (crittersActor.parentActorId > -1)
		{
			return CrittersActor.GetRootActor(crittersActor.parentActorId);
		}
		return crittersActor;
	}

	// Token: 0x060000DB RID: 219 RVA: 0x0006B548 File Offset: 0x00069748
	public static CrittersActor GetParentActor(int actorId)
	{
		CrittersActor result;
		if (CrittersManager.instance.actorById.TryGetValue(actorId, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x060000DC RID: 220 RVA: 0x0006B570 File Offset: 0x00069770
	public bool AllowGrabbingActor(CrittersActor grabbedBy)
	{
		if (this.parentActorId == -1)
		{
			return true;
		}
		if (grabbedBy.crittersActorType != CrittersActor.CrittersActorType.Grabber)
		{
			return true;
		}
		CrittersActor rootActor = CrittersActor.GetRootActor(grabbedBy.actorId);
		CrittersActor crittersActor;
		if (CrittersManager.instance.actorById.TryGetValue(this.parentActorId, out crittersActor))
		{
			if (crittersActor.crittersActorType == CrittersActor.CrittersActorType.Bag)
			{
				if (!CrittersManager.instance.allowGrabbingFromBags)
				{
					CrittersActor rootActor2 = CrittersActor.GetRootActor(this.actorId);
					Debug.Log(string.Format("Grieffing - FromBag {0} == {1} || {2} == -1 || {3} == -1  - ", new object[]
					{
						rootActor2.rigPlayerId,
						rootActor.rigPlayerId,
						crittersActor.parentActorId,
						rootActor.rigPlayerId
					}) + string.Format(" {0}", rootActor2.rigPlayerId == rootActor.rigPlayerId || rootActor2.rigPlayerId == -1 || rootActor.rigPlayerId == -1));
					return rootActor2.rigPlayerId == rootActor.rigPlayerId || rootActor2.rigPlayerId == -1 || rootActor.rigPlayerId == -1;
				}
			}
			else if (crittersActor.crittersActorType == CrittersActor.CrittersActorType.BodyAttachPoint)
			{
				if (!CrittersManager.instance.allowGrabbingEntireBag)
				{
					Debug.Log(string.Format("Grieffing - EntireBag {0} == {1} || {2} == -1 || {3} == -1  -  {4}", new object[]
					{
						crittersActor.rigPlayerId,
						rootActor.rigPlayerId,
						crittersActor.parentActorId,
						rootActor.rigPlayerId,
						crittersActor.rigPlayerId == rootActor.rigPlayerId || crittersActor.rigPlayerId == -1 || rootActor.rigPlayerId == -1
					}));
					return crittersActor.rigPlayerId == rootActor.rigPlayerId || crittersActor.rigPlayerId == -1 || rootActor.rigPlayerId == -1;
				}
			}
			else if (crittersActor.crittersActorType == CrittersActor.CrittersActorType.Grabber && !CrittersManager.instance.allowGrabbingOutOfHands)
			{
				Debug.Log(string.Format("Grieffing - InHand {0} == {1} || {2} == -1 || {3} == -1  -  {4}", new object[]
				{
					crittersActor.rigPlayerId,
					rootActor.rigPlayerId,
					crittersActor.parentActorId,
					rootActor.rigPlayerId,
					crittersActor.rigPlayerId == rootActor.rigPlayerId || crittersActor.rigPlayerId == -1 || rootActor.rigPlayerId == -1
				}));
				return crittersActor.rigPlayerId == rootActor.rigPlayerId || crittersActor.rigPlayerId == -1 || rootActor.rigPlayerId == -1;
			}
		}
		return true;
	}

	// Token: 0x060000DD RID: 221 RVA: 0x0006B804 File Offset: 0x00069A04
	public bool IsCurrentlyAttachedToBag()
	{
		CrittersActor crittersActor;
		return CrittersManager.instance.actorById.TryGetValue(this.parentActorId, out crittersActor) && crittersActor.crittersActorType == CrittersActor.CrittersActorType.Bag;
	}

	// Token: 0x060000DE RID: 222 RVA: 0x00030DF2 File Offset: 0x0002EFF2
	public void SetTransformToDefaultParent(bool resetOrigin = false)
	{
		if (this.IsNull())
		{
			return;
		}
		base.transform.SetParent(this.defaultParentTransform, true);
		if (resetOrigin)
		{
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
		}
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00030E32 File Offset: 0x0002F032
	public void SetDefaultParent(Transform newDefaultParent)
	{
		this.defaultParentTransform = newDefaultParent;
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x00030E3B File Offset: 0x0002F03B
	protected virtual void RemoteGrabbed(CrittersActor actor)
	{
		Action<CrittersActor> onGrabbedChild = this.OnGrabbedChild;
		if (onGrabbedChild != null)
		{
			onGrabbedChild(actor);
		}
		actor.RemoteGrabbedBy(this);
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x00030E56 File Offset: 0x0002F056
	protected virtual void RemoteGrabbedBy(CrittersActor grabbingActor)
	{
		this.GlobalGrabbedBy(grabbingActor);
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x0006B838 File Offset: 0x00069A38
	public virtual void GrabbedBy(CrittersActor grabbingActor, bool positionOverride = false, Quaternion localRotation = default(Quaternion), Vector3 localOffset = default(Vector3), bool disableGrabbing = false)
	{
		this.GlobalGrabbedBy(grabbingActor);
		if (this.parentActorId >= 0)
		{
			this.AttemptRemoveStoredObjectCollider(this.parentActorId, true);
		}
		this.isGrabDisabled = disableGrabbing;
		this.parentActorId = grabbingActor.actorId;
		if (grabbingActor.isOnPlayer)
		{
			this.lastGrabbedPlayer = grabbingActor.rigPlayerId;
		}
		base.transform.SetParent(grabbingActor.transform, true);
		if (localRotation.w == 0f && localRotation.x == 0f && localRotation.y == 0f && localRotation.z == 0f)
		{
			localRotation = Quaternion.identity;
		}
		if (positionOverride)
		{
			this.MoveActor(localOffset, localRotation, true, false, true);
		}
		this.UpdateImpulses(true, true);
		this.rb.isKinematic = true;
		this.rb.interpolation = RigidbodyInterpolation.None;
		this.rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
		if (CrittersManager.instance.IsNotNull() && PhotonNetwork.InRoom && !CrittersManager.instance.LocalAuthority())
		{
			CrittersManager.instance.SendRPC("RemoteCrittersActorGrabbedby", CrittersManager.instance.guard.currentOwner, new object[]
			{
				this.actorId,
				grabbingActor.actorId,
				this.lastImpulseQuaternion,
				this.lastImpulsePosition,
				this.isGrabDisabled
			});
		}
		Action<CrittersActor> onGrabbedChild = grabbingActor.OnGrabbedChild;
		if (onGrabbedChild != null)
		{
			onGrabbedChild(this);
		}
		this.AttemptAddStoredObjectCollider(grabbingActor);
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void GlobalGrabbedBy(CrittersActor grabbingActor)
	{
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x00030E5F File Offset: 0x0002F05F
	protected virtual void HandleRemoteReleased()
	{
		this.DisconnectJoint();
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x0006B9C0 File Offset: 0x00069BC0
	public virtual void Released(bool keepWorldPosition, Quaternion rotation = default(Quaternion), Vector3 position = default(Vector3), Vector3 impulseVelocity = default(Vector3), Vector3 impulseAngularVelocity = default(Vector3))
	{
		if (this.parentActorId >= 0)
		{
			this.AttemptRemoveStoredObjectCollider(this.parentActorId, true);
		}
		this.isGrabDisabled = false;
		this.parentActorId = -1;
		if (this.equipmentStorable)
		{
			this.localCanStore = false;
		}
		this.DisconnectJoint();
		this.SetTransformToDefaultParent(false);
		if (rotation.w == 0f && rotation.x == 0f && rotation.y == 0f && rotation.z == 0f)
		{
			rotation = Quaternion.identity;
		}
		if (!keepWorldPosition)
		{
			if (position.sqrMagnitude > 1f)
			{
				this.MoveActor(position, rotation, false, false, true);
			}
			else
			{
				GTDev.Log<string>(string.Format("Release called for: {0}, but sent in suspicious position data: {1}", base.name, position), null);
			}
		}
		if (this.despawnWhenIdle)
		{
			if (this.preventDespawnUntilGrabbed)
			{
				this.isDespawnBlocked = false;
			}
			this.despawnTime = (double)this.despawnDelay + (PhotonNetwork.InRoom ? PhotonNetwork.Time : ((double)Time.time));
		}
		this.UpdateImpulses(false, false);
		this.SetImpulseVelocity(impulseVelocity, impulseAngularVelocity);
		this.TogglePhysics(this.usesRB);
		this.SetImpulse();
		if (CrittersManager.instance.IsNotNull() && PhotonNetwork.InRoom && !CrittersManager.instance.LocalAuthority())
		{
			CrittersManager.instance.SendRPC("RemoteCritterActorReleased", CrittersManager.instance.guard.currentOwner, new object[]
			{
				this.actorId,
				false,
				rotation,
				position,
				impulseVelocity,
				impulseAngularVelocity
			});
		}
		this.ReleasedEvent.Invoke(this);
		this.ReleasedEvent.RemoveAllListeners();
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x0006BB80 File Offset: 0x00069D80
	public void PlacePlayerCrittersActor()
	{
		if (this.rigIndex == -1)
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
			}
			return;
		}
		RigContainer rigContainer;
		CrittersRigActorSetup crittersRigActorSetup;
		if (!VRRigCache.Instance.TryGetVrrig(NetworkSystem.Instance.GetPlayer(this.rigPlayerId), out rigContainer) || !CrittersManager.instance.rigSetupByRig.TryGetValue(rigContainer.Rig, out crittersRigActorSetup))
		{
			rigContainer != null;
			return;
		}
		if (this.rigPlayerId == NetworkSystem.Instance.LocalPlayer.ActorNumber && !CrittersManager.instance.rigSetupByRig.TryGetValue(GorillaTagger.Instance.offlineVRRig, out crittersRigActorSetup))
		{
			return;
		}
		if (this.rigIndex < 0 || this.rigIndex >= crittersRigActorSetup.rigActors.Length)
		{
			return;
		}
		base.gameObject.SetActive(true);
		base.transform.parent = crittersRigActorSetup.rigActors[this.rigIndex].location;
		this.MoveActor(Vector3.zero, Quaternion.identity, true, true, true);
		crittersRigActorSetup.rigActors[this.rigIndex] = new CrittersRigActorSetup.RigActor
		{
			actorSet = this,
			location = crittersRigActorSetup.rigActors[this.rigIndex].location,
			type = crittersRigActorSetup.rigActors[this.rigIndex].type,
			subIndex = crittersRigActorSetup.rigActors[this.rigIndex].subIndex
		};
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x0006BCFC File Offset: 0x00069EFC
	public void MoveActor(Vector3 position, Quaternion rotation, bool local = false, bool updateImpulses = true, bool updateImpulseTime = true)
	{
		bool isKinematic = this.rb.isKinematic;
		this.TogglePhysics(false);
		if (local)
		{
			base.transform.localRotation = rotation;
			base.transform.localPosition = position;
			if (updateImpulses)
			{
				this.UpdateImpulses(true, updateImpulseTime);
			}
		}
		else
		{
			base.transform.rotation = rotation.normalized;
			base.transform.position = position;
			if (updateImpulses)
			{
				this.UpdateImpulses(false, updateImpulseTime);
			}
		}
		if (!isKinematic)
		{
			this.TogglePhysics(true);
		}
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x0006BD7C File Offset: 0x00069F7C
	public void UpdateImpulses(bool local = false, bool updateTime = false)
	{
		if (local)
		{
			this.lastImpulsePosition = base.transform.localPosition;
			this.lastImpulseQuaternion = base.transform.localRotation;
		}
		else
		{
			this.lastImpulsePosition = base.transform.position;
			this.lastImpulseQuaternion = base.transform.rotation;
		}
		if (updateTime)
		{
			this.SetImpulseTime();
		}
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x00030E67 File Offset: 0x0002F067
	public void UpdateImpulseVelocity()
	{
		if (this.rb)
		{
			this.lastImpulseVelocity = this.rb.velocity;
			this.lastImpulseAngularVelocity = this.rb.angularVelocity;
		}
	}

	// Token: 0x060000EA RID: 234 RVA: 0x0006BDDC File Offset: 0x00069FDC
	public virtual void CalculateFear(CrittersPawn critter, float multiplier)
	{
		critter.IncreaseFear(this.FearCurve.Evaluate(Vector3.Distance(critter.transform.position, base.transform.position) / this.maxRangeOfFearAttraction) * multiplier * this.FearAmount * Time.deltaTime, this);
	}

	// Token: 0x060000EB RID: 235 RVA: 0x0006BE2C File Offset: 0x0006A02C
	public virtual void CalculateAttraction(CrittersPawn critter, float multiplier)
	{
		critter.IncreaseAttraction(this.AttractionCurve.Evaluate(Vector3.Distance(critter.transform.position, base.transform.position) / this.maxRangeOfFearAttraction) * multiplier * this.AttractionAmount * Time.deltaTime, this);
	}

	// Token: 0x060000EC RID: 236 RVA: 0x00030E98 File Offset: 0x0002F098
	public void SetImpulseVelocity(Vector3 velocity, Vector3 angularVelocity)
	{
		this.lastImpulseVelocity = velocity;
		this.lastImpulseAngularVelocity = angularVelocity;
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00030EA8 File Offset: 0x0002F0A8
	public void SetImpulseTime()
	{
		this.lastImpulseTime = (PhotonNetwork.InRoom ? PhotonNetwork.Time : ((double)Time.time));
	}

	// Token: 0x060000EE RID: 238 RVA: 0x0006BE7C File Offset: 0x0006A07C
	public virtual bool ShouldDespawn()
	{
		return this.despawnWhenIdle && this.parentActorId == -1 && !this.isDespawnBlocked && 0.0 < this.despawnTime && this.despawnTime <= (PhotonNetwork.InRoom ? PhotonNetwork.Time : ((double)Time.time));
	}

	// Token: 0x060000EF RID: 239 RVA: 0x00030EC4 File Offset: 0x0002F0C4
	public void RemoveDespawnBlock()
	{
		if (this.despawnWhenIdle)
		{
			this.isDespawnBlocked = false;
			this.despawnTime = (double)this.despawnDelay + (PhotonNetwork.InRoom ? PhotonNetwork.Time : ((double)Time.time));
		}
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x0006BED4 File Offset: 0x0006A0D4
	public virtual bool CheckStorable()
	{
		if (!this.localCanStore)
		{
			return false;
		}
		Vector3 b = this.storeCollider.transform.up * MathF.Max(0f, this.storeCollider.height / 2f - this.storeCollider.radius);
		int num = Physics.OverlapCapsuleNonAlloc(this.storeCollider.transform.position + b, this.storeCollider.transform.position - b, this.storeCollider.radius, this.colliders, CrittersManager.instance.containerLayer, QueryTriggerInteraction.Collide);
		bool flag = false;
		CrittersBag crittersBag = null;
		bool flag2 = true;
		CrittersActor crittersActor = null;
		if (this.lastGrabbedPlayer == PhotonNetwork.LocalPlayer.ActorNumber && CrittersManager.instance.actorById.TryGetValue(this.parentActorId, out crittersActor) && crittersActor.GetAverageSpeed > CrittersManager.instance.MaxAttachSpeed)
		{
			return false;
		}
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				CrittersActor component = this.colliders[i].attachedRigidbody.GetComponent<CrittersActor>();
				if (!(component == null) && !(component == this))
				{
					CrittersBag crittersBag2 = component as CrittersBag;
					if (!(crittersBag2 == null))
					{
						if (crittersBag2 == this.lastStoredObject)
						{
							flag = true;
							flag2 = false;
							break;
						}
						if (crittersBag2.IsActorValidStore(this))
						{
							if (crittersBag2.attachableCollider != this.colliders[i] && !this.colliders[i].isTrigger)
							{
								Vector3 vector;
								float num2;
								Physics.ComputePenetration(this.colliders[i], this.colliders[i].transform.position, this.colliders[i].transform.rotation, this.storeCollider, this.storeCollider.transform.position, this.storeCollider.transform.rotation, out vector, out num2);
								if (num2 >= CrittersManager.instance.overlapDistanceMax)
								{
									flag2 = false;
									break;
								}
							}
							else
							{
								crittersBag = crittersBag2;
							}
						}
					}
				}
			}
		}
		if (crittersBag.IsNotNull() && flag2)
		{
			if (crittersActor.IsNotNull())
			{
				CrittersGrabber crittersGrabber = crittersActor as CrittersGrabber;
				if (crittersGrabber.IsNotNull())
				{
					GorillaTagger.Instance.StartVibration(crittersGrabber.isLeft, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
				}
			}
			this.GrabbedBy(crittersBag, false, default(Quaternion), default(Vector3), false);
			this.localCanStore = false;
			this.lastStoredObject = crittersBag;
			this.DisconnectJoint();
			return true;
		}
		if (!flag)
		{
			this.lastStoredObject = null;
		}
		return false;
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x0006C18C File Offset: 0x0006A38C
	public void SetJointRigid(Rigidbody rbToConnect)
	{
		if (this.joint != null)
		{
			return;
		}
		string str = "Critters SetJointRigid ";
		GameObject gameObject = base.gameObject;
		Debug.Log(str + ((gameObject != null) ? gameObject.ToString() : null));
		this.CreateJoint(rbToConnect, false);
		this.joint.xMotion = ConfigurableJointMotion.Locked;
		this.joint.yMotion = ConfigurableJointMotion.Locked;
		this.joint.zMotion = ConfigurableJointMotion.Locked;
		this.joint.angularXMotion = ConfigurableJointMotion.Locked;
		this.joint.angularYMotion = ConfigurableJointMotion.Locked;
		this.joint.angularZMotion = ConfigurableJointMotion.Locked;
		this.rb.mass = CrittersManager.instance.heavyMass;
		this.TogglePhysics(true);
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x0006C238 File Offset: 0x0006A438
	public void SetJointSoft(Rigidbody rbToConnect)
	{
		if (this.joint != null)
		{
			return;
		}
		string str = "Critters SetJointSoft ";
		GameObject gameObject = base.gameObject;
		Debug.Log(str + ((gameObject != null) ? gameObject.ToString() : null));
		this.CreateJoint(rbToConnect, true);
		this.joint.xMotion = ConfigurableJointMotion.Limited;
		this.joint.yMotion = ConfigurableJointMotion.Limited;
		this.joint.zMotion = ConfigurableJointMotion.Limited;
		this.joint.angularXMotion = ConfigurableJointMotion.Limited;
		this.joint.angularYMotion = ConfigurableJointMotion.Limited;
		this.joint.angularZMotion = ConfigurableJointMotion.Limited;
		this.rb.mass = CrittersManager.instance.lightMass;
		this.TogglePhysics(true);
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x0006C2E4 File Offset: 0x0006A4E4
	private void CreateJoint(Rigidbody rbToConnect, bool setParentNull = true)
	{
		if (this.joint != null)
		{
			return;
		}
		this.joint = base.gameObject.AddComponent<ConfigurableJoint>();
		this.drive = new JointDrive
		{
			positionSpring = CrittersManager.instance.springForce,
			positionDamper = CrittersManager.instance.damperForce,
			maximumForce = 10000f
		};
		this.angularDrive = new JointDrive
		{
			positionSpring = CrittersManager.instance.springAngularForce,
			positionDamper = CrittersManager.instance.damperAngularForce,
			maximumForce = 10000f
		};
		this.linearLimitDrive = new SoftJointLimit
		{
			limit = CrittersManager.instance.springForce
		};
		this.linearLimitSpringDrive = new SoftJointLimitSpring
		{
			spring = CrittersManager.instance.springForce
		};
		this.joint.linearLimit = this.linearLimitDrive;
		this.joint.linearLimitSpring = this.linearLimitSpringDrive;
		this.joint.angularYLimit = this.joint.linearLimit;
		this.joint.angularZLimit = this.joint.linearLimit;
		this.joint.angularXDrive = this.angularDrive;
		this.joint.angularYZDrive = this.angularDrive;
		this.joint.xDrive = this.drive;
		this.joint.yDrive = this.drive;
		this.joint.zDrive = this.drive;
		this.joint.autoConfigureConnectedAnchor = true;
		this.joint.enableCollision = false;
		this.joint.connectedBody = rbToConnect;
		this.rb.excludeLayers = CrittersManager.instance.movementLayers;
		this.rb.useGravity = false;
		if (setParentNull)
		{
			base.transform.SetParent(null, true);
		}
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x0006C4D4 File Offset: 0x0006A6D4
	public void DisconnectJoint()
	{
		this.rb.excludeLayers = CrittersManager.instance.containerLayer;
		this.rb.useGravity = true;
		if (this.joint != null)
		{
			UnityEngine.Object.Destroy(this.joint);
		}
		this.joint = null;
		if (this.parentActorId != -1)
		{
			CrittersActor crittersActor;
			CrittersManager.instance.actorById.TryGetValue(this.parentActorId, out crittersActor);
			base.transform.SetParent(crittersActor.transform, true);
			this.MoveActor(this.lastImpulsePosition, this.lastImpulseQuaternion, true, false, true);
			this.TogglePhysics(false);
		}
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x0006C578 File Offset: 0x0006A778
	public void AttemptRemoveStoredObjectCollider(int oldParentId, bool playSound = true)
	{
		CrittersActor crittersActor;
		if (CrittersManager.instance.actorById.TryGetValue(oldParentId, out crittersActor) && crittersActor is CrittersBag)
		{
			((CrittersBag)crittersActor).RemoveStoredObjectCollider(this, playSound);
		}
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00030EF7 File Offset: 0x0002F0F7
	public void AttemptAddStoredObjectCollider(CrittersActor actor)
	{
		if (actor is CrittersBag)
		{
			((CrittersBag)actor).AddStoredObjectCollider(this);
		}
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00030F0D File Offset: 0x0002F10D
	public bool AttemptSetEquipmentStorable()
	{
		if (!this.equipmentStorable)
		{
			return false;
		}
		this.localCanStore = true;
		return true;
	}

	// Token: 0x040000DC RID: 220
	public CrittersActor.CrittersActorType crittersActorType;

	// Token: 0x040000DD RID: 221
	public bool isSceneActor;

	// Token: 0x040000DE RID: 222
	public bool isOnPlayer;

	// Token: 0x040000DF RID: 223
	[NonSerialized]
	protected bool _isOnPlayerDefault;

	// Token: 0x040000E0 RID: 224
	public int rigPlayerId;

	// Token: 0x040000E1 RID: 225
	public int rigIndex;

	// Token: 0x040000E2 RID: 226
	public bool grabbable;

	// Token: 0x040000E3 RID: 227
	protected bool isGrabDisabled;

	// Token: 0x040000E4 RID: 228
	public int lastGrabbedPlayer;

	// Token: 0x040000E5 RID: 229
	public UnityEvent<CrittersActor> ReleasedEvent;

	// Token: 0x040000E7 RID: 231
	public Rigidbody rb;

	// Token: 0x040000E8 RID: 232
	[NonSerialized]
	public int actorId;

	// Token: 0x040000E9 RID: 233
	[NonSerialized]
	protected Transform defaultParentTransform;

	// Token: 0x040000EA RID: 234
	[NonSerialized]
	public int parentActorId = -1;

	// Token: 0x040000EB RID: 235
	[NonSerialized]
	protected int lastParentActorId;

	// Token: 0x040000EC RID: 236
	[NonSerialized]
	public Vector3 lastImpulsePosition;

	// Token: 0x040000ED RID: 237
	[NonSerialized]
	public Vector3 lastImpulseVelocity;

	// Token: 0x040000EE RID: 238
	[NonSerialized]
	public Vector3 lastImpulseAngularVelocity;

	// Token: 0x040000EF RID: 239
	[NonSerialized]
	public Quaternion lastImpulseQuaternion;

	// Token: 0x040000F0 RID: 240
	[NonSerialized]
	public double lastImpulseTime;

	// Token: 0x040000F1 RID: 241
	[NonSerialized]
	public bool updatedSinceLastFrame;

	// Token: 0x040000F2 RID: 242
	public bool isEnabled = true;

	// Token: 0x040000F3 RID: 243
	public bool wasEnabled = true;

	// Token: 0x040000F4 RID: 244
	[NonSerialized]
	protected double localLastImpulse;

	// Token: 0x040000F5 RID: 245
	[NonSerialized]
	protected Transform parentActor;

	// Token: 0x040000F6 RID: 246
	public GameObject[] subObjects;

	// Token: 0x040000F7 RID: 247
	public int subObjectIndex = -1;

	// Token: 0x040000F8 RID: 248
	public bool usesRB;

	// Token: 0x040000F9 RID: 249
	public bool resetPhysicsOnSpawn;

	// Token: 0x040000FA RID: 250
	public bool despawnWhenIdle;

	// Token: 0x040000FB RID: 251
	public bool preventDespawnUntilGrabbed;

	// Token: 0x040000FC RID: 252
	public int despawnDelay;

	// Token: 0x040000FD RID: 253
	public double despawnTime;

	// Token: 0x040000FE RID: 254
	public bool isDespawnBlocked;

	// Token: 0x040000FF RID: 255
	public bool equipmentStorable;

	// Token: 0x04000100 RID: 256
	public bool localCanStore;

	// Token: 0x04000101 RID: 257
	public CrittersActor lastStoredObject;

	// Token: 0x04000102 RID: 258
	public CapsuleCollider storeCollider;

	// Token: 0x04000103 RID: 259
	[NonSerialized]
	public Collider[] colliders;

	// Token: 0x04000104 RID: 260
	[NonSerialized]
	public ConfigurableJoint joint;

	// Token: 0x04000105 RID: 261
	[NonSerialized]
	public float timeLastTouched;

	// Token: 0x04000106 RID: 262
	private JointDrive drive;

	// Token: 0x04000107 RID: 263
	private JointDrive angularDrive;

	// Token: 0x04000108 RID: 264
	private SoftJointLimit linearLimitDrive;

	// Token: 0x04000109 RID: 265
	private SoftJointLimitSpring linearLimitSpringDrive;

	// Token: 0x0400010A RID: 266
	public CapsuleCollider equipmentStoreTriggerCollider;

	// Token: 0x0400010B RID: 267
	public bool disconnectJointFlag;

	// Token: 0x0400010C RID: 268
	public bool forceUpdate;

	// Token: 0x0400010D RID: 269
	public float FearAmount = 1f;

	// Token: 0x0400010E RID: 270
	public AnimationCurve FearCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

	// Token: 0x0400010F RID: 271
	public float AttractionAmount = 1f;

	// Token: 0x04000110 RID: 272
	public AnimationCurve AttractionCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

	// Token: 0x04000111 RID: 273
	[FormerlySerializedAs("maxDetectionDistance")]
	public float maxRangeOfFearAttraction = 3f;

	// Token: 0x04000112 RID: 274
	protected float[] averageSpeed = new float[6];

	// Token: 0x04000113 RID: 275
	protected int averageSpeedIndex;

	// Token: 0x04000114 RID: 276
	private Vector3 lastPosition = Vector3.zero;

	// Token: 0x02000034 RID: 52
	public enum CrittersActorType
	{
		// Token: 0x04000116 RID: 278
		Creature,
		// Token: 0x04000117 RID: 279
		Food,
		// Token: 0x04000118 RID: 280
		LoudNoise,
		// Token: 0x04000119 RID: 281
		BrightLight,
		// Token: 0x0400011A RID: 282
		Darkness,
		// Token: 0x0400011B RID: 283
		HidingArea,
		// Token: 0x0400011C RID: 284
		Disappear,
		// Token: 0x0400011D RID: 285
		Spawn,
		// Token: 0x0400011E RID: 286
		Player,
		// Token: 0x0400011F RID: 287
		Grabber,
		// Token: 0x04000120 RID: 288
		Cage,
		// Token: 0x04000121 RID: 289
		FoodSpawner,
		// Token: 0x04000122 RID: 290
		AttachPoint,
		// Token: 0x04000123 RID: 291
		StunBomb,
		// Token: 0x04000124 RID: 292
		Bag,
		// Token: 0x04000125 RID: 293
		BodyAttachPoint,
		// Token: 0x04000126 RID: 294
		NoiseMaker,
		// Token: 0x04000127 RID: 295
		StickyTrap,
		// Token: 0x04000128 RID: 296
		StickyGoo
	}
}
