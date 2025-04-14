using System;
using System.Collections;
using System.Globalization;
using GorillaNetworking;
using TMPro;
using UnityEngine;

namespace GameObjectScheduling
{
	// Token: 0x02000C7F RID: 3199
	public class CountdownText : MonoBehaviour
	{
		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x060050B7 RID: 20663 RVA: 0x001883D1 File Offset: 0x001865D1
		// (set) Token: 0x060050B8 RID: 20664 RVA: 0x001883DC File Offset: 0x001865DC
		public CountdownTextDate Countdown
		{
			get
			{
				return this.CountdownTo;
			}
			set
			{
				this.CountdownTo = value;
				if (this.CountdownTo.FormatString.Length > 0)
				{
					this.displayTextFormat = this.CountdownTo.FormatString;
				}
				this.displayText.text = this.CountdownTo.DefaultString;
				if (base.gameObject.activeInHierarchy && !this.useExternalTime && this.monitor == null && this.CountdownTo != null)
				{
					this.monitor = base.StartCoroutine(this.MonitorTime());
				}
			}
		}

		// Token: 0x060050B9 RID: 20665 RVA: 0x00188468 File Offset: 0x00186668
		private void Awake()
		{
			this.displayText = base.GetComponent<TMP_Text>();
			this.displayTextFormat = this.displayText.text.Trim();
			this.displayText.text = string.Empty;
			if (this.CountdownTo == null)
			{
				return;
			}
			if (this.displayTextFormat.Length == 0 && this.CountdownTo.FormatString.Length > 0)
			{
				this.displayTextFormat = this.CountdownTo.FormatString;
			}
			this.displayText.text = this.CountdownTo.DefaultString;
		}

		// Token: 0x060050BA RID: 20666 RVA: 0x001884FD File Offset: 0x001866FD
		private void OnEnable()
		{
			if (this.CountdownTo == null)
			{
				return;
			}
			if (this.monitor == null && !this.useExternalTime)
			{
				this.monitor = base.StartCoroutine(this.MonitorTime());
			}
		}

		// Token: 0x060050BB RID: 20667 RVA: 0x00188530 File Offset: 0x00186730
		private void OnDisable()
		{
			this.StopMonitorTime();
			this.StopDisplayRefresh();
		}

		// Token: 0x060050BC RID: 20668 RVA: 0x0018853E File Offset: 0x0018673E
		private IEnumerator MonitorTime()
		{
			while (GorillaComputer.instance == null || GorillaComputer.instance.startupMillis == 0L)
			{
				yield return null;
			}
			this.monitor = null;
			this.targetTime = this.TryParseDateTime();
			if (this.updateDisplay)
			{
				this.StartDisplayRefresh();
			}
			else
			{
				this.RefreshDisplay();
			}
			yield break;
		}

		// Token: 0x060050BD RID: 20669 RVA: 0x0018854D File Offset: 0x0018674D
		private IEnumerator MonitorExternalTime(DateTime countdown)
		{
			while (GorillaComputer.instance == null || GorillaComputer.instance.startupMillis == 0L)
			{
				yield return null;
			}
			this.monitor = null;
			this.targetTime = countdown;
			if (this.updateDisplay)
			{
				this.StartDisplayRefresh();
			}
			else
			{
				this.RefreshDisplay();
			}
			yield break;
		}

		// Token: 0x060050BE RID: 20670 RVA: 0x00188563 File Offset: 0x00186763
		private void StopMonitorTime()
		{
			if (this.monitor != null)
			{
				base.StopCoroutine(this.monitor);
			}
			this.monitor = null;
		}

		// Token: 0x060050BF RID: 20671 RVA: 0x00188580 File Offset: 0x00186780
		public void SetCountdownTime(DateTime countdown)
		{
			this.StopMonitorTime();
			this.StopDisplayRefresh();
			this.monitor = base.StartCoroutine(this.MonitorExternalTime(countdown));
		}

		// Token: 0x060050C0 RID: 20672 RVA: 0x001885A1 File Offset: 0x001867A1
		public void SetFixedText(string text)
		{
			this.StopMonitorTime();
			this.StopDisplayRefresh();
			this.displayText.text = text;
		}

		// Token: 0x060050C1 RID: 20673 RVA: 0x001885BB File Offset: 0x001867BB
		private void StartDisplayRefresh()
		{
			this.StopDisplayRefresh();
			this.displayRefresh = base.StartCoroutine(this.WaitForDisplayRefresh());
		}

		// Token: 0x060050C2 RID: 20674 RVA: 0x001885D5 File Offset: 0x001867D5
		private void StopDisplayRefresh()
		{
			if (this.displayRefresh != null)
			{
				base.StopCoroutine(this.displayRefresh);
			}
			this.displayRefresh = null;
		}

