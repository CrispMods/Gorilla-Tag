using System;
using GorillaTagScripts;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020004F0 RID: 1264
public class BuilderResourceMeter : MonoBehaviour
{
	// Token: 0x06001EC4 RID: 7876 RVA: 0x000EB79C File Offset: 0x000E999C
	private void Awake()
	{
		this.fillColor = this.resourceColors.GetColorForType(this._resourceType);
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		this.fillCube.GetPropertyBlock(materialPropertyBlock);
		materialPropertyBlock.SetColor("_BaseColor", this.fillColor);
		this.fillCube.SetPropertyBlock(materialPropertyBlock);
		materialPropertyBlock.SetColor("_BaseColor", this.emptyColor);
		this.emptyCube.SetPropertyBlock(materialPropertyBlock);
		this.fillAmount = this.fillTarget;
	}

	// Token: 0x06001EC5 RID: 7877 RVA: 0x00044DA2 File Offset: 0x00042FA2
	private void Start()
	{
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
		this.OnZoneChanged();
	}

	// Token: 0x06001EC6 RID: 7878 RVA: 0x00044DD0 File Offset: 0x00042FD0
	private void OnDestroy()
	{
		if (ZoneManagement.instance != null)
		{
			ZoneManagement instance = ZoneManagement.instance;
			instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
		}
	}

	// Token: 0x06001EC7 RID: 7879 RVA: 0x000EB818 File Offset: 0x000E9A18
	private void OnZoneChanged()
	{
		bool flag = ZoneManagement.instance.IsZoneActive(GTZone.monkeBlocks);
		if (flag != this.inBuilderZone)
		{
			this.inBuilderZone = flag;
			if (!flag)
			{
				this.fillCube.enabled = false;
				this.emptyCube.enabled = false;
				return;
			}
			this.fillCube.enabled = true;
			this.emptyCube.enabled = true;
			this.OnAvailableResourcesChange();
		}
	}

	// Token: 0x06001EC8 RID: 7880 RVA: 0x000EB87C File Offset: 0x000E9A7C
	public void OnAvailableResourcesChange()
	{
		this.resourceMax = BuilderTable.instance.maxResources[(int)this._resourceType];
		int num = BuilderTable.instance.usedResources[(int)this._resourceType];
		if (num != this.usedResource)
		{
			this.usedResource = num;
			this.SetNormalizedFillTarget((float)(this.resourceMax - this.usedResource) / (float)this.resourceMax);
		}
	}

	// Token: 0x06001EC9 RID: 7881 RVA: 0x000EB8E0 File Offset: 0x000E9AE0
	public void UpdateMeterFill()
	{
		if (this.animatingMeter)
		{
			float newFill = Mathf.MoveTowards(this.fillAmount, this.fillTarget, this.lerpSpeed * Time.deltaTime);
			this.UpdateFill(newFill);
		}
	}

	// Token: 0x06001ECA RID: 7882 RVA: 0x000EB91C File Offset: 0x000E9B1C
	private void UpdateFill(float newFill)
	{
		this.fillAmount = newFill;
		if (Mathf.Approximately(this.fillAmount, this.fillTarget))
		{
			this.fillAmount = this.fillTarget;
			this.animatingMeter = false;
		}
		if (!this.inBuilderZone)
		{
			return;
		}
		if (this.fillAmount <= 1E-45f)
		{
			this.fillCube.enabled = false;
			float y = this.meterHeight / this.meshHeight;
			Vector3 localScale = new Vector3(this.emptyCube.transform.localScale.x, y, this.emptyCube.transform.localScale.z);
			Vector3 localPosition = new Vector3(0f, this.meterHeight / 2f, 0f);
			this.emptyCube.transform.localScale = localScale;
			this.emptyCube.transform.localPosition = localPosition;
			this.emptyCube.enabled = true;
			return;
		}
		if (this.fillAmount >= 1f)
		{
			float y2 = this.meterHeight / this.meshHeight;
			Vector3 localScale2 = new Vector3(this.fillCube.transform.localScale.x, y2, this.fillCube.transform.localScale.z);
			Vector3 localPosition2 = new Vector3(0f, this.meterHeight / 2f, 0f);
			this.fillCube.transform.localScale = localScale2;
			this.fillCube.transform.localPosition = localPosition2;
			this.fillCube.enabled = true;
			this.emptyCube.enabled = false;
			return;
		}
		float num = this.meterHeight / this.meshHeight * this.fillAmount;
		Vector3 localScale3 = new Vector3(this.fillCube.transform.localScale.x, num, this.fillCube.transform.localScale.z);
		Vector3 localPosition3 = new Vector3(0f, num * this.meshHeight / 2f, 0f);
		this.fillCube.transform.localScale = localScale3;
		this.fillCube.transform.localPosition = localPosition3;
		this.fillCube.enabled = true;
		float num2 = this.meterHeight / this.meshHeight * (1f - this.fillAmount);
		Vector3 localScale4 = new Vector3(this.emptyCube.transform.localScale.x, num2, this.emptyCube.transform.localScale.z);
		Vector3 localPosition4 = new Vector3(0f, this.meterHeight - num2 * this.meshHeight / 2f, 0f);
		this.emptyCube.transform.localScale = localScale4;
		this.emptyCube.transform.localPosition = localPosition4;
		this.emptyCube.enabled = true;
	}

	// Token: 0x06001ECB RID: 7883 RVA: 0x00044E05 File Offset: 0x00043005
	public void SetNormalizedFillTarget(float fill)
	{
		this.fillTarget = Mathf.Clamp(fill, 0f, 1f);
		this.animatingMeter = true;
	}

	// Token: 0x04002251 RID: 8785
	public BuilderResourceColors resourceColors;

	// Token: 0x04002252 RID: 8786
	public MeshRenderer fillCube;

	// Token: 0x04002253 RID: 8787
	public MeshRenderer emptyCube;

	// Token: 0x04002254 RID: 8788
	private Color fillColor = Color.white;

	// Token: 0x04002255 RID: 8789
	public Color emptyColor = Color.black;

	// Token: 0x04002256 RID: 8790
	[FormerlySerializedAs("MeterHeight")]
	public float meterHeight = 2f;

	// Token: 0x04002257 RID: 8791
	public float meshHeight = 1f;

	// Token: 0x04002258 RID: 8792
	public BuilderResourceType _resourceType;

	// Token: 0x04002259 RID: 8793
	private float fillAmount;

	// Token: 0x0400225A RID: 8794
	[Range(0f, 1f)]
	[SerializeField]
	private float fillTarget;

	// Token: 0x0400225B RID: 8795
	public float lerpSpeed = 0.5f;

	// Token: 0x0400225C RID: 8796
	private bool animatingMeter;

	// Token: 0x0400225D RID: 8797
	private int resourceMax = -1;

	// Token: 0x0400225E RID: 8798
	private int usedResource = -1;

	// Token: 0x0400225F RID: 8799
	private bool inBuilderZone;
}
