using System;
using UnityEngine;

// Token: 0x02000467 RID: 1127
public static class DeepLinkSender
{
	// Token: 0x06001BBB RID: 7099 RVA: 0x00042FC7 File Offset: 0x000411C7
	public static bool SendDeepLink(ulong deepLinkAppID, string deepLinkMessage, Action<string> onSent)
	{
		Debug.LogError("[DeepLinkSender::SendDeepLink] Called on non-oculus platform!");
		return false;
	}

	// Token: 0x04001EB5 RID: 7861
	private static Action<string> currentDeepLinkSentCallback;
}
