using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004A9 RID: 1193
public class Breakable : MonoBehaviour
{
	// Token: 0x06001CF6 RID: 7414 RVA: 0x0008D0EC File Offset: 0x0008B2EC
	private void Awake()
	{
		this._breakSignal.OnSignal += this.BreakRPC;
	}

	// Token: 0x06001CF7 RID: 7415 RVA: 0x0008D108 File Offset: 0x0008B308
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

	// Token: 0x06001CF8 RID: 7416 RVA: 0x0008D144 File Offset: 0x0008B344
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

	// Token: 0x06001CF9 RID: 7417 RVA: 0x0008D1EB File Offset: 0x0008B3EB
	private void OnCollisionEnter(Collision col)
	{
		this.OnBreak(true, true);
	}

	// Token: 0x06001CFA RID: 7418 RVA: 0x0008D1EB File Offset: 0x0008B3EB
	private void OnCollisionStay(Collision col)
	{
		this.OnBreak(true, true);
	}

	// Token: 0x06001CFB RID: 7419 RVA: 0x0008D1EB File Offset: 0x0008B3EB
	private void OnTriggerEnter(Collider col)
	{
		this.OnBreak(true, true);
	}

	// Token: 0x06001CFC RID: 7420 RVA: 0x0008D1EB File Offset: 0x0008B3EB
	private void OnTriggerStay(Collider col)
	{
		this.OnBreak(true, true);
	}

	// Token: 0x06001CFD RID: 7421 RVA: 0x0008D1F5 File Offset: 0x0008B3F5
	private void OnEnable()
	{
		this._breakSignal.Enable();
		this._broken = false;
		this.OnSpawn(true);
	}

	// Token: 0x06001CFE RID: 7422 RVA: 0x0008D210 File Offset: 0x0008B410
	private void OnDisable()
	{
		this._breakSignal.Disable();
		this._broken = false;
		this.OnReset(false);
		this.ShowRenderers(false);
	}

	// Token: 0x06001CFF RID: 7423 RVA: 0x0008D1EB File Offset: 0x0008B3EB
	public void Break()
	{
		this.OnBreak(true, true);
	}

	// Token: 0x06001D00 RID: 7424 RVA: 0x0008D232 File Offset: 0x0008B432
	public void Reset()
	{
		this.OnReset(true);
	}

	// Token: 0x06001D01 RID: 7425 RVA: 0x0008D23C File Offset: 0x0008B43C
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

	// Token: 0x06001D02 RID: 7426 RVA: 0x0008D288 File Offset: 0x0008B488
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

	// Token: 0x06001D03 RID: 7427 RVA: 0x0008D2DC File Offset: 0x0008B4DC
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

	// Token: 0x06001D04 RID: 7428 RVA: 0x0008D318 File Offset: 0x0008B518
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

	// Token: 0x06001D05 RID: 7429 RVA: 0x0008D3C4 File Offset: 0x0008B5C4
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

	// Token: 0x04001FE4 RID: 8164
	[SerializeField]
	private Collider _collider;

	// Token: 0x04001FE5 RID: 8165
	[SerializeField]
	private Rigidbody _rigidbody;

	// Token: 0x04001FE6 RID: 8166
	[SerializeField]
	private GameObject rendererRoot;

	// Token: 0x04001FE7 RID: 8167
	[SerializeField]
	private Renderer[] _renderers = new Renderer[0];

	// Token: 0x04001FE8 RID: 8168
	[Space]
	[SerializeField]
	private ParticleSystem _breakEffect;

	// Token: 0x04001FE9 RID: 8169
	[SerializeField]
	private UnityLayerMask _physicsMask = UnityLayerMask.GorillaHand;

	// Token: 0x04001FEA RID: 8170
	public UnityEvent<Breakable> onSpawn;

	// Token: 0x04001FEB RID: 8171
	public UnityEvent<Breakable> onBreak;

	// Token: 0x04001FEC RID: 8172
	public UnityEvent<Breakable> onReset;

	// Token: 0x04001FED RID: 8173
	public float canBreakDelay = 1f;

	// Token: 0x04001FEE RID: 8174
	[SerializeField]
	private PhotonSignal<int> _breakSignal = "_breakSignal";

	// Token: 0x04001FEF RID: 8175
	[Space]
	[NonSerialized]
	private bool _broken;

	// Token: 0x04001FF0 RID: 8176
	private float startTime;

	// Token: 0x04001FF1 RID: 8177
	private float endTime;
}
