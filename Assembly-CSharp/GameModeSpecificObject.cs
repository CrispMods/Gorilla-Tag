using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GorillaGameModes;
using UnityEngine;

// Token: 0x02000086 RID: 134
public class GameModeSpecificObject : MonoBehaviour
{
	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06000374 RID: 884 RVA: 0x00079398 File Offset: 0x00077598
	// (remove) Token: 0x06000375 RID: 885 RVA: 0x000793CC File Offset: 0x000775CC
	public static event GameModeSpecificObject.GameModeSpecificObjectDelegate OnAwake;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06000376 RID: 886 RVA: 0x00079400 File Offset: 0x00077600
	// (remove) Token: 0x06000377 RID: 887 RVA: 0x00079434 File Offset: 0x00077634
	public static event GameModeSpecificObject.GameModeSpecificObjectDelegate OnDestroyed;

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x06000378 RID: 888 RVA: 0x00032A96 File Offset: 0x00030C96
	public GameModeSpecificObject.ValidationMethod Validation
	{
		get
		{
			return this.validationMethod;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x06000379 RID: 889 RVA: 0x00032A9E File Offset: 0x00030C9E
	public List<GameModeType> GameModes
	{
		get
		{
			return this.gameModes;
		}
	}

	// Token: 0x0600037A RID: 890 RVA: 0x00079468 File Offset: 0x00077668
	private void Awake()
	{
		GameModeSpecificObject.<Awake>d__15 <Awake>d__;
		<Awake>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Awake>d__.<>4__this = this;
		<Awake>d__.<>1__state = -1;
		<Awake>d__.<>t__builder.Start<GameModeSpecificObject.<Awake>d__15>(ref <Awake>d__);
	}

	// Token: 0x0600037B RID: 891 RVA: 0x00032AA6 File Offset: 0x00030CA6
	private void OnDestroy()
	{
		if (GameModeSpecificObject.OnDestroyed != null)
		{
			GameModeSpecificObject.OnDestroyed(this);
		}
	}

	// Token: 0x0600037C RID: 892 RVA: 0x00032ABA File Offset: 0x00030CBA
	public bool CheckValid(GameModeType gameMode)
	{
		if (this.validationMethod == GameModeSpecificObject.ValidationMethod.Exclusion)
		{
			return !this.gameModes.Contains(gameMode);
		}
		return this.gameModes.Contains(gameMode);
	}

	// Token: 0x04000404 RID: 1028
	[SerializeField]
	private GameModeSpecificObject.ValidationMethod validationMethod;

	// Token: 0x04000405 RID: 1029
	[SerializeField]
	private GameModeType[] _gameModes;

	// Token: 0x04000406 RID: 1030
	private List<GameModeType> gameModes;

	// Token: 0x02000087 RID: 135
	// (Invoke) Token: 0x0600037F RID: 895
	public delegate void GameModeSpecificObjectDelegate(GameModeSpecificObject gameModeSpecificObject);

	// Token: 0x02000088 RID: 136
	[Serializable]
	public enum ValidationMethod
	{
		// Token: 0x04000408 RID: 1032
		Inclusion,
		// Token: 0x04000409 RID: 1033
		Exclusion
	}
}
