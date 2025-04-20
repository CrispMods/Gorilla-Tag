using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009D2 RID: 2514
	public class DecorativeItem : TransferrableObject
	{
		// Token: 0x06003E63 RID: 15971 RVA: 0x00057B4F File Offset: 0x00055D4F
		public override bool ShouldBeKinematic()
		{
			return this.itemState == TransferrableObject.ItemStates.State2 || this.itemState == TransferrableObject.ItemStates.State4 || base.ShouldBeKinematic();
		}

		// Token: 0x06003E64 RID: 15972 RVA: 0x00058959 File Offset: 0x00056B59
		public override void OnSpawn(VRRig rig)
		{
			base.OnSpawn(rig);
			this.parent = base.transform.parent;
		}

		// Token: 0x06003E65 RID: 15973 RVA: 0x00057B6C File Offset: 0x00055D6C
		protected override void Start()
		{
			base.Start();
			this.itemState = TransferrableObject.ItemStates.State4;
			this.currentState = TransferrableObject.PositionState.Dropped;
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x00163978 File Offset: 0x00161B78
		private new void OnStateChanged()
		{
			TransferrableObject.ItemStates itemState = this.itemState;
			if (itemState == TransferrableObject.ItemStates.State2)
			{
				this.SnapItem(this.reliableState.isSnapped, this.reliableState.snapPosition);
				return;
			}
			if (itemState != TransferrableObject.ItemStates.State3)
			{
				return;
			}
			this.Respawn(this.reliableState.respawnPosition, this.reliableState.respawnRotation);
		}

		// Token: 0x06003E67 RID: 15975 RVA: 0x001639D0 File Offset: 0x00161BD0
		protected override void LateUpdateShared()
		{
			base.LateUpdateShared();
			if (base.InHand())
			{
				this.itemState = TransferrableObject.ItemStates.State0;
			}
			DecorativeItem.DecorativeItemState itemState = (DecorativeItem.DecorativeItemState)this.itemState;
			if (itemState != this.previousItemState)
			{
				this.OnStateChanged();
			}
			this.previousItemState = itemState;
		}

		// Token: 0x06003E68 RID: 15976 RVA: 0x00058973 File Offset: 0x00056B73
		protected override void LateUpdateLocal()
		{
			base.LateUpdateLocal();
			if (this.itemState == TransferrableObject.ItemStates.State4 && this.worldShareableInstance && this.worldShareableInstance.guard.isTrulyMine)
			{
				this.InvokeRespawn();
			}
		}

		// Token: 0x06003E69 RID: 15977 RVA: 0x000589AA File Offset: 0x00056BAA
		public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
		{
			base.OnGrab(pointGrabbed, grabbingHand);
			this.itemState = TransferrableObject.ItemStates.State0;
		}

		// Token: 0x06003E6A RID: 15978 RVA: 0x000589BB File Offset: 0x00056BBB
		public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
		{
			if (!base.OnRelease(zoneReleased, releasingHand))
			{
				return false;
			}
			this.itemState = TransferrableObject.ItemStates.State1;
			this.Reparent(null);
			return true;
		}

		// Token: 0x06003E6B RID: 15979 RVA: 0x000547CF File Offset: 0x000529CF
		private void SetWillTeleport()
		{
			this.worldShareableInstance.SetWillTeleport();
		}

		// Token: 0x06003E6C RID: 15980 RVA: 0x00163A10 File Offset: 0x00161C10
		public void Respawn(Vector3 randPosition, Quaternion randRotation)
		{
			if (base.InHand())
			{
				return;
			}
			if (this.shatterVFX && this.ShouldPlayFX())
			{
				this.PlayVFX(this.shatterVFX);
			}
			this.itemState = TransferrableObject.ItemStates.State3;
			this.SetWillTeleport();
			Transform transform = base.transform;
			transform.position = randPosition;
			transform.rotation = randRotation;
			if (this.reliableState)
			{
				this.reliableState.respawnPosition = randPosition;
				this.reliableState.respawnRotation = randRotation;
			}
		}

		// Token: 0x06003E6D RID: 15981 RVA: 0x00044AC5 File Offset: 0x00042CC5
		private void PlayVFX(GameObject vfx)
		{
			ObjectPools.instance.Instantiate(vfx, base.transform.position);
		}

		// Token: 0x06003E6E RID: 15982 RVA: 0x000589D9 File Offset: 0x00056BD9
		private bool Reparent(Transform _transform)
		{
			if (!this.allowReparenting)
			{
				return false;
			}
			if (this.parent)
			{
				this.parent.SetParent(_transform);
				base.transform.SetParent(this.parent);
				return true;
			}
			return false;
		}

		// Token: 0x06003E6F RID: 15983 RVA: 0x00163A8C File Offset: 0x00161C8C
		public void SnapItem(bool snap, Vector3 attachPoint)
		{
			if (!this.reliableState)
			{
				return;
			}
			if (snap)
			{
				AttachPoint currentAttachPointByPosition = DecorativeItemsManager.Instance.getCurrentAttachPointByPosition(attachPoint);
				if (!currentAttachPointByPosition)
				{
					this.reliableState.isSnapped = false;
					this.reliableState.snapPosition = Vector3.zero;
					return;
				}
				Transform attachPoint2 = currentAttachPointByPosition.attachPoint;
				if (!this.Reparent(attachPoint2))
				{
					this.reliableState.isSnapped = false;
					this.reliableState.snapPosition = Vector3.zero;
					return;
				}
				this.itemState = TransferrableObject.ItemStates.State2;
				base.transform.parent.localPosition = Vector3.zero;
				base.transform.localPosition = Vector3.zero;
				this.reliableState.isSnapped = true;
				if (this.audioSource && this.snapAudio && this.ShouldPlayFX())
				{
					this.audioSource.GTPlayOneShot(this.snapAudio, 1f);
				}
				currentAttachPointByPosition.SetIsHook(true);
			}
			else
			{
				this.Reparent(null);
				this.reliableState.isSnapped = false;
			}
			this.reliableState.snapPosition = attachPoint;
		}

		// Token: 0x06003E70 RID: 15984 RVA: 0x00058A12 File Offset: 0x00056C12
		private void InvokeRespawn()
		{
			if (this.itemState == TransferrableObject.ItemStates.State2)
			{
				return;
			}
			UnityAction<DecorativeItem> unityAction = this.respawnItem;
			if (unityAction == null)
			{
				return;
			}
			unityAction(this);
		}

		// Token: 0x06003E71 RID: 15985 RVA: 0x00058A2F File Offset: 0x00056C2F
		private bool ShouldPlayFX()
		{
			return this.previousItemState == DecorativeItem.DecorativeItemState.isHeld || this.previousItemState == DecorativeItem.DecorativeItemState.dropped;
		}

		// Token: 0x06003E72 RID: 15986 RVA: 0x00058A46 File Offset: 0x00056C46
		private void OnCollisionEnter(Collision other)
		{
			if (this.breakItemLayerMask != (this.breakItemLayerMask | 1 << other.gameObject.layer))
			{
				return;
			}
			this.InvokeRespawn();
		}

		// Token: 0x04003F88 RID: 16264
		public DecorativeItemReliableState reliableState;

		// Token: 0x04003F89 RID: 16265
		public UnityAction<DecorativeItem> respawnItem;

		// Token: 0x04003F8A RID: 16266
		public LayerMask breakItemLayerMask;

		// Token: 0x04003F8B RID: 16267
		private Coroutine respawnTimer;

		// Token: 0x04003F8C RID: 16268
		private Transform parent;

		// Token: 0x04003F8D RID: 16269
		private float _respawnTimestamp;

		// Token: 0x04003F8E RID: 16270
		private bool isSnapped;

		// Token: 0x04003F8F RID: 16271
		private Vector3 currentPosition;

		// Token: 0x04003F90 RID: 16272
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04003F91 RID: 16273
		public AudioClip snapAudio;

		// Token: 0x04003F92 RID: 16274
		public GameObject shatterVFX;

		// Token: 0x04003F93 RID: 16275
		private DecorativeItem.DecorativeItemState previousItemState = DecorativeItem.DecorativeItemState.dropped;

		// Token: 0x020009D3 RID: 2515
		private enum DecorativeItemState
		{
			// Token: 0x04003F95 RID: 16277
			isHeld = 1,
			// Token: 0x04003F96 RID: 16278
			dropped,
			// Token: 0x04003F97 RID: 16279
			snapped = 4,
			// Token: 0x04003F98 RID: 16280
			respawn = 8,
			// Token: 0x04003F99 RID: 16281
			none = 16
		}
	}
}
