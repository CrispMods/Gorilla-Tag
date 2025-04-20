using System;
using System.Collections;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C88 RID: 3208
	public class SnakeInCanHoldable : TransferrableObject
	{
		// Token: 0x0600501B RID: 20507 RVA: 0x00064553 File Offset: 0x00062753
		protected override void Awake()
		{
			base.Awake();
			this.topRigPosition = this.topRigObject.transform.position;
		}

		// Token: 0x0600501C RID: 20508 RVA: 0x001BAB18 File Offset: 0x001B8D18
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

		// Token: 0x0600501D RID: 20509 RVA: 0x001BAC10 File Offset: 0x001B8E10
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

		// Token: 0x0600501E RID: 20510 RVA: 0x001BAC68 File Offset: 0x001B8E68
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

		// Token: 0x0600501F RID: 20511 RVA: 0x001BACF0 File Offset: 0x001B8EF0
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

		// Token: 0x06005020 RID: 20512 RVA: 0x001BAD5C File Offset: 0x001B8F5C
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

		// Token: 0x06005021 RID: 20513 RVA: 0x00064571 File Offset: 0x00062771
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

		// Token: 0x06005022 RID: 20514 RVA: 0x00064580 File Offset: 0x00062780
		public void OnButtonPressed()
		{
			this.EnableObjectLocal(true);
		}

		// Token: 0x0400536E RID: 21358
		[SerializeField]
		private float jumpSpeed;

		// Token: 0x0400536F RID: 21359
		[SerializeField]
		private Transform stretchedPoint;

		// Token: 0x04005370 RID: 21360
		[SerializeField]
		private Transform compressedPoint;

		// Token: 0x04005371 RID: 21361
		[SerializeField]
		private GameObject topRigObject;

		// Token: 0x04005372 RID: 21362
		[SerializeField]
		private GameObject disableObjectBeforeTrigger;

		// Token: 0x04005373 RID: 21363
		private CallLimiter snakeInCanCallLimiter = new CallLimiter(10, 2f, 0.5f);

		// Token: 0x04005374 RID: 21364
		private Vector3 topRigPosition;

		// Token: 0x04005375 RID: 21365
		private Vector3 originalTopRigPosition;

		// Token: 0x04005376 RID: 21366
		private RubberDuckEvents _events;
	}
}
