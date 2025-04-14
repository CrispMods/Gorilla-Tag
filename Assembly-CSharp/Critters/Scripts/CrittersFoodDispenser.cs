using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Critters.Scripts
{
	// Token: 0x02000C7A RID: 3194
	public class CrittersFoodDispenser : CrittersActor
	{
		// Token: 0x060050A9 RID: 20649 RVA: 0x00188282 File Offset: 0x00186482
		public override void Initialize()
		{
			base.Initialize();
			this.heldByPlayer = false;
		}

		// Token: 0x060050AA RID: 20650 RVA: 0x00188291 File Offset: 0x00186491
		public override void GrabbedBy(CrittersActor grabbingActor, bool positionOverride = false, Quaternion localRotation = default(Quaternion), Vector3 localOffset = default(Vector3), bool disableGrabbing = false)
		{
			base.GrabbedBy(grabbingActor, positionOverride, localRotation, localOffset, disableGrabbing);
			this.heldByPlayer = grabbingActor.isOnPlayer;
		}

		// Token: 0x060050AB RID: 20651 RVA: 0x001882AC File Offset: 0x001864AC
		protected override void RemoteGrabbedBy(CrittersActor grabbingActor)
		{
			base.RemoteGrabbedBy(grabbingActor);
			this.heldByPlayer = grabbingActor.isOnPlayer;
		}

		// Token: 0x060050AC RID: 20652 RVA: 0x001882C1 File Offset: 0x001864C1
		public override void Released(bool keepWorldPosition, Quaternion rotation = default(Quaternion), Vector3 position = default(Vector3), Vector3 impulseVelocity = default(Vector3), Vector3 impulseAngularVelocity = default(Vector3))
		{
			base.Released(keepWorldPosition, rotation, position, impulseVelocity, impulseAngularVelocity);
			this.heldByPlayer = false;
		}

		// Token: 0x060050AD RID: 20653 RVA: 0x001882D7 File Offset: 0x001864D7
		protected override void HandleRemoteReleased()
		{
			base.HandleRemoteReleased();
			this.heldByPlayer = false;
		}

		// Token: 0x04005322 RID: 21282
		[FormerlySerializedAs("isHeldByPlayer")]
		public bool heldByPlayer;
	}
}
