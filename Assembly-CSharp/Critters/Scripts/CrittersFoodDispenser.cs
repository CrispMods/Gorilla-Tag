using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Critters.Scripts
{
	// Token: 0x02000CAB RID: 3243
	public class CrittersFoodDispenser : CrittersActor
	{
		// Token: 0x0600520B RID: 21003 RVA: 0x00065360 File Offset: 0x00063560
		public override void Initialize()
		{
			base.Initialize();
			this.heldByPlayer = false;
		}

		// Token: 0x0600520C RID: 21004 RVA: 0x0006536F File Offset: 0x0006356F
		public override void GrabbedBy(CrittersActor grabbingActor, bool positionOverride = false, Quaternion localRotation = default(Quaternion), Vector3 localOffset = default(Vector3), bool disableGrabbing = false)
		{
			base.GrabbedBy(grabbingActor, positionOverride, localRotation, localOffset, disableGrabbing);
			this.heldByPlayer = grabbingActor.isOnPlayer;
		}

		// Token: 0x0600520D RID: 21005 RVA: 0x0006538A File Offset: 0x0006358A
		protected override void RemoteGrabbedBy(CrittersActor grabbingActor)
		{
			base.RemoteGrabbedBy(grabbingActor);
			this.heldByPlayer = grabbingActor.isOnPlayer;
		}

		// Token: 0x0600520E RID: 21006 RVA: 0x0006539F File Offset: 0x0006359F
		public override void Released(bool keepWorldPosition, Quaternion rotation = default(Quaternion), Vector3 position = default(Vector3), Vector3 impulseVelocity = default(Vector3), Vector3 impulseAngularVelocity = default(Vector3))
		{
			base.Released(keepWorldPosition, rotation, position, impulseVelocity, impulseAngularVelocity);
			this.heldByPlayer = false;
		}

		// Token: 0x0600520F RID: 21007 RVA: 0x000653B5 File Offset: 0x000635B5
		protected override void HandleRemoteReleased()
		{
			base.HandleRemoteReleased();
			this.heldByPlayer = false;
		}

		// Token: 0x0400542E RID: 21550
		[FormerlySerializedAs("isHeldByPlayer")]
		public bool heldByPlayer;
	}
}
