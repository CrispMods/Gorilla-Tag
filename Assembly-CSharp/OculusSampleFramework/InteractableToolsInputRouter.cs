using System;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A83 RID: 2691
	public class InteractableToolsInputRouter : MonoBehaviour
	{
		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06004322 RID: 17186 RVA: 0x00177398 File Offset: 0x00175598
		public static InteractableToolsInputRouter Instance
		{
			get
			{
				if (InteractableToolsInputRouter._instance == null)
				{
					InteractableToolsInputRouter[] array = UnityEngine.Object.FindObjectsOfType<InteractableToolsInputRouter>();
					if (array.Length != 0)
					{
						InteractableToolsInputRouter._instance = array[0];
						for (int i = 1; i < array.Length; i++)
						{
							UnityEngine.Object.Destroy(array[i].gameObject);
						}
					}
				}
				return InteractableToolsInputRouter._instance;
			}
		}

		// Token: 0x06004323 RID: 17187 RVA: 0x001773E4 File Offset: 0x001755E4
		public void RegisterInteractableTool(InteractableTool interactableTool)
		{
			if (interactableTool.IsRightHandedTool)
			{
				if (interactableTool.IsFarFieldTool)
				{
					this._rightHandFarTools.Add(interactableTool);
					return;
				}
				this._rightHandNearTools.Add(interactableTool);
				return;
			}
			else
			{
				if (interactableTool.IsFarFieldTool)
				{
					this._leftHandFarTools.Add(interactableTool);
					return;
				}
				this._leftHandNearTools.Add(interactableTool);
				return;
			}
		}

		// Token: 0x06004324 RID: 17188 RVA: 0x00177440 File Offset: 0x00175640
		public void UnregisterInteractableTool(InteractableTool interactableTool)
		{
			if (interactableTool.IsRightHandedTool)
			{
				if (interactableTool.IsFarFieldTool)
				{
					this._rightHandFarTools.Remove(interactableTool);
					return;
				}
				this._rightHandNearTools.Remove(interactableTool);
				return;
			}
			else
			{
				if (interactableTool.IsFarFieldTool)
				{
					this._leftHandFarTools.Remove(interactableTool);
					return;
				}
				this._leftHandNearTools.Remove(interactableTool);
				return;
			}
		}

		// Token: 0x06004325 RID: 17189 RVA: 0x0017749C File Offset: 0x0017569C
		private void Update()
		{
			if (!HandsManager.Instance.IsInitialized())
			{
				return;
			}
			bool flag = HandsManager.Instance.LeftHand.IsTracked && HandsManager.Instance.LeftHand.HandConfidence == OVRHand.TrackingConfidence.High;
			bool flag2 = HandsManager.Instance.RightHand.IsTracked && HandsManager.Instance.RightHand.HandConfidence == OVRHand.TrackingConfidence.High;
			bool isPointerPoseValid = HandsManager.Instance.LeftHand.IsPointerPoseValid;
			bool isPointerPoseValid2 = HandsManager.Instance.RightHand.IsPointerPoseValid;
			bool flag3 = this.UpdateToolsAndEnableState(this._leftHandNearTools, flag);
			this.UpdateToolsAndEnableState(this._leftHandFarTools, !flag3 && flag && isPointerPoseValid);
			bool flag4 = this.UpdateToolsAndEnableState(this._rightHandNearTools, flag2);
			this.UpdateToolsAndEnableState(this._rightHandFarTools, !flag4 && flag2 && isPointerPoseValid2);
		}

		// Token: 0x06004326 RID: 17190 RVA: 0x0005BCEB File Offset: 0x00059EEB
		private bool UpdateToolsAndEnableState(HashSet<InteractableTool> tools, bool toolsAreEnabledThisFrame)
		{
			bool result = this.UpdateTools(tools, !toolsAreEnabledThisFrame);
			this.ToggleToolsEnableState(tools, toolsAreEnabledThisFrame);
			return result;
		}

		// Token: 0x06004327 RID: 17191 RVA: 0x00177578 File Offset: 0x00175778
		private bool UpdateTools(HashSet<InteractableTool> tools, bool resetCollisionData = false)
		{
			bool flag = false;
			foreach (InteractableTool interactableTool in tools)
			{
				List<InteractableCollisionInfo> nextIntersectingObjects = interactableTool.GetNextIntersectingObjects();
				if (nextIntersectingObjects.Count > 0 && !resetCollisionData)
				{
					if (!flag)
					{
						flag = (nextIntersectingObjects.Count > 0);
					}
					interactableTool.UpdateCurrentCollisionsBasedOnDepth();
					if (interactableTool.IsFarFieldTool)
					{
						KeyValuePair<Interactable, InteractableCollisionInfo> firstCurrentCollisionInfo = interactableTool.GetFirstCurrentCollisionInfo();
						if (interactableTool.ToolInputState == ToolInputState.PrimaryInputUp)
						{
							firstCurrentCollisionInfo.Value.InteractableCollider = firstCurrentCollisionInfo.Key.ActionCollider;
							firstCurrentCollisionInfo.Value.CollisionDepth = InteractableCollisionDepth.Action;
						}
						else
						{
							firstCurrentCollisionInfo.Value.InteractableCollider = firstCurrentCollisionInfo.Key.ContactCollider;
							firstCurrentCollisionInfo.Value.CollisionDepth = InteractableCollisionDepth.Contact;
						}
						interactableTool.FocusOnInteractable(firstCurrentCollisionInfo.Key, firstCurrentCollisionInfo.Value.InteractableCollider);
					}
				}
				else
				{
					interactableTool.DeFocus();
					interactableTool.ClearAllCurrentCollisionInfos();
				}
				interactableTool.UpdateLatestCollisionData();
			}
			return flag;
		}

		// Token: 0x06004328 RID: 17192 RVA: 0x0017768C File Offset: 0x0017588C
		private void ToggleToolsEnableState(HashSet<InteractableTool> tools, bool enableState)
		{
			foreach (InteractableTool interactableTool in tools)
			{
				if (interactableTool.EnableState != enableState)
				{
					interactableTool.EnableState = enableState;
				}
			}
		}

		// Token: 0x040043EB RID: 17387
		private static InteractableToolsInputRouter _instance;

		// Token: 0x040043EC RID: 17388
		private bool _leftPinch;

		// Token: 0x040043ED RID: 17389
		private bool _rightPinch;

		// Token: 0x040043EE RID: 17390
		private HashSet<InteractableTool> _leftHandNearTools = new HashSet<InteractableTool>();

		// Token: 0x040043EF RID: 17391
		private HashSet<InteractableTool> _leftHandFarTools = new HashSet<InteractableTool>();

		// Token: 0x040043F0 RID: 17392
		private HashSet<InteractableTool> _rightHandNearTools = new HashSet<InteractableTool>();

		// Token: 0x040043F1 RID: 17393
		private HashSet<InteractableTool> _rightHandFarTools = new HashSet<InteractableTool>();
	}
}
