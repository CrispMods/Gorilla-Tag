using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009D7 RID: 2519
	public class Flower : MonoBehaviour
	{
		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06003E90 RID: 16016 RVA: 0x00058C16 File Offset: 0x00056E16
		// (set) Token: 0x06003E91 RID: 16017 RVA: 0x00058C1E File Offset: 0x00056E1E
		public bool IsWatered { get; private set; }

		// Token: 0x06003E92 RID: 16018 RVA: 0x001644B0 File Offset: 0x001626B0
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

		// Token: 0x06003E93 RID: 16019 RVA: 0x00058C27 File Offset: 0x00056E27
		private void OnDestroy()
		{
			this.timer.onTimerStopped.RemoveListener(new UnityAction<GorillaTimer>(this.HandleOnFlowerTimerEnded));
		}

		// Token: 0x06003E94 RID: 16020 RVA: 0x00164548 File Offset: 0x00162748
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

		// Token: 0x06003E95 RID: 16021 RVA: 0x001645B8 File Offset: 0x001627B8
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

		// Token: 0x06003E96 RID: 16022 RVA: 0x00164610 File Offset: 0x00162810
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

		// Token: 0x06003E97 RID: 16023 RVA: 0x00058C45 File Offset: 0x00056E45
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

		// Token: 0x06003E98 RID: 16024 RVA: 0x00058C69 File Offset: 0x00056E69
		private void ChangeState(Flower.FlowerState state)
		{
			this.lastState = this.currentState;
			this.currentState = state;
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x00058C7E File Offset: 0x00056E7E
		public Flower.FlowerState GetCurrentState()
		{
			return this.currentState;
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x001646EC File Offset: 0x001628EC
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

		// Token: 0x06003E9B RID: 16027 RVA: 0x00058C86 File Offset: 0x00056E86
		public void UpdateVisuals(bool enable)
		{
			this.shouldUpdateVisuals = enable;
			this.meshStatesGameObject.SetActive(enable);
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x0016474C File Offset: 0x0016294C
		public void AnimCatch()
		{
			if (this.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
			{
				this.OnAnimationIsDone(0);
			}
		}

		// Token: 0x04003FAD RID: 16301
		private Animator anim;

		// Token: 0x04003FAE RID: 16302
		private SkinnedMeshRenderer meshRenderer;

		// Token: 0x04003FAF RID: 16303
		[HideInInspector]
		public GorillaTimer timer;

		// Token: 0x04003FB0 RID: 16304
		private BeePerchPoint perchPoint;

		// Token: 0x04003FB1 RID: 16305
		public ParticleSystem wateredFx;

		// Token: 0x04003FB2 RID: 16306
		public ParticleSystem sparkleFx;

		// Token: 0x04003FB3 RID: 16307
		public GameObject meshStatesGameObject;

		// Token: 0x04003FB4 RID: 16308
		public GameObject[] meshStates;

		// Token: 0x04003FB5 RID: 16309
		private static readonly int healthy_to_middle = Animator.StringToHash("healthy_to_middle");

		// Token: 0x04003FB6 RID: 16310
		private static readonly int middle_to_healthy = Animator.StringToHash("middle_to_healthy");

		// Token: 0x04003FB7 RID: 16311
		private static readonly int wilted_to_middle = Animator.StringToHash("wilted_to_middle");

		// Token: 0x04003FB8 RID: 16312
		private static readonly int middle_to_wilted = Animator.StringToHash("middle_to_wilted");

		// Token: 0x04003FB9 RID: 16313
		private Flower.FlowerState currentState;

		// Token: 0x04003FBA RID: 16314
		private string id;

		// Token: 0x04003FBB RID: 16315
		private bool shouldUpdateVisuals;

		// Token: 0x04003FBC RID: 16316
		private Flower.FlowerState lastState;

		// Token: 0x020009D8 RID: 2520
		public enum FlowerState
		{
			// Token: 0x04003FBF RID: 16319
			None = -1,
			// Token: 0x04003FC0 RID: 16320
			Healthy,
			// Token: 0x04003FC1 RID: 16321
			Middle,
			// Token: 0x04003FC2 RID: 16322
			Wilted
		}
	}
}
