using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x020001C8 RID: 456
public class LocalChestController : MonoBehaviour
{
	// Token: 0x06000AA1 RID: 2721 RVA: 0x00039B14 File Offset: 0x00037D14
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

	// Token: 0x04000D18 RID: 3352
	public PlayableDirector director;

	// Token: 0x04000D19 RID: 3353
	public MazePlayerCollection playerCollectionVolume;

	// Token: 0x04000D1A RID: 3354
	private bool isOpen;
}
