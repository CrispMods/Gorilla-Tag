using System;
using System.Collections;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C57 RID: 3159
	public class SnakeInCanHoldable : TransferrableObject
	{
		// Token: 0x06004EBB RID: 20155 RVA: 0x00182936 File Offset: 0x00180B36
		protected override void Awake()
		{
			base.Awake();
			this.topRigPosition = this.topRigObject.transform.position;
		}

		// Token: 0x06004EBC RID: 20156 RVA: 0x00182954 File Offset: 0x00180B54
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

		// Token: 0x06004EBD RID: 20157 RVA: 0x00182A4C File Offset: 0x00180C4C
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

		// Token: 0x06004EBE RID: 20158 RVA: 0x00182AA4 File Offset: 0x00180CA4
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

		// Token: 0x06004EBF RID: 20159 RVA: 0x00182B2C File Offset: 0x00180D2C
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

		// Token: 0x06004EC0 RID: 20160 RVA: 0x00182B98 File Offset: 0x00180D98
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

		// Token: 0x06004EC1 RID: 20161 RVA: 0x00182C10 File Offset: 0x00180E10
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

		// Token: 0x06004EC2 RID: 20162 RVA: 0x00182C1F File Offset: 0x00180E1F
		public void OnButtonPressed()
		{
			this.EnableObjectLocal(true);
		}

		// Token: 0x04005262 RID: 21090
		[SerializeField]
		private float jumpSpeed;

		// Token: 0x04005263 RID: 21091
		[SerializeField]
		private Transform stretchedPoint;

		// Token: 0x04005264 RID: 21092
		[SerializeField]
		private Transform compressedPoint;

		// Token: 0x04005265 RID: 21093
		[SerializeField]
		private GameObject topRigObject;

		// Token: 0x04005266 RID: 21094
		[SerializeField]
		private GameObject disableObjectBeforeTrigger;

		// Token: 0x04005267 RID: 21095
		private CallLimiter snakeInCanCallLimiter = new CallLimiter(10, 2f, 0.5f);

		// Token: 0x04005268 RID: 21096
		private Vector3 topRigPosition;

		// Token: 0x04005269 RID: 21097
		private Vector3 originalTopRigPosition;

		// Token: 0x0400526A RID: 21098
		private RubberDuckEvents _events;
	}
}
