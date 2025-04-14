using System;
using GorillaTagScripts;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020004E3 RID: 1251
public class BuilderResourceMeter : MonoBehaviour
{
	// Token: 0x06001E6B RID: 7787 RVA: 0x00098FD4 File Offset: 0x000971D4
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

	// Token: 0x06001E6C RID: 7788 RVA: 0x00099050 File Offset: 0x00097250
	private void Start()
	{
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
		this.OnZoneChanged();
	}

	// Token: 0x06001E6D RID: 7789 RVA: 0x0009907E File Offset: 0x0009727E
	private void OnDestroy()
	{
		if (ZoneManagement.instance != null)
		{
			ZoneManagement instance = ZoneManagement.instance;
			instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
		}
	}

	// Token: 0x06001E6E RID: 7790 RVA: 0x000990B4 File Offset: 0x000972B4
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

	// Token: 0x06001E6F RID: 7791 RVA: 0x00099118 File Offset: 0x00097318
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

	// Token: 0x06001E70 RID: 7792 RVA: 0x0009917C File Offset: 0x0009737C
	public void UpdateMeterFill()
	{
		if (this.animatingMeter)
		{
			float newFill = Mathf.MoveTowards(this.fillAmount, this.fillTarget, this.lerpSpeed * Time.deltaTime);
			this.UpdateFill(newFill);
		}
	}

	// Token: 0x06001E71 RID: 7793 RVA: 0x000991B8 File Offset: 0x000973B8
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

	// Token: 0x06001E72 RID: 7794 RVA: 0x0009948C File Offset: 0x0009768C
	public void SetNormalizedFillTarget(float fill)
	{
		this.fillTarget = Mathf.Clamp(fill, 0f, 1f);
		this.animatingMeter = true;
	}

	// Token: 0x040021FE RID: 8702
	public BuilderResourceColors resourceColors;

	// Token: 0x040021FF RID: 8703
	public MeshRenderer fillCube;

	// Token: 0x04002200 RID: 8704
	public MeshRenderer emptyCube;

	// Token: 0x04002201 RID: 8705
	private Color fillColor = Color.white;

	// Token: 0x04002202 RID: 8706
	public Color emptyColor = Color.black;

	// Token: 0x04002203 RID: 8707
	[FormerlySerializedAs("MeterHeight")]
	public float meterHeight = 2f;

	// Token: 0x04002204 RID: 8708
	public float meshHeight = 1f;

	// Token: 0x04002205 RID: 8709
	public BuilderResourceType _resourceType;

	// Token: 0x04002206 RID: 8710
	private float fillAmount;

	// Token: 0x04002207 RID: 8711
	[Range(0f, 1f)]
	[SerializeField]
	private float fillTarget;

	// Token: 0x04002208 RID: 8712
	public float lerpSpeed = 0.5f;

	// Token: 0x04002209 RID: 8713
	private bool animatingMeter;

	// Token: 0x0400220A RID: 8714
	private int resourceMax = -1;

	// Token: 0x0400220B RID: 8715
	private int usedResource = -1;

	// Token: 0x0400220C RID: 8716
	private bool inBuilderZone;
}
