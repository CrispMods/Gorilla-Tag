using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003F7 RID: 1015
public class TestManipulatableSpinnerIcons : MonoBehaviour
{
	// Token: 0x060018C9 RID: 6345 RVA: 0x00040C0D File Offset: 0x0003EE0D
	private void Awake()
	{
		this.GenerateRollers();
	}

	// Token: 0x060018CA RID: 6346 RVA: 0x00040C15 File Offset: 0x0003EE15
	private void LateUpdate()
	{
		this.currentRotation = this.spinner.angle * this.rotationScale;
		this.UpdateSelectedIndex();
		this.UpdateRollers();
	}

	// Token: 0x060018CB RID: 6347 RVA: 0x000CD3EC File Offset: 0x000CB5EC
	private void GenerateRollers()
	{
		for (int i = 0; i < this.rollerElementCount; i++)
		{
			float x = this.rollerElementAngle * (float)i + this.rollerElementAngle * 0.5f;
			UnityEngine.Object.Instantiate<GameObject>(this.rollerElementTemplate, base.transform).transform.localRotation = Quaternion.Euler(x, 0f, 0f);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.iconElementTemplate, this.iconCanvas.transform);
			gameObject.transform.localRotation = Quaternion.Euler(x, 0f, 0f);
			this.visibleIcons.Add(gameObject.GetComponentInChildren<Text>());
		}
		this.rollerElementTemplate.SetActive(false);
		this.iconElementTemplate.SetActive(false);
		this.UpdateRollers();
	}

	// Token: 0x060018CC RID: 6348 RVA: 0x000CD4B4 File Offset: 0x000CB6B4
	private void UpdateSelectedIndex()
	{
		float num = this.currentRotation / this.rollerElementAngle;
		if (this.rollerElementCount % 2 == 1)
		{
			num += 0.5f;
		}
		this.selectedIndex = Mathf.FloorToInt(num);
		this.selectedIndex %= this.scrollableCount;
		if (this.selectedIndex < 0)
		{
			this.selectedIndex = this.scrollableCount + this.selectedIndex;
		}
	}

	// Token: 0x060018CD RID: 6349 RVA: 0x000CD520 File Offset: 0x000CB720
	private void UpdateRollers()
	{
		float num = this.currentRotation;
		if (Mathf.Abs(num) > this.rollerElementAngle / 2f)
		{
			if (num > 0f)
			{
				num += this.rollerElementAngle / 2f;
				num %= this.rollerElementAngle;
				num -= this.rollerElementAngle / 2f;
			}
			else
			{
				num -= this.rollerElementAngle / 2f;
				num %= this.rollerElementAngle;
				num += this.rollerElementAngle / 2f;
			}
		}
		num -= (float)this.rollerElementCount / 2f * this.rollerElementAngle;
		base.transform.localRotation = Quaternion.Euler(num, 0f, 0f);
		this.iconCanvas.transform.localRotation = Quaternion.Euler(num, 0f, 0f);
		int num2 = this.rollerElementCount / 2;
		for (int i = 0; i < this.visibleIcons.Count; i++)
		{
			int num3 = this.selectedIndex - i + num2;
			if (num3 < 0)
			{
				num3 += this.scrollableCount;
			}
			else
			{
				num3 %= this.scrollableCount;
			}
			this.visibleIcons[i].text = string.Format("{0}", num3 + 1);
		}
	}

	// Token: 0x04001B61 RID: 7009
	public ManipulatableSpinner spinner;

	// Token: 0x04001B62 RID: 7010
	public float rotationScale = 1f;

	// Token: 0x04001B63 RID: 7011
	public int rollerElementCount = 5;

	// Token: 0x04001B64 RID: 7012
	public GameObject rollerElementTemplate;

	// Token: 0x04001B65 RID: 7013
	public GameObject iconCanvas;

	// Token: 0x04001B66 RID: 7014
	public GameObject iconElementTemplate;

	// Token: 0x04001B67 RID: 7015
	public float iconOffset = 1f;

	// Token: 0x04001B68 RID: 7016
	public float rollerElementAngle = 15f;

	// Token: 0x04001B69 RID: 7017
	private List<Text> visibleIcons = new List<Text>();

	// Token: 0x04001B6A RID: 7018
	private float currentRotation;

	// Token: 0x04001B6B RID: 7019
	public int scrollableCount = 50;

	// Token: 0x04001B6C RID: 7020
	public int selectedIndex;
}
