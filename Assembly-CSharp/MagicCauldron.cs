using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Fusion;
using GorillaLocomotion;
using GorillaLocomotion.Gameplay;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005CC RID: 1484
[NetworkBehaviourWeaved(4)]
public class MagicCauldron : NetworkComponent
{
	// Token: 0x060024D5 RID: 9429 RVA: 0x000B6FE8 File Offset: 0x000B51E8
	private new void Awake()
	{
		this.currentIngredients.Clear();
		this.witchesComponent.Clear();
		this.currentStateElapsedTime = 0f;
		this.currentRecipeIndex = -1;
		this.ingredientIndex = -1;
		if (this.flyingWitchesContainer != null)
		{
			for (int i = 0; i < this.flyingWitchesContainer.transform.childCount; i++)
			{
				NoncontrollableBroomstick componentInChildren = this.flyingWitchesContainer.transform.GetChild(i).gameObject.GetComponentInChildren<NoncontrollableBroomstick>();
				this.witchesComponent.Add(componentInChildren);
				if (componentInChildren)
				{
					componentInChildren.gameObject.SetActive(false);
				}
			}
		}
		if (this.reusableFXContext == null)
		{
			this.reusableFXContext = new MagicCauldron.IngrediantFXContext();
		}
		if (this.reusableIngrediantArgs == null)
		{
			this.reusableIngrediantArgs = new MagicCauldron.IngredientArgs();
		}
		this.reusableFXContext.fxCallBack = new MagicCauldron.IngrediantFXContext.Callback(this.OnIngredientAdd);
	}

	// Token: 0x060024D6 RID: 9430 RVA: 0x000B70C6 File Offset: 0x000B52C6
	private new void Start()
	{
		this.ChangeState(MagicCauldron.CauldronState.notReady);
	}

	// Token: 0x060024D7 RID: 9431 RVA: 0x000B70CF File Offset: 0x000B52CF
	private void LateUpdate()
	{
		this.UpdateState();
	}

	// Token: 0x060024D8 RID: 9432 RVA: 0x000B70D7 File Offset: 0x000B52D7
	private IEnumerator LevitationSpellCoroutine()
	{
		GTPlayer.Instance.SetHalloweenLevitation(this.levitationStrength, this.levitationDuration, this.levitationBlendOutDuration, this.levitationBonusStrength, this.levitationBonusOffAtYSpeed, this.levitationBonusFullAtYSpeed);
		yield return new WaitForSeconds(this.levitationSpellDuration);
		GTPlayer.Instance.SetHalloweenLevitation(0f, this.levitationDuration, this.levitationBlendOutDuration, 0f, this.levitationBonusOffAtYSpeed, this.levitationBonusFullAtYSpeed);
		yield break;
	}

	// Token: 0x060024D9 RID: 9433 RVA: 0x000B70E8 File Offset: 0x000B52E8
	private void ChangeState(MagicCauldron.CauldronState state)
	{
		this.currentState = state;
		if (base.IsMine)
		{
			this.currentStateElapsedTime = 0f;
		}
		bool flag = state == MagicCauldron.CauldronState.summoned;
		foreach (NoncontrollableBroomstick noncontrollableBroomstick in this.witchesComponent)
		{
			if (noncontrollableBroomstick.gameObject.activeSelf != flag)
			{
				noncontrollableBroomstick.gameObject.SetActive(flag);
			}
		}
		if (this.currentState == MagicCauldron.CauldronState.summoned && Vector3.Distance(GTPlayer.Instance.transform.position, base.transform.position) < this.levitationRadius)
		{
			base.StartCoroutine(this.LevitationSpellCoroutine());
		}
		switch (this.currentState)
		{
		case MagicCauldron.CauldronState.notReady:
			this.currentIngredients.Clear();
			this.UpdateCauldronColor(this.CauldronNotReadyColor);
			return;
		case MagicCauldron.CauldronState.ready:
			this.UpdateCauldronColor(this.CauldronActiveColor);
			return;
		case MagicCauldron.CauldronState.recipeCollecting:
			if (this.ingredientIndex >= 0 && this.ingredientIndex < this.allIngredients.Length)
			{
				this.UpdateCauldronColor(this.allIngredients[this.ingredientIndex].color);
				return;
			}
			break;
		case MagicCauldron.CauldronState.recipeActivated:
			if (this.audioSource && this.recipes[this.currentRecipeIndex].successAudio)
			{
				this.audioSource.GTPlayOneShot(this.recipes[this.currentRecipeIndex].successAudio, 1f);
			}
			if (this.successParticle)
			{
				this.successParticle.Play();
				return;
			}
			break;
		case MagicCauldron.CauldronState.summoned:
			break;
		case MagicCauldron.CauldronState.failed:
			this.currentIngredients.Clear();
			this.UpdateCauldronColor(this.CauldronFailedColor);
			this.audioSource.GTPlayOneShot(this.recipeFailedAudio, 1f);
			return;
		case MagicCauldron.CauldronState.cooldown:
			this.currentIngredients.Clear();
			this.UpdateCauldronColor(this.CauldronFailedColor);
			break;
		default:
			return;
		}
	}

