using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002F7 RID: 759
public class TeleportTransitionWarp : TeleportTransition
{
	// Token: 0x0600123B RID: 4667 RVA: 0x0003C681 File Offset: 0x0003A881
	protected override void LocomotionTeleportOnEnterStateTeleporting()
	{
		base.StartCoroutine(this.DoWarp());
	}

	// Token: 0x0600123C RID: 4668 RVA: 0x0003C690 File Offset: 0x0003A890
	private IEnumerator DoWarp()
	{
		base.LocomotionTeleport.IsTransitioning = true;
		Vector3 startPosition = base.LocomotionTeleport.GetCharacterPosition();
		float elapsedTime = 0f;
		while (elapsedTime < this.TransitionDuration)
		{
			elapsedTime += Time.deltaTime;
			float time = elapsedTime / this.TransitionDuration;
			float positionPercent = this.PositionLerp.Evaluate(time);
			base.LocomotionTeleport.DoWarp(startPosition, positionPercent);
			yield return null;
		}
		base.LocomotionTeleport.DoWarp(startPosition, 1f);
		base.LocomotionTeleport.IsTransitioning = false;
		yield break;
	}

	// Token: 0x0400140A RID: 5130
	[Tooltip("How much time the warp transition takes to complete.")]
	[Range(0.01f, 1f)]
	public float TransitionDuration = 0.5f;

	// Token: 0x0400140B RID: 5131
	[HideInInspector]
	public AnimationCurve PositionLerp = AnimationCurve.Linear(0f, 0f, 1f, 1f);
}
