using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009A4 RID: 2468
	public class AttachPoint : MonoBehaviour
	{
		// Token: 0x06003C7C RID: 15484 RVA: 0x000577A2 File Offset: 0x000559A2
		private void Start()
		{
			base.transform.parent.parent = null;
		}

		// Token: 0x06003C7D RID: 15485 RVA: 0x00154BE8 File Offset: 0x00152DE8
		private void OnTriggerEnter(Collider other)
		{
			if (this.attachPoint.childCount == 0)
			{
				this.UpdateHookState(false);
			}
			DecorativeItem componentInParent = other.GetComponentInParent<DecorativeItem>();
			if (componentInParent == null || componentInParent.InHand())
			{
				return;
			}
			if (this.IsHooked())
			{
				return;
			}
			this.UpdateHookState(true);
			componentInParent.SnapItem(true, this.attachPoint.position);
		}

		// Token: 0x06003C7E RID: 15486 RVA: 0x00154C44 File Offset: 0x00152E44
		private void OnTriggerExit(Collider other)
		{
			DecorativeItem componentInParent = other.GetComponentInParent<DecorativeItem>();
			if (componentInParent == null || !componentInParent.InHand())
			{
				return;
			}
			this.UpdateHookState(false);
			componentInParent.SnapItem(false, Vector3.zero);
		}

		// Token: 0x06003C7F RID: 15487 RVA: 0x000577B5 File Offset: 0x000559B5
		private void UpdateHookState(bool isHooked)
		{
			this.SetIsHook(isHooked);
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x000577BE File Offset: 0x000559BE
		internal void SetIsHook(bool isHooked)
		{
			this.isHooked = isHooked;
			UnityAction unityAction = this.onHookedChanged;
			if (unityAction == null)
			{
				return;
			}
			unityAction();
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x000577D7 File Offset: 0x000559D7
		public bool IsHooked()
		{
			return this.isHooked || this.attachPoint.childCount != 0;
		}

		// Token: 0x04003D6E RID: 15726
		public Transform attachPoint;

		// Token: 0x04003D6F RID: 15727
		public UnityAction onHookedChanged;

		// Token: 0x04003D70 RID: 15728
		private bool isHooked;

		// Token: 0x04003D71 RID: 15729
		private bool wasHooked;

		// Token: 0x04003D72 RID: 15730
		public bool inForest;
	}
}
