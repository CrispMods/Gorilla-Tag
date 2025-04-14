using System;
using UnityEngine;

// Token: 0x0200045B RID: 1115
public static class DeepLinkSender
{
	// Token: 0x06001B67 RID: 7015 RVA: 0x00086E50 File Offset: 0x00085050
	public static bool SendDeepLink(ulong deepLinkAppID, string deepLinkMessage, Action<string> onSent)
	{
		Debug.LogError("[DeepLinkSender::SendDeepLink] Called on non-oculus platform!");
		return false;
	}

	// Token: 0x04001E66 RID: 7782
	private static Action<string> currentDeepLinkSentCallback;
}
