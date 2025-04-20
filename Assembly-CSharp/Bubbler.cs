using System;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000453 RID: 1107
public class Bubbler : TransferrableObject
{
	// Token: 0x06001B4B RID: 6987 RVA: 0x000D9634 File Offset: 0x000D7834
	public override void OnSpawn(VRRig rig)
	{
		base.OnSpawn(rig);
		this.hasParticleSystem = (this.bubbleParticleSystem != null);
		if (this.hasParticleSystem)
		{
			this.bubbleParticleArray = new ParticleSystem.Particle[this.bubbleParticleSystem.main.maxParticles];
			this.bubbleParticleSystem.trigger.SetCollider(0, GorillaTagger.Instance.leftHandTriggerCollider.GetComponent<SphereCollider>());
			this.bubbleParticleSystem.trigger.SetCollider(1, GorillaTagger.Instance.rightHandTriggerCollider.GetComponent<SphereCollider>());
		}
		this.initialTriggerDuration = 0.05f;
		this.itemState = TransferrableObject.ItemStates.State0;
	}

	// Token: 0x06001B4C RID: 6988 RVA: 0x000D96D8 File Offset: 0x000D78D8
	internal override void OnEnable()
	{
		base.OnEnable();
		this.itemState = TransferrableObject.ItemStates.State0;
		this.hasBubblerAudio = (this.bubblerAudio != null && this.bubblerAudio.clip != null);
		this.hasPopBubbleAudio = (this.popBubbleAudio != null && this.popBubbleAudio.clip != null);
		this.hasFan = (this.fan != null);
		this.hasActiveOnlyComponent = (this.gameObjectActiveOnlyWhileTriggerDown != null);
	}

	// Token: 0x06001B4D RID: 6989 RVA: 0x000D9768 File Offset: 0x000D7968
	private void InitToDefault()
	{
		this.itemState = TransferrableObject.ItemStates.State0;
		if (this.hasParticleSystem && this.bubbleParticleSystem.isPlaying)
		{
			this.bubbleParticleSystem.Stop();
		}
		if (this.hasBubblerAudio && this.bubblerAudio.isPlaying)
		{
			this.bubblerAudio.GTStop();
		}
	}

	// Token: 0x06001B4E RID: 6990 RVA: 0x000D97BC File Offset: 0x000D79BC
	internal override void OnDisable()
	{
		base.OnDisable();
		this.itemState = TransferrableObject.ItemStates.State0;
		if (this.hasParticleSystem && this.bubbleParticleSystem.isPlaying)
		{
			this.bubbleParticleSystem.Stop();
		}
		if (this.hasBubblerAudio && this.bubblerAudio.isPlaying)
		{
			this.bubblerAudio.GTStop();
		}
		this.currentParticles.Clear();
		this.particleInfoDict.Clear();
	}

	// Token: 0x06001B4F RID: 6991 RVA: 0x000428F1 File Offset: 0x00040AF1
	public override void ResetToDefaultState()
	{
		base.ResetToDefaultState();
		this.InitToDefault();
	}

	// Token: 0x06001B50 RID: 6992 RVA: 0x000428FF File Offset: 0x00040AFF
	protected override void LateUpdateLocal()
	{
		base.LateUpdateLocal();
		if (!this._worksInWater && GTPlayer.Instance.InWater)
		{
			this.itemState = TransferrableObject.ItemStates.State0;
		}
	}

