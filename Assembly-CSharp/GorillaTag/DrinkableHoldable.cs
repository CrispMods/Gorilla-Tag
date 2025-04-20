using System;
using emotitron.Compression;
using GorillaNetworking;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BCA RID: 3018
	public class DrinkableHoldable : TransferrableObject
	{
		// Token: 0x06004C3B RID: 19515 RVA: 0x001A3E70 File Offset: 0x001A2070
		internal override void OnEnable()
		{
			base.OnEnable();
			base.enabled = (this.containerLiquid != null);
			this.itemState = (TransferrableObject.ItemStates)DrinkableHoldable.PackValues(this.sipSoundCooldown, this.containerLiquid.fillAmount, this.coolingDown);
			this.myByteArray = new byte[32];
		}

		// Token: 0x06004C3C RID: 19516 RVA: 0x001A3EC4 File Offset: 0x001A20C4
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

		// Token: 0x06004C3D RID: 19517 RVA: 0x001A40C0 File Offset: 0x001A22C0
		protected override void LateUpdateReplicated()
		{
			base.LateUpdateReplicated();
			int itemState = (int)this.itemState;
			this.UnpackValuesNonstatic(itemState, out this.lastTimeSipSoundPlayed, out this.containerLiquid.fillAmount, out this.coolingDown);
		}

		// Token: 0x06004C3E RID: 19518 RVA: 0x000622AB File Offset: 0x000604AB
		protected override void LateUpdateShared()
		{
			base.LateUpdateShared();
			if (this.coolingDown && !this.wasCoolingDown)
			{
				this.sipSoundBankPlayer.Play();
			}
			this.wasCoolingDown = this.coolingDown;
		}

		// Token: 0x06004C3F RID: 19519 RVA: 0x001A40FC File Offset: 0x001A22FC
		private static int PackValues(float cooldownStartTime, float fillAmount, bool coolingDown)
		{
			byte[] array = new byte[32];
			int num = 0;
			array.WriteBool(coolingDown, ref num);
			array.Write((ulong)((double)cooldownStartTime * 100.0), ref num, 25);
			array.Write((ulong)((double)fillAmount * 63.0), ref num, 6);
			return BitConverter.ToInt32(array, 0);
		}

		// Token: 0x06004C40 RID: 19520 RVA: 0x001A4150 File Offset: 0x001A2350
		private void UnpackValuesNonstatic(in int packed, out float cooldownStartTime, out float fillAmount, out bool coolingDown)
		{
			DrinkableHoldable.GetBytes(packed, ref this.myByteArray);
			int num = 0;
			coolingDown = this.myByteArray.ReadBool(ref num);
			cooldownStartTime = (float)(this.myByteArray.Read(ref num, 25) / 100.0);
			fillAmount = this.myByteArray.Read(ref num, 6) / 63f;
		}

		// Token: 0x06004C41 RID: 19521 RVA: 0x001A41B4 File Offset: 0x001A23B4
		public static void GetBytes(int value, ref byte[] bytes)
		{
			for (int i = 0; i < bytes.Length; i++)
			{
				bytes[i] = (byte)(value >> 8 * i & 255);
			}
		}

		// Token: 0x06004C42 RID: 19522 RVA: 0x001A41E4 File Offset: 0x001A23E4
		private static void UnpackValuesStatic(in int packed, out float cooldownStartTime, out float fillAmount, out bool coolingDown)
		{
			byte[] bytes = BitConverter.GetBytes(packed);
			int num = 0;
			coolingDown = bytes.ReadBool(ref num);
			cooldownStartTime = (float)(bytes.Read(ref num, 25) / 100.0);
			fillAmount = bytes.Read(ref num, 6) / 63f;
		}

		// Token: 0x04004D5D RID: 19805
		[AssignInCorePrefab]
		public ContainerLiquid containerLiquid;

		// Token: 0x04004D5E RID: 19806
		[AssignInCorePrefab]
		[SoundBankInfo]
		public SoundBankPlayer sipSoundBankPlayer;

		// Token: 0x04004D5F RID: 19807
		[AssignInCorePrefab]
		public float sipRate = 0.1f;

		// Token: 0x04004D60 RID: 19808
		[AssignInCorePrefab]
		public float sipSoundCooldown = 0.5f;

		// Token: 0x04004D61 RID: 19809
		[AssignInCorePrefab]
		public Vector3 headToMouthOffset = new Vector3(0f, 0.0208f, 0.171f);

		// Token: 0x04004D62 RID: 19810
		[AssignInCorePrefab]
		public float sipRadius = 0.15f;

		// Token: 0x04004D63 RID: 19811
		private float lastTimeSipSoundPlayed;

		// Token: 0x04004D64 RID: 19812
		private bool wasSipping;

		// Token: 0x04004D65 RID: 19813
		private bool coolingDown;

		// Token: 0x04004D66 RID: 19814
		private bool wasCoolingDown;

		// Token: 0x04004D67 RID: 19815
		private byte[] myByteArray;
	}
}
