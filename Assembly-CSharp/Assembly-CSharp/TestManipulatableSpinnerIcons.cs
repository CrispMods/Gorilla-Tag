using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003EC RID: 1004
public class TestManipulatableSpinnerIcons : MonoBehaviour
{
	// Token: 0x0600187F RID: 6271 RVA: 0x000772EA File Offset: 0x000754EA
	private void Awake()
	{
		this.GenerateRollers();
	}

	// Token: 0x06001880 RID: 6272 RVA: 0x000772F2 File Offset: 0x000754F2
	private void LateUpdate()
	{
		this.currentRotation = this.spinner.angle * this.rotationScale;
		this.UpdateSelectedIndex();
		this.UpdateRollers();
	}

	// Token: 0x06001881 RID: 6273 RVA: 0x00077318 File Offset: 0x00075518
	private void GenerateRollers()
	{
		for (int i = 0; i < this.rollerElementCount; i++)
		{
			float x = this.rollerElementAngle * (float)i + this.rollerElementAngle * 0.5f;
			Object.Instantiate<GameObject>(this.rollerElementTemplate, base.transform).transform.localRotation = Quaternion.Euler(x, 0f, 0f);
			GameObject gameObject = Object.Instantiate<GameObject>(this.iconElementTemplate, this.iconCanvas.transform);
			gameObject.transform.localRotation = Quaternion.Euler(x, 0f, 0f);
			this.visibleIcons.Add(gameObject.GetComponentInChildren<Text>());
		}
		this.rollerElementTemplate.SetActive(false);
		this.iconElementTemplate.SetActive(false);
		this.UpdateRollers();
	}

	// Token: 0x06001882 RID: 6274 RVA: 0x000773E0 File Offset: 0x000755E0
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

	// Token: 0x06001883 RID: 6275 RVA: 0x0007744C File Offset: 0x0007564C
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

	// Token: 0x04001B19 RID: 6937
	public ManipulatableSpinner spinner;

	// Token: 0x04001B1A RID: 6938
	public float rotationScale = 1f;

	// Token: 0x04001B1B RID: 6939
	public int rollerElementCount = 5;

	// Token: 0x04001B1C RID: 6940
	public GameObject rollerElementTemplate;

	// Token: 0x04001B1D RID: 6941
	public GameObject iconCanvas;

	// Token: 0x04001B1E RID: 6942
	public GameObject iconElementTemplate;

	// Token: 0x04001B1F RID: 6943
	public float iconOffset = 1f;

	// Token: 0x04001B20 RID: 6944
	public float rollerElementAngle = 15f;

	// Token: 0x04001B21 RID: 6945
	private List<Text> visibleIcons = new List<Text>();

	// Token: 0x04001B22 RID: 6946
	private float currentRotation;

	// Token: 0x04001B23 RID: 6947
	public int scrollableCount = 50;

	// Token: 0x04001B24 RID: 6948
	public int selectedIndex;
}
