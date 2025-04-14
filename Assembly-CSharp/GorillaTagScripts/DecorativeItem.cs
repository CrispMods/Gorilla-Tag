using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009AC RID: 2476
	public class DecorativeItem : TransferrableObject
	{
		// Token: 0x06003D4B RID: 15691 RVA: 0x00113632 File Offset: 0x00111832
		public override bool ShouldBeKinematic()
		{
			return this.itemState == TransferrableObject.ItemStates.State2 || this.itemState == TransferrableObject.ItemStates.State4 || base.ShouldBeKinematic();
		}

		// Token: 0x06003D4C RID: 15692 RVA: 0x00121846 File Offset: 0x0011FA46
		public override void OnSpawn(VRRig rig)
		{
			base.OnSpawn(rig);
			this.parent = base.transform.parent;
		}

		// Token: 0x06003D4D RID: 15693 RVA: 0x001136B3 File Offset: 0x001118B3
		protected override void Start()
		{
			base.Start();
			this.itemState = TransferrableObject.ItemStates.State4;
			this.currentState = TransferrableObject.PositionState.Dropped;
		}

		// Token: 0x06003D4E RID: 15694 RVA: 0x00121860 File Offset: 0x0011FA60
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

		// Token: 0x06003D4F RID: 15695 RVA: 0x001218B8 File Offset: 0x0011FAB8
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

		// Token: 0x06003D50 RID: 15696 RVA: 0x001218F7 File Offset: 0x0011FAF7
		protected override void LateUpdateLocal()
		{
			base.LateUpdateLocal();
			if (this.itemState == TransferrableObject.ItemStates.State4 && this.worldShareableInstance && this.worldShareableInstance.guard.isTrulyMine)
			{
				this.InvokeRespawn();
			}
		}

		// Token: 0x06003D51 RID: 15697 RVA: 0x0012192E File Offset: 0x0011FB2E
		public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
		{
			base.OnGrab(pointGrabbed, grabbingHand);
			this.itemState = TransferrableObject.ItemStates.State0;
		}

		// Token: 0x06003D52 RID: 15698 RVA: 0x0012193F File Offset: 0x0011FB3F
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

		// Token: 0x06003D53 RID: 15699 RVA: 0x00101539 File Offset: 0x000FF739
		private void SetWillTeleport()
		{
			this.worldShareableInstance.SetWillTeleport();
		}

		// Token: 0x06003D54 RID: 15700 RVA: 0x00121960 File Offset: 0x0011FB60
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

		// Token: 0x06003D55 RID: 15701 RVA: 0x00092E3E File Offset: 0x0009103E
		private void PlayVFX(GameObject vfx)
		{
			ObjectPools.instance.Instantiate(vfx, base.transform.position);
		}

		// Token: 0x06003D56 RID: 15702 RVA: 0x001219DC File Offset: 0x0011FBDC
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

		// Token: 0x06003D57 RID: 15703 RVA: 0x00121A18 File Offset: 0x0011FC18
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

		// Token: 0x06003D58 RID: 15704 RVA: 0x00121B33 File Offset: 0x0011FD33
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

		// Token: 0x06003D59 RID: 15705 RVA: 0x00121B50 File Offset: 0x0011FD50
		private bool ShouldPlayFX()
		{
			return this.previousItemState == DecorativeItem.DecorativeItemState.isHeld || this.previousItemState == DecorativeItem.DecorativeItemState.dropped;
		}

		// Token: 0x06003D5A RID: 15706 RVA: 0x00121B67 File Offset: 0x0011FD67
		private void OnCollisionEnter(Collision other)
		{
			if (this.breakItemLayerMask != (this.breakItemLayerMask | 1 << other.gameObject.layer))
			{
				return;
			}
			this.InvokeRespawn();
		}

		// Token: 0x04003EAE RID: 16046
		public DecorativeItemReliableState reliableState;

		// Token: 0x04003EAF RID: 16047
		public UnityAction<DecorativeItem> respawnItem;

		// Token: 0x04003EB0 RID: 16048
		public LayerMask breakItemLayerMask;

		// Token: 0x04003EB1 RID: 16049
		private Coroutine respawnTimer;

		// Token: 0x04003EB2 RID: 16050
		private Transform parent;

		// Token: 0x04003EB3 RID: 16051
		private float _respawnTimestamp;

		// Token: 0x04003EB4 RID: 16052
		private bool isSnapped;

		// Token: 0x04003EB5 RID: 16053
		private Vector3 currentPosition;

		// Token: 0x04003EB6 RID: 16054
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04003EB7 RID: 16055
		public AudioClip snapAudio;

		// Token: 0x04003EB8 RID: 16056
		public GameObject shatterVFX;

		// Token: 0x04003EB9 RID: 16057
		private DecorativeItem.DecorativeItemState previousItemState = DecorativeItem.DecorativeItemState.dropped;

		// Token: 0x020009AD RID: 2477
		private enum DecorativeItemState
		{
			// Token: 0x04003EBB RID: 16059
			isHeld = 1,
			// Token: 0x04003EBC RID: 16060
			dropped,
			// Token: 0x04003EBD RID: 16061
			snapped = 4,
			// Token: 0x04003EBE RID: 16062
			respawn = 8,
			// Token: 0x04003EBF RID: 16063
			none = 16
		}
	}
}
