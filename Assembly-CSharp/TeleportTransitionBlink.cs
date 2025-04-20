using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002F4 RID: 756
public class TeleportTransitionBlink : TeleportTransition
{
	// Token: 0x06001230 RID: 4656 RVA: 0x0003C637 File Offset: 0x0003A837
	protected override void LocomotionTeleportOnEnterStateTeleporting()
	{
		base.StartCoroutine(this.BlinkCoroutine());
	}

	// Token: 0x06001231 RID: 4657 RVA: 0x0003C646 File Offset: 0x0003A846
	protected IEnumerator BlinkCoroutine()
	{
		base.LocomotionTeleport.IsTransitioning = true;
		float elapsedTime = 0f;
		float teleportTime = this.TransitionDuration * this.TeleportDelay;
		bool teleported = false;
		while (elapsedTime < this.TransitionDuration)
		{
			yield return null;
			elapsedTime += Time.deltaTime;
			if (!teleported && elapsedTime >= teleportTime)
			{
				teleported = true;
				base.LocomotionTeleport.DoTeleport();
			}
		}
		base.LocomotionTeleport.IsTransitioning = false;
		yield break;
	}

	// Token: 0x04001401 RID: 5121
	[Tooltip("How long the transition takes. Usually this is greater than Teleport Delay.")]
	[Range(0.01f, 2f)]
	public float TransitionDuration = 0.5f;

	// Token: 0x04001402 RID: 5122
	[Tooltip("At what percentage of the elapsed transition time does the teleport occur?")]
	[Range(0f, 1f)]
	public float TeleportDelay = 0.5f;

	// Token: 0x04001403 RID: 5123
	[Tooltip("Fade to black over the duration of the transition")]
	public AnimationCurve FadeLevels = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.5f, 1f),
		new Keyframe(1f, 0f)
	});
}
