using System;
using TMPro;
using UnityEngine;

// Token: 0x020004A6 RID: 1190
public class MonkeBallShotclock : MonoBehaviour
{
	// Token: 0x06001CEB RID: 7403 RVA: 0x0008CF44 File Offset: 0x0008B144
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

	// Token: 0x06001CEC RID: 7404 RVA: 0x0008CF98 File Offset: 0x0008B198
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

	// Token: 0x06001CED RID: 7405 RVA: 0x0008CFED File Offset: 0x0008B1ED
	private void SetBackboard(Material teamMaterial)
	{
		if (this.backboard != null)
		{
			this.backboard.material = teamMaterial;
		}
	}

	// Token: 0x06001CEE RID: 7406 RVA: 0x0008D00C File Offset: 0x0008B20C
	private void UpdateTimeText(float time)
	{
		int num = Mathf.CeilToInt(time);
		if (this._timeInt != num)
		{
			this._timeInt = num;
			this.timeRemainingLabel.text = this._timeInt.ToString("#00");
		}
	}

	// Token: 0x04001FDB RID: 8155
	public Renderer backboard;

	// Token: 0x04001FDC RID: 8156
	public Material[] teamMaterials;

	// Token: 0x04001FDD RID: 8157
	public Material neutralMaterial;

	// Token: 0x04001FDE RID: 8158
	public TextMeshPro timeRemainingLabel;

	// Token: 0x04001FDF RID: 8159
	private float _time;

	// Token: 0x04001FE0 RID: 8160
	private int _timeInt = -1;
}
