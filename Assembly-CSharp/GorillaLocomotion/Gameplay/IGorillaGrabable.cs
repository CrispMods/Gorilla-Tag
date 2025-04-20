using System;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B86 RID: 2950
	internal interface IGorillaGrabable
	{
		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x060049EC RID: 18924
		string name { get; }

		// Token: 0x060049ED RID: 18925
		bool MomentaryGrabOnly();

		// Token: 0x060049EE RID: 18926
		bool CanBeGrabbed(GorillaGrabber grabber);

		// Token: 0x060049EF RID: 18927
		void OnGrabbed(GorillaGrabber grabber, out Transform grabbedTransform, out Vector3 localGrabbedPosition);

		// Token: 0x060049F0 RID: 18928
		void OnGrabReleased(GorillaGrabber grabber);
	}
}
