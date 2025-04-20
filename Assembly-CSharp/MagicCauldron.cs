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

// Token: 0x020005DA RID: 1498
[NetworkBehaviourWeaved(4)]
public class MagicCauldron : NetworkComponent
{
	// Token: 0x06002537 RID: 9527 RVA: 0x00105484 File Offset: 0x00103684
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

	// Token: 0x06002538 RID: 9528 RVA: 0x000493A9 File Offset: 0x000475A9
	private new void Start()
	{
		this.ChangeState(MagicCauldron.CauldronState.notReady);
	}

	// Token: 0x06002539 RID: 9529 RVA: 0x000493B2 File Offset: 0x000475B2
	private void LateUpdate()
	{
		this.UpdateState();
	}

	// Token: 0x0600253A RID: 9530 RVA: 0x000493BA File Offset: 0x000475BA
	private IEnumerator LevitationSpellCoroutine()
	{
		GTPlayer.Instance.SetHalloweenLevitation(this.levitationStrength, this.levitationDuration, this.levitationBlendOutDuration, this.levitationBonusStrength, this.levitationBonusOffAtYSpeed, this.levitationBonusFullAtYSpeed);
		yield return new WaitForSeconds(this.levitationSpellDuration);
		GTPlayer.Instance.SetHalloweenLevitation(0f, this.levitationDuration, this.levitationBlendOutDuration, 0f, this.levitationBonusOffAtYSpeed, this.levitationBonusFullAtYSpeed);
		yield break;
	}

	// Token: 0x0600253B RID: 9531 RVA: 0x00105564 File Offset: 0x00103764
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

	// Token: 0x0600253C RID: 9532 RVA: 0x0010575C File Offset: 0x0010395C
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

	// Token: 0x0600253D RID: 9533 RVA: 0x000493C9 File Offset: 0x000475C9
	public void OnEventStart()
	{
		this.ChangeState(MagicCauldron.CauldronState.ready);
	}

	// Token: 0x0600253E RID: 9534 RVA: 0x000493A9 File Offset: 0x000475A9
	public void OnEventEnd()
	{
		this.ChangeState(MagicCauldron.CauldronState.notReady);
	}

	// Token: 0x0600253F RID: 9535 RVA: 0x000493D2 File Offset: 0x000475D2
	[PunRPC]
	public void OnIngredientAdd(int _ingredientIndex, PhotonMessageInfo info)
	{
		this.OnIngredientAddShared(_ingredientIndex, info);
	}

	// Token: 0x06002540 RID: 9536 RVA: 0x00105828 File Offset: 0x00103A28
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

	// Token: 0x06002541 RID: 9537 RVA: 0x00105968 File Offset: 0x00103B68
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

	// Token: 0x06002542 RID: 9538 RVA: 0x001059CC File Offset: 0x00103BCC
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

	// Token: 0x06002543 RID: 9539 RVA: 0x00105B00 File Offset: 0x00103D00
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

	// Token: 0x06002544 RID: 9540 RVA: 0x00105B78 File Offset: 0x00103D78
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

	// Token: 0x06002545 RID: 9541 RVA: 0x00105C34 File Offset: 0x00103E34
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

