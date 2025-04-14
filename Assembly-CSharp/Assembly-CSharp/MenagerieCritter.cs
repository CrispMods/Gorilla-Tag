using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x0200006D RID: 109
public class MenagerieCritter : MonoBehaviour, IHoldableObject, IEyeScannable
{
	// Token: 0x17000025 RID: 37
	// (get) Token: 0x060002C9 RID: 713 RVA: 0x00011F7A File Offset: 0x0001017A
	public Menagerie.CritterData CritterData
	{
		get
		{
			return this._critterData;
		}
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x060002CA RID: 714 RVA: 0x00011F82 File Offset: 0x00010182
	// (set) Token: 0x060002CB RID: 715 RVA: 0x00011F8C File Offset: 0x0001018C
	public MenagerieSlot Slot
	{
		get
		{
			return this._slot;
		}
		set
		{
			if (value == this._slot)
			{
				return;
			}
			if (this._slot && this._slot.critter == this)
			{
				this._slot.critter = null;
			}
			this._slot = value;
			if (this._slot)
			{
				this._slot.critter = this;
			}
		}
	}

	// Token: 0x060002CC RID: 716 RVA: 0x00011FF4 File Offset: 0x000101F4
	private void Update()
	{
		this.UpdateAnimation();
	}

	// Token: 0x060002CD RID: 717 RVA: 0x00011FFC File Offset: 0x000101FC
	public void ApplyCritterData(Menagerie.CritterData critterData)
	{
		this._critterData = critterData;
		this._critterConfiguration = this._critterData.GetConfiguration();
		this._critterData.instance = this;
		this._critterData.GetConfiguration().ApplyVisualsTo(this.visuals, false);
		this.visuals.SetAppearance(this._critterData.appearance);
		this._animRoot = this.visuals.bodyRoot;
		this._bodyScale = this._animRoot.localScale;
		this.PlayAnimation(this.heldAnimation, UnityEngine.Random.value);
	}

	// Token: 0x060002CE RID: 718 RVA: 0x00012090 File Offset: 0x00010290
	private void PlayAnimation(CrittersAnim anim, float time = 0f)
	{
		this._currentAnim = anim;
		this._currentAnimTime = time;
		if (this._currentAnim == null)
		{
			this._animRoot.localPosition = Vector3.zero;
			this._animRoot.localRotation = Quaternion.identity;
			this._animRoot.localScale = this._bodyScale;
		}
	}

	// Token: 0x060002CF RID: 719 RVA: 0x000120E4 File Offset: 0x000102E4
	private void UpdateAnimation()
	{
		if (this._currentAnim != null)
		{
			this._currentAnimTime += Time.deltaTime * this._currentAnim.playSpeed;
			this._currentAnimTime %= 1f;
			float num = this._currentAnim.squashAmount.Evaluate(this._currentAnimTime);
			float z = this._currentAnim.forwardOffset.Evaluate(this._currentAnimTime);
			float x = this._currentAnim.horizontalOffset.Evaluate(this._currentAnimTime);
			float y = this._currentAnim.verticalOffset.Evaluate(this._currentAnimTime);
			this._animRoot.localPosition = Vector3.Scale(this._bodyScale, new Vector3(x, y, z));
			float num2 = 1f - num;
			num2 *= 0.5f;
			num2 += 1f;
			this._animRoot.localScale = Vector3.Scale(this._bodyScale, new Vector3(num2, num, num2));
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x060002D0 RID: 720 RVA: 0x00002076 File Offset: 0x00000276
	public bool TwoHanded
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x000121E4 File Offset: 0x000103E4
	public void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		this.isHeld = true;
		this.isHeldLeftHand = (grabbingHand == EquipmentInteractor.instance.leftHand);
		if (this.grabbedHaptics)
		{
			CrittersManager.PlayHaptics(this.grabbedHaptics, this.grabbedHapticsStrength, this.isHeldLeftHand);
		}
		if (this.grabbedFX)
		{
			this.grabbedFX.SetActive(true);
		}
		EquipmentInteractor.instance.UpdateHandEquipment(this, this.isHeldLeftHand);
		base.transform.parent = grabbingHand.transform;
		this.isHeld = true;
		this.heldBy = grabbingHand;
		Action onDataChange = this.OnDataChange;
		if (onDataChange == null)
		{
			return;
		}
		onDataChange();
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x00012290 File Offset: 0x00010490
	public bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		if (EquipmentInteractor.instance.rightHandHeldEquipment == this && releasingHand != EquipmentInteractor.instance.rightHand)
		{
			return false;
		}
		if (EquipmentInteractor.instance.leftHandHeldEquipment == this && releasingHand != EquipmentInteractor.instance.leftHand)
		{
			return false;
		}
		if (this.grabbedHaptics)
		{
			CrittersManager.StopHaptics(this.isHeldLeftHand);
		}
		if (this.grabbedFX)
		{
			this.grabbedFX.SetActive(false);
		}
		EquipmentInteractor.instance.UpdateHandEquipment(null, this.isHeldLeftHand);
		this.isHeld = false;
		this.isHeldLeftHand = false;
		Action<MenagerieCritter> onReleased = this.OnReleased;
		if (onReleased != null)
		{
			onReleased(this);
		}
		Action onDataChange = this.OnDataChange;
		if (onDataChange != null)
		{
			onDataChange();
		}
		this.ResetToTransform();
		return true;
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x00012363 File Offset: 0x00010563
	public void ResetToTransform()
	{
		base.transform.parent = this._slot.transform;
		base.transform.localPosition = Vector3.zero;
		base.transform.localRotation = quaternion.identity;
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x000023F4 File Offset: 0x000005F4
	public void DropItemCleanup()
	{
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000EDE7 File Offset: 0x0000CFE7
	int IEyeScannable.scannableId
	{
		get
		{
			return base.gameObject.GetInstanceID();
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x060002D7 RID: 727 RVA: 0x000123A0 File Offset: 0x000105A0
	Vector3 IEyeScannable.Position
	{
		get
		{
			return this.bodyCollider.bounds.center;
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x060002D8 RID: 728 RVA: 0x000123C0 File Offset: 0x000105C0
	Bounds IEyeScannable.Bounds
	{
		get
		{
			return this.bodyCollider.bounds;
		}
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x060002D9 RID: 729 RVA: 0x000123CD File Offset: 0x000105CD
	IList<KeyValueStringPair> IEyeScannable.Entries
	{
		get
		{
			return this.BuildEyeScannerData();
		}
	}

	// Token: 0x060002DA RID: 730 RVA: 0x000123D5 File Offset: 0x000105D5
	public void OnEnable()
	{
		EyeScannerMono.Register(this);
	}

	// Token: 0x060002DB RID: 731 RVA: 0x000123DD File Offset: 0x000105DD
	public void OnDisable()
	{
		EyeScannerMono.Unregister(this);
	}

	// Token: 0x060002DC RID: 732 RVA: 0x000123E8 File Offset: 0x000105E8
	private IList<KeyValueStringPair> BuildEyeScannerData()
	{
		this.eyeScanData[0] = new KeyValueStringPair("Name", this._critterConfiguration.critterName);
		this.eyeScanData[1] = new KeyValueStringPair("Type", this._critterConfiguration.animalType.ToString());
		this.eyeScanData[2] = new KeyValueStringPair("Temperament", this._critterConfiguration.behaviour.temperament);
		this.eyeScanData[3] = new KeyValueStringPair("Habitat", this._critterConfiguration.biome.GetHabitatDescription());
		this.eyeScanData[4] = new KeyValueStringPair("Size", this.visuals.Appearance.size.ToString("0.00"));
		this.eyeScanData[5] = new KeyValueStringPair("State", this.GetCurrentStateName());
		return this.eyeScanData;
	}

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x060002DD RID: 733 RVA: 0x000124E4 File Offset: 0x000106E4
	// (remove) Token: 0x060002DE RID: 734 RVA: 0x0001251C File Offset: 0x0001071C
	public event Action OnDataChange;

	// Token: 0x060002DF RID: 735 RVA: 0x00012551 File Offset: 0x00010751
	private string GetCurrentStateName()
	{
		if (!this.isHeld)
		{
			return "Content";
		}
		return "Happy";
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x00012597 File Offset: 0x00010797
	GameObject IHoldableObject.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x0001259F File Offset: 0x0001079F
	string IHoldableObject.get_name()
	{
		return base.name;
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x000125A7 File Offset: 0x000107A7
	void IHoldableObject.set_name(string value)
	{
		base.name = value;
	}

	// Token: 0x04000366 RID: 870
	public CritterVisuals visuals;

	// Token: 0x04000367 RID: 871
	public Collider bodyCollider;

	// Token: 0x04000368 RID: 872
	[Header("Feedback")]
	public CrittersAnim heldAnimation;

	// Token: 0x04000369 RID: 873
	public AudioClip grabbedHaptics;

	// Token: 0x0400036A RID: 874
	public float grabbedHapticsStrength = 1f;

	// Token: 0x0400036B RID: 875
	public GameObject grabbedFX;

	// Token: 0x0400036C RID: 876
	private CrittersAnim _currentAnim;

	// Token: 0x0400036D RID: 877
	private float _currentAnimTime;

	// Token: 0x0400036E RID: 878
	private Transform _animRoot;

	// Token: 0x0400036F RID: 879
	private Vector3 _bodyScale;

	// Token: 0x04000370 RID: 880
	public MenagerieCritter.MenagerieCritterState currentState = MenagerieCritter.MenagerieCritterState.Displaying;

	// Token: 0x04000371 RID: 881
	private CritterConfiguration _critterConfiguration;

	// Token: 0x04000372 RID: 882
	private Menagerie.CritterData _critterData;

	// Token: 0x04000373 RID: 883
	private MenagerieSlot _slot;

	// Token: 0x04000374 RID: 884
	private List<GorillaGrabber> activeGrabbers = new List<GorillaGrabber>();

	// Token: 0x04000375 RID: 885
	private GameObject heldBy;

	// Token: 0x04000376 RID: 886
	private bool isHeld;

	// Token: 0x04000377 RID: 887
	private bool isHeldLeftHand;

	// Token: 0x04000378 RID: 888
	public Action<MenagerieCritter> OnReleased;

	// Token: 0x04000379 RID: 889
	private KeyValueStringPair[] eyeScanData = new KeyValueStringPair[6];

	// Token: 0x0200006E RID: 110
	public enum MenagerieCritterState
	{
		// Token: 0x0400037C RID: 892
		Donating,
		// Token: 0x0400037D RID: 893
		Displaying
	}
}
