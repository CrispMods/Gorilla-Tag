using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020008CD RID: 2253
public class TeleportNode : GorillaTriggerBox
{
	// Token: 0x060036A4 RID: 13988 RVA: 0x00144FB4 File Offset: 0x001431B4
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

	// Token: 0x040038CE RID: 14542
	[SerializeField]
	private XSceneRef teleportFromRef;

	// Token: 0x040038CF RID: 14543
	[SerializeField]
	private XSceneRef teleportToRef;

	// Token: 0x040038D0 RID: 14544
	private float teleportTime;
}
