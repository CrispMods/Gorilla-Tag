﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009AF RID: 2479
	public class DecorativeItem : TransferrableObject
	{
		// Token: 0x06003D57 RID: 15703 RVA: 0x000562B8 File Offset: 0x000544B8
		public override bool ShouldBeKinematic()
		{
			return this.itemState == TransferrableObject.ItemStates.State2 || this.itemState == TransferrableObject.ItemStates.State4 || base.ShouldBeKinematic();
		}

		// Token: 0x06003D58 RID: 15704 RVA: 0x000570C2 File Offset: 0x000552C2
		public override void OnSpawn(VRRig rig)
		{
			base.OnSpawn(rig);
			this.parent = base.transform.parent;
		}

		// Token: 0x06003D59 RID: 15705 RVA: 0x000562D5 File Offset: 0x000544D5
		protected override void Start()
		{
			base.Start();
			this.itemState = TransferrableObject.ItemStates.State4;
			this.currentState = TransferrableObject.PositionState.Dropped;
		}

		// Token: 0x06003D5A RID: 15706 RVA: 0x0015D990 File Offset: 0x0015BB90
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

		// Token: 0x06003D5B RID: 15707 RVA: 0x0015D9E8 File Offset: 0x0015BBE8
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

		// Token: 0x06003D5C RID: 15708 RVA: 0x000570DC File Offset: 0x000552DC
		protected override void LateUpdateLocal()
		{
			base.LateUpdateLocal();
			if (this.itemState == TransferrableObject.ItemStates.State4 && this.worldShareableInstance && this.worldShareableInstance.guard.isTrulyMine)
			{
				this.InvokeRespawn();
			}
		}

		// Token: 0x06003D5D RID: 15709 RVA: 0x00057113 File Offset: 0x00055313
		public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
		{
			base.OnGrab(pointGrabbed, grabbingHand);
			this.itemState = TransferrableObject.ItemStates.State0;
		}

		// Token: 0x06003D5E RID: 15710 RVA: 0x00057124 File Offset: 0x00055324
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

		// Token: 0x06003D5F RID: 15711 RVA: 0x000532B2 File Offset: 0x000514B2
		private void SetWillTeleport()
		{
			this.worldShareableInstance.SetWillTeleport();
		}

		// Token: 0x06003D60 RID: 15712 RVA: 0x0015DA28 File Offset: 0x0015BC28
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

		// Token: 0x06003D61 RID: 15713 RVA: 0x00043726 File Offset: 0x00041926
		private void PlayVFX(GameObject vfx)
		{
			ObjectPools.instance.Instantiate(vfx, base.transform.position);
		}

		// Token: 0x06003D62 RID: 15714 RVA: 0x00057142 File Offset: 0x00055342
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

		// Token: 0x06003D63 RID: 15715 RVA: 0x0015DAA4 File Offset: 0x0015BCA4
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

		// Token: 0x06003D64 RID: 15716 RVA: 0x0005717B File Offset: 0x0005537B
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

		// Token: 0x06003D65 RID: 15717 RVA: 0x00057198 File Offset: 0x00055398
		private bool ShouldPlayFX()
		{
			return this.previousItemState == DecorativeItem.DecorativeItemState.isHeld || this.previousItemState == DecorativeItem.DecorativeItemState.dropped;
		}

		// Token: 0x06003D66 RID: 15718 RVA: 0x000571AF File Offset: 0x000553AF
		private void OnCollisionEnter(Collision other)
		{
			if (this.breakItemLayerMask != (this.breakItemLayerMask | 1 << other.gameObject.layer))
			{
				return;
			}
			this.InvokeRespawn();
		}

		// Token: 0x04003EC0 RID: 16064
		public DecorativeItemReliableState reliableState;

		// Token: 0x04003EC1 RID: 16065
		public UnityAction<DecorativeItem> respawnItem;

		// Token: 0x04003EC2 RID: 16066
		public LayerMask breakItemLayerMask;

		// Token: 0x04003EC3 RID: 16067
		private Coroutine respawnTimer;

		// Token: 0x04003EC4 RID: 16068
		private Transform parent;

		// Token: 0x04003EC5 RID: 16069
		private float _respawnTimestamp;

		// Token: 0x04003EC6 RID: 16070
		private bool isSnapped;

		// Token: 0x04003EC7 RID: 16071
		private Vector3 currentPosition;

		// Token: 0x04003EC8 RID: 16072
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04003EC9 RID: 16073
		public AudioClip snapAudio;

		// Token: 0x04003ECA RID: 16074
		public GameObject shatterVFX;

		// Token: 0x04003ECB RID: 16075
		private DecorativeItem.DecorativeItemState previousItemState = DecorativeItem.DecorativeItemState.dropped;

		// Token: 0x020009B0 RID: 2480
		private enum DecorativeItemState
		{
			// Token: 0x04003ECD RID: 16077
			isHeld = 1,
			// Token: 0x04003ECE RID: 16078
			dropped,
			// Token: 0x04003ECF RID: 16079
			snapped = 4,
			// Token: 0x04003ED0 RID: 16080
			respawn = 8,
			// Token: 0x04003ED1 RID: 16081
			none = 16
		}
	}
}