	// Token: 0x060024DA RID: 9434 RVA: 0x000B72E0 File Offset: 0x000B54E0
	private void UpdateState()
	{
		if (base.IsMine)
		{
			this.currentStateElapsedTime += Time.deltaTime;
			switch (this.currentState)
			{
			case MagicCauldron.CauldronState.notReady:
			case MagicCauldron.CauldronState.ready:
				break;
			case MagicCauldron.CauldronState.recipeCollecting:
				if (this.currentStateElapsedTime >= this.maxTimeToAddAllIngredients && !this.CheckIngredients())
				{
					this.ChangeState(MagicCauldron.CauldronState.failed);
					return;
				}
				break;
			case MagicCauldron.CauldronState.recipeActivated:
				if (this.currentStateElapsedTime >= this.waitTimeToSummonWitches)
				{
					this.ChangeState(MagicCauldron.CauldronState.summoned);
					return;
				}
				break;
			case MagicCauldron.CauldronState.summoned:
				if (this.currentStateElapsedTime >= this.summonWitchesDuration)
				{
					this.ChangeState(MagicCauldron.CauldronState.cooldown);
					return;
				}
				break;
			case MagicCauldron.CauldronState.failed:
				if (this.currentStateElapsedTime >= this.recipeFailedDuration)
				{
					this.ChangeState(MagicCauldron.CauldronState.ready);
					return;
				}
				break;
			case MagicCauldron.CauldronState.cooldown:
				if (this.currentStateElapsedTime >= this.cooldownDuration)
				{
					this.ChangeState(MagicCauldron.CauldronState.ready);
				}
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x060024DB RID: 9435 RVA: 0x000B73A9 File Offset: 0x000B55A9
	public void OnEventStart()
	{
		this.ChangeState(MagicCauldron.CauldronState.ready);
	}

	// Token: 0x060024DC RID: 9436 RVA: 0x000B70C6 File Offset: 0x000B52C6
	public void OnEventEnd()
	{
		this.ChangeState(MagicCauldron.CauldronState.notReady);
	}

	// Token: 0x060024DD RID: 9437 RVA: 0x000B73B2 File Offset: 0x000B55B2
	[PunRPC]
	public void OnIngredientAdd(int _ingredientIndex, PhotonMessageInfo info)
	{
		this.OnIngredientAddShared(_ingredientIndex, info);
	}

	// Token: 0x060024DE RID: 9438 RVA: 0x000B73C4 File Offset: 0x000B55C4
	[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
	public unsafe void RPC_OnIngredientAdd(int _ingredientIndex, RpcInfo info = default(RpcInfo))
	{
		if (!this.InvokeRpc)
		{
			NetworkBehaviourUtils.ThrowIfBehaviourNotInitialized(this);
			if (base.Runner.Stage != SimulationStages.Resimulate)
			{
				int localAuthorityMask = base.Object.GetLocalAuthorityMask();
				if ((localAuthorityMask & 1) == 0)
				{
					NetworkBehaviourUtils.NotifyLocalSimulationNotAllowedToSendRpc("System.Void MagicCauldron::RPC_OnIngredientAdd(System.Int32,Fusion.RpcInfo)", base.Object, 1);
				}
				else
				{
					if (base.Runner.HasAnyActiveConnections())
					{
						int num = 8;
						num += 4;
						SimulationMessage* ptr = SimulationMessage.Allocate(base.Runner.Simulation, num);
						byte* data = SimulationMessage.GetData(ptr);
						int num2 = RpcHeader.Write(RpcHeader.Create(base.Object.Id, this.ObjectIndex, 1), data);
						*(int*)(data + num2) = _ingredientIndex;
						num2 += 4;
						ptr->Offset = num2 * 8;
						base.Runner.SendRpc(ptr);
					}
					if ((localAuthorityMask & 7) != 0)
					{
						info = RpcInfo.FromLocal(base.Runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
						goto IL_12;
					}
				}
			}
			return;
		}
		this.InvokeRpc = false;
		IL_12:
		this.OnIngredientAddShared(_ingredientIndex, info);
	}

	// Token: 0x060024DF RID: 9439 RVA: 0x000B7504 File Offset: 0x000B5704
	private void OnIngredientAddShared(int _ingredientIndex, PhotonMessageInfoWrapped info)
	{
		GorillaNot.IncrementRPCCall(info, "OnIngredientAdd");
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(info.Sender, out rigContainer))
		{
			return;
		}
		this.reusableFXContext.playerSettings = rigContainer.Rig.fxSettings;
		this.reusableIngrediantArgs.key = _ingredientIndex;
		FXSystem.PlayFX<MagicCauldron.IngredientArgs>(FXType.HWIngredients, this.reusableFXContext, this.reusableIngrediantArgs, info);
	}

	// Token: 0x060024E0 RID: 9440 RVA: 0x000B7568 File Offset: 0x000B5768
	private void OnIngredientAdd(int _ingredientIndex)
	{
		if (this.audioSource)
		{
			this.audioSource.GTPlayOneShot(this.ingredientAddedAudio, 1f);
		}
		if (!RoomSystem.AmITheHost)
		{
			return;
		}
		if (_ingredientIndex < 0 || _ingredientIndex >= this.allIngredients.Length || (this.currentState != MagicCauldron.CauldronState.ready && this.currentState != MagicCauldron.CauldronState.recipeCollecting))
		{
			return;
		}
		MagicIngredientType magicIngredientType = this.allIngredients[_ingredientIndex];
		Debug.Log(string.Format("Received ingredient RPC {0} = {1}", _ingredientIndex, magicIngredientType));
		MagicIngredientType magicIngredientType2 = null;
		if (this.recipes[0].recipeIngredients.Count > this.currentIngredients.Count)
		{
			magicIngredientType2 = this.recipes[0].recipeIngredients[this.currentIngredients.Count];
		}
		if (!(magicIngredientType == magicIngredientType2))
		{
			Debug.Log(string.Format("Failure: Expected ingredient {0}, got {1} from recipe[{2}]", magicIngredientType2, magicIngredientType, this.currentIngredients.Count));
			this.ChangeState(MagicCauldron.CauldronState.failed);
			return;
		}
		this.ingredientIndex = _ingredientIndex;
		this.currentIngredients.Add(magicIngredientType);
		if (this.CheckIngredients())
		{
			this.ChangeState(MagicCauldron.CauldronState.recipeActivated);
			return;
		}
		if (this.currentState == MagicCauldron.CauldronState.ready)
		{
			this.ChangeState(MagicCauldron.CauldronState.recipeCollecting);
			return;
		}
		this.UpdateCauldronColor(magicIngredientType.color);
	}

	// Token: 0x060024E1 RID: 9441 RVA: 0x000B769C File Offset: 0x000B589C
	private bool CheckIngredients()
	{
		foreach (MagicCauldron.Recipe recipe in this.recipes)
		{
			if (this.currentIngredients.SequenceEqual(recipe.recipeIngredients))
			{
				this.currentRecipeIndex = this.recipes.IndexOf(recipe);
				return true;
			}
		}
		return false;
	}

	// Token: 0x060024E2 RID: 9442 RVA: 0x000B7714 File Offset: 0x000B5914
	private void UpdateCauldronColor(Color color)
	{
		if (this.bubblesParticle)
		{
			if (this.bubblesParticle.isPlaying)
			{
				if (this.currentState == MagicCauldron.CauldronState.failed || this.currentState == MagicCauldron.CauldronState.notReady)
				{
					this.bubblesParticle.Stop();
				}
			}
			else
			{
				this.bubblesParticle.Play();
			}
		}
		this.currentColor = this.cauldronColor;
		if (this.currentColor == color)
		{
			return;
		}
		if (this.rendr)
		{
			this._liquid.AnimateColorFromTo(this.cauldronColor, color, 1f);
			this.cauldronColor = color;
		}
		if (this.bubblesParticle)
		{
			this.bubblesParticle.main.startColor = color;
		}
	}

	// Token: 0x060024E3 RID: 9443 RVA: 0x000B77D0 File Offset: 0x000B59D0
	private void OnTriggerEnter(Collider other)
	{
		ThrowableSetDressing componentInParent = other.GetComponentInParent<ThrowableSetDressing>();
		if (componentInParent == null || componentInParent.IngredientTypeSO == null || componentInParent.InHand())
		{
			return;
		}
		if (componentInParent.IsLocalOwnedWorldShareable)
		{
			if (componentInParent.IngredientTypeSO != null && (this.currentState == MagicCauldron.CauldronState.ready || this.currentState == MagicCauldron.CauldronState.recipeCollecting))
			{
				int num = this.allIngredients.IndexOfRef(componentInParent.IngredientTypeSO);
				Debug.Log(string.Format("Sending ingredient RPC {0} = {1}", componentInParent.IngredientTypeSO, num));
				base.SendRPC("OnIngredientAdd", RpcTarget.Others, new object[]
				{
					num
				});
				this.OnIngredientAdd(num);
			}
			componentInParent.StartRespawnTimer(0f);
		}
		if (componentInParent.IngredientTypeSO != null && this.splashParticle)
		{
			this.splashParticle.Play();
		}
	}

	// Token: 0x060024E4 RID: 9444 RVA: 0x000B78AC File Offset: 0x000B5AAC
	internal override void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
		base.OnDisable();
		this.currentIngredients.Clear();
	}

	// Token: 0x170003C7 RID: 967
	// (get) Token: 0x060024E5 RID: 9445 RVA: 0x000B78C5 File Offset: 0x000B5AC5
	// (set) Token: 0x060024E6 RID: 9446 RVA: 0x000B78EF File Offset: 0x000B5AEF
	[Networked]
	[NetworkedWeaved(0, 4)]
	private unsafe MagicCauldron.MagicCauldronData Data
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing MagicCauldron.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			return *(MagicCauldron.MagicCauldronData*)(this.Ptr + 0);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing MagicCauldron.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			*(MagicCauldron.MagicCauldronData*)(this.Ptr + 0) = value;
		}
	}

	// Token: 0x060024E7 RID: 9447 RVA: 0x000B791A File Offset: 0x000B5B1A
	public override void WriteDataFusion()
	{
		this.Data = new MagicCauldron.MagicCauldronData(this.currentStateElapsedTime, this.currentRecipeIndex, this.currentState, this.ingredientIndex);
	}

	// Token: 0x060024E8 RID: 9448 RVA: 0x000B7940 File Offset: 0x000B5B40
	public override void ReadDataFusion()
	{
		this.ReadDataShared(this.Data.CurrentStateElapsedTime, this.Data.CurrentRecipeIndex, this.Data.CurrentState, this.Data.IngredientIndex);
	}

	// Token: 0x060024E9 RID: 9449 RVA: 0x000B798C File Offset: 0x000B5B8C
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		stream.SendNext(this.currentStateElapsedTime);
		stream.SendNext(this.currentRecipeIndex);
		stream.SendNext(this.currentState);
		stream.SendNext(this.ingredientIndex);
	}

