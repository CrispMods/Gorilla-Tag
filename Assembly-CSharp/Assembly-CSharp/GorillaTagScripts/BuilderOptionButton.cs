using System;

namespace GorillaTagScripts
{
	// Token: 0x0200098F RID: 2447
	public class BuilderOptionButton : GorillaPressableButton
	{
		// Token: 0x06003BE8 RID: 15336 RVA: 0x001142DF File Offset: 0x001124DF
		public override void Start()
		{
			base.Start();
		}

		// Token: 0x06003BE9 RID: 15337 RVA: 0x000023F4 File Offset: 0x000005F4
		private void OnDestroy()
		{
		}

		// Token: 0x06003BEA RID: 15338 RVA: 0x001142E7 File Offset: 0x001124E7
		public void Setup(Action<BuilderOptionButton, bool> onPressed)
		{
			this.onPressed = onPressed;
		}

		// Token: 0x06003BEB RID: 15339 RVA: 0x001142F0 File Offset: 0x001124F0
		public override void ButtonActivationWithHand(bool isLeftHand)
		{
			Action<BuilderOptionButton, bool> action = this.onPressed;
			if (action == null)
			{
				return;
			}
			action(this, isLeftHand);
		}

		// Token: 0x06003BEC RID: 15340 RVA: 0x00114304 File Offset: 0x00112504
		public void SetPressed(bool pressed)
		{
			this.buttonRenderer.material = (pressed ? this.pressedMaterial : this.unpressedMaterial);
		}

		// Token: 0x04003D1D RID: 15645
		private new Action<BuilderOptionButton, bool> onPressed;
	}
}
