using System;
using UnityEngine;

// Token: 0x0200044E RID: 1102
public class SyncToPlayerColor : MonoBehaviour
{
	// Token: 0x06001B2A RID: 6954 RVA: 0x00086275 File Offset: 0x00084475
	protected virtual void Awake()
	{
		this.rig = base.GetComponentInParent<VRRig>();
		this._colorFunc = new Action<Color>(this.UpdateColor);
	}

	// Token: 0x06001B2B RID: 6955 RVA: 0x00086296 File Offset: 0x00084496
	protected virtual void Start()
	{
		this.UpdateColor(this.rig.playerColor);
		this.rig.OnColorInitialized(this._colorFunc);
	}

	// Token: 0x06001B2C RID: 6956 RVA: 0x000862BA File Offset: 0x000844BA
	protected virtual void OnEnable()
	{
		this.rig.OnColorChanged += this._colorFunc;
	}

	// Token: 0x06001B2D RID: 6957 RVA: 0x000862CD File Offset: 0x000844CD
	protected virtual void OnDisable()
	{
		this.rig.OnColorChanged -= this._colorFunc;
	}

	// Token: 0x06001B2E RID: 6958 RVA: 0x000862E0 File Offset: 0x000844E0
	public virtual void UpdateColor(Color color)
	{
		if (!this.target)
		{
			return;
		}
		if (this.colorPropertiesToSync == null)
		{
			return;
		}
		for (int i = 0; i < this.colorPropertiesToSync.Length; i++)
		{
			ShaderHashId h = this.colorPropertiesToSync[i];
			this.target.SetColor(h, color);
		}
	}

	// Token: 0x04001E1C RID: 7708
	public VRRig rig;

	// Token: 0x04001E1D RID: 7709
	public Material target;

	// Token: 0x04001E1E RID: 7710
	public ShaderHashId[] colorPropertiesToSync = new ShaderHashId[]
	{
		"_BaseColor"
	};

	// Token: 0x04001E1F RID: 7711
	private Action<Color> _colorFunc;
}