		// Token: 0x060050C3 RID: 20675 RVA: 0x001885F2 File Offset: 0x001867F2
		private IEnumerator WaitForDisplayRefresh()
		{
			for (;;)
			{
				this.RefreshDisplay();
				TimeSpan timeSpan;
				if (this.countdownTime.Days > 0)
				{
					timeSpan = this.countdownTime - TimeSpan.FromDays((double)this.countdownTime.Days);
				}
				else if (this.countdownTime.Hours > 0)
				{
					timeSpan = this.countdownTime - TimeSpan.FromHours((double)this.countdownTime.Hours);
				}
				else if (this.countdownTime.Minutes > 0)
				{
					timeSpan = this.countdownTime - TimeSpan.FromMinutes((double)this.countdownTime.Minutes);
				}
				else
				{
					if (this.countdownTime.Seconds <= 0)
					{
						break;
					}
					timeSpan = this.countdownTime - TimeSpan.FromSeconds((double)this.countdownTime.Seconds);
				}
				yield return new WaitForSeconds((float)timeSpan.TotalSeconds);
			}
			yield break;
		}

		// Token: 0x060050C4 RID: 20676 RVA: 0x00188604 File Offset: 0x00186804
		private void RefreshDisplay()
		{
			this.countdownTime = this.targetTime.Subtract(GorillaComputer.instance.GetServerTime());
			this.displayText.text = CountdownText.GetTimeDisplay(this.countdownTime, this.displayTextFormat, this.CountdownTo.DaysThreshold, string.Empty, this.CountdownTo.DefaultString);
		}

		// Token: 0x060050C5 RID: 20677 RVA: 0x00188665 File Offset: 0x00186865
		public static string GetTimeDisplay(TimeSpan ts, string format)
		{
			return CountdownText.GetTimeDisplay(ts, format, int.MaxValue, string.Empty, string.Empty);
		}

		// Token: 0x060050C6 RID: 20678 RVA: 0x00188680 File Offset: 0x00186880
		public static string GetTimeDisplay(TimeSpan ts, string format, int maxDaysToDisplay, string elapsedString, string overMaxString)
		{
			if (ts.TotalSeconds < 0.0)
			{
				return elapsedString;
			}
			if (ts.TotalDays < (double)maxDaysToDisplay)
			{
				if (ts.Days > 0)
				{
					return string.Format(format, ts.Days, CountdownText.getTimeChunkString(CountdownText.TimeChunk.DAY, ts.Days));
				}
				if (ts.Hours > 0)
				{
					return string.Format(format, ts.Hours, CountdownText.getTimeChunkString(CountdownText.TimeChunk.HOUR, ts.Hours));
				}
				if (ts.Minutes > 0)
				{
					return string.Format(format, ts.Minutes, CountdownText.getTimeChunkString(CountdownText.TimeChunk.MINUTE, ts.Minutes));
				}
				if (ts.Seconds > 0)
				{
					return string.Format(format, ts.Seconds, CountdownText.getTimeChunkString(CountdownText.TimeChunk.SECOND, ts.Seconds));
				}
			}
			return overMaxString;
		}

		// Token: 0x060050C7 RID: 20679 RVA: 0x0018875C File Offset: 0x0018695C
		private static string getTimeChunkString(CountdownText.TimeChunk chunk, int n)
		{
			switch (chunk)
			{
			case CountdownText.TimeChunk.DAY:
				if (n == 1)
				{
					return "DAY";
				}
				return "DAYS";
			case CountdownText.TimeChunk.HOUR:
				if (n == 1)
				{
					return "HOUR";
				}
				return "HOURS";
			case CountdownText.TimeChunk.MINUTE:
				if (n == 1)
				{
					return "MINUTE";
				}
				return "MINUTES";
			case CountdownText.TimeChunk.SECOND:
				if (n == 1)
				{
					return "SECOND";
				}
				return "SECONDS";
			default:
				return string.Empty;
			}
		}

		// Token: 0x060050C8 RID: 20680 RVA: 0x001887C8 File Offset: 0x001869C8
		private DateTime TryParseDateTime()
		{
			DateTime result;
			try
			{
				result = DateTime.Parse(this.CountdownTo.CountdownTo, CultureInfo.InvariantCulture);
			}
			catch
			{
				result = DateTime.MinValue;
			}
			return result;
		}

		// Token: 0x04005328 RID: 21288
		[SerializeField]
		private CountdownTextDate CountdownTo;

		// Token: 0x04005329 RID: 21289
		[SerializeField]
		private bool updateDisplay;

		// Token: 0x0400532A RID: 21290
		[SerializeField]
		private bool useExternalTime;

		// Token: 0x0400532B RID: 21291
		private TMP_Text displayText;

		// Token: 0x0400532C RID: 21292
		private string displayTextFormat;

		// Token: 0x0400532D RID: 21293
		private DateTime targetTime;

		// Token: 0x0400532E RID: 21294
		private TimeSpan countdownTime;

		// Token: 0x0400532F RID: 21295
		private Coroutine monitor;

		// Token: 0x04005330 RID: 21296
		private Coroutine displayRefresh;

		// Token: 0x02000C80 RID: 3200
		private enum TimeChunk
		{
			// Token: 0x04005332 RID: 21298
			DAY,
			// Token: 0x04005333 RID: 21299
			HOUR,
			// Token: 0x04005334 RID: 21300
			MINUTE,
			// Token: 0x04005335 RID: 21301
			SECOND
		}
	}
}
