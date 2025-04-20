using System;
using GorillaTagScripts;
using UnityEngine;

// Token: 0x0200055E RID: 1374
public class GorillaCaveCrystalVisuals : MonoBehaviour
{
	// Token: 0x17000374 RID: 884
	// (get) Token: 0x060021A5 RID: 8613 RVA: 0x00046EDF File Offset: 0x000450DF
	// (set) Token: 0x060021A6 RID: 8614 RVA: 0x00046EE7 File Offset: 0x000450E7
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

	// Token: 0x060021A7 RID: 8615 RVA: 0x000F62E8 File Offset: 0x000F44E8
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

	// Token: 0x060021A8 RID: 8616 RVA: 0x00046EF0 File Offset: 0x000450F0
	private void Start()
	{
		this.UpdateAlbedo();
		this.ForceUpdate();
	}

	// Token: 0x060021A9 RID: 8617 RVA: 0x000F6364 File Offset: 0x000F4564
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

	// Token: 0x060021AA RID: 8618 RVA: 0x00046EFE File Offset: 0x000450FE
	private void Awake()
	{
		this.UpdateAlbedo();
		this.Update();
	}

	// Token: 0x060021AB RID: 8619 RVA: 0x000F63DC File Offset: 0x000F45DC
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

	// Token: 0x060021AC RID: 8620 RVA: 0x00046F0C File Offset: 0x0004510C
	public void ForceUpdate()
	{
		this._lastState = 0;
		this.Update();
	}

	// Token: 0x060021AD RID: 8621 RVA: 0x000F64D4 File Offset: 0x000F46D4
	private static void InitializeCrystals()
	{
		foreach (GorillaCaveCrystalVisuals gorillaCaveCrystalVisuals in UnityEngine.Object.FindObjectsByType<GorillaCaveCrystalVisuals>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID))
		{
			gorillaCaveCrystalVisuals.UpdateAlbedo();
			gorillaCaveCrystalVisuals.ForceUpdate();
			gorillaCaveCrystalVisuals._lastState = -1;
		}
	}

	// Token: 0x0400254B RID: 9547
	public CrystalVisualsPreset crysalPreset;

	// Token: 0x0400254C RID: 9548
	[SerializeField]
	[Range(0f, 1f)]
	private float _lerp;

	// Token: 0x0400254D RID: 9549
	[Space]
	public MeshRenderer _renderer;

	// Token: 0x0400254E RID: 9550
	public Material _sharedMaterial;

	// Token: 0x0400254F RID: 9551
	[SerializeField]
	public Texture2D instanceAlbedo;

	// Token: 0x04002550 RID: 9552
	[SerializeField]
	private bool _initialized;

	// Token: 0x04002551 RID: 9553
	[SerializeField]
	private int _lastState;

	// Token: 0x04002552 RID: 9554
	[SerializeField]
	public GorillaCaveCrystalSetup _setup;

	// Token: 0x04002553 RID: 9555
	private MaterialPropertyBlock _block;

	// Token: 0x04002554 RID: 9556
	[NonSerialized]
	private bool _ranSetupOnce;

	// Token: 0x04002555 RID: 9557
	private static readonly ShaderHashId _Color = "_Color";

	// Token: 0x04002556 RID: 9558
	private static readonly ShaderHashId _EmissionColor = "_EmissionColor";

	// Token: 0x04002557 RID: 9559
	private static readonly ShaderHashId _MainTex = "_MainTex";
}
