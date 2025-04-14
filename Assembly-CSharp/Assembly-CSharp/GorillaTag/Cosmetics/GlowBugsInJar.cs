using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C47 RID: 3143
	public class GlowBugsInJar : MonoBehaviour
	{
		// Token: 0x06004E5F RID: 20063 RVA: 0x001818F0 File Offset: 0x0017FAF0
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

		// Token: 0x06004E60 RID: 20064 RVA: 0x001819C8 File Offset: 0x0017FBC8
		private void OnDisable()
		{
			if (this._events != null)
			{
				this._events.Activate -= this.OnShakeEvent;
				this._events.Dispose();
				this._events = null;
			}
		}

		// Token: 0x06004E61 RID: 20065 RVA: 0x00181A18 File Offset: 0x0017FC18
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

		// Token: 0x06004E62 RID: 20066 RVA: 0x00181A78 File Offset: 0x0017FC78
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

		// Token: 0x06004E63 RID: 20067 RVA: 0x00181AD2 File Offset: 0x0017FCD2
		private void ShakeStartLocal()
		{
			this.currentGlowAmount = 0f;
			this.shakeStarted = true;
			this.shakeTimer = 0f;
		}

		// Token: 0x06004E64 RID: 20068 RVA: 0x00181AF4 File Offset: 0x0017FCF4
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

		// Token: 0x06004E65 RID: 20069 RVA: 0x00181B4E File Offset: 0x0017FD4E
		private void ShakeEndLocal()
		{
			this.shakeStarted = false;
			this.shakeTimer = 0f;
		}

		// Token: 0x06004E66 RID: 20070 RVA: 0x00181B64 File Offset: 0x0017FD64
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

		// Token: 0x06004E67 RID: 20071 RVA: 0x00181C30 File Offset: 0x0017FE30
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

		// Token: 0x040051F5 RID: 20981
		[SerializeField]
		private TransferrableObject transferrableObject;

		// Token: 0x040051F6 RID: 20982
		[Space]
		[Tooltip("Time interval - every X seconds update the glow value")]
		[SerializeField]
		private float glowUpdateInterval = 2f;

		// Token: 0x040051F7 RID: 20983
		[Tooltip("step increment - increase the glow value one step for N amount")]
		[SerializeField]
		private float glowIncreaseStepAmount = 0.1f;

		// Token: 0x040051F8 RID: 20984
		[Tooltip("step decrement - decrease the glow value one step for N amount")]
		[SerializeField]
		private float glowDecreaseStepAmount = 0.2f;

		// Token: 0x040051F9 RID: 20985
		[Space]
		[SerializeField]
		private string shaderProperty = "_EmissionColor";

		// Token: 0x040051FA RID: 20986
		[SerializeField]
		private Renderer[] renderers;

		// Token: 0x040051FB RID: 20987
		private bool shakeStarted = true;

		// Token: 0x040051FC RID: 20988
		private static int EmissionColor;

		// Token: 0x040051FD RID: 20989
		private float currentGlowAmount;

		// Token: 0x040051FE RID: 20990
		private float shakeTimer;

		// Token: 0x040051FF RID: 20991
		private RubberDuckEvents _events;

		// Token: 0x04005200 RID: 20992
		private CallLimiter callLimiter = new CallLimiter(10, 2f, 0.5f);
	}
}
