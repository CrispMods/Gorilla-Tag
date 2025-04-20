using System;

namespace GorillaTagScripts
{
	// Token: 0x020009B2 RID: 2482
	public class BuilderOptionButton : GorillaPressableButton
	{
		// Token: 0x06003CF4 RID: 15604 RVA: 0x00057CD6 File Offset: 0x00055ED6
		public override void Start()
		{
			base.Start();
		}

		// Token: 0x06003CF5 RID: 15605 RVA: 0x00030607 File Offset: 0x0002E807
		private void OnDestroy()
		{
		}

		// Token: 0x06003CF6 RID: 15606 RVA: 0x00057CDE File Offset: 0x00055EDE
		public void Setup(Action<BuilderOptionButton, bool> onPressed)
		{
			this.onPressed = onPressed;
		}

		// Token: 0x06003CF7 RID: 15607 RVA: 0x00057CE7 File Offset: 0x00055EE7
		public override void ButtonActivationWithHand(bool isLeftHand)
		{
			Action<BuilderOptionButton, bool> action = this.onPressed;
			if (action == null)
			{
				return;
			}
			action(this, isLeftHand);
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x00057CFB File Offset: 0x00055EFB
		public void SetPressed(bool pressed)
		{
			this.buttonRenderer.material = (pressed ? this.pressedMaterial : this.unpressedMaterial);
		}

		// Token: 0x04003DE5 RID: 15845
		private new Action<BuilderOptionButton, bool> onPressed;
	}
}
