using System;
using GorillaNetworking;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009CB RID: 2507
	public class MenorahCandle : MonoBehaviourPun
	{
		// Token: 0x06003E7D RID: 15997 RVA: 0x000023F4 File Offset: 0x000005F4
		private void Awake()
		{
		}

		// Token: 0x06003E7E RID: 15998 RVA: 0x001283EC File Offset: 0x001265EC
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

		// Token: 0x06003E7F RID: 15999 RVA: 0x0012846A File Offset: 0x0012666A
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

		// Token: 0x06003E80 RID: 16000 RVA: 0x00128497 File Offset: 0x00126697
		private void OnTimeChanged()
		{
			this.currentDate = GorillaComputer.instance.GetServerTime();
			this.UpdateMenorah();
		}

		// Token: 0x06003E81 RID: 16001 RVA: 0x001284B1 File Offset: 0x001266B1
		public void OnTimeEventStart()
		{
			this.activeTimeEventDay = true;
			this.UpdateMenorah();
		}

		// Token: 0x06003E82 RID: 16002 RVA: 0x001284C0 File Offset: 0x001266C0
		public void OnTimeEventEnd()
		{
			this.activeTimeEventDay = false;
			this.UpdateMenorah();
		}

		// Token: 0x06003E83 RID: 16003 RVA: 0x001284CF File Offset: 0x001266CF
		private void EnableCandle(bool enable)
		{
			if (this.candle)
			{
				this.candle.SetActive(enable);
			}
		}

		// Token: 0x06003E84 RID: 16004 RVA: 0x001284EA File Offset: 0x001266EA
		private bool CandleShouldBeVisible()
		{
			return this.currentDate >= this.litDate;
		}

		// Token: 0x06003E85 RID: 16005 RVA: 0x001284FD File Offset: 0x001266FD
		private void EnableFlame(bool enable)
		{
			if (this.flame)
			{
				this.flame.SetActive(enable);
			}
		}

		// Token: 0x06003E86 RID: 16006 RVA: 0x00128518 File Offset: 0x00126718
		private bool ShouldLightCandle()
		{
			return !this.activeTimeEventDay && this.CandleShouldBeVisible() && !this.flame.activeSelf;
		}

		// Token: 0x06003E87 RID: 16007 RVA: 0x0012853A File Offset: 0x0012673A
		private bool ShouldSnuffCandle()
		{
			return this.activeTimeEventDay && this.flame.activeSelf;
		}

		// Token: 0x06003E88 RID: 16008 RVA: 0x00128551 File Offset: 0x00126751
		private void OnDestroy()
		{
			if (GorillaComputer.instance)
			{
				GorillaComputer instance = GorillaComputer.instance;
				instance.OnServerTimeUpdated = (Action)Delegate.Remove(instance.OnServerTimeUpdated, new Action(this.OnTimeChanged));
			}
		}

		// Token: 0x04003FC8 RID: 16328
		public int day;

		// Token: 0x04003FC9 RID: 16329
		public int month;

		// Token: 0x04003FCA RID: 16330
		public int year;

		// Token: 0x04003FCB RID: 16331
		public GameObject flame;

		// Token: 0x04003FCC RID: 16332
		public GameObject candle;

		// Token: 0x04003FCD RID: 16333
		private DateTime litDate;

		// Token: 0x04003FCE RID: 16334
		private bool activeTimeEventDay;

		// Token: 0x04003FCF RID: 16335
		private DateTime currentDate;
	}
}
