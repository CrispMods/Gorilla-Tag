using System;
using emotitron.Compression;
using GorillaNetworking;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B9D RID: 2973
	public class DrinkableHoldable : TransferrableObject
	{
		// Token: 0x06004AF0 RID: 19184 RVA: 0x0016A90C File Offset: 0x00168B0C
		internal override void OnEnable()
		{
			base.OnEnable();
			base.enabled = (this.containerLiquid != null);
			this.itemState = (TransferrableObject.ItemStates)DrinkableHoldable.PackValues(this.sipSoundCooldown, this.containerLiquid.fillAmount, this.coolingDown);
			this.myByteArray = new byte[32];
		}

		// Token: 0x06004AF1 RID: 19185 RVA: 0x0016A960 File Offset: 0x00168B60
		protected override void LateUpdateLocal()
		{
			if (!this.containerLiquid.isActiveAndEnabled || !GorillaParent.hasInstance || !GorillaComputer.hasInstance)
			{
				base.LateUpdateLocal();
				return;
			}
			float num = (float)((GorillaComputer.instance.startupMillis + (long)Time.realtimeSinceStartup * 1000L) % 259200000L) / 1000f;
			if (Mathf.Abs(num - this.lastTimeSipSoundPlayed) > 129600f)
			{
				this.lastTimeSipSoundPlayed = num;
			}
			float num2 = this.sipRadius * this.sipRadius;
			bool flag = (GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.TransformPoint(this.headToMouthOffset) - this.containerLiquid.cupTopWorldPos).sqrMagnitude < num2;
			if (!flag)
			{
				foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
				{
					if (!vrrig.isOfflineVRRig)
					{
						if (flag || vrrig.head == null)
						{
							break;
						}
						if (vrrig.head.rigTarget == null)
						{
							break;
						}
						flag = ((vrrig.head.rigTarget.transform.TransformPoint(this.headToMouthOffset) - this.containerLiquid.cupTopWorldPos).sqrMagnitude < num2);
					}
				}
			}
			if (flag)
			{
				this.containerLiquid.fillAmount = Mathf.Clamp01(this.containerLiquid.fillAmount - this.sipRate * Time.deltaTime);
				if (num > this.lastTimeSipSoundPlayed + this.sipSoundCooldown)
				{
					if (!this.wasSipping)
					{
						this.lastTimeSipSoundPlayed = num;
						this.coolingDown = true;
					}
				}
				else
				{
					this.coolingDown = false;
				}
			}
			this.wasSipping = flag;
			this.itemState = (TransferrableObject.ItemStates)DrinkableHoldable.PackValues(this.lastTimeSipSoundPlayed, this.containerLiquid.fillAmount, this.coolingDown);
			base.LateUpdateLocal();
		}

		// Token: 0x06004AF2 RID: 19186 RVA: 0x0016AB5C File Offset: 0x00168D5C
		protected override void LateUpdateReplicated()
		{
			base.LateUpdateReplicated();
			int itemState = (int)this.itemState;
			this.UnpackValuesNonstatic(itemState, out this.lastTimeSipSoundPlayed, out this.containerLiquid.fillAmount, out this.coolingDown);
		}

		// Token: 0x06004AF3 RID: 19187 RVA: 0x0016AB95 File Offset: 0x00168D95
		protected override void LateUpdateShared()
		{
			base.LateUpdateShared();
			if (this.coolingDown && !this.wasCoolingDown)
			{
				this.sipSoundBankPlayer.Play();
			}
			this.wasCoolingDown = this.coolingDown;
		}

		// Token: 0x06004AF4 RID: 19188 RVA: 0x0016ABC4 File Offset: 0x00168DC4
		private static int PackValues(float cooldownStartTime, float fillAmount, bool coolingDown)
		{
			byte[] array = new byte[32];
			int num = 0;
			array.WriteBool(coolingDown, ref num);
			array.Write((ulong)((double)cooldownStartTime * 100.0), ref num, 25);
			array.Write((ulong)((double)fillAmount * 63.0), ref num, 6);
			return BitConverter.ToInt32(array, 0);
		}

		// Token: 0x06004AF5 RID: 19189 RVA: 0x0016AC18 File Offset: 0x00168E18
		private void UnpackValuesNonstatic(in int packed, out float cooldownStartTime, out float fillAmount, out bool coolingDown)
		{
			DrinkableHoldable.GetBytes(packed, ref this.myByteArray);
			int num = 0;
			coolingDown = this.myByteArray.ReadBool(ref num);
			cooldownStartTime = (float)(this.myByteArray.Read(ref num, 25) / 100.0);
			fillAmount = this.myByteArray.Read(ref num, 6) / 63f;
		}

		// Token: 0x06004AF6 RID: 19190 RVA: 0x0016AC7C File Offset: 0x00168E7C
		public static void GetBytes(int value, ref byte[] bytes)
		{
			for (int i = 0; i < bytes.Length; i++)
			{
				bytes[i] = (byte)(value >> 8 * i & 255);
			}
		}

		// Token: 0x06004AF7 RID: 19191 RVA: 0x0016ACAC File Offset: 0x00168EAC
		private static void UnpackValuesStatic(in int packed, out float cooldownStartTime, out float fillAmount, out bool coolingDown)
		{
			byte[] bytes = BitConverter.GetBytes(packed);
			int num = 0;
			coolingDown = bytes.ReadBool(ref num);
			cooldownStartTime = (float)(bytes.Read(ref num, 25) / 100.0);
			fillAmount = bytes.Read(ref num, 6) / 63f;
		}

		// Token: 0x04004C67 RID: 19559
		[AssignInCorePrefab]
		public ContainerLiquid containerLiquid;

		// Token: 0x04004C68 RID: 19560
		[AssignInCorePrefab]
		[SoundBankInfo]
		public SoundBankPlayer sipSoundBankPlayer;

		// Token: 0x04004C69 RID: 19561
		[AssignInCorePrefab]
		public float sipRate = 0.1f;

		// Token: 0x04004C6A RID: 19562
		[AssignInCorePrefab]
		public float sipSoundCooldown = 0.5f;

		// Token: 0x04004C6B RID: 19563
		[AssignInCorePrefab]
		public Vector3 headToMouthOffset = new Vector3(0f, 0.0208f, 0.171f);

		// Token: 0x04004C6C RID: 19564
		[AssignInCorePrefab]
		public float sipRadius = 0.15f;

		// Token: 0x04004C6D RID: 19565
		private float lastTimeSipSoundPlayed;

		// Token: 0x04004C6E RID: 19566
		private bool wasSipping;

		// Token: 0x04004C6F RID: 19567
		private bool coolingDown;

		// Token: 0x04004C70 RID: 19568
		private bool wasCoolingDown;

		// Token: 0x04004C71 RID: 19569
		private byte[] myByteArray;
	}
}
