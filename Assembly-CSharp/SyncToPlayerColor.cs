using System;
using UnityEngine;

// Token: 0x0200044E RID: 1102
public class SyncToPlayerColor : MonoBehaviour
{
	// Token: 0x06001B27 RID: 6951 RVA: 0x00085EF1 File Offset: 0x000840F1
	protected virtual void Awake()
	{
		this.rig = base.GetComponentInParent<VRRig>();
		this._colorFunc = new Action<Color>(this.UpdateColor);
	}

	// Token: 0x06001B28 RID: 6952 RVA: 0x00085F12 File Offset: 0x00084112
	protected virtual void Start()
	{
		this.UpdateColor(this.rig.playerColor);
		this.rig.OnColorInitialized(this._colorFunc);
	}

	// Token: 0x06001B29 RID: 6953 RVA: 0x00085F36 File Offset: 0x00084136
	protected virtual void OnEnable()
	{
		this.rig.OnColorChanged += this._colorFunc;
	}

	// Token: 0x06001B2A RID: 6954 RVA: 0x00085F49 File Offset: 0x00084149
	protected virtual void OnDisable()
	{
		this.rig.OnColorChanged -= this._colorFunc;
	}

	// Token: 0x06001B2B RID: 6955 RVA: 0x00085F5C File Offset: 0x0008415C
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

	// Token: 0x04001E1B RID: 7707
	public VRRig rig;

	// Token: 0x04001E1C RID: 7708
	public Material target;

	// Token: 0x04001E1D RID: 7709
	public ShaderHashId[] colorPropertiesToSync = new ShaderHashId[]
	{
		"_BaseColor"
	};

	// Token: 0x04001E1E RID: 7710
	private Action<Color> _colorFunc;
}
