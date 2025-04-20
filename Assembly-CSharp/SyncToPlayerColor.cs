using System;
using UnityEngine;

// Token: 0x0200045A RID: 1114
public class SyncToPlayerColor : MonoBehaviour
{
	// Token: 0x06001B7B RID: 7035 RVA: 0x00042C4F File Offset: 0x00040E4F
	protected virtual void Awake()
	{
		this.rig = base.GetComponentInParent<VRRig>();
		this._colorFunc = new Action<Color>(this.UpdateColor);
	}

	// Token: 0x06001B7C RID: 7036 RVA: 0x00042C70 File Offset: 0x00040E70
	protected virtual void Start()
	{
		this.UpdateColor(this.rig.playerColor);
		this.rig.OnColorInitialized(this._colorFunc);
	}

	// Token: 0x06001B7D RID: 7037 RVA: 0x00042C94 File Offset: 0x00040E94
	protected virtual void OnEnable()
	{
		this.rig.OnColorChanged += this._colorFunc;
	}

	// Token: 0x06001B7E RID: 7038 RVA: 0x00042CA7 File Offset: 0x00040EA7
	protected virtual void OnDisable()
	{
		this.rig.OnColorChanged -= this._colorFunc;
	}

	// Token: 0x06001B7F RID: 7039 RVA: 0x000DA76C File Offset: 0x000D896C
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

	// Token: 0x04001E6A RID: 7786
	public VRRig rig;

	// Token: 0x04001E6B RID: 7787
	public Material target;

	// Token: 0x04001E6C RID: 7788
	public ShaderHashId[] colorPropertiesToSync = new ShaderHashId[]
	{
		"_BaseColor"
	};

	// Token: 0x04001E6D RID: 7789
	private Action<Color> _colorFunc;
}
