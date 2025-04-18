﻿using System;
using GorillaExtensions;
using TagEffects;
using UnityEngine;

// Token: 0x0200022E RID: 558
[RequireComponent(typeof(Collider))]
public class HandEffectsTester : MonoBehaviour, IHandEffectsTrigger
{
	// Token: 0x17000136 RID: 310
	// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x00042E21 File Offset: 0x00041021
	public bool Static
	{
		get
		{
			return this.isStatic;
		}
	}

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x00042E29 File Offset: 0x00041029
	Transform IHandEffectsTrigger.Transform
	{
		get
		{
			return base.transform;
		}
	}

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x00042E31 File Offset: 0x00041031
	VRRig IHandEffectsTrigger.Rig
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x00042E34 File Offset: 0x00041034
	IHandEffectsTrigger.Mode IHandEffectsTrigger.EffectMode
	{
		get
		{
			return this.mode;
		}
	}

	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x00042E3C File Offset: 0x0004103C
	bool IHandEffectsTrigger.FingersDown
	{
		get
		{
			return this.mode == IHandEffectsTrigger.Mode.FistBump || this.mode == IHandEffectsTrigger.Mode.HighFive_And_FistBump;
		}
	}

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x00042E53 File Offset: 0x00041053
	bool IHandEffectsTrigger.FingersUp
	{
		get
		{
			return this.mode == IHandEffectsTrigger.Mode.HighFive || this.mode == IHandEffectsTrigger.Mode.HighFive_And_FistBump;
		}
	}

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x00042E69 File Offset: 0x00041069
	public bool RightHand { get; }

	// Token: 0x06000CBA RID: 3258 RVA: 0x00042E71 File Offset: 0x00041071
	private void Awake()
	{
		this.triggerZone = base.GetComponent<Collider>();
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x00042E7F File Offset: 0x0004107F
	private void OnEnable()
	{
		if (!HandEffectsTriggerRegistry.HasInstance)
		{
			HandEffectsTriggerRegistry.FindInstance();
		}
		HandEffectsTriggerRegistry.Instance.Register(this);
	}

	// Token: 0x06000CBC RID: 3260 RVA: 0x00042E98 File Offset: 0x00041098
	private void OnDisable()
	{
		HandEffectsTriggerRegistry.Instance.Unregister(this);
	}

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x06000CBD RID: 3261 RVA: 0x00042EA5 File Offset: 0x000410A5
	Vector3 IHandEffectsTrigger.Velocity
	{
		get
		{
			if (this.mode == IHandEffectsTrigger.Mode.HighFive)
			{
				return Vector3.zero;
			}
			IHandEffectsTrigger.Mode mode = this.mode;
			return Vector3.zero;
		}
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x06000CBE RID: 3262 RVA: 0x00042EC3 File Offset: 0x000410C3
	TagEffectPack IHandEffectsTrigger.CosmeticEffectPack
	{
		get
		{
			return this.cosmeticEffectPack;
		}
	}

	// Token: 0x06000CBF RID: 3263 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnTriggerEntered(IHandEffectsTrigger other)
	{
	}

	// Token: 0x06000CC0 RID: 3264 RVA: 0x00042ECC File Offset: 0x000410CC
	public bool InTriggerZone(IHandEffectsTrigger t)
	{
		if (!(base.transform.position - t.Transform.position).IsShorterThan(this.triggerZone.bounds.size))
		{
			return false;
		}
		RaycastHit raycastHit;
		switch (this.mode)
		{
		case IHandEffectsTrigger.Mode.HighFive:
			return t.FingersUp && this.triggerZone.Raycast(new Ray(t.Transform.position, t.Transform.up), out raycastHit, this.triggerRadius);
		case IHandEffectsTrigger.Mode.FistBump:
			return t.FingersDown && this.triggerZone.Raycast(new Ray(t.Transform.position, t.Transform.right), out raycastHit, this.triggerRadius);
		case IHandEffectsTrigger.Mode.HighFive_And_FistBump:
			return (t.FingersUp && this.triggerZone.Raycast(new Ray(t.Transform.position, t.Transform.up), out raycastHit, this.triggerRadius)) || (t.FingersDown && this.triggerZone.Raycast(new Ray(t.Transform.position, t.Transform.right), out raycastHit, this.triggerRadius));
		}
		return this.triggerZone.Raycast(new Ray(t.Transform.position, this.triggerZone.bounds.center - t.Transform.position), out raycastHit, this.triggerRadius);
	}

	// Token: 0x04001024 RID: 4132
	[SerializeField]
	private TagEffectPack cosmeticEffectPack;

	// Token: 0x04001025 RID: 4133
	private Collider triggerZone;

	// Token: 0x04001026 RID: 4134
	public IHandEffectsTrigger.Mode mode;

	// Token: 0x04001027 RID: 4135
	[SerializeField]
	private float triggerRadius = 0.07f;

	// Token: 0x04001028 RID: 4136
	[SerializeField]
	private bool isStatic = true;
}
