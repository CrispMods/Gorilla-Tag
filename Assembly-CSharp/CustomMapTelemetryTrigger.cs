using System;
using GorillaLocomotion;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000675 RID: 1653
public class CustomMapTelemetryTrigger : MonoBehaviour
{
	// Token: 0x06002909 RID: 10505 RVA: 0x0004BCA5 File Offset: 0x00049EA5
	public void OnTriggerEnter(Collider other)
	{
		if (other == GTPlayer.Instance.headCollider && CustomMapTelemetry.IsActive)
		{
			CustomMapTelemetry.EndMapTracking();
		}
	}

	// Token: 0x0600290A RID: 10506 RVA: 0x0004BCC5 File Offset: 0x00049EC5
	public void OnTriggerExit(Collider other)
	{
		if (other == GTPlayer.Instance.headCollider && GorillaComputer.instance.IsPlayerInVirtualStump() && !CustomMapTelemetry.IsActive)
		{
			CustomMapTelemetry.StartMapTracking();
		}
	}
}
