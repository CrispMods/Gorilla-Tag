using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityChan
{
	// Token: 0x02000CAB RID: 3243
	public class IdleChanger : MonoBehaviour
	{
		// Token: 0x060051D4 RID: 20948 RVA: 0x0006441E File Offset: 0x0006261E
		private void Start()
		{
			this.currentState = this.UnityChanA.GetCurrentAnimatorStateInfo(0);
			this.previousState = this.currentState;
			base.StartCoroutine("RandomChange");
			this.kb = Keyboard.current;
		}

		// Token: 0x060051D5 RID: 20949 RVA: 0x001BF17C File Offset: 0x001BD37C
		private void Update()
		{
			if (this.kb.upArrowKey.wasPressedThisFrame || this.kb.spaceKey.wasPressedThisFrame)
			{
				this.UnityChanA.SetBool("Next", true);
				this.UnityChanB.SetBool("Next", true);
			}
			if (this.kb.downArrowKey.wasPressedThisFrame)
			{
				this.UnityChanA.SetBool("Back", true);
				this.UnityChanB.SetBool("Back", true);
			}
			if (this.UnityChanA.GetBool("Next"))
			{
				this.currentState = this.UnityChanA.GetCurrentAnimatorStateInfo(0);
				if (this.previousState.fullPathHash != this.currentState.fullPathHash)
				{
					this.UnityChanA.SetBool("Next", false);
					this.UnityChanB.SetBool("Next", false);
					this.previousState = this.currentState;
				}
			}
			if (this.UnityChanA.GetBool("Back"))
			{
				this.currentState = this.UnityChanA.GetCurrentAnimatorStateInfo(0);
				if (this.previousState.fullPathHash != this.currentState.fullPathHash)
				{
					this.UnityChanA.SetBool("Back", false);
					this.UnityChanB.SetBool("Back", false);
					this.previousState = this.currentState;
				}
			}
		}

		// Token: 0x060051D6 RID: 20950 RVA: 0x001BF2D8 File Offset: 0x001BD4D8
		private void OnGUI()
		{
			if (this.isGUI)
			{
				GUI.Box(new Rect((float)(Screen.width - 110), 10f, 100f, 90f), "Change Motion");
				if (GUI.Button(new Rect((float)(Screen.width - 100), 40f, 80f, 20f), "Next"))
				{
					this.UnityChanA.SetBool("Next", true);
					this.UnityChanB.SetBool("Next", true);
				}
				if (GUI.Button(new Rect((float)(Screen.width - 100), 70f, 80f, 20f), "Back"))
				{
					this.UnityChanA.SetBool("Back", true);
					this.UnityChanB.SetBool("Back", true);
				}
			}
		}

		// Token: 0x060051D7 RID: 20951 RVA: 0x00064455 File Offset: 0x00062655
		private IEnumerator RandomChange()
		{
			for (;;)
			{
				if (this._random)
				{
					float num = UnityEngine.Random.Range(0f, 1f);
					if (num < this._threshold)
					{
						this.UnityChanA.SetBool("Back", true);
						this.UnityChanB.SetBool("Back", true);
					}
					else if (num >= this._threshold)
					{
						this.UnityChanA.SetBool("Next", true);
						this.UnityChanB.SetBool("Next", true);
					}
				}
				yield return new WaitForSeconds(this._interval);
			}
			yield break;
		}

		// Token: 0x040053F8 RID: 21496
		private AnimatorStateInfo currentState;

		// Token: 0x040053F9 RID: 21497
		private AnimatorStateInfo previousState;

		// Token: 0x040053FA RID: 21498
		public bool _random;

		// Token: 0x040053FB RID: 21499
		public float _threshold = 0.5f;

		// Token: 0x040053FC RID: 21500
		public float _interval = 10f;

		// Token: 0x040053FD RID: 21501
		public bool isGUI = true;

		// Token: 0x040053FE RID: 21502
		public Animator UnityChanA;

		// Token: 0x040053FF RID: 21503
		public Animator UnityChanB;

		// Token: 0x04005400 RID: 21504
		private Keyboard kb;
	}
}
