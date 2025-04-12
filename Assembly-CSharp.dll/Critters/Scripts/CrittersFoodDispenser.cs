using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Critters.Scripts
{
	// Token: 0x02000C7D RID: 3197
	public class CrittersFoodDispenser : CrittersActor
	{
		// Token: 0x060050B5 RID: 20661 RVA: 0x000638EA File Offset: 0x00061AEA
		public override void Initialize()
		{
			base.Initialize();
			this.heldByPlayer = false;
		}

		// Token: 0x060050B6 RID: 20662 RVA: 0x000638F9 File Offset: 0x00061AF9
		public override void GrabbedBy(CrittersActor grabbingActor, bool positionOverride = false, Quaternion localRotation = default(Quaternion), Vector3 localOffset = default(Vector3), bool disableGrabbing = false)
		{
			base.GrabbedBy(grabbingActor, positionOverride, localRotation, localOffset, disableGrabbing);
			this.heldByPlayer = grabbingActor.isOnPlayer;
		}

		// Token: 0x060050B7 RID: 20663 RVA: 0x00063914 File Offset: 0x00061B14
		protected override void RemoteGrabbedBy(CrittersActor grabbingActor)
		{
			base.RemoteGrabbedBy(grabbingActor);
			this.heldByPlayer = grabbingActor.isOnPlayer;
		}

		// Token: 0x060050B8 RID: 20664 RVA: 0x00063929 File Offset: 0x00061B29
		public override void Released(bool keepWorldPosition, Quaternion rotation = default(Quaternion), Vector3 position = default(Vector3), Vector3 impulseVelocity = default(Vector3), Vector3 impulseAngularVelocity = default(Vector3))
		{
			base.Released(keepWorldPosition, rotation, position, impulseVelocity, impulseAngularVelocity);
			this.heldByPlayer = false;
		}

		// Token: 0x060050B9 RID: 20665 RVA: 0x0006393F File Offset: 0x00061B3F
		protected override void HandleRemoteReleased()
		{
			base.HandleRemoteReleased();
			this.heldByPlayer = false;
		}

		// Token: 0x04005334 RID: 21300
		[FormerlySerializedAs("isHeldByPlayer")]
		public bool heldByPlayer;
	}
}
