using System;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// Token: 0x0200020D RID: 525
public class ToggleableWearable : MonoBehaviour
{
	// Token: 0x06000C59 RID: 3161 RVA: 0x0009E26C File Offset: 0x0009C46C
	protected void Awake()
	{
		this.ownerRig = base.GetComponentInParent<VRRig>();
		if (this.ownerRig == null)
		{
			GorillaTagger componentInParent = base.GetComponentInParent<GorillaTagger>();
			if (componentInParent != null)
			{
				this.ownerRig = componentInParent.offlineVRRig;
				this.ownerIsLocal = (this.ownerRig != null);
			}
		}
		if (this.ownerRig == null)
		{
			Debug.LogError("TriggerToggler: Disabling cannot find VRRig.");
			base.enabled = false;
			return;
		}
		foreach (Renderer renderer in this.renderers)
		{
			if (renderer == null)
			{
				Debug.LogError("TriggerToggler: Disabling because a renderer is null.");
				base.enabled = false;
				break;
			}
			renderer.enabled = this.startOn;
		}
		this.hasAudioSource = (this.audioSource != null);
		this.assignedSlotBitIndex = (int)this.assignedSlot;
		if (this.oneShot)
		{
			this.toggleCooldownRange.x = this.toggleCooldownRange.x + this.animationTransitionDuration;
			this.toggleCooldownRange.y = this.toggleCooldownRange.y + this.animationTransitionDuration;
		}
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x0009E374 File Offset: 0x0009C574
	protected void LateUpdate()
	{
		if (this.ownerIsLocal)
		{
			this.toggleCooldownTimer -= Time.deltaTime;
			Transform transform = base.transform;
			if (Physics.OverlapSphereNonAlloc(transform.TransformPoint(this.triggerOffset), this.triggerRadius * transform.localScale.x, this.colliders, this.layerMask) > 0 && this.toggleCooldownTimer < 0f)
			{
				XRController componentInParent = this.colliders[0].GetComponentInParent<XRController>();
				if (componentInParent != null)
				{
					this.LocalToggle(componentInParent.controllerNode == XRNode.LeftHand, true);
				}
				this.toggleCooldownTimer = UnityEngine.Random.Range(this.toggleCooldownRange.x, this.toggleCooldownRange.y);
			}
		}
		else
		{
			bool flag = (this.ownerRig.WearablePackedStates & 1 << this.assignedSlotBitIndex) != 0;
			if (this.isOn != flag)
			{
				this.SharedSetState(flag, true);
			}
		}
		if (this.oneShot)
		{
			if (this.isOn)
			{
				this.progress = Mathf.MoveTowards(this.progress, 1f, Time.deltaTime / this.animationTransitionDuration);
				if (this.progress == 1f)
				{
					if (this.ownerIsLocal)
					{
						this.LocalToggle(false, false);
					}
					else
					{
						this.SharedSetState(false, false);
					}
					this.progress = 0f;
				}
			}
		}
		else
		{
			this.progress = Mathf.MoveTowards(this.progress, this.isOn ? 1f : 0f, Time.deltaTime / this.animationTransitionDuration);
		}
		Animator[] array = this.animators;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetFloat(ToggleableWearable.animParam_Progress, this.progress);
		}
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x0009E52C File Offset: 0x0009C72C
	private void LocalToggle(bool isLeftHand, bool playAudio)
	{
		this.ownerRig.WearablePackedStates ^= 1 << this.assignedSlotBitIndex;
		this.SharedSetState((this.ownerRig.WearablePackedStates & 1 << this.assignedSlotBitIndex) != 0, playAudio);
		if (playAudio && GorillaTagger.Instance)
		{
			GorillaTagger.Instance.StartVibration(isLeftHand, this.isOn ? this.turnOnVibrationDuration : this.turnOffVibrationDuration, this.isOn ? this.turnOnVibrationStrength : this.turnOffVibrationStrength);
		}
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x0009E5C0 File Offset: 0x0009C7C0
	private void SharedSetState(bool state, bool playAudio)
	{
		this.isOn = state;
		Renderer[] array = this.renderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = this.isOn;
		}
		if (!playAudio || !this.hasAudioSource)
		{
			return;
		}
		AudioClip audioClip = this.isOn ? this.toggleOnSound : this.toggleOffSound;
		if (audioClip == null)
		{
			return;
		}
		if (this.oneShot)
		{
			this.audioSource.clip = audioClip;
			this.audioSource.GTPlay();
			return;
		}
		this.audioSource.GTPlayOneShot(audioClip, 1f);
	}

	// Token: 0x04000EAC RID: 3756
	public Renderer[] renderers;

	// Token: 0x04000EAD RID: 3757
	public Animator[] animators;

	// Token: 0x04000EAE RID: 3758
	public float animationTransitionDuration = 1f;

	// Token: 0x04000EAF RID: 3759
	[Tooltip("Whether the wearable state is toggled on by default.")]
	public bool startOn;

	// Token: 0x04000EB0 RID: 3760
	[Tooltip("AudioSource to play toggle sounds.")]
	public AudioSource audioSource;

	// Token: 0x04000EB1 RID: 3761
	[Tooltip("Sound to play when toggled on.")]
	public AudioClip toggleOnSound;

	// Token: 0x04000EB2 RID: 3762
	[Tooltip("Sound to play when toggled off.")]
	public AudioClip toggleOffSound;

	// Token: 0x04000EB3 RID: 3763
	[Tooltip("Layer to check for trigger sphere collisions.")]
	public LayerMask layerMask;

	// Token: 0x04000EB4 RID: 3764
	[Tooltip("Radius of the trigger sphere.")]
	public float triggerRadius = 0.2f;

	// Token: 0x04000EB5 RID: 3765
	[Tooltip("Position in local space to move the trigger sphere.")]
	public Vector3 triggerOffset = Vector3.zero;

	// Token: 0x04000EB6 RID: 3766
	[Tooltip("This is to determine what bit to change in VRRig.WearablesPackedStates.")]
	public VRRig.WearablePackedStateSlots assignedSlot;

	// Token: 0x04000EB7 RID: 3767
	[Header("Vibration")]
	public float turnOnVibrationDuration = 0.05f;

	// Token: 0x04000EB8 RID: 3768
	public float turnOnVibrationStrength = 0.2f;

	// Token: 0x04000EB9 RID: 3769
	public float turnOffVibrationDuration = 0.05f;

	// Token: 0x04000EBA RID: 3770
	public float turnOffVibrationStrength = 0.2f;

	// Token: 0x04000EBB RID: 3771
	private VRRig ownerRig;

	// Token: 0x04000EBC RID: 3772
	private bool ownerIsLocal;

	// Token: 0x04000EBD RID: 3773
	private bool isOn;

	// Token: 0x04000EBE RID: 3774
	[SerializeField]
	private Vector2 toggleCooldownRange = new Vector2(0.2f, 0.2f);

	// Token: 0x04000EBF RID: 3775
	private bool hasAudioSource;

	// Token: 0x04000EC0 RID: 3776
	private readonly Collider[] colliders = new Collider[1];

	// Token: 0x04000EC1 RID: 3777
	private int framesSinceCooldownAndExitingVolume;

	// Token: 0x04000EC2 RID: 3778
	private float toggleCooldownTimer;

	// Token: 0x04000EC3 RID: 3779
	private int assignedSlotBitIndex;

	// Token: 0x04000EC4 RID: 3780
	private static readonly int animParam_Progress = Animator.StringToHash("Progress");

	// Token: 0x04000EC5 RID: 3781
	private float progress;

	// Token: 0x04000EC6 RID: 3782
	[SerializeField]
	private bool oneShot;
}
