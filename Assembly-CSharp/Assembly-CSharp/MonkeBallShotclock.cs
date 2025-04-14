using System;
using TMPro;
using UnityEngine;

// Token: 0x020004A6 RID: 1190
public class MonkeBallShotclock : MonoBehaviour
{
	// Token: 0x06001CEE RID: 7406 RVA: 0x0008D2C8 File Offset: 0x0008B4C8
	private void Update()
	{
		if (this._time >= 0f)
		{
			this._time -= Time.deltaTime;
			this.UpdateTimeText(this._time);
			if (this._time < 0f)
			{
				this.SetBackboard(this.neutralMaterial);
			}
		}
	}

	// Token: 0x06001CEF RID: 7407 RVA: 0x0008D31C File Offset: 0x0008B51C
	public void SetTime(int teamId, float time)
	{
		this._time = time;
		if (teamId == -1)
		{
			this._time = 0f;
			this.SetBackboard(this.neutralMaterial);
		}
		else if (teamId >= 0 && teamId < this.teamMaterials.Length)
		{
			this.SetBackboard(this.teamMaterials[teamId]);
		}
		this.UpdateTimeText(time);
	}

	// Token: 0x06001CF0 RID: 7408 RVA: 0x0008D371 File Offset: 0x0008B571
	private void SetBackboard(Material teamMaterial)
	{
		if (this.backboard != null)
		{
			this.backboard.material = teamMaterial;
		}
	}

	// Token: 0x06001CF1 RID: 7409 RVA: 0x0008D390 File Offset: 0x0008B590
	private void UpdateTimeText(float time)
	{
		int num = Mathf.CeilToInt(time);
		if (this._timeInt != num)
		{
			this._timeInt = num;
			this.timeRemainingLabel.text = this._timeInt.ToString("#00");
		}
	}

	// Token: 0x04001FDC RID: 8156
	public Renderer backboard;

	// Token: 0x04001FDD RID: 8157
	public Material[] teamMaterials;

	// Token: 0x04001FDE RID: 8158
	public Material neutralMaterial;

	// Token: 0x04001FDF RID: 8159
	public TextMeshPro timeRemainingLabel;

	// Token: 0x04001FE0 RID: 8160
	private float _time;

	// Token: 0x04001FE1 RID: 8161
	private int _timeInt = -1;
}
