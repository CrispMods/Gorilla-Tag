using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Critters.Scripts
{
	// Token: 0x02000C7D RID: 3197
	public class CrittersFoodDispenser : CrittersActor
	{
		// Token: 0x060050B5 RID: 20661 RVA: 0x0018884A File Offset: 0x00186A4A
		public override void Initialize()
		{
			base.Initialize();
			this.heldByPlayer = false;
		}

		// Token: 0x060050B6 RID: 20662 RVA: 0x00188859 File Offset: 0x00186A59
		public override void GrabbedBy(CrittersActor grabbingActor, bool positionOverride = false, Quaternion localRotation = default(Quaternion), Vector3 localOffset = default(Vector3), bool disableGrabbing = false)
		{
			base.GrabbedBy(grabbingActor, positionOverride, localRotation, localOffset, disableGrabbing);
			this.heldByPlayer = grabbingActor.isOnPlayer;
		}

		// Token: 0x060050B7 RID: 20663 RVA: 0x00188874 File Offset: 0x00186A74
		protected override void RemoteGrabbedBy(CrittersActor grabbingActor)
		{
			base.RemoteGrabbedBy(grabbingActor);
			this.heldByPlayer = grabbingActor.isOnPlayer;
		}

		// Token: 0x060050B8 RID: 20664 RVA: 0x00188889 File Offset: 0x00186A89
		public override void Released(bool keepWorldPosition, Quaternion rotation = default(Quaternion), Vector3 position = default(Vector3), Vector3 impulseVelocity = default(Vector3), Vector3 impulseAngularVelocity = default(Vector3))
		{
			base.Released(keepWorldPosition, rotation, position, impulseVelocity, impulseAngularVelocity);
			this.heldByPlayer = false;
		}

		// Token: 0x060050B9 RID: 20665 RVA: 0x0018889F File Offset: 0x00186A9F
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
