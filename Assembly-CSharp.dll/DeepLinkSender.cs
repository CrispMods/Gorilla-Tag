using System;
using UnityEngine;

// Token: 0x0200045B RID: 1115
public static class DeepLinkSender
{
	// Token: 0x06001B6A RID: 7018 RVA: 0x00041C8E File Offset: 0x0003FE8E
	public static bool SendDeepLink(ulong deepLinkAppID, string deepLinkMessage, Action<string> onSent)
	{
		Debug.LogError("[DeepLinkSender::SendDeepLink] Called on non-oculus platform!");
		return false;
	}

	// Token: 0x04001E67 RID: 7783
	private static Action<string> currentDeepLinkSentCallback;
}