	// Token: 0x060024EA RID: 9450 RVA: 0x000B79EC File Offset: 0x000B5BEC
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		float stateElapsedTime = (float)stream.ReceiveNext();
		int recipeIndex = (int)stream.ReceiveNext();
		MagicCauldron.CauldronState state = (MagicCauldron.CauldronState)stream.ReceiveNext();
		int num = (int)stream.ReceiveNext();
		this.ReadDataShared(stateElapsedTime, recipeIndex, state, num);
	}

	// Token: 0x060024EB RID: 9451 RVA: 0x000B7A44 File Offset: 0x000B5C44
	private void ReadDataShared(float stateElapsedTime, int recipeIndex, MagicCauldron.CauldronState state, int ingredientIndex)
	{
		MagicCauldron.CauldronState cauldronState = this.currentState;
		this.currentStateElapsedTime = stateElapsedTime;
		this.currentRecipeIndex = recipeIndex;
		this.currentState = state;
		this.ingredientIndex = ingredientIndex;
		if (cauldronState != this.currentState)
		{
			this.ChangeState(this.currentState);
			return;
		}
		if (this.currentState == MagicCauldron.CauldronState.recipeCollecting && ingredientIndex != ingredientIndex && ingredientIndex >= 0 && ingredientIndex < this.allIngredients.Length)
		{
			this.UpdateCauldronColor(this.allIngredients[ingredientIndex].color);
		}
	}

	// Token: 0x060024ED RID: 9453 RVA: 0x000B7B41 File Offset: 0x000B5D41
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x060024EE RID: 9454 RVA: 0x000B7B59 File Offset: 0x000B5D59
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x060024EF RID: 9455 RVA: 0x000B7B70 File Offset: 0x000B5D70
	[NetworkRpcWeavedInvoker(1, 1, 7)]
	[Preserve]
	[WeaverGenerated]
	protected unsafe static void RPC_OnIngredientAdd@Invoker(NetworkBehaviour behaviour, SimulationMessage* message)
	{
		byte* data = SimulationMessage.GetData(message);
		int num = RpcHeader.ReadSize(data) + 3 & -4;
		int num2 = *(int*)(data + num);
		num += 4;
		int num3 = num2;
		RpcInfo info = RpcInfo.FromMessage(behaviour.Runner, message, RpcHostMode.SourceIsServer);
		behaviour.InvokeRpc = true;
		((MagicCauldron)behaviour).RPC_OnIngredientAdd(num3, info);
	}

	// Token: 0x040028F0 RID: 10480
	public List<MagicCauldron.Recipe> recipes = new List<MagicCauldron.Recipe>();

	// Token: 0x040028F1 RID: 10481
	public float maxTimeToAddAllIngredients = 30f;

	// Token: 0x040028F2 RID: 10482
	public float summonWitchesDuration = 20f;

	// Token: 0x040028F3 RID: 10483
	public float recipeFailedDuration = 5f;

	// Token: 0x040028F4 RID: 10484
	public float cooldownDuration = 30f;

	// Token: 0x040028F5 RID: 10485
	public MagicIngredientType[] allIngredients;

	// Token: 0x040028F6 RID: 10486
	public GameObject flyingWitchesContainer;

	// Token: 0x040028F7 RID: 10487
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x040028F8 RID: 10488
	public AudioClip ingredientAddedAudio;

	// Token: 0x040028F9 RID: 10489
	public AudioClip recipeFailedAudio;

	// Token: 0x040028FA RID: 10490
	public ParticleSystem bubblesParticle;

	// Token: 0x040028FB RID: 10491
	public ParticleSystem successParticle;

	// Token: 0x040028FC RID: 10492
	public ParticleSystem splashParticle;

	// Token: 0x040028FD RID: 10493
	public Color CauldronActiveColor;

	// Token: 0x040028FE RID: 10494
	public Color CauldronFailedColor;

	// Token: 0x040028FF RID: 10495
	[Tooltip("only if we are using the time of day event")]
	public Color CauldronNotReadyColor;

	// Token: 0x04002900 RID: 10496
	private readonly List<NoncontrollableBroomstick> witchesComponent = new List<NoncontrollableBroomstick>();

	// Token: 0x04002901 RID: 10497
	private readonly List<MagicIngredientType> currentIngredients = new List<MagicIngredientType>();

	// Token: 0x04002902 RID: 10498
	private float currentStateElapsedTime;

	// Token: 0x04002903 RID: 10499
	private MagicCauldron.CauldronState currentState;

	// Token: 0x04002904 RID: 10500
	[SerializeField]
	private Renderer rendr;

	// Token: 0x04002905 RID: 10501
	private Color cauldronColor;

	// Token: 0x04002906 RID: 10502
	private Color currentColor;

	// Token: 0x04002907 RID: 10503
	private int currentRecipeIndex;

	// Token: 0x04002908 RID: 10504
	private int ingredientIndex;

	// Token: 0x04002909 RID: 10505
	private float waitTimeToSummonWitches = 2f;

	// Token: 0x0400290A RID: 10506
	[Space]
	[SerializeField]
	private MagicCauldronLiquid _liquid;

	// Token: 0x0400290B RID: 10507
	private MagicCauldron.IngrediantFXContext reusableFXContext = new MagicCauldron.IngrediantFXContext();

	// Token: 0x0400290C RID: 10508
	private MagicCauldron.IngredientArgs reusableIngrediantArgs = new MagicCauldron.IngredientArgs();

	// Token: 0x0400290D RID: 10509
	public bool testLevitationAlwaysOn;

	// Token: 0x0400290E RID: 10510
	public float levitationRadius;

	// Token: 0x0400290F RID: 10511
	public float levitationSpellDuration;

	// Token: 0x04002910 RID: 10512
	public float levitationStrength;

	// Token: 0x04002911 RID: 10513
	public float levitationDuration;

	// Token: 0x04002912 RID: 10514
	public float levitationBlendOutDuration;

	// Token: 0x04002913 RID: 10515
	public float levitationBonusStrength;

	// Token: 0x04002914 RID: 10516
	public float levitationBonusOffAtYSpeed;

	// Token: 0x04002915 RID: 10517
	public float levitationBonusFullAtYSpeed;

	// Token: 0x04002916 RID: 10518
	[WeaverGenerated]
	[DefaultForProperty("Data", 0, 4)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private MagicCauldron.MagicCauldronData _Data;

	// Token: 0x020005CD RID: 1485
	private enum CauldronState
	{
		// Token: 0x04002918 RID: 10520
		notReady,
		// Token: 0x04002919 RID: 10521
		ready,
		// Token: 0x0400291A RID: 10522
		recipeCollecting,
		// Token: 0x0400291B RID: 10523
		recipeActivated,
		// Token: 0x0400291C RID: 10524
		summoned,
		// Token: 0x0400291D RID: 10525
		failed,
		// Token: 0x0400291E RID: 10526
		cooldown
	}

	// Token: 0x020005CE RID: 1486
	[Serializable]
	public struct Recipe
	{
		// Token: 0x0400291F RID: 10527
		public List<MagicIngredientType> recipeIngredients;

		// Token: 0x04002920 RID: 10528
		public AudioClip successAudio;
	}

	// Token: 0x020005CF RID: 1487
	private class IngredientArgs : FXSArgs
	{
		// Token: 0x04002921 RID: 10529
		public int key;
	}

	// Token: 0x020005D0 RID: 1488
	private class IngrediantFXContext : IFXContextParems<MagicCauldron.IngredientArgs>
	{
		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x060024F1 RID: 9457 RVA: 0x000B7BDF File Offset: 0x000B5DDF
		FXSystemSettings IFXContextParems<MagicCauldron.IngredientArgs>.settings
		{
			get
			{
				return this.playerSettings;
			}
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x000B7BE7 File Offset: 0x000B5DE7
		void IFXContextParems<MagicCauldron.IngredientArgs>.OnPlayFX(MagicCauldron.IngredientArgs args)
		{
			this.fxCallBack(args.key);
		}

		// Token: 0x04002922 RID: 10530
		public FXSystemSettings playerSettings;

		// Token: 0x04002923 RID: 10531
		public MagicCauldron.IngrediantFXContext.Callback fxCallBack;

		// Token: 0x020005D1 RID: 1489
		// (Invoke) Token: 0x060024F5 RID: 9461
		public delegate void Callback(int key);
	}

	// Token: 0x020005D2 RID: 1490
	[NetworkStructWeaved(4)]
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	private struct MagicCauldronData : INetworkStruct
	{
		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x060024F8 RID: 9464 RVA: 0x000B7BFA File Offset: 0x000B5DFA
		// (set) Token: 0x060024F9 RID: 9465 RVA: 0x000B7C02 File Offset: 0x000B5E02
		public float CurrentStateElapsedTime { readonly get; set; }

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x060024FA RID: 9466 RVA: 0x000B7C0B File Offset: 0x000B5E0B
		// (set) Token: 0x060024FB RID: 9467 RVA: 0x000B7C13 File Offset: 0x000B5E13
		public int CurrentRecipeIndex { readonly get; set; }

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x060024FC RID: 9468 RVA: 0x000B7C1C File Offset: 0x000B5E1C
		// (set) Token: 0x060024FD RID: 9469 RVA: 0x000B7C24 File Offset: 0x000B5E24
		public MagicCauldron.CauldronState CurrentState { readonly get; set; }

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x060024FE RID: 9470 RVA: 0x000B7C2D File Offset: 0x000B5E2D
		// (set) Token: 0x060024FF RID: 9471 RVA: 0x000B7C35 File Offset: 0x000B5E35
		public int IngredientIndex { readonly get; set; }

		// Token: 0x06002500 RID: 9472 RVA: 0x000B7C3E File Offset: 0x000B5E3E
		public MagicCauldronData(float stateElapsedTime, int recipeIndex, MagicCauldron.CauldronState state, int ingredientIndex)
		{
			this.CurrentStateElapsedTime = stateElapsedTime;
			this.CurrentRecipeIndex = recipeIndex;
			this.CurrentState = state;
			this.IngredientIndex = ingredientIndex;
		}
	}
}
