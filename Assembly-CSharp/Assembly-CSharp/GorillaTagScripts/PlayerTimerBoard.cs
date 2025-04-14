using System;
using System.Collections.Generic;
using System.Text;
using KID.Model;
using TMPro;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009CF RID: 2511
	public class PlayerTimerBoard : MonoBehaviour
	{
		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06003E96 RID: 16022 RVA: 0x00128B51 File Offset: 0x00126D51
		// (set) Token: 0x06003E97 RID: 16023 RVA: 0x00128B59 File Offset: 0x00126D59
		public bool IsDirty { get; set; } = true;

		// Token: 0x06003E98 RID: 16024 RVA: 0x00128B62 File Offset: 0x00126D62
		private void Start()
		{
			this.TryInit();
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x00128B62 File Offset: 0x00126D62
		private void OnEnable()
		{
			this.TryInit();
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x00128B6A File Offset: 0x00126D6A
		private void TryInit()
		{
			if (this.isInitialized)
			{
				return;
			}
			if (PlayerTimerManager.instance == null)
			{
				return;
			}
			PlayerTimerManager.instance.RegisterTimerBoard(this);
			this.isInitialized = true;
		}

		// Token: 0x06003E9B RID: 16027 RVA: 0x00128B95 File Offset: 0x00126D95
		private void OnDisable()
		{
			if (PlayerTimerManager.instance != null)
			{
				PlayerTimerManager.instance.UnregisterTimerBoard(this);
			}
			this.isInitialized = false;
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x00128BB6 File Offset: 0x00126DB6
		public void SetSleepState(bool awake)
		{
			this.playerColumn.enabled = awake;
			this.timeColumn.enabled = awake;
			if (this.linesParent != null)
			{
				this.linesParent.SetActive(awake);
			}
		}

		// Token: 0x06003E9D RID: 16029 RVA: 0x00128BEA File Offset: 0x00126DEA
		public void SortLines()
		{
			this.lines.Sort(new Comparison<PlayerTimerBoardLine>(PlayerTimerBoardLine.CompareByTotalTime));
		}

		// Token: 0x06003E9E RID: 16030 RVA: 0x00128C04 File Offset: 0x00126E04
		public void RedrawPlayerLines()
		{
			this.stringBuilder.Clear();
			this.stringBuilderTime.Clear();
			this.stringBuilder.Append("<b><color=yellow>PLAYER</color></b>");
			this.stringBuilderTime.Append("<b><color=yellow>LATEST TIME</color></b>");
			this.SortLines();
			ValueTuple<bool, Permission.ManagedByEnum> customNicknamePermissionStatus = KIDManager.Instance.GetCustomNicknamePermissionStatus();
			bool flag = (customNicknamePermissionStatus.Item1 || customNicknamePermissionStatus.Item2 == Permission.ManagedByEnum.PLAYER) && customNicknamePermissionStatus.Item2 != Permission.ManagedByEnum.PROHIBITED;
			for (int i = 0; i < this.lines.Count; i++)
			{
				try
				{
					if (this.lines[i].gameObject.activeInHierarchy)
					{
						this.lines[i].gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0f, (float)(this.startingYValue - this.lineHeight * i), 0f);
						if (this.lines[i].linePlayer != null && this.lines[i].linePlayer.InRoom)
						{
							this.stringBuilder.Append("\n ");
							this.stringBuilder.Append(flag ? this.lines[i].playerNameVisible : this.lines[i].linePlayer.DefaultName);
							this.stringBuilderTime.Append("\n ");
							this.stringBuilderTime.Append(this.lines[i].playerTimeStr);
						}
					}
				}
				catch
				{
				}
			}
			this.playerColumn.text = this.stringBuilder.ToString();
			this.timeColumn.text = this.stringBuilderTime.ToString();
			this.IsDirty = false;
		}

		// Token: 0x04003FE2 RID: 16354
		[SerializeField]
		private GameObject linesParent;

		// Token: 0x04003FE3 RID: 16355
		public List<PlayerTimerBoardLine> lines;

		// Token: 0x04003FE4 RID: 16356
		public TextMeshPro notInRoomText;

		// Token: 0x04003FE5 RID: 16357
		public TextMeshPro playerColumn;

		// Token: 0x04003FE6 RID: 16358
		public TextMeshPro timeColumn;

		// Token: 0x04003FE7 RID: 16359
		[SerializeField]
		private int startingYValue;

		// Token: 0x04003FE8 RID: 16360
		[SerializeField]
		private int lineHeight;

		// Token: 0x04003FE9 RID: 16361
		private StringBuilder stringBuilder = new StringBuilder(220);

		// Token: 0x04003FEA RID: 16362
		private StringBuilder stringBuilderTime = new StringBuilder(220);

		// Token: 0x04003FEB RID: 16363
		private bool isInitialized;
	}
}
