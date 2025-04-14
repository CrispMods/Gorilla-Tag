using System;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000447 RID: 1095
public class Bubbler : TransferrableObject
{
	// Token: 0x06001AF7 RID: 6903 RVA: 0x00084A50 File Offset: 0x00082C50
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

	// Token: 0x06001AF8 RID: 6904 RVA: 0x00084AF4 File Offset: 0x00082CF4
	internal override void OnEnable()
	{
		base.OnEnable();
		this.itemState = TransferrableObject.ItemStates.State0;
		this.hasBubblerAudio = (this.bubblerAudio != null && this.bubblerAudio.clip != null);
		this.hasPopBubbleAudio = (this.popBubbleAudio != null && this.popBubbleAudio.clip != null);
		this.hasFan = (this.fan != null);
		this.hasActiveOnlyComponent = (this.gameObjectActiveOnlyWhileTriggerDown != null);
	}

	// Token: 0x06001AF9 RID: 6905 RVA: 0x00084B84 File Offset: 0x00082D84
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

	// Token: 0x06001AFA RID: 6906 RVA: 0x00084BD8 File Offset: 0x00082DD8
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

	// Token: 0x06001AFB RID: 6907 RVA: 0x00084C48 File Offset: 0x00082E48
	public override void ResetToDefaultState()
	{
		base.ResetToDefaultState();
		this.InitToDefault();
	}

	// Token: 0x06001AFC RID: 6908 RVA: 0x00084C56 File Offset: 0x00082E56
	protected override void LateUpdateLocal()
	{
		base.LateUpdateLocal();
		if (!this._worksInWater && GTPlayer.Instance.InWater)
		{
			this.itemState = TransferrableObject.ItemStates.State0;
		}
	}

	// Token: 0x06001AFD RID: 6909 RVA: 0x00084C7C File Offset: 0x00082E7C
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

	// Token: 0x06001AFE RID: 6910 RVA: 0x00085088 File Offset: 0x00083288
	public override void OnActivate()
	{
		base.OnActivate();
		this.itemState = TransferrableObject.ItemStates.State1;
	}

	// Token: 0x06001AFF RID: 6911 RVA: 0x00020C68 File Offset: 0x0001EE68
	public override void OnDeactivate()
	{
		base.OnDeactivate();
		this.itemState = TransferrableObject.ItemStates.State0;
	}

	// Token: 0x06001B00 RID: 6912 RVA: 0x00085097 File Offset: 0x00083297
	public override bool CanActivate()
	{
		return !this.disableActivation;
	}

	// Token: 0x06001B01 RID: 6913 RVA: 0x000850A2 File Offset: 0x000832A2
	public override bool CanDeactivate()
	{
		return !this.disableDeactivation;
	}

	// Token: 0x04001DD2 RID: 7634
	[SerializeField]
	private bool _worksInWater = true;

	// Token: 0x04001DD3 RID: 7635
	public ParticleSystem bubbleParticleSystem;

	// Token: 0x04001DD4 RID: 7636
	private ParticleSystem.Particle[] bubbleParticleArray;

	// Token: 0x04001DD5 RID: 7637
	public AudioSource bubblerAudio;

	// Token: 0x04001DD6 RID: 7638
	public AudioSource popBubbleAudio;

	// Token: 0x04001DD7 RID: 7639
	private List<uint> currentParticles = new List<uint>();

	// Token: 0x04001DD8 RID: 7640
	private Dictionary<uint, Vector3> particleInfoDict = new Dictionary<uint, Vector3>();

	// Token: 0x04001DD9 RID: 7641
	private Vector3 outPosition;

	// Token: 0x04001DDA RID: 7642
	private bool allBubblesPopped;

	// Token: 0x04001DDB RID: 7643
	public bool disableActivation;

	// Token: 0x04001DDC RID: 7644
	public bool disableDeactivation;

	// Token: 0x04001DDD RID: 7645
	public float rotationSpeed = 5f;

	// Token: 0x04001DDE RID: 7646
	public GameObject fan;

	// Token: 0x04001DDF RID: 7647
	public bool fanYaxisinstead;

	// Token: 0x04001DE0 RID: 7648
	public float ongoingStrength = 0.005f;

	// Token: 0x04001DE1 RID: 7649
	public float triggerStrength = 0.2f;

	// Token: 0x04001DE2 RID: 7650
	private float initialTriggerPull;

	// Token: 0x04001DE3 RID: 7651
	private float initialTriggerDuration;

	// Token: 0x04001DE4 RID: 7652
	private bool hasBubblerAudio;

	// Token: 0x04001DE5 RID: 7653
	private bool hasPopBubbleAudio;

	// Token: 0x04001DE6 RID: 7654
	public GameObject gameObjectActiveOnlyWhileTriggerDown;

	// Token: 0x04001DE7 RID: 7655
	public Behaviour[] behavioursToEnableWhenTriggerPressed;

	// Token: 0x04001DE8 RID: 7656
	private bool hasParticleSystem;

	// Token: 0x04001DE9 RID: 7657
	private bool hasFan;

	// Token: 0x04001DEA RID: 7658
	private bool hasActiveOnlyComponent;

	// Token: 0x02000448 RID: 1096
	private enum BubblerState
	{
		// Token: 0x04001DEC RID: 7660
		None = 1,
		// Token: 0x04001DED RID: 7661
		Bubbling
	}
}
