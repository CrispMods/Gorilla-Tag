using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000073 RID: 115
public class MenagerieCritter : MonoBehaviour, IHoldableObject, IEyeScannable
{
	// Token: 0x17000028 RID: 40
	// (get) Token: 0x060002F6 RID: 758 RVA: 0x00032556 File Offset: 0x00030756
	public Menagerie.CritterData CritterData
	{
		get
		{
			return this._critterData;
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x060002F7 RID: 759 RVA: 0x0003255E File Offset: 0x0003075E
	// (set) Token: 0x060002F8 RID: 760 RVA: 0x00076590 File Offset: 0x00074790
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

	// Token: 0x060002F9 RID: 761 RVA: 0x00032566 File Offset: 0x00030766
	private void Update()
	{
		this.UpdateAnimation();
	}

	// Token: 0x060002FA RID: 762 RVA: 0x000765F8 File Offset: 0x000747F8
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

	// Token: 0x060002FB RID: 763 RVA: 0x0007668C File Offset: 0x0007488C
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

	// Token: 0x060002FC RID: 764 RVA: 0x000766E0 File Offset: 0x000748E0
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

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x060002FD RID: 765 RVA: 0x00030498 File Offset: 0x0002E698
	public bool TwoHanded
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060002FE RID: 766 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
	}

	// Token: 0x060002FF RID: 767 RVA: 0x000767E0 File Offset: 0x000749E0
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

	// Token: 0x06000300 RID: 768 RVA: 0x0007688C File Offset: 0x00074A8C
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

	// Token: 0x06000301 RID: 769 RVA: 0x0003256E File Offset: 0x0003076E
	public void ResetToTransform()
	{
		base.transform.parent = this._slot.transform;
		base.transform.localPosition = Vector3.zero;
		base.transform.localRotation = quaternion.identity;
	}

	// Token: 0x06000302 RID: 770 RVA: 0x00030607 File Offset: 0x0002E807
	public void DropItemCleanup()
	{
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x06000303 RID: 771 RVA: 0x00031CCF File Offset: 0x0002FECF
	int IEyeScannable.scannableId
	{
		get
		{
			return base.gameObject.GetInstanceID();
		}
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x06000304 RID: 772 RVA: 0x00076960 File Offset: 0x00074B60
	Vector3 IEyeScannable.Position
	{
		get
		{
			return this.bodyCollider.bounds.center;
		}
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000305 RID: 773 RVA: 0x000325AB File Offset: 0x000307AB
	Bounds IEyeScannable.Bounds
	{
		get
		{
			return this.bodyCollider.bounds;
		}
	}

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x06000306 RID: 774 RVA: 0x000325B8 File Offset: 0x000307B8
	IList<KeyValueStringPair> IEyeScannable.Entries
	{
		get
		{
			return this.BuildEyeScannerData();
		}
	}

	// Token: 0x06000307 RID: 775 RVA: 0x000325C0 File Offset: 0x000307C0
	public void OnEnable()
	{
		EyeScannerMono.Register(this);
	}

	// Token: 0x06000308 RID: 776 RVA: 0x000325C8 File Offset: 0x000307C8
	public void OnDisable()
	{
		EyeScannerMono.Unregister(this);
	}

	// Token: 0x06000309 RID: 777 RVA: 0x00076980 File Offset: 0x00074B80
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
	// (add) Token: 0x0600030A RID: 778 RVA: 0x00076A7C File Offset: 0x00074C7C
	// (remove) Token: 0x0600030B RID: 779 RVA: 0x00076AB4 File Offset: 0x00074CB4
	public event Action OnDataChange;

	// Token: 0x0600030C RID: 780 RVA: 0x000325D0 File Offset: 0x000307D0
	private string GetCurrentStateName()
	{
		if (!this.isHeld)
		{
			return "Content";
		}
		return "Happy";
	}

	// Token: 0x0600030E RID: 782 RVA: 0x00032616 File Offset: 0x00030816
	GameObject IHoldableObject.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0600030F RID: 783 RVA: 0x0003261E File Offset: 0x0003081E
	string IHoldableObject.get_name()
	{
		return base.name;
	}

	// Token: 0x06000310 RID: 784 RVA: 0x00032626 File Offset: 0x00030826
	void IHoldableObject.set_name(string value)
	{
		base.name = value;
	}

	// Token: 0x04000397 RID: 919
	public CritterVisuals visuals;

	// Token: 0x04000398 RID: 920
	public Collider bodyCollider;

	// Token: 0x04000399 RID: 921
	[Header("Feedback")]
	public CrittersAnim heldAnimation;

	// Token: 0x0400039A RID: 922
	public AudioClip grabbedHaptics;

	// Token: 0x0400039B RID: 923
	public float grabbedHapticsStrength = 1f;

	// Token: 0x0400039C RID: 924
	public GameObject grabbedFX;

	// Token: 0x0400039D RID: 925
	private CrittersAnim _currentAnim;

	// Token: 0x0400039E RID: 926
	private float _currentAnimTime;

	// Token: 0x0400039F RID: 927
	private Transform _animRoot;

	// Token: 0x040003A0 RID: 928
	private Vector3 _bodyScale;

	// Token: 0x040003A1 RID: 929
	public MenagerieCritter.MenagerieCritterState currentState = MenagerieCritter.MenagerieCritterState.Displaying;

	// Token: 0x040003A2 RID: 930
	private CritterConfiguration _critterConfiguration;

	// Token: 0x040003A3 RID: 931
	private Menagerie.CritterData _critterData;

	// Token: 0x040003A4 RID: 932
	private MenagerieSlot _slot;

	// Token: 0x040003A5 RID: 933
	private List<GorillaGrabber> activeGrabbers = new List<GorillaGrabber>();

	// Token: 0x040003A6 RID: 934
	private GameObject heldBy;

	// Token: 0x040003A7 RID: 935
	private bool isHeld;

	// Token: 0x040003A8 RID: 936
	private bool isHeldLeftHand;

	// Token: 0x040003A9 RID: 937
	public Action<MenagerieCritter> OnReleased;

	// Token: 0x040003AA RID: 938
	private KeyValueStringPair[] eyeScanData = new KeyValueStringPair[6];

	// Token: 0x02000074 RID: 116
	public enum MenagerieCritterState
	{
		// Token: 0x040003AD RID: 941
		Donating,
		// Token: 0x040003AE RID: 942
		Displaying
	}
}
