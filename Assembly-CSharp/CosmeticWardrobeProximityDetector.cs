using System;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000438 RID: 1080
[RequireComponent(typeof(SphereCollider))]
public class CosmeticWardrobeProximityDetector : MonoBehaviour
{
	// Token: 0x06001AB9 RID: 6841 RVA: 0x00042184 File Offset: 0x00040384
	private void OnEnable()
	{
		if (this.wardrobeNearbyCollider != null)
		{
			CosmeticWardrobeProximityDetector.wardrobeNearbyDetection.Add(this.wardrobeNearbyCollider);
		}
	}

	// Token: 0x06001ABA RID: 6842 RVA: 0x000421A4 File Offset: 0x000403A4
	private void OnDisable()
	{
		if (this.wardrobeNearbyCollider != null)
		{
			CosmeticWardrobeProximityDetector.wardrobeNearbyDetection.Remove(this.wardrobeNearbyCollider);
		}
	}

	// Token: 0x06001ABB RID: 6843 RVA: 0x000D7750 File Offset: 0x000D5950
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

	// Token: 0x04001D7E RID: 7550
	[SerializeField]
	private SphereCollider wardrobeNearbyCollider;

	// Token: 0x04001D7F RID: 7551
	private static List<SphereCollider> wardrobeNearbyDetection = new List<SphereCollider>();

	// Token: 0x04001D80 RID: 7552
	private static readonly Collider[] overlapColliders = new Collider[20];
}
