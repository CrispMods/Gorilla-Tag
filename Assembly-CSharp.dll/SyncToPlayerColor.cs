using System;
using UnityEngine;

// Token: 0x0200044E RID: 1102
public class SyncToPlayerColor : MonoBehaviour
{
	// Token: 0x06001B2A RID: 6954 RVA: 0x00041916 File Offset: 0x0003FB16
	protected virtual void Awake()
	{
		this.rig = base.GetComponentInParent<VRRig>();
		this._colorFunc = new Action<Color>(this.UpdateColor);
	}

	// Token: 0x06001B2B RID: 6955 RVA: 0x00041937 File Offset: 0x0003FB37
	protected virtual void Start()
	{
		this.UpdateColor(this.rig.playerColor);
		this.rig.OnColorInitialized(this._colorFunc);
	}

	// Token: 0x06001B2C RID: 6956 RVA: 0x0004195B File Offset: 0x0003FB5B
	protected virtual void OnEnable()
	{
		this.rig.OnColorChanged += this._colorFunc;
	}

	// Token: 0x06001B2D RID: 6957 RVA: 0x0004196E File Offset: 0x0003FB6E
	protected virtual void OnDisable()
	{
		this.rig.OnColorChanged -= this._colorFunc;
	}

	// Token: 0x06001B2E RID: 6958 RVA: 0x000D7ACC File Offset: 0x000D5CCC
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
