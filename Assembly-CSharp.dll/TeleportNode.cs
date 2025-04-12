using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020008B4 RID: 2228
public class TeleportNode : GorillaTriggerBox
{
	// Token: 0x060035E8 RID: 13800 RVA: 0x0013F9F4 File Offset: 0x0013DBF4
	public override void OnBoxTriggered()
	{
		if (Time.time - this.teleportTime < 0.1f)
		{
			return;
		}
		base.OnBoxTriggered();
		Transform transform;
		Transform transform2;
		if (this.teleportFromRef.TryResolve<Transform>(out transform) && this.teleportToRef.TryResolve<Transform>(out transform2))
		{
			GTPlayer instance = GTPlayer.Instance;
			Vector3 position = transform2.TransformPoint(transform.InverseTransformPoint(instance.transform.position));
			instance.TeleportTo(position, instance.transform.rotation);
			this.teleportTime = Time.time;
		}
	}

	// Token: 0x0400381F RID: 14367
	[SerializeField]
	private XSceneRef teleportFromRef;

	// Token: 0x04003820 RID: 14368
	[SerializeField]
	private XSceneRef teleportToRef;

	// Token: 0x04003821 RID: 14369
	private float teleportTime;
}
