using System;
using System.Collections;
using System.Globalization;
using GorillaNetworking;
using TMPro;
using UnityEngine;

namespace GameObjectScheduling
{
	// Token: 0x02000C82 RID: 3202
	public class CountdownText : MonoBehaviour
	{
		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x060050C3 RID: 20675 RVA: 0x00063982 File Offset: 0x00061B82
		// (set) Token: 0x060050C4 RID: 20676 RVA: 0x001B742C File Offset: 0x001B562C
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

		// Token: 0x060050C5 RID: 20677 RVA: 0x001B74B8 File Offset: 0x001B56B8
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

		// Token: 0x060050C6 RID: 20678 RVA: 0x0006398A File Offset: 0x00061B8A
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

		// Token: 0x060050C7 RID: 20679 RVA: 0x000639BD File Offset: 0x00061BBD
		private void OnDisable()
		{
			this.StopMonitorTime();
			this.StopDisplayRefresh();
		}

		// Token: 0x060050C8 RID: 20680 RVA: 0x000639CB File Offset: 0x00061BCB
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

		// Token: 0x060050C9 RID: 20681 RVA: 0x000639DA File Offset: 0x00061BDA
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

		// Token: 0x060050CA RID: 20682 RVA: 0x000639F0 File Offset: 0x00061BF0
		private void StopMonitorTime()
		{
			if (this.monitor != null)
			{
				base.StopCoroutine(this.monitor);
			}
			this.monitor = null;
		}

		// Token: 0x060050CB RID: 20683 RVA: 0x00063A0D File Offset: 0x00061C0D
		public void SetCountdownTime(DateTime countdown)
		{
			this.StopMonitorTime();
			this.StopDisplayRefresh();
			this.monitor = base.StartCoroutine(this.MonitorExternalTime(countdown));
		}

		// Token: 0x060050CC RID: 20684 RVA: 0x00063A2E File Offset: 0x00061C2E
		public void SetFixedText(string text)
		{
			this.StopMonitorTime();
			this.StopDisplayRefresh();
			this.displayText.text = text;
		}

		// Token: 0x060050CD RID: 20685 RVA: 0x00063A48 File Offset: 0x00061C48
		private void StartDisplayRefresh()
		{
			this.StopDisplayRefresh();
			this.displayRefresh = base.StartCoroutine(this.WaitForDisplayRefresh());
		}

		// Token: 0x060050CE RID: 20686 RVA: 0x00063A62 File Offset: 0x00061C62
		private void StopDisplayRefresh()
		{
			if (this.displayRefresh != null)
			{
				base.StopCoroutine(this.displayRefresh);
			}
			this.displayRefresh = null;
		}

		// Token: 0x060050CF RID: 20687 RVA: 0x00063A7F File Offset: 0x00061C7F
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

		// Token: 0x060050D0 RID: 20688 RVA: 0x001B7550 File Offset: 0x001B5750
		private void RefreshDisplay()
		{
			this.countdownTime = this.targetTime.Subtract(GorillaComputer.instance.GetServerTime());
			this.displayText.text = CountdownText.GetTimeDisplay(this.countdownTime, this.displayTextFormat, this.CountdownTo.DaysThreshold, string.Empty, this.CountdownTo.DefaultString);
		}

		// Token: 0x060050D1 RID: 20689 RVA: 0x00063A8E File Offset: 0x00061C8E
		public static string GetTimeDisplay(TimeSpan ts, string format)
		{
			return CountdownText.GetTimeDisplay(ts, format, int.MaxValue, string.Empty, string.Empty);
		}

		// Token: 0x060050D2 RID: 20690 RVA: 0x001B75B4 File Offset: 0x001B57B4
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

		// Token: 0x060050D3 RID: 20691 RVA: 0x001B7690 File Offset: 0x001B5890
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

		// Token: 0x060050D4 RID: 20692 RVA: 0x001B76FC File Offset: 0x001B58FC
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

		// Token: 0x0400533A RID: 21306
		[SerializeField]
		private CountdownTextDate CountdownTo;

		// Token: 0x0400533B RID: 21307
		[SerializeField]
		private bool updateDisplay;

		// Token: 0x0400533C RID: 21308
		[SerializeField]
		private bool useExternalTime;

		// Token: 0x0400533D RID: 21309
		private TMP_Text displayText;

		// Token: 0x0400533E RID: 21310
		private string displayTextFormat;

		// Token: 0x0400533F RID: 21311
		private DateTime targetTime;

		// Token: 0x04005340 RID: 21312
		private TimeSpan countdownTime;

		// Token: 0x04005341 RID: 21313
		private Coroutine monitor;

		// Token: 0x04005342 RID: 21314
		private Coroutine displayRefresh;

		// Token: 0x02000C83 RID: 3203
		private enum TimeChunk
		{
			// Token: 0x04005344 RID: 21316
			DAY,
			// Token: 0x04005345 RID: 21317
			HOUR,
			// Token: 0x04005346 RID: 21318
			MINUTE,
			// Token: 0x04005347 RID: 21319
			SECOND
		}
	}
}
