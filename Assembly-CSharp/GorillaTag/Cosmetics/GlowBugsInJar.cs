using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C44 RID: 3140
	public class GlowBugsInJar : MonoBehaviour
	{
		// Token: 0x06004E53 RID: 20051 RVA: 0x00181328 File Offset: 0x0017F528
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

		// Token: 0x06004E54 RID: 20052 RVA: 0x00181400 File Offset: 0x0017F600
		private void OnDisable()
		{
			if (this._events != null)
			{
				this._events.Activate -= this.OnShakeEvent;
				this._events.Dispose();
				this._events = null;
			}
		}

		// Token: 0x06004E55 RID: 20053 RVA: 0x00181450 File Offset: 0x0017F650
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

		// Token: 0x06004E56 RID: 20054 RVA: 0x001814B0 File Offset: 0x0017F6B0
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

		// Token: 0x06004E57 RID: 20055 RVA: 0x0018150A File Offset: 0x0017F70A
		private void ShakeStartLocal()
		{
			this.currentGlowAmount = 0f;
			this.shakeStarted = true;
			this.shakeTimer = 0f;
		}

		// Token: 0x06004E58 RID: 20056 RVA: 0x0018152C File Offset: 0x0017F72C
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

		// Token: 0x06004E59 RID: 20057 RVA: 0x00181586 File Offset: 0x0017F786
		private void ShakeEndLocal()
		{
			this.shakeStarted = false;
			this.shakeTimer = 0f;
		}

		// Token: 0x06004E5A RID: 20058 RVA: 0x0018159C File Offset: 0x0017F79C
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

		// Token: 0x06004E5B RID: 20059 RVA: 0x00181668 File Offset: 0x0017F868
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

		// Token: 0x040051E3 RID: 20963
		[SerializeField]
		private TransferrableObject transferrableObject;

		// Token: 0x040051E4 RID: 20964
		[Space]
		[Tooltip("Time interval - every X seconds update the glow value")]
		[SerializeField]
		private float glowUpdateInterval = 2f;

		// Token: 0x040051E5 RID: 20965
		[Tooltip("step increment - increase the glow value one step for N amount")]
		[SerializeField]
		private float glowIncreaseStepAmount = 0.1f;

		// Token: 0x040051E6 RID: 20966
		[Tooltip("step decrement - decrease the glow value one step for N amount")]
		[SerializeField]
		private float glowDecreaseStepAmount = 0.2f;

		// Token: 0x040051E7 RID: 20967
		[Space]
		[SerializeField]
		private string shaderProperty = "_EmissionColor";

		// Token: 0x040051E8 RID: 20968
		[SerializeField]
		private Renderer[] renderers;

		// Token: 0x040051E9 RID: 20969
		private bool shakeStarted = true;

		// Token: 0x040051EA RID: 20970
		private static int EmissionColor;

		// Token: 0x040051EB RID: 20971
		private float currentGlowAmount;

		// Token: 0x040051EC RID: 20972
		private float shakeTimer;

		// Token: 0x040051ED RID: 20973
		private RubberDuckEvents _events;

		// Token: 0x040051EE RID: 20974
		private CallLimiter callLimiter = new CallLimiter(10, 2f, 0.5f);
	}
}
