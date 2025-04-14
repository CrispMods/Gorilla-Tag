﻿using System;
using System.Collections.Generic;
using GorillaTagScripts;
using Photon.Pun;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

// Token: 0x020004D1 RID: 1233
public class BuilderPieceInteractor : MonoBehaviour
{
	// Token: 0x06001E07 RID: 7687 RVA: 0x000937EC File Offset: 0x000919EC
	private void Awake()
	{
		if (BuilderPieceInteractor.instance == null)
		{
			BuilderPieceInteractor.instance = this;
			BuilderPieceInteractor.hasInstance = true;
		}
		else if (BuilderPieceInteractor.instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		this.velocityEstimator = new List<GorillaVelocityEstimator>(2)
		{
			this.velocityEstimatorLeft,
			this.velocityEstimatorRight
		};
		this.laserSight = new List<BuilderLaserSight>(2)
		{
			this.laserSightLeft,
			this.laserSightRight
		};
		this.handState = new List<BuilderPieceInteractor.HandState>(2);
		this.heldPiece = new List<BuilderPiece>(2);
		this.potentialHeldPiece = new List<BuilderPiece>(2);
		this.potentialGrabbedOffsetDist = new List<float>(2);
		this.heldInitialRot = new List<Quaternion>(2);
		this.heldCurrentRot = new List<Quaternion>(2);
		this.heldInitialPos = new List<Vector3>(2);
		this.heldCurrentPos = new List<Vector3>(2);
		this.heldChainLength = new int[2];
		this.heldChainLength[0] = 0;
		this.heldChainLength[1] = 0;
		this.heldChainCost = new List<int[]>(2);
		for (int i = 0; i < 2; i++)
		{
			this.heldChainCost.Add(new int[3]);
		}
		BuilderPieceInteractor.allPotentialPlacements = new List<BuilderPotentialPlacement>[2];
		this.delayedPotentialPlacement = new List<BuilderPotentialPlacement>(2);
		this.delayedPlacementTime = new List<float>(2);
		this.prevPotentialPlacement = new List<BuilderPotentialPlacement>(2);
		this.glowBumps = new List<List<BuilderBumpGlow>>(2);
		for (int j = 0; j < 2; j++)
		{
			this.handState.Add(BuilderPieceInteractor.HandState.Empty);
			this.heldPiece.Add(null);
			this.potentialHeldPiece.Add(null);
			this.potentialGrabbedOffsetDist.Add(0f);
			this.heldInitialRot.Add(Quaternion.identity);
			this.heldCurrentRot.Add(Quaternion.identity);
			this.heldInitialPos.Add(Vector3.zero);
			this.heldCurrentPos.Add(Vector3.zero);
			this.delayedPotentialPlacement.Add(default(BuilderPotentialPlacement));
			this.delayedPlacementTime.Add(-1f);
			this.prevPotentialPlacement.Add(default(BuilderPotentialPlacement));
			BuilderPieceInteractor.allPotentialPlacements[j] = new List<BuilderPotentialPlacement>(128);
			this.glowBumps.Add(new List<BuilderBumpGlow>(1024));
		}
		this.checkPiecesInSphere = new NativeArray<OverlapSphereCommand>(2, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.checkPiecesInSphereResults = new NativeArray<ColliderHit>(2048, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.grabSphereCast = new NativeArray<SpherecastCommand>(2, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.grabSphereCastResults = new NativeArray<RaycastHit>(128, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		BuilderPieceInteractor.handGridPlaneData = new NativeList<BuilderGridPlaneData>[2];
		BuilderPieceInteractor.handPieceData = new NativeList<BuilderPieceData>[2];
		BuilderPieceInteractor.localAttachableGridPlaneData = new NativeList<BuilderGridPlaneData>[2];
		BuilderPieceInteractor.localAttachablePieceData = new NativeList<BuilderPieceData>[2];
		for (int k = 0; k < 2; k++)
		{
			BuilderPieceInteractor.handGridPlaneData[k] = new NativeList<BuilderGridPlaneData>(512, Allocator.Persistent);
			BuilderPieceInteractor.handPieceData[k] = new NativeList<BuilderPieceData>(512, Allocator.Persistent);
			BuilderPieceInteractor.localAttachableGridPlaneData[k] = new NativeList<BuilderGridPlaneData>(10240, Allocator.Persistent);
			BuilderPieceInteractor.localAttachablePieceData[k] = new NativeList<BuilderPieceData>(2560, Allocator.Persistent);
		}
	}

	// Token: 0x06001E08 RID: 7688 RVA: 0x000023F4 File Offset: 0x000005F4
	public void PreInteract()
	{
	}

	// Token: 0x06001E09 RID: 7689 RVA: 0x00093B24 File Offset: 0x00091D24
	public void StartFindNearbyPieces()
	{
		if (BuilderTable.instance == null || !BuilderTable.instance.IsInBuilderZone())
		{
			return;
		}
		VRRig offlineVRRig = GorillaTagger.Instance.offlineVRRig;
		QueryParameters queryParameters = new QueryParameters
		{
			layerMask = BuilderTable.instance.allPiecesMask
		};
		this.checkPiecesInSphere[0] = new OverlapSphereCommand(offlineVRRig.leftHand.overrideTarget.position, (this.handState[0] == BuilderPieceInteractor.HandState.Empty) ? 0.0375f : 1f, queryParameters);
		this.checkPiecesInSphere[1] = new OverlapSphereCommand(offlineVRRig.rightHand.overrideTarget.position, (this.handState[1] == BuilderPieceInteractor.HandState.Empty) ? 0.0375f : 1f, queryParameters);
		this.checkNearbyPiecesHandle = OverlapSphereCommand.ScheduleBatch(this.checkPiecesInSphere, this.checkPiecesInSphereResults, 1, 1024, default(JobHandle));
		for (int i = 0; i < 64; i++)
		{
			this.grabSphereCastResults[i] = this.emptyRaycastHit;
		}
		this.grabSphereCast[0] = new SpherecastCommand(offlineVRRig.leftHand.overrideTarget.position, 0.0375f, offlineVRRig.leftHand.overrideTarget.rotation * Vector3.right, queryParameters, 0.15f);
		this.grabSphereCast[1] = new SpherecastCommand(offlineVRRig.rightHand.overrideTarget.position, 0.0375f, offlineVRRig.rightHand.overrideTarget.rotation * -Vector3.right, queryParameters, 0.15f);
		this.findPiecesToGrab = SpherecastCommand.ScheduleBatch(this.grabSphereCast, this.grabSphereCastResults, 1, 64, default(JobHandle));
		JobHandle.ScheduleBatchedJobs();
	}

	// Token: 0x06001E0A RID: 7690 RVA: 0x00093CF4 File Offset: 0x00091EF4
	private void CalcLocalGridPlanes()
	{
		VRRig offlineVRRig = GorillaTagger.Instance.offlineVRRig;
		this.checkNearbyPiecesHandle.Complete();
		BuilderTable builderTable = BuilderTable.instance;
		for (int i = 0; i < 2; i++)
		{
			if (this.handState[i] == BuilderPieceInteractor.HandState.Grabbed)
			{
				BuilderPieceInteractor.localAttachableGridPlaneData[i].Clear();
				BuilderPieceInteractor.localAttachablePieceData[i].Clear();
				BuilderPieceInteractor.tempPieceSet.Clear();
				if (builderTable.IsInBuilderZone())
				{
					for (int j = 0; j < 1024; j++)
					{
						int index = i * 1024 + j;
						if (this.checkPiecesInSphereResults[index].instanceID == 0)
						{
							break;
						}
						BuilderPiece pieceInHand = this.heldPiece[i];
						BuilderPiece builderPieceFromCollider = BuilderPiece.GetBuilderPieceFromCollider(this.checkPiecesInSphereResults[index].collider);
						if (builderPieceFromCollider != null && !BuilderPieceInteractor.tempPieceSet.Contains(builderPieceFromCollider))
						{
							BuilderPieceInteractor.tempPieceSet.Add(builderPieceFromCollider);
							if (builderTable.CanPiecesPotentiallySnap(pieceInHand, builderPieceFromCollider))
							{
								int length = BuilderPieceInteractor.localAttachablePieceData[i].Length;
								NativeList<BuilderPieceData>[] array = BuilderPieceInteractor.localAttachablePieceData;
								int num = i;
								BuilderPieceData builderPieceData = new BuilderPieceData(builderPieceFromCollider);
								array[num].Add(builderPieceData);
								for (int k = 0; k < builderPieceFromCollider.gridPlanes.Count; k++)
								{
									NativeList<BuilderGridPlaneData>[] array2 = BuilderPieceInteractor.localAttachableGridPlaneData;
									int num2 = i;
									BuilderGridPlaneData builderGridPlaneData = new BuilderGridPlaneData(builderPieceFromCollider.gridPlanes[k], length);
									array2[num2].Add(builderGridPlaneData);
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06001E0B RID: 7691 RVA: 0x00093E84 File Offset: 0x00092084
	private void OnDestroy()
	{
		if (BuilderPieceInteractor.instance == this)
		{
			BuilderPieceInteractor.hasInstance = false;
			BuilderPieceInteractor.instance = null;
		}
		if (this.checkPiecesInSphere.IsCreated)
		{
			this.checkPiecesInSphere.Dispose();
		}
		if (this.checkPiecesInSphereResults.IsCreated)
		{
			this.checkPiecesInSphereResults.Dispose();
		}
		if (this.grabSphereCast.IsCreated)
		{
			this.grabSphereCast.Dispose();
		}
		if (this.grabSphereCastResults.IsCreated)
		{
			this.grabSphereCastResults.Dispose();
		}
		for (int i = 0; i < 2; i++)
		{
			if (BuilderPieceInteractor.handGridPlaneData[i].IsCreated)
			{
				BuilderPieceInteractor.handGridPlaneData[i].Dispose();
			}
			if (BuilderPieceInteractor.handPieceData[i].IsCreated)
			{
				BuilderPieceInteractor.handPieceData[i].Dispose();
			}
			if (BuilderPieceInteractor.localAttachableGridPlaneData[i].IsCreated)
			{
				BuilderPieceInteractor.localAttachableGridPlaneData[i].Dispose();
			}
			if (BuilderPieceInteractor.localAttachablePieceData[i].IsCreated)
			{
				BuilderPieceInteractor.localAttachablePieceData[i].Dispose();
			}
		}
	}

	// Token: 0x06001E0C RID: 7692 RVA: 0x00093FA8 File Offset: 0x000921A8
	public bool BlockSnowballCreation()
	{
		return !(BuilderTable.instance == null) && !(GorillaTagger.Instance == null) && (BuilderTable.instance.IsInBuilderZone() && GorillaTagger.Instance.offlineVRRig.scaleFactor >= 0.99f);
	}

	// Token: 0x06001E0D RID: 7693 RVA: 0x00093FF8 File Offset: 0x000921F8
	public void OnLateUpdate()
	{
		if (BuilderTable.instance == null)
		{
			return;
		}
		if (!BuilderTable.instance.IsInBuilderZone())
		{
			return;
		}
		this.CalcLocalGridPlanes();
		VRRig offlineVRRig = GorillaTagger.Instance.offlineVRRig;
		BodyDockPositions myBodyDockPositions = offlineVRRig.myBodyDockPositions;
		this.findPiecesToGrab.Complete();
		this.UpdateHandState(BuilderPieceInteractor.HandType.Left, offlineVRRig.leftHand.overrideTarget, Vector3.right, myBodyDockPositions.leftHandTransform, this.equipmentInteractor.isLeftGrabbing, this.equipmentInteractor.wasLeftGrabPressed, this.equipmentInteractor.leftHandHeldEquipment, this.equipmentInteractor.disableLeftGrab);
		this.UpdateHandState(BuilderPieceInteractor.HandType.Right, offlineVRRig.rightHand.overrideTarget, -Vector3.right, myBodyDockPositions.rightHandTransform, this.equipmentInteractor.isRightGrabbing, this.equipmentInteractor.wasRightGrabPressed, this.equipmentInteractor.rightHandHeldEquipment, this.equipmentInteractor.disableRightGrab);
		this.UpdatePieceDisables();
		if (offlineVRRig != null)
		{
			bool flag = offlineVRRig.scaleFactor < 1f;
			if (flag && !this.isRigSmall)
			{
				if (offlineVRRig.builderArmShelfLeft != null)
				{
					offlineVRRig.builderArmShelfLeft.DropAttachedPieces();
					if (offlineVRRig.builderArmShelfLeft.piece != null)
					{
						foreach (Collider collider in offlineVRRig.builderArmShelfLeft.piece.colliders)
						{
							collider.enabled = false;
						}
					}
				}
				if (!(offlineVRRig.builderArmShelfRight != null))
				{
					goto IL_2B2;
				}
				offlineVRRig.builderArmShelfRight.DropAttachedPieces();
				if (!(offlineVRRig.builderArmShelfRight.piece != null))
				{
					goto IL_2B2;
				}
				using (List<Collider>.Enumerator enumerator = offlineVRRig.builderArmShelfRight.piece.colliders.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Collider collider2 = enumerator.Current;
						collider2.enabled = false;
					}
					goto IL_2B2;
				}
			}
			if (!flag && this.isRigSmall)
			{
				if (offlineVRRig.builderArmShelfLeft != null && offlineVRRig.builderArmShelfLeft.piece != null)
				{
					foreach (Collider collider3 in offlineVRRig.builderArmShelfLeft.piece.colliders)
					{
						collider3.enabled = true;
					}
				}
				if (offlineVRRig.builderArmShelfRight != null && offlineVRRig.builderArmShelfRight.piece != null)
				{
					foreach (Collider collider4 in offlineVRRig.builderArmShelfRight.piece.colliders)
					{
						collider4.enabled = true;
					}
				}
			}
			IL_2B2:
			this.isRigSmall = flag;
		}
	}

	// Token: 0x06001E0E RID: 7694 RVA: 0x000942F4 File Offset: 0x000924F4
	private void SetHandState(int handIndex, BuilderPieceInteractor.HandState newState)
	{
		if (this.handState[handIndex] == BuilderPieceInteractor.HandState.Empty && this.potentialHeldPiece[handIndex] != null)
		{
			this.potentialHeldPiece[handIndex].PotentialGrab(false);
			this.potentialHeldPiece[handIndex] = null;
		}
		this.handState[handIndex] = newState;
		switch (this.handState[handIndex])
		{
		case BuilderPieceInteractor.HandState.Empty:
			this.heldChainLength[handIndex] = 0;
			for (int i = 0; i < this.heldChainCost[handIndex].Length; i++)
			{
				this.heldChainCost[handIndex][i] = 0;
			}
			break;
		case BuilderPieceInteractor.HandState.Grabbed:
			this.heldChainLength[handIndex] = this.heldPiece[handIndex].GetChildCount() + 1;
			this.heldPiece[handIndex].GetChainCost(this.heldChainCost[handIndex]);
			return;
		case BuilderPieceInteractor.HandState.PotentialGrabbed:
			this.heldChainLength[handIndex] = 0;
			for (int j = 0; j < this.heldChainCost[handIndex].Length; j++)
			{
				this.heldChainCost[handIndex][j] = 0;
			}
			return;
		case BuilderPieceInteractor.HandState.WaitForGrabbed:
			break;
		default:
			return;
		}
	}

	// Token: 0x06001E0F RID: 7695 RVA: 0x00094410 File Offset: 0x00092610
	public void OnCountChangedForRoot(BuilderPiece piece)
	{
		if (piece == null)
		{
			return;
		}
		if (this.heldPiece[0] != null && this.heldPiece[0].Equals(piece))
		{
			this.heldChainLength[0] = this.heldPiece[0].GetChainCostAndCount(this.heldChainCost[0]);
			return;
		}
		if (this.heldPiece[1] != null && this.heldPiece[1].Equals(piece))
		{
			this.heldChainLength[1] = this.heldPiece[1].GetChainCostAndCount(this.heldChainCost[1]);
		}
	}

	// Token: 0x06001E10 RID: 7696 RVA: 0x000944C4 File Offset: 0x000926C4
	private void UpdateHandState(BuilderPieceInteractor.HandType handType, Transform handTransform, Vector3 palmForwardLocal, Transform handAttachPoint, bool isGrabbing, bool wasGrabPressed, IHoldableObject heldEquipment, bool grabDisabled)
	{
		int index = (int)((handType + 1) % (BuilderPieceInteractor.HandType)2);
		bool flag = GorillaTagger.Instance.offlineVRRig.scaleFactor < 1f;
		bool flag2 = isGrabbing && !wasGrabPressed;
		bool flag3 = this.heldPiece[(int)handType] != null && (!isGrabbing || flag);
		bool flag4 = heldEquipment != null;
		bool flag5 = this.heldPiece[(int)handType] != null;
		bool flag6 = !flag4 && !flag5 && !grabDisabled && !flag && BuilderTable.instance.IsInBuilderZone();
		BuilderPiece builderPiece = null;
		Vector3 position = handTransform.position;
		handTransform.rotation * palmForwardLocal;
		Vector3 zero = Vector3.zero;
		switch (this.handState[(int)handType])
		{
		case BuilderPieceInteractor.HandState.Empty:
			if (!flag)
			{
				float num = float.MaxValue;
				for (int i = 0; i < 1024; i++)
				{
					int index2 = (int)(handType * (BuilderPieceInteractor.HandType)1024 + i);
					ColliderHit colliderHit = this.checkPiecesInSphereResults[index2];
					if (colliderHit.instanceID == 0)
					{
						break;
					}
					BuilderPiece builderPieceFromCollider = BuilderPiece.GetBuilderPieceFromCollider(colliderHit.collider);
					if (builderPieceFromCollider != null && !builderPieceFromCollider.isBuiltIntoTable)
					{
						float num2 = Vector3.SqrMagnitude(colliderHit.collider.transform.position - handTransform.position);
						if ((builderPiece == null || num2 < num) && builderPieceFromCollider.CanPlayerGrabPiece(PhotonNetwork.LocalPlayer.ActorNumber, builderPieceFromCollider.transform.position))
						{
							builderPiece = builderPieceFromCollider;
							num = num2;
						}
					}
				}
				if (builderPiece == null)
				{
					for (int j = 0; j < 64; j++)
					{
						int index3 = (int)(handType * (BuilderPieceInteractor.HandType)64 + j);
						RaycastHit raycastHit = this.grabSphereCastResults[index3];
						if (raycastHit.colliderInstanceID == 0)
						{
							break;
						}
						BuilderPiece builderPieceFromCollider2 = BuilderPiece.GetBuilderPieceFromCollider(raycastHit.collider);
						if (builderPieceFromCollider2 != null && !builderPieceFromCollider2.isBuiltIntoTable && (builderPiece == null || raycastHit.distance < num) && builderPieceFromCollider2.CanPlayerGrabPiece(PhotonNetwork.LocalPlayer.ActorNumber, builderPieceFromCollider2.transform.position))
						{
							builderPiece = builderPieceFromCollider2;
							num = raycastHit.distance;
						}
					}
				}
			}
			if (this.potentialHeldPiece[(int)handType] != builderPiece)
			{
				if (this.potentialHeldPiece[(int)handType] != null)
				{
					this.potentialHeldPiece[(int)handType].PotentialGrab(false);
				}
				this.potentialHeldPiece[(int)handType] = builderPiece;
				if (this.potentialHeldPiece[(int)handType] != null)
				{
					this.potentialHeldPiece[(int)handType].PotentialGrab(true);
				}
			}
			if (flag6 && flag2 && builderPiece != null)
			{
				Vector3 vector;
				Quaternion quaternion;
				this.CalcPieceLocalPosAndRot(builderPiece.transform.position, builderPiece.transform.rotation, handAttachPoint, out vector, out quaternion);
				if (BuilderPiece.IsDroppedState(builderPiece.state) || this.heldPiece[index] == builderPiece)
				{
					builderPiece.PlayGrabbedFx();
					BuilderTable.instance.RequestGrabPiece(builderPiece, handType == BuilderPieceInteractor.HandType.Left, vector, quaternion);
					return;
				}
				builderPiece.PlayGrabbedFx();
				this.SetHandState((int)handType, BuilderPieceInteractor.HandState.PotentialGrabbed);
				this.potentialGrabbedOffsetDist[(int)handType] = 0f;
				this.heldPiece[(int)handType] = builderPiece;
				this.heldInitialRot[(int)handType] = quaternion;
				this.heldCurrentRot[(int)handType] = quaternion;
				this.heldInitialPos[(int)handType] = vector;
				this.heldCurrentPos[(int)handType] = vector;
				return;
			}
			break;
		case BuilderPieceInteractor.HandState.Grabbed:
		{
			if (flag3)
			{
				Vector3 vector2 = this.velocityEstimator[(int)handType].linearVelocity;
				if (flag)
				{
					Vector3 point = BuilderTable.instance.roomCenter.position - this.velocityEstimator[(int)handType].handPos;
					point.Normalize();
					Vector3 a = Quaternion.Euler(0f, 180f, 0f) * point;
					vector2 = BuilderTable.DROP_ZONE_REPEL * a;
				}
				else if (this.prevPotentialPlacement[(int)handType].attachPiece == this.heldPiece[(int)handType] && this.prevPotentialPlacement[(int)handType].parentPiece != null)
				{
					Vector3 a2 = this.prevPotentialPlacement[(int)handType].parentPiece.gridPlanes[this.prevPotentialPlacement[(int)handType].parentAttachIndex].transform.TransformDirection(this.prevPotentialPlacement[(int)handType].attachPlaneNormal);
					vector2 += a2 * 1f;
				}
				BuilderTable.instance.TryDropPiece(handType == BuilderPieceInteractor.HandType.Left, this.heldPiece[(int)handType], vector2, this.velocityEstimator[(int)handType].angularVelocity);
				return;
			}
			BuilderPiece builderPiece2 = this.heldPiece[(int)handType];
			if (builderPiece2 != null)
			{
				builderPiece2.transform.localRotation = this.heldInitialRot[(int)handType];
				builderPiece2.transform.localPosition = this.heldInitialPos[(int)handType];
				Quaternion quaternion2 = this.heldCurrentRot[(int)handType];
				Vector3 vector3 = this.heldCurrentPos[(int)handType];
				NativeList<BuilderGridPlaneData>[] array = BuilderPieceInteractor.localAttachableGridPlaneData;
				BuilderPieceInteractor.handPieceData[(int)handType].Clear();
				BuilderPieceInteractor.handGridPlaneData[(int)handType].Clear();
				BuilderTableJobs.BuildTestPieceListForJob(builderPiece2, BuilderPieceInteractor.handPieceData[(int)handType], BuilderPieceInteractor.handGridPlaneData[(int)handType]);
				BuilderPieceInteractor.allPotentialPlacements[(int)handType].Clear();
				BuilderPotentialPlacement builderPotentialPlacement;
				bool flag7 = BuilderTable.instance.TryPlacePieceOnTableNoDropJobs(BuilderPieceInteractor.handGridPlaneData[(int)handType], BuilderPieceInteractor.handPieceData[(int)handType], BuilderPieceInteractor.localAttachableGridPlaneData[(int)handType], BuilderPieceInteractor.localAttachablePieceData[(int)handType], out builderPotentialPlacement, BuilderPieceInteractor.allPotentialPlacements[(int)handType]);
				if (flag7)
				{
					BuilderPiece.State state = builderPotentialPlacement.attachPiece.state;
					BuilderPiece.State state2 = (builderPotentialPlacement.parentPiece == null) ? BuilderPiece.State.None : builderPotentialPlacement.parentPiece.state;
					bool flag8 = state == BuilderPiece.State.Grabbed || state == BuilderPiece.State.GrabbedLocal;
					bool flag9 = state2 == BuilderPiece.State.Grabbed || state2 == BuilderPiece.State.GrabbedLocal;
					bool flag10 = flag8 && flag9;
					int index4 = (int)((handType + 1) % (BuilderPieceInteractor.HandType)2);
					BuilderPieceInteractor.HandState handState = this.handState[index4];
					if (flag10 && builderPotentialPlacement.attachPiece.gridPlanes[builderPotentialPlacement.attachIndex].male)
					{
						flag7 = false;
					}
					else if (flag10 && handState == BuilderPieceInteractor.HandState.WaitingForSnap)
					{
						flag7 = false;
					}
				}
				if (flag7)
				{
					for (int k = 0; k < BuilderPieceInteractor.allPotentialPlacements[(int)handType].Count; k++)
					{
						BuilderPotentialPlacement builderPotentialPlacement2 = BuilderPieceInteractor.allPotentialPlacements[(int)handType][k];
						BuilderAttachGridPlane builderAttachGridPlane = builderPotentialPlacement2.attachPiece.gridPlanes[builderPotentialPlacement2.attachIndex];
						BuilderAttachGridPlane builderAttachGridPlane2 = (builderPotentialPlacement2.parentPiece == null) ? null : builderPotentialPlacement2.parentPiece.gridPlanes[builderPotentialPlacement2.parentAttachIndex];
						bool flag11 = builderAttachGridPlane.IsConnected(builderPotentialPlacement2.attachBounds);
						if (!flag11)
						{
							flag11 = builderAttachGridPlane2.IsConnected(builderPotentialPlacement2.parentAttachBounds);
						}
						if (flag11)
						{
							flag7 = false;
							break;
						}
					}
				}
				if (flag7)
				{
					Vector3 position2 = builderPotentialPlacement.localPosition;
					Quaternion rhs = builderPotentialPlacement.localRotation;
					Vector3 vector4 = builderPotentialPlacement.attachPlaneNormal;
					if (builderPotentialPlacement.parentPiece != null)
					{
						BuilderAttachGridPlane builderAttachGridPlane3 = builderPotentialPlacement.parentPiece.gridPlanes[builderPotentialPlacement.parentAttachIndex];
						position2 = builderAttachGridPlane3.transform.TransformPoint(builderPotentialPlacement.localPosition);
						rhs = builderAttachGridPlane3.transform.rotation * builderPotentialPlacement.localRotation;
						vector4 = builderAttachGridPlane3.transform.TransformDirection(builderPotentialPlacement.attachPlaneNormal);
					}
					Vector3 a3 = handAttachPoint.transform.InverseTransformPoint(position2);
					Quaternion a4 = Quaternion.Inverse(handAttachPoint.transform.rotation) * rhs;
					float attachDistance = builderPotentialPlacement.attachDistance;
					float num3 = Mathf.InverseLerp(BuilderTable.instance.pushAndEaseParams.snapDelayOffsetDist, BuilderTable.instance.pushAndEaseParams.maxOffsetY, attachDistance);
					num3 = Mathf.Clamp(num3, 0f, 1f);
					bool flag12 = builderPotentialPlacement.attachPiece == builderPiece2;
					bool flag13 = builderPotentialPlacement.attachPiece == builderPiece2;
					if (flag12)
					{
						Quaternion b = this.heldInitialRot[(int)handType];
						Quaternion b2 = Quaternion.Slerp(a4, b, num3);
						quaternion2 = Quaternion.Slerp(quaternion2, b2, 0.1f);
					}
					if (flag13)
					{
						Vector3 vector5 = this.heldInitialPos[(int)handType];
						Vector3 vector6 = handAttachPoint.transform.InverseTransformDirection(vector4);
						Vector3 vector7 = a3 + vector6 * BuilderTable.instance.pushAndEaseParams.snapDelayOffsetDist - vector5;
						float num4 = Vector3.Dot(vector7, vector6);
						num4 = Mathf.Min(0f, num4);
						Vector3 b3 = vector6 * num4;
						Vector3 a5 = vector7 - b3;
						Vector3 b4 = vector5 + Vector3.Lerp(a5, Vector3.zero, num3);
						vector3 = Vector3.Lerp(vector3, b4, 0.5f);
					}
					this.heldCurrentRot[(int)handType] = quaternion2;
					this.heldCurrentPos[(int)handType] = vector3;
					builderPiece2.transform.localRotation = quaternion2;
					builderPiece2.transform.localPosition = vector3;
					bool flag14 = Vector3.Dot(this.velocityEstimator[(int)handType].linearVelocity, vector4) > 0f;
					float snapAttachDistance = BuilderTable.instance.pushAndEaseParams.snapAttachDistance;
					if (builderPotentialPlacement.attachDistance < snapAttachDistance && !flag14 && BuilderPiece.CanPlayerAttachPieceToPiece(PhotonNetwork.LocalPlayer.ActorNumber, builderPiece2, builderPotentialPlacement.parentPiece))
					{
						GorillaTagger.Instance.StartVibration(handType == BuilderPieceInteractor.HandType.Left, GorillaTagger.Instance.tapHapticStrength, BuilderTable.instance.pushAndEaseParams.snapDelayTime * 2f);
						if (((builderPotentialPlacement.parentPiece == null) ? BuilderPiece.State.None : builderPotentialPlacement.parentPiece.state) == BuilderPiece.State.GrabbedLocal)
						{
							GorillaTagger.Instance.StartVibration(handType > BuilderPieceInteractor.HandType.Left, GorillaTagger.Instance.tapHapticStrength, BuilderTable.instance.pushAndEaseParams.snapDelayTime * 2f);
						}
						this.delayedPotentialPlacement[(int)handType] = builderPotentialPlacement;
						this.delayedPlacementTime[(int)handType] = 0f;
						this.SetHandState((int)handType, BuilderPieceInteractor.HandState.WaitingForSnap);
					}
					else
					{
						float num5 = BuilderTable.instance.gridSize * 0.5f * (BuilderTable.instance.gridSize * 0.5f);
						if (this.prevPotentialPlacement[(int)handType].attachPiece != builderPotentialPlacement.attachPiece || this.prevPotentialPlacement[(int)handType].parentPiece != builderPotentialPlacement.parentPiece || this.prevPotentialPlacement[(int)handType].attachIndex != builderPotentialPlacement.attachIndex || this.prevPotentialPlacement[(int)handType].parentAttachIndex != builderPotentialPlacement.parentAttachIndex || Vector3.SqrMagnitude(this.prevPotentialPlacement[(int)handType].localPosition - builderPotentialPlacement.localPosition) > num5)
						{
							GorillaTagger.Instance.StartVibration(handType == BuilderPieceInteractor.HandType.Left, GorillaTagger.Instance.tapHapticStrength * 0.15f, BuilderTable.instance.pushAndEaseParams.snapDelayTime);
							try
							{
								this.ClearGlowBumps((int)handType);
								this.AddGlowBumps((int)handType, BuilderPieceInteractor.allPotentialPlacements[(int)handType]);
							}
							catch (Exception ex)
							{
								Debug.LogErrorFormat("Error adding glow bumps {0}", new object[]
								{
									ex.ToString()
								});
							}
						}
					}
					this.UpdateGlowBumps((int)handType, 1f - num3);
					this.prevPotentialPlacement[(int)handType] = builderPotentialPlacement;
					return;
				}
				this.ClearGlowBumps((int)handType);
				Quaternion b5 = this.heldInitialRot[(int)handType];
				quaternion2 = Quaternion.Slerp(quaternion2, b5, 0.1f);
				Vector3 b6 = this.heldInitialPos[(int)handType];
				vector3 = Vector3.Lerp(vector3, b6, 0.1f);
				this.heldCurrentRot[(int)handType] = quaternion2;
				this.heldCurrentPos[(int)handType] = vector3;
				builderPiece2.transform.localRotation = quaternion2;
				builderPiece2.transform.localPosition = vector3;
				this.prevPotentialPlacement[(int)handType] = default(BuilderPotentialPlacement);
				return;
			}
			break;
		}
		case BuilderPieceInteractor.HandState.PotentialGrabbed:
		{
			if (flag3)
			{
				BuilderPiece builderPiece3 = this.heldPiece[(int)handType];
				this.ClearUnSnapOffset((int)handType, builderPiece3);
				this.RemovePieceFromHand(builderPiece3, (int)handType);
				this.heldPiece[(int)handType] = null;
				this.SetHandState((int)handType, BuilderPieceInteractor.HandState.Empty);
				return;
			}
			BuilderPiece builderPiece4 = this.heldPiece[(int)handType];
			Vector3 b7;
			Quaternion quaternion3;
			this.CalcPieceLocalPosAndRot(builderPiece4.transform.position, builderPiece4.transform.rotation, handAttachPoint, out b7, out quaternion3);
			if (BuilderPiece.IsDroppedState(builderPiece4.state))
			{
				BuilderTable.instance.RequestGrabPiece(builderPiece4, handType == BuilderPieceInteractor.HandType.Left, this.heldInitialPos[(int)handType], this.heldInitialRot[(int)handType]);
				return;
			}
			Vector3 vector8 = this.heldInitialPos[(int)handType] - b7;
			this.UpdatePullApartOffset((int)handType, builderPiece4, handAttachPoint.TransformVector(vector8));
			float num6 = BuilderTable.instance.pushAndEaseParams.unSnapDelayDist * BuilderTable.instance.pushAndEaseParams.unSnapDelayDist;
			if (vector8.sqrMagnitude > num6)
			{
				GorillaTagger.Instance.StartVibration(handType == BuilderPieceInteractor.HandType.Left, GorillaTagger.Instance.tapHapticStrength * 0.15f, BuilderTable.instance.pushAndEaseParams.unSnapDelayTime * 2f);
				if (((builderPiece4 == null) ? BuilderPiece.State.None : builderPiece4.state) == BuilderPiece.State.GrabbedLocal)
				{
					GorillaTagger.Instance.StartVibration(handType > BuilderPieceInteractor.HandType.Left, GorillaTagger.Instance.tapHapticStrength * 0.15f, BuilderTable.instance.pushAndEaseParams.unSnapDelayTime * 2f);
				}
				this.SetHandState((int)handType, BuilderPieceInteractor.HandState.WaitingForUnSnap);
				this.delayedPlacementTime[(int)handType] = 0f;
				return;
			}
			break;
		}
		case BuilderPieceInteractor.HandState.WaitForGrabbed:
			break;
		case BuilderPieceInteractor.HandState.WaitingForSnap:
		{
			BuilderPiece builderPiece5 = this.heldPiece[(int)handType];
			if (builderPiece5 != null)
			{
				builderPiece5.transform.localRotation = this.heldInitialRot[(int)handType];
				builderPiece5.transform.localPosition = this.heldInitialPos[(int)handType];
				Quaternion quaternion4 = this.heldCurrentRot[(int)handType];
				Vector3 vector9 = this.heldCurrentPos[(int)handType];
				if (this.delayedPlacementTime[(int)handType] >= 0f)
				{
					BuilderPotentialPlacement builderPotentialPlacement3 = this.delayedPotentialPlacement[(int)handType];
					if (this.delayedPlacementTime[(int)handType] > BuilderTable.instance.pushAndEaseParams.snapDelayTime)
					{
						BuilderAttachGridPlane builderAttachGridPlane4 = builderPotentialPlacement3.attachPiece.gridPlanes[builderPotentialPlacement3.attachIndex];
						BuilderAttachGridPlane builderAttachGridPlane5 = (builderPotentialPlacement3.parentPiece == null) ? null : builderPotentialPlacement3.parentPiece.gridPlanes[builderPotentialPlacement3.parentAttachIndex];
						bool flag15 = builderAttachGridPlane4.IsConnected(builderPotentialPlacement3.attachBounds);
						if (!flag15)
						{
							flag15 = builderAttachGridPlane5.IsConnected(builderPotentialPlacement3.parentAttachBounds);
						}
						if (flag15)
						{
							Debug.LogError("Snap Overlapping Why are we doing this!!??");
						}
						if (!BuilderPiece.CanPlayerAttachPieceToPiece(PhotonNetwork.LocalPlayer.ActorNumber, builderPiece5, builderPotentialPlacement3.parentPiece))
						{
							this.SetHandState((int)handType, BuilderPieceInteractor.HandState.Grabbed);
						}
						BuilderTable.instance.RequestPlacePiece(builderPiece5, builderPotentialPlacement3.attachPiece, builderPotentialPlacement3.bumpOffsetX, builderPotentialPlacement3.bumpOffsetZ, builderPotentialPlacement3.twist, builderPotentialPlacement3.parentPiece, builderPotentialPlacement3.attachIndex, builderPotentialPlacement3.parentAttachIndex);
						return;
					}
					this.delayedPlacementTime[(int)handType] = this.delayedPlacementTime[(int)handType] + Time.deltaTime;
					Transform parent = builderPiece5.transform.parent;
					Vector3 position3 = builderPotentialPlacement3.localPosition;
					Quaternion rhs2 = builderPotentialPlacement3.localRotation;
					Vector3 direction = builderPotentialPlacement3.attachPlaneNormal;
					if (builderPotentialPlacement3.parentPiece != null)
					{
						BuilderAttachGridPlane builderAttachGridPlane6 = builderPotentialPlacement3.parentPiece.gridPlanes[builderPotentialPlacement3.parentAttachIndex];
						position3 = builderAttachGridPlane6.transform.TransformPoint(builderPotentialPlacement3.localPosition);
						rhs2 = builderAttachGridPlane6.transform.rotation * builderPotentialPlacement3.localRotation;
						direction = builderAttachGridPlane6.transform.TransformDirection(builderPotentialPlacement3.attachPlaneNormal);
					}
					Vector3 a6 = parent.transform.InverseTransformPoint(position3);
					Quaternion quaternion5 = Quaternion.Inverse(parent.transform.rotation) * rhs2;
					bool flag16 = builderPotentialPlacement3.attachPiece == builderPiece5;
					bool flag17 = builderPotentialPlacement3.attachPiece == builderPiece5;
					if (flag16)
					{
						quaternion4 = quaternion5;
					}
					if (flag17)
					{
						Vector3 a7 = parent.transform.InverseTransformDirection(direction);
						vector9 = a6 + a7 * BuilderTable.instance.pushAndEaseParams.snapDelayOffsetDist;
					}
					this.heldCurrentRot[(int)handType] = quaternion4;
					this.heldCurrentPos[(int)handType] = vector9;
					builderPiece5.transform.localRotation = quaternion4;
					builderPiece5.transform.localPosition = vector9;
				}
			}
			break;
		}
		case BuilderPieceInteractor.HandState.WaitingForUnSnap:
		{
			BuilderPiece builderPiece6 = this.heldPiece[(int)handType];
			if (BuilderPiece.IsDroppedState(builderPiece6.state))
			{
				BuilderTable.instance.RequestGrabPiece(builderPiece6, handType == BuilderPieceInteractor.HandType.Left, this.heldInitialPos[(int)handType], this.heldInitialRot[(int)handType]);
				return;
			}
			if (this.delayedPlacementTime[(int)handType] <= BuilderTable.instance.pushAndEaseParams.unSnapDelayTime)
			{
				Vector3 b8;
				Quaternion quaternion6;
				this.CalcPieceLocalPosAndRot(builderPiece6.transform.position, builderPiece6.transform.rotation, handAttachPoint, out b8, out quaternion6);
				Vector3 vector10 = this.heldInitialPos[(int)handType] - b8;
				this.UpdatePullApartOffset((int)handType, builderPiece6, handAttachPoint.TransformVector(vector10));
				this.delayedPlacementTime[(int)handType] = this.delayedPlacementTime[(int)handType] + Time.deltaTime;
				return;
			}
			if (builderPiece6.GetChildCount() > this.maxHoldablePieceStackCount)
			{
				builderPiece6.PlayTooHeavyFx();
				this.ClearUnSnapOffset((int)handType, builderPiece6);
				this.RemovePieceFromHand(builderPiece6, (int)handType);
				return;
			}
			BuilderTable.instance.RequestGrabPiece(builderPiece6, handType == BuilderPieceInteractor.HandType.Left, this.heldInitialPos[(int)handType], this.heldInitialRot[(int)handType]);
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x06001E11 RID: 7697 RVA: 0x00095684 File Offset: 0x00093884
	private void ClearGlowBumps(int handIndex)
	{
		BuilderPool builderPool = BuilderTable.instance.builderPool;
		List<BuilderBumpGlow> list = this.glowBumps[handIndex];
		if (builderPool != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				builderPool.DestroyBumpGlow(list[i]);
			}
		}
		else
		{
			Debug.LogError("BuilderPieceInteractor could not find Builderpool");
		}
		list.Clear();
	}

	// Token: 0x06001E12 RID: 7698 RVA: 0x000956E4 File Offset: 0x000938E4
	private void AddGlowBumps(int handIndex, List<BuilderPotentialPlacement> allPotentialPlacements)
	{
		this.ClearGlowBumps(handIndex);
		if (allPotentialPlacements == null)
		{
			Debug.LogError("How is allPotentialPlacements null");
			return;
		}
		BuilderPool builderPool = BuilderTable.instance.builderPool;
		if (builderPool == null)
		{
			Debug.LogError("How is the pool null?");
			return;
		}
		float gridSize = BuilderTable.instance.gridSize;
		for (int i = 0; i < allPotentialPlacements.Count; i++)
		{
			BuilderPotentialPlacement builderPotentialPlacement = allPotentialPlacements[i];
			if (builderPotentialPlacement.parentPiece != null && builderPotentialPlacement.attachPiece != null && builderPotentialPlacement.attachPiece.gridPlanes != null && builderPotentialPlacement.parentPiece.gridPlanes != null)
			{
				BuilderAttachGridPlane builderAttachGridPlane = builderPotentialPlacement.parentPiece.gridPlanes[builderPotentialPlacement.parentAttachIndex];
				if (builderAttachGridPlane != null)
				{
					Vector2Int min = builderPotentialPlacement.parentAttachBounds.min;
					Vector2Int max = builderPotentialPlacement.parentAttachBounds.max;
					for (int j = min.x; j <= max.x; j++)
					{
						for (int k = min.y; k <= max.y; k++)
						{
							Vector3 gridPosition = builderAttachGridPlane.GetGridPosition(j, k, gridSize);
							BuilderBumpGlow builderBumpGlow = builderPool.CreateGlowBump();
							if (builderBumpGlow != null)
							{
								builderBumpGlow.transform.SetPositionAndRotation(gridPosition, builderAttachGridPlane.transform.rotation);
								builderBumpGlow.transform.SetParent(builderAttachGridPlane.transform, true);
								builderBumpGlow.transform.localScale = Vector3.one;
								builderBumpGlow.gameObject.SetActive(true);
								this.glowBumps[handIndex].Add(builderBumpGlow);
							}
						}
					}
				}
				BuilderAttachGridPlane builderAttachGridPlane2 = builderPotentialPlacement.attachPiece.gridPlanes[builderPotentialPlacement.attachIndex];
				if (builderAttachGridPlane2 != null)
				{
					Vector2Int min2 = builderPotentialPlacement.attachBounds.min;
					Vector2Int max2 = builderPotentialPlacement.attachBounds.max;
					for (int l = min2.x; l <= max2.x; l++)
					{
						for (int m = min2.y; m <= max2.y; m++)
						{
							Vector3 gridPosition2 = builderAttachGridPlane2.GetGridPosition(l, m, gridSize);
							BuilderBumpGlow builderBumpGlow2 = builderPool.CreateGlowBump();
							if (builderBumpGlow2 != null)
							{
								Quaternion rhs = Quaternion.Euler(180f, 0f, 0f);
								builderBumpGlow2.transform.SetPositionAndRotation(gridPosition2, builderAttachGridPlane2.transform.rotation * rhs);
								builderBumpGlow2.transform.SetParent(builderAttachGridPlane2.transform, true);
								builderBumpGlow2.transform.localScale = Vector3.one;
								builderBumpGlow2.gameObject.SetActive(true);
								this.glowBumps[handIndex].Add(builderBumpGlow2);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06001E13 RID: 7699 RVA: 0x000959B4 File Offset: 0x00093BB4
	private void UpdateGlowBumps(int handIndex, float intensity)
	{
		List<BuilderBumpGlow> list = this.glowBumps[handIndex];
		for (int i = 0; i < list.Count; i++)
		{
			list[i].SetIntensity(intensity);
		}
	}

	// Token: 0x06001E14 RID: 7700 RVA: 0x000959EC File Offset: 0x00093BEC
	private void UpdatePullApartOffset(int handIndex, BuilderPiece potentialGrabPiece, Vector3 pullApartDiff)
	{
		BuilderPiece parentPiece = potentialGrabPiece.parentPiece;
		BuilderAttachGridPlane builderAttachGridPlane = null;
		if (parentPiece != null)
		{
			builderAttachGridPlane = parentPiece.gridPlanes[potentialGrabPiece.parentAttachIndex];
		}
		Vector3 vector = Vector3.up;
		if (builderAttachGridPlane != null)
		{
			vector = builderAttachGridPlane.transform.TransformDirection(vector);
			if (!builderAttachGridPlane.male)
			{
				vector *= -1f;
			}
		}
		float num = Vector3.Dot(pullApartDiff, vector);
		num = Mathf.Max(num, 0f);
		float num2 = 0.0025f;
		float num3 = num / num2;
		num3 = 1f - 1f / (1f + num3);
		num = num3 * num2;
		Vector3 vector2 = vector * this.potentialGrabbedOffsetDist[handIndex];
		this.potentialGrabbedOffsetDist[handIndex] = num;
		Vector3 vector3 = vector * this.potentialGrabbedOffsetDist[handIndex];
		if (builderAttachGridPlane != null)
		{
			vector2 = builderAttachGridPlane.transform.InverseTransformVector(vector2);
			vector3 = builderAttachGridPlane.transform.InverseTransformVector(vector3);
		}
		Vector3 a = potentialGrabPiece.transform.localPosition - vector2;
		potentialGrabPiece.transform.localPosition = a + vector3;
	}

	// Token: 0x06001E15 RID: 7701 RVA: 0x00095B0E File Offset: 0x00093D0E
	private void ClearUnSnapOffset(int handIndex, BuilderPiece potentialGrabPiece)
	{
		this.UpdatePullApartOffset(handIndex, potentialGrabPiece, Vector3.zero);
	}

	// Token: 0x06001E16 RID: 7702 RVA: 0x00095B20 File Offset: 0x00093D20
	public void AddPieceToHeld(BuilderPiece piece, bool isLeft, Vector3 localPosition, Quaternion localRotation)
	{
		int num = isLeft ? 0 : 1;
		this.AddPieceToHand(piece, num, localPosition, localRotation);
		int num2 = (num + 1) % 2;
		if (this.heldPiece[num2] == piece)
		{
			this.RemovePieceFromHand(piece, num2);
		}
	}

	// Token: 0x06001E17 RID: 7703 RVA: 0x00095B64 File Offset: 0x00093D64
	public void RemovePieceFromHeld(BuilderPiece piece)
	{
		for (int i = 0; i < 2; i++)
		{
			if (this.heldPiece[i] == piece)
			{
				this.RemovePieceFromHand(piece, i);
			}
		}
	}

	// Token: 0x06001E18 RID: 7704 RVA: 0x00095B9C File Offset: 0x00093D9C
	private void AddPieceToHand(BuilderPiece piece, int handIndex, Vector3 localPosition, Quaternion localRotation)
	{
		this.heldPiece[handIndex] = piece;
		this.delayedPlacementTime[handIndex] = -1f;
		this.SetHandState(handIndex, BuilderPieceInteractor.HandState.Grabbed);
		this.heldInitialRot[handIndex] = localRotation;
		this.heldCurrentRot[handIndex] = localRotation;
		this.heldInitialPos[handIndex] = localPosition;
		this.heldCurrentPos[handIndex] = localPosition;
	}

	// Token: 0x06001E19 RID: 7705 RVA: 0x00095C05 File Offset: 0x00093E05
	private void RemovePieceFromHand(BuilderPiece piece, int handIndex)
	{
		this.heldPiece[handIndex] = null;
		this.delayedPlacementTime[handIndex] = -1f;
		this.SetHandState(handIndex, BuilderPieceInteractor.HandState.Empty);
		this.ClearGlowBumps(handIndex);
	}

	// Token: 0x06001E1A RID: 7706 RVA: 0x00095C34 File Offset: 0x00093E34
	public void RemovePiecesFromHands()
	{
		for (int i = 0; i < 2; i++)
		{
			this.heldPiece[i] = null;
			this.delayedPlacementTime[i] = -1f;
			this.SetHandState(i, BuilderPieceInteractor.HandState.Empty);
			this.ClearGlowBumps(i);
		}
	}

	// Token: 0x06001E1B RID: 7707 RVA: 0x00095C7C File Offset: 0x00093E7C
	private void CalcPieceLocalPosAndRot(Vector3 worldPosition, Quaternion worldRotation, Transform attachPoint, out Vector3 localPosition, out Quaternion localRotation)
	{
		Quaternion rotation = attachPoint.transform.rotation;
		Vector3 position = attachPoint.transform.position;
		localRotation = Quaternion.Inverse(rotation) * worldRotation;
		localPosition = Quaternion.Inverse(rotation) * (worldPosition - position);
	}

	// Token: 0x06001E1C RID: 7708 RVA: 0x00095CD1 File Offset: 0x00093ED1
	public void DisableCollisionsWithHands()
	{
		this.DisableCollisionsWithHand(true);
		this.DisableCollisionsWithHand(false);
	}

	// Token: 0x06001E1D RID: 7709 RVA: 0x00095CE4 File Offset: 0x00093EE4
	private void DisableCollisionsWithHand(bool leftHand)
	{
		VRRig offlineVRRig = GorillaTagger.Instance.offlineVRRig;
		Transform transform = leftHand ? offlineVRRig.leftHand.overrideTarget : offlineVRRig.rightHand.overrideTarget;
		List<GameObject> list = leftHand ? this.collisionDisabledPiecesLeft : this.collisionDisabledPiecesRight;
		int num = Physics.OverlapSphereNonAlloc(transform.position, 0.15f, BuilderPieceInteractor.tempDisableColliders, BuilderTable.instance.allPiecesMask);
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = BuilderPieceInteractor.tempDisableColliders[i].gameObject;
			BuilderPiece builderPieceFromCollider = BuilderPiece.GetBuilderPieceFromCollider(BuilderPieceInteractor.tempDisableColliders[i]);
			if (builderPieceFromCollider != null && builderPieceFromCollider.state == BuilderPiece.State.AttachedAndPlaced && !list.Contains(gameObject))
			{
				gameObject.layer = BuilderTable.heldLayer;
				list.Add(gameObject);
			}
		}
	}

	// Token: 0x06001E1E RID: 7710 RVA: 0x00095DA6 File Offset: 0x00093FA6
	public void UpdatePieceDisables()
	{
		this.UpdatePieceDisablesForHand(true);
		this.UpdatePieceDisablesForHand(false);
	}

	// Token: 0x06001E1F RID: 7711 RVA: 0x00095DB8 File Offset: 0x00093FB8
	public void UpdatePieceDisablesForHand(bool leftHand)
	{
		VRRig offlineVRRig = GorillaTagger.Instance.offlineVRRig;
		Transform transform = leftHand ? offlineVRRig.leftHand.overrideTarget : offlineVRRig.rightHand.overrideTarget;
		List<GameObject> list = leftHand ? this.collisionDisabledPiecesLeft : this.collisionDisabledPiecesRight;
		List<GameObject> list2 = (!leftHand) ? this.collisionDisabledPiecesLeft : this.collisionDisabledPiecesRight;
		Vector3 position = transform.position;
		float num = 0.040000003f;
		for (int i = 0; i < list.Count; i++)
		{
			GameObject gameObject = list[i];
			if (gameObject == null)
			{
				list.RemoveAt(i);
				i--;
			}
			else if ((gameObject.transform.position - position).sqrMagnitude > num)
			{
				BuilderPiece builderPieceFromTransform = BuilderPiece.GetBuilderPieceFromTransform(gameObject.transform);
				if (builderPieceFromTransform.state == BuilderPiece.State.AttachedAndPlaced && !list2.Contains(gameObject))
				{
					builderPieceFromTransform.SetColliderLayers<Collider>(builderPieceFromTransform.colliders, BuilderTable.placedLayer);
				}
				list.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x04002129 RID: 8489
	[OnEnterPlay_SetNull]
	public static volatile BuilderPieceInteractor instance;

	// Token: 0x0400212A RID: 8490
	[OnEnterPlay_Set(false)]
	public static bool hasInstance;

	// Token: 0x0400212B RID: 8491
	public EquipmentInteractor equipmentInteractor;

	// Token: 0x0400212C RID: 8492
	private const int NUM_HANDS = 2;

	// Token: 0x0400212D RID: 8493
	public GorillaVelocityEstimator velocityEstimatorLeft;

	// Token: 0x0400212E RID: 8494
	public GorillaVelocityEstimator velocityEstimatorRight;

	// Token: 0x0400212F RID: 8495
	public BuilderLaserSight laserSightLeft;

	// Token: 0x04002130 RID: 8496
	public BuilderLaserSight laserSightRight;

	// Token: 0x04002131 RID: 8497
	public int maxHoldablePieceStackCount = 50;

	// Token: 0x04002132 RID: 8498
	public List<GorillaVelocityEstimator> velocityEstimator;

	// Token: 0x04002133 RID: 8499
	public List<BuilderPieceInteractor.HandState> handState;

	// Token: 0x04002134 RID: 8500
	public List<BuilderPiece> heldPiece;

	// Token: 0x04002135 RID: 8501
	public List<BuilderPiece> potentialHeldPiece;

	// Token: 0x04002136 RID: 8502
	public List<float> potentialGrabbedOffsetDist;

	// Token: 0x04002137 RID: 8503
	public List<Quaternion> heldInitialRot;

	// Token: 0x04002138 RID: 8504
	public List<Quaternion> heldCurrentRot;

	// Token: 0x04002139 RID: 8505
	public List<Vector3> heldInitialPos;

	// Token: 0x0400213A RID: 8506
	public List<Vector3> heldCurrentPos;

	// Token: 0x0400213B RID: 8507
	public List<BuilderPotentialPlacement> delayedPotentialPlacement;

	// Token: 0x0400213C RID: 8508
	public List<float> delayedPlacementTime;

	// Token: 0x0400213D RID: 8509
	public List<BuilderPotentialPlacement> prevPotentialPlacement;

	// Token: 0x0400213E RID: 8510
	public List<BuilderLaserSight> laserSight;

	// Token: 0x0400213F RID: 8511
	public int[] heldChainLength;

	// Token: 0x04002140 RID: 8512
	public List<int[]> heldChainCost;

	// Token: 0x04002141 RID: 8513
	private static List<BuilderPotentialPlacement>[] allPotentialPlacements;

	// Token: 0x04002142 RID: 8514
	private static NativeList<BuilderGridPlaneData>[] handGridPlaneData;

	// Token: 0x04002143 RID: 8515
	private static NativeList<BuilderPieceData>[] handPieceData;

	// Token: 0x04002144 RID: 8516
	private static NativeList<BuilderGridPlaneData>[] localAttachableGridPlaneData;

	// Token: 0x04002145 RID: 8517
	private static NativeList<BuilderPieceData>[] localAttachablePieceData;

	// Token: 0x04002146 RID: 8518
	private JobHandle findNearbyJobHandle;

	// Token: 0x04002147 RID: 8519
	public List<GameObject> collisionDisabledPiecesLeft = new List<GameObject>();

	// Token: 0x04002148 RID: 8520
	public List<GameObject> collisionDisabledPiecesRight = new List<GameObject>();

	// Token: 0x04002149 RID: 8521
	public const int MAX_SPHERE_CHECK_RESULTS = 1024;

	// Token: 0x0400214A RID: 8522
	public NativeArray<OverlapSphereCommand> checkPiecesInSphere;

	// Token: 0x0400214B RID: 8523
	public NativeArray<ColliderHit> checkPiecesInSphereResults;

	// Token: 0x0400214C RID: 8524
	public JobHandle checkNearbyPiecesHandle;

	// Token: 0x0400214D RID: 8525
	public const float GRAB_CAST_RADIUS = 0.0375f;

	// Token: 0x0400214E RID: 8526
	public const int MAX_GRAB_CAST_RESULTS = 64;

	// Token: 0x0400214F RID: 8527
	public NativeArray<SpherecastCommand> grabSphereCast;

	// Token: 0x04002150 RID: 8528
	public NativeArray<RaycastHit> grabSphereCastResults;

	// Token: 0x04002151 RID: 8529
	public JobHandle findPiecesToGrab;

	// Token: 0x04002152 RID: 8530
	private RaycastHit emptyRaycastHit;

	// Token: 0x04002153 RID: 8531
	public BuilderBumpGlow glowBumpPrefab;

	// Token: 0x04002154 RID: 8532
	public List<List<BuilderBumpGlow>> glowBumps;

	// Token: 0x04002155 RID: 8533
	private const int MAX_GRID_PLANES = 8192;

	// Token: 0x04002156 RID: 8534
	private bool isRigSmall;

	// Token: 0x04002157 RID: 8535
	private static HashSet<BuilderPiece> tempPieceSet = new HashSet<BuilderPiece>(512);

	// Token: 0x04002158 RID: 8536
	private static RaycastHit[] tempHitResults = new RaycastHit[64];

	// Token: 0x04002159 RID: 8537
	private const float PIECE_DISTANCE_DISABLE = 0.15f;

	// Token: 0x0400215A RID: 8538
	private const float PIECE_DISTANCE_ENABLE = 0.2f;

	// Token: 0x0400215B RID: 8539
	private static Collider[] tempDisableColliders = new Collider[128];

	// Token: 0x020004D2 RID: 1234
	public enum HandType
	{
		// Token: 0x0400215D RID: 8541
		Invalid = -1,
		// Token: 0x0400215E RID: 8542
		Left,
		// Token: 0x0400215F RID: 8543
		Right
	}

	// Token: 0x020004D3 RID: 1235
	public enum HandState
	{
		// Token: 0x04002161 RID: 8545
		Invalid = -1,
		// Token: 0x04002162 RID: 8546
		Empty,
		// Token: 0x04002163 RID: 8547
		Grabbed,
		// Token: 0x04002164 RID: 8548
		PotentialGrabbed,
		// Token: 0x04002165 RID: 8549
		WaitForGrabbed,
		// Token: 0x04002166 RID: 8550
		WaitingForSnap,
		// Token: 0x04002167 RID: 8551
		WaitingForUnSnap
	}
}
