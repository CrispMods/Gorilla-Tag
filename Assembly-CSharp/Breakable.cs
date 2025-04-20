using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004B5 RID: 1205
public class Breakable : MonoBehaviour
{
	// Token: 0x06001D4A RID: 7498 RVA: 0x000440E1 File Offset: 0x000422E1
	private void Awake()
	{
		this._breakSignal.OnSignal += this.BreakRPC;
	}

	// Token: 0x06001D4B RID: 7499 RVA: 0x000E04A8 File Offset: 0x000DE6A8
	private void BreakRPC(int owner, PhotonSignalInfo info)
	{
		VRRig vrrig = base.GetComponent<OwnerRig>();
		if (vrrig == null)
		{
			return;
		}
		if (vrrig.OwningNetPlayer.ActorNumber != owner)
		{
			return;
		}
		this.OnBreak(true, false);
	}

	// Token: 0x06001D4C RID: 7500 RVA: 0x000E04E4 File Offset: 0x000DE6E4
	private void Setup()
	{
		if (this._collider == null)
		{
			SphereCollider collider;
			this.GetOrAddComponent(out collider);
			this._collider = collider;
		}
		this._collider.enabled = true;
		if (this._rigidbody == null)
		{
			this.GetOrAddComponent(out this._rigidbody);
		}
		this._rigidbody.isKinematic = false;
		this._rigidbody.useGravity = false;
		this._rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		this.UpdatePhysMasks();
		if (this.rendererRoot == null)
		{
			this._renderers = base.GetComponentsInChildren<Renderer>();
			return;
		}
		this._renderers = this.rendererRoot.GetComponentsInChildren<Renderer>();
	}

	// Token: 0x06001D4D RID: 7501 RVA: 0x000440FA File Offset: 0x000422FA
	private void OnCollisionEnter(Collision col)
	{
		this.OnBreak(true, true);
	}

	// Token: 0x06001D4E RID: 7502 RVA: 0x000440FA File Offset: 0x000422FA
	private void OnCollisionStay(Collision col)
	{
		this.OnBreak(true, true);
	}

	// Token: 0x06001D4F RID: 7503 RVA: 0x000440FA File Offset: 0x000422FA
	private void OnTriggerEnter(Collider col)
	{
		this.OnBreak(true, true);
	}

	// Token: 0x06001D50 RID: 7504 RVA: 0x000440FA File Offset: 0x000422FA
	private void OnTriggerStay(Collider col)
	{
		this.OnBreak(true, true);
	}

	// Token: 0x06001D51 RID: 7505 RVA: 0x00044104 File Offset: 0x00042304
	private void OnEnable()
	{
		this._breakSignal.Enable();
		this._broken = false;
		this.OnSpawn(true);
	}

	// Token: 0x06001D52 RID: 7506 RVA: 0x0004411F File Offset: 0x0004231F
	private void OnDisable()
	{
		this._breakSignal.Disable();
		this._broken = false;
		this.OnReset(false);
		this.ShowRenderers(false);
	}

	// Token: 0x06001D53 RID: 7507 RVA: 0x000440FA File Offset: 0x000422FA
	public void Break()
	{
		this.OnBreak(true, true);
	}

	// Token: 0x06001D54 RID: 7508 RVA: 0x00044141 File Offset: 0x00042341
	public void Reset()
	{
		this.OnReset(true);
	}

	// Token: 0x06001D55 RID: 7509 RVA: 0x000E058C File Offset: 0x000DE78C
	protected virtual void ShowRenderers(bool visible)
	{
		if (this._renderers.IsNullOrEmpty<Renderer>())
		{
			return;
		}
		for (int i = 0; i < this._renderers.Length; i++)
		{
			Renderer renderer = this._renderers[i];
			if (renderer)
			{
				renderer.forceRenderingOff = !visible;
			}
		}
	}

	// Token: 0x06001D56 RID: 7510 RVA: 0x000E05D8 File Offset: 0x000DE7D8
	protected virtual void OnReset(bool callback = true)
	{
		if (this._breakEffect && this._breakEffect.isPlaying)
		{
			this._breakEffect.Stop();
		}
		this.ShowRenderers(true);
		this._broken = false;
		if (callback)
		{
			UnityEvent<Breakable> unityEvent = this.onReset;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(this);
		}
	}

	// Token: 0x06001D57 RID: 7511 RVA: 0x0004414A File Offset: 0x0004234A
	protected virtual void OnSpawn(bool callback = true)
	{
		this.startTime = Time.time;
		this.endTime = this.startTime + this.canBreakDelay;
		this.ShowRenderers(true);
		if (callback)
		{
			UnityEvent<Breakable> unityEvent = this.onSpawn;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(this);
		}
	}

	// Token: 0x06001D58 RID: 7512 RVA: 0x000E062C File Offset: 0x000DE82C
	protected virtual void OnBreak(bool callback = true, bool signal = true)
	{
		if (this._broken)
		{
			return;
		}
		if (Time.time < this.endTime)
		{
			return;
		}
		if (this._breakEffect)
		{
			if (this._breakEffect.isPlaying)
			{
				this._breakEffect.Stop();
			}
			this._breakEffect.Play();
		}
		if (signal && PhotonNetwork.InRoom)
		{
			VRRig vrrig = base.GetComponent<OwnerRig>();
			if (vrrig != null)
			{
				this._breakSignal.Raise(vrrig.OwningNetPlayer.ActorNumber);
			}
		}
		this.ShowRenderers(false);
		this._broken = true;
		if (callback)
		{
			UnityEvent<Breakable> unityEvent = this.onBreak;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(this);
		}
	}

	// Token: 0x06001D59 RID: 7513 RVA: 0x000E06D8 File Offset: 0x000DE8D8
	private void UpdatePhysMasks()
	{
		int physicsMask = (int)this._physicsMask;
		if (this._collider)
		{
			this._collider.includeLayers = physicsMask;
			this._collider.excludeLayers = ~physicsMask;
		}
		if (this._rigidbody)
		{
			this._rigidbody.includeLayers = physicsMask;
			this._rigidbody.excludeLayers = ~physicsMask;
		}
	}

	// Token: 0x04002033 RID: 8243
	[SerializeField]
	private Collider _collider;

	// Token: 0x04002034 RID: 8244
	[SerializeField]
	private Rigidbody _rigidbody;

	// Token: 0x04002035 RID: 8245
	[SerializeField]
	private GameObject rendererRoot;

	// Token: 0x04002036 RID: 8246
	[SerializeField]
	private Renderer[] _renderers = new Renderer[0];

	// Token: 0x04002037 RID: 8247
	[Space]
	[SerializeField]
	private ParticleSystem _breakEffect;

	// Token: 0x04002038 RID: 8248
	[SerializeField]
	private UnityLayerMask _physicsMask = UnityLayerMask.GorillaHand;

	// Token: 0x04002039 RID: 8249
	public UnityEvent<Breakable> onSpawn;

	// Token: 0x0400203A RID: 8250
	public UnityEvent<Breakable> onBreak;

	// Token: 0x0400203B RID: 8251
	public UnityEvent<Breakable> onReset;

	// Token: 0x0400203C RID: 8252
	public float canBreakDelay = 1f;

	// Token: 0x0400203D RID: 8253
	[SerializeField]
	private PhotonSignal<int> _breakSignal = "_breakSignal";

	// Token: 0x0400203E RID: 8254
	[Space]
	[NonSerialized]
	private bool _broken;

	// Token: 0x0400203F RID: 8255
	private float startTime;

	// Token: 0x04002040 RID: 8256
	private float endTime;
}
