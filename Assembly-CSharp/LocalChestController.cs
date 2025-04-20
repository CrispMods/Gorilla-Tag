using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x020001D3 RID: 467
public class LocalChestController : MonoBehaviour
{
	// Token: 0x06000AED RID: 2797 RVA: 0x000991D0 File Offset: 0x000973D0
	private void OnTriggerEnter(Collider other)
	{
		if (this.isOpen)
		{
			return;
		}
		TransformFollow component = other.GetComponent<TransformFollow>();
		if (component == null)
		{
			return;
		}
		Transform transformToFollow = component.transformToFollow;
		if (transformToFollow == null)
		{
			return;
		}
		VRRig componentInParent = transformToFollow.GetComponentInParent<VRRig>();
		if (componentInParent == null)
		{
			return;
		}
		if (this.playerCollectionVolume != null && !this.playerCollectionVolume.containedRigs.Contains(componentInParent))
		{
			return;
		}
		this.isOpen = true;
		this.director.Play();
	}

	// Token: 0x04000D5E RID: 3422
	public PlayableDirector director;

	// Token: 0x04000D5F RID: 3423
	public MazePlayerCollection playerCollectionVolume;

	// Token: 0x04000D60 RID: 3424
	private bool isOpen;
}
