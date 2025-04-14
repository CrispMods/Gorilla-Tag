using System;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A56 RID: 2646
	public class InteractableToolsInputRouter : MonoBehaviour
	{
		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x060041DD RID: 16861 RVA: 0x00137834 File Offset: 0x00135A34
		public static InteractableToolsInputRouter Instance
		{
			get
			{
				if (InteractableToolsInputRouter._instance == null)
				{
					InteractableToolsInputRouter[] array = Object.FindObjectsOfType<InteractableToolsInputRouter>();
					if (array.Length != 0)
					{
						InteractableToolsInputRouter._instance = array[0];
						for (int i = 1; i < array.Length; i++)
						{
							Object.Destroy(array[i].gameObject);
						}
					}
				}
				return InteractableToolsInputRouter._instance;
			}
		}

		// Token: 0x060041DE RID: 16862 RVA: 0x00137880 File Offset: 0x00135A80
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

		// Token: 0x060041DF RID: 16863 RVA: 0x001378DC File Offset: 0x00135ADC
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

		// Token: 0x060041E0 RID: 16864 RVA: 0x00137938 File Offset: 0x00135B38
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

		// Token: 0x060041E1 RID: 16865 RVA: 0x00137A12 File Offset: 0x00135C12
		private bool UpdateToolsAndEnableState(HashSet<InteractableTool> tools, bool toolsAreEnabledThisFrame)
		{
			bool result = this.UpdateTools(tools, !toolsAreEnabledThisFrame);
			this.ToggleToolsEnableState(tools, toolsAreEnabledThisFrame);
			return result;
		}

		// Token: 0x060041E2 RID: 16866 RVA: 0x00137A28 File Offset: 0x00135C28
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

		// Token: 0x060041E3 RID: 16867 RVA: 0x00137B3C File Offset: 0x00135D3C
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

		// Token: 0x040042F1 RID: 17137
		private static InteractableToolsInputRouter _instance;

		// Token: 0x040042F2 RID: 17138
		private bool _leftPinch;

		// Token: 0x040042F3 RID: 17139
		private bool _rightPinch;

		// Token: 0x040042F4 RID: 17140
		private HashSet<InteractableTool> _leftHandNearTools = new HashSet<InteractableTool>();

		// Token: 0x040042F5 RID: 17141
		private HashSet<InteractableTool> _leftHandFarTools = new HashSet<InteractableTool>();

		// Token: 0x040042F6 RID: 17142
		private HashSet<InteractableTool> _rightHandNearTools = new HashSet<InteractableTool>();

		// Token: 0x040042F7 RID: 17143
		private HashSet<InteractableTool> _rightHandFarTools = new HashSet<InteractableTool>();
	}
}
