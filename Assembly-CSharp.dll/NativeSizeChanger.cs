using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000250 RID: 592
public class NativeSizeChanger : MonoBehaviour
{
	// Token: 0x06000DC1 RID: 3521 RVA: 0x00038E09 File Offset: 0x00037009
	public void Activate(NativeSizeChangerSettings settings)
	{
		settings.WorldPosition = base.transform.position;
		settings.ActivationTime = Time.time;
		GTPlayer.Instance.SetNativeScale(settings);
	}
}
