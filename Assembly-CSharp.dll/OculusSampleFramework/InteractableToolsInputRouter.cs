using System;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A59 RID: 2649
	public class InteractableToolsInputRouter : MonoBehaviour
	{
		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x060041E9 RID: 16873 RVA: 0x00170514 File Offset: 0x0016E714
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

		// Token: 0x060041EA RID: 16874 RVA: 0x00170560 File Offset: 0x0016E760
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

		// Token: 0x060041EB RID: 16875 RVA: 0x001705BC File Offset: 0x0016E7BC
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

		// Token: 0x060041EC RID: 16876 RVA: 0x00170618 File Offset: 0x0016E818
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

		// Token: 0x060041ED RID: 16877 RVA: 0x0005A2E9 File Offset: 0x000584E9
		private bool UpdateToolsAndEnableState(HashSet<InteractableTool> tools, bool toolsAreEnabledThisFrame)
		{
			bool result = this.UpdateTools(tools, !toolsAreEnabledThisFrame);
			this.ToggleToolsEnableState(tools, toolsAreEnabledThisFrame);
			return result;
		}

		// Token: 0x060041EE RID: 16878 RVA: 0x001706F4 File Offset: 0x0016E8F4
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

		// Token: 0x060041EF RID: 16879 RVA: 0x00170808 File Offset: 0x0016EA08
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

		// Token: 0x04004303 RID: 17155
		private static InteractableToolsInputRouter _instance;

		// Token: 0x04004304 RID: 17156
		private bool _leftPinch;

		// Token: 0x04004305 RID: 17157
		private bool _rightPinch;

		// Token: 0x04004306 RID: 17158
		private HashSet<InteractableTool> _leftHandNearTools = new HashSet<InteractableTool>();

		// Token: 0x04004307 RID: 17159
		private HashSet<InteractableTool> _leftHandFarTools = new HashSet<InteractableTool>();

		// Token: 0x04004308 RID: 17160
		private HashSet<InteractableTool> _rightHandNearTools = new HashSet<InteractableTool>();

		// Token: 0x04004309 RID: 17161
		private HashSet<InteractableTool> _rightHandFarTools = new HashSet<InteractableTool>();
	}
}
