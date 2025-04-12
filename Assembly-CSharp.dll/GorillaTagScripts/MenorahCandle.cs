using System;
using GorillaNetworking;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009CE RID: 2510
	public class MenorahCandle : MonoBehaviourPun
	{
		// Token: 0x06003E89 RID: 16009 RVA: 0x0002F75F File Offset: 0x0002D95F
		private void Awake()
		{
		}

		// Token: 0x06003E8A RID: 16010 RVA: 0x00163624 File Offset: 0x00161824
		private void Start()
		{
			this.EnableCandle(false);
			this.EnableFlame(false);
			this.litDate = new DateTime(this.year, this.month, this.day);
			this.currentDate = DateTime.Now;
			this.EnableCandle(this.CandleShouldBeVisible());
			this.EnableFlame(false);
			GorillaComputer instance = GorillaComputer.instance;
			instance.OnServerTimeUpdated = (Action)Delegate.Combine(instance.OnServerTimeUpdated, new Action(this.OnTimeChanged));
		}

		// Token: 0x06003E8B RID: 16011 RVA: 0x00057FA1 File Offset: 0x000561A1
		private void UpdateMenorah()
		{
			this.EnableCandle(this.CandleShouldBeVisible());
			if (this.ShouldLightCandle())
			{
				this.EnableFlame(true);
				return;
			}
			if (this.ShouldSnuffCandle())
			{
				this.EnableFlame(false);
			}
		}

		// Token: 0x06003E8C RID: 16012 RVA: 0x00057FCE File Offset: 0x000561CE
		private void OnTimeChanged()
		{
			this.currentDate = GorillaComputer.instance.GetServerTime();
			this.UpdateMenorah();
		}

		// Token: 0x06003E8D RID: 16013 RVA: 0x00057FE8 File Offset: 0x000561E8
		public void OnTimeEventStart()
		{
			this.activeTimeEventDay = true;
			this.UpdateMenorah();
		}

		// Token: 0x06003E8E RID: 16014 RVA: 0x00057FF7 File Offset: 0x000561F7
		public void OnTimeEventEnd()
		{
			this.activeTimeEventDay = false;
			this.UpdateMenorah();
		}

		// Token: 0x06003E8F RID: 16015 RVA: 0x00058006 File Offset: 0x00056206
		private void EnableCandle(bool enable)
		{
			if (this.candle)
			{
				this.candle.SetActive(enable);
			}
		}

		// Token: 0x06003E90 RID: 16016 RVA: 0x00058021 File Offset: 0x00056221
		private bool CandleShouldBeVisible()
		{
			return this.currentDate >= this.litDate;
		}

		// Token: 0x06003E91 RID: 16017 RVA: 0x00058034 File Offset: 0x00056234
		private void EnableFlame(bool enable)
		{
			if (this.flame)
			{
				this.flame.SetActive(enable);
			}
		}

		// Token: 0x06003E92 RID: 16018 RVA: 0x0005804F File Offset: 0x0005624F
		private bool ShouldLightCandle()
		{
			return !this.activeTimeEventDay && this.CandleShouldBeVisible() && !this.flame.activeSelf;
		}

		// Token: 0x06003E93 RID: 16019 RVA: 0x00058071 File Offset: 0x00056271
		private bool ShouldSnuffCandle()
		{
			return this.activeTimeEventDay && this.flame.activeSelf;
		}

		// Token: 0x06003E94 RID: 16020 RVA: 0x00058088 File Offset: 0x00056288
		private void OnDestroy()
		{
			if (GorillaComputer.instance)
			{
				GorillaComputer instance = GorillaComputer.instance;
				instance.OnServerTimeUpdated = (Action)Delegate.Remove(instance.OnServerTimeUpdated, new Action(this.OnTimeChanged));
			}
		}

		// Token: 0x04003FDA RID: 16346
		public int day;

		// Token: 0x04003FDB RID: 16347
		public int month;

		// Token: 0x04003FDC RID: 16348
		public int year;

		// Token: 0x04003FDD RID: 16349
		public GameObject flame;

		// Token: 0x04003FDE RID: 16350
		public GameObject candle;

		// Token: 0x04003FDF RID: 16351
		private DateTime litDate;

		// Token: 0x04003FE0 RID: 16352
		private bool activeTimeEventDay;

		// Token: 0x04003FE1 RID: 16353
		private DateTime currentDate;
	}
}
