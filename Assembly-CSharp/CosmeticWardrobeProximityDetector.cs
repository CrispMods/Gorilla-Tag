using System;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200042C RID: 1068
[RequireComponent(typeof(SphereCollider))]
public class CosmeticWardrobeProximityDetector : MonoBehaviour
{
	// Token: 0x06001A65 RID: 6757 RVA: 0x000823F5 File Offset: 0x000805F5
	private void OnEnable()
	{
		if (this.wardrobeNearbyCollider != null)
		{
			CosmeticWardrobeProximityDetector.wardrobeNearbyDetection.Add(this.wardrobeNearbyCollider);
		}
	}

	// Token: 0x06001A66 RID: 6758 RVA: 0x00082415 File Offset: 0x00080615
	private void OnDisable()
	{
		if (this.wardrobeNearbyCollider != null)
		{
			CosmeticWardrobeProximityDetector.wardrobeNearbyDetection.Remove(this.wardrobeNearbyCollider);
		}
	}

	// Token: 0x06001A67 RID: 6759 RVA: 0x00082438 File Offset: 0x00080638
	public static bool IsUserNearWardrobe(string userID)
	{
		int layerMask = LayerMask.GetMask(new string[]
		{
			"Gorilla Tag Collider"
		}) | LayerMask.GetMask(new string[]
		{
			"Gorilla Body Collider"
		});
		foreach (SphereCollider sphereCollider in CosmeticWardrobeProximityDetector.wardrobeNearbyDetection)
		{
			int num = Physics.OverlapSphereNonAlloc(sphereCollider.transform.position, sphereCollider.radius, CosmeticWardrobeProximityDetector.overlapColliders, layerMask);
			num = Mathf.Min(num, CosmeticWardrobeProximityDetector.overlapColliders.Length);
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					Collider collider = CosmeticWardrobeProximityDetector.overlapColliders[i];
					if (!(collider == null))
					{
						GameObject gameObject = collider.attachedRigidbody.gameObject;
						VRRig component = gameObject.GetComponent<VRRig>();
						if (component == null || component.creator == null || component.creator.IsNull || string.IsNullOrEmpty(component.creator.UserId))
						{
							if (gameObject.GetComponent<GTPlayer>() == null || NetworkSystem.Instance.LocalPlayer == null)
							{
								goto IL_135;
							}
							if (userID == NetworkSystem.Instance.LocalPlayer.UserId)
							{
								return true;
							}
						}
						else if (userID == component.creator.UserId)
						{
							return true;
						}
						CosmeticWardrobeProximityDetector.overlapColliders[i] = null;
					}
					IL_135:;
				}
			}
		}
		return false;
	}

	// Token: 0x04001D2F RID: 7471
	[SerializeField]
	private SphereCollider wardrobeNearbyCollider;

	// Token: 0x04001D30 RID: 7472
	private static List<SphereCollider> wardrobeNearbyDetection = new List<SphereCollider>();

	// Token: 0x04001D31 RID: 7473
	private static readonly Collider[] overlapColliders = new Collider[20];
}
