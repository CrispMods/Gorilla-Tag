using System;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B59 RID: 2905
	internal interface IGorillaGrabable
	{
		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x060048A1 RID: 18593
		string name { get; }

		// Token: 0x060048A2 RID: 18594
		bool MomentaryGrabOnly();

		// Token: 0x060048A3 RID: 18595
		bool CanBeGrabbed(GorillaGrabber grabber);

		// Token: 0x060048A4 RID: 18596
		void OnGrabbed(GorillaGrabber grabber, out Transform grabbedTransform, out Vector3 localGrabbedPosition);

		// Token: 0x060048A5 RID: 18597
		void OnGrabReleased(GorillaGrabber grabber);
	}
}
