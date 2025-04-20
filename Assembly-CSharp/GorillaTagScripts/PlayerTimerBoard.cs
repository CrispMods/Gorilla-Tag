using System;
using System.Collections.Generic;
using System.Text;
using KID.Model;
using TMPro;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009F2 RID: 2546
	public class PlayerTimerBoard : MonoBehaviour
	{
		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06003FA2 RID: 16290 RVA: 0x00059957 File Offset: 0x00057B57
		// (set) Token: 0x06003FA3 RID: 16291 RVA: 0x0005995F File Offset: 0x00057B5F
		public bool IsDirty { get; set; } = true;

		// Token: 0x06003FA4 RID: 16292 RVA: 0x00059968 File Offset: 0x00057B68
		private void Start()
		{
			this.TryInit();
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x00059968 File Offset: 0x00057B68
		private void OnEnable()
		{
			this.TryInit();
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x00059970 File Offset: 0x00057B70
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

		// Token: 0x06003FA7 RID: 16295 RVA: 0x0005999B File Offset: 0x00057B9B
		private void OnDisable()
		{
			if (PlayerTimerManager.instance != null)
			{
				PlayerTimerManager.instance.UnregisterTimerBoard(this);
			}
			this.isInitialized = false;
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x000599BC File Offset: 0x00057BBC
		public void SetSleepState(bool awake)
		{
			this.playerColumn.enabled = awake;
			this.timeColumn.enabled = awake;
			if (this.linesParent != null)
			{
				this.linesParent.SetActive(awake);
			}
		}

		// Token: 0x06003FA9 RID: 16297 RVA: 0x000599F0 File Offset: 0x00057BF0
		public void SortLines()
		{
			this.lines.Sort(new Comparison<PlayerTimerBoardLine>(PlayerTimerBoardLine.CompareByTotalTime));
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x001696C8 File Offset: 0x001678C8
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

		// Token: 0x040040AA RID: 16554
		[SerializeField]
		private GameObject linesParent;

		// Token: 0x040040AB RID: 16555
		public List<PlayerTimerBoardLine> lines;

		// Token: 0x040040AC RID: 16556
		public TextMeshPro notInRoomText;

		// Token: 0x040040AD RID: 16557
		public TextMeshPro playerColumn;

		// Token: 0x040040AE RID: 16558
		public TextMeshPro timeColumn;

		// Token: 0x040040AF RID: 16559
		[SerializeField]
		private int startingYValue;

		// Token: 0x040040B0 RID: 16560
		[SerializeField]
		private int lineHeight;

		// Token: 0x040040B1 RID: 16561
		private StringBuilder stringBuilder = new StringBuilder(220);

		// Token: 0x040040B2 RID: 16562
		private StringBuilder stringBuilderTime = new StringBuilder(220);

		// Token: 0x040040B3 RID: 16563
		private bool isInitialized;
	}
}
