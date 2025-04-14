using System;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B5C RID: 2908
	internal interface IGorillaGrabable
	{
		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x060048AD RID: 18605
		string name { get; }

		// Token: 0x060048AE RID: 18606
		bool MomentaryGrabOnly();

		// Token: 0x060048AF RID: 18607
		bool CanBeGrabbed(GorillaGrabber grabber);

		// Token: 0x060048B0 RID: 18608
		void OnGrabbed(GorillaGrabber grabber, out Transform grabbedTransform, out Vector3 localGrabbedPosition);

		// Token: 0x060048B1 RID: 18609
		void OnGrabReleased(GorillaGrabber grabber);
	}
}