	// Token: 0x06001B51 RID: 6993 RVA: 0x000D982C File Offset: 0x000D7A2C
	protected override void LateUpdateShared()
	{
		base.LateUpdateShared();
		if (!this.IsMyItem() && base.myOnlineRig != null && base.myOnlineRig.muted)
		{
			this.itemState = TransferrableObject.ItemStates.State0;
		}
		bool forLeftController = this.currentState == TransferrableObject.PositionState.InLeftHand;
		bool enabled = this.itemState != TransferrableObject.ItemStates.State0;
		Behaviour[] array = this.behavioursToEnableWhenTriggerPressed;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = enabled;
		}
		if (this.itemState == TransferrableObject.ItemStates.State0)
		{
			if (this.hasParticleSystem && this.bubbleParticleSystem.isPlaying)
			{
				this.bubbleParticleSystem.Stop();
			}
			if (this.hasBubblerAudio && this.bubblerAudio.isPlaying)
			{
				this.bubblerAudio.GTStop();
			}
			if (this.hasActiveOnlyComponent)
			{
				this.gameObjectActiveOnlyWhileTriggerDown.SetActive(false);
			}
		}
		else
		{
			if (this.hasParticleSystem && !this.bubbleParticleSystem.isEmitting)
			{
				this.bubbleParticleSystem.Play();
			}
			if (this.hasBubblerAudio && !this.bubblerAudio.isPlaying)
			{
				this.bubblerAudio.GTPlay();
			}
			if (this.hasActiveOnlyComponent && !this.gameObjectActiveOnlyWhileTriggerDown.activeSelf)
			{
				this.gameObjectActiveOnlyWhileTriggerDown.SetActive(true);
			}
			if (this.IsMyItem())
			{
				this.initialTriggerPull = Time.time;
				GorillaTagger.Instance.StartVibration(forLeftController, this.triggerStrength, this.initialTriggerDuration);
				if (Time.time > this.initialTriggerPull + this.initialTriggerDuration)
				{
					GorillaTagger.Instance.StartVibration(forLeftController, this.ongoingStrength, Time.deltaTime);
				}
			}
			if (this.hasFan)
			{
				if (!this.fanYaxisinstead)
				{
					float z = this.fan.transform.localEulerAngles.z + this.rotationSpeed * Time.fixedDeltaTime;
					this.fan.transform.localEulerAngles = new Vector3(0f, 0f, z);
				}
				else
				{
					float y = this.fan.transform.localEulerAngles.y + this.rotationSpeed * Time.fixedDeltaTime;
					this.fan.transform.localEulerAngles = new Vector3(0f, y, 0f);
				}
			}
		}
		if (this.hasParticleSystem && (!this.allBubblesPopped || this.itemState == TransferrableObject.ItemStates.State1))
		{
			int particles = this.bubbleParticleSystem.GetParticles(this.bubbleParticleArray);
			this.allBubblesPopped = (particles <= 0);
			if (!this.allBubblesPopped)
			{
				for (int j = 0; j < particles; j++)
				{
					if (this.currentParticles.Contains(this.bubbleParticleArray[j].randomSeed))
					{
						this.currentParticles.Remove(this.bubbleParticleArray[j].randomSeed);
					}
				}
				foreach (uint key in this.currentParticles)
				{
					if (this.particleInfoDict.TryGetValue(key, out this.outPosition))
					{
						if (this.hasPopBubbleAudio)
						{
							GTAudioSourceExtensions.GTPlayClipAtPoint(this.popBubbleAudio.clip, this.outPosition);
						}
						this.particleInfoDict.Remove(key);
					}
				}
				this.currentParticles.Clear();
				for (int k = 0; k < particles; k++)
				{
					if (this.particleInfoDict.TryGetValue(this.bubbleParticleArray[k].randomSeed, out this.outPosition))
					{
						this.particleInfoDict[this.bubbleParticleArray[k].randomSeed] = this.bubbleParticleArray[k].position;
					}
					else
					{
						this.particleInfoDict.Add(this.bubbleParticleArray[k].randomSeed, this.bubbleParticleArray[k].position);
					}
					this.currentParticles.Add(this.bubbleParticleArray[k].randomSeed);
				}
			}
		}
	}

	// Token: 0x06001B52 RID: 6994 RVA: 0x00042922 File Offset: 0x00040B22
	public override void OnActivate()
	{
		base.OnActivate();
		this.itemState = TransferrableObject.ItemStates.State1;
	}

	// Token: 0x06001B53 RID: 6995 RVA: 0x000344EB File Offset: 0x000326EB
	public override void OnDeactivate()
	{
		base.OnDeactivate();
		this.itemState = TransferrableObject.ItemStates.State0;
	}

	// Token: 0x06001B54 RID: 6996 RVA: 0x00042931 File Offset: 0x00040B31
	public override bool CanActivate()
	{
		return !this.disableActivation;
	}

	// Token: 0x06001B55 RID: 6997 RVA: 0x0004293C File Offset: 0x00040B3C
	public override bool CanDeactivate()
	{
		return !this.disableDeactivation;
	}

	// Token: 0x04001E21 RID: 7713
	[SerializeField]
	private bool _worksInWater = true;

	// Token: 0x04001E22 RID: 7714
	public ParticleSystem bubbleParticleSystem;

	// Token: 0x04001E23 RID: 7715
	private ParticleSystem.Particle[] bubbleParticleArray;

	// Token: 0x04001E24 RID: 7716
	public AudioSource bubblerAudio;

	// Token: 0x04001E25 RID: 7717
	public AudioSource popBubbleAudio;

	// Token: 0x04001E26 RID: 7718
	private List<uint> currentParticles = new List<uint>();

	// Token: 0x04001E27 RID: 7719
	private Dictionary<uint, Vector3> particleInfoDict = new Dictionary<uint, Vector3>();

	// Token: 0x04001E28 RID: 7720
	private Vector3 outPosition;

	// Token: 0x04001E29 RID: 7721
	private bool allBubblesPopped;

	// Token: 0x04001E2A RID: 7722
	public bool disableActivation;

	// Token: 0x04001E2B RID: 7723
	public bool disableDeactivation;

	// Token: 0x04001E2C RID: 7724
	public float rotationSpeed = 5f;

	// Token: 0x04001E2D RID: 7725
	public GameObject fan;

	// Token: 0x04001E2E RID: 7726
	public bool fanYaxisinstead;

	// Token: 0x04001E2F RID: 7727
	public float ongoingStrength = 0.005f;

	// Token: 0x04001E30 RID: 7728
	public float triggerStrength = 0.2f;

	// Token: 0x04001E31 RID: 7729
	private float initialTriggerPull;

	// Token: 0x04001E32 RID: 7730
	private float initialTriggerDuration;

	// Token: 0x04001E33 RID: 7731
	private bool hasBubblerAudio;

	// Token: 0x04001E34 RID: 7732
	private bool hasPopBubbleAudio;

	// Token: 0x04001E35 RID: 7733
	public GameObject gameObjectActiveOnlyWhileTriggerDown;

	// Token: 0x04001E36 RID: 7734
	public Behaviour[] behavioursToEnableWhenTriggerPressed;

	// Token: 0x04001E37 RID: 7735
	private bool hasParticleSystem;

	// Token: 0x04001E38 RID: 7736
	private bool hasFan;

	// Token: 0x04001E39 RID: 7737
	private bool hasActiveOnlyComponent;

	// Token: 0x02000454 RID: 1108
	private enum BubblerState
	{
		// Token: 0x04001E3B RID: 7739
		None = 1,
		// Token: 0x04001E3C RID: 7740
		Bubbling
	}
}
