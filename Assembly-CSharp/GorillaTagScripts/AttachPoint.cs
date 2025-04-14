using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x0200097E RID: 2430
	public class AttachPoint : MonoBehaviour
	{
		// Token: 0x06003B64 RID: 15204 RVA: 0x001118F1 File Offset: 0x0010FAF1
		private void Start()
		{
			base.transform.parent.parent = null;
		}

		// Token: 0x06003B65 RID: 15205 RVA: 0x00111904 File Offset: 0x0010FB04
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

		// Token: 0x06003B66 RID: 15206 RVA: 0x00111960 File Offset: 0x0010FB60
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

		// Token: 0x06003B67 RID: 15207 RVA: 0x00111999 File Offset: 0x0010FB99
		private void UpdateHookState(bool isHooked)
		{
			this.SetIsHook(isHooked);
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x001119A2 File Offset: 0x0010FBA2
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

		// Token: 0x06003B69 RID: 15209 RVA: 0x001119BB File Offset: 0x0010FBBB
		public bool IsHooked()
		{
			return this.isHooked || this.attachPoint.childCount != 0;
		}

		// Token: 0x04003C94 RID: 15508
		public Transform attachPoint;

		// Token: 0x04003C95 RID: 15509
		public UnityAction onHookedChanged;

		// Token: 0x04003C96 RID: 15510
		private bool isHooked;

		// Token: 0x04003C97 RID: 15511
		private bool wasHooked;

		// Token: 0x04003C98 RID: 15512
		public bool inForest;
	}
}
