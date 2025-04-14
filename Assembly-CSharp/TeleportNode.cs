using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020008B1 RID: 2225
public class TeleportNode : GorillaTriggerBox
{
	// Token: 0x060035DC RID: 13788 RVA: 0x000FF328 File Offset: 0x000FD528
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

	// Token: 0x0400380D RID: 14349
	[SerializeField]
	private XSceneRef teleportFromRef;

	// Token: 0x0400380E RID: 14350
	[SerializeField]
	private XSceneRef teleportToRef;

	// Token: 0x0400380F RID: 14351
	private float teleportTime;
}
