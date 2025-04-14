using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009B4 RID: 2484
	public class Flower : MonoBehaviour
	{
		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06003D84 RID: 15748 RVA: 0x00122C0B File Offset: 0x00120E0B
		// (set) Token: 0x06003D85 RID: 15749 RVA: 0x00122C13 File Offset: 0x00120E13
		public bool IsWatered { get; private set; }

		// Token: 0x06003D86 RID: 15750 RVA: 0x00122C1C File Offset: 0x00120E1C
		private void Awake()
		{
			this.shouldUpdateVisuals = true;
			this.anim = base.GetComponent<Animator>();
			this.timer = base.GetComponent<GorillaTimer>();
			this.perchPoint = base.GetComponent<BeePerchPoint>();
			this.timer.onTimerStopped.AddListener(new UnityAction<GorillaTimer>(this.HandleOnFlowerTimerEnded));
			this.currentState = Flower.FlowerState.None;
			this.wateredFx = this.wateredFx.GetComponent<ParticleSystem>();
			this.IsWatered = false;
			this.meshRenderer = base.GetComponent<SkinnedMeshRenderer>();
			this.meshRenderer.enabled = false;
			this.anim.enabled = false;
		}

		// Token: 0x06003D87 RID: 15751 RVA: 0x00122CB3 File Offset: 0x00120EB3
		private void OnDestroy()
		{
			this.timer.onTimerStopped.RemoveListener(new UnityAction<GorillaTimer>(this.HandleOnFlowerTimerEnded));
		}

		// Token: 0x06003D88 RID: 15752 RVA: 0x00122CD4 File Offset: 0x00120ED4
		public void WaterFlower(bool isWatered = false)
		{
			this.IsWatered = isWatered;
			switch (this.currentState)
			{
			case Flower.FlowerState.None:
				this.UpdateFlowerState(Flower.FlowerState.Healthy, false, true);
				return;
			case Flower.FlowerState.Healthy:
				if (!isWatered)
				{
					this.UpdateFlowerState(Flower.FlowerState.Middle, false, true);
					return;
				}
				break;
			case Flower.FlowerState.Middle:
				if (isWatered)
				{
					this.UpdateFlowerState(Flower.FlowerState.Healthy, true, true);
					return;
				}
				this.UpdateFlowerState(Flower.FlowerState.Wilted, false, true);
				return;
			case Flower.FlowerState.Wilted:
				if (isWatered)
				{
					this.UpdateFlowerState(Flower.FlowerState.Middle, true, true);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06003D89 RID: 15753 RVA: 0x00122D44 File Offset: 0x00120F44
		public void UpdateFlowerState(Flower.FlowerState newState, bool isWatered = false, bool updateVisual = true)
		{
			if (FlowersManager.Instance.IsMine)
			{
				this.timer.RestartTimer();
			}
			this.ChangeState(newState);
			if (this.perchPoint)
			{
				this.perchPoint.enabled = (this.currentState == Flower.FlowerState.Healthy);
			}
			if (updateVisual)
			{
				this.LocalUpdateFlowers(newState, isWatered);
			}
		}

		// Token: 0x06003D8A RID: 15754 RVA: 0x00122D9C File Offset: 0x00120F9C
		private void LocalUpdateFlowers(Flower.FlowerState state, bool isWatered = false)
		{
			GameObject[] array = this.meshStates;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			if (!this.shouldUpdateVisuals)
			{
				this.meshStates[(int)this.currentState].SetActive(true);
				return;
			}
			if (isWatered && this.wateredFx)
			{
				this.wateredFx.Play();
			}
			this.meshRenderer.enabled = true;
			this.anim.enabled = true;
			switch (state)
			{
			case Flower.FlowerState.Healthy:
				this.anim.SetTrigger(Flower.middle_to_healthy);
				return;
			case Flower.FlowerState.Middle:
				if (this.lastState == Flower.FlowerState.Wilted)
				{
					this.anim.SetTrigger(Flower.wilted_to_middle);
					return;
				}
				this.anim.SetTrigger(Flower.healthy_to_middle);
				return;
			case Flower.FlowerState.Wilted:
				this.anim.SetTrigger(Flower.middle_to_wilted);
				return;
			default:
				return;
			}
		}

		// Token: 0x06003D8B RID: 15755 RVA: 0x00122E75 File Offset: 0x00121075
		private void HandleOnFlowerTimerEnded(GorillaTimer _timer)
		{
			if (!FlowersManager.Instance.IsMine)
			{
				return;
			}
			if (this.timer == _timer)
			{
				this.WaterFlower(false);
			}
		}

		// Token: 0x06003D8C RID: 15756 RVA: 0x00122E99 File Offset: 0x00121099
		private void ChangeState(Flower.FlowerState state)
		{
			this.lastState = this.currentState;
			this.currentState = state;
		}

		// Token: 0x06003D8D RID: 15757 RVA: 0x00122EAE File Offset: 0x001210AE
		public Flower.FlowerState GetCurrentState()
		{
			return this.currentState;
		}

		// Token: 0x06003D8E RID: 15758 RVA: 0x00122EB8 File Offset: 0x001210B8
		public void OnAnimationIsDone(int state)
		{
			if (this.meshRenderer.enabled)
			{
				for (int i = 0; i < this.meshStates.Length; i++)
				{
					bool active = i == (int)this.currentState;
					this.meshStates[i].SetActive(active);
				}
				this.anim.enabled = false;
				this.meshRenderer.enabled = false;
			}
		}

		// Token: 0x06003D8F RID: 15759 RVA: 0x00122F15 File Offset: 0x00121115
		public void UpdateVisuals(bool enable)
		{
			this.shouldUpdateVisuals = enable;
			this.meshStatesGameObject.SetActive(enable);
		}

		// Token: 0x06003D90 RID: 15760 RVA: 0x00122F2C File Offset: 0x0012112C
		public void AnimCatch()
		{
			if (this.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
			{
				this.OnAnimationIsDone(0);
			}
		}

		// Token: 0x04003EE5 RID: 16101
		private Animator anim;

		// Token: 0x04003EE6 RID: 16102
		private SkinnedMeshRenderer meshRenderer;

		// Token: 0x04003EE7 RID: 16103
		[HideInInspector]
		public GorillaTimer timer;

		// Token: 0x04003EE8 RID: 16104
		private BeePerchPoint perchPoint;

		// Token: 0x04003EE9 RID: 16105
		public ParticleSystem wateredFx;

		// Token: 0x04003EEA RID: 16106
		public ParticleSystem sparkleFx;

		// Token: 0x04003EEB RID: 16107
		public GameObject meshStatesGameObject;

		// Token: 0x04003EEC RID: 16108
		public GameObject[] meshStates;

		// Token: 0x04003EED RID: 16109
		private static readonly int healthy_to_middle = Animator.StringToHash("healthy_to_middle");

		// Token: 0x04003EEE RID: 16110
		private static readonly int middle_to_healthy = Animator.StringToHash("middle_to_healthy");

		// Token: 0x04003EEF RID: 16111
		private static readonly int wilted_to_middle = Animator.StringToHash("wilted_to_middle");

		// Token: 0x04003EF0 RID: 16112
		private static readonly int middle_to_wilted = Animator.StringToHash("middle_to_wilted");

		// Token: 0x04003EF1 RID: 16113
		private Flower.FlowerState currentState;

		// Token: 0x04003EF2 RID: 16114
		private string id;

		// Token: 0x04003EF3 RID: 16115
		private bool shouldUpdateVisuals;

		// Token: 0x04003EF4 RID: 16116
		private Flower.FlowerState lastState;

		// Token: 0x020009B5 RID: 2485
		public enum FlowerState
		{
			// Token: 0x04003EF7 RID: 16119
			None = -1,
			// Token: 0x04003EF8 RID: 16120
			Healthy,
			// Token: 0x04003EF9 RID: 16121
			Middle,
			// Token: 0x04003EFA RID: 16122
			Wilted
		}
	}
}
