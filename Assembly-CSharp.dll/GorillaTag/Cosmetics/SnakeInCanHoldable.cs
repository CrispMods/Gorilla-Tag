using System;
using System.Collections;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C5A RID: 3162
	public class SnakeInCanHoldable : TransferrableObject
	{
		// Token: 0x06004EC7 RID: 20167 RVA: 0x00062B2E File Offset: 0x00060D2E
		protected override void Awake()
		{
			base.Awake();
			this.topRigPosition = this.topRigObject.transform.position;
		}

		// Token: 0x06004EC8 RID: 20168 RVA: 0x001B2A34 File Offset: 0x001B0C34
		internal override void OnEnable()
		{
			base.OnEnable();
			this.disableObjectBeforeTrigger.SetActive(false);
			if (this.compressedPoint != null)
			{
				this.topRigObject.transform.position = this.compressedPoint.position;
			}
			if (this._events == null)
			{
				this._events = base.gameObject.GetOrAddComponent<RubberDuckEvents>();
				NetPlayer netPlayer = (base.myOnlineRig != null) ? base.myOnlineRig.creator : ((base.myRig != null) ? ((base.myRig.creator != null) ? base.myRig.creator : NetworkSystem.Instance.LocalPlayer) : null);
				if (netPlayer != null)
				{
					this._events.Init(netPlayer);
				}
			}
			if (this._events != null)
			{
				this._events.Activate += this.OnEnableObject;
			}
		}

		// Token: 0x06004EC9 RID: 20169 RVA: 0x001B2B2C File Offset: 0x001B0D2C
		internal override void OnDisable()
		{
			base.OnDisable();
			if (this._events != null)
			{
				this._events.Activate -= this.OnEnableObject;
				this._events.Dispose();
				this._events = null;
			}
		}

		// Token: 0x06004ECA RID: 20170 RVA: 0x001B2B84 File Offset: 0x001B0D84
		public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
		{
			if (!base.OnRelease(zoneReleased, releasingHand))
			{
				return false;
			}
			if (VRRigCache.Instance.localRig.Rig != this.ownerRig)
			{
				return false;
			}
			if (PhotonNetwork.InRoom && this._events != null && this._events.Activate != null)
			{
				this._events.Activate.RaiseOthers(new object[]
				{
					false
				});
			}
			this.EnableObjectLocal(false);
			return true;
		}

		// Token: 0x06004ECB RID: 20171 RVA: 0x001B2C0C File Offset: 0x001B0E0C
		private void OnEnableObject(int sender, int target, object[] arg, PhotonMessageInfoWrapped info)
		{
			if (info.senderID != this.ownerRig.creator.ActorNumber)
			{
				return;
			}
			if (arg.Length != 1 || !(arg[0] is bool))
			{
				return;
			}
			if (sender != target)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "OnEnableObject");
			if (!this.snakeInCanCallLimiter.CheckCallTime(Time.time))
			{
				return;
			}
			bool enable = (bool)arg[0];
			this.EnableObjectLocal(enable);
		}

		// Token: 0x06004ECC RID: 20172 RVA: 0x001B2C78 File Offset: 0x001B0E78
		private void EnableObjectLocal(bool enable)
		{
			this.disableObjectBeforeTrigger.SetActive(enable);
			if (!enable)
			{
				if (this.compressedPoint != null)
				{
					this.topRigObject.transform.position = this.compressedPoint.position;
				}
				return;
			}
			if (this.stretchedPoint != null)
			{
				base.StartCoroutine(this.SmoothTransition());
				return;
			}
			this.topRigObject.transform.position = this.topRigPosition;
		}

		// Token: 0x06004ECD RID: 20173 RVA: 0x00062B4C File Offset: 0x00060D4C
		private IEnumerator SmoothTransition()
		{
			while (Vector3.Distance(this.topRigObject.transform.position, this.stretchedPoint.position) > 0.01f)
			{
				this.topRigObject.transform.position = Vector3.MoveTowards(this.topRigObject.transform.position, this.stretchedPoint.position, this.jumpSpeed * Time.deltaTime);
				yield return null;
			}
			this.topRigObject.transform.position = this.stretchedPoint.position;
			yield break;
		}

		// Token: 0x06004ECE RID: 20174 RVA: 0x00062B5B File Offset: 0x00060D5B
		public void OnButtonPressed()
		{
			this.EnableObjectLocal(true);
		}

		// Token: 0x04005274 RID: 21108
		[SerializeField]
		private float jumpSpeed;

		// Token: 0x04005275 RID: 21109
		[SerializeField]
		private Transform stretchedPoint;

		// Token: 0x04005276 RID: 21110
		[SerializeField]
		private Transform compressedPoint;

		// Token: 0x04005277 RID: 21111
		[SerializeField]
		private GameObject topRigObject;

		// Token: 0x04005278 RID: 21112
		[SerializeField]
		private GameObject disableObjectBeforeTrigger;

		// Token: 0x04005279 RID: 21113
		private CallLimiter snakeInCanCallLimiter = new CallLimiter(10, 2f, 0.5f);

		// Token: 0x0400527A RID: 21114
		private Vector3 topRigPosition;

		// Token: 0x0400527B RID: 21115
		private Vector3 originalTopRigPosition;

		// Token: 0x0400527C RID: 21116
		private RubberDuckEvents _events;
	}
}
