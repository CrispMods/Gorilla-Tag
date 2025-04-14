using System;
using GorillaTagScripts;
using UnityEngine;

// Token: 0x02000551 RID: 1361
public class GorillaCaveCrystalVisuals : MonoBehaviour
{
	// Token: 0x1700036D RID: 877
	// (get) Token: 0x0600214F RID: 8527 RVA: 0x000A5FCE File Offset: 0x000A41CE
	// (set) Token: 0x06002150 RID: 8528 RVA: 0x000A5FD6 File Offset: 0x000A41D6
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

	// Token: 0x06002151 RID: 8529 RVA: 0x000A5FE0 File Offset: 0x000A41E0
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

	// Token: 0x06002152 RID: 8530 RVA: 0x000A605C File Offset: 0x000A425C
	private void Start()
	{
		this.UpdateAlbedo();
		this.ForceUpdate();
	}

	// Token: 0x06002153 RID: 8531 RVA: 0x000A606C File Offset: 0x000A426C
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

	// Token: 0x06002154 RID: 8532 RVA: 0x000A60E1 File Offset: 0x000A42E1
	private void Awake()
	{
		this.UpdateAlbedo();
		this.Update();
	}

	// Token: 0x06002155 RID: 8533 RVA: 0x000A60F0 File Offset: 0x000A42F0
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

	// Token: 0x06002156 RID: 8534 RVA: 0x000A61E6 File Offset: 0x000A43E6
	public void ForceUpdate()
	{
		this._lastState = 0;
		this.Update();
	}

	// Token: 0x06002157 RID: 8535 RVA: 0x000A61F8 File Offset: 0x000A43F8
	private static void InitializeCrystals()
	{
		foreach (GorillaCaveCrystalVisuals gorillaCaveCrystalVisuals in Object.FindObjectsByType<GorillaCaveCrystalVisuals>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID))
		{
			gorillaCaveCrystalVisuals.UpdateAlbedo();
			gorillaCaveCrystalVisuals.ForceUpdate();
			gorillaCaveCrystalVisuals._lastState = -1;
		}
	}

	// Token: 0x040024F9 RID: 9465
	public CrystalVisualsPreset crysalPreset;

	// Token: 0x040024FA RID: 9466
	[SerializeField]
	[Range(0f, 1f)]
	private float _lerp;

	// Token: 0x040024FB RID: 9467
	[Space]
	public MeshRenderer _renderer;

	// Token: 0x040024FC RID: 9468
	public Material _sharedMaterial;

	// Token: 0x040024FD RID: 9469
	[SerializeField]
	public Texture2D instanceAlbedo;

	// Token: 0x040024FE RID: 9470
	[SerializeField]
	private bool _initialized;

	// Token: 0x040024FF RID: 9471
	[SerializeField]
	private int _lastState;

	// Token: 0x04002500 RID: 9472
	[SerializeField]
	public GorillaCaveCrystalSetup _setup;

	// Token: 0x04002501 RID: 9473
	private MaterialPropertyBlock _block;

	// Token: 0x04002502 RID: 9474
	[NonSerialized]
	private bool _ranSetupOnce;

	// Token: 0x04002503 RID: 9475
	private static readonly ShaderHashId _Color = "_Color";

	// Token: 0x04002504 RID: 9476
	private static readonly ShaderHashId _EmissionColor = "_EmissionColor";

	// Token: 0x04002505 RID: 9477
	private static readonly ShaderHashId _MainTex = "_MainTex";
}
