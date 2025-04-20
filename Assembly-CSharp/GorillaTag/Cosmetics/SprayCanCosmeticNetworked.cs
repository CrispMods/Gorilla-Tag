using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C8A RID: 3210
	public class SprayCanCosmeticNetworked : MonoBehaviour
	{
		// Token: 0x0600502A RID: 20522 RVA: 0x001BAE98 File Offset: 0x001B9098
		private void OnEnable()
		{
			if (this._events == null)
			{
				this._events = base.gameObject.GetOrAddComponent<RubberDuckEvents>();
				NetPlayer netPlayer = (this.transferrableObject.myOnlineRig != null) ? this.transferrableObject.myOnlineRig.creator : ((this.transferrableObject.myRig != null) ? (this.transferrableObject.myRig.creator ?? NetworkSystem.Instance.LocalPlayer) : null);
				if (netPlayer != null)
				{
					this._events.Init(netPlayer);
				}
			}
			if (this._events != null)
			{
				this._events.Activate += this.OnShakeEvent;
			}
		}

		// Token: 0x0600502B RID: 20523 RVA: 0x001BAF60 File Offset: 0x001B9160
		private void OnDisable()
		{
			if (this._events != null)
			{
				this._events.Activate -= this.OnShakeEvent;
				this._events.Dispose();
				this._events = null;
			}
		}

		// Token: 0x0600502C RID: 20524 RVA: 0x001BAFB0 File Offset: 0x001B91B0
		private void OnShakeEvent(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
		{
			if (sender != target)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "OnShakeEvent");
			NetPlayer sender2 = info.Sender;
			VRRig myOnlineRig = this.transferrableObject.myOnlineRig;
			if (sender2 != ((myOnlineRig != null) ? myOnlineRig.creator : null))
			{
				return;
			}
			if (!this.callLimiter.CheckCallTime(Time.time))
			{
				return;
			}
			object obj = args[0];
			if (!(obj is bool))
			{
				return;
			}
			bool flag = (bool)obj;
			if (flag)
			{
				UnityEvent handleOnShakeStart = this.HandleOnShakeStart;
				if (handleOnShakeStart == null)
				{
					return;
				}
				handleOnShakeStart.Invoke();
				return;
			}
			else
			{
				UnityEvent handleOnShakeEnd = this.HandleOnShakeEnd;
				if (handleOnShakeEnd == null)
				{
					return;
				}
				handleOnShakeEnd.Invoke();
				return;
			}
		}

		// Token: 0x0600502D RID: 20525 RVA: 0x001BB03C File Offset: 0x001B923C
		public void OnShakeStart()
		{
			if (PhotonNetwork.InRoom && this._events != null && this._events.Activate != null)
			{
				this._events.Activate.RaiseOthers(new object[]
				{
					true
				});
			}
			UnityEvent handleOnShakeStart = this.HandleOnShakeStart;
			if (handleOnShakeStart == null)
			{
				return;
			}
			handleOnShakeStart.Invoke();
		}

		// Token: 0x0600502E RID: 20526 RVA: 0x001BB0A0 File Offset: 0x001B92A0
		public void OnShakeEnd()
		{
			if (PhotonNetwork.InRoom && this._events != null && this._events.Activate != null)
			{
				this._events.Activate.RaiseOthers(new object[]
				{
					false
				});
			}
			UnityEvent handleOnShakeEnd = this.HandleOnShakeEnd;
			if (handleOnShakeEnd == null)
			{
				return;
			}
			handleOnShakeEnd.Invoke();
		}

		// Token: 0x0400537A RID: 21370
		[SerializeField]
		private TransferrableObject transferrableObject;

		// Token: 0x0400537B RID: 21371
		private RubberDuckEvents _events;

		// Token: 0x0400537C RID: 21372
		private CallLimiter callLimiter = new CallLimiter(10, 1f, 0.5f);

		// Token: 0x0400537D RID: 21373
		public UnityEvent HandleOnShakeStart;

		// Token: 0x0400537E RID: 21374
		public UnityEvent HandleOnShakeEnd;
	}
}
