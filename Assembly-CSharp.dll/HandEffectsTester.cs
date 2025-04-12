using System;
using GorillaExtensions;
using TagEffects;
using UnityEngine;

// Token: 0x0200022E RID: 558
[RequireComponent(typeof(Collider))]
public class HandEffectsTester : MonoBehaviour, IHandEffectsTrigger
{
	// Token: 0x17000136 RID: 310
	// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x00037F7B File Offset: 0x0003617B
	public bool Static
	{
		get
		{
			return this.isStatic;
		}
	}

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x00037F83 File Offset: 0x00036183
	Transform IHandEffectsTrigger.Transform
	{
		get
		{
			return base.transform;
		}
	}

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x00037F8B File Offset: 0x0003618B
	VRRig IHandEffectsTrigger.Rig
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x00037F8E File Offset: 0x0003618E
	IHandEffectsTrigger.Mode IHandEffectsTrigger.EffectMode
	{
		get
		{
			return this.mode;
		}
	}

	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x00037F96 File Offset: 0x00036196
	bool IHandEffectsTrigger.FingersDown
	{
		get
		{
			return this.mode == IHandEffectsTrigger.Mode.FistBump || this.mode == IHandEffectsTrigger.Mode.HighFive_And_FistBump;
		}
	}

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x06000CBA RID: 3258 RVA: 0x00037FAD File Offset: 0x000361AD
	bool IHandEffectsTrigger.FingersUp
	{
		get
		{
			return this.mode == IHandEffectsTrigger.Mode.HighFive || this.mode == IHandEffectsTrigger.Mode.HighFive_And_FistBump;
		}
	}

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x06000CBB RID: 3259 RVA: 0x00037FC3 File Offset: 0x000361C3
	public bool RightHand { get; }

	// Token: 0x06000CBC RID: 3260 RVA: 0x00037FCB File Offset: 0x000361CB
	private void Awake()
	{
		this.triggerZone = base.GetComponent<Collider>();
	}

	// Token: 0x06000CBD RID: 3261 RVA: 0x00037FD9 File Offset: 0x000361D9
	private void OnEnable()
	{
		if (!HandEffectsTriggerRegistry.HasInstance)
		{
			HandEffectsTriggerRegistry.FindInstance();
		}
		HandEffectsTriggerRegistry.Instance.Register(this);
	}

	// Token: 0x06000CBE RID: 3262 RVA: 0x00037FF2 File Offset: 0x000361F2
	private void OnDisable()
	{
		HandEffectsTriggerRegistry.Instance.Unregister(this);
	}

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x06000CBF RID: 3263 RVA: 0x00037FFF File Offset: 0x000361FF
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
	// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x0003801D File Offset: 0x0003621D
	TagEffectPack IHandEffectsTrigger.CosmeticEffectPack
	{
		get
		{
			return this.cosmeticEffectPack;
		}
	}

	// Token: 0x06000CC1 RID: 3265 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void OnTriggerEntered(IHandEffectsTrigger other)
	{
	}

	// Token: 0x06000CC2 RID: 3266 RVA: 0x0009E4CC File Offset: 0x0009C6CC
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

	// Token: 0x04001025 RID: 4133
	[SerializeField]
	private TagEffectPack cosmeticEffectPack;

	// Token: 0x04001026 RID: 4134
	private Collider triggerZone;

	// Token: 0x04001027 RID: 4135
	public IHandEffectsTrigger.Mode mode;

	// Token: 0x04001028 RID: 4136
	[SerializeField]
	private float triggerRadius = 0.07f;

	// Token: 0x04001029 RID: 4137
	[SerializeField]
	private bool isStatic = true;
}
