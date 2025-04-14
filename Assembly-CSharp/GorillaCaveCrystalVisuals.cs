using System;
using GorillaTagScripts;
using UnityEngine;

// Token: 0x02000550 RID: 1360
public class GorillaCaveCrystalVisuals : MonoBehaviour
{
	// Token: 0x1700036C RID: 876
	// (get) Token: 0x06002147 RID: 8519 RVA: 0x000A5B4E File Offset: 0x000A3D4E
	// (set) Token: 0x06002148 RID: 8520 RVA: 0x000A5B56 File Offset: 0x000A3D56
	public float lerp
	{
		get
		{
			return this._lerp;
		}
		set
		{
			this._lerp = value;
		}
	}

	// Token: 0x06002149 RID: 8521 RVA: 0x000A5B60 File Offset: 0x000A3D60
	public void Setup()
	{
		base.TryGetComponent<MeshRenderer>(out this._renderer);
		if (this._renderer == null)
		{
			return;
		}
		this._setup = GorillaCaveCrystalSetup.Instance;
		this._sharedMaterial = this._renderer.sharedMaterial;
		this._initialized = (this.crysalPreset != null && this._renderer != null && this._sharedMaterial != null);
		this.Update();
	}

	// Token: 0x0600214A RID: 8522 RVA: 0x000A5BDC File Offset: 0x000A3DDC
	private void Start()
	{
		this.UpdateAlbedo();
		this.ForceUpdate();
	}

	// Token: 0x0600214B RID: 8523 RVA: 0x000A5BEC File Offset: 0x000A3DEC
	public void UpdateAlbedo()
	{
		if (!this._initialized)
		{
			return;
		}
		if (this.instanceAlbedo == null)
		{
			return;
		}
		if (this._block == null)
		{
			this._block = new MaterialPropertyBlock();
		}
		this._renderer.GetPropertyBlock(this._block);
		this._block.SetTexture(GorillaCaveCrystalVisuals._MainTex, this.instanceAlbedo);
		this._renderer.SetPropertyBlock(this._block);
	}

	// Token: 0x0600214C RID: 8524 RVA: 0x000A5C61 File Offset: 0x000A3E61
	private void Awake()
	{
		this.UpdateAlbedo();
		this.Update();
	}

	// Token: 0x0600214D RID: 8525 RVA: 0x000A5C70 File Offset: 0x000A3E70
	private void Update()
	{
		if (!this._initialized)
		{
			return;
		}
		if (Application.isPlaying)
		{
			int hashCode = new ValueTuple<CrystalVisualsPreset, float>(this.crysalPreset, this._lerp).GetHashCode();
			if (this._lastState == hashCode)
			{
				return;
			}
			this._lastState = hashCode;
		}
		if (this._block == null)
		{
			this._block = new MaterialPropertyBlock();
		}
		CrystalVisualsPreset.VisualState stateA = this.crysalPreset.stateA;
		CrystalVisualsPreset.VisualState stateB = this.crysalPreset.stateB;
		Color value = Color.Lerp(stateA.albedo, stateB.albedo, this._lerp);
		Color value2 = Color.Lerp(stateA.emission, stateB.emission, this._lerp);
		this._renderer.GetPropertyBlock(this._block);
		this._block.SetColor(GorillaCaveCrystalVisuals._Color, value);
		this._block.SetColor(GorillaCaveCrystalVisuals._EmissionColor, value2);
		this._renderer.SetPropertyBlock(this._block);
	}

	// Token: 0x0600214E RID: 8526 RVA: 0x000A5D66 File Offset: 0x000A3F66
	public void ForceUpdate()
	{
		this._lastState = 0;
		this.Update();
	}

	// Token: 0x0600214F RID: 8527 RVA: 0x000A5D78 File Offset: 0x000A3F78
	private static void InitializeCrystals()
	{
		foreach (GorillaCaveCrystalVisuals gorillaCaveCrystalVisuals in Object.FindObjectsByType<GorillaCaveCrystalVisuals>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID))
		{
			gorillaCaveCrystalVisuals.UpdateAlbedo();
			gorillaCaveCrystalVisuals.ForceUpdate();
			gorillaCaveCrystalVisuals._lastState = -1;
		}
	}

	// Token: 0x040024F3 RID: 9459
	public CrystalVisualsPreset crysalPreset;

	// Token: 0x040024F4 RID: 9460
	[SerializeField]
	[Range(0f, 1f)]
	private float _lerp;

	// Token: 0x040024F5 RID: 9461
	[Space]
	public MeshRenderer _renderer;

	// Token: 0x040024F6 RID: 9462
	public Material _sharedMaterial;

	// Token: 0x040024F7 RID: 9463
	[SerializeField]
	public Texture2D instanceAlbedo;

	// Token: 0x040024F8 RID: 9464
	[SerializeField]
	private bool _initialized;

	// Token: 0x040024F9 RID: 9465
	[SerializeField]
	private int _lastState;

	// Token: 0x040024FA RID: 9466
	[SerializeField]
	public GorillaCaveCrystalSetup _setup;

	// Token: 0x040024FB RID: 9467
	private MaterialPropertyBlock _block;

	// Token: 0x040024FC RID: 9468
	[NonSerialized]
	private bool _ranSetupOnce;

	// Token: 0x040024FD RID: 9469
	private static readonly ShaderHashId _Color = "_Color";

	// Token: 0x040024FE RID: 9470
	private static readonly ShaderHashId _EmissionColor = "_EmissionColor";

	// Token: 0x040024FF RID: 9471
	private static readonly ShaderHashId _MainTex = "_MainTex";
}
