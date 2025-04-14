using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GorillaGameModes;
using UnityEngine;

// Token: 0x0200007F RID: 127
public class GameModeSpecificObject : MonoBehaviour
{
	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06000344 RID: 836 RVA: 0x00015298 File Offset: 0x00013498
	// (remove) Token: 0x06000345 RID: 837 RVA: 0x000152CC File Offset: 0x000134CC
	public static event GameModeSpecificObject.GameModeSpecificObjectDelegate OnAwake;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06000346 RID: 838 RVA: 0x00015300 File Offset: 0x00013500
	// (remove) Token: 0x06000347 RID: 839 RVA: 0x00015334 File Offset: 0x00013534
	public static event GameModeSpecificObject.GameModeSpecificObjectDelegate OnDestroyed;

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x06000348 RID: 840 RVA: 0x00015367 File Offset: 0x00013567
	public GameModeSpecificObject.ValidationMethod Validation
	{
		get
		{
			return this.validationMethod;
		}
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x06000349 RID: 841 RVA: 0x0001536F File Offset: 0x0001356F
	public List<GameModeType> GameModes
	{
		get
		{
			return this.gameModes;
		}
	}

	// Token: 0x0600034A RID: 842 RVA: 0x00015378 File Offset: 0x00013578
	private void Awake()
	{
		GameModeSpecificObject.<Awake>d__15 <Awake>d__;
		<Awake>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Awake>d__.<>4__this = this;
		<Awake>d__.<>1__state = -1;
		<Awake>d__.<>t__builder.Start<GameModeSpecificObject.<Awake>d__15>(ref <Awake>d__);
	}

	// Token: 0x0600034B RID: 843 RVA: 0x000153AF File Offset: 0x000135AF
	private void OnDestroy()
	{
		if (GameModeSpecificObject.OnDestroyed != null)
		{
			GameModeSpecificObject.OnDestroyed(this);
		}
	}

	// Token: 0x0600034C RID: 844 RVA: 0x000153C3 File Offset: 0x000135C3
	public bool CheckValid(GameModeType gameMode)
	{
		if (this.validationMethod == GameModeSpecificObject.ValidationMethod.Exclusion)
		{
			return !this.gameModes.Contains(gameMode);
		}
		return this.gameModes.Contains(gameMode);
	}

	// Token: 0x040003D1 RID: 977
	[SerializeField]
	private GameModeSpecificObject.ValidationMethod validationMethod;

	// Token: 0x040003D2 RID: 978
	[SerializeField]
	private GameModeType[] _gameModes;

	// Token: 0x040003D3 RID: 979
	private List<GameModeType> gameModes;

	// Token: 0x02000080 RID: 128
	// (Invoke) Token: 0x0600034F RID: 847
	public delegate void GameModeSpecificObjectDelegate(GameModeSpecificObject gameModeSpecificObject);

	// Token: 0x02000081 RID: 129
	[Serializable]
	public enum ValidationMethod
	{
		// Token: 0x040003D5 RID: 981
		Inclusion,
		// Token: 0x040003D6 RID: 982
		Exclusion
	}
}
