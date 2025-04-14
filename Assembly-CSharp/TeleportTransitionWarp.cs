using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002EC RID: 748
public class TeleportTransitionWarp : TeleportTransition
{
	// Token: 0x060011EF RID: 4591 RVA: 0x00054CE7 File Offset: 0x00052EE7
	protected override void LocomotionTeleportOnEnterStateTeleporting()
	{
		base.StartCoroutine(this.DoWarp());
	}

	// Token: 0x060011F0 RID: 4592 RVA: 0x00054CF6 File Offset: 0x00052EF6
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

	// Token: 0x040013C2 RID: 5058
	[Tooltip("How much time the warp transition takes to complete.")]
	[Range(0.01f, 1f)]
	public float TransitionDuration = 0.5f;

	// Token: 0x040013C3 RID: 5059
	[HideInInspector]
	public AnimationCurve PositionLerp = AnimationCurve.Linear(0f, 0f, 1f, 1f);
}
