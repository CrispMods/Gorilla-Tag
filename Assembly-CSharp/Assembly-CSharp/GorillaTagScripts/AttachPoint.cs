using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x02000981 RID: 2433
	public class AttachPoint : MonoBehaviour
	{
		// Token: 0x06003B70 RID: 15216 RVA: 0x00111EB9 File Offset: 0x001100B9
		private void Start()
		{
			base.transform.parent.parent = null;
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x00111ECC File Offset: 0x001100CC
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

		// Token: 0x06003B72 RID: 15218 RVA: 0x00111F28 File Offset: 0x00110128
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

		// Token: 0x06003B73 RID: 15219 RVA: 0x00111F61 File Offset: 0x00110161
		private void UpdateHookState(bool isHooked)
		{
			this.SetIsHook(isHooked);
		}

		// Token: 0x06003B74 RID: 15220 RVA: 0x00111F6A File Offset: 0x0011016A
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

		// Token: 0x06003B75 RID: 15221 RVA: 0x00111F83 File Offset: 0x00110183
		public bool IsHooked()
		{
			return this.isHooked || this.attachPoint.childCount != 0;
		}

		// Token: 0x04003CA6 RID: 15526
		public Transform attachPoint;

		// Token: 0x04003CA7 RID: 15527
		public UnityAction onHookedChanged;

		// Token: 0x04003CA8 RID: 15528
		private bool isHooked;

		// Token: 0x04003CA9 RID: 15529
		private bool wasHooked;

		// Token: 0x04003CAA RID: 15530
		public bool inForest;
	}
}
