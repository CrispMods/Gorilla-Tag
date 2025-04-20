using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A81 RID: 2689
	public class InteractableToolsCreator : MonoBehaviour
	{
		// Token: 0x06004317 RID: 17175 RVA: 0x0017715C File Offset: 0x0017535C
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

		// Token: 0x06004318 RID: 17176 RVA: 0x0005BC74 File Offset: 0x00059E74
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

		// Token: 0x06004319 RID: 17177 RVA: 0x0005BC91 File Offset: 0x00059E91
		private void AttachToolToHandTransform(Transform tool, bool isRightHanded)
		{
			Transform transform = UnityEngine.Object.Instantiate<Transform>(tool).transform;
			transform.localPosition = Vector3.zero;
			InteractableTool component = transform.GetComponent<InteractableTool>();
			component.IsRightHandedTool = isRightHanded;
			component.Initialize();
		}

		// Token: 0x040043E0 RID: 17376
		[SerializeField]
		private Transform[] LeftHandTools;

		// Token: 0x040043E1 RID: 17377
		[SerializeField]
		private Transform[] RightHandTools;
	}
}
