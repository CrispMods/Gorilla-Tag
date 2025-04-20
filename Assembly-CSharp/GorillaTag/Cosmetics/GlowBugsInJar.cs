using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C75 RID: 3189
	public class GlowBugsInJar : MonoBehaviour
	{
		// Token: 0x06004FB3 RID: 20403 RVA: 0x001B999C File Offset: 0x001B7B9C
		private void OnEnable()
		{
			this.shakeStarted = false;
			this.UpdateGlow(0f);
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

		// Token: 0x06004FB4 RID: 20404 RVA: 0x001B9A74 File Offset: 0x001B7C74
		private void OnDisable()
		{
			if (this._events != null)
			{
				this._events.Activate -= this.OnShakeEvent;
				this._events.Dispose();
				this._events = null;
			}
		}

		// Token: 0x06004FB5 RID: 20405 RVA: 0x001B9AC4 File Offset: 0x001B7CC4
		private void OnShakeEvent(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
		{
			if (sender != target)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "OnShakeEvent");
			if (!this.callLimiter.CheckCallTime(Time.time))
			{
				return;
			}
			if (args != null && args.Length == 1)
			{
				object obj = args[0];
				if (obj is bool)
				{
					bool flag = (bool)obj;
					if (flag)
					{
						this.ShakeStartLocal();
						return;
					}
					this.ShakeEndLocal();
					return;
				}
			}
		}

		// Token: 0x06004FB6 RID: 20406 RVA: 0x001B9B24 File Offset: 0x001B7D24
		public void HandleOnShakeStart()
		{
			if (PhotonNetwork.InRoom && this._events != null && this._events.Activate != null)
			{
				this._events.Activate.RaiseOthers(new object[]
				{
					true
				});
			}
			this.ShakeStartLocal();
		}

		// Token: 0x06004FB7 RID: 20407 RVA: 0x000640BA File Offset: 0x000622BA
		private void ShakeStartLocal()
		{
			this.currentGlowAmount = 0f;
			this.shakeStarted = true;
			this.shakeTimer = 0f;
		}

		// Token: 0x06004FB8 RID: 20408 RVA: 0x001B9B80 File Offset: 0x001B7D80
		public void HandleOnShakeEnd()
		{
			if (PhotonNetwork.InRoom && this._events != null && this._events.Activate != null)
			{
				this._events.Activate.RaiseOthers(new object[]
				{
					false
				});
			}
			this.ShakeEndLocal();
		}

		// Token: 0x06004FB9 RID: 20409 RVA: 0x000640D9 File Offset: 0x000622D9
		private void ShakeEndLocal()
		{
			this.shakeStarted = false;
			this.shakeTimer = 0f;
		}

		// Token: 0x06004FBA RID: 20410 RVA: 0x001B9BDC File Offset: 0x001B7DDC
		public void Update()
		{
			if (this.shakeStarted)
			{
				this.shakeTimer += 1f;
				if (this.shakeTimer >= this.glowUpdateInterval && this.currentGlowAmount < 1f)
				{
					this.currentGlowAmount += this.glowIncreaseStepAmount;
					this.UpdateGlow(this.currentGlowAmount);
					this.shakeTimer = 0f;
					return;
				}
			}
			else
			{
				this.shakeTimer += 1f;
				if (this.shakeTimer >= this.glowUpdateInterval && this.currentGlowAmount > 0f)
				{
					this.currentGlowAmount -= this.glowDecreaseStepAmount;
					this.UpdateGlow(this.currentGlowAmount);
					this.shakeTimer = 0f;
				}
			}
		}

		// Token: 0x06004FBB RID: 20411 RVA: 0x001B9CA8 File Offset: 0x001B7EA8
		private void UpdateGlow(float value)
		{
			if (this.renderers.Length != 0)
			{
				for (int i = 0; i < this.renderers.Length; i++)
				{
					Material material = this.renderers[i].material;
					Color color = material.GetColor(this.shaderProperty);
					color.a = value;
					material.SetColor(this.shaderProperty, color);
					material.EnableKeyword("_EMISSION");
				}
			}
		}

		// Token: 0x040052EF RID: 21231
		[SerializeField]
		private TransferrableObject transferrableObject;

		// Token: 0x040052F0 RID: 21232
		[Space]
		[Tooltip("Time interval - every X seconds update the glow value")]
		[SerializeField]
		private float glowUpdateInterval = 2f;

		// Token: 0x040052F1 RID: 21233
		[Tooltip("step increment - increase the glow value one step for N amount")]
		[SerializeField]
		private float glowIncreaseStepAmount = 0.1f;

		// Token: 0x040052F2 RID: 21234
		[Tooltip("step decrement - decrease the glow value one step for N amount")]
		[SerializeField]
		private float glowDecreaseStepAmount = 0.2f;

		// Token: 0x040052F3 RID: 21235
		[Space]
		[SerializeField]
		private string shaderProperty = "_EmissionColor";

		// Token: 0x040052F4 RID: 21236
		[SerializeField]
		private Renderer[] renderers;

		// Token: 0x040052F5 RID: 21237
		private bool shakeStarted = true;

		// Token: 0x040052F6 RID: 21238
		private static int EmissionColor;

		// Token: 0x040052F7 RID: 21239
		private float currentGlowAmount;

		// Token: 0x040052F8 RID: 21240
		private float shakeTimer;

		// Token: 0x040052F9 RID: 21241
		private RubberDuckEvents _events;

		// Token: 0x040052FA RID: 21242
		private CallLimiter callLimiter = new CallLimiter(10, 2f, 0.5f);
	}
}
