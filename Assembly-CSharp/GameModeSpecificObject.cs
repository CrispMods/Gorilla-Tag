using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GorillaGameModes;
using UnityEngine;

// Token: 0x0200007F RID: 127
public class GameModeSpecificObject : MonoBehaviour
{
	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06000342 RID: 834 RVA: 0x00014F74 File Offset: 0x00013174
	// (remove) Token: 0x06000343 RID: 835 RVA: 0x00014FA8 File Offset: 0x000131A8
	public static event GameModeSpecificObject.GameModeSpecificObjectDelegate OnAwake;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06000344 RID: 836 RVA: 0x00014FDC File Offset: 0x000131DC
	// (remove) Token: 0x06000345 RID: 837 RVA: 0x00015010 File Offset: 0x00013210
	public static event GameModeSpecificObject.GameModeSpecificObjectDelegate OnDestroyed;

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x06000346 RID: 838 RVA: 0x00015043 File Offset: 0x00013243
	public GameModeSpecificObject.ValidationMethod Validation
	{
		get
		{
			return this.validationMethod;
		}
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x06000347 RID: 839 RVA: 0x0001504B File Offset: 0x0001324B
	public List<GameModeType> GameModes
	{
		get
		{
			return this.gameModes;
		}
	}

	// Token: 0x06000348 RID: 840 RVA: 0x00015054 File Offset: 0x00013254
	private void Awake()
	{
		GameModeSpecificObject.<Awake>d__15 <Awake>d__;
		<Awake>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Awake>d__.<>4__this = this;
		<Awake>d__.<>1__state = -1;
		<Awake>d__.<>t__builder.Start<GameModeSpecificObject.<Awake>d__15>(ref <Awake>d__);
	}

	// Token: 0x06000349 RID: 841 RVA: 0x0001508B File Offset: 0x0001328B
	private void OnDestroy()
	{
		if (GameModeSpecificObject.OnDestroyed != null)
		{
			GameModeSpecificObject.OnDestroyed(this);
		}
	}

	// Token: 0x0600034A RID: 842 RVA: 0x0001509F File Offset: 0x0001329F
	public bool CheckValid(GameModeType gameMode)
	{
		if (this.validationMethod == GameModeSpecificObject.ValidationMethod.Exclusion)
		{
			return !this.gameModes.Contains(gameMode);
		}
		return this.gameModes.Contains(gameMode);
	}

	// Token: 0x040003D0 RID: 976
	[SerializeField]
	private GameModeSpecificObject.ValidationMethod validationMethod;

	// Token: 0x040003D1 RID: 977
	[SerializeField]
	private GameModeType[] _gameModes;

	// Token: 0x040003D2 RID: 978
	private List<GameModeType> gameModes;

	// Token: 0x02000080 RID: 128
	// (Invoke) Token: 0x0600034D RID: 845
	public delegate void GameModeSpecificObjectDelegate(GameModeSpecificObject gameModeSpecificObject);

	// Token: 0x02000081 RID: 129
	[Serializable]
	public enum ValidationMethod
	{
		// Token: 0x040003D4 RID: 980
		Inclusion,
		// Token: 0x040003D5 RID: 981
		Exclusion
	}
}
