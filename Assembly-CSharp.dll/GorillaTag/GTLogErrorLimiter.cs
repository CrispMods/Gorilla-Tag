using System;
using System.Runtime.CompilerServices;
using System.Text;
using Cysharp.Text;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B93 RID: 2963
	public class GTLogErrorLimiter
	{
		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06004AC7 RID: 19143 RVA: 0x0006051A File Offset: 0x0005E71A
		// (set) Token: 0x06004AC8 RID: 19144 RVA: 0x00060522 File Offset: 0x0005E722
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

		// Token: 0x06004AC9 RID: 19145 RVA: 0x00060534 File Offset: 0x0005E734
		public GTLogErrorLimiter(string baseMessage, int countdown = 10, string occurrencesJoinString = "\n- ")
		{
			this.baseMessage = baseMessage;
			this.countdown = countdown;
			this.sb = ZString.CreateStringBuilder();
			this.sb.Append(this.baseMessage);
			this.occurrencesJoinString = occurrencesJoinString;
		}

		// Token: 0x06004ACA RID: 19146 RVA: 0x0019C2DC File Offset: 0x0019A4DC
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

		// Token: 0x06004ACB RID: 19147 RVA: 0x0006056D File Offset: 0x0005E76D
		public void Log(UnityEngine.Object obj, UnityEngine.Object context = null, [CallerMemberName] string caller = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int line = 0)
		{
			if (!obj)
			{
				this.Log("__NULL__", context, caller, sourceFilePath, line);
				return;
			}
			this.Log(obj.ToString(), null, "Log", "C:\\Users\\root\\GT\\Assets\\GorillaTag\\Shared\\Scripts\\MonkeFX\\GTLogErrorLimiter.cs", 137);
		}

		// Token: 0x06004ACC RID: 19148 RVA: 0x000605A5 File Offset: 0x0005E7A5
		public void AddOccurrence(string s)
		{
			this.occurrenceCount++;
			this.sb.Append(this.occurrencesJoinString ?? "\n- ");
			this.sb.Append(s);
		}

		// Token: 0x06004ACD RID: 19149 RVA: 0x000605DB File Offset: 0x0005E7DB
		public void AddOccurrence(StringBuilder stringBuilder)
		{
			this.occurrenceCount++;
			this.sb.Append(this.occurrencesJoinString ?? "\n- ");
			this.sb.Append<StringBuilder>(stringBuilder);
		}

		// Token: 0x06004ACE RID: 19150 RVA: 0x0019C3D4 File Offset: 0x0019A5D4
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

		// Token: 0x06004ACF RID: 19151 RVA: 0x0019C430 File Offset: 0x0019A630
		public void LogOccurrences(Component component = null, UnityEngine.Object obj = null, [CallerMemberName] string caller = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int line = 0)
		{
			if (this.occurrenceCount <= 0)
			{
				return;
			}
			this.sb.Insert(0, string.Format("Occurred {0} times: ", this.occurrenceCount));
			this.Log("\"" + component.GetComponentPath(int.MaxValue) + "\"", obj, caller, sourceFilePath, line);
		}

		// Token: 0x06004AD0 RID: 19152 RVA: 0x0019C490 File Offset: 0x0019A690
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

		// Token: 0x04004C3E RID: 19518
		private const string __NULL__ = "__NULL__";

		// Token: 0x04004C3F RID: 19519
		public int countdown;

		// Token: 0x04004C40 RID: 19520
		public int occurrenceCount;

		// Token: 0x04004C41 RID: 19521
		public string occurrencesJoinString;

		// Token: 0x04004C42 RID: 19522
		private string _baseMessage;

		// Token: 0x04004C43 RID: 19523
		public Utf16ValueStringBuilder sb;

		// Token: 0x04004C44 RID: 19524
		private const string k_lastMsgHeader = "!!!! THIS MESSAGE HAS REACHED MAX SPAM COUNT AND WILL NO LONGER BE LOGGED !!!!\n";
	}
}
