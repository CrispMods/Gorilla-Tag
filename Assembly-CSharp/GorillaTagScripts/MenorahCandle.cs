using System;
using GorillaNetworking;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009F1 RID: 2545
	public class MenorahCandle : MonoBehaviourPun
	{
		// Token: 0x06003F95 RID: 16277 RVA: 0x00030607 File Offset: 0x0002E807
		private void Awake()
		{
		}

		// Token: 0x06003F96 RID: 16278 RVA: 0x00169648 File Offset: 0x00167848
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

		// Token: 0x06003F97 RID: 16279 RVA: 0x00059838 File Offset: 0x00057A38
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

		// Token: 0x06003F98 RID: 16280 RVA: 0x00059865 File Offset: 0x00057A65
		private void OnTimeChanged()
		{
			this.currentDate = GorillaComputer.instance.GetServerTime();
			this.UpdateMenorah();
		}

		// Token: 0x06003F99 RID: 16281 RVA: 0x0005987F File Offset: 0x00057A7F
		public void OnTimeEventStart()
		{
			this.activeTimeEventDay = true;
			this.UpdateMenorah();
		}

		// Token: 0x06003F9A RID: 16282 RVA: 0x0005988E File Offset: 0x00057A8E
		public void OnTimeEventEnd()
		{
			this.activeTimeEventDay = false;
			this.UpdateMenorah();
		}

		// Token: 0x06003F9B RID: 16283 RVA: 0x0005989D File Offset: 0x00057A9D
		private void EnableCandle(bool enable)
		{
			if (this.candle)
			{
				this.candle.SetActive(enable);
			}
		}

		// Token: 0x06003F9C RID: 16284 RVA: 0x000598B8 File Offset: 0x00057AB8
		private bool CandleShouldBeVisible()
		{
			return this.currentDate >= this.litDate;
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x000598CB File Offset: 0x00057ACB
		private void EnableFlame(bool enable)
		{
			if (this.flame)
			{
				this.flame.SetActive(enable);
			}
		}

		// Token: 0x06003F9E RID: 16286 RVA: 0x000598E6 File Offset: 0x00057AE6
		private bool ShouldLightCandle()
		{
			return !this.activeTimeEventDay && this.CandleShouldBeVisible() && !this.flame.activeSelf;
		}

		// Token: 0x06003F9F RID: 16287 RVA: 0x00059908 File Offset: 0x00057B08
		private bool ShouldSnuffCandle()
		{
			return this.activeTimeEventDay && this.flame.activeSelf;
		}

		// Token: 0x06003FA0 RID: 16288 RVA: 0x0005991F File Offset: 0x00057B1F
		private void OnDestroy()
		{
			if (GorillaComputer.instance)
			{
				GorillaComputer instance = GorillaComputer.instance;
				instance.OnServerTimeUpdated = (Action)Delegate.Remove(instance.OnServerTimeUpdated, new Action(this.OnTimeChanged));
			}
		}

		// Token: 0x040040A2 RID: 16546
		public int day;

		// Token: 0x040040A3 RID: 16547
		public int month;

		// Token: 0x040040A4 RID: 16548
		public int year;

		// Token: 0x040040A5 RID: 16549
		public GameObject flame;

		// Token: 0x040040A6 RID: 16550
		public GameObject candle;

		// Token: 0x040040A7 RID: 16551
		private DateTime litDate;

		// Token: 0x040040A8 RID: 16552
		private bool activeTimeEventDay;

		// Token: 0x040040A9 RID: 16553
		private DateTime currentDate;
	}
}
