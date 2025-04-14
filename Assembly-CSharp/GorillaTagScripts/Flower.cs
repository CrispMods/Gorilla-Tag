using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009B1 RID: 2481
	public class Flower : MonoBehaviour
	{
		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06003D78 RID: 15736 RVA: 0x00122643 File Offset: 0x00120843
		// (set) Token: 0x06003D79 RID: 15737 RVA: 0x0012264B File Offset: 0x0012084B
		public bool IsWatered { get; private set; }

		// Token: 0x06003D7A RID: 15738 RVA: 0x00122654 File Offset: 0x00120854
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

		// Token: 0x06003D7B RID: 15739 RVA: 0x001226EB File Offset: 0x001208EB
		private void OnDestroy()
		{
			this.timer.onTimerStopped.RemoveListener(new UnityAction<GorillaTimer>(this.HandleOnFlowerTimerEnded));
		}

		// Token: 0x06003D7C RID: 15740 RVA: 0x0012270C File Offset: 0x0012090C
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

		// Token: 0x06003D7D RID: 15741 RVA: 0x0012277C File Offset: 0x0012097C
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

		// Token: 0x06003D7E RID: 15742 RVA: 0x001227D4 File Offset: 0x001209D4
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

		// Token: 0x06003D7F RID: 15743 RVA: 0x001228AD File Offset: 0x00120AAD
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

		// Token: 0x06003D80 RID: 15744 RVA: 0x001228D1 File Offset: 0x00120AD1
		private void ChangeState(Flower.FlowerState state)
		{
			this.lastState = this.currentState;
			this.currentState = state;
		}

		// Token: 0x06003D81 RID: 15745 RVA: 0x001228E6 File Offset: 0x00120AE6
		public Flower.FlowerState GetCurrentState()
		{
			return this.currentState;
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x001228F0 File Offset: 0x00120AF0
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

		// Token: 0x06003D83 RID: 15747 RVA: 0x0012294D File Offset: 0x00120B4D
		public void UpdateVisuals(bool enable)
		{
			this.shouldUpdateVisuals = enable;
			this.meshStatesGameObject.SetActive(enable);
		}

		// Token: 0x06003D84 RID: 15748 RVA: 0x00122964 File Offset: 0x00120B64
		public void AnimCatch()
		{
			if (this.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
			{
				this.OnAnimationIsDone(0);
			}
		}

		// Token: 0x04003ED3 RID: 16083
		private Animator anim;

		// Token: 0x04003ED4 RID: 16084
		private SkinnedMeshRenderer meshRenderer;

		// Token: 0x04003ED5 RID: 16085
		[HideInInspector]
		public GorillaTimer timer;

		// Token: 0x04003ED6 RID: 16086
		private BeePerchPoint perchPoint;

		// Token: 0x04003ED7 RID: 16087
		public ParticleSystem wateredFx;

		// Token: 0x04003ED8 RID: 16088
		public ParticleSystem sparkleFx;

		// Token: 0x04003ED9 RID: 16089
		public GameObject meshStatesGameObject;

		// Token: 0x04003EDA RID: 16090
		public GameObject[] meshStates;

		// Token: 0x04003EDB RID: 16091
		private static readonly int healthy_to_middle = Animator.StringToHash("healthy_to_middle");

		// Token: 0x04003EDC RID: 16092
		private static readonly int middle_to_healthy = Animator.StringToHash("middle_to_healthy");

		// Token: 0x04003EDD RID: 16093
		private static readonly int wilted_to_middle = Animator.StringToHash("wilted_to_middle");

		// Token: 0x04003EDE RID: 16094
		private static readonly int middle_to_wilted = Animator.StringToHash("middle_to_wilted");

		// Token: 0x04003EDF RID: 16095
		private Flower.FlowerState currentState;

		// Token: 0x04003EE0 RID: 16096
		private string id;

		// Token: 0x04003EE1 RID: 16097
		private bool shouldUpdateVisuals;

		// Token: 0x04003EE2 RID: 16098
		private Flower.FlowerState lastState;

		// Token: 0x020009B2 RID: 2482
		public enum FlowerState
		{
			// Token: 0x04003EE5 RID: 16101
			None = -1,
			// Token: 0x04003EE6 RID: 16102
			Healthy,
			// Token: 0x04003EE7 RID: 16103
			Middle,
			// Token: 0x04003EE8 RID: 16104
			Wilted
		}
	}
}
