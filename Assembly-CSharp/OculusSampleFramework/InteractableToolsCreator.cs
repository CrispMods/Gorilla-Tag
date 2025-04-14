using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A54 RID: 2644
	public class InteractableToolsCreator : MonoBehaviour
	{
		// Token: 0x060041D2 RID: 16850 RVA: 0x00137580 File Offset: 0x00135780
		private void Awake()
		{
			if (this.LeftHandTools != null && this.LeftHandTools.Length != 0)
			{
				base.StartCoroutine(this.AttachToolsToHands(this.LeftHandTools, false));
			}
			if (this.RightHandTools != null && this.RightHandTools.Length != 0)
			{
				base.StartCoroutine(this.AttachToolsToHands(this.RightHandTools, true));
			}
		}

		// Token: 0x060041D3 RID: 16851 RVA: 0x001375D7 File Offset: 0x001357D7
		private IEnumerator AttachToolsToHands(Transform[] toolObjects, bool isRightHand)
		{
			HandsManager handsManagerObj = null;
			while ((handsManagerObj = HandsManager.Instance) == null || !handsManagerObj.IsInitialized())
			{
				yield return null;
			}
			HashSet<Transform> hashSet = new HashSet<Transform>();
			foreach (Transform transform in toolObjects)
			{
				hashSet.Add(transform.transform);
			}
			foreach (Transform toolObject in hashSet)
			{
				OVRSkeleton handSkeletonToAttachTo = isRightHand ? handsManagerObj.RightHandSkeleton : handsManagerObj.LeftHandSkeleton;
				while (handSkeletonToAttachTo == null || handSkeletonToAttachTo.Bones == null)
				{
					yield return null;
				}
				this.AttachToolToHandTransform(toolObject, isRightHand);
				handSkeletonToAttachTo = null;
				toolObject = null;
			}
			HashSet<Transform>.Enumerator enumerator = default(HashSet<Transform>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060041D4 RID: 16852 RVA: 0x001375F4 File Offset: 0x001357F4
		private void AttachToolToHandTransform(Transform tool, bool isRightHanded)
		{
			Transform transform = Object.Instantiate<Transform>(tool).transform;
			transform.localPosition = Vector3.zero;
			InteractableTool component = transform.GetComponent<InteractableTool>();
			component.IsRightHandedTool = isRightHanded;
			component.Initialize();
		}

		// Token: 0x040042E6 RID: 17126
		[SerializeField]
		private Transform[] LeftHandTools;

		// Token: 0x040042E7 RID: 17127
		[SerializeField]
		private Transform[] RightHandTools;
	}
}