	// Token: 0x06002546 RID: 9542 RVA: 0x000493E1 File Offset: 0x000475E1
	internal override void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
		base.OnDisable();
		this.currentIngredients.Clear();
	}

	// Token: 0x170003CF RID: 975
	// (get) Token: 0x06002547 RID: 9543 RVA: 0x000493FA File Offset: 0x000475FA
	// (set) Token: 0x06002548 RID: 9544 RVA: 0x00049424 File Offset: 0x00047624
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

	// Token: 0x06002549 RID: 9545 RVA: 0x0004944F File Offset: 0x0004764F
	public override void WriteDataFusion()
	{
		this.Data = new MagicCauldron.MagicCauldronData(this.currentStateElapsedTime, this.currentRecipeIndex, this.currentState, this.ingredientIndex);
	}

	// Token: 0x0600254A RID: 9546 RVA: 0x00105D10 File Offset: 0x00103F10
	public override void ReadDataFusion()
	{
		this.ReadDataShared(this.Data.CurrentStateElapsedTime, this.Data.CurrentRecipeIndex, this.Data.CurrentState, this.Data.IngredientIndex);
	}

	// Token: 0x0600254B RID: 9547 RVA: 0x00105D5C File Offset: 0x00103F5C
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

	// Token: 0x0600254C RID: 9548 RVA: 0x00105DBC File Offset: 0x00103FBC
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

	// Token: 0x0600254D RID: 9549 RVA: 0x00105E14 File Offset: 0x00104014
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

	// Token: 0x0600254F RID: 9551 RVA: 0x00049474 File Offset: 0x00047674
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x06002550 RID: 9552 RVA: 0x0004948C File Offset: 0x0004768C
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x06002551 RID: 9553 RVA: 0x00105F14 File Offset: 0x00104114
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

	// Token: 0x0400294F RID: 10575
	public List<MagicCauldron.Recipe> recipes = new List<MagicCauldron.Recipe>();

	// Token: 0x04002950 RID: 10576
	public float maxTimeToAddAllIngredients = 30f;

	// Token: 0x04002951 RID: 10577
	public float summonWitchesDuration = 20f;

	// Token: 0x04002952 RID: 10578
	public float recipeFailedDuration = 5f;

	// Token: 0x04002953 RID: 10579
	public float cooldownDuration = 30f;

	// Token: 0x04002954 RID: 10580
	public MagicIngredientType[] allIngredients;

	// Token: 0x04002955 RID: 10581
	public GameObject flyingWitchesContainer;

	// Token: 0x04002956 RID: 10582
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04002957 RID: 10583
	public AudioClip ingredientAddedAudio;

	// Token: 0x04002958 RID: 10584
	public AudioClip recipeFailedAudio;

	// Token: 0x04002959 RID: 10585
	public ParticleSystem bubblesParticle;

	// Token: 0x0400295A RID: 10586
	public ParticleSystem successParticle;

	// Token: 0x0400295B RID: 10587
	public ParticleSystem splashParticle;

	// Token: 0x0400295C RID: 10588
	public Color CauldronActiveColor;

	// Token: 0x0400295D RID: 10589
	public Color CauldronFailedColor;

	// Token: 0x0400295E RID: 10590
	[Tooltip("only if we are using the time of day event")]
	public Color CauldronNotReadyColor;

	// Token: 0x0400295F RID: 10591
	private readonly List<NoncontrollableBroomstick> witchesComponent = new List<NoncontrollableBroomstick>();

	// Token: 0x04002960 RID: 10592
	private readonly List<MagicIngredientType> currentIngredients = new List<MagicIngredientType>();

	// Token: 0x04002961 RID: 10593
	private float currentStateElapsedTime;

	// Token: 0x04002962 RID: 10594
	private MagicCauldron.CauldronState currentState;

	// Token: 0x04002963 RID: 10595
	[SerializeField]
	private Renderer rendr;

	// Token: 0x04002964 RID: 10596
	private Color cauldronColor;

	// Token: 0x04002965 RID: 10597
	private Color currentColor;

	// Token: 0x04002966 RID: 10598
	private int currentRecipeIndex;

	// Token: 0x04002967 RID: 10599
	private int ingredientIndex;

	// Token: 0x04002968 RID: 10600
	private float waitTimeToSummonWitches = 2f;

	// Token: 0x04002969 RID: 10601
	[Space]
	[SerializeField]
	private MagicCauldronLiquid _liquid;

	// Token: 0x0400296A RID: 10602
	private MagicCauldron.IngrediantFXContext reusableFXContext = new MagicCauldron.IngrediantFXContext();

	// Token: 0x0400296B RID: 10603
	private MagicCauldron.IngredientArgs reusableIngrediantArgs = new MagicCauldron.IngredientArgs();

	// Token: 0x0400296C RID: 10604
	public bool testLevitationAlwaysOn;

	// Token: 0x0400296D RID: 10605
	public float levitationRadius;

	// Token: 0x0400296E RID: 10606
	public float levitationSpellDuration;

	// Token: 0x0400296F RID: 10607
	public float levitationStrength;

	// Token: 0x04002970 RID: 10608
	public float levitationDuration;

	// Token: 0x04002971 RID: 10609
	public float levitationBlendOutDuration;

	// Token: 0x04002972 RID: 10610
	public float levitationBonusStrength;

	// Token: 0x04002973 RID: 10611
	public float levitationBonusOffAtYSpeed;

	// Token: 0x04002974 RID: 10612
	public float levitationBonusFullAtYSpeed;

	// Token: 0x04002975 RID: 10613
	[WeaverGenerated]
	[DefaultForProperty("Data", 0, 4)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private MagicCauldron.MagicCauldronData _Data;

	// Token: 0x020005DB RID: 1499
	private enum CauldronState
	{
		// Token: 0x04002977 RID: 10615
		notReady,
		// Token: 0x04002978 RID: 10616
		ready,
		// Token: 0x04002979 RID: 10617
		recipeCollecting,
		// Token: 0x0400297A RID: 10618
		recipeActivated,
		// Token: 0x0400297B RID: 10619
		summoned,
		// Token: 0x0400297C RID: 10620
		failed,
		// Token: 0x0400297D RID: 10621
		cooldown
	}

	// Token: 0x020005DC RID: 1500
	[Serializable]
	public struct Recipe
	{
		// Token: 0x0400297E RID: 10622
		public List<MagicIngredientType> recipeIngredients;

		// Token: 0x0400297F RID: 10623
		public AudioClip successAudio;
	}

	// Token: 0x020005DD RID: 1501
	private class IngredientArgs : FXSArgs
	{
		// Token: 0x04002980 RID: 10624
		public int key;
	}

	// Token: 0x020005DE RID: 1502
	private class IngrediantFXContext : IFXContextParems<MagicCauldron.IngredientArgs>
	{
		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06002553 RID: 9555 RVA: 0x000494A0 File Offset: 0x000476A0
		FXSystemSettings IFXContextParems<MagicCauldron.IngredientArgs>.settings
		{
			get
			{
				return this.playerSettings;
			}
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x000494A8 File Offset: 0x000476A8
		void IFXContextParems<MagicCauldron.IngredientArgs>.OnPlayFX(MagicCauldron.IngredientArgs args)
		{
			this.fxCallBack(args.key);
		}

		// Token: 0x04002981 RID: 10625
		public FXSystemSettings playerSettings;

		// Token: 0x04002982 RID: 10626
		public MagicCauldron.IngrediantFXContext.Callback fxCallBack;

		// Token: 0x020005DF RID: 1503
		// (Invoke) Token: 0x06002557 RID: 9559
		public delegate void Callback(int key);
	}

	// Token: 0x020005E0 RID: 1504
	[NetworkStructWeaved(4)]
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	private struct MagicCauldronData : INetworkStruct
	{
		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x0600255A RID: 9562 RVA: 0x000494BB File Offset: 0x000476BB
		// (set) Token: 0x0600255B RID: 9563 RVA: 0x000494C3 File Offset: 0x000476C3
		public float CurrentStateElapsedTime { readonly get; set; }

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x0600255C RID: 9564 RVA: 0x000494CC File Offset: 0x000476CC
		// (set) Token: 0x0600255D RID: 9565 RVA: 0x000494D4 File Offset: 0x000476D4
		public int CurrentRecipeIndex { readonly get; set; }

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x0600255E RID: 9566 RVA: 0x000494DD File Offset: 0x000476DD
		// (set) Token: 0x0600255F RID: 9567 RVA: 0x000494E5 File Offset: 0x000476E5
		public MagicCauldron.CauldronState CurrentState { readonly get; set; }

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06002560 RID: 9568 RVA: 0x000494EE File Offset: 0x000476EE
		// (set) Token: 0x06002561 RID: 9569 RVA: 0x000494F6 File Offset: 0x000476F6
		public int IngredientIndex { readonly get; set; }

		// Token: 0x06002562 RID: 9570 RVA: 0x000494FF File Offset: 0x000476FF
		public MagicCauldronData(float stateElapsedTime, int recipeIndex, MagicCauldron.CauldronState state, int ingredientIndex)
		{
			this.CurrentStateElapsedTime = stateElapsedTime;
			this.CurrentRecipeIndex = recipeIndex;
			this.CurrentState = state;
			this.IngredientIndex = ingredientIndex;
		}
	}
}
