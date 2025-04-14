using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000250 RID: 592
public class NativeSizeChanger : MonoBehaviour
{
	// Token: 0x06000DBF RID: 3519 RVA: 0x000460D8 File Offset: 0x000442D8
	public void Activate(NativeSizeChangerSettings settings)
	{
		settings.WorldPosition = base.transform.position;
		settings.ActivationTime = Time.time;
		GTPlayer.Instance.SetNativeScale(settings);
	}
}
