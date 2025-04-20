using System;
using TMPro;
using UnityEngine;

// Token: 0x020004B2 RID: 1202
public class MonkeBallShotclock : MonoBehaviour
{
	// Token: 0x06001D3F RID: 7487 RVA: 0x000E0378 File Offset: 0x000DE578
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

	// Token: 0x06001D40 RID: 7488 RVA: 0x000E03CC File Offset: 0x000DE5CC
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

	// Token: 0x06001D41 RID: 7489 RVA: 0x00044068 File Offset: 0x00042268
	private void SetBackboard(Material teamMaterial)
	{
		if (this.backboard != null)
		{
			this.backboard.material = teamMaterial;
		}
	}

	// Token: 0x06001D42 RID: 7490 RVA: 0x000E0424 File Offset: 0x000DE624
	private void UpdateTimeText(float time)
	{
		int num = Mathf.CeilToInt(time);
		if (this._timeInt != num)
		{
			this._timeInt = num;
			this.timeRemainingLabel.text = this._timeInt.ToString("#00");
		}
	}

	// Token: 0x0400202A RID: 8234
	public Renderer backboard;

	// Token: 0x0400202B RID: 8235
	public Material[] teamMaterials;

	// Token: 0x0400202C RID: 8236
	public Material neutralMaterial;

	// Token: 0x0400202D RID: 8237
	public TextMeshPro timeRemainingLabel;

	// Token: 0x0400202E RID: 8238
	private float _time;

	// Token: 0x0400202F RID: 8239
	private int _timeInt = -1;
}
