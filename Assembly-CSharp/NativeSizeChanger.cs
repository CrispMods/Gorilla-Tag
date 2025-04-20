using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200025B RID: 603
public class NativeSizeChanger : MonoBehaviour
{
	// Token: 0x06000E0A RID: 3594 RVA: 0x0003A0C9 File Offset: 0x000382C9
	public void Activate(NativeSizeChangerSettings settings)
	{
		settings.WorldPosition = base.transform.position;
		settings.ActivationTime = Time.time;
		GTPlayer.Instance.SetNativeScale(settings);
	}
}
