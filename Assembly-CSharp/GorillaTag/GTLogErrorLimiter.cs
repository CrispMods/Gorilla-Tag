using System;
using System.Runtime.CompilerServices;
using System.Text;
using Cysharp.Text;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BBD RID: 3005
	public class GTLogErrorLimiter
	{
		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06004C06 RID: 19462 RVA: 0x00061F52 File Offset: 0x00060152
		// (set) Token: 0x06004C07 RID: 19463 RVA: 0x00061F5A File Offset: 0x0006015A
		public string baseMessage
		{
			get
			{
				return this._baseMessage;
			}
			set
			{
				this._baseMessage = (value ?? "__NULL__");
			}
		}

		// Token: 0x06004C08 RID: 19464 RVA: 0x00061F6C File Offset: 0x0006016C
		public GTLogErrorLimiter(string baseMessage, int countdown = 10, string occurrencesJoinString = "\n- ")
		{
			this.baseMessage = baseMessage;
			this.countdown = countdown;
			this.sb = ZString.CreateStringBuilder();
			this.sb.Append(this.baseMessage);
			this.occurrencesJoinString = occurrencesJoinString;
		}

		// Token: 0x06004C09 RID: 19465 RVA: 0x001A32F4 File Offset: 0x001A14F4
		public void Log(string subMessage = "", UnityEngine.Object context = null, [CallerMemberName] string caller = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int line = 0)
		{
			if (this.countdown < 0)
			{
				return;
			}
			if (this.countdown == 0)
			{
				this.sb.Insert(0, "!!!! THIS MESSAGE HAS REACHED MAX SPAM COUNT AND WILL NO LONGER BE LOGGED !!!!\n");
			}
			this.sb.Append(subMessage ?? "__NULL__");
			this.sb.Append("\n\nError origin - Caller: ");
			this.sb.Append(caller ?? "__NULL__");
			this.sb.Append(", Line: ");
			this.sb.Append(line);
			this.sb.Append("File: ");
			this.sb.Append(sourceFilePath ?? "__NULL__");
			Debug.LogError(this.sb.ToString(), context);
			this.sb.Clear();
			this.sb.Append(this.baseMessage);
			this.countdown--;
			this.occurrenceCount = 0;
		}

		// Token: 0x06004C0A RID: 19466 RVA: 0x00061FA5 File Offset: 0x000601A5
		public void Log(UnityEngine.Object obj, UnityEngine.Object context = null, [CallerMemberName] string caller = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int line = 0)
		{
			if (!obj)
			{
				this.Log("__NULL__", context, caller, sourceFilePath, line);
				return;
			}
			this.Log(obj.ToString(), null, "Log", "E:\\Dev\\GT\\Assets\\GorillaTag\\Shared\\Scripts\\MonkeFX\\GTLogErrorLimiter.cs", 137);
		}

		// Token: 0x06004C0B RID: 19467 RVA: 0x00061FDD File Offset: 0x000601DD
		public void AddOccurrence(string s)
		{
			this.occurrenceCount++;
			this.sb.Append(this.occurrencesJoinString ?? "\n- ");
			this.sb.Append(s);
		}

		// Token: 0x06004C0C RID: 19468 RVA: 0x00062013 File Offset: 0x00060213
		public void AddOccurrence(StringBuilder stringBuilder)
		{
			this.occurrenceCount++;
			this.sb.Append(this.occurrencesJoinString ?? "\n- ");
			this.sb.Append<StringBuilder>(stringBuilder);
		}

		// Token: 0x06004C0D RID: 19469 RVA: 0x001A33EC File Offset: 0x001A15EC
		public void AddOccurence(GameObject gObj)
		{
			this.occurrenceCount++;
			if (gObj == null)
			{
				this.AddOccurrence("__NULL__");
				return;
			}
			this.sb.Append(this.occurrencesJoinString ?? "\n- ");
			this.sb.Q(gObj.GetPath());
		}

		// Token: 0x06004C0E RID: 19470 RVA: 0x001A3448 File Offset: 0x001A1648
		public void LogOccurrences(Component component = null, UnityEngine.Object obj = null, [CallerMemberName] string caller = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int line = 0)
		{
			if (this.occurrenceCount <= 0)
			{
				return;
			}
			this.sb.Insert(0, string.Format("Occurred {0} times: ", this.occurrenceCount));
			this.Log("\"" + component.GetComponentPath(int.MaxValue) + "\"", obj, caller, sourceFilePath, line);
		}

		// Token: 0x06004C0F RID: 19471 RVA: 0x001A34A8 File Offset: 0x001A16A8
		public void LogOccurrences(Utf16ValueStringBuilder subMessage, UnityEngine.Object obj = null, [CallerMemberName] string caller = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int line = 0)
		{
			if (this.occurrenceCount <= 0)
			{
				return;
			}
			this.sb.Insert(0, string.Format("Occurred {0} times: ", this.occurrenceCount));
			this.sb.Append<Utf16ValueStringBuilder>(subMessage);
			this.Log("", obj, caller, sourceFilePath, line);
		}

		// Token: 0x04004D22 RID: 19746
		private const string __NULL__ = "__NULL__";

		// Token: 0x04004D23 RID: 19747
		public int countdown;

		// Token: 0x04004D24 RID: 19748
		public int occurrenceCount;

		// Token: 0x04004D25 RID: 19749
		public string occurrencesJoinString;

		// Token: 0x04004D26 RID: 19750
		private string _baseMessage;

		// Token: 0x04004D27 RID: 19751
		public Utf16ValueStringBuilder sb;

		// Token: 0x04004D28 RID: 19752
		private const string k_lastMsgHeader = "!!!! THIS MESSAGE HAS REACHED MAX SPAM COUNT AND WILL NO LONGER BE LOGGED !!!!\n";
	}
}
