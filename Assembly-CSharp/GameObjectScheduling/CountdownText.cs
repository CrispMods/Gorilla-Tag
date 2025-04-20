using System;
using System.Collections;
using System.Globalization;
using GorillaNetworking;
using TMPro;
using UnityEngine;

namespace GameObjectScheduling
{
	// Token: 0x02000CB0 RID: 3248
	public class CountdownText : MonoBehaviour
	{
		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06005219 RID: 21017 RVA: 0x000653F8 File Offset: 0x000635F8
		// (set) Token: 0x0600521A RID: 21018 RVA: 0x001BF510 File Offset: 0x001BD710
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

		// Token: 0x0600521B RID: 21019 RVA: 0x001BF59C File Offset: 0x001BD79C
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

		// Token: 0x0600521C RID: 21020 RVA: 0x00065400 File Offset: 0x00063600
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

		// Token: 0x0600521D RID: 21021 RVA: 0x00065433 File Offset: 0x00063633
		private void OnDisable()
		{
			this.StopMonitorTime();
			this.StopDisplayRefresh();
		}

		// Token: 0x0600521E RID: 21022 RVA: 0x00065441 File Offset: 0x00063641
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

		// Token: 0x0600521F RID: 21023 RVA: 0x00065450 File Offset: 0x00063650
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

		// Token: 0x06005220 RID: 21024 RVA: 0x00065466 File Offset: 0x00063666
		private void StopMonitorTime()
		{
			if (this.monitor != null)
			{
				base.StopCoroutine(this.monitor);
			}
			this.monitor = null;
		}

		// Token: 0x06005221 RID: 21025 RVA: 0x00065483 File Offset: 0x00063683
		public void SetCountdownTime(DateTime countdown)
		{
			this.StopMonitorTime();
			this.StopDisplayRefresh();
			this.monitor = base.StartCoroutine(this.MonitorExternalTime(countdown));
		}

		// Token: 0x06005222 RID: 21026 RVA: 0x000654A4 File Offset: 0x000636A4
		public void SetFixedText(string text)
		{
			this.StopMonitorTime();
			this.StopDisplayRefresh();
			this.displayText.text = text;
		}

		// Token: 0x06005223 RID: 21027 RVA: 0x000654BE File Offset: 0x000636BE
		private void StartDisplayRefresh()
		{
			this.StopDisplayRefresh();
			this.displayRefresh = base.StartCoroutine(this.WaitForDisplayRefresh());
		}

		// Token: 0x06005224 RID: 21028 RVA: 0x000654D8 File Offset: 0x000636D8
		private void StopDisplayRefresh()
		{
			if (this.displayRefresh != null)
			{
				base.StopCoroutine(this.displayRefresh);
			}
			this.displayRefresh = null;
		}

		// Token: 0x06005225 RID: 21029 RVA: 0x000654F5 File Offset: 0x000636F5
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

		// Token: 0x06005226 RID: 21030 RVA: 0x001BF634 File Offset: 0x001BD834
		private void RefreshDisplay()
		{
			this.countdownTime = this.targetTime.Subtract(GorillaComputer.instance.GetServerTime());
			this.displayText.text = CountdownText.GetTimeDisplay(this.countdownTime, this.displayTextFormat, this.CountdownTo.DaysThreshold, string.Empty, this.CountdownTo.DefaultString);
		}

		// Token: 0x06005227 RID: 21031 RVA: 0x00065504 File Offset: 0x00063704
		public static string GetTimeDisplay(TimeSpan ts, string format)
		{
			return CountdownText.GetTimeDisplay(ts, format, int.MaxValue, string.Empty, string.Empty);
		}

		// Token: 0x06005228 RID: 21032 RVA: 0x001BF698 File Offset: 0x001BD898
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

		// Token: 0x06005229 RID: 21033 RVA: 0x001BF774 File Offset: 0x001BD974
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

		// Token: 0x0600522A RID: 21034 RVA: 0x001BF7E0 File Offset: 0x001BD9E0
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

		// Token: 0x04005434 RID: 21556
		[SerializeField]
		private CountdownTextDate CountdownTo;

		// Token: 0x04005435 RID: 21557
		[SerializeField]
		private bool updateDisplay;

		// Token: 0x04005436 RID: 21558
		[SerializeField]
		private bool useExternalTime;

		// Token: 0x04005437 RID: 21559
		private TMP_Text displayText;

		// Token: 0x04005438 RID: 21560
		private string displayTextFormat;

		// Token: 0x04005439 RID: 21561
		private DateTime targetTime;

		// Token: 0x0400543A RID: 21562
		private TimeSpan countdownTime;

		// Token: 0x0400543B RID: 21563
		private Coroutine monitor;

		// Token: 0x0400543C RID: 21564
		private Coroutine displayRefresh;

		// Token: 0x02000CB1 RID: 3249
		private enum TimeChunk
		{
			// Token: 0x0400543E RID: 21566
			DAY,
			// Token: 0x0400543F RID: 21567
			HOUR,
			// Token: 0x04005440 RID: 21568
			MINUTE,
			// Token: 0x04005441 RID: 21569
			SECOND
		}
	}
}
