using System;
using System.Runtime.CompilerServices;
using System.Text;
using Cysharp.Text;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B90 RID: 2960
	public class GTLogErrorLimiter
	{
		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06004ABB RID: 19131 RVA: 0x00169A37 File Offset: 0x00167C37
		// (set) Token: 0x06004ABC RID: 19132 RVA: 0x00169A3F File Offset: 0x00167C3F
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

		// Token: 0x06004ABD RID: 19133 RVA: 0x00169A51 File Offset: 0x00167C51
		public GTLogErrorLimiter(string baseMessage, int countdown = 10, string occurrencesJoinString = "\n- ")
		{
			this.baseMessage = baseMessage;
			this.countdown = countdown;
			this.sb = ZString.CreateStringBuilder();
			this.sb.Append(this.baseMessage);
			this.occurrencesJoinString = occurrencesJoinString;
		}

		// Token: 0x06004ABE RID: 19134 RVA: 0x00169A8C File Offset: 0x00167C8C
		public void Log(string subMessage = "", Object context = null, [CallerMemberName] string caller = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int line = 0)
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

		// Token: 0x06004ABF RID: 19135 RVA: 0x00169B81 File Offset: 0x00167D81
		public void Log(Object obj, Object context = null, [CallerMemberName] string caller = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int line = 0)
		{
			if (!obj)
			{
				this.Log("__NULL__", context, caller, sourceFilePath, line);
				return;
			}
			this.Log(obj.ToString(), null, "Log", "E:\\Dev\\GT\\Assets\\GorillaTag\\Shared\\Scripts\\MonkeFX\\GTLogErrorLimiter.cs", 137);
		}

		// Token: 0x06004AC0 RID: 19136 RVA: 0x00169BB9 File Offset: 0x00167DB9
		public void AddOccurrence(string s)
		{
			this.occurrenceCount++;
			this.sb.Append(this.occurrencesJoinString ?? "\n- ");
			this.sb.Append(s);
		}

		// Token: 0x06004AC1 RID: 19137 RVA: 0x00169BEF File Offset: 0x00167DEF
		public void AddOccurrence(StringBuilder stringBuilder)
		{
			this.occurrenceCount++;
			this.sb.Append(this.occurrencesJoinString ?? "\n- ");
			this.sb.Append<StringBuilder>(stringBuilder);
		}

		// Token: 0x06004AC2 RID: 19138 RVA: 0x00169C28 File Offset: 0x00167E28
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

		// Token: 0x06004AC3 RID: 19139 RVA: 0x00169C84 File Offset: 0x00167E84
		public void LogOccurrences(Component component = null, Object obj = null, [CallerMemberName] string caller = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int line = 0)
		{
			if (this.occurrenceCount <= 0)
			{
				return;
			}
			this.sb.Insert(0, string.Format("Occurred {0} times: ", this.occurrenceCount));
			this.Log("\"" + component.GetComponentPath(int.MaxValue) + "\"", obj, caller, sourceFilePath, line);
		}

		// Token: 0x06004AC4 RID: 19140 RVA: 0x00169CE4 File Offset: 0x00167EE4
		public void LogOccurrences(Utf16ValueStringBuilder subMessage, Object obj = null, [CallerMemberName] string caller = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int line = 0)
		{
			if (this.occurrenceCount <= 0)
			{
				return;
			}
			this.sb.Insert(0, string.Format("Occurred {0} times: ", this.occurrenceCount));
			this.sb.Append<Utf16ValueStringBuilder>(subMessage);
			this.Log("", obj, caller, sourceFilePath, line);
		}

		// Token: 0x04004C2C RID: 19500
		private const string __NULL__ = "__NULL__";

		// Token: 0x04004C2D RID: 19501
		public int countdown;

		// Token: 0x04004C2E RID: 19502
		public int occurrenceCount;

		// Token: 0x04004C2F RID: 19503
		public string occurrencesJoinString;

		// Token: 0x04004C30 RID: 19504
		private string _baseMessage;

		// Token: 0x04004C31 RID: 19505
		public Utf16ValueStringBuilder sb;

		// Token: 0x04004C32 RID: 19506
		private const string k_lastMsgHeader = "!!!! THIS MESSAGE HAS REACHED MAX SPAM COUNT AND WILL NO LONGER BE LOGGED !!!!\n";
	}
}
