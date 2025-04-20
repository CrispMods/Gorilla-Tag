using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using BoingKit;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaNetworking;
using GorillaTagScripts.Builder;
using Ionic.Zlib;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using Unity.Collections;
using Unity.Jobs;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009B8 RID: 2488
	public class BuilderTable : MonoBehaviour
	{
		// Token: 0x06003D16 RID: 15638 RVA: 0x00157950 File Offset: 0x00155B50
		private void ExecuteAction(BuilderAction action)
		{
			BuilderPiece piece = this.GetPiece(action.pieceId);
			BuilderPiece piece2 = this.GetPiece(action.parentPieceId);
			int playerActorNumber = action.playerActorNumber;
			bool flag = PhotonNetwork.LocalPlayer.ActorNumber == action.playerActorNumber;
			switch (action.type)
			{
			case BuilderActionType.AttachToPlayer:
			{
				piece.ClearParentHeld();
				piece.ClearParentPiece(false);
				piece.transform.localScale = Vector3.one;
				RigContainer rigContainer;
				if (!VRRigCache.Instance.TryGetVrrig(NetworkSystem.Instance.GetPlayer(playerActorNumber), out rigContainer))
				{
					string.Format("Execute Builder Action {0} {1} {2} {3} {4}", new object[]
					{
						action.localCommandId,
						action.type,
						action.pieceId,
						action.playerActorNumber,
						action.isLeftHand
					});
					return;
				}
				BodyDockPositions myBodyDockPositions = rigContainer.Rig.myBodyDockPositions;
				Transform parentHeld = action.isLeftHand ? myBodyDockPositions.leftHandTransform : myBodyDockPositions.rightHandTransform;
				piece.SetParentHeld(parentHeld, playerActorNumber, action.isLeftHand);
				piece.transform.SetLocalPositionAndRotation(action.localPosition, action.localRotation);
				BuilderPiece.State newState = flag ? BuilderPiece.State.GrabbedLocal : BuilderPiece.State.Grabbed;
				piece.SetState(newState, false);
				if (!flag)
				{
					BuilderPieceInteractor.instance.RemovePieceFromHeld(piece);
				}
				if (flag)
				{
					BuilderPieceInteractor.instance.AddPieceToHeld(piece, action.isLeftHand, action.localPosition, action.localRotation);
					return;
				}
				break;
			}
			case BuilderActionType.DetachFromPlayer:
				if (flag)
				{
					BuilderPieceInteractor.instance.RemovePieceFromHeld(piece);
				}
				piece.ClearParentHeld();
				piece.ClearParentPiece(false);
				piece.transform.localScale = Vector3.one;
				return;
			case BuilderActionType.AttachToPiece:
			{
				piece.ClearParentHeld();
				piece.ClearParentPiece(false);
				piece.transform.localScale = Vector3.one;
				Quaternion identity = Quaternion.identity;
				Vector3 zero = Vector3.zero;
				Vector3 position = piece.transform.position;
				Quaternion rotation = piece.transform.rotation;
				if (piece2 != null)
				{
					piece.BumpTwistToPositionRotation(action.twist, action.bumpOffsetx, action.bumpOffsetz, action.attachIndex, piece2.gridPlanes[action.parentAttachIndex], out zero, out identity, out position, out rotation);
				}
				piece.transform.SetPositionAndRotation(position, rotation);
				BuilderPiece.State stateWhenPlaced;
				if (piece2 == null)
				{
					stateWhenPlaced = BuilderPiece.State.AttachedAndPlaced;
				}
				else if (piece2.isArmShelf || piece2.state == BuilderPiece.State.AttachedToArm)
				{
					stateWhenPlaced = BuilderPiece.State.AttachedToArm;
				}
				else if (piece2.isBuiltIntoTable || piece2.state == BuilderPiece.State.AttachedAndPlaced)
				{
					stateWhenPlaced = BuilderPiece.State.AttachedAndPlaced;
				}
				else if (piece2.state == BuilderPiece.State.Grabbed)
				{
					stateWhenPlaced = BuilderPiece.State.Grabbed;
				}
				else if (piece2.state == BuilderPiece.State.GrabbedLocal)
				{
					stateWhenPlaced = BuilderPiece.State.GrabbedLocal;
				}
				else
				{
					stateWhenPlaced = BuilderPiece.State.AttachedToDropped;
				}
				BuilderPiece rootPiece = piece2.GetRootPiece();
				this.gridPlaneData.Clear();
				this.checkGridPlaneData.Clear();
				this.allPotentialPlacements.Clear();
				BuilderTable.tempPieceSet.Clear();
				QueryParameters queryParameters = new QueryParameters
				{
					layerMask = BuilderTable.instance.allPiecesMask
				};
				OverlapSphereCommand value = new OverlapSphereCommand(position, 1f, queryParameters);
				this.nearbyPiecesCommands[0] = value;
				OverlapSphereCommand.ScheduleBatch(this.nearbyPiecesCommands, this.nearbyPiecesResults, 1, 1024, default(JobHandle)).Complete();
				int num = 0;
				while (num < 1024 && this.nearbyPiecesResults[num].instanceID != 0)
				{
					BuilderPiece pieceInHand = piece;
					BuilderPiece builderPieceFromCollider = BuilderPiece.GetBuilderPieceFromCollider(this.nearbyPiecesResults[num].collider);
					if (builderPieceFromCollider != null && !BuilderTable.tempPieceSet.Contains(builderPieceFromCollider))
					{
						BuilderTable.tempPieceSet.Add(builderPieceFromCollider);
						if (this.CanPiecesPotentiallyOverlap(pieceInHand, rootPiece, stateWhenPlaced, builderPieceFromCollider))
						{
							for (int i = 0; i < builderPieceFromCollider.gridPlanes.Count; i++)
							{
								BuilderGridPlaneData builderGridPlaneData = new BuilderGridPlaneData(builderPieceFromCollider.gridPlanes[i], -1);
								this.checkGridPlaneData.Add(builderGridPlaneData);
							}
						}
					}
					num++;
				}
				BuilderTableJobs.BuildTestPieceListForJob(piece, this.gridPlaneData);
				BuilderPotentialPlacement potentialPlacement = new BuilderPotentialPlacement
				{
					localPosition = zero,
					localRotation = identity,
					attachIndex = action.attachIndex,
					parentAttachIndex = action.parentAttachIndex,
					attachPiece = piece,
					parentPiece = piece2
				};
				BuilderTable.instance.CalcAllPotentialPlacements(this.gridPlaneData, this.checkGridPlaneData, potentialPlacement, this.allPotentialPlacements);
				piece.SetParentPiece(action.attachIndex, piece2, action.parentAttachIndex);
				for (int j = 0; j < this.allPotentialPlacements.Count; j++)
				{
					BuilderPotentialPlacement builderPotentialPlacement = this.allPotentialPlacements[j];
					BuilderAttachGridPlane builderAttachGridPlane = builderPotentialPlacement.attachPiece.gridPlanes[builderPotentialPlacement.attachIndex];
					BuilderAttachGridPlane builderAttachGridPlane2 = builderPotentialPlacement.parentPiece.gridPlanes[builderPotentialPlacement.parentAttachIndex];
					BuilderAttachGridPlane movingParentGrid = builderAttachGridPlane.GetMovingParentGrid();
					bool flag2 = movingParentGrid != null;
					BuilderAttachGridPlane movingParentGrid2 = builderAttachGridPlane2.GetMovingParentGrid();
					bool flag3 = movingParentGrid2 != null;
					if (flag2 == flag3 && (!flag2 || !(movingParentGrid != movingParentGrid2)))
					{
						SnapOverlap newOverlap = this.builderPool.CreateSnapOverlap(builderAttachGridPlane2, builderPotentialPlacement.attachBounds);
						builderAttachGridPlane.AddSnapOverlap(newOverlap);
						SnapOverlap newOverlap2 = this.builderPool.CreateSnapOverlap(builderAttachGridPlane, builderPotentialPlacement.parentAttachBounds);
						builderAttachGridPlane2.AddSnapOverlap(newOverlap2);
					}
				}
				piece.transform.SetLocalPositionAndRotation(zero, identity);
				if (piece2 != null && piece2.state == BuilderPiece.State.GrabbedLocal)
				{
					BuilderPiece rootPiece2 = piece2.GetRootPiece();
					BuilderPieceInteractor.instance.OnCountChangedForRoot(rootPiece2);
				}
				if (piece2 == null)
				{
					piece.SetActivateTimeStamp(action.timeStamp);
					piece.SetState(BuilderPiece.State.AttachedAndPlaced, false);
					this.SetIsDirty(true);
					if (flag)
					{
						BuilderPieceInteractor.instance.DisableCollisionsWithHands();
						return;
					}
				}
				else
				{
					if (piece2.isArmShelf || piece2.state == BuilderPiece.State.AttachedToArm)
					{
						piece.SetState(BuilderPiece.State.AttachedToArm, false);
						return;
					}
					if (piece2.isBuiltIntoTable || piece2.state == BuilderPiece.State.AttachedAndPlaced)
					{
						piece.SetActivateTimeStamp(action.timeStamp);
						piece.SetState(BuilderPiece.State.AttachedAndPlaced, false);
						if (piece2 != null)
						{
							BuilderPiece attachedBuiltInPiece = piece2.GetAttachedBuiltInPiece();
							BuilderPiecePrivatePlot builderPiecePrivatePlot;
							if (attachedBuiltInPiece != null && attachedBuiltInPiece.TryGetPlotComponent(out builderPiecePrivatePlot))
							{
								builderPiecePrivatePlot.OnPieceAttachedToPlot(piece);
							}
						}
						this.SetIsDirty(true);
						if (flag)
						{
							BuilderPieceInteractor.instance.DisableCollisionsWithHands();
							return;
						}
					}
					else
					{
						if (piece2.state == BuilderPiece.State.Grabbed)
						{
							piece.SetState(BuilderPiece.State.Grabbed, false);
							return;
						}
						if (piece2.state == BuilderPiece.State.GrabbedLocal)
						{
							piece.SetState(BuilderPiece.State.GrabbedLocal, false);
							return;
						}
						piece.SetState(BuilderPiece.State.AttachedToDropped, false);
						return;
					}
				}
				break;
			}
			case BuilderActionType.DetachFromPiece:
			{
				BuilderPiece piece3 = piece;
				bool flag4 = piece.state == BuilderPiece.State.GrabbedLocal;
				if (flag4)
				{
					piece3 = piece.GetRootPiece();
				}
				if (piece.state == BuilderPiece.State.AttachedAndPlaced)
				{
					this.SetIsDirty(true);
					BuilderPiece attachedBuiltInPiece2 = piece.GetAttachedBuiltInPiece();
					BuilderPiecePrivatePlot builderPiecePrivatePlot2;
					if (attachedBuiltInPiece2 != null && attachedBuiltInPiece2.TryGetPlotComponent(out builderPiecePrivatePlot2))
					{
						builderPiecePrivatePlot2.OnPieceDetachedFromPlot(piece);
					}
				}
				piece.ClearParentHeld();
				piece.ClearParentPiece(false);
				piece.transform.localScale = Vector3.one;
				if (flag4)
				{
					BuilderPieceInteractor.instance.OnCountChangedForRoot(piece3);
					return;
				}
				break;
			}
			case BuilderActionType.MakePieceRoot:
				BuilderPiece.MakePieceRoot(piece);
				return;
			case BuilderActionType.DropPiece:
				piece.ClearParentHeld();
				piece.ClearParentPiece(false);
				piece.transform.localScale = Vector3.one;
				piece.SetState(BuilderPiece.State.Dropped, false);
				piece.transform.SetLocalPositionAndRotation(action.localPosition, action.localRotation);
				if (piece.rigidBody != null)
				{
					piece.rigidBody.position = action.localPosition;
					piece.rigidBody.rotation = action.localRotation;
					piece.rigidBody.velocity = action.velocity;
					piece.rigidBody.angularVelocity = action.angVelocity;
					return;
				}
				break;
			case BuilderActionType.AttachToShelf:
			{
				piece.ClearParentHeld();
				piece.ClearParentPiece(false);
				int attachIndex = action.attachIndex;
				bool isLeftHand = action.isLeftHand;
				int parentAttachIndex = action.parentAttachIndex;
				float x = action.velocity.x;
				piece.transform.localScale = Vector3.one;
				piece.SetState(isLeftHand ? BuilderPiece.State.OnConveyor : BuilderPiece.State.OnShelf, false);
				if (isLeftHand)
				{
					if (attachIndex >= 0 && attachIndex < this.conveyors.Count)
					{
						BuilderConveyor builderConveyor = this.conveyors[attachIndex];
						float num2 = x / builderConveyor.GetFrameMovement();
						if (PhotonNetwork.ServerTimestamp >= parentAttachIndex)
						{
							uint num3 = (uint)(PhotonNetwork.ServerTimestamp - parentAttachIndex);
							num2 += num3 / 1000f;
						}
						piece.shelfOwner = attachIndex;
						builderConveyor.OnShelfPieceCreated(piece, num2);
						return;
					}
				}
				else
				{
					if (attachIndex >= 0 && attachIndex < this.dispenserShelves.Count)
					{
						BuilderDispenserShelf builderDispenserShelf = this.dispenserShelves[attachIndex];
						piece.shelfOwner = attachIndex;
						builderDispenserShelf.OnShelfPieceCreated(piece, false);
						return;
					}
					piece.transform.SetLocalPositionAndRotation(action.localPosition, action.localRotation);
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x06003D17 RID: 15639 RVA: 0x00158204 File Offset: 0x00156404
		public static bool AreStatesCompatibleForOverlap(BuilderPiece.State stateA, BuilderPiece.State stateB, BuilderPiece rootA, BuilderPiece rootB)
		{
			switch (stateA)
			{
			case BuilderPiece.State.None:
				return false;
			case BuilderPiece.State.AttachedAndPlaced:
				return stateB == BuilderPiece.State.AttachedAndPlaced;
			case BuilderPiece.State.AttachedToDropped:
			case BuilderPiece.State.Dropped:
			case BuilderPiece.State.OnShelf:
			case BuilderPiece.State.OnConveyor:
				return (stateB == BuilderPiece.State.AttachedToDropped || stateB == BuilderPiece.State.Dropped || stateB == BuilderPiece.State.OnShelf || stateB == BuilderPiece.State.OnConveyor) && rootA.Equals(rootB);
			case BuilderPiece.State.Grabbed:
				return stateB == BuilderPiece.State.Grabbed && rootA.Equals(rootB);
			case BuilderPiece.State.Displayed:
				return false;
			case BuilderPiece.State.GrabbedLocal:
				return stateB == BuilderPiece.State.GrabbedLocal && rootA.heldInLeftHand == rootB.heldInLeftHand;
			case BuilderPiece.State.AttachedToArm:
			{
				if (stateB != BuilderPiece.State.AttachedToArm)
				{
					return false;
				}
				object obj = (rootA.parentPiece != null) ? rootA.parentPiece : rootA;
				BuilderPiece obj2 = (rootB.parentPiece != null) ? rootB.parentPiece : rootB;
				return obj.Equals(obj2);
			}
			default:
				return false;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06003D18 RID: 15640 RVA: 0x00057D9C File Offset: 0x00055F9C
		// (set) Token: 0x06003D19 RID: 15641 RVA: 0x00057DA4 File Offset: 0x00055FA4
		public int CurrentSaveSlot
		{
			get
			{
				return this.currentSaveSlot;
			}
			set
			{
				if (this.saveInProgress)
				{
					return;
				}
				if (value < 0 || value > BuilderScanKiosk.NUM_SAVE_SLOTS)
				{
					this.currentSaveSlot = -1;
				}
				if (this.currentSaveSlot != value)
				{
					this.SetIsDirty(true);
				}
				this.currentSaveSlot = value;
			}
		}

		// Token: 0x06003D1A RID: 15642 RVA: 0x001582CC File Offset: 0x001564CC
		private void Awake()
		{
			BuilderTable.instance = this;
			if (this.buttonSnapRotation != null)
			{
				this.buttonSnapRotation.Setup(new Action<BuilderOptionButton, bool>(this.OnButtonFreeRotation));
				this.buttonSnapRotation.SetPressed(this.useSnapRotation);
			}
			if (this.buttonSnapPosition != null)
			{
				this.buttonSnapPosition.Setup(new Action<BuilderOptionButton, bool>(this.OnButtonFreePosition));
				this.buttonSnapPosition.SetPressed(this.usePlacementStyle > BuilderPlacementStyle.Float);
			}
			if (this.buttonSaveLayout != null)
			{
				this.buttonSaveLayout.Setup(new Action<BuilderOptionButton, bool>(this.OnButtonSaveLayout));
			}
			if (this.buttonClearLayout != null)
			{
				this.buttonClearLayout.Setup(new Action<BuilderOptionButton, bool>(this.OnButtonClearLayout));
			}
			this.isSetup = false;
			this.nextPieceId = 10000;
			this.queuedBuildCommands = new List<BuilderTable.BuilderCommand>(1028);
			this.plotOwners = new Dictionary<int, int>(10);
			this.doesLocalPlayerOwnPlot = false;
			this.playerToArmShelfLeft = new Dictionary<int, int>(10);
			this.playerToArmShelfRight = new Dictionary<int, int>(10);
			this.rollBackBufferedCommands = new List<BuilderTable.BuilderCommand>(1028);
			this.rollBackActions = new List<BuilderAction>(1028);
			this.rollForwardCommands = new List<BuilderTable.BuilderCommand>(1028);
			BuilderTable.placedLayer = LayerMask.NameToLayer("Gorilla Object");
			BuilderTable.heldLayerLocal = LayerMask.NameToLayer("Prop");
			BuilderTable.heldLayer = LayerMask.NameToLayer("BuilderProp");
			BuilderTable.droppedLayer = LayerMask.NameToLayer("BuilderProp");
			this.tableState = BuilderTable.TableState.WaitingForZoneAndRoom;
			this.inRoom = false;
			this.inBuilderZone = false;
			this.currSnapParams = this.pushAndEaseParams;
			this.droppedPieces = new List<BuilderPiece>(BuilderTable.DROPPED_PIECE_LIMIT + 50);
			this.droppedPieceData = new List<BuilderTable.DroppedPieceData>(BuilderTable.DROPPED_PIECE_LIMIT + 50);
			this.builderNetworking.SetTable(this);
			if (this.shelves == null)
			{
				this.shelves = new List<BuilderShelf>(64);
			}
			if (this.shelvesRoot != null)
			{
				this.shelvesRoot.GetComponentsInChildren<BuilderShelf>(this.shelves);
			}
			this.conveyors = new List<BuilderConveyor>(32);
			this.dispenserShelves = new List<BuilderDispenserShelf>(32);
			if (this.allShelvesRoot != null)
			{
				for (int i = 0; i < this.allShelvesRoot.Count; i++)
				{
					this.allShelvesRoot[i].GetComponentsInChildren<BuilderConveyor>(BuilderTable.tempConveyors);
					this.conveyors.AddRange(BuilderTable.tempConveyors);
					BuilderTable.tempConveyors.Clear();
					this.allShelvesRoot[i].GetComponentsInChildren<BuilderDispenserShelf>(BuilderTable.tempDispensers);
					this.dispenserShelves.AddRange(BuilderTable.tempDispensers);
					BuilderTable.tempDispensers.Clear();
				}
			}
			this.recyclers = new List<BuilderRecycler>(5);
			if (this.recyclerRoot != null)
			{
				for (int j = 0; j < this.recyclerRoot.Count; j++)
				{
					this.recyclerRoot[j].GetComponentsInChildren<BuilderRecycler>(BuilderTable.tempRecyclers);
					this.recyclers.AddRange(BuilderTable.tempRecyclers);
					BuilderTable.tempRecyclers.Clear();
				}
			}
			for (int k = 0; k < this.recyclers.Count; k++)
			{
				this.recyclers[k].recyclerID = k;
			}
			this.dropZones = new List<BuilderDropZone>(6);
			this.dropZoneRoot.GetComponentsInChildren<BuilderDropZone>(this.dropZones);
			for (int l = 0; l < this.dropZones.Count; l++)
			{
				this.dropZones[l].dropZoneID = l;
			}
			this.maxResources = new int[3];
			if (this.totalResources != null && this.totalResources.quantities != null)
			{
				for (int m = 0; m < this.totalResources.quantities.Count; m++)
				{
					if (this.totalResources.quantities[m].type >= BuilderResourceType.Basic && this.totalResources.quantities[m].type < BuilderResourceType.Count)
					{
						this.maxResources[(int)this.totalResources.quantities[m].type] += this.totalResources.quantities[m].count;
					}
				}
			}
			this.usedResources = new int[3];
			this.reservedResources = new int[3];
			if (this.totalReservedResources != null && this.totalReservedResources.quantities != null)
			{
				for (int n = 0; n < this.totalReservedResources.quantities.Count; n++)
				{
					if (this.totalReservedResources.quantities[n].type >= BuilderResourceType.Basic && this.totalReservedResources.quantities[n].type < BuilderResourceType.Count)
					{
						this.reservedResources[(int)this.totalReservedResources.quantities[n].type] += this.totalReservedResources.quantities[n].count;
					}
				}
			}
			this.plotMaxResources = new int[3];
			if (this.resourcesPerPrivatePlot != null && this.resourcesPerPrivatePlot.quantities != null)
			{
				for (int num = 0; num < this.resourcesPerPrivatePlot.quantities.Count; num++)
				{
					if (this.resourcesPerPrivatePlot.quantities[num].type >= BuilderResourceType.Basic && this.resourcesPerPrivatePlot.quantities[num].type < BuilderResourceType.Count)
					{
						this.plotMaxResources[(int)this.resourcesPerPrivatePlot.quantities[num].type] += this.resourcesPerPrivatePlot.quantities[num].count;
					}
				}
			}
			this.OnAvailableResourcesChange();
			this.gridPlaneData = new NativeList<BuilderGridPlaneData>(1024, Allocator.Persistent);
			this.checkGridPlaneData = new NativeList<BuilderGridPlaneData>(1024, Allocator.Persistent);
			this.nearbyPiecesResults = new NativeArray<ColliderHit>(1024, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.nearbyPiecesCommands = new NativeArray<OverlapSphereCommand>(1, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.allPotentialPlacements = new List<BuilderPotentialPlacement>(1024);
			BuilderTable.saveDateKeys.Clear();
			for (int num2 = 0; num2 < BuilderScanKiosk.NUM_SAVE_SLOTS; num2++)
			{
				BuilderTable.saveDateKeys.Add(this.GetSaveDataTimeKey(num2));
			}
		}

		// Token: 0x06003D1B RID: 15643 RVA: 0x00057DD9 File Offset: 0x00055FD9
		private void Start()
		{
			ZoneManagement zoneManagement = ZoneManagement.instance;
			zoneManagement.onZoneChanged = (Action)Delegate.Combine(zoneManagement.onZoneChanged, new Action(this.HandleOnZoneChanged));
			base.StartCoroutine(this.RequestTitleDataOnLogIn());
		}

		// Token: 0x06003D1C RID: 15644 RVA: 0x00057E0E File Offset: 0x0005600E
		private void OnApplicationQuit()
		{
			this.ClearTable();
			this.tableState = BuilderTable.TableState.WaitingForZoneAndRoom;
		}

		// Token: 0x06003D1D RID: 15645 RVA: 0x00158918 File Offset: 0x00156B18
		private void OnDestroy()
		{
			ZoneManagement zoneManagement = ZoneManagement.instance;
			zoneManagement.onZoneChanged = (Action)Delegate.Remove(zoneManagement.onZoneChanged, new Action(this.HandleOnZoneChanged));
			if (this.gridPlaneData.IsCreated)
			{
				this.gridPlaneData.Dispose();
			}
			if (this.checkGridPlaneData.IsCreated)
			{
				this.checkGridPlaneData.Dispose();
			}
			if (this.nearbyPiecesResults.IsCreated)
			{
				this.nearbyPiecesResults.Dispose();
			}
			if (this.nearbyPiecesCommands.IsCreated)
			{
				this.nearbyPiecesCommands.Dispose();
			}
			this.DestroyData();
		}

		// Token: 0x06003D1E RID: 15646 RVA: 0x001589B4 File Offset: 0x00156BB4
		private void HandleOnZoneChanged()
		{
			bool flag = ZoneManagement.instance.IsZoneActive(GTZone.monkeBlocks);
			this.SetInBuilderZone(flag);
		}

		// Token: 0x06003D1F RID: 15647 RVA: 0x001589D8 File Offset: 0x00156BD8
		public void InitIfNeeded()
		{
			if (!this.isSetup)
			{
				for (int i = 0; i < this.factories.Count; i++)
				{
					this.factories[i].Setup();
				}
				this.builderRenderer.BuildRenderer(this.factories[0].pieceList);
				this.baseGridPlanes.Clear();
				this.basePieces = new List<BuilderPiece>(1024);
				for (int j = 0; j < this.builtInPieceRoots.Count; j++)
				{
					this.builtInPieceRoots[j].SetActive(true);
					this.builtInPieceRoots[j].GetComponentsInChildren<BuilderPiece>(false, BuilderTable.tempPieces);
					this.basePieces.AddRange(BuilderTable.tempPieces);
				}
				this.allPrivatePlots = new List<BuilderPiecePrivatePlot>(20);
				this.CreateData();
				for (int k = 0; k < this.basePieces.Count; k++)
				{
					BuilderPiece builderPiece = this.basePieces[k];
					builderPiece.pieceId = 5 + k;
					builderPiece.SetScale(this.pieceScale);
					builderPiece.SetupPiece(this.gridSize);
					builderPiece.OnCreate();
					builderPiece.SetState(BuilderPiece.State.OnShelf, true);
					this.baseGridPlanes.AddRange(builderPiece.gridPlanes);
					BuilderPiecePrivatePlot item;
					if (builderPiece.IsPrivatePlot() && builderPiece.TryGetPlotComponent(out item))
					{
						this.allPrivatePlots.Add(item);
					}
					this.AddPieceData(builderPiece);
				}
				this.builderPool.Setup(this.factories[0]);
				this.builderPool.BuildFromPieceSets();
				for (int l = 0; l < this.factories.Count; l++)
				{
					this.factories[l].Show();
				}
				for (int m = 0; m < this.conveyors.Count; m++)
				{
					this.conveyors[m].table = this;
					this.conveyors[m].shelfID = m;
					this.conveyors[m].Setup();
				}
				for (int n = 0; n < this.dispenserShelves.Count; n++)
				{
					this.dispenserShelves[n].table = this;
					this.dispenserShelves[n].shelfID = n;
					this.dispenserShelves[n].Setup();
				}
				this.conveyorManager.Setup(this);
				this.repelledPieceRoots = new HashSet<int>[this.repelHistoryLength];
				for (int num = 0; num < this.repelHistoryLength; num++)
				{
					this.repelledPieceRoots[num] = new HashSet<int>(10);
				}
				this.sharedBuildAreas = this.sharedBuildArea.GetComponents<BoxCollider>();
				BoxCollider[] array = this.sharedBuildAreas;
				for (int num2 = 0; num2 < array.Length; num2++)
				{
					array[num2].enabled = false;
				}
				this.sharedBuildArea.SetActive(false);
				this.isSetup = true;
			}
		}

		// Token: 0x06003D20 RID: 15648 RVA: 0x00057E1D File Offset: 0x0005601D
		private void SetIsDirty(bool dirty)
		{
			if (this.isDirty != dirty)
			{
				UnityEvent<bool> onSaveDirtyChanged = this.OnSaveDirtyChanged;
				if (onSaveDirtyChanged != null)
				{
					onSaveDirtyChanged.Invoke(dirty);
				}
			}
			this.isDirty = dirty;
		}

		// Token: 0x06003D21 RID: 15649 RVA: 0x00158CC0 File Offset: 0x00156EC0
		private void FixedUpdate()
		{
			if (this.tableState != BuilderTable.TableState.Ready && this.tableState != BuilderTable.TableState.WaitForMasterResync)
			{
				return;
			}
			foreach (IBuilderPieceFunctional builderPieceFunctional in this.funcComponentsToRegisterFixed)
			{
				if (builderPieceFunctional != null)
				{
					this.fixedUpdateFunctionalComponents.Add(builderPieceFunctional);
				}
			}
			foreach (IBuilderPieceFunctional item in this.funcComponentsToUnregisterFixed)
			{
				this.fixedUpdateFunctionalComponents.Remove(item);
			}
			this.funcComponentsToRegisterFixed.Clear();
			this.funcComponentsToUnregisterFixed.Clear();
			foreach (IBuilderPieceFunctional builderPieceFunctional2 in this.fixedUpdateFunctionalComponents)
			{
				builderPieceFunctional2.FunctionalPieceFixedUpdate();
			}
		}

		// Token: 0x06003D22 RID: 15650 RVA: 0x00158DCC File Offset: 0x00156FCC
		private void Update()
		{
			this.InitIfNeeded();
			this.UpdateTableState();
			this.UpdateDroppedPieces(Time.deltaTime);
			this.repelHistoryIndex = (this.repelHistoryIndex + 1) % this.repelHistoryLength;
			int num = (this.repelHistoryIndex + 1) % this.repelHistoryLength;
			this.repelledPieceRoots[num].Clear();
		}

		// Token: 0x06003D23 RID: 15651 RVA: 0x00057E41 File Offset: 0x00056041
		public void AddQueuedCommand(BuilderTable.BuilderCommand cmd)
		{
			this.queuedBuildCommands.Add(cmd);
		}

		// Token: 0x06003D24 RID: 15652 RVA: 0x00057E4F File Offset: 0x0005604F
		public void ClearQueuedCommands()
		{
			if (this.queuedBuildCommands != null)
			{
				this.queuedBuildCommands.Clear();
			}
			this.RemoveRollBackActions();
			if (this.rollBackBufferedCommands != null)
			{
				this.rollBackBufferedCommands.Clear();
			}
			this.RemoveRollForwardCommands();
		}

		// Token: 0x06003D25 RID: 15653 RVA: 0x00057E83 File Offset: 0x00056083
		public int GetNumQueuedCommands()
		{
			if (this.queuedBuildCommands != null)
			{
				return this.queuedBuildCommands.Count;
			}
			return 0;
		}

		// Token: 0x06003D26 RID: 15654 RVA: 0x00057E9A File Offset: 0x0005609A
		public void AddRollbackAction(BuilderAction action)
		{
			this.rollBackActions.Add(action);
		}

		// Token: 0x06003D27 RID: 15655 RVA: 0x00057EA8 File Offset: 0x000560A8
		public void RemoveRollBackActions()
		{
			this.rollBackActions.Clear();
		}

		// Token: 0x06003D28 RID: 15656 RVA: 0x00158E24 File Offset: 0x00157024
		public void RemoveRollBackActions(int localCommandId)
		{
			for (int i = this.rollBackActions.Count - 1; i >= 0; i--)
			{
				if (localCommandId == -1 || this.rollBackActions[i].localCommandId == localCommandId)
				{
					this.rollBackActions.RemoveAt(i);
				}
			}
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x00158E70 File Offset: 0x00157070
		public bool HasRollBackActionsForCommand(int localCommandId)
		{
			for (int i = 0; i < this.rollBackActions.Count; i++)
			{
				if (this.rollBackActions[i].localCommandId == localCommandId)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003D2A RID: 15658 RVA: 0x00057EB5 File Offset: 0x000560B5
		public void AddRollForwardCommand(BuilderTable.BuilderCommand command)
		{
			this.rollForwardCommands.Add(command);
		}

		// Token: 0x06003D2B RID: 15659 RVA: 0x00057EC3 File Offset: 0x000560C3
		public void RemoveRollForwardCommands()
		{
			this.rollForwardCommands.Clear();
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x00158EAC File Offset: 0x001570AC
		public void RemoveRollForwardCommands(int localCommandId)
		{
			for (int i = this.rollForwardCommands.Count - 1; i >= 0; i--)
			{
				if (localCommandId == -1 || this.rollForwardCommands[i].localCommandId == localCommandId)
				{
					this.rollForwardCommands.RemoveAt(i);
				}
			}
		}

		// Token: 0x06003D2D RID: 15661 RVA: 0x00158EF8 File Offset: 0x001570F8
		public bool HasRollForwardCommand(int localCommandId)
		{
			for (int i = 0; i < this.rollForwardCommands.Count; i++)
			{
				if (this.rollForwardCommands[i].localCommandId == localCommandId)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003D2E RID: 15662 RVA: 0x00158F34 File Offset: 0x00157134
		public bool ShouldRollbackBufferCommand(BuilderTable.BuilderCommand cmd)
		{
			return cmd.type != BuilderTable.BuilderCommandType.Create && cmd.type != BuilderTable.BuilderCommandType.CreateArmShelf && this.rollBackActions.Count > 0 && (cmd.player == null || !cmd.player.IsLocal || !this.HasRollForwardCommand(cmd.localCommandId));
		}

		// Token: 0x06003D2F RID: 15663 RVA: 0x00057ED0 File Offset: 0x000560D0
		public void AddRollbackBufferedCommand(BuilderTable.BuilderCommand bufferedCmd)
		{
			this.rollBackBufferedCommands.Add(bufferedCmd);
		}

		// Token: 0x06003D30 RID: 15664 RVA: 0x00158F8C File Offset: 0x0015718C
		private void ExecuteRollBackActions()
		{
			for (int i = this.rollBackActions.Count - 1; i >= 0; i--)
			{
				this.ExecuteAction(this.rollBackActions[i]);
			}
			this.rollBackActions.Clear();
		}

		// Token: 0x06003D31 RID: 15665 RVA: 0x00158FD0 File Offset: 0x001571D0
		private void ExecuteRollbackBufferedCommands()
		{
			for (int i = 0; i < this.rollBackBufferedCommands.Count; i++)
			{
				BuilderTable.BuilderCommand cmd = this.rollBackBufferedCommands[i];
				cmd.isQueued = false;
				cmd.canRollback = false;
				this.ExecuteBuildCommand(cmd);
			}
			this.rollBackBufferedCommands.Clear();
		}

		// Token: 0x06003D32 RID: 15666 RVA: 0x00159024 File Offset: 0x00157224
		private void ExecuteRollForwardCommands()
		{
			BuilderTable.tempRollForwardCommands.Clear();
			for (int i = 0; i < this.rollForwardCommands.Count; i++)
			{
				BuilderTable.tempRollForwardCommands.Add(this.rollForwardCommands[i]);
			}
			this.rollForwardCommands.Clear();
			for (int j = 0; j < BuilderTable.tempRollForwardCommands.Count; j++)
			{
				BuilderTable.BuilderCommand cmd = BuilderTable.tempRollForwardCommands[j];
				cmd.isQueued = true;
				cmd.canRollback = true;
				this.ExecuteBuildCommand(cmd);
			}
			BuilderTable.tempRollForwardCommands.Clear();
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x001590B4 File Offset: 0x001572B4
		private void UpdateRollForwardCommandData()
		{
			for (int i = 0; i < this.rollForwardCommands.Count; i++)
			{
				BuilderTable.BuilderCommand builderCommand = this.rollForwardCommands[i];
				if (builderCommand.type == BuilderTable.BuilderCommandType.Drop)
				{
					BuilderPiece piece = this.GetPiece(builderCommand.pieceId);
					if (piece != null && piece.rigidBody != null)
					{
						builderCommand.localPosition = piece.rigidBody.position;
						builderCommand.localRotation = piece.rigidBody.rotation;
						builderCommand.velocity = piece.rigidBody.velocity;
						builderCommand.angVelocity = piece.rigidBody.angularVelocity;
						this.rollForwardCommands[i] = builderCommand;
					}
				}
			}
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x0015916C File Offset: 0x0015736C
		public bool TryRollbackAndReExecute(int localCommandId)
		{
			if (this.HasRollBackActionsForCommand(localCommandId))
			{
				if (this.rollBackBufferedCommands.Count > 0)
				{
					this.UpdateRollForwardCommandData();
					this.ExecuteRollBackActions();
					this.ExecuteRollbackBufferedCommands();
					this.ExecuteRollForwardCommands();
					this.RemoveRollBackActions(localCommandId);
					this.RemoveRollForwardCommands(localCommandId);
				}
				else
				{
					this.RemoveRollBackActions(localCommandId);
					this.RemoveRollForwardCommands(localCommandId);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06003D35 RID: 15669 RVA: 0x00057EDE File Offset: 0x000560DE
		public void RollbackFailedCommand(int localCommandId)
		{
			if (this.HasRollBackActionsForCommand(localCommandId))
			{
				this.UpdateRollForwardCommandData();
				this.ExecuteRollBackActions();
				this.ExecuteRollbackBufferedCommands();
				this.RemoveRollForwardCommands(-1);
				this.ExecuteRollForwardCommands();
			}
		}

		// Token: 0x06003D36 RID: 15670 RVA: 0x00057F08 File Offset: 0x00056108
		public BuilderTable.TableState GetTableState()
		{
			return this.tableState;
		}

		// Token: 0x06003D37 RID: 15671 RVA: 0x001591CC File Offset: 0x001573CC
		public void SetTableState(BuilderTable.TableState newState)
		{
			this.InitIfNeeded();
			if (newState == this.tableState)
			{
				return;
			}
			BuilderTable.TableState tableState = this.tableState;
			this.tableState = newState;
			switch (this.tableState)
			{
			case BuilderTable.TableState.WaitingForInitalBuild:
				this.GetLastSaveTime();
				return;
			case BuilderTable.TableState.ReceivingInitialBuild:
			case BuilderTable.TableState.ReceivingMasterResync:
			case BuilderTable.TableState.InitialBuild:
			case BuilderTable.TableState.ExecuteQueuedCommands:
				break;
			case BuilderTable.TableState.WaitForInitialBuildMaster:
				this.nextPieceId = 10000;
				this.BuildInitialTableForPlayer();
				return;
			case BuilderTable.TableState.WaitForMasterResync:
				this.ClearQueuedCommands();
				this.ResetConveyors();
				return;
			case BuilderTable.TableState.Ready:
				this.OnAvailableResourcesChange();
				return;
			case BuilderTable.TableState.BadData:
				this.ClearTable();
				this.ClearQueuedCommands();
				break;
			default:
				return;
			}
		}

		// Token: 0x06003D38 RID: 15672 RVA: 0x00159268 File Offset: 0x00157468
		public void SetInRoom(bool inRoom)
		{
			this.inRoom = inRoom;
			bool flag = inRoom && this.inBuilderZone;
			if (flag && this.tableState == BuilderTable.TableState.WaitingForZoneAndRoom)
			{
				this.SetTableState(BuilderTable.TableState.WaitingForInitalBuild);
				this.builderNetworking.PlayerEnterBuilder();
				return;
			}
			if (!flag && this.tableState != BuilderTable.TableState.WaitingForZoneAndRoom && !this.builderNetworking.IsPrivateMasterClient())
			{
				this.SetTableState(BuilderTable.TableState.WaitingForZoneAndRoom);
				this.builderNetworking.PlayerExitBuilder();
				return;
			}
			if (flag && PhotonNetwork.IsMasterClient)
			{
				this.builderNetworking.RequestCreateArmShelfForPlayer(PhotonNetwork.LocalPlayer);
				return;
			}
			if (!flag && this.builderNetworking.IsPrivateMasterClient())
			{
				this.RemoveArmShelfForPlayer(PhotonNetwork.LocalPlayer);
			}
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x00057F10 File Offset: 0x00056110
		public static bool IsLocalPlayerInBuilderZone()
		{
			return !(BuilderTable.instance == null) && BuilderTable.instance.IsInBuilderZone();
		}

		// Token: 0x06003D3A RID: 15674 RVA: 0x00057F2B File Offset: 0x0005612B
		public bool IsInBuilderZone()
		{
			return this.inBuilderZone;
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x00159308 File Offset: 0x00157508
		public void SetInBuilderZone(bool inBuilderZone)
		{
			this.inBuilderZone = inBuilderZone;
			if (this.builderRenderer != null)
			{
				this.builderRenderer.Show(inBuilderZone);
			}
			bool flag = this.inRoom && inBuilderZone;
			if (flag && this.tableState == BuilderTable.TableState.WaitingForZoneAndRoom)
			{
				this.SetTableState(BuilderTable.TableState.WaitingForInitalBuild);
				this.builderNetworking.PlayerEnterBuilder();
				return;
			}
			if (!flag && this.tableState != BuilderTable.TableState.WaitingForZoneAndRoom && !this.builderNetworking.IsPrivateMasterClient())
			{
				this.SetTableState(BuilderTable.TableState.WaitingForZoneAndRoom);
				this.builderNetworking.PlayerExitBuilder();
				return;
			}
			if (flag && PhotonNetwork.IsMasterClient)
			{
				this.builderNetworking.RequestCreateArmShelfForPlayer(PhotonNetwork.LocalPlayer);
				return;
			}
			if (!flag && this.builderNetworking.IsPrivateMasterClient())
			{
				this.RemoveArmShelfForPlayer(PhotonNetwork.LocalPlayer);
			}
		}

		// Token: 0x06003D3C RID: 15676 RVA: 0x001593C0 File Offset: 0x001575C0
		private void UpdateTableState()
		{
			switch (this.tableState)
			{
			case BuilderTable.TableState.InitialBuild:
			{
				BuilderTableNetworking.PlayerTableInitState localTableInit = this.builderNetworking.GetLocalTableInit();
				try
				{
					this.ClearTable();
					this.ClearQueuedCommands();
					byte[] array = GZipStream.UncompressBuffer(localTableInit.serializedTableState);
					localTableInit.totalSerializedBytes = array.Length;
					Array.Copy(array, 0, localTableInit.serializedTableState, 0, localTableInit.totalSerializedBytes);
					this.DeserializeTableState(localTableInit.serializedTableState, localTableInit.numSerializedBytes);
					this.SetTableState(BuilderTable.TableState.ExecuteQueuedCommands);
					this.SetIsDirty(true);
					return;
				}
				catch (Exception)
				{
					this.SetTableState(BuilderTable.TableState.BadData);
					return;
				}
				break;
			}
			case BuilderTable.TableState.ExecuteQueuedCommands:
				break;
			case BuilderTable.TableState.Ready:
			{
				this.conveyorManager.UpdateManager();
				JobHandle jobHandle = this.conveyorManager.ConstructJobHandle();
				JobHandle.ScheduleBatchedJobs();
				foreach (BuilderDispenserShelf builderDispenserShelf in this.dispenserShelves)
				{
					builderDispenserShelf.UpdateShelf();
				}
				foreach (BuilderPiecePrivatePlot builderPiecePrivatePlot in this.allPrivatePlots)
				{
					builderPiecePrivatePlot.UpdatePlot();
				}
				foreach (BuilderRecycler builderRecycler in this.recyclers)
				{
					builderRecycler.UpdateRecycler();
				}
				for (int i = this.shelfSliceUpdateIndex; i < this.dispenserShelves.Count; i += BuilderTable.SHELF_SLICE_BUCKETS)
				{
					this.dispenserShelves[i].UpdateShelfSliced();
				}
				this.shelfSliceUpdateIndex = (this.shelfSliceUpdateIndex + 1) % BuilderTable.SHELF_SLICE_BUCKETS;
				foreach (IBuilderPieceFunctional builderPieceFunctional in this.funcComponentsToRegister)
				{
					if (builderPieceFunctional != null)
					{
						this.activeFunctionalComponents.Add(builderPieceFunctional);
					}
				}
				foreach (IBuilderPieceFunctional item in this.funcComponentsToUnregister)
				{
					this.activeFunctionalComponents.Remove(item);
				}
				this.funcComponentsToRegister.Clear();
				this.funcComponentsToUnregister.Clear();
				foreach (IBuilderPieceFunctional builderPieceFunctional2 in this.activeFunctionalComponents)
				{
					if (builderPieceFunctional2 != null)
					{
						builderPieceFunctional2.FunctionalPieceUpdate();
					}
				}
				foreach (BuilderResourceMeter builderResourceMeter in this.resourceMeters)
				{
					builderResourceMeter.UpdateMeterFill();
				}
				this.CleanUpDroppedPiece();
				jobHandle.Complete();
				return;
			}
			default:
				return;
			}
			for (int j = 0; j < this.queuedBuildCommands.Count; j++)
			{
				BuilderTable.BuilderCommand cmd = this.queuedBuildCommands[j];
				cmd.isQueued = true;
				this.ExecuteBuildCommand(cmd);
			}
			this.queuedBuildCommands.Clear();
			this.SetTableState(BuilderTable.TableState.Ready);
		}

		// Token: 0x06003D3D RID: 15677 RVA: 0x00159724 File Offset: 0x00157924
		private void RouteNewCommand(BuilderTable.BuilderCommand cmd, bool force)
		{
			bool flag = this.ShouldExecuteCommand();
			if (force)
			{
				this.ExecuteBuildCommand(cmd);
				return;
			}
			if (flag && this.ShouldRollbackBufferCommand(cmd))
			{
				this.AddRollbackBufferedCommand(cmd);
				return;
			}
			if (flag)
			{
				this.ExecuteBuildCommand(cmd);
				return;
			}
			if (this.ShouldQueueCommand())
			{
				this.AddQueuedCommand(cmd);
				return;
			}
			this.ShouldDiscardCommand();
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x0015977C File Offset: 0x0015797C
		private void ExecuteBuildCommand(BuilderTable.BuilderCommand cmd)
		{
			switch (cmd.type)
			{
			case BuilderTable.BuilderCommandType.Create:
				this.ExecutePieceCreated(cmd);
				return;
			case BuilderTable.BuilderCommandType.Place:
				this.ExecutePiecePlacedWithActions(cmd);
				return;
			case BuilderTable.BuilderCommandType.Grab:
				this.ExecutePieceGrabbedWithActions(cmd);
				return;
			case BuilderTable.BuilderCommandType.Drop:
				this.ExecutePieceDroppedWithActions(cmd);
				return;
			case BuilderTable.BuilderCommandType.Remove:
				break;
			case BuilderTable.BuilderCommandType.Paint:
				this.ExecutePiecePainted(cmd);
				return;
			case BuilderTable.BuilderCommandType.Recycle:
				this.ExecutePieceRecycled(cmd);
				return;
			case BuilderTable.BuilderCommandType.ClaimPlot:
				this.ExecuteClaimPlot(cmd);
				return;
			case BuilderTable.BuilderCommandType.FreePlot:
				this.ExecuteFreePlot(cmd);
				return;
			case BuilderTable.BuilderCommandType.CreateArmShelf:
				this.ExecuteArmShelfCreated(cmd);
				return;
			case BuilderTable.BuilderCommandType.PlayerLeftRoom:
				this.ExecutePlayerLeftRoom(cmd);
				return;
			case BuilderTable.BuilderCommandType.FunctionalStateChange:
				this.ExecuteSetFunctionalPieceState(cmd);
				return;
			case BuilderTable.BuilderCommandType.SetSelection:
				this.ExecuteSetSelection(cmd);
				return;
			case BuilderTable.BuilderCommandType.Repel:
				this.ExecutePieceRepelled(cmd);
				break;
			default:
				return;
			}
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x00057F33 File Offset: 0x00056133
		public void ClearTable()
		{
			this.ClearTableInternal();
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x00159838 File Offset: 0x00157A38
		private void ClearTableInternal()
		{
			BuilderTable.tempDeletePieces.Clear();
			for (int i = 0; i < this.pieces.Count; i++)
			{
				BuilderTable.tempDeletePieces.Add(this.pieces[i]);
			}
			this.droppedPieces.Clear();
			this.droppedPieceData.Clear();
			for (int j = 0; j < BuilderTable.tempDeletePieces.Count; j++)
			{
				BuilderTable.tempDeletePieces[j].ClearParentPiece(false);
				BuilderTable.tempDeletePieces[j].ClearParentHeld();
				BuilderTable.tempDeletePieces[j].SetState(BuilderPiece.State.None, false);
				this.RemovePiece(BuilderTable.tempDeletePieces[j]);
			}
			for (int k = 0; k < BuilderTable.tempDeletePieces.Count; k++)
			{
				this.builderPool.DestroyPiece(BuilderTable.tempDeletePieces[k]);
			}
			BuilderTable.tempDeletePieces.Clear();
			this.pieces.Clear();
			this.pieceIDToIndexCache.Clear();
			this.nextPieceId = 10000;
			this.conveyorManager.OnClearTable();
			foreach (BuilderDispenserShelf builderDispenserShelf in this.dispenserShelves)
			{
				builderDispenserShelf.OnClearTable();
			}
			for (int l = 0; l < this.repelHistoryLength; l++)
			{
				this.repelledPieceRoots[l].Clear();
			}
			this.funcComponentsToRegister.Clear();
			this.funcComponentsToUnregister.Clear();
			this.activeFunctionalComponents.Clear();
			foreach (BuilderPiece builderPiece in this.basePieces)
			{
				foreach (BuilderAttachGridPlane builderAttachGridPlane in builderPiece.gridPlanes)
				{
					builderAttachGridPlane.OnReturnToPool(this.builderPool);
				}
			}
			this.ClearBuiltInPlots();
			this.playerToArmShelfLeft.Clear();
			this.playerToArmShelfRight.Clear();
			if (BuilderPieceInteractor.instance != null)
			{
				BuilderPieceInteractor.instance.RemovePiecesFromHands();
			}
		}

		// Token: 0x06003D41 RID: 15681 RVA: 0x00159A8C File Offset: 0x00157C8C
		private void ClearBuiltInPlots()
		{
			foreach (BuilderPiecePrivatePlot builderPiecePrivatePlot in this.allPrivatePlots)
			{
				builderPiecePrivatePlot.ClearPlot();
			}
			this.plotOwners.Clear();
			this.SetLocalPlayerOwnsPlot(false);
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x00159AF0 File Offset: 0x00157CF0
		private void OnDeserializeUpdatePlots()
		{
			foreach (BuilderPiecePrivatePlot builderPiecePrivatePlot in this.allPrivatePlots)
			{
				builderPiecePrivatePlot.RecountPlotCost();
			}
		}

		// Token: 0x06003D43 RID: 15683 RVA: 0x00159B40 File Offset: 0x00157D40
		public void BuildPiecesOnShelves()
		{
			if (this.shelves == null)
			{
				return;
			}
			for (int i = 0; i < this.shelves.Count; i++)
			{
				if (this.shelves[i] != null)
				{
					this.shelves[i].Init();
				}
			}
			bool flag = true;
			while (flag)
			{
				flag = false;
				for (int j = 0; j < this.shelves.Count; j++)
				{
					if (this.shelves[j].HasOpenSlot())
					{
						this.shelves[j].BuildNextPiece(this);
						if (this.shelves[j].HasOpenSlot())
						{
							flag = true;
						}
					}
				}
			}
		}

		// Token: 0x06003D44 RID: 15684 RVA: 0x00057F3B File Offset: 0x0005613B
		private void OnFinishedInitialTableBuild()
		{
			this.BuildPiecesOnShelves();
			this.SetTableState(BuilderTable.TableState.Ready);
			this.CreateArmShelvesForPlayersInBuilder();
		}

		// Token: 0x06003D45 RID: 15685 RVA: 0x00057F50 File Offset: 0x00056150
		public int CreatePieceId()
		{
			int result = this.nextPieceId;
			if (this.nextPieceId == 2147483647)
			{
				this.nextPieceId = 20000;
			}
			this.nextPieceId++;
			return result;
		}

		// Token: 0x06003D46 RID: 15686 RVA: 0x00159BEC File Offset: 0x00157DEC
		public void ResetConveyors()
		{
			foreach (BuilderConveyor builderConveyor in this.conveyors)
			{
				builderConveyor.ResetConveyorState();
			}
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x00159C3C File Offset: 0x00157E3C
		public void RequestCreateConveyorPiece(int newPieceType, int materialType, int shelfID)
		{
			if (shelfID < 0 || shelfID >= this.conveyors.Count)
			{
				return;
			}
			BuilderConveyor builderConveyor = this.conveyors[shelfID];
			if (builderConveyor == null)
			{
				return;
			}
			Transform spawnTransform = builderConveyor.GetSpawnTransform();
			this.builderNetworking.CreateShelfPiece(newPieceType, spawnTransform.position, spawnTransform.rotation, materialType, BuilderPiece.State.OnConveyor, shelfID);
		}

		// Token: 0x06003D48 RID: 15688 RVA: 0x00057F7E File Offset: 0x0005617E
		public void RequestCreateDispenserShelfPiece(int pieceType, Vector3 position, Quaternion rotation, int materialType, int shelfID)
		{
			if (shelfID < 0 || shelfID >= this.dispenserShelves.Count)
			{
				return;
			}
			if (this.dispenserShelves[shelfID] == null)
			{
				return;
			}
			this.builderNetworking.CreateShelfPiece(pieceType, position, rotation, materialType, BuilderPiece.State.OnShelf, shelfID);
		}

		// Token: 0x06003D49 RID: 15689 RVA: 0x00159C98 File Offset: 0x00157E98
		public void CreateConveyorPiece(int pieceType, int pieceId, Vector3 position, Quaternion rotation, int materialType, int shelfID, int sendTimestamp)
		{
			if (shelfID < 0 || shelfID >= this.conveyors.Count)
			{
				return;
			}
			if (this.conveyors[shelfID] == null)
			{
				return;
			}
			BuilderTable.BuilderCommand cmd = new BuilderTable.BuilderCommand
			{
				type = BuilderTable.BuilderCommandType.Create,
				pieceType = pieceType,
				pieceId = pieceId,
				localPosition = position,
				localRotation = rotation,
				materialType = materialType,
				state = BuilderPiece.State.OnConveyor,
				parentPieceId = shelfID,
				parentAttachIndex = sendTimestamp,
				player = NetworkSystem.Instance.MasterClient
			};
			this.RouteNewCommand(cmd, false);
		}

		// Token: 0x06003D4A RID: 15690 RVA: 0x00159D40 File Offset: 0x00157F40
		public void CreateDispenserShelfPiece(int pieceType, int pieceId, Vector3 position, Quaternion rotation, int materialType, int shelfID)
		{
			if (shelfID < 0 || shelfID >= this.dispenserShelves.Count)
			{
				return;
			}
			if (this.dispenserShelves[shelfID] == null)
			{
				return;
			}
			BuilderTable.BuilderCommand cmd = new BuilderTable.BuilderCommand
			{
				type = BuilderTable.BuilderCommandType.Create,
				pieceType = pieceType,
				pieceId = pieceId,
				localPosition = position,
				localRotation = rotation,
				materialType = materialType,
				state = BuilderPiece.State.OnShelf,
				parentPieceId = shelfID,
				isLeft = true,
				player = NetworkSystem.Instance.MasterClient
			};
			this.RouteNewCommand(cmd, false);
		}

		// Token: 0x06003D4B RID: 15691 RVA: 0x00057FBE File Offset: 0x000561BE
		public void RequestShelfSelection(int shelfId, int setId, bool isConveyor)
		{
			if (this.tableState != BuilderTable.TableState.Ready)
			{
				return;
			}
			this.builderNetworking.RequestShelfSelection(shelfId, setId, isConveyor);
		}

		// Token: 0x06003D4C RID: 15692 RVA: 0x00159DE8 File Offset: 0x00157FE8
		public void VerifySetSelections()
		{
			foreach (BuilderConveyor builderConveyor in this.conveyors)
			{
				builderConveyor.VerifySetSelection();
			}
			foreach (BuilderDispenserShelf builderDispenserShelf in this.dispenserShelves)
			{
				builderDispenserShelf.VerifySetSelection();
			}
		}

		// Token: 0x06003D4D RID: 15693 RVA: 0x00159E78 File Offset: 0x00158078
		public bool ValidateShelfSelectionParams(int shelfId, int setId, bool isConveyor, Player player)
		{
			bool flag = shelfId >= 0 && ((isConveyor && shelfId < this.conveyors.Count) || (!isConveyor && shelfId < this.dispenserShelves.Count)) && BuilderSetManager.instance.DoesPlayerOwnPieceSet(player, setId);
			if (PhotonNetwork.IsMasterClient)
			{
				if (isConveyor)
				{
					BuilderConveyor builderConveyor = this.conveyors[shelfId];
					bool flag2 = this.IsPlayerHandNearAction(NetPlayer.Get(player), builderConveyor.transform.position, false, true, 4f);
					flag = (flag && flag2);
				}
				else
				{
					BuilderDispenserShelf builderDispenserShelf = this.dispenserShelves[shelfId];
					bool flag3 = this.IsPlayerHandNearAction(NetPlayer.Get(player), builderDispenserShelf.transform.position, false, true, 4f);
					flag = (flag && flag3);
				}
			}
			return flag;
		}

		// Token: 0x06003D4E RID: 15694 RVA: 0x00159F34 File Offset: 0x00158134
		private void SetConveyorSelection(int conveyorId, int setId)
		{
			BuilderConveyor builderConveyor = this.conveyors[conveyorId];
			if (builderConveyor == null)
			{
				return;
			}
			builderConveyor.SetSelection(setId);
		}

		// Token: 0x06003D4F RID: 15695 RVA: 0x00159F60 File Offset: 0x00158160
		private void SetDispenserSelection(int conveyorId, int setId)
		{
			BuilderDispenserShelf builderDispenserShelf = this.dispenserShelves[conveyorId];
			if (builderDispenserShelf == null)
			{
				return;
			}
			builderDispenserShelf.SetSelection(setId);
		}

		// Token: 0x06003D50 RID: 15696 RVA: 0x00159F8C File Offset: 0x0015818C
		public void ChangeSetSelection(int shelfID, int setID, bool isConveyor)
		{
			BuilderTable.BuilderCommand cmd = new BuilderTable.BuilderCommand
			{
				type = BuilderTable.BuilderCommandType.SetSelection,
				parentPieceId = shelfID,
				pieceType = setID,
				isLeft = isConveyor,
				player = NetworkSystem.Instance.MasterClient
			};
			this.RouteNewCommand(cmd, false);
		}

		// Token: 0x06003D51 RID: 15697 RVA: 0x00159FE0 File Offset: 0x001581E0
		public void ExecuteSetSelection(BuilderTable.BuilderCommand cmd)
		{
			bool isLeft = cmd.isLeft;
			int parentPieceId = cmd.parentPieceId;
			int pieceType = cmd.pieceType;
			if (isLeft)
			{
				this.SetConveyorSelection(parentPieceId, pieceType);
				return;
			}
			this.SetDispenserSelection(parentPieceId, pieceType);
		}

		// Token: 0x06003D52 RID: 15698 RVA: 0x0015A014 File Offset: 0x00158214
		public bool ValidateFunctionalPieceState(int pieceID, byte state, NetPlayer player)
		{
			BuilderPiece piece = this.GetPiece(pieceID);
			return !(piece == null) && piece.functionalPieceComponent != null && (!NetworkSystem.Instance.IsMasterClient || player.IsMasterClient || this.IsPlayerHandNearAction(player, piece.transform.position, true, false, piece.functionalPieceComponent.GetInteractionDistace())) && piece.functionalPieceComponent.IsStateValid(state);
		}

		// Token: 0x06003D53 RID: 15699 RVA: 0x0015A084 File Offset: 0x00158284
		public void OnFunctionalStateRequest(int pieceID, byte state, NetPlayer player, int timeStamp)
		{
			BuilderPiece piece = this.GetPiece(pieceID);
			if (piece == null)
			{
				return;
			}
			if (piece.functionalPieceComponent == null)
			{
				return;
			}
			if (player == null)
			{
				return;
			}
			piece.functionalPieceComponent.OnStateRequest(state, player, timeStamp);
		}

		// Token: 0x06003D54 RID: 15700 RVA: 0x0015A0C0 File Offset: 0x001582C0
		public void SetFunctionalPieceState(int pieceID, byte state, NetPlayer player, int timeStamp)
		{
			BuilderTable.BuilderCommand cmd = new BuilderTable.BuilderCommand
			{
				type = BuilderTable.BuilderCommandType.FunctionalStateChange,
				pieceId = pieceID,
				twist = state,
				player = player,
				serverTimeStamp = timeStamp
			};
			this.RouteNewCommand(cmd, false);
		}

		// Token: 0x06003D55 RID: 15701 RVA: 0x0015A10C File Offset: 0x0015830C
		public void ExecuteSetFunctionalPieceState(BuilderTable.BuilderCommand cmd)
		{
			BuilderPiece piece = this.GetPiece(cmd.pieceId);
			if (piece == null)
			{
				return;
			}
			piece.SetFunctionalPieceState(cmd.twist, cmd.player, cmd.serverTimeStamp);
		}

		// Token: 0x06003D56 RID: 15702 RVA: 0x00057FD8 File Offset: 0x000561D8
		public void RegisterFunctionalPiece(IBuilderPieceFunctional component)
		{
			if (component != null)
			{
				this.funcComponentsToRegister.Add(component);
			}
		}

		// Token: 0x06003D57 RID: 15703 RVA: 0x00057FE9 File Offset: 0x000561E9
		public void UnregisterFunctionalPiece(IBuilderPieceFunctional component)
		{
			if (component != null)
			{
				this.funcComponentsToUnregister.Add(component);
			}
		}

		// Token: 0x06003D58 RID: 15704 RVA: 0x00057FFA File Offset: 0x000561FA
		public void RegisterFunctionalPieceFixedUpdate(IBuilderPieceFunctional component)
		{
			if (component != null)
			{
				this.funcComponentsToRegisterFixed.Add(component);
			}
		}

		// Token: 0x06003D59 RID: 15705 RVA: 0x0005800B File Offset: 0x0005620B
		public void UnregisterFunctionalPieceFixedUpdate(IBuilderPieceFunctional component)
		{
			if (component != null)
			{
				this.funcComponentsToRegisterFixed.Remove(component);
			}
		}

		// Token: 0x06003D5A RID: 15706 RVA: 0x00030607 File Offset: 0x0002E807
		public void RequestCreatePiece(int newPieceType, Vector3 position, Quaternion rotation, int materialType)
		{
		}

		// Token: 0x06003D5B RID: 15707 RVA: 0x0015A148 File Offset: 0x00158348
		public void CreatePiece(int pieceType, int pieceId, Vector3 position, Quaternion rotation, int materialType, BuilderPiece.State state, Player player)
		{
			BuilderTable.BuilderCommand cmd = new BuilderTable.BuilderCommand
			{
				type = BuilderTable.BuilderCommandType.Create,
				pieceType = pieceType,
				pieceId = pieceId,
				localPosition = position,
				localRotation = rotation,
				materialType = materialType,
				state = state,
				player = NetPlayer.Get(player)
			};
			this.RouteNewCommand(cmd, false);
		}

		// Token: 0x06003D5C RID: 15708 RVA: 0x0005801D File Offset: 0x0005621D
		public void RequestRecyclePiece(BuilderPiece piece, bool playFX, int recyclerID)
		{
			this.builderNetworking.RequestRecyclePiece(piece.pieceId, piece.transform.position, piece.transform.rotation, playFX, recyclerID);
		}

		// Token: 0x06003D5D RID: 15709 RVA: 0x0015A1B0 File Offset: 0x001583B0
		public void RecyclePiece(int pieceId, Vector3 position, Quaternion rotation, bool playFX, int recyclerID, Player player)
		{
			BuilderTable.BuilderCommand cmd = new BuilderTable.BuilderCommand
			{
				type = BuilderTable.BuilderCommandType.Recycle,
				pieceId = pieceId,
				localPosition = position,
				localRotation = rotation,
				player = NetPlayer.Get(player),
				isLeft = playFX,
				parentPieceId = recyclerID
			};
			this.RouteNewCommand(cmd, false);
		}

		// Token: 0x06003D5E RID: 15710 RVA: 0x00058048 File Offset: 0x00056248
		private bool ShouldExecuteCommand()
		{
			return this.tableState == BuilderTable.TableState.Ready || this.tableState == BuilderTable.TableState.WaitForInitialBuildMaster;
		}

		// Token: 0x06003D5F RID: 15711 RVA: 0x0005805E File Offset: 0x0005625E
		private bool ShouldQueueCommand()
		{
			return this.tableState == BuilderTable.TableState.ReceivingInitialBuild || this.tableState == BuilderTable.TableState.ReceivingMasterResync || this.tableState == BuilderTable.TableState.InitialBuild || this.tableState == BuilderTable.TableState.ExecuteQueuedCommands;
		}

		// Token: 0x06003D60 RID: 15712 RVA: 0x00058086 File Offset: 0x00056286
		private bool ShouldDiscardCommand()
		{
			return this.tableState == BuilderTable.TableState.WaitingForInitalBuild || this.tableState == BuilderTable.TableState.WaitForInitialBuildMaster || this.tableState == BuilderTable.TableState.WaitingForZoneAndRoom;
		}

		// Token: 0x06003D61 RID: 15713 RVA: 0x0015A210 File Offset: 0x00158410
		public bool DoesChainContainPiece(BuilderPiece targetPiece, BuilderPiece firstInChain, BuilderPiece nextInChain)
		{
			return !(targetPiece == null) && !(firstInChain == null) && (targetPiece.Equals(firstInChain) || (!(nextInChain == null) && (targetPiece.Equals(nextInChain) || (!(firstInChain == nextInChain) && this.DoesChainContainPiece(targetPiece, firstInChain, nextInChain.parentPiece)))));
		}

		// Token: 0x06003D62 RID: 15714 RVA: 0x0015A26C File Offset: 0x0015846C
		public bool DoesChainContainChain(BuilderPiece chainARoot, BuilderPiece chainBAttachPiece)
		{
			if (chainARoot == null || chainBAttachPiece == null)
			{
				return false;
			}
			if (this.DoesChainContainPiece(chainARoot, chainBAttachPiece, chainBAttachPiece.parentPiece))
			{
				return true;
			}
			BuilderPiece builderPiece = chainARoot.firstChildPiece;
			while (builderPiece != null)
			{
				if (this.DoesChainContainChain(builderPiece, chainBAttachPiece))
				{
					return true;
				}
				builderPiece = builderPiece.nextSiblingPiece;
			}
			return false;
		}

		// Token: 0x06003D63 RID: 15715 RVA: 0x0015A2C8 File Offset: 0x001584C8
		private bool IsPlayerHandNearAction(NetPlayer player, Vector3 worldPosition, bool isLeftHand, bool checkBothHands, float acceptableRadius = 2.5f)
		{
			bool flag = true;
			RigContainer rigContainer;
			if (player != null && VRRigCache.Instance != null && VRRigCache.Instance.TryGetVrrig(player, out rigContainer))
			{
				if (isLeftHand || checkBothHands)
				{
					flag = ((worldPosition - rigContainer.Rig.leftHandTransform.position).sqrMagnitude < acceptableRadius * acceptableRadius);
				}
				if (!isLeftHand || checkBothHands)
				{
					float sqrMagnitude = (worldPosition - rigContainer.Rig.rightHandTransform.position).sqrMagnitude;
					flag = (flag && sqrMagnitude < acceptableRadius * acceptableRadius);
				}
			}
			return flag;
		}

		// Token: 0x06003D64 RID: 15716 RVA: 0x0015A35C File Offset: 0x0015855C
		public bool ValidatePlacePieceParams(int pieceId, int attachPieceId, sbyte bumpOffsetX, sbyte bumpOffsetZ, byte twist, int parentPieceId, int attachIndex, int parentAttachIndex, NetPlayer placedByPlayer)
		{
			BuilderPiece piece = this.GetPiece(pieceId);
			if (piece == null)
			{
				return false;
			}
			BuilderPiece piece2 = this.GetPiece(attachPieceId);
			if (piece2 == null)
			{
				return false;
			}
			if (piece.heldByPlayerActorNumber != placedByPlayer.ActorNumber)
			{
				return false;
			}
			if (piece.isBuiltIntoTable || piece2.isBuiltIntoTable)
			{
				return false;
			}
			if (twist > 3)
			{
				return false;
			}
			BuilderPiece piece3 = this.GetPiece(parentPieceId);
			if (!(piece3 != null))
			{
				return false;
			}
			if (!BuilderPiece.CanPlayerAttachPieceToPiece(placedByPlayer.ActorNumber, piece2, piece3))
			{
				return false;
			}
			if (this.DoesChainContainChain(piece2, piece3))
			{
				return false;
			}
			if (attachIndex < 0 || attachIndex >= piece2.gridPlanes.Count)
			{
				return false;
			}
			if (piece3 != null && (parentAttachIndex < 0 || parentAttachIndex >= piece3.gridPlanes.Count))
			{
				return false;
			}
			if (piece3 != null)
			{
				bool flag = (long)(twist % 2) == 1L;
				BuilderAttachGridPlane builderAttachGridPlane = piece2.gridPlanes[attachIndex];
				int num = flag ? builderAttachGridPlane.length : builderAttachGridPlane.width;
				int num2 = flag ? builderAttachGridPlane.width : builderAttachGridPlane.length;
				BuilderAttachGridPlane builderAttachGridPlane2 = piece3.gridPlanes[parentAttachIndex];
				int num3 = Mathf.FloorToInt((float)builderAttachGridPlane2.width / 2f);
				int num4 = num3 - (builderAttachGridPlane2.width - 1);
				if ((int)bumpOffsetX < num4 - num || (int)bumpOffsetX > num3 + num)
				{
					return false;
				}
				int num5 = Mathf.FloorToInt((float)builderAttachGridPlane2.length / 2f);
				int num6 = num5 - (builderAttachGridPlane2.length - 1);
				if ((int)bumpOffsetZ < num6 - num2 || (int)bumpOffsetZ > num5 + num2)
				{
					return false;
				}
			}
			if (placedByPlayer == null)
			{
				return false;
			}
			if (PhotonNetwork.IsMasterClient && piece3 != null)
			{
				Vector3 vector;
				Quaternion quaternion;
				Vector3 vector2;
				Quaternion rotation;
				piece2.BumpTwistToPositionRotation(twist, bumpOffsetX, bumpOffsetZ, attachIndex, piece3.gridPlanes[parentAttachIndex], out vector, out quaternion, out vector2, out rotation);
				Vector3 point = piece2.transform.InverseTransformPoint(piece.transform.position);
				Vector3 worldPosition = vector2 + rotation * point;
				if (!this.IsPlayerHandNearAction(placedByPlayer, worldPosition, piece.heldInLeftHand, false, 2.5f))
				{
					return false;
				}
				if (!this.ValidatePieceWorldTransform(vector2, rotation))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003D65 RID: 15717 RVA: 0x0015A570 File Offset: 0x00158770
		public bool ValidatePlacePieceState(int pieceId, int attachPieceId, sbyte bumpOffsetX, sbyte bumpOffsetZ, byte twist, int parentPieceId, int attachIndex, int parentAttachIndex, Player placedByPlayer)
		{
			BuilderPiece piece = this.GetPiece(pieceId);
			if (piece == null)
			{
				return false;
			}
			BuilderPiece piece2 = this.GetPiece(attachPieceId);
			return !(piece2 == null) && !(this.GetPiece(parentPieceId) == null) && placedByPlayer != null && !piece2.GetRootPiece() != piece;
		}

		// Token: 0x06003D66 RID: 15718 RVA: 0x0015A5D4 File Offset: 0x001587D4
		public void ExecutePieceCreated(BuilderTable.BuilderCommand cmd)
		{
			if ((cmd.player == null || !cmd.player.IsLocal) && !this.ValidateCreatePieceParams(cmd.pieceType, cmd.pieceId, cmd.state, cmd.materialType))
			{
				return;
			}
			BuilderPiece builderPiece = this.CreatePieceInternal(cmd.pieceType, cmd.pieceId, cmd.localPosition, cmd.localRotation, cmd.state, cmd.materialType, 0);
			if (!(builderPiece != null) || cmd.state != BuilderPiece.State.OnConveyor)
			{
				if (builderPiece != null && cmd.isLeft && cmd.state == BuilderPiece.State.OnShelf)
				{
					if (cmd.parentPieceId < 0 || cmd.parentPieceId >= this.dispenserShelves.Count)
					{
						return;
					}
					builderPiece.shelfOwner = cmd.parentPieceId;
					this.dispenserShelves[builderPiece.shelfOwner].OnShelfPieceCreated(builderPiece, true);
				}
				return;
			}
			if (cmd.parentPieceId < 0 || cmd.parentPieceId >= this.conveyors.Count)
			{
				return;
			}
			builderPiece.shelfOwner = cmd.parentPieceId;
			BuilderConveyor builderConveyor = this.conveyors[builderPiece.shelfOwner];
			int parentAttachIndex = cmd.parentAttachIndex;
			float timeOffset = 0f;
			if (PhotonNetwork.ServerTimestamp > parentAttachIndex)
			{
				timeOffset = (PhotonNetwork.ServerTimestamp - parentAttachIndex) / 1000f;
			}
			builderConveyor.OnShelfPieceCreated(builderPiece, timeOffset);
		}

		// Token: 0x06003D67 RID: 15719 RVA: 0x000580A5 File Offset: 0x000562A5
		public void ExecutePieceRecycled(BuilderTable.BuilderCommand cmd)
		{
			this.RecyclePieceInternal(cmd.pieceId, false, cmd.isLeft, cmd.parentPieceId);
		}

		// Token: 0x06003D68 RID: 15720 RVA: 0x000580C0 File Offset: 0x000562C0
		private bool ValidateCreatePieceParams(int newPieceType, int newPieceId, BuilderPiece.State state, int materialType)
		{
			return !(this.GetPiecePrefab(newPieceType) == null) && !(this.GetPiece(newPieceId) != null);
		}

		// Token: 0x06003D69 RID: 15721 RVA: 0x0015A71C File Offset: 0x0015891C
		private bool ValidateDeserializedRootPieceState(int pieceId, BuilderPiece.State state, int shelfOwner, int heldByActor, Vector3 localPosition, Quaternion localRotation)
		{
			switch (state)
			{
			case BuilderPiece.State.Grabbed:
			case BuilderPiece.State.GrabbedLocal:
				if (heldByActor == -1)
				{
					return false;
				}
				if (localPosition.sqrMagnitude > 6.25f)
				{
					return false;
				}
				break;
			case BuilderPiece.State.Dropped:
				if (!this.ValidatePieceWorldTransform(localPosition, localRotation))
				{
					return false;
				}
				break;
			case BuilderPiece.State.OnShelf:
			case BuilderPiece.State.Displayed:
				if (shelfOwner == -1 && !this.ValidatePieceWorldTransform(localPosition, localRotation))
				{
					return false;
				}
				break;
			case BuilderPiece.State.OnConveyor:
				if (shelfOwner == -1)
				{
					return false;
				}
				break;
			case BuilderPiece.State.AttachedToArm:
				if (heldByActor == -1)
				{
					return false;
				}
				if (localPosition.sqrMagnitude > 6.25f)
				{
					return false;
				}
				break;
			default:
				return false;
			}
			return true;
		}

		// Token: 0x06003D6A RID: 15722 RVA: 0x000580E5 File Offset: 0x000562E5
		private bool ValidateDeserializedChildPieceState(int pieceId, BuilderPiece.State state)
		{
			switch (state)
			{
			case BuilderPiece.State.AttachedAndPlaced:
			case BuilderPiece.State.AttachedToDropped:
			case BuilderPiece.State.Grabbed:
			case BuilderPiece.State.OnShelf:
			case BuilderPiece.State.Displayed:
			case BuilderPiece.State.GrabbedLocal:
			case BuilderPiece.State.AttachedToArm:
				return true;
			}
			return false;
		}

		// Token: 0x06003D6B RID: 15723 RVA: 0x0015A7A8 File Offset: 0x001589A8
		public bool ValidatePieceWorldTransform(Vector3 position, Quaternion rotation)
		{
			float num = 10000f;
			return position.IsValid(num) && rotation.IsValid() && (this.roomCenter.position - position).sqrMagnitude <= 169f;
		}

		// Token: 0x06003D6C RID: 15724 RVA: 0x0015A7F4 File Offset: 0x001589F4
		private BuilderPiece CreatePieceInternal(int newPieceType, int newPieceId, Vector3 position, Quaternion rotation, BuilderPiece.State state, int materialType, int activateTimeStamp)
		{
			if (this.GetPiecePrefab(newPieceType) == null)
			{
				return null;
			}
			if (!PhotonNetwork.IsMasterClient)
			{
				this.nextPieceId = newPieceId + 1;
			}
			BuilderPiece builderPiece = this.builderPool.CreatePiece(newPieceType, false);
			builderPiece.SetScale(this.pieceScale);
			builderPiece.transform.SetPositionAndRotation(position, rotation);
			builderPiece.pieceType = newPieceType;
			builderPiece.pieceId = newPieceId;
			builderPiece.gameObject.SetActive(true);
			builderPiece.SetupPiece(this.gridSize);
			builderPiece.OnCreate();
			builderPiece.activatedTimeStamp = ((state == BuilderPiece.State.AttachedAndPlaced) ? activateTimeStamp : 0);
			builderPiece.SetMaterial(materialType, true);
			builderPiece.SetState(state, true);
			this.AddPiece(builderPiece);
			return builderPiece;
		}

		// Token: 0x06003D6D RID: 15725 RVA: 0x0015A8A0 File Offset: 0x00158AA0
		private void RecyclePieceInternal(int pieceId, bool ignoreHaptics, bool playFX, int recyclerId)
		{
			BuilderPiece piece = this.GetPiece(pieceId);
			if (piece == null)
			{
				return;
			}
			if (playFX)
			{
				try
				{
					piece.PlayRecycleFx();
				}
				catch (Exception)
				{
				}
			}
			if (!ignoreHaptics)
			{
				BuilderPiece rootPiece = piece.GetRootPiece();
				if (rootPiece != null && rootPiece.IsHeldLocal())
				{
					GorillaTagger.Instance.StartVibration(piece.IsHeldInLeftHand(), GorillaTagger.Instance.tapHapticStrength, BuilderTable.instance.pushAndEaseParams.snapDelayTime * 2f);
				}
			}
			BuilderPiece builderPiece = piece.firstChildPiece;
			while (builderPiece != null)
			{
				int pieceId2 = builderPiece.pieceId;
				builderPiece = builderPiece.nextSiblingPiece;
				this.RecyclePieceInternal(pieceId2, true, playFX, recyclerId);
			}
			if (recyclerId >= 0 && recyclerId < this.recyclers.Count)
			{
				this.recyclers[recyclerId].OnRecycleRequestedAtRecycler(piece);
			}
			if (piece.state == BuilderPiece.State.OnConveyor && piece.shelfOwner >= 0 && piece.shelfOwner < this.conveyors.Count)
			{
				this.conveyors[piece.shelfOwner].OnShelfPieceRecycled(piece);
			}
			else if ((piece.state == BuilderPiece.State.OnShelf || piece.state == BuilderPiece.State.Displayed) && piece.shelfOwner >= 0 && piece.shelfOwner < this.dispenserShelves.Count)
			{
				this.dispenserShelves[piece.shelfOwner].OnShelfPieceRecycled(piece);
			}
			if (piece.isArmShelf)
			{
				if (piece.armShelf != null)
				{
					piece.armShelf.piece = null;
					piece.armShelf = null;
				}
				int num;
				if (piece.heldInLeftHand && this.playerToArmShelfLeft.TryGetValue(piece.heldByPlayerActorNumber, out num) && num == piece.pieceId)
				{
					this.playerToArmShelfLeft.Remove(piece.heldByPlayerActorNumber);
				}
				int num2;
				if (!piece.heldInLeftHand && this.playerToArmShelfRight.TryGetValue(piece.heldByPlayerActorNumber, out num2) && num2 == piece.pieceId)
				{
					this.playerToArmShelfRight.Remove(piece.heldByPlayerActorNumber);
				}
			}
			else if (PhotonNetwork.LocalPlayer.ActorNumber == piece.heldByPlayerActorNumber)
			{
				BuilderPieceInteractor.instance.RemovePieceFromHeld(piece);
			}
			int pieceId3 = piece.pieceId;
			piece.ClearParentPiece(false);
			piece.ClearParentHeld();
			piece.SetState(BuilderPiece.State.None, false);
			this.RemovePiece(piece);
			this.builderPool.DestroyPiece(piece);
		}

		// Token: 0x06003D6E RID: 15726 RVA: 0x00058116 File Offset: 0x00056316
		public BuilderPiece GetPiecePrefab(int pieceType)
		{
			return this.factories[0].GetPiecePrefab(pieceType);
		}

		// Token: 0x06003D6F RID: 15727 RVA: 0x0015AAEC File Offset: 0x00158CEC
		private bool ValidateAttachPieceParams(int pieceId, int attachIndex, int parentId, int parentAttachIndex, int piecePlacement)
		{
			BuilderPiece piece = this.GetPiece(pieceId);
			if (piece == null)
			{
				return false;
			}
			BuilderPiece piece2 = this.GetPiece(parentId);
			if (piece2 == null)
			{
				return false;
			}
			if ((piecePlacement & 262143) != piecePlacement)
			{
				return false;
			}
			if (piece.isBuiltIntoTable)
			{
				return false;
			}
			if (this.DoesChainContainChain(piece, piece2))
			{
				return false;
			}
			if (attachIndex < 0 || attachIndex >= piece.gridPlanes.Count)
			{
				return false;
			}
			if (parentAttachIndex < 0 || parentAttachIndex >= piece2.gridPlanes.Count)
			{
				return false;
			}
			byte b;
			sbyte b2;
			sbyte b3;
			BuilderTable.UnpackPiecePlacement(piecePlacement, out b, out b2, out b3);
			bool flag = (long)(b % 2) == 1L;
			BuilderAttachGridPlane builderAttachGridPlane = piece.gridPlanes[attachIndex];
			int num = flag ? builderAttachGridPlane.length : builderAttachGridPlane.width;
			int num2 = flag ? builderAttachGridPlane.width : builderAttachGridPlane.length;
			BuilderAttachGridPlane builderAttachGridPlane2 = piece2.gridPlanes[parentAttachIndex];
			int num3 = Mathf.FloorToInt((float)builderAttachGridPlane2.width / 2f);
			int num4 = num3 - (builderAttachGridPlane2.width - 1);
			if ((int)b2 < num4 - num || (int)b2 > num3 + num)
			{
				return false;
			}
			int num5 = Mathf.FloorToInt((float)builderAttachGridPlane2.length / 2f);
			int num6 = num5 - (builderAttachGridPlane2.length - 1);
			return (int)b3 >= num6 - num2 && (int)b3 <= num5 + num2;
		}

		// Token: 0x06003D70 RID: 15728 RVA: 0x0015AC38 File Offset: 0x00158E38
		private void AttachPieceInternal(int pieceId, int attachIndex, int parentId, int parentAttachIndex, int placement)
		{
			BuilderPiece piece = this.GetPiece(pieceId);
			BuilderPiece piece2 = this.GetPiece(parentId);
			if (piece == null)
			{
				return;
			}
			byte b;
			sbyte xOffset;
			sbyte zOffset;
			BuilderTable.UnpackPiecePlacement(placement, out b, out xOffset, out zOffset);
			Vector3 zero = Vector3.zero;
			Quaternion localRotation;
			if (piece2 != null && parentAttachIndex >= 0 && parentAttachIndex < piece2.gridPlanes.Count)
			{
				Vector3 vector;
				Quaternion quaternion;
				piece.BumpTwistToPositionRotation(b, xOffset, zOffset, attachIndex, piece2.gridPlanes[parentAttachIndex], out zero, out localRotation, out vector, out quaternion);
			}
			else
			{
				localRotation = Quaternion.Euler(0f, (float)b * 90f, 0f);
			}
			piece.SetParentPiece(attachIndex, piece2, parentAttachIndex);
			piece.transform.SetLocalPositionAndRotation(zero, localRotation);
		}

		// Token: 0x06003D71 RID: 15729 RVA: 0x0015ACE4 File Offset: 0x00158EE4
		private void AttachPieceToActorInternal(int pieceId, int actorNumber, bool isLeftHand)
		{
			BuilderPiece piece = this.GetPiece(pieceId);
			if (piece == null)
			{
				return;
			}
			NetPlayer player = NetworkSystem.Instance.GetPlayer(actorNumber);
			RigContainer rigContainer;
			if (!VRRigCache.Instance.TryGetVrrig(player, out rigContainer))
			{
				return;
			}
			VRRig rig = rigContainer.Rig;
			BodyDockPositions myBodyDockPositions = rig.myBodyDockPositions;
			Transform parentHeld = isLeftHand ? myBodyDockPositions.leftHandTransform : myBodyDockPositions.rightHandTransform;
			if (piece.isArmShelf)
			{
				parentHeld = (isLeftHand ? rig.builderArmShelfLeft.pieceAnchor : rig.builderArmShelfRight.pieceAnchor);
				if (isLeftHand)
				{
					rig.builderArmShelfLeft.piece = piece;
					piece.armShelf = rig.builderArmShelfLeft;
					int num;
					if (this.playerToArmShelfLeft.TryGetValue(actorNumber, out num) && num != pieceId)
					{
						BuilderPiece piece2 = this.GetPiece(num);
						if (piece2 != null && piece2.isArmShelf)
						{
							piece2.ClearParentHeld();
							this.playerToArmShelfLeft.Remove(actorNumber);
						}
					}
					this.playerToArmShelfLeft.TryAdd(actorNumber, pieceId);
				}
				else
				{
					rig.builderArmShelfRight.piece = piece;
					piece.armShelf = rig.builderArmShelfRight;
					int num2;
					if (this.playerToArmShelfRight.TryGetValue(actorNumber, out num2) && num2 != pieceId)
					{
						BuilderPiece piece3 = this.GetPiece(num2);
						if (piece3 != null && piece3.isArmShelf)
						{
							piece3.ClearParentHeld();
							this.playerToArmShelfRight.Remove(actorNumber);
						}
					}
					this.playerToArmShelfRight.TryAdd(actorNumber, pieceId);
				}
			}
			Vector3 localPosition = piece.transform.localPosition;
			Quaternion localRotation = piece.transform.localRotation;
			piece.ClearParentHeld();
			piece.ClearParentPiece(false);
			piece.SetParentHeld(parentHeld, actorNumber, isLeftHand);
			piece.transform.SetLocalPositionAndRotation(localPosition, localRotation);
			BuilderPiece.State newState = player.IsLocal ? BuilderPiece.State.GrabbedLocal : BuilderPiece.State.Grabbed;
			if (piece.isArmShelf)
			{
				newState = BuilderPiece.State.AttachedToArm;
				piece.transform.localScale = Vector3.one;
			}
			piece.SetState(newState, false);
			if (!player.IsLocal)
			{
				BuilderPieceInteractor.instance.RemovePieceFromHeld(piece);
			}
			if (player.IsLocal && !piece.isArmShelf)
			{
				BuilderPieceInteractor.instance.AddPieceToHeld(piece, isLeftHand, localPosition, localRotation);
			}
		}

		// Token: 0x06003D72 RID: 15730 RVA: 0x0015AEF4 File Offset: 0x001590F4
		public void RequestPlacePiece(BuilderPiece piece, BuilderPiece attachPiece, sbyte bumpOffsetX, sbyte bumpOffsetZ, byte twist, BuilderPiece parentPiece, int attachIndex, int parentAttachIndex)
		{
			if (this.tableState != BuilderTable.TableState.Ready)
			{
				return;
			}
			this.builderNetworking.RequestPlacePiece(piece, attachPiece, bumpOffsetX, bumpOffsetZ, twist, parentPiece, attachIndex, parentAttachIndex);
		}

		// Token: 0x06003D73 RID: 15731 RVA: 0x0015AF24 File Offset: 0x00159124
		public void PlacePiece(int localCommandId, int pieceId, int attachPieceId, sbyte bumpOffsetX, sbyte bumpOffsetZ, byte twist, int parentPieceId, int attachIndex, int parentAttachIndex, NetPlayer placedByPlayer, int timeStamp, bool force)
		{
			this.PiecePlacedInternal(localCommandId, pieceId, attachPieceId, bumpOffsetX, bumpOffsetZ, twist, parentPieceId, attachIndex, parentAttachIndex, placedByPlayer, timeStamp, force);
		}

		// Token: 0x06003D74 RID: 15732 RVA: 0x0015AF4C File Offset: 0x0015914C
		public void PiecePlacedInternal(int localCommandId, int pieceId, int attachPieceId, sbyte bumpOffsetX, sbyte bumpOffsetZ, byte twist, int parentPieceId, int attachIndex, int parentAttachIndex, NetPlayer placedByPlayer, int timeStamp, bool force)
		{
			if (!force && placedByPlayer == NetworkSystem.Instance.LocalPlayer && this.HasRollForwardCommand(localCommandId) && this.TryRollbackAndReExecute(localCommandId))
			{
				return;
			}
			BuilderTable.BuilderCommand cmd = new BuilderTable.BuilderCommand
			{
				type = BuilderTable.BuilderCommandType.Place,
				pieceId = pieceId,
				bumpOffsetX = bumpOffsetX,
				bumpOffsetZ = bumpOffsetZ,
				twist = twist,
				attachPieceId = attachPieceId,
				parentPieceId = parentPieceId,
				attachIndex = attachIndex,
				parentAttachIndex = parentAttachIndex,
				player = placedByPlayer,
				canRollback = force,
				localCommandId = localCommandId,
				serverTimeStamp = timeStamp
			};
			this.RouteNewCommand(cmd, force);
		}

		// Token: 0x06003D75 RID: 15733 RVA: 0x0015B004 File Offset: 0x00159204
		public void ExecutePiecePlacedWithActions(BuilderTable.BuilderCommand cmd)
		{
			int pieceId = cmd.pieceId;
			int attachPieceId = cmd.attachPieceId;
			int parentPieceId = cmd.parentPieceId;
			int parentAttachIndex = cmd.parentAttachIndex;
			int attachIndex = cmd.attachIndex;
			NetPlayer player = cmd.player;
			int localCommandId = cmd.localCommandId;
			int actorNumber = cmd.player.ActorNumber;
			byte twist = cmd.twist;
			sbyte bumpOffsetX = cmd.bumpOffsetX;
			sbyte bumpOffsetZ = cmd.bumpOffsetZ;
			if ((player == null || !player.IsLocal) && !this.ValidatePlacePieceParams(pieceId, attachPieceId, bumpOffsetX, bumpOffsetZ, twist, parentPieceId, attachIndex, parentAttachIndex, player))
			{
				return;
			}
			BuilderPiece piece = this.GetPiece(pieceId);
			if (piece == null)
			{
				return;
			}
			BuilderPiece piece2 = this.GetPiece(attachPieceId);
			if (piece2 == null)
			{
				return;
			}
			BuilderAction action = BuilderActions.CreateDetachFromPlayer(localCommandId, pieceId, actorNumber);
			BuilderAction action2 = BuilderActions.CreateMakeRoot(localCommandId, attachPieceId);
			BuilderAction action3 = BuilderActions.CreateAttachToPiece(localCommandId, attachPieceId, cmd.parentPieceId, cmd.attachIndex, cmd.parentAttachIndex, bumpOffsetX, bumpOffsetZ, twist, actorNumber, cmd.serverTimeStamp);
			if (cmd.canRollback)
			{
				BuilderAction action4 = BuilderActions.CreateDetachFromPiece(localCommandId, attachPieceId, actorNumber);
				BuilderAction action5 = BuilderActions.CreateMakeRoot(localCommandId, pieceId);
				BuilderAction action6 = BuilderActions.CreateAttachToPlayerRollback(localCommandId, piece);
				this.AddRollbackAction(action6);
				this.AddRollbackAction(action5);
				this.AddRollbackAction(action4);
				this.AddRollForwardCommand(cmd);
			}
			this.ExecuteAction(action);
			this.ExecuteAction(action2);
			this.ExecuteAction(action3);
			if (!cmd.isQueued)
			{
				piece2.PlayPlacementFx();
			}
		}

		// Token: 0x06003D76 RID: 15734 RVA: 0x0015B168 File Offset: 0x00159368
		public bool ValidateGrabPieceParams(int pieceId, bool isLeftHand, Vector3 localPosition, Quaternion localRotation, NetPlayer grabbedByPlayer)
		{
			float num = 10000f;
			if (!localPosition.IsValid(num) || !localRotation.IsValid())
			{
				return false;
			}
			BuilderPiece piece = this.GetPiece(pieceId);
			if (piece == null)
			{
				return false;
			}
			if (piece.isBuiltIntoTable)
			{
				return false;
			}
			if (grabbedByPlayer == null)
			{
				return false;
			}
			if (!piece.CanPlayerGrabPiece(grabbedByPlayer.ActorNumber, piece.transform.position))
			{
				return false;
			}
			if (localPosition.sqrMagnitude > 6400f)
			{
				return false;
			}
			if (PhotonNetwork.IsMasterClient)
			{
				Vector3 position = piece.transform.position;
				if (!this.IsPlayerHandNearAction(grabbedByPlayer, position, isLeftHand, false, 2.5f))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003D77 RID: 15735 RVA: 0x0015B208 File Offset: 0x00159408
		public bool ValidateGrabPieceState(int pieceId, bool isLeftHand, Vector3 localPosition, Quaternion localRotation, Player grabbedByPlayer)
		{
			BuilderPiece piece = this.GetPiece(pieceId);
			return !(piece == null) && piece.state != BuilderPiece.State.Displayed && piece.state != BuilderPiece.State.None;
		}

		// Token: 0x06003D78 RID: 15736 RVA: 0x0015B240 File Offset: 0x00159440
		public bool IsLocationWithinSharedBuildArea(Vector3 worldPosition)
		{
			Vector3 vector = this.sharedBuildArea.transform.InverseTransformPoint(worldPosition);
			foreach (BoxCollider boxCollider in this.sharedBuildAreas)
			{
				Vector3 vector2 = boxCollider.center + boxCollider.size / 2f;
				Vector3 vector3 = boxCollider.center - boxCollider.size / 2f;
				if (vector.x >= vector3.x && vector.x <= vector2.x && vector.y >= vector3.y && vector.y <= vector2.y && vector.z >= vector3.z && vector.z <= vector2.z)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003D79 RID: 15737 RVA: 0x0005812A File Offset: 0x0005632A
		public void RequestGrabPiece(BuilderPiece piece, bool isLefHand, Vector3 localPosition, Quaternion localRotation)
		{
			if (this.tableState != BuilderTable.TableState.Ready)
			{
				return;
			}
			this.builderNetworking.RequestGrabPiece(piece, isLefHand, localPosition, localRotation);
		}

		// Token: 0x06003D7A RID: 15738 RVA: 0x00058146 File Offset: 0x00056346
		public void GrabPiece(int localCommandId, int pieceId, bool isLeftHand, Vector3 localPosition, Quaternion localRotation, NetPlayer grabbedByPlayer, bool force)
		{
			this.PieceGrabbedInternal(localCommandId, pieceId, isLeftHand, localPosition, localRotation, grabbedByPlayer, force);
		}

		// Token: 0x06003D7B RID: 15739 RVA: 0x0015B320 File Offset: 0x00159520
		public void PieceGrabbedInternal(int localCommandId, int pieceId, bool isLeftHand, Vector3 localPosition, Quaternion localRotation, NetPlayer grabbedByPlayer, bool force)
		{
			if (!force && grabbedByPlayer == NetworkSystem.Instance.LocalPlayer && this.HasRollForwardCommand(localCommandId) && this.TryRollbackAndReExecute(localCommandId))
			{
				return;
			}
			BuilderTable.BuilderCommand cmd = new BuilderTable.BuilderCommand
			{
				type = BuilderTable.BuilderCommandType.Grab,
				pieceId = pieceId,
				attachPieceId = -1,
				isLeft = isLeftHand,
				localPosition = localPosition,
				localRotation = localRotation,
				player = grabbedByPlayer,
				canRollback = force,
				localCommandId = localCommandId
			};
			this.RouteNewCommand(cmd, force);
		}

		// Token: 0x06003D7C RID: 15740 RVA: 0x0015B3B4 File Offset: 0x001595B4
		public void ExecutePieceGrabbedWithActions(BuilderTable.BuilderCommand cmd)
		{
			int pieceId = cmd.pieceId;
			bool isLeft = cmd.isLeft;
			NetPlayer player = cmd.player;
			Vector3 localPosition = cmd.localPosition;
			Quaternion localRotation = cmd.localRotation;
			int localCommandId = cmd.localCommandId;
			int actorNumber = cmd.player.ActorNumber;
			if ((player == null || !player.Equals(NetworkSystem.Instance.LocalPlayer)) && !this.ValidateGrabPieceParams(pieceId, isLeft, localPosition, localRotation, player))
			{
				return;
			}
			BuilderPiece piece = this.GetPiece(pieceId);
			if (piece == null)
			{
				return;
			}
			bool flag = PhotonNetwork.CurrentRoom.GetPlayer(piece.heldByPlayerActorNumber, false) != null;
			bool flag2 = BuilderPiece.IsDroppedState(piece.state);
			bool flag3 = piece.state == BuilderPiece.State.OnConveyor || piece.state == BuilderPiece.State.OnShelf || piece.state == BuilderPiece.State.Displayed;
			BuilderAction action = BuilderActions.CreateAttachToPlayer(localCommandId, pieceId, cmd.localPosition, cmd.localRotation, actorNumber, cmd.isLeft);
			BuilderAction action2 = BuilderActions.CreateDetachFromPlayer(localCommandId, pieceId, actorNumber);
			if (flag)
			{
				BuilderAction action3 = BuilderActions.CreateDetachFromPlayer(localCommandId, pieceId, piece.heldByPlayerActorNumber);
				if (cmd.canRollback)
				{
					BuilderAction action4 = BuilderActions.CreateAttachToPlayerRollback(localCommandId, piece);
					this.AddRollbackAction(action4);
					this.AddRollbackAction(action2);
					this.AddRollForwardCommand(cmd);
				}
				this.ExecuteAction(action3);
				this.ExecuteAction(action);
				return;
			}
			if (flag3)
			{
				BuilderAction action5;
				if (piece.state == BuilderPiece.State.OnConveyor)
				{
					int serverTimestamp = PhotonNetwork.ServerTimestamp;
					float splineProgressForPiece = this.conveyorManager.GetSplineProgressForPiece(piece);
					action5 = BuilderActions.CreateAttachToShelfRollback(localCommandId, piece, piece.shelfOwner, true, serverTimestamp, splineProgressForPiece);
				}
				else
				{
					if (piece.state == BuilderPiece.State.Displayed)
					{
						int actorNumber2 = NetworkSystem.Instance.LocalPlayer.ActorNumber;
					}
					action5 = BuilderActions.CreateAttachToShelfRollback(localCommandId, piece, piece.shelfOwner, false, 0, 0f);
				}
				BuilderAction action6 = BuilderActions.CreateMakeRoot(localCommandId, pieceId);
				BuilderPiece rootPiece = piece.GetRootPiece();
				BuilderAction action7 = BuilderActions.CreateMakeRoot(localCommandId, rootPiece.pieceId);
				if (cmd.canRollback)
				{
					this.AddRollbackAction(action5);
					this.AddRollbackAction(action7);
					this.AddRollbackAction(action2);
					this.AddRollForwardCommand(cmd);
				}
				this.ExecuteAction(action6);
				this.ExecuteAction(action);
				return;
			}
			if (flag2)
			{
				BuilderAction action8 = BuilderActions.CreateMakeRoot(localCommandId, pieceId);
				BuilderPiece rootPiece2 = piece.GetRootPiece();
				BuilderAction action9 = BuilderActions.CreateDropPieceRollback(localCommandId, rootPiece2, actorNumber);
				BuilderAction action10 = BuilderActions.CreateMakeRoot(localCommandId, rootPiece2.pieceId);
				if (cmd.canRollback)
				{
					this.AddRollbackAction(action9);
					this.AddRollbackAction(action10);
					this.AddRollbackAction(action2);
					this.AddRollForwardCommand(cmd);
				}
				this.ExecuteAction(action8);
				this.ExecuteAction(action);
				return;
			}
			if (piece.parentPiece != null)
			{
				BuilderAction action11 = BuilderActions.CreateDetachFromPiece(localCommandId, pieceId, actorNumber);
				BuilderAction action12 = BuilderActions.CreateAttachToPieceRollback(localCommandId, piece, actorNumber);
				if (cmd.canRollback)
				{
					this.AddRollbackAction(action12);
					this.AddRollbackAction(action2);
					this.AddRollForwardCommand(cmd);
				}
				this.ExecuteAction(action11);
				this.ExecuteAction(action);
			}
		}

		// Token: 0x06003D7D RID: 15741 RVA: 0x0015B688 File Offset: 0x00159888
		public bool ValidateDropPieceParams(int pieceId, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angVelocity, NetPlayer droppedByPlayer)
		{
			float num = 10000f;
			if (position.IsValid(num) && rotation.IsValid())
			{
				float num2 = 10000f;
				if (velocity.IsValid(num2))
				{
					float num3 = 10000f;
					if (angVelocity.IsValid(num3))
					{
						BuilderPiece piece = this.GetPiece(pieceId);
						if (piece == null)
						{
							return false;
						}
						if (piece.isBuiltIntoTable)
						{
							return false;
						}
						if (droppedByPlayer == null)
						{
							return false;
						}
						if (velocity.sqrMagnitude > BuilderTable.MAX_DROP_VELOCITY * BuilderTable.MAX_DROP_VELOCITY)
						{
							return false;
						}
						if (angVelocity.sqrMagnitude > BuilderTable.MAX_DROP_ANG_VELOCITY * BuilderTable.MAX_DROP_ANG_VELOCITY)
						{
							return false;
						}
						if ((this.roomCenter.position - position).sqrMagnitude > 169f)
						{
							return false;
						}
						if (piece.state == BuilderPiece.State.AttachedToArm)
						{
							if (piece.parentPiece == null)
							{
								return false;
							}
							if (piece.parentPiece.heldByPlayerActorNumber != droppedByPlayer.ActorNumber)
							{
								return false;
							}
						}
						else if (piece.heldByPlayerActorNumber != droppedByPlayer.ActorNumber)
						{
							return false;
						}
						return !PhotonNetwork.IsMasterClient || this.IsPlayerHandNearAction(droppedByPlayer, position, piece.heldInLeftHand, false, 2.5f);
					}
				}
			}
			return false;
		}

		// Token: 0x06003D7E RID: 15742 RVA: 0x0015B7A8 File Offset: 0x001599A8
		public bool ValidateDropPieceState(int pieceId, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angVelocity, Player droppedByPlayer)
		{
			BuilderPiece piece = this.GetPiece(pieceId);
			if (piece == null)
			{
				return false;
			}
			bool flag = piece.state == BuilderPiece.State.AttachedToArm;
			return (flag || piece.heldByPlayerActorNumber == droppedByPlayer.ActorNumber) && (!flag || piece.parentPiece.heldByPlayerActorNumber == droppedByPlayer.ActorNumber);
		}

		// Token: 0x06003D7F RID: 15743 RVA: 0x00058159 File Offset: 0x00056359
		public void RequestDropPiece(BuilderPiece piece, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angVelocity)
		{
			if (this.tableState != BuilderTable.TableState.Ready)
			{
				return;
			}
			this.builderNetworking.RequestDropPiece(piece, position, rotation, velocity, angVelocity);
		}

		// Token: 0x06003D80 RID: 15744 RVA: 0x0015B800 File Offset: 0x00159A00
		public void DropPiece(int localCommandId, int pieceId, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angVelocity, NetPlayer droppedByPlayer, bool force)
		{
			this.PieceDroppedInternal(localCommandId, pieceId, position, rotation, velocity, angVelocity, droppedByPlayer, force);
		}

		// Token: 0x06003D81 RID: 15745 RVA: 0x0015B820 File Offset: 0x00159A20
		public void PieceDroppedInternal(int localCommandId, int pieceId, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angVelocity, NetPlayer droppedByPlayer, bool force)
		{
			if (!force && droppedByPlayer == NetworkSystem.Instance.LocalPlayer && this.HasRollForwardCommand(localCommandId) && this.TryRollbackAndReExecute(localCommandId))
			{
				return;
			}
			BuilderTable.BuilderCommand cmd = new BuilderTable.BuilderCommand
			{
				type = BuilderTable.BuilderCommandType.Drop,
				pieceId = pieceId,
				parentPieceId = pieceId,
				localPosition = position,
				localRotation = rotation,
				velocity = velocity,
				angVelocity = angVelocity,
				player = droppedByPlayer,
				canRollback = force,
				localCommandId = localCommandId
			};
			this.RouteNewCommand(cmd, force);
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x0015B8BC File Offset: 0x00159ABC
		public void ExecutePieceDroppedWithActions(BuilderTable.BuilderCommand cmd)
		{
			int pieceId = cmd.pieceId;
			int localCommandId = cmd.localCommandId;
			int actorNumber = cmd.player.ActorNumber;
			if ((cmd.player == null || !cmd.player.IsLocal) && !this.ValidateDropPieceParams(pieceId, cmd.localPosition, cmd.localRotation, cmd.velocity, cmd.angVelocity, cmd.player))
			{
				return;
			}
			BuilderPiece piece = this.GetPiece(pieceId);
			if (piece == null)
			{
				return;
			}
			if (piece.state == BuilderPiece.State.AttachedToArm)
			{
				BuilderPiece parentPiece = piece.parentPiece;
				BuilderAction action = BuilderActions.CreateDetachFromPiece(localCommandId, pieceId, actorNumber);
				BuilderAction action2 = BuilderActions.CreateDropPiece(localCommandId, pieceId, cmd.localPosition, cmd.localRotation, cmd.velocity, cmd.angVelocity, actorNumber);
				if (cmd.canRollback)
				{
					BuilderAction action3 = BuilderActions.CreateAttachToPieceRollback(localCommandId, piece, actorNumber);
					this.AddRollbackAction(action3);
					this.AddRollForwardCommand(cmd);
				}
				this.ExecuteAction(action);
				this.ExecuteAction(action2);
				return;
			}
			BuilderAction action4 = BuilderActions.CreateDetachFromPlayer(localCommandId, pieceId, actorNumber);
			BuilderAction action5 = BuilderActions.CreateDropPiece(localCommandId, pieceId, cmd.localPosition, cmd.localRotation, cmd.velocity, cmd.angVelocity, actorNumber);
			if (cmd.canRollback)
			{
				BuilderAction action6 = BuilderActions.CreateAttachToPlayerRollback(localCommandId, piece);
				this.AddRollbackAction(action6);
				this.AddRollForwardCommand(cmd);
			}
			this.ExecuteAction(action4);
			this.ExecuteAction(action5);
		}

		// Token: 0x06003D83 RID: 15747 RVA: 0x0015BA00 File Offset: 0x00159C00
		public void ExecutePieceRepelled(BuilderTable.BuilderCommand cmd)
		{
			int pieceId = cmd.pieceId;
			int localCommandId = cmd.localCommandId;
			int actorNumber = cmd.player.ActorNumber;
			int attachPieceId = cmd.attachPieceId;
			BuilderPiece piece = this.GetPiece(pieceId);
			Vector3 velocity = cmd.velocity;
			if (piece == null)
			{
				return;
			}
			if (piece.isBuiltIntoTable || piece.isArmShelf)
			{
				return;
			}
			if (piece.state != BuilderPiece.State.Grabbed && piece.state != BuilderPiece.State.GrabbedLocal && piece.state != BuilderPiece.State.Dropped && piece.state != BuilderPiece.State.AttachedToDropped && piece.state != BuilderPiece.State.AttachedToArm)
			{
				return;
			}
			if (attachPieceId >= 0 && attachPieceId < this.dropZones.Count)
			{
				BuilderDropZone builderDropZone = this.dropZones[attachPieceId];
				builderDropZone.PlayEffect();
				if (builderDropZone.overrideDirection)
				{
					velocity = builderDropZone.GetRepelDirectionWorld() * BuilderTable.DROP_ZONE_REPEL;
				}
			}
			if (piece.heldByPlayerActorNumber >= 0)
			{
				BuilderAction action = BuilderActions.CreateDetachFromPlayer(localCommandId, pieceId, piece.heldByPlayerActorNumber);
				BuilderAction action2 = BuilderActions.CreateDropPiece(localCommandId, pieceId, cmd.localPosition, cmd.localRotation, velocity, cmd.angVelocity, actorNumber);
				this.ExecuteAction(action);
				this.ExecuteAction(action2);
				return;
			}
			if (piece.state == BuilderPiece.State.AttachedToArm && piece.parentPiece != null)
			{
				BuilderAction action3 = BuilderActions.CreateDetachFromPiece(localCommandId, pieceId, piece.heldByPlayerActorNumber);
				BuilderAction action4 = BuilderActions.CreateDropPiece(localCommandId, pieceId, cmd.localPosition, cmd.localRotation, velocity, cmd.angVelocity, actorNumber);
				this.ExecuteAction(action3);
				this.ExecuteAction(action4);
				return;
			}
			BuilderAction action5 = BuilderActions.CreateDropPiece(localCommandId, pieceId, cmd.localPosition, cmd.localRotation, velocity, cmd.angVelocity, actorNumber);
			this.ExecuteAction(action5);
		}

		// Token: 0x06003D84 RID: 15748 RVA: 0x0015BBA0 File Offset: 0x00159DA0
		private void CleanUpDroppedPiece()
		{
			if (!PhotonNetwork.IsMasterClient || this.droppedPieces.Count <= BuilderTable.DROPPED_PIECE_LIMIT)
			{
				return;
			}
			BuilderPiece builderPiece = this.FindFirstSleepingPiece();
			if (builderPiece != null && builderPiece.state == BuilderPiece.State.Dropped)
			{
				this.RequestRecyclePiece(builderPiece, false, -1);
				return;
			}
			Debug.LogErrorFormat("Piece {0} in Dropped List is {1}", new object[]
			{
				builderPiece.pieceId,
				builderPiece.state
			});
		}

		// Token: 0x06003D85 RID: 15749 RVA: 0x0015BC18 File Offset: 0x00159E18
		public void AddPieceToDropList(BuilderPiece piece)
		{
			this.droppedPieces.Add(piece);
			this.droppedPieceData.Add(new BuilderTable.DroppedPieceData
			{
				speedThreshCrossedTime = 0f,
				droppedState = BuilderTable.DroppedPieceState.Light,
				filteredSpeed = 0f
			});
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x0015BC68 File Offset: 0x00159E68
		private BuilderPiece FindFirstSleepingPiece()
		{
			if (this.droppedPieces.Count < 1)
			{
				return null;
			}
			BuilderPiece builderPiece = this.droppedPieces[0];
			for (int i = 0; i < this.droppedPieces.Count; i++)
			{
				if (this.droppedPieces[i].rigidBody != null && this.droppedPieces[i].rigidBody.IsSleeping())
				{
					BuilderPiece result = this.droppedPieces[i];
					this.droppedPieces.RemoveAt(i);
					this.droppedPieceData.RemoveAt(i);
					return result;
				}
			}
			BuilderPiece result2 = this.droppedPieces[0];
			this.droppedPieces.RemoveAt(0);
			this.droppedPieceData.RemoveAt(0);
			return result2;
		}

		// Token: 0x06003D87 RID: 15751 RVA: 0x00058177 File Offset: 0x00056377
		public void RemovePieceFromDropList(BuilderPiece piece)
		{
			if (piece.state == BuilderPiece.State.Dropped)
			{
				this.droppedPieces.Remove(piece);
			}
		}

		// Token: 0x06003D88 RID: 15752 RVA: 0x0015BD24 File Offset: 0x00159F24
		private void UpdateDroppedPieces(float dt)
		{
			for (int i = 0; i < this.droppedPieces.Count; i++)
			{
				Rigidbody rigidBody = this.droppedPieces[i].rigidBody;
				if (rigidBody != null)
				{
					BuilderTable.DroppedPieceData droppedPieceData = this.droppedPieceData[i];
					float magnitude = rigidBody.velocity.magnitude;
					droppedPieceData.filteredSpeed = droppedPieceData.filteredSpeed * 0.95f + magnitude * 0.05f;
					BuilderTable.DroppedPieceState droppedState = droppedPieceData.droppedState;
					if (droppedState != BuilderTable.DroppedPieceState.Light)
					{
						if (droppedState == BuilderTable.DroppedPieceState.Heavy)
						{
							droppedPieceData.speedThreshCrossedTime += dt;
							droppedPieceData.speedThreshCrossedTime = ((droppedPieceData.filteredSpeed > 0.075f) ? (droppedPieceData.speedThreshCrossedTime + dt) : 0f);
							if (droppedPieceData.speedThreshCrossedTime > 0.5f)
							{
								rigidBody.mass = 1f;
								droppedPieceData.droppedState = BuilderTable.DroppedPieceState.Light;
								droppedPieceData.speedThreshCrossedTime = 0f;
							}
						}
					}
					else
					{
						droppedPieceData.speedThreshCrossedTime = ((droppedPieceData.filteredSpeed < 0.05f) ? (droppedPieceData.speedThreshCrossedTime + dt) : 0f);
						if (droppedPieceData.speedThreshCrossedTime > 0f)
						{
							rigidBody.mass = 10000f;
							droppedPieceData.droppedState = BuilderTable.DroppedPieceState.Heavy;
							droppedPieceData.speedThreshCrossedTime = 0f;
						}
					}
					this.droppedPieceData[i] = droppedPieceData;
				}
			}
		}

		// Token: 0x06003D89 RID: 15753 RVA: 0x0005818F File Offset: 0x0005638F
		private void SetLocalPlayerOwnsPlot(bool ownsPlot)
		{
			this.doesLocalPlayerOwnPlot = ownsPlot;
			UnityEvent<bool> onLocalPlayerClaimedPlot = this.OnLocalPlayerClaimedPlot;
			if (onLocalPlayerClaimedPlot == null)
			{
				return;
			}
			onLocalPlayerClaimedPlot.Invoke(this.doesLocalPlayerOwnPlot);
		}

		// Token: 0x06003D8A RID: 15754 RVA: 0x0015BE84 File Offset: 0x0015A084
		public void PlotClaimed(int plotPieceId, Player claimingPlayer)
		{
			BuilderTable.BuilderCommand cmd = new BuilderTable.BuilderCommand
			{
				type = BuilderTable.BuilderCommandType.ClaimPlot,
				pieceId = plotPieceId,
				player = NetPlayer.Get(claimingPlayer)
			};
			this.RouteNewCommand(cmd, false);
		}

		// Token: 0x06003D8B RID: 15755 RVA: 0x0015BEC0 File Offset: 0x0015A0C0
		public void ExecuteClaimPlot(BuilderTable.BuilderCommand cmd)
		{
			int pieceId = cmd.pieceId;
			NetPlayer player = cmd.player;
			if (pieceId == -1)
			{
				return;
			}
			BuilderPiece piece = this.GetPiece(pieceId);
			if (piece == null || !piece.IsPrivatePlot())
			{
				return;
			}
			if (player == null)
			{
				return;
			}
			BuilderPiecePrivatePlot builderPiecePrivatePlot;
			if (this.plotOwners.TryAdd(player.ActorNumber, pieceId) && piece.TryGetPlotComponent(out builderPiecePrivatePlot))
			{
				builderPiecePrivatePlot.ClaimPlotForPlayerNumber(player.ActorNumber);
				if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
				{
					this.SetLocalPlayerOwnsPlot(true);
				}
			}
		}

		// Token: 0x06003D8C RID: 15756 RVA: 0x0015BF44 File Offset: 0x0015A144
		public void PlayerLeftRoom(int playerActorNumber)
		{
			BuilderTable.BuilderCommand cmd = new BuilderTable.BuilderCommand
			{
				type = BuilderTable.BuilderCommandType.PlayerLeftRoom,
				pieceId = playerActorNumber,
				player = null
			};
			bool force = this.tableState == BuilderTable.TableState.WaitForMasterResync;
			this.RouteNewCommand(cmd, force);
		}

		// Token: 0x06003D8D RID: 15757 RVA: 0x0015BF88 File Offset: 0x0015A188
		public void ExecutePlayerLeftRoom(BuilderTable.BuilderCommand cmd)
		{
			NetPlayer player = cmd.player;
			int num = (player != null) ? player.ActorNumber : cmd.pieceId;
			this.FreePlotInternal(-1, num);
			int pieceId;
			if (this.playerToArmShelfLeft.TryGetValue(num, out pieceId))
			{
				this.RecyclePieceInternal(pieceId, true, false, -1);
			}
			this.playerToArmShelfLeft.Remove(num);
			int pieceId2;
			if (this.playerToArmShelfRight.TryGetValue(num, out pieceId2))
			{
				this.RecyclePieceInternal(pieceId2, true, false, -1);
			}
			this.playerToArmShelfRight.Remove(num);
		}

		// Token: 0x06003D8E RID: 15758 RVA: 0x0015C004 File Offset: 0x0015A204
		public void PlotFreed(int plotPieceId, Player claimingPlayer)
		{
			BuilderTable.BuilderCommand cmd = new BuilderTable.BuilderCommand
			{
				type = BuilderTable.BuilderCommandType.FreePlot,
				pieceId = plotPieceId,
				player = NetPlayer.Get(claimingPlayer)
			};
			this.RouteNewCommand(cmd, false);
		}

		// Token: 0x06003D8F RID: 15759 RVA: 0x0015C040 File Offset: 0x0015A240
		public void ExecuteFreePlot(BuilderTable.BuilderCommand cmd)
		{
			int pieceId = cmd.pieceId;
			NetPlayer player = cmd.player;
			if (player == null)
			{
				return;
			}
			this.FreePlotInternal(pieceId, player.ActorNumber);
		}

		// Token: 0x06003D90 RID: 15760 RVA: 0x0015C06C File Offset: 0x0015A26C
		private void FreePlotInternal(int plotPieceId, int requestingPlayer)
		{
			if (plotPieceId == -1 && !this.plotOwners.TryGetValue(requestingPlayer, out plotPieceId))
			{
				return;
			}
			BuilderPiece piece = this.GetPiece(plotPieceId);
			if (piece == null || !piece.IsPrivatePlot())
			{
				return;
			}
			BuilderPiecePrivatePlot builderPiecePrivatePlot;
			if (piece.TryGetPlotComponent(out builderPiecePrivatePlot))
			{
				int ownerActorNumber = builderPiecePrivatePlot.GetOwnerActorNumber();
				this.plotOwners.Remove(ownerActorNumber);
				builderPiecePrivatePlot.FreePlot();
				if (ownerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
				{
					this.SetLocalPlayerOwnsPlot(false);
				}
			}
		}

		// Token: 0x06003D91 RID: 15761 RVA: 0x000581AE File Offset: 0x000563AE
		public bool DoesPlayerOwnPlot(int actorNum)
		{
			return this.plotOwners.ContainsKey(actorNum);
		}

		// Token: 0x06003D92 RID: 15762 RVA: 0x000581BC File Offset: 0x000563BC
		public void RequestPaintPiece(int pieceId, int materialType)
		{
			this.builderNetworking.RequestPaintPiece(pieceId, materialType);
		}

		// Token: 0x06003D93 RID: 15763 RVA: 0x000581CB File Offset: 0x000563CB
		public void PaintPiece(int pieceId, int materialType, Player paintingPlayer, bool force)
		{
			this.PaintPieceInternal(pieceId, materialType, paintingPlayer, force);
		}

		// Token: 0x06003D94 RID: 15764 RVA: 0x0015C0E0 File Offset: 0x0015A2E0
		private void PaintPieceInternal(int pieceId, int materialType, Player paintingPlayer, bool force)
		{
			if (!force && paintingPlayer == PhotonNetwork.LocalPlayer)
			{
				return;
			}
			BuilderTable.BuilderCommand cmd = new BuilderTable.BuilderCommand
			{
				type = BuilderTable.BuilderCommandType.Paint,
				pieceId = pieceId,
				materialType = materialType,
				player = NetPlayer.Get(paintingPlayer)
			};
			this.RouteNewCommand(cmd, force);
		}

		// Token: 0x06003D95 RID: 15765 RVA: 0x0015C134 File Offset: 0x0015A334
		public void ExecutePiecePainted(BuilderTable.BuilderCommand cmd)
		{
			int pieceId = cmd.pieceId;
			int materialType = cmd.materialType;
			BuilderPiece piece = this.GetPiece(pieceId);
			if (piece != null && !piece.isBuiltIntoTable)
			{
				piece.SetMaterial(materialType, false);
			}
		}

		// Token: 0x06003D96 RID: 15766 RVA: 0x0015C170 File Offset: 0x0015A370
		public void CreateArmShelvesForPlayersInBuilder()
		{
			if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
			{
				foreach (Player player in this.builderNetworking.armShelfRequests)
				{
					if (player != null)
					{
						this.builderNetworking.RequestCreateArmShelfForPlayer(player);
					}
				}
				this.builderNetworking.armShelfRequests.Clear();
			}
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x0015C1F0 File Offset: 0x0015A3F0
		public void RemoveArmShelfForPlayer(Player player)
		{
			if (player == null)
			{
				return;
			}
			if (this.tableState != BuilderTable.TableState.Ready)
			{
				this.builderNetworking.armShelfRequests.Remove(player);
				return;
			}
			int pieceId;
			if (this.playerToArmShelfLeft.TryGetValue(player.ActorNumber, out pieceId))
			{
				BuilderPiece piece = this.GetPiece(pieceId);
				this.playerToArmShelfLeft.Remove(player.ActorNumber);
				if (piece.armShelf != null)
				{
					piece.armShelf.piece = null;
					piece.armShelf = null;
				}
				if (PhotonNetwork.IsMasterClient)
				{
					this.builderNetworking.RequestRecyclePiece(pieceId, piece.transform.position, piece.transform.rotation, false, -1);
				}
				else
				{
					this.DropPieceForPlayerLeavingInternal(piece, player.ActorNumber);
				}
			}
			int pieceId2;
			if (this.playerToArmShelfRight.TryGetValue(player.ActorNumber, out pieceId2))
			{
				BuilderPiece piece2 = this.GetPiece(pieceId2);
				this.playerToArmShelfRight.Remove(player.ActorNumber);
				if (piece2.armShelf != null)
				{
					piece2.armShelf.piece = null;
					piece2.armShelf = null;
				}
				if (PhotonNetwork.IsMasterClient)
				{
					this.builderNetworking.RequestRecyclePiece(pieceId2, piece2.transform.position, piece2.transform.rotation, false, -1);
					return;
				}
				this.DropPieceForPlayerLeavingInternal(piece2, player.ActorNumber);
			}
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x0015C330 File Offset: 0x0015A530
		public void DropAllPiecesForPlayerLeaving(int playerActorNumber)
		{
			List<BuilderPiece> list = this.pieces;
			if (list == null)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				BuilderPiece builderPiece = list[i];
				if (builderPiece != null && builderPiece.heldByPlayerActorNumber == playerActorNumber && (builderPiece.state == BuilderPiece.State.Grabbed || builderPiece.state == BuilderPiece.State.GrabbedLocal))
				{
					this.DropPieceForPlayerLeavingInternal(builderPiece, playerActorNumber);
				}
			}
		}

		// Token: 0x06003D99 RID: 15769 RVA: 0x0015C390 File Offset: 0x0015A590
		public void RecycleAllPiecesForPlayerLeaving(int playerActorNumber)
		{
			List<BuilderPiece> list = this.pieces;
			if (list == null)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				BuilderPiece builderPiece = list[i];
				if (builderPiece != null && builderPiece.heldByPlayerActorNumber == playerActorNumber && (builderPiece.state == BuilderPiece.State.Grabbed || builderPiece.state == BuilderPiece.State.GrabbedLocal))
				{
					this.RecyclePieceForPlayerLeavingInternal(builderPiece, playerActorNumber);
				}
			}
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x0015C3F0 File Offset: 0x0015A5F0
		private void DropPieceForPlayerLeavingInternal(BuilderPiece piece, int playerActorNumber)
		{
			BuilderAction action = BuilderActions.CreateDetachFromPlayer(-1, piece.pieceId, playerActorNumber);
			BuilderAction action2 = BuilderActions.CreateDropPiece(-1, piece.pieceId, piece.transform.position, piece.transform.rotation, Vector3.zero, Vector3.zero, playerActorNumber);
			this.ExecuteAction(action);
			this.ExecuteAction(action2);
		}

		// Token: 0x06003D9B RID: 15771 RVA: 0x000581D8 File Offset: 0x000563D8
		private void RecyclePieceForPlayerLeavingInternal(BuilderPiece piece, int playerActorNumber)
		{
			this.builderNetworking.RequestRecyclePiece(piece.pieceId, piece.transform.position, piece.transform.rotation, false, -1);
		}

		// Token: 0x06003D9C RID: 15772 RVA: 0x0015C448 File Offset: 0x0015A648
		private void DetachPieceForPlayerLeavingInternal(BuilderPiece piece, int playerActorNumber)
		{
			BuilderAction action = BuilderActions.CreateDetachFromPiece(-1, piece.pieceId, playerActorNumber);
			BuilderAction action2 = BuilderActions.CreateDropPiece(-1, piece.pieceId, piece.transform.position, piece.transform.rotation, Vector3.zero, Vector3.zero, playerActorNumber);
			this.ExecuteAction(action);
			this.ExecuteAction(action2);
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x0015C4A0 File Offset: 0x0015A6A0
		public void CreateArmShelf(int pieceIdLeft, int pieceIdRight, int pieceType, Player player)
		{
			BuilderTable.BuilderCommand cmd = new BuilderTable.BuilderCommand
			{
				type = BuilderTable.BuilderCommandType.CreateArmShelf,
				pieceId = pieceIdLeft,
				pieceType = pieceType,
				player = NetPlayer.Get(player),
				isLeft = true
			};
			this.RouteNewCommand(cmd, false);
			BuilderTable.BuilderCommand cmd2 = new BuilderTable.BuilderCommand
			{
				type = BuilderTable.BuilderCommandType.CreateArmShelf,
				pieceId = pieceIdRight,
				pieceType = pieceType,
				player = NetPlayer.Get(player),
				isLeft = false
			};
			this.RouteNewCommand(cmd2, false);
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x0015C530 File Offset: 0x0015A730
		public void ExecuteArmShelfCreated(BuilderTable.BuilderCommand cmd)
		{
			NetPlayer player = cmd.player;
			if (player == null)
			{
				return;
			}
			bool isLeft = cmd.isLeft;
			if (this.GetPiece(cmd.pieceId) != null)
			{
				return;
			}
			RigContainer rigContainer;
			if (VRRigCache.Instance.TryGetVrrig(player, out rigContainer))
			{
				BuilderArmShelf builderArmShelf = isLeft ? rigContainer.Rig.builderArmShelfLeft : rigContainer.Rig.builderArmShelfRight;
				if (builderArmShelf != null)
				{
					if (builderArmShelf.piece != null)
					{
						if (builderArmShelf.piece.isArmShelf && builderArmShelf.piece.isActiveAndEnabled)
						{
							builderArmShelf.piece.armShelf = null;
							this.RecyclePiece(builderArmShelf.piece.pieceId, builderArmShelf.piece.transform.position, builderArmShelf.piece.transform.rotation, false, -1, PhotonNetwork.LocalPlayer);
						}
						else
						{
							builderArmShelf.piece = null;
						}
						BuilderPiece builderPiece = this.CreatePieceInternal(cmd.pieceType, cmd.pieceId, builderArmShelf.pieceAnchor.position, builderArmShelf.pieceAnchor.rotation, BuilderPiece.State.AttachedToArm, -1, 0);
						builderArmShelf.piece = builderPiece;
						builderPiece.armShelf = builderArmShelf;
						builderPiece.SetParentHeld(builderArmShelf.pieceAnchor, cmd.player.ActorNumber, isLeft);
						builderPiece.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
						builderPiece.transform.localScale = Vector3.one;
						if (isLeft)
						{
							this.playerToArmShelfLeft.AddOrUpdate(player.ActorNumber, cmd.pieceId);
							return;
						}
						this.playerToArmShelfRight.AddOrUpdate(player.ActorNumber, cmd.pieceId);
						return;
					}
					else
					{
						BuilderPiece builderPiece2 = this.CreatePieceInternal(cmd.pieceType, cmd.pieceId, builderArmShelf.pieceAnchor.position, builderArmShelf.pieceAnchor.rotation, BuilderPiece.State.AttachedToArm, -1, 0);
						builderArmShelf.piece = builderPiece2;
						builderPiece2.armShelf = builderArmShelf;
						builderPiece2.SetParentHeld(builderArmShelf.pieceAnchor, cmd.player.ActorNumber, isLeft);
						builderPiece2.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
						builderPiece2.transform.localScale = Vector3.one;
						if (isLeft)
						{
							this.playerToArmShelfLeft.TryAdd(player.ActorNumber, cmd.pieceId);
							return;
						}
						this.playerToArmShelfRight.TryAdd(player.ActorNumber, cmd.pieceId);
					}
				}
			}
		}

		// Token: 0x06003D9F RID: 15775 RVA: 0x0015C778 File Offset: 0x0015A978
		public void ClearLocalArmShelf()
		{
			VRRig offlineVRRig = GorillaTagger.Instance.offlineVRRig;
			if (offlineVRRig != null)
			{
				BuilderArmShelf builderArmShelf = offlineVRRig.builderArmShelfLeft;
				if (builderArmShelf != null)
				{
					BuilderPiece piece = builderArmShelf.piece;
					builderArmShelf.piece = null;
					if (piece != null)
					{
						piece.transform.SetParent(null);
					}
				}
				builderArmShelf = offlineVRRig.builderArmShelfRight;
				if (builderArmShelf != null)
				{
					BuilderPiece piece2 = builderArmShelf.piece;
					builderArmShelf.piece = null;
					if (piece2 != null)
					{
						piece2.transform.SetParent(null);
					}
				}
			}
		}

		// Token: 0x06003DA0 RID: 15776 RVA: 0x0015C800 File Offset: 0x0015AA00
		public void PieceEnteredDropZone(int pieceId, Vector3 worldPos, Quaternion worldRot, int dropZoneId)
		{
			Vector3 velocity = (this.roomCenter.position - worldPos).normalized * BuilderTable.DROP_ZONE_REPEL;
			BuilderTable.BuilderCommand cmd = new BuilderTable.BuilderCommand
			{
				type = BuilderTable.BuilderCommandType.Repel,
				pieceId = pieceId,
				parentPieceId = pieceId,
				attachPieceId = dropZoneId,
				localPosition = worldPos,
				localRotation = worldRot,
				velocity = velocity,
				angVelocity = Vector3.zero,
				player = NetworkSystem.Instance.MasterClient,
				canRollback = false
			};
			this.RouteNewCommand(cmd, false);
		}

		// Token: 0x06003DA1 RID: 15777 RVA: 0x0015C8A4 File Offset: 0x0015AAA4
		public bool ValidateRepelPiece(BuilderPiece piece)
		{
			if (!this.isSetup)
			{
				return false;
			}
			if (piece.isBuiltIntoTable || piece.isArmShelf)
			{
				return false;
			}
			if (piece.state == BuilderPiece.State.Grabbed || piece.state == BuilderPiece.State.GrabbedLocal || piece.state == BuilderPiece.State.Dropped || piece.state == BuilderPiece.State.AttachedToDropped || piece.state == BuilderPiece.State.AttachedToArm)
			{
				bool flag = false;
				for (int i = 0; i < this.repelHistoryLength; i++)
				{
					flag = (flag || this.repelledPieceRoots[i].Contains(piece.pieceId));
					if (flag)
					{
						return false;
					}
				}
				this.repelledPieceRoots[this.repelHistoryIndex].Add(piece.pieceId);
				return true;
			}
			return false;
		}

		// Token: 0x06003DA2 RID: 15778 RVA: 0x0015C948 File Offset: 0x0015AB48
		public void RepelPieceTowardTable(int pieceID)
		{
			BuilderPiece piece = this.GetPiece(pieceID);
			if (piece == null)
			{
				return;
			}
			Vector3 position = piece.transform.position;
			Quaternion rotation = piece.transform.rotation;
			if (position.y < this.tableCenter.position.y)
			{
				position.y = this.tableCenter.position.y;
			}
			Vector3 velocity = (this.tableCenter.position - position).normalized * BuilderTable.DROP_ZONE_REPEL;
			if (piece.IsHeldLocal())
			{
				BuilderPieceInteractor.instance.RemovePieceFromHeld(piece);
			}
			piece.ClearParentHeld();
			piece.ClearParentPiece(false);
			piece.transform.localScale = Vector3.one;
			piece.SetState(BuilderPiece.State.Dropped, false);
			piece.transform.SetLocalPositionAndRotation(position, rotation);
			if (piece.rigidBody != null)
			{
				piece.rigidBody.position = position;
				piece.rigidBody.rotation = rotation;
				piece.rigidBody.velocity = velocity;
				piece.rigidBody.AddForce(Vector3.up * (BuilderTable.DROP_ZONE_REPEL / 2f), ForceMode.VelocityChange);
				piece.rigidBody.angularVelocity = Vector3.zero;
			}
		}

		// Token: 0x06003DA3 RID: 15779 RVA: 0x0015CA80 File Offset: 0x0015AC80
		public BuilderPiece GetPiece(int pieceId)
		{
			int num;
			if (this.pieceIDToIndexCache.TryGetValue(pieceId, out num))
			{
				if (num >= 0 && num < this.pieces.Count)
				{
					return this.pieces[num];
				}
				this.pieceIDToIndexCache.Remove(pieceId);
			}
			for (int i = 0; i < this.pieces.Count; i++)
			{
				if (this.pieces[i].pieceId == pieceId)
				{
					this.pieceIDToIndexCache.Add(pieceId, i);
					return this.pieces[i];
				}
			}
			for (int j = 0; j < this.basePieces.Count; j++)
			{
				if (this.basePieces[j].pieceId == pieceId)
				{
					return this.basePieces[j];
				}
			}
			return null;
		}

		// Token: 0x06003DA4 RID: 15780 RVA: 0x00058203 File Offset: 0x00056403
		public void AddPiece(BuilderPiece piece)
		{
			this.pieces.Add(piece);
			this.UseResources(piece);
			this.AddPieceData(piece);
		}

		// Token: 0x06003DA5 RID: 15781 RVA: 0x00058220 File Offset: 0x00056420
		public void RemovePiece(BuilderPiece piece)
		{
			this.pieces.Remove(piece);
			this.AddResources(piece);
			this.RemovePieceData(piece);
			this.pieceIDToIndexCache.Clear();
		}

		// Token: 0x06003DA6 RID: 15782 RVA: 0x00030607 File Offset: 0x0002E807
		private void CreateData()
		{
		}

		// Token: 0x06003DA7 RID: 15783 RVA: 0x00030607 File Offset: 0x0002E807
		private void DestroyData()
		{
		}

		// Token: 0x06003DA8 RID: 15784 RVA: 0x000470CF File Offset: 0x000452CF
		private int AddPieceData(BuilderPiece piece)
		{
			return -1;
		}

		// Token: 0x06003DA9 RID: 15785 RVA: 0x00030607 File Offset: 0x0002E807
		public void UpdatePieceData(BuilderPiece piece)
		{
		}

		// Token: 0x06003DAA RID: 15786 RVA: 0x00030607 File Offset: 0x0002E807
		private void RemovePieceData(BuilderPiece piece)
		{
		}

		// Token: 0x06003DAB RID: 15787 RVA: 0x000470CF File Offset: 0x000452CF
		private int AddGridPlaneData(BuilderAttachGridPlane gridPlane)
		{
			return -1;
		}

		// Token: 0x06003DAC RID: 15788 RVA: 0x00030607 File Offset: 0x0002E807
		private void RemoveGridPlaneData(BuilderAttachGridPlane gridPlane)
		{
		}

		// Token: 0x06003DAD RID: 15789 RVA: 0x000470CF File Offset: 0x000452CF
		private int AddPrivatePlotData(BuilderPiecePrivatePlot plot)
		{
			return -1;
		}

		// Token: 0x06003DAE RID: 15790 RVA: 0x00030607 File Offset: 0x0002E807
		private void RemovePrivatePlotData(BuilderPiecePrivatePlot plot)
		{
		}

		// Token: 0x06003DAF RID: 15791 RVA: 0x00058248 File Offset: 0x00056448
		public void OnButtonFreeRotation(BuilderOptionButton button, bool isLeftHand)
		{
			this.useSnapRotation = !this.useSnapRotation;
			button.SetPressed(this.useSnapRotation);
		}

		// Token: 0x06003DB0 RID: 15792 RVA: 0x00058265 File Offset: 0x00056465
		public void OnButtonFreePosition(BuilderOptionButton button, bool isLeftHand)
		{
			if (this.usePlacementStyle == BuilderPlacementStyle.Float)
			{
				this.usePlacementStyle = BuilderPlacementStyle.SnapDown;
			}
			else if (this.usePlacementStyle == BuilderPlacementStyle.SnapDown)
			{
				this.usePlacementStyle = BuilderPlacementStyle.Float;
			}
			button.SetPressed(this.usePlacementStyle > BuilderPlacementStyle.Float);
		}

		// Token: 0x06003DB1 RID: 15793 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnButtonSaveLayout(BuilderOptionButton button, bool isLeftHand)
		{
		}

		// Token: 0x06003DB2 RID: 15794 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnButtonClearLayout(BuilderOptionButton button, bool isLeftHand)
		{
		}

		// Token: 0x06003DB3 RID: 15795 RVA: 0x0015CB48 File Offset: 0x0015AD48
		public bool TryPlaceGridPlane(BuilderPiece piece, BuilderAttachGridPlane gridPlane, List<BuilderAttachGridPlane> checkGridPlanes, out BuilderPotentialPlacement potentialPlacement)
		{
			potentialPlacement = default(BuilderPotentialPlacement);
			potentialPlacement.Reset();
			Vector3 position = gridPlane.transform.position;
			Quaternion rotation = gridPlane.transform.rotation;
			if (this.gridSize <= 0f)
			{
				return false;
			}
			bool result = false;
			for (int i = 0; i < checkGridPlanes.Count; i++)
			{
				BuilderAttachGridPlane checkGridPlane = checkGridPlanes[i];
				this.TryPlaceGridPlaneOnGridPlane(piece, gridPlane, position, rotation, checkGridPlane, ref potentialPlacement, ref result);
			}
			return result;
		}

		// Token: 0x06003DB4 RID: 15796 RVA: 0x0015CBBC File Offset: 0x0015ADBC
		public bool TryPlaceGridPlaneOnGridPlane(BuilderPiece piece, BuilderAttachGridPlane gridPlane, Vector3 gridPlanePos, Quaternion gridPlaneRot, BuilderAttachGridPlane checkGridPlane, ref BuilderPotentialPlacement potentialPlacement, ref bool success)
		{
			if (checkGridPlane.male == gridPlane.male)
			{
				return false;
			}
			if (checkGridPlane.piece == gridPlane.piece)
			{
				return false;
			}
			Transform center = checkGridPlane.center;
			Vector3 position = center.position;
			float sqrMagnitude = (position - gridPlanePos).sqrMagnitude;
			float num = checkGridPlane.boundingRadius + gridPlane.boundingRadius;
			if (sqrMagnitude > num * num)
			{
				return false;
			}
			Quaternion rotation = center.rotation;
			Quaternion quaternion = Quaternion.Inverse(rotation);
			Quaternion quaternion2 = quaternion * gridPlaneRot;
			if (Vector3.Dot(Vector3.up, quaternion2 * Vector3.up) < this.currSnapParams.maxUpDotProduct)
			{
				return false;
			}
			Vector3 vector = quaternion * (gridPlanePos - position);
			float y = vector.y;
			float num2 = -Mathf.Abs(y);
			if (success && num2 < potentialPlacement.score)
			{
				return false;
			}
			if (Mathf.Abs(y) > 1f)
			{
				return false;
			}
			if ((gridPlane.male && y > this.currSnapParams.minOffsetY) || (!gridPlane.male && y < -this.currSnapParams.minOffsetY))
			{
				return false;
			}
			if (Mathf.Abs(y) > this.currSnapParams.maxOffsetY)
			{
				return false;
			}
			Quaternion quaternion3;
			Quaternion rotation2;
			QuaternionUtil.DecomposeSwingTwist(quaternion2, Vector3.up, out quaternion3, out rotation2);
			float maxTwistDotProduct = this.currSnapParams.maxTwistDotProduct;
			Vector3 lhs = rotation2 * Vector3.forward;
			float num3 = Vector3.Dot(lhs, Vector3.forward);
			float num4 = Vector3.Dot(lhs, Vector3.right);
			bool flag = Mathf.Abs(num3) > maxTwistDotProduct;
			bool flag2 = Mathf.Abs(num4) > maxTwistDotProduct;
			if (!flag && !flag2)
			{
				return false;
			}
			float y2;
			uint num5;
			if (flag)
			{
				y2 = ((num3 > 0f) ? 0f : 180f);
				num5 = ((num3 > 0f) ? 0U : 2U);
			}
			else
			{
				y2 = ((num4 > 0f) ? 90f : 270f);
				num5 = ((num4 > 0f) ? 1U : 3U);
			}
			int num6 = flag2 ? gridPlane.width : gridPlane.length;
			int num7 = flag2 ? gridPlane.length : gridPlane.width;
			float num8 = (num7 % 2 == 0) ? (this.gridSize / 2f) : 0f;
			float num9 = (num6 % 2 == 0) ? (this.gridSize / 2f) : 0f;
			float num10 = (checkGridPlane.width % 2 == 0) ? (this.gridSize / 2f) : 0f;
			float num11 = (checkGridPlane.length % 2 == 0) ? (this.gridSize / 2f) : 0f;
			float num12 = num8 - num10;
			float num13 = num9 - num11;
			int num14 = Mathf.RoundToInt((vector.x - num12) / this.gridSize);
			int num15 = Mathf.RoundToInt((vector.z - num13) / this.gridSize);
			int num16 = num14 + Mathf.FloorToInt((float)num7 / 2f);
			int num17 = num15 + Mathf.FloorToInt((float)num6 / 2f);
			int num18 = num16 - (num7 - 1);
			int num19 = num17 - (num6 - 1);
			int num20 = Mathf.FloorToInt((float)checkGridPlane.width / 2f);
			int num21 = Mathf.FloorToInt((float)checkGridPlane.length / 2f);
			int num22 = num20 - (checkGridPlane.width - 1);
			int num23 = num21 - (checkGridPlane.length - 1);
			if (num18 > num20 || num16 < num22 || num19 > num21 || num17 < num23)
			{
				return false;
			}
			BuilderPiece rootPiece = checkGridPlane.piece.GetRootPiece();
			if (BuilderTable.ShareSameRoot(gridPlane.piece, rootPiece))
			{
				return false;
			}
			if (!BuilderPiece.CanPlayerAttachPieceToPiece(PhotonNetwork.LocalPlayer.ActorNumber, gridPlane.piece, rootPiece))
			{
				return false;
			}
			BuilderPiece piece2 = checkGridPlane.piece;
			if (piece2 != null)
			{
				if (piece2.preventSnapUntilMoved > 0)
				{
					return false;
				}
				if (piece2.requestedParentPiece != null && BuilderTable.ShareSameRoot(piece, piece2.requestedParentPiece))
				{
					return false;
				}
			}
			Quaternion rhs = Quaternion.Euler(0f, y2, 0f);
			Quaternion lhs2 = rotation * rhs;
			float x = (float)num14 * this.gridSize + num12;
			float z = (float)num15 * this.gridSize + num13;
			Vector3 point = new Vector3(x, 0f, z);
			Vector3 a = position + rotation * point;
			Transform center2 = gridPlane.center;
			Quaternion quaternion4 = lhs2 * Quaternion.Inverse(center2.localRotation);
			Vector3 point2 = piece.transform.InverseTransformPoint(center2.position);
			Vector3 localPosition = a - quaternion4 * point2;
			potentialPlacement.localPosition = localPosition;
			potentialPlacement.localRotation = quaternion4;
			potentialPlacement.score = num2;
			success = true;
			potentialPlacement.parentPiece = piece2;
			potentialPlacement.parentAttachIndex = checkGridPlane.attachIndex;
			potentialPlacement.attachDistance = Mathf.Abs(y);
			potentialPlacement.attachPlaneNormal = Vector3.up;
			if (!checkGridPlane.male)
			{
				potentialPlacement.attachPlaneNormal *= -1f;
			}
			if (potentialPlacement.parentPiece != null)
			{
				BuilderAttachGridPlane builderAttachGridPlane = potentialPlacement.parentPiece.gridPlanes[potentialPlacement.parentAttachIndex];
				potentialPlacement.localPosition = builderAttachGridPlane.transform.InverseTransformPoint(potentialPlacement.localPosition);
				potentialPlacement.localRotation = Quaternion.Inverse(builderAttachGridPlane.transform.rotation) * potentialPlacement.localRotation;
			}
			potentialPlacement.parentAttachBounds.min.x = Mathf.Max(num22, num18);
			potentialPlacement.parentAttachBounds.min.y = Mathf.Max(num23, num19);
			potentialPlacement.parentAttachBounds.max.x = Mathf.Min(num20, num16);
			potentialPlacement.parentAttachBounds.max.y = Mathf.Min(num21, num17);
			Vector2Int v = Vector2Int.zero;
			Vector2Int v2 = Vector2Int.zero;
			v.x = potentialPlacement.parentAttachBounds.min.x - num14;
			v2.x = potentialPlacement.parentAttachBounds.max.x - num14;
			v.y = potentialPlacement.parentAttachBounds.min.y - num15;
			v2.y = potentialPlacement.parentAttachBounds.max.y - num15;
			potentialPlacement.twist = (byte)num5;
			potentialPlacement.bumpOffsetX = (sbyte)num14;
			potentialPlacement.bumpOffsetZ = (sbyte)num15;
			int offsetX = (num7 % 2 == 0) ? 1 : 0;
			int offsetY = (num6 % 2 == 0) ? 1 : 0;
			if (flag && num3 < 0f)
			{
				v = this.Rotate180(v, offsetX, offsetY);
				v2 = this.Rotate180(v2, offsetX, offsetY);
			}
			else if (flag2 && num4 < 0f)
			{
				v = this.Rotate270(v, offsetX, offsetY);
				v2 = this.Rotate270(v2, offsetX, offsetY);
			}
			else if (flag2 && num4 > 0f)
			{
				v = this.Rotate90(v, offsetX, offsetY);
				v2 = this.Rotate90(v2, offsetX, offsetY);
			}
			potentialPlacement.attachBounds.min.x = Mathf.Min(v.x, v2.x);
			potentialPlacement.attachBounds.min.y = Mathf.Min(v.y, v2.y);
			potentialPlacement.attachBounds.max.x = Mathf.Max(v.x, v2.x);
			potentialPlacement.attachBounds.max.y = Mathf.Max(v.y, v2.y);
			return true;
		}

		// Token: 0x06003DB5 RID: 15797 RVA: 0x00058297 File Offset: 0x00056497
		private Vector2Int Rotate90(Vector2Int v, int offsetX, int offsetY)
		{
			return new Vector2Int(v.y * -1 + offsetY, v.x);
		}

		// Token: 0x06003DB6 RID: 15798 RVA: 0x000582B0 File Offset: 0x000564B0
		private Vector2Int Rotate270(Vector2Int v, int offsetX, int offsetY)
		{
			return new Vector2Int(v.y, v.x * -1 + offsetX);
		}

		// Token: 0x06003DB7 RID: 15799 RVA: 0x000582C9 File Offset: 0x000564C9
		private Vector2Int Rotate180(Vector2Int v, int offsetX, int offsetY)
		{
			return new Vector2Int(v.x * -1 + offsetX, v.y * -1 + offsetY);
		}

		// Token: 0x06003DB8 RID: 15800 RVA: 0x000582E6 File Offset: 0x000564E6
		public bool ShareSameRoot(BuilderAttachGridPlane plane, BuilderAttachGridPlane otherPlane)
		{
			return !(plane == null) && !(otherPlane == null) && !(otherPlane.piece == null) && BuilderTable.ShareSameRoot(plane.piece, otherPlane.piece);
		}

		// Token: 0x06003DB9 RID: 15801 RVA: 0x0015D338 File Offset: 0x0015B538
		public static bool ShareSameRoot(BuilderPiece piece, BuilderPiece otherPiece)
		{
			if (otherPiece == null || piece == null)
			{
				return false;
			}
			if (piece == otherPiece)
			{
				return true;
			}
			BuilderPiece builderPiece = piece;
			int num = 2048;
			while (builderPiece.parentPiece != null && !builderPiece.parentPiece.isBuiltIntoTable)
			{
				builderPiece = builderPiece.parentPiece;
				num--;
				if (num <= 0)
				{
					return true;
				}
			}
			num = 2048;
			BuilderPiece builderPiece2 = otherPiece;
			while (builderPiece2.parentPiece != null && !builderPiece2.parentPiece.isBuiltIntoTable)
			{
				builderPiece2 = builderPiece2.parentPiece;
				num--;
				if (num <= 0)
				{
					return true;
				}
			}
			return builderPiece == builderPiece2;
		}

		// Token: 0x06003DBA RID: 15802 RVA: 0x0015D3D8 File Offset: 0x0015B5D8
		public bool TryPlacePieceOnTableNoDrop(bool leftHand, BuilderPiece testPiece, List<BuilderAttachGridPlane> checkGridPlanesMale, List<BuilderAttachGridPlane> checkGridPlanesFemale, out BuilderPotentialPlacement potentialPlacement)
		{
			potentialPlacement = default(BuilderPotentialPlacement);
			potentialPlacement.Reset();
			if (this == null)
			{
				return false;
			}
			if (testPiece == null)
			{
				return false;
			}
			this.currSnapParams = this.pushAndEaseParams;
			return this.TryPlacePieceGridPlanesOnTableInternal(testPiece, this.maxPlacementChildDepth, checkGridPlanesMale, checkGridPlanesFemale, out potentialPlacement);
		}

		// Token: 0x06003DBB RID: 15803 RVA: 0x0015D428 File Offset: 0x0015B628
		public bool TryPlacePieceOnTableNoDropJobs(NativeList<BuilderGridPlaneData> gridPlaneData, NativeList<BuilderPieceData> pieceData, NativeList<BuilderGridPlaneData> checkGridPlaneData, NativeList<BuilderPieceData> checkPieceData, out BuilderPotentialPlacement potentialPlacement, List<BuilderPotentialPlacement> allPlacements)
		{
			potentialPlacement = default(BuilderPotentialPlacement);
			potentialPlacement.Reset();
			if (this == null)
			{
				return false;
			}
			this.currSnapParams = this.pushAndEaseParams;
			NativeQueue<BuilderPotentialPlacementData> nativeQueue = new NativeQueue<BuilderPotentialPlacementData>(Allocator.TempJob);
			new BuilderFindPotentialSnaps
			{
				gridSize = this.gridSize,
				currSnapParams = this.currSnapParams,
				gridPlanes = gridPlaneData,
				checkGridPlanes = checkGridPlaneData,
				worldToLocalPos = Vector3.zero,
				worldToLocalRot = Quaternion.identity,
				localToWorldPos = Vector3.zero,
				localToWorldRot = Quaternion.identity,
				potentialPlacements = nativeQueue.AsParallelWriter()
			}.Schedule(gridPlaneData.Length, 32, default(JobHandle)).Complete();
			BuilderPotentialPlacementData builderPotentialPlacementData = default(BuilderPotentialPlacementData);
			bool flag = false;
			while (!nativeQueue.IsEmpty())
			{
				BuilderPotentialPlacementData builderPotentialPlacementData2 = nativeQueue.Dequeue();
				if (!flag || builderPotentialPlacementData2.score > builderPotentialPlacementData.score)
				{
					builderPotentialPlacementData = builderPotentialPlacementData2;
					flag = true;
				}
			}
			if (flag)
			{
				potentialPlacement = builderPotentialPlacementData.ToPotentialPlacement(this);
			}
			if (flag)
			{
				nativeQueue.Clear();
				this.currSnapParams = this.overlapParams;
				Vector3 worldToLocalPos = -potentialPlacement.attachPiece.transform.position;
				Quaternion worldToLocalRot = Quaternion.Inverse(potentialPlacement.attachPiece.transform.rotation);
				BuilderAttachGridPlane builderAttachGridPlane = potentialPlacement.parentPiece.gridPlanes[potentialPlacement.parentAttachIndex];
				Quaternion localToWorldRot = builderAttachGridPlane.transform.rotation * potentialPlacement.localRotation;
				Vector3 localToWorldPos = builderAttachGridPlane.transform.TransformPoint(potentialPlacement.localPosition);
				new BuilderFindPotentialSnaps
				{
					gridSize = this.gridSize,
					currSnapParams = this.currSnapParams,
					gridPlanes = gridPlaneData,
					checkGridPlanes = checkGridPlaneData,
					worldToLocalPos = worldToLocalPos,
					worldToLocalRot = worldToLocalRot,
					localToWorldPos = localToWorldPos,
					localToWorldRot = localToWorldRot,
					potentialPlacements = nativeQueue.AsParallelWriter()
				}.Schedule(gridPlaneData.Length, 32, default(JobHandle)).Complete();
				while (!nativeQueue.IsEmpty())
				{
					BuilderPotentialPlacementData builderPotentialPlacementData3 = nativeQueue.Dequeue();
					if (builderPotentialPlacementData3.attachDistance < this.currSnapParams.maxBlockSnapDist)
					{
						allPlacements.Add(builderPotentialPlacementData3.ToPotentialPlacement(this));
					}
				}
			}
			nativeQueue.Dispose();
			return flag;
		}

		// Token: 0x06003DBC RID: 15804 RVA: 0x0015D694 File Offset: 0x0015B894
		public bool CalcAllPotentialPlacements(NativeList<BuilderGridPlaneData> gridPlaneData, NativeList<BuilderGridPlaneData> checkGridPlaneData, BuilderPotentialPlacement potentialPlacement, List<BuilderPotentialPlacement> allPlacements)
		{
			if (this == null)
			{
				return false;
			}
			bool result = false;
			this.currSnapParams = this.overlapParams;
			NativeQueue<BuilderPotentialPlacementData> nativeQueue = new NativeQueue<BuilderPotentialPlacementData>(Allocator.TempJob);
			nativeQueue.Clear();
			Vector3 worldToLocalPos = -potentialPlacement.attachPiece.transform.position;
			Quaternion worldToLocalRot = Quaternion.Inverse(potentialPlacement.attachPiece.transform.rotation);
			BuilderAttachGridPlane builderAttachGridPlane = potentialPlacement.parentPiece.gridPlanes[potentialPlacement.parentAttachIndex];
			Quaternion localToWorldRot = builderAttachGridPlane.transform.rotation * potentialPlacement.localRotation;
			Vector3 localToWorldPos = builderAttachGridPlane.transform.TransformPoint(potentialPlacement.localPosition);
			new BuilderFindPotentialSnaps
			{
				gridSize = this.gridSize,
				currSnapParams = this.currSnapParams,
				gridPlanes = gridPlaneData,
				checkGridPlanes = checkGridPlaneData,
				worldToLocalPos = worldToLocalPos,
				worldToLocalRot = worldToLocalRot,
				localToWorldPos = localToWorldPos,
				localToWorldRot = localToWorldRot,
				potentialPlacements = nativeQueue.AsParallelWriter()
			}.Schedule(gridPlaneData.Length, 32, default(JobHandle)).Complete();
			while (!nativeQueue.IsEmpty())
			{
				BuilderPotentialPlacementData builderPotentialPlacementData = nativeQueue.Dequeue();
				if (builderPotentialPlacementData.attachDistance < this.currSnapParams.maxBlockSnapDist)
				{
					allPlacements.Add(builderPotentialPlacementData.ToPotentialPlacement(this));
				}
			}
			nativeQueue.Dispose();
			return result;
		}

		// Token: 0x06003DBD RID: 15805 RVA: 0x0015D800 File Offset: 0x0015BA00
		public bool CanPiecesPotentiallySnap(BuilderPiece pieceInHand, BuilderPiece piece)
		{
			BuilderPiece rootPiece = piece.GetRootPiece();
			return !(rootPiece == pieceInHand) && BuilderPiece.CanPlayerAttachPieceToPiece(PhotonNetwork.LocalPlayer.ActorNumber, pieceInHand, rootPiece) && (!(piece.requestedParentPiece != null) || !BuilderTable.ShareSameRoot(pieceInHand, piece.requestedParentPiece)) && piece.preventSnapUntilMoved <= 0;
		}

		// Token: 0x06003DBE RID: 15806 RVA: 0x0015D864 File Offset: 0x0015BA64
		public bool CanPiecesPotentiallyOverlap(BuilderPiece pieceInHand, BuilderPiece rootWhenPlaced, BuilderPiece.State stateWhenPlaced, BuilderPiece otherPiece)
		{
			BuilderPiece rootPiece = otherPiece.GetRootPiece();
			if (rootPiece == pieceInHand)
			{
				return false;
			}
			if (!BuilderPiece.CanPlayerAttachPieceToPiece(PhotonNetwork.LocalPlayer.ActorNumber, pieceInHand, rootPiece))
			{
				return false;
			}
			if (otherPiece.requestedParentPiece != null && BuilderTable.ShareSameRoot(pieceInHand, otherPiece.requestedParentPiece))
			{
				return false;
			}
			if (otherPiece.preventSnapUntilMoved > 0)
			{
				return false;
			}
			BuilderPiece.State stateB = otherPiece.state;
			if (otherPiece.isBuiltIntoTable && !otherPiece.isArmShelf)
			{
				stateB = BuilderPiece.State.AttachedAndPlaced;
			}
			return BuilderTable.AreStatesCompatibleForOverlap(stateWhenPlaced, stateB, rootWhenPlaced, rootPiece);
		}

		// Token: 0x06003DBF RID: 15807 RVA: 0x0005831B File Offset: 0x0005651B
		public void TryDropPiece(bool leftHand, BuilderPiece testPiece, Vector3 velocity, Vector3 angVelocity)
		{
			if (this == null)
			{
				return;
			}
			if (testPiece == null)
			{
				return;
			}
			BuilderTable.instance.RequestDropPiece(testPiece, testPiece.transform.position, testPiece.transform.rotation, velocity, angVelocity);
		}

		// Token: 0x06003DC0 RID: 15808 RVA: 0x0015D8F0 File Offset: 0x0015BAF0
		public bool TryPlacePieceGridPlanesOnTableInternal(BuilderPiece testPiece, int recurse, List<BuilderAttachGridPlane> checkGridPlanesMale, List<BuilderAttachGridPlane> checkGridPlanesFemale, out BuilderPotentialPlacement potentialPlacement)
		{
			potentialPlacement = default(BuilderPotentialPlacement);
			potentialPlacement.Reset();
			bool result = false;
			bool flag = false;
			if (testPiece != null && testPiece.gridPlanes != null && testPiece.gridPlanes.Count > 0 && testPiece.gridPlanes != null)
			{
				for (int i = 0; i < testPiece.gridPlanes.Count; i++)
				{
					List<BuilderAttachGridPlane> checkGridPlanes = testPiece.gridPlanes[i].male ? checkGridPlanesFemale : checkGridPlanesMale;
					BuilderPotentialPlacement builderPotentialPlacement;
					if (this.TryPlaceGridPlane(testPiece, testPiece.gridPlanes[i], checkGridPlanes, out builderPotentialPlacement))
					{
						if (builderPotentialPlacement.attachDistance < this.currSnapParams.snapAttachDistance * 1.1f)
						{
							flag = true;
						}
						if (builderPotentialPlacement.score > potentialPlacement.score && testPiece.preventSnapUntilMoved <= 0)
						{
							potentialPlacement = builderPotentialPlacement;
							potentialPlacement.attachIndex = i;
							potentialPlacement.attachPiece = testPiece;
							result = true;
						}
					}
				}
			}
			if (recurse > 0)
			{
				BuilderPiece builderPiece = testPiece.firstChildPiece;
				while (builderPiece != null)
				{
					BuilderPotentialPlacement builderPotentialPlacement2;
					if (this.TryPlacePieceGridPlanesOnTableInternal(builderPiece, recurse - 1, checkGridPlanesMale, checkGridPlanesFemale, out builderPotentialPlacement2))
					{
						if (builderPotentialPlacement2.attachDistance < this.currSnapParams.snapAttachDistance * 1.1f)
						{
							flag = true;
						}
						if (builderPotentialPlacement2.score > potentialPlacement.score && testPiece.preventSnapUntilMoved <= 0)
						{
							potentialPlacement = builderPotentialPlacement2;
							result = true;
						}
					}
					builderPiece = builderPiece.nextSiblingPiece;
				}
			}
			if (testPiece.preventSnapUntilMoved > 0 && !flag)
			{
				testPiece.preventSnapUntilMoved--;
				BuilderTable.instance.UpdatePieceData(testPiece);
			}
			return result;
		}

		// Token: 0x06003DC1 RID: 15809 RVA: 0x0015DA78 File Offset: 0x0015BC78
		public void TryPlaceRandomlyOnTable(BuilderPiece piece)
		{
			BuilderAttachGridPlane builderAttachGridPlane = piece.gridPlanes[UnityEngine.Random.Range(0, piece.gridPlanes.Count)];
			List<BuilderAttachGridPlane> list = this.baseGridPlanes;
			int num = UnityEngine.Random.Range(0, list.Count);
			int i = 0;
			while (i < list.Count)
			{
				int index = (i + num) % list.Count;
				BuilderAttachGridPlane builderAttachGridPlane2 = list[index];
				if (builderAttachGridPlane2.male != builderAttachGridPlane.male && !(builderAttachGridPlane2.piece == builderAttachGridPlane.piece) && !this.ShareSameRoot(builderAttachGridPlane, builderAttachGridPlane2))
				{
					Vector3 zero = Vector3.zero;
					Quaternion identity = Quaternion.identity;
					BuilderPiece piece2 = builderAttachGridPlane2.piece;
					int attachIndex = builderAttachGridPlane2.attachIndex;
					Transform center = builderAttachGridPlane.center;
					Quaternion quaternion = builderAttachGridPlane2.transform.rotation * Quaternion.Inverse(center.localRotation);
					Vector3 point = piece.transform.InverseTransformPoint(center.position);
					Vector3 a = builderAttachGridPlane2.transform.position - quaternion * point;
					if (piece2 != null)
					{
						BuilderAttachGridPlane builderAttachGridPlane3 = piece2.gridPlanes[attachIndex];
						Vector3 lossyScale = builderAttachGridPlane3.transform.lossyScale;
						Vector3 b = new Vector3(1f / lossyScale.x, 1f / lossyScale.y, 1f / lossyScale.z);
						Quaternion.Inverse(builderAttachGridPlane3.transform.rotation) * Vector3.Scale(a - builderAttachGridPlane3.transform.position, b);
						Quaternion.Inverse(builderAttachGridPlane3.transform.rotation) * quaternion;
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06003DC2 RID: 15810 RVA: 0x0015DC34 File Offset: 0x0015BE34
		public void UseResources(BuilderPiece piece)
		{
			BuilderResources cost = piece.cost;
			if (cost == null)
			{
				return;
			}
			for (int i = 0; i < cost.quantities.Count; i++)
			{
				this.UseResource(cost.quantities[i]);
			}
		}

		// Token: 0x06003DC3 RID: 15811 RVA: 0x00058355 File Offset: 0x00056555
		private void UseResource(BuilderResourceQuantity quantity)
		{
			if (quantity.type < BuilderResourceType.Basic || quantity.type >= BuilderResourceType.Count)
			{
				return;
			}
			this.usedResources[(int)quantity.type] += quantity.count;
			if (this.tableState == BuilderTable.TableState.Ready)
			{
				this.OnAvailableResourcesChange();
			}
		}

		// Token: 0x06003DC4 RID: 15812 RVA: 0x0015DC7C File Offset: 0x0015BE7C
		public void AddResources(BuilderPiece piece)
		{
			BuilderResources cost = piece.cost;
			if (cost == null)
			{
				return;
			}
			for (int i = 0; i < cost.quantities.Count; i++)
			{
				this.AddResource(cost.quantities[i]);
			}
		}

		// Token: 0x06003DC5 RID: 15813 RVA: 0x00058394 File Offset: 0x00056594
		private void AddResource(BuilderResourceQuantity quantity)
		{
			if (quantity.type < BuilderResourceType.Basic || quantity.type >= BuilderResourceType.Count)
			{
				return;
			}
			this.usedResources[(int)quantity.type] -= quantity.count;
			if (this.tableState == BuilderTable.TableState.Ready)
			{
				this.OnAvailableResourcesChange();
			}
		}

		// Token: 0x06003DC6 RID: 15814 RVA: 0x0015DCC4 File Offset: 0x0015BEC4
		public bool HasEnoughUnreservedResources(BuilderResources resources)
		{
			if (resources == null)
			{
				return false;
			}
			for (int i = 0; i < resources.quantities.Count; i++)
			{
				if (!this.HasEnoughUnreservedResource(resources.quantities[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003DC7 RID: 15815 RVA: 0x0015DD0C File Offset: 0x0015BF0C
		public bool HasEnoughUnreservedResource(BuilderResourceQuantity quantity)
		{
			return quantity.type >= BuilderResourceType.Basic && quantity.type < BuilderResourceType.Count && this.usedResources[(int)quantity.type] + this.reservedResources[(int)quantity.type] + quantity.count <= this.maxResources[(int)quantity.type];
		}

		// Token: 0x06003DC8 RID: 15816 RVA: 0x0015DD64 File Offset: 0x0015BF64
		public bool HasEnoughResources(BuilderPiece piece)
		{
			BuilderResources cost = piece.cost;
			if (cost == null)
			{
				return false;
			}
			for (int i = 0; i < cost.quantities.Count; i++)
			{
				if (!this.HasEnoughResource(cost.quantities[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x000583D3 File Offset: 0x000565D3
		public bool HasEnoughResource(BuilderResourceQuantity quantity)
		{
			return quantity.type >= BuilderResourceType.Basic && quantity.type < BuilderResourceType.Count && this.usedResources[(int)quantity.type] + quantity.count <= this.maxResources[(int)quantity.type];
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x0005840F File Offset: 0x0005660F
		public int GetAvailableResources(BuilderResourceType type)
		{
			if (type < BuilderResourceType.Basic || type >= BuilderResourceType.Count)
			{
				return 0;
			}
			return this.maxResources[(int)type] - this.usedResources[(int)type];
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x0015DDB0 File Offset: 0x0015BFB0
		private void OnAvailableResourcesChange()
		{
			for (int i = 0; i < this.factories.Count; i++)
			{
				this.factories[i].OnAvailableResourcesChange();
			}
			if (this.isSetup)
			{
				for (int j = 0; j < this.conveyors.Count; j++)
				{
					this.conveyors[j].OnAvailableResourcesChange();
				}
			}
			foreach (BuilderResourceMeter builderResourceMeter in this.resourceMeters)
			{
				builderResourceMeter.OnAvailableResourcesChange();
			}
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x0005842C File Offset: 0x0005662C
		public int GetPrivateResourceLimitForType(int type)
		{
			if (this.plotMaxResources == null)
			{
				return 0;
			}
			return this.plotMaxResources[type];
		}

		// Token: 0x06003DCD RID: 15821 RVA: 0x00058440 File Offset: 0x00056640
		private void WriteVector3(BinaryWriter writer, Vector3 data)
		{
			writer.Write(data.x);
			writer.Write(data.y);
			writer.Write(data.z);
		}

		// Token: 0x06003DCE RID: 15822 RVA: 0x00058466 File Offset: 0x00056666
		private void WriteQuaternion(BinaryWriter writer, Quaternion data)
		{
			writer.Write(data.x);
			writer.Write(data.y);
			writer.Write(data.z);
			writer.Write(data.w);
		}

		// Token: 0x06003DCF RID: 15823 RVA: 0x0015DE58 File Offset: 0x0015C058
		private Vector3 ReadVector3(BinaryReader reader)
		{
			Vector3 result;
			result.x = reader.ReadSingle();
			result.y = reader.ReadSingle();
			result.z = reader.ReadSingle();
			return result;
		}

		// Token: 0x06003DD0 RID: 15824 RVA: 0x0015DE90 File Offset: 0x0015C090
		private Quaternion ReadQuaternion(BinaryReader reader)
		{
			Quaternion result;
			result.x = reader.ReadSingle();
			result.y = reader.ReadSingle();
			result.z = reader.ReadSingle();
			result.w = reader.ReadSingle();
			return result;
		}

		// Token: 0x06003DD1 RID: 15825 RVA: 0x0015DED4 File Offset: 0x0015C0D4
		public static int PackPiecePlacement(byte twist, sbyte xOffset, sbyte zOffset)
		{
			int num = (int)(twist & 3);
			int num2 = (int)xOffset + 128;
			int num3 = (int)zOffset + 128;
			return num2 + (num3 << 8) + (num << 16);
		}

		// Token: 0x06003DD2 RID: 15826 RVA: 0x0015DF00 File Offset: 0x0015C100
		public static void UnpackPiecePlacement(int packed, out byte twist, out sbyte xOffset, out sbyte zOffset)
		{
			int num = packed & 255;
			int num2 = packed >> 8 & 255;
			int num3 = packed >> 16 & 3;
			twist = (byte)num3;
			xOffset = (sbyte)(num - 128);
			zOffset = (sbyte)(num2 - 128);
		}

		// Token: 0x06003DD3 RID: 15827 RVA: 0x0015DF40 File Offset: 0x0015C140
		private long PackSnapInfo(int attachGridIndex, int otherAttachGridIndex, Vector2Int min, Vector2Int max)
		{
			long num = (long)Mathf.Clamp(attachGridIndex, 0, 31);
			long num2 = (long)Mathf.Clamp(otherAttachGridIndex, 0, 31);
			long num3 = (long)Mathf.Clamp(min.x + 1024, 0, 2047);
			long num4 = (long)Mathf.Clamp(min.y + 1024, 0, 2047);
			long num5 = (long)Mathf.Clamp(max.x + 1024, 0, 2047);
			long num6 = (long)Mathf.Clamp(max.y + 1024, 0, 2047);
			return num + (num2 << 5) + (num3 << 10) + (num4 << 21) + (num5 << 32) + (num6 << 43);
		}

		// Token: 0x06003DD4 RID: 15828 RVA: 0x0015DFE4 File Offset: 0x0015C1E4
		private void UnpackSnapInfo(long packed, out int attachGridIndex, out int otherAttachGridIndex, out Vector2Int min, out Vector2Int max)
		{
			long num = packed & 31L;
			attachGridIndex = (int)num;
			num = (packed >> 5 & 31L);
			otherAttachGridIndex = (int)num;
			int x = (int)(packed >> 10 & 2047L) - 1024;
			int y = (int)(packed >> 21 & 2047L) - 1024;
			min = new Vector2Int(x, y);
			int x2 = (int)(packed >> 32 & 2047L) - 1024;
			int y2 = (int)(packed >> 43 & 2047L) - 1024;
			max = new Vector2Int(x2, y2);
		}

		// Token: 0x06003DD5 RID: 15829 RVA: 0x00058498 File Offset: 0x00056698
		private IEnumerator RequestTitleDataOnLogIn()
		{
			int attempts = 100;
			while (attempts > 0 && !PlayFabClientAPI.IsClientLoggedIn())
			{
				yield return new WaitForSeconds(10f);
				int num = attempts;
				attempts = num - 1;
			}
			if (PlayFabClientAPI.IsClientLoggedIn())
			{
				this.PullConfigurationFromTitleData();
			}
			yield break;
		}

		// Token: 0x06003DD6 RID: 15830 RVA: 0x0015E074 File Offset: 0x0015C274
		private void PullConfigurationFromTitleData()
		{
			if (this.hasRequestedConfig)
			{
				return;
			}
			this.hasRequestedConfig = true;
			PlayFabClientAPI.GetTitleData(new GetTitleDataRequest
			{
				Keys = new List<string>
				{
					"BuilderTableConfiguration"
				}
			}, new Action<GetTitleDataResult>(this.OnGetConfigurationSuccess), new Action<PlayFabError>(this.OnGetConfigurationFail), null, null);
		}

		// Token: 0x06003DD7 RID: 15831 RVA: 0x0015E0CC File Offset: 0x0015C2CC
		private void OnGetConfigurationSuccess(GetTitleDataResult result)
		{
			string dataRecord;
			if (result.Data.TryGetValue("BuilderTableConfiguration", out dataRecord))
			{
				this.ParseTableConfiguration(dataRecord);
			}
		}

		// Token: 0x06003DD8 RID: 15832 RVA: 0x000584A7 File Offset: 0x000566A7
		private void OnGetConfigurationFail(PlayFabError error)
		{
			if (error.Error == PlayFabErrorCode.NotAuthenticated)
			{
				PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
				this.hasRequestedConfig = false;
			}
		}

		// Token: 0x06003DD9 RID: 15833 RVA: 0x0015E0F4 File Offset: 0x0015C2F4
		private void ParseTableConfiguration(string dataRecord)
		{
			if (string.IsNullOrEmpty(dataRecord))
			{
				return;
			}
			BuilderTableConfiguration builderTableConfiguration = JsonUtility.FromJson<BuilderTableConfiguration>(dataRecord);
			if (builderTableConfiguration != null)
			{
				if (builderTableConfiguration.TableResourceLimits != null)
				{
					for (int i = 0; i < builderTableConfiguration.TableResourceLimits.Length; i++)
					{
						int num = builderTableConfiguration.TableResourceLimits[i];
						if (num >= 0)
						{
							this.maxResources[i] = num;
						}
					}
				}
				if (builderTableConfiguration.PlotResourceLimits != null)
				{
					for (int j = 0; j < builderTableConfiguration.PlotResourceLimits.Length; j++)
					{
						int num2 = builderTableConfiguration.PlotResourceLimits[j];
						if (num2 >= 0)
						{
							this.plotMaxResources[j] = num2;
						}
					}
				}
				int droppedPieceLimit = builderTableConfiguration.DroppedPieceLimit;
				if (droppedPieceLimit >= 0)
				{
					BuilderTable.DROPPED_PIECE_LIMIT = droppedPieceLimit;
				}
				if (builderTableConfiguration.updateCountdownDate != null && !string.IsNullOrEmpty(builderTableConfiguration.updateCountdownDate))
				{
					try
					{
						DateTime.Parse(builderTableConfiguration.updateCountdownDate, CultureInfo.InvariantCulture);
						BuilderTable.nextUpdateOverride = builderTableConfiguration.updateCountdownDate;
						goto IL_DC;
					}
					catch
					{
						BuilderTable.nextUpdateOverride = string.Empty;
						goto IL_DC;
					}
				}
				BuilderTable.nextUpdateOverride = string.Empty;
				IL_DC:
				this.OnAvailableResourcesChange();
				UnityEvent onTableConfigurationUpdated = this.OnTableConfigurationUpdated;
				if (onTableConfigurationUpdated == null)
				{
					return;
				}
				onTableConfigurationUpdated.Invoke();
			}
		}

		// Token: 0x06003DDA RID: 15834 RVA: 0x0015E204 File Offset: 0x0015C404
		private void DumpTableConfig()
		{
			BuilderTableConfiguration builderTableConfiguration = new BuilderTableConfiguration();
			Array.Clear(builderTableConfiguration.TableResourceLimits, 0, builderTableConfiguration.TableResourceLimits.Length);
			Array.Clear(builderTableConfiguration.PlotResourceLimits, 0, builderTableConfiguration.PlotResourceLimits.Length);
			foreach (BuilderResourceQuantity builderResourceQuantity in this.totalResources.quantities)
			{
				if (builderResourceQuantity.type >= BuilderResourceType.Basic && builderResourceQuantity.type < (BuilderResourceType)builderTableConfiguration.TableResourceLimits.Length)
				{
					builderTableConfiguration.TableResourceLimits[(int)builderResourceQuantity.type] = builderResourceQuantity.count;
				}
			}
			foreach (BuilderResourceQuantity builderResourceQuantity2 in this.resourcesPerPrivatePlot.quantities)
			{
				if (builderResourceQuantity2.type >= BuilderResourceType.Basic && builderResourceQuantity2.type < (BuilderResourceType)builderTableConfiguration.PlotResourceLimits.Length)
				{
					builderTableConfiguration.PlotResourceLimits[(int)builderResourceQuantity2.type] = builderResourceQuantity2.count;
				}
			}
			builderTableConfiguration.DroppedPieceLimit = BuilderTable.DROPPED_PIECE_LIMIT;
			builderTableConfiguration.updateCountdownDate = "1/10/2025 16:00:00";
			string str = JsonUtility.ToJson(builderTableConfiguration);
			Debug.Log("Configuration Dump \n" + str);
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x000584C9 File Offset: 0x000566C9
		private string GetSaveDataTimeKey(int slot)
		{
			return BuilderTable.personalBuildKey + slot.ToString("D2") + "Time";
		}

		// Token: 0x06003DDC RID: 15836 RVA: 0x000584E6 File Offset: 0x000566E6
		private string GetSaveDataKey(int slot)
		{
			return BuilderTable.personalBuildKey + slot.ToString("D2");
		}

		// Token: 0x06003DDD RID: 15837 RVA: 0x0015E350 File Offset: 0x0015C550
		private void BuildInitialTableForPlayer()
		{
			if (NetworkSystem.Instance.IsNull() || !NetworkSystem.Instance.InRoom || !NetworkSystem.Instance.SessionIsPrivate || NetworkSystem.Instance.GetLocalPlayer() == null || !NetworkSystem.Instance.IsMasterClient)
			{
				this.GetLastSaveTime();
				PlayFabClientAPI.GetTitleData(new GetTitleDataRequest
				{
					Keys = new List<string>
					{
						"BuilderTable01"
					}
				}, new Action<GetTitleDataResult>(this.OnGetTitleDataInitialState), new Action<PlayFabError>(this.OnGetInitialStateFail), null, null);
				return;
			}
			if (this.currentSaveSlot < 0 || this.currentSaveSlot >= BuilderScanKiosk.NUM_SAVE_SLOTS)
			{
				PlayFabClientAPI.GetTitleData(new GetTitleDataRequest
				{
					Keys = new List<string>
					{
						"BuilderTable01"
					}
				}, new Action<GetTitleDataResult>(this.OnGetTitleDataInitialState), new Action<PlayFabError>(this.OnGetInitialStateFail), null, null);
				return;
			}
			PlayFabClientAPI.GetUserData(new PlayFab.ClientModels.GetUserDataRequest
			{
				PlayFabId = PlayFabAuthenticator.instance.GetPlayFabPlayerId(),
				Keys = new List<string>
				{
					this.GetSaveDataKey(this.currentSaveSlot)
				}
			}, new Action<GetUserDataResult>(this.OnGetUserDataInitialState), new Action<PlayFabError>(this.OnGetUserDataInitialStateFail), null, null);
		}

		// Token: 0x06003DDE RID: 15838 RVA: 0x000584FE File Offset: 0x000566FE
		public long GetSaveTimeForSlot(int slot)
		{
			if (slot < 0 || slot >= BuilderScanKiosk.NUM_SAVE_SLOTS)
			{
				return DateTime.MinValue.ToBinary();
			}
			return this.saveDateTime[slot].ToBinary();
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x0015E490 File Offset: 0x0015C690
		private void GetLastSaveTime()
		{
			if (!this.hasQueriedSaveTime)
			{
				PlayFabClientAPI.GetUserData(new PlayFab.ClientModels.GetUserDataRequest
				{
					PlayFabId = PlayFabAuthenticator.instance.GetPlayFabPlayerId(),
					Keys = BuilderTable.saveDateKeys
				}, new Action<GetUserDataResult>(this.OnGetLastSaveTimeSuccess), new Action<PlayFabError>(this.OnGetLastSaveTimeFailure), null, null);
				this.hasQueriedSaveTime = true;
				return;
			}
			UnityEvent onSaveTimeUpdated = this.OnSaveTimeUpdated;
			if (onSaveTimeUpdated == null)
			{
				return;
			}
			onSaveTimeUpdated.Invoke();
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x0015E500 File Offset: 0x0015C700
		private void OnGetLastSaveTimeSuccess(GetUserDataResult result)
		{
			for (int i = 0; i < BuilderScanKiosk.NUM_SAVE_SLOTS; i++)
			{
				UserDataRecord userDataRecord;
				if (result.Data.TryGetValue(this.GetSaveDataTimeKey(i), out userDataRecord))
				{
					DateTime lastUpdated = userDataRecord.LastUpdated;
					this.saveDateTime[i] = lastUpdated + DateTimeOffset.Now.Offset;
				}
			}
			UnityEvent onSaveTimeUpdated = this.OnSaveTimeUpdated;
			if (onSaveTimeUpdated == null)
			{
				return;
			}
			onSaveTimeUpdated.Invoke();
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x0015E56C File Offset: 0x0015C76C
		private void OnGetLastSaveTimeFailure(PlayFabError error)
		{
			PlayFabErrorCode error2 = error.Error;
			if (error2 <= PlayFabErrorCode.ServiceUnavailable)
			{
				if (error2 != PlayFabErrorCode.ConnectionError)
				{
					if (error2 == PlayFabErrorCode.NotAuthenticated)
					{
						PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
						this.hasQueriedSaveTime = false;
						return;
					}
					if (error2 != PlayFabErrorCode.ServiceUnavailable)
					{
						return;
					}
				}
			}
			else if (error2 <= PlayFabErrorCode.APIRequestLimitExceeded)
			{
				if (error2 != PlayFabErrorCode.DownstreamServiceUnavailable && error2 != PlayFabErrorCode.APIRequestLimitExceeded)
				{
					return;
				}
			}
			else if (error2 != PlayFabErrorCode.ConcurrentEditError && error2 != PlayFabErrorCode.APIClientRequestRateLimitExceeded)
			{
				return;
			}
			this.hasQueriedSaveTime = false;
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x0015E5E4 File Offset: 0x0015C7E4
		private void OnGetUserDataInitialState(GetUserDataResult result)
		{
			if (this.tableState != BuilderTable.TableState.WaitForInitialBuildMaster)
			{
				return;
			}
			UserDataRecord userDataRecord;
			if (!result.Data.TryGetValue(this.GetSaveDataKey(this.currentSaveSlot), out userDataRecord))
			{
				this.OnGetUserDataInitialStateFail(null);
				return;
			}
			if (!this.BuildTableFromJson(userDataRecord.Value, false))
			{
				this.OnGetUserDataInitialStateFail(null);
				return;
			}
			DateTime lastUpdated = userDataRecord.LastUpdated;
			if (this.currentSaveSlot >= 0 && this.currentSaveSlot < BuilderScanKiosk.NUM_SAVE_SLOTS)
			{
				this.saveDateTime[this.currentSaveSlot] = lastUpdated + DateTimeOffset.Now.Offset;
			}
			UnityEvent onSaveTimeUpdated = this.OnSaveTimeUpdated;
			if (onSaveTimeUpdated != null)
			{
				onSaveTimeUpdated.Invoke();
			}
			this.hasQueriedSaveTime = true;
			this.SetIsDirty(false);
			this.OnFinishedInitialTableBuild();
		}

		// Token: 0x06003DE3 RID: 15843 RVA: 0x00058528 File Offset: 0x00056728
		private void OnGetUserDataInitialStateFail(PlayFabError error)
		{
			PlayFabClientAPI.GetTitleData(new GetTitleDataRequest
			{
				Keys = new List<string>
				{
					"BuilderTable01"
				}
			}, new Action<GetTitleDataResult>(this.OnGetTitleDataInitialState), new Action<PlayFabError>(this.OnGetInitialStateFail), null, null);
		}

		// Token: 0x06003DE4 RID: 15844 RVA: 0x0015E69C File Offset: 0x0015C89C
		private void OnGetTitleDataInitialState(GetTitleDataResult result)
		{
			if (this.tableState != BuilderTable.TableState.WaitForInitialBuildMaster)
			{
				return;
			}
			string tableJson;
			if (!result.Data.TryGetValue("BuilderTable01", out tableJson))
			{
				this.tableData = new BuilderTableData();
			}
			else if (!this.BuildTableFromJson(tableJson, true))
			{
				this.tableData = new BuilderTableData();
			}
			this.OnFinishedInitialTableBuild();
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x00058564 File Offset: 0x00056764
		private void OnGetInitialStateFail(PlayFabError error)
		{
			if (this.tableState != BuilderTable.TableState.WaitForInitialBuildMaster)
			{
				return;
			}
			this.tableData = new BuilderTableData();
			this.OnFinishedInitialTableBuild();
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x0015E6F0 File Offset: 0x0015C8F0
		public void SaveTableForPlayer()
		{
			this.saveInProgress = true;
			if (this.currentSaveSlot < 0 || this.currentSaveSlot >= BuilderScanKiosk.NUM_SAVE_SLOTS)
			{
				this.saveInProgress = false;
				return;
			}
			if (!this.isDirty)
			{
				this.saveInProgress = false;
				UnityEvent onSaveTimeUpdated = this.OnSaveTimeUpdated;
				if (onSaveTimeUpdated == null)
				{
					return;
				}
				onSaveTimeUpdated.Invoke();
				return;
			}
			else
			{
				if (this.tableData == null)
				{
					this.tableData = new BuilderTableData();
				}
				this.SetIsDirty(false);
				this.tableData.numEdits++;
				string text = this.WriteTableToJson();
				text = Convert.ToBase64String(GZipStream.CompressString(text));
				UpdateUserDataRequest request = new UpdateUserDataRequest
				{
					Data = new Dictionary<string, string>
					{
						{
							this.GetSaveDataKey(this.currentSaveSlot),
							text
						},
						{
							this.GetSaveDataTimeKey(this.currentSaveSlot),
							DateTime.UtcNow.ToString()
						}
					}
				};
				if (!PlayFabClientAPI.IsClientLoggedIn())
				{
					UnityEvent<string> onSaveFailure = this.OnSaveFailure;
					if (onSaveFailure != null)
					{
						onSaveFailure.Invoke("WAIT FOR LOG IN");
					}
					if (PlayFabAuthenticator.instance != null)
					{
						PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
					}
					return;
				}
				PlayFabClientAPI.UpdateUserData(request, new Action<UpdateUserDataResult>(this.OnSaveTableSuccess), new Action<PlayFabError>(this.OnSaveTableFailure), null, null);
				return;
			}
		}

		// Token: 0x06003DE7 RID: 15847 RVA: 0x00058581 File Offset: 0x00056781
		private void OnSaveTableSuccess(UpdateUserDataResult result)
		{
			this.saveDateTime[this.currentSaveSlot] = DateTime.Now;
			this.saveInProgress = false;
			UnityEvent onSaveTimeUpdated = this.OnSaveTimeUpdated;
			if (onSaveTimeUpdated == null)
			{
				return;
			}
			onSaveTimeUpdated.Invoke();
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x0015E820 File Offset: 0x0015CA20
		private void OnSaveTableFailure(PlayFabError error)
		{
			PlayFabErrorCode error2 = error.Error;
			string arg;
			if (error2 <= PlayFabErrorCode.NotAuthenticated)
			{
				if (error2 == PlayFabErrorCode.ConnectionError)
				{
					arg = "BAD CONNECTION";
					goto IL_99;
				}
				if (error2 == PlayFabErrorCode.AccountBanned)
				{
					Application.Quit();
					PhotonNetwork.Disconnect();
					UnityEngine.Object.DestroyImmediate(PhotonNetworkController.Instance);
					UnityEngine.Object.DestroyImmediate(GTPlayer.Instance);
					arg = "BANNED";
					goto IL_99;
				}
				if (error2 == PlayFabErrorCode.NotAuthenticated)
				{
					PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
					arg = "ERROR";
					goto IL_99;
				}
			}
			else
			{
				if (error2 == PlayFabErrorCode.ServiceUnavailable || error2 == PlayFabErrorCode.DownstreamServiceUnavailable)
				{
					arg = "SERVICE DOWN :(";
					goto IL_99;
				}
				if (error2 == PlayFabErrorCode.DataUpdateRateExceeded)
				{
					arg = "BUSY, TRY LATER";
					goto IL_99;
				}
			}
			arg = "ERROR";
			IL_99:
			this.SetIsDirty(true);
			this.saveInProgress = false;
			UnityEvent<string> onSaveFailure = this.OnSaveFailure;
			if (onSaveFailure == null)
			{
				return;
			}
			onSaveFailure.Invoke(arg);
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x0015E8E8 File Offset: 0x0015CAE8
		private string WriteTableToJson()
		{
			this.tableData.Clear();
			BuilderTable.tempDuplicateOverlaps.Clear();
			for (int i = 0; i < this.pieces.Count; i++)
			{
				if (this.pieces[i].state == BuilderPiece.State.AttachedAndPlaced)
				{
					this.tableData.pieceType.Add(this.pieces[i].overrideSavedPiece ? this.pieces[i].savedPieceType : this.pieces[i].pieceType);
					this.tableData.pieceId.Add(this.pieces[i].pieceId);
					this.tableData.parentId.Add((this.pieces[i].parentPiece == null) ? -1 : this.pieces[i].parentPiece.pieceId);
					this.tableData.attachIndex.Add(this.pieces[i].attachIndex);
					this.tableData.parentAttachIndex.Add((this.pieces[i].parentPiece == null) ? -1 : this.pieces[i].parentAttachIndex);
					this.tableData.placement.Add(this.pieces[i].GetPiecePlacement());
					this.tableData.materialType.Add(this.pieces[i].overrideSavedPiece ? this.pieces[i].savedMaterialType : this.pieces[i].materialType);
					BuilderMovingSnapPiece component = this.pieces[i].GetComponent<BuilderMovingSnapPiece>();
					int item = (component == null) ? 0 : component.GetTimeOffset();
					this.tableData.timeOffset.Add(item);
					for (int j = 0; j < this.pieces[i].gridPlanes.Count; j++)
					{
						if (!(this.pieces[i].gridPlanes[j] == null))
						{
							for (SnapOverlap snapOverlap = this.pieces[i].gridPlanes[j].firstOverlap; snapOverlap != null; snapOverlap = snapOverlap.nextOverlap)
							{
								if (snapOverlap.otherPlane.piece.state == BuilderPiece.State.AttachedAndPlaced || snapOverlap.otherPlane.piece.isBuiltIntoTable)
								{
									BuilderTable.SnapOverlapKey item2 = BuilderTable.BuildOverlapKey(this.pieces[i].pieceId, snapOverlap.otherPlane.piece.pieceId, j, snapOverlap.otherPlane.attachIndex);
									if (!BuilderTable.tempDuplicateOverlaps.Contains(item2))
									{
										BuilderTable.tempDuplicateOverlaps.Add(item2);
										long item3 = this.PackSnapInfo(j, snapOverlap.otherPlane.attachIndex, snapOverlap.bounds.min, snapOverlap.bounds.max);
										this.tableData.overlapingPieces.Add(this.pieces[i].pieceId);
										this.tableData.overlappedPieces.Add(snapOverlap.otherPlane.piece.pieceId);
										this.tableData.overlapInfo.Add(item3);
									}
								}
							}
						}
					}
				}
			}
			foreach (BuilderPiece builderPiece in this.basePieces)
			{
				if (!(builderPiece == null))
				{
					for (int k = 0; k < builderPiece.gridPlanes.Count; k++)
					{
						if (!(builderPiece.gridPlanes[k] == null))
						{
							for (SnapOverlap snapOverlap2 = builderPiece.gridPlanes[k].firstOverlap; snapOverlap2 != null; snapOverlap2 = snapOverlap2.nextOverlap)
							{
								if (snapOverlap2.otherPlane.piece.state == BuilderPiece.State.AttachedAndPlaced || snapOverlap2.otherPlane.piece.isBuiltIntoTable)
								{
									BuilderTable.SnapOverlapKey item4 = BuilderTable.BuildOverlapKey(builderPiece.pieceId, snapOverlap2.otherPlane.piece.pieceId, k, snapOverlap2.otherPlane.attachIndex);
									if (!BuilderTable.tempDuplicateOverlaps.Contains(item4))
									{
										BuilderTable.tempDuplicateOverlaps.Add(item4);
										long item5 = this.PackSnapInfo(k, snapOverlap2.otherPlane.attachIndex, snapOverlap2.bounds.min, snapOverlap2.bounds.max);
										this.tableData.overlapingPieces.Add(builderPiece.pieceId);
										this.tableData.overlappedPieces.Add(snapOverlap2.otherPlane.piece.pieceId);
										this.tableData.overlapInfo.Add(item5);
									}
								}
							}
						}
					}
				}
			}
			BuilderTable.tempDuplicateOverlaps.Clear();
			this.tableData.numPieces = this.tableData.pieceType.Count;
			return JsonUtility.ToJson(this.tableData);
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x0015EE3C File Offset: 0x0015D03C
		private static BuilderTable.SnapOverlapKey BuildOverlapKey(int pieceId, int otherPieceId, int attachGridIndex, int otherAttachGridIndex)
		{
			BuilderTable.SnapOverlapKey result = default(BuilderTable.SnapOverlapKey);
			result.piece = (long)pieceId;
			result.piece <<= 32;
			result.piece |= (long)attachGridIndex;
			result.otherPiece = (long)otherPieceId;
			result.otherPiece <<= 32;
			result.otherPiece |= (long)otherAttachGridIndex;
			return result;
		}

		// Token: 0x06003DEB RID: 15851 RVA: 0x0015EE98 File Offset: 0x0015D098
		private bool BuildTableFromJson(string tableJson, bool fromTitleData)
		{
			if (string.IsNullOrEmpty(tableJson))
			{
				return false;
			}
			this.tableData = null;
			try
			{
				this.tableData = JsonUtility.FromJson<BuilderTableData>(tableJson);
			}
			catch
			{
			}
			try
			{
				if (this.tableData == null)
				{
					tableJson = GZipStream.UncompressString(Convert.FromBase64String(tableJson));
					this.tableData = JsonUtility.FromJson<BuilderTableData>(tableJson);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.ToString());
			}
			if (this.tableData == null)
			{
				return false;
			}
			if (this.tableData.version < 4)
			{
				return false;
			}
			Dictionary<int, int> dictionary = new Dictionary<int, int>((this.tableData.pieceType == null) ? 0 : this.tableData.pieceType.Count);
			bool flag = this.tableData.timeOffset != null && this.tableData.timeOffset.Count > 0;
			int i = 0;
			while (i < this.tableData.pieceType.Count)
			{
				int num = this.CreatePieceId();
				dictionary.TryAdd(this.tableData.pieceId[i], num);
				int num2 = (this.tableData.materialType != null && this.tableData.materialType.Count > i) ? this.tableData.materialType[i] : -1;
				int newPieceType = this.tableData.pieceType[i];
				int num3 = num2;
				bool flag2 = true;
				if (fromTitleData)
				{
					goto IL_1ED;
				}
				BuilderPiece piecePrefab = this.GetPiecePrefab(this.tableData.pieceType[i]);
				if (piecePrefab == null)
				{
					this.ClearTable();
					return false;
				}
				if (num3 == -1 && piecePrefab.materialOptions != null)
				{
					int num4;
					Material material;
					int num5;
					piecePrefab.materialOptions.GetDefaultMaterial(out num4, out material, out num5);
					num3 = num4;
				}
				flag2 = BuilderSetManager.instance.IsPieceOwnedLocally(this.tableData.pieceType[i], num3);
				if (!flag2)
				{
					if (!piecePrefab.fallbackInfo.materialSwapThisPrefab)
					{
						if (piecePrefab.fallbackInfo.prefab == null)
						{
							goto IL_25B;
						}
						newPieceType = piecePrefab.fallbackInfo.prefab.name.GetStaticHash();
					}
					num3 = -1;
					goto IL_1ED;
				}
				goto IL_1ED;
				IL_25B:
				i++;
				continue;
				IL_1ED:
				int num6 = flag ? this.tableData.timeOffset[i] : 0;
				BuilderPiece builderPiece = this.CreatePieceInternal(newPieceType, num, Vector3.zero, Quaternion.identity, BuilderPiece.State.AttachedAndPlaced, num3, NetworkSystem.Instance.ServerTimestamp - num6);
				if (!fromTitleData && !flag2)
				{
					builderPiece.overrideSavedPiece = true;
					builderPiece.savedPieceType = this.tableData.pieceType[i];
					builderPiece.savedMaterialType = num2;
					goto IL_25B;
				}
				goto IL_25B;
			}
			for (int j = 0; j < this.tableData.pieceType.Count; j++)
			{
				int parentAttachIndex = (this.tableData.parentAttachIndex == null || this.tableData.parentAttachIndex.Count <= j) ? -1 : this.tableData.parentAttachIndex[j];
				int attachIndex = (this.tableData.attachIndex == null || this.tableData.attachIndex.Count <= j) ? -1 : this.tableData.attachIndex[j];
				int valueOrDefault = dictionary.GetValueOrDefault(this.tableData.pieceId[j], -1);
				int parentId = -1;
				int num7;
				if (dictionary.TryGetValue(this.tableData.parentId[j], out num7))
				{
					parentId = num7;
				}
				else if (this.tableData.parentId[j] < 10000 && this.tableData.parentId[j] >= 5)
				{
					parentId = this.tableData.parentId[j];
				}
				this.AttachPieceInternal(valueOrDefault, attachIndex, parentId, parentAttachIndex, this.tableData.placement[j]);
			}
			foreach (BuilderPiece builderPiece2 in this.pieces)
			{
				if (builderPiece2.state == BuilderPiece.State.AttachedAndPlaced)
				{
					builderPiece2.OnPlacementDeserialized();
				}
			}
			this.OnDeserializeUpdatePlots();
			BuilderTable.tempDuplicateOverlaps.Clear();
			if (this.tableData.overlapingPieces != null)
			{
				int num8 = 0;
				while (num8 < this.tableData.overlapingPieces.Count && num8 < this.tableData.overlappedPieces.Count && num8 < this.tableData.overlapInfo.Count)
				{
					int num9 = -1;
					int num10;
					if (dictionary.TryGetValue(this.tableData.overlapingPieces[num8], out num10))
					{
						num9 = num10;
					}
					else if (this.tableData.overlapingPieces[num8] < 10000 && this.tableData.overlapingPieces[num8] >= 5)
					{
						num9 = this.tableData.overlapingPieces[num8];
					}
					int num11 = -1;
					int num12;
					if (dictionary.TryGetValue(this.tableData.overlappedPieces[num8], out num12))
					{
						num11 = num12;
					}
					else if (this.tableData.overlappedPieces[num8] < 10000 && this.tableData.overlappedPieces[num8] >= 5)
					{
						num11 = this.tableData.overlappedPieces[num8];
					}
					if (num9 != -1 && num11 != -1)
					{
						long packed = this.tableData.overlapInfo[num8];
						BuilderPiece piece = this.GetPiece(num9);
						if (!(piece == null))
						{
							BuilderPiece piece2 = this.GetPiece(num11);
							if (!(piece2 == null))
							{
								int num13;
								int num14;
								Vector2Int min;
								Vector2Int max;
								this.UnpackSnapInfo(packed, out num13, out num14, out min, out max);
								if (num13 >= 0 && num13 < piece.gridPlanes.Count && num14 >= 0 && num14 < piece2.gridPlanes.Count)
								{
									BuilderTable.SnapOverlapKey item = BuilderTable.BuildOverlapKey(num9, num11, num13, num14);
									if (!BuilderTable.tempDuplicateOverlaps.Contains(item))
									{
										BuilderTable.tempDuplicateOverlaps.Add(item);
										piece.gridPlanes[num13].AddSnapOverlap(this.builderPool.CreateSnapOverlap(piece2.gridPlanes[num14], new SnapBounds(min, max)));
									}
								}
							}
						}
					}
					num8++;
				}
			}
			BuilderTable.tempDuplicateOverlaps.Clear();
			return true;
		}

		// Token: 0x06003DEC RID: 15852 RVA: 0x0015F4F4 File Offset: 0x0015D6F4
		public int SerializeTableState(byte[] bytes, int maxBytes)
		{
			MemoryStream memoryStream = new MemoryStream(bytes);
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(this.conveyors.Count);
			foreach (BuilderConveyor builderConveyor in this.conveyors)
			{
				int selectedSetID = builderConveyor.GetSelectedSetID();
				binaryWriter.Write(selectedSetID);
			}
			binaryWriter.Write(this.dispenserShelves.Count);
			foreach (BuilderDispenserShelf builderDispenserShelf in this.dispenserShelves)
			{
				int selectedSetID2 = builderDispenserShelf.GetSelectedSetID();
				binaryWriter.Write(selectedSetID2);
			}
			BuilderTable.childPieces.Clear();
			BuilderTable.rootPieces.Clear();
			BuilderTable.childPieces.EnsureCapacity(this.pieces.Count);
			BuilderTable.rootPieces.EnsureCapacity(this.pieces.Count);
			foreach (BuilderPiece builderPiece in this.pieces)
			{
				if (builderPiece.parentPiece == null)
				{
					BuilderTable.rootPieces.Add(builderPiece);
				}
				else
				{
					BuilderTable.childPieces.Add(builderPiece);
				}
			}
			binaryWriter.Write(BuilderTable.rootPieces.Count);
			for (int i = 0; i < BuilderTable.rootPieces.Count; i++)
			{
				BuilderPiece builderPiece2 = BuilderTable.rootPieces[i];
				binaryWriter.Write(builderPiece2.pieceType);
				binaryWriter.Write(builderPiece2.pieceId);
				binaryWriter.Write((byte)builderPiece2.state);
				if (builderPiece2.state == BuilderPiece.State.OnConveyor || builderPiece2.state == BuilderPiece.State.OnShelf || builderPiece2.state == BuilderPiece.State.Displayed)
				{
					binaryWriter.Write(builderPiece2.shelfOwner);
				}
				else
				{
					binaryWriter.Write(builderPiece2.heldByPlayerActorNumber);
				}
				binaryWriter.Write(builderPiece2.heldInLeftHand ? 1 : 0);
				binaryWriter.Write(builderPiece2.materialType);
				long value = BitPackUtils.PackWorldPosForNetwork(builderPiece2.transform.localPosition);
				int value2 = BitPackUtils.PackQuaternionForNetwork(builderPiece2.transform.localRotation);
				binaryWriter.Write(value);
				binaryWriter.Write(value2);
				if (builderPiece2.state == BuilderPiece.State.AttachedAndPlaced)
				{
					binaryWriter.Write(builderPiece2.functionalPieceState);
					binaryWriter.Write(builderPiece2.activatedTimeStamp);
				}
				if (builderPiece2.state == BuilderPiece.State.OnConveyor)
				{
					binaryWriter.Write(this.conveyorManager.GetPieceCreateTimestamp(builderPiece2));
				}
			}
			binaryWriter.Write(BuilderTable.childPieces.Count);
			for (int j = 0; j < BuilderTable.childPieces.Count; j++)
			{
				BuilderPiece builderPiece3 = BuilderTable.childPieces[j];
				binaryWriter.Write(builderPiece3.pieceType);
				binaryWriter.Write(builderPiece3.pieceId);
				int value3 = (builderPiece3.parentPiece == null) ? -1 : builderPiece3.parentPiece.pieceId;
				binaryWriter.Write(value3);
				binaryWriter.Write(builderPiece3.attachIndex);
				binaryWriter.Write(builderPiece3.parentAttachIndex);
				binaryWriter.Write((byte)builderPiece3.state);
				if (builderPiece3.state == BuilderPiece.State.OnConveyor || builderPiece3.state == BuilderPiece.State.OnShelf || builderPiece3.state == BuilderPiece.State.Displayed)
				{
					binaryWriter.Write(builderPiece3.shelfOwner);
				}
				else
				{
					binaryWriter.Write(builderPiece3.heldByPlayerActorNumber);
				}
				binaryWriter.Write(builderPiece3.heldInLeftHand ? 1 : 0);
				binaryWriter.Write(builderPiece3.materialType);
				int piecePlacement = builderPiece3.GetPiecePlacement();
				binaryWriter.Write(piecePlacement);
				if (builderPiece3.state == BuilderPiece.State.AttachedAndPlaced)
				{
					binaryWriter.Write(builderPiece3.functionalPieceState);
					binaryWriter.Write(builderPiece3.activatedTimeStamp);
				}
				if (builderPiece3.state == BuilderPiece.State.OnConveyor)
				{
					binaryWriter.Write(this.conveyorManager.GetPieceCreateTimestamp(builderPiece3));
				}
			}
			binaryWriter.Write(this.plotOwners.Count);
			foreach (KeyValuePair<int, int> keyValuePair in this.plotOwners)
			{
				binaryWriter.Write(keyValuePair.Key);
				binaryWriter.Write(keyValuePair.Value);
			}
			long position = memoryStream.Position;
			BuilderTable.overlapPieces.Clear();
			BuilderTable.overlapOtherPieces.Clear();
			BuilderTable.overlapPacked.Clear();
			BuilderTable.tempDuplicateOverlaps.Clear();
			foreach (BuilderPiece builderPiece4 in this.pieces)
			{
				if (!(builderPiece4 == null))
				{
					for (int k = 0; k < builderPiece4.gridPlanes.Count; k++)
					{
						if (!(builderPiece4.gridPlanes[k] == null))
						{
							for (SnapOverlap snapOverlap = builderPiece4.gridPlanes[k].firstOverlap; snapOverlap != null; snapOverlap = snapOverlap.nextOverlap)
							{
								BuilderTable.SnapOverlapKey item = BuilderTable.BuildOverlapKey(builderPiece4.pieceId, snapOverlap.otherPlane.piece.pieceId, k, snapOverlap.otherPlane.attachIndex);
								if (!BuilderTable.tempDuplicateOverlaps.Contains(item))
								{
									BuilderTable.tempDuplicateOverlaps.Add(item);
									long item2 = this.PackSnapInfo(k, snapOverlap.otherPlane.attachIndex, snapOverlap.bounds.min, snapOverlap.bounds.max);
									BuilderTable.overlapPieces.Add(builderPiece4.pieceId);
									BuilderTable.overlapOtherPieces.Add(snapOverlap.otherPlane.piece.pieceId);
									BuilderTable.overlapPacked.Add(item2);
								}
							}
						}
					}
				}
			}
			foreach (BuilderPiece builderPiece5 in this.basePieces)
			{
				if (!(builderPiece5 == null))
				{
					for (int l = 0; l < builderPiece5.gridPlanes.Count; l++)
					{
						if (!(builderPiece5.gridPlanes[l] == null))
						{
							for (SnapOverlap snapOverlap2 = builderPiece5.gridPlanes[l].firstOverlap; snapOverlap2 != null; snapOverlap2 = snapOverlap2.nextOverlap)
							{
								BuilderTable.SnapOverlapKey item3 = BuilderTable.BuildOverlapKey(builderPiece5.pieceId, snapOverlap2.otherPlane.piece.pieceId, l, snapOverlap2.otherPlane.attachIndex);
								if (!BuilderTable.tempDuplicateOverlaps.Contains(item3))
								{
									BuilderTable.tempDuplicateOverlaps.Add(item3);
									long item4 = this.PackSnapInfo(l, snapOverlap2.otherPlane.attachIndex, snapOverlap2.bounds.min, snapOverlap2.bounds.max);
									BuilderTable.overlapPieces.Add(builderPiece5.pieceId);
									BuilderTable.overlapOtherPieces.Add(snapOverlap2.otherPlane.piece.pieceId);
									BuilderTable.overlapPacked.Add(item4);
								}
							}
						}
					}
				}
			}
			BuilderTable.tempDuplicateOverlaps.Clear();
			binaryWriter.Write(BuilderTable.overlapPieces.Count);
			for (int m = 0; m < BuilderTable.overlapPieces.Count; m++)
			{
				binaryWriter.Write(BuilderTable.overlapPieces[m]);
				binaryWriter.Write(BuilderTable.overlapOtherPieces[m]);
				binaryWriter.Write(BuilderTable.overlapPacked[m]);
			}
			return (int)memoryStream.Position;
		}

		// Token: 0x06003DED RID: 15853 RVA: 0x0015FD08 File Offset: 0x0015DF08
		public void DeserializeTableState(byte[] bytes, int numBytes)
		{
			if (numBytes <= 0)
			{
				return;
			}
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(bytes));
			BuilderTable.tempPeiceIds.Clear();
			BuilderTable.tempParentPeiceIds.Clear();
			BuilderTable.tempAttachIndexes.Clear();
			BuilderTable.tempParentAttachIndexes.Clear();
			BuilderTable.tempParentActorNumbers.Clear();
			BuilderTable.tempInLeftHand.Clear();
			BuilderTable.tempPiecePlacement.Clear();
			int num = binaryReader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				int selection = binaryReader.ReadInt32();
				if (i < this.conveyors.Count)
				{
					this.conveyors[i].SetSelection(selection);
				}
			}
			int num2 = binaryReader.ReadInt32();
			for (int j = 0; j < num2; j++)
			{
				int selection2 = binaryReader.ReadInt32();
				if (j < this.dispenserShelves.Count)
				{
					this.dispenserShelves[j].SetSelection(selection2);
				}
			}
			int num3 = binaryReader.ReadInt32();
			for (int k = 0; k < num3; k++)
			{
				int newPieceType = binaryReader.ReadInt32();
				int num4 = binaryReader.ReadInt32();
				BuilderPiece.State state = (BuilderPiece.State)binaryReader.ReadByte();
				int num5 = binaryReader.ReadInt32();
				bool item = binaryReader.ReadByte() > 0;
				int materialType = binaryReader.ReadInt32();
				long data = binaryReader.ReadInt64();
				int data2 = binaryReader.ReadInt32();
				Vector3 vector = BitPackUtils.UnpackWorldPosFromNetwork(data);
				Quaternion quaternion = BitPackUtils.UnpackQuaternionFromNetwork(data2);
				byte fState = (state == BuilderPiece.State.AttachedAndPlaced) ? binaryReader.ReadByte() : 0;
				int activateTimeStamp = (state == BuilderPiece.State.AttachedAndPlaced) ? binaryReader.ReadInt32() : 0;
				int num6 = (state == BuilderPiece.State.OnConveyor) ? binaryReader.ReadInt32() : 0;
				float num7 = 10000f;
				if (!vector.IsValid(num7) || !quaternion.IsValid() || !this.ValidateCreatePieceParams(newPieceType, num4, state, materialType))
				{
					this.SetTableState(BuilderTable.TableState.BadData);
					return;
				}
				int num8 = -1;
				if (state == BuilderPiece.State.OnConveyor || state == BuilderPiece.State.OnShelf || state == BuilderPiece.State.Displayed)
				{
					num8 = num5;
					num5 = -1;
				}
				if (this.ValidateDeserializedRootPieceState(num4, state, num8, num5, vector, quaternion))
				{
					BuilderPiece builderPiece = this.CreatePieceInternal(newPieceType, num4, vector, quaternion, state, materialType, activateTimeStamp);
					BuilderTable.tempPeiceIds.Add(num4);
					BuilderTable.tempParentActorNumbers.Add(num5);
					BuilderTable.tempInLeftHand.Add(item);
					builderPiece.SetFunctionalPieceState(fState, NetPlayer.Get(PhotonNetwork.MasterClient), PhotonNetwork.ServerTimestamp);
					if (num8 >= 0)
					{
						builderPiece.shelfOwner = num8;
						if (state == BuilderPiece.State.OnConveyor)
						{
							BuilderConveyor builderConveyor = this.conveyors[num8];
							float timeOffset = 0f;
							if (PhotonNetwork.ServerTimestamp > num6)
							{
								timeOffset = (PhotonNetwork.ServerTimestamp - num6) / 1000f;
							}
							builderConveyor.OnShelfPieceCreated(builderPiece, timeOffset);
						}
						else if (state == BuilderPiece.State.OnShelf || state == BuilderPiece.State.Displayed)
						{
							this.dispenserShelves[num8].OnShelfPieceCreated(builderPiece, false);
						}
					}
				}
			}
			for (int l = 0; l < BuilderTable.tempPeiceIds.Count; l++)
			{
				if (BuilderTable.tempParentActorNumbers[l] >= 0)
				{
					this.AttachPieceToActorInternal(BuilderTable.tempPeiceIds[l], BuilderTable.tempParentActorNumbers[l], BuilderTable.tempInLeftHand[l]);
				}
			}
			BuilderTable.tempPeiceIds.Clear();
			BuilderTable.tempParentActorNumbers.Clear();
			BuilderTable.tempInLeftHand.Clear();
			int num9 = binaryReader.ReadInt32();
			for (int m = 0; m < num9; m++)
			{
				int newPieceType2 = binaryReader.ReadInt32();
				int num10 = binaryReader.ReadInt32();
				int item2 = binaryReader.ReadInt32();
				int item3 = binaryReader.ReadInt32();
				int item4 = binaryReader.ReadInt32();
				BuilderPiece.State state2 = (BuilderPiece.State)binaryReader.ReadByte();
				int num11 = binaryReader.ReadInt32();
				bool item5 = binaryReader.ReadByte() > 0;
				int materialType2 = binaryReader.ReadInt32();
				int item6 = binaryReader.ReadInt32();
				byte fState2 = (state2 == BuilderPiece.State.AttachedAndPlaced) ? binaryReader.ReadByte() : 0;
				int activateTimeStamp2 = (state2 == BuilderPiece.State.AttachedAndPlaced) ? binaryReader.ReadInt32() : 0;
				int num12 = (state2 == BuilderPiece.State.OnConveyor) ? binaryReader.ReadInt32() : 0;
				if (!this.ValidateCreatePieceParams(newPieceType2, num10, state2, materialType2))
				{
					this.SetTableState(BuilderTable.TableState.BadData);
					return;
				}
				int num13 = -1;
				if (state2 == BuilderPiece.State.OnConveyor || state2 == BuilderPiece.State.OnShelf || state2 == BuilderPiece.State.Displayed)
				{
					num13 = num11;
					num11 = -1;
				}
				if (this.ValidateDeserializedChildPieceState(num10, state2))
				{
					BuilderPiece builderPiece2 = this.CreatePieceInternal(newPieceType2, num10, this.roomCenter.position, Quaternion.identity, state2, materialType2, activateTimeStamp2);
					builderPiece2.SetFunctionalPieceState(fState2, NetPlayer.Get(PhotonNetwork.MasterClient), PhotonNetwork.ServerTimestamp);
					BuilderTable.tempPeiceIds.Add(num10);
					BuilderTable.tempParentPeiceIds.Add(item2);
					BuilderTable.tempAttachIndexes.Add(item3);
					BuilderTable.tempParentAttachIndexes.Add(item4);
					BuilderTable.tempParentActorNumbers.Add(num11);
					BuilderTable.tempInLeftHand.Add(item5);
					BuilderTable.tempPiecePlacement.Add(item6);
					if (num13 >= 0)
					{
						builderPiece2.shelfOwner = num13;
						if (state2 == BuilderPiece.State.OnConveyor)
						{
							BuilderConveyor builderConveyor2 = this.conveyors[num13];
							float timeOffset2 = 0f;
							if (PhotonNetwork.ServerTimestamp > num12)
							{
								timeOffset2 = (PhotonNetwork.ServerTimestamp - num12) / 1000f;
							}
							builderConveyor2.OnShelfPieceCreated(builderPiece2, timeOffset2);
						}
						else if (state2 == BuilderPiece.State.OnShelf || state2 == BuilderPiece.State.Displayed)
						{
							this.dispenserShelves[num13].OnShelfPieceCreated(builderPiece2, false);
						}
					}
				}
			}
			for (int n = 0; n < BuilderTable.tempPeiceIds.Count; n++)
			{
				if (!this.ValidateAttachPieceParams(BuilderTable.tempPeiceIds[n], BuilderTable.tempAttachIndexes[n], BuilderTable.tempParentPeiceIds[n], BuilderTable.tempParentAttachIndexes[n], BuilderTable.tempPiecePlacement[n]))
				{
					this.RecyclePieceInternal(BuilderTable.tempPeiceIds[n], true, false, -1);
				}
				else
				{
					this.AttachPieceInternal(BuilderTable.tempPeiceIds[n], BuilderTable.tempAttachIndexes[n], BuilderTable.tempParentPeiceIds[n], BuilderTable.tempParentAttachIndexes[n], BuilderTable.tempPiecePlacement[n]);
				}
			}
			for (int num14 = 0; num14 < BuilderTable.tempPeiceIds.Count; num14++)
			{
				if (BuilderTable.tempParentActorNumbers[num14] >= 0)
				{
					this.AttachPieceToActorInternal(BuilderTable.tempPeiceIds[num14], BuilderTable.tempParentActorNumbers[num14], BuilderTable.tempInLeftHand[num14]);
				}
			}
			foreach (BuilderPiece builderPiece3 in this.pieces)
			{
				if (builderPiece3.state == BuilderPiece.State.AttachedAndPlaced)
				{
					builderPiece3.OnPlacementDeserialized();
				}
			}
			this.plotOwners.Clear();
			this.doesLocalPlayerOwnPlot = false;
			int num15 = binaryReader.ReadInt32();
			for (int num16 = 0; num16 < num15; num16++)
			{
				int num17 = binaryReader.ReadInt32();
				int num18 = binaryReader.ReadInt32();
				BuilderPiecePrivatePlot builderPiecePrivatePlot;
				if (this.plotOwners.TryAdd(num17, num18) && this.GetPiece(num18).TryGetPlotComponent(out builderPiecePrivatePlot))
				{
					builderPiecePrivatePlot.ClaimPlotForPlayerNumber(num17);
					if (num17 == PhotonNetwork.LocalPlayer.ActorNumber)
					{
						this.doesLocalPlayerOwnPlot = true;
					}
				}
			}
			UnityEvent<bool> onLocalPlayerClaimedPlot = this.OnLocalPlayerClaimedPlot;
			if (onLocalPlayerClaimedPlot != null)
			{
				onLocalPlayerClaimedPlot.Invoke(this.doesLocalPlayerOwnPlot);
			}
			this.OnDeserializeUpdatePlots();
			BuilderTable.tempDuplicateOverlaps.Clear();
			int num19 = binaryReader.ReadInt32();
			for (int num20 = 0; num20 < num19; num20++)
			{
				int pieceId = binaryReader.ReadInt32();
				int num21 = binaryReader.ReadInt32();
				long packed = binaryReader.ReadInt64();
				BuilderPiece piece = this.GetPiece(pieceId);
				if (!(piece == null))
				{
					BuilderPiece piece2 = this.GetPiece(num21);
					if (!(piece2 == null))
					{
						int num22;
						int num23;
						Vector2Int min;
						Vector2Int max;
						this.UnpackSnapInfo(packed, out num22, out num23, out min, out max);
						if (num22 >= 0 && num22 < piece.gridPlanes.Count && num23 >= 0 && num23 < piece2.gridPlanes.Count)
						{
							BuilderTable.SnapOverlapKey item7 = BuilderTable.BuildOverlapKey(pieceId, num21, num22, num23);
							if (!BuilderTable.tempDuplicateOverlaps.Contains(item7))
							{
								BuilderTable.tempDuplicateOverlaps.Add(item7);
								piece.gridPlanes[num22].AddSnapOverlap(this.builderPool.CreateSnapOverlap(piece2.gridPlanes[num23], new SnapBounds(min, max)));
							}
						}
					}
				}
			}
			BuilderTable.tempDuplicateOverlaps.Clear();
		}

		// Token: 0x04003E18 RID: 15896
		public const GTZone BUILDER_ZONE = GTZone.monkeBlocks;

		// Token: 0x04003E19 RID: 15897
		private const int INITIAL_BUILTIN_PIECE_ID = 5;

		// Token: 0x04003E1A RID: 15898
		private const int INITIAL_CREATED_PIECE_ID = 10000;

		// Token: 0x04003E1B RID: 15899
		public static float MAX_DROP_VELOCITY = 20f;

		// Token: 0x04003E1C RID: 15900
		public static float MAX_DROP_ANG_VELOCITY = 50f;

		// Token: 0x04003E1D RID: 15901
		private const float MAX_DISTANCE_FROM_CENTER = 13f;

		// Token: 0x04003E1E RID: 15902
		private const float MAX_LOCAL_MAGNITUDE = 80f;

		// Token: 0x04003E1F RID: 15903
		public const float MAX_DISTANCE_FROM_HAND = 2.5f;

		// Token: 0x04003E20 RID: 15904
		public static float DROP_ZONE_REPEL = 2.25f;

		// Token: 0x04003E21 RID: 15905
		public static int placedLayer;

		// Token: 0x04003E22 RID: 15906
		public static int heldLayer;

		// Token: 0x04003E23 RID: 15907
		public static int heldLayerLocal;

		// Token: 0x04003E24 RID: 15908
		public static int droppedLayer;

		// Token: 0x04003E25 RID: 15909
		public List<BuilderPiece> builderPieces;

		// Token: 0x04003E26 RID: 15910
		[Header("Scene References")]
		public BuilderTableNetworking builderNetworking;

		// Token: 0x04003E27 RID: 15911
		public BuilderRenderer builderRenderer;

		// Token: 0x04003E28 RID: 15912
		public BuilderPool builderPool;

		// Token: 0x04003E29 RID: 15913
		public SizeChanger sizeChanger;

		// Token: 0x04003E2A RID: 15914
		public GameObject shelvesRoot;

		// Token: 0x04003E2B RID: 15915
		public GameObject dropZoneRoot;

		// Token: 0x04003E2C RID: 15916
		public List<GameObject> recyclerRoot;

		// Token: 0x04003E2D RID: 15917
		public List<GameObject> allShelvesRoot;

		// Token: 0x04003E2E RID: 15918
		public List<BuilderFactory> factories;

		// Token: 0x04003E2F RID: 15919
		[NonSerialized]
		public List<BuilderConveyor> conveyors;

		// Token: 0x04003E30 RID: 15920
		[NonSerialized]
		public List<BuilderDispenserShelf> dispenserShelves;

		// Token: 0x04003E31 RID: 15921
		public BuilderConveyorManager conveyorManager;

		// Token: 0x04003E32 RID: 15922
		public List<BuilderResourceMeter> resourceMeters;

		// Token: 0x04003E33 RID: 15923
		[NonSerialized]
		public List<BuilderRecycler> recyclers;

		// Token: 0x04003E34 RID: 15924
		[NonSerialized]
		public List<BuilderDropZone> dropZones;

		// Token: 0x04003E35 RID: 15925
		private int shelfSliceUpdateIndex;

		// Token: 0x04003E36 RID: 15926
		public static int SHELF_SLICE_BUCKETS = 6;

		// Token: 0x04003E37 RID: 15927
		[Header("Tint Settings")]
		public float defaultTint = 1f;

		// Token: 0x04003E38 RID: 15928
		public float droppedTint = 0.75f;

		// Token: 0x04003E39 RID: 15929
		public float grabbedTint = 0.75f;

		// Token: 0x04003E3A RID: 15930
		public float shelfTint = 1f;

		// Token: 0x04003E3B RID: 15931
		public float potentialGrabTint = 0.75f;

		// Token: 0x04003E3C RID: 15932
		public float paintingTint = 0.6f;

		// Token: 0x04003E3D RID: 15933
		[Header("Table Transform")]
		public Transform tableCenter;

		// Token: 0x04003E3E RID: 15934
		public Transform roomCenter;

		// Token: 0x04003E3F RID: 15935
		public Transform worldCenter;

		// Token: 0x04003E40 RID: 15936
		public GameObject sharedBuildArea;

		// Token: 0x04003E41 RID: 15937
		private BoxCollider[] sharedBuildAreas;

		// Token: 0x04003E42 RID: 15938
		[Header("Table Scale")]
		public float tableToWorldScale = 50f;

		// Token: 0x04003E43 RID: 15939
		public float pieceScale = 0.04f;

		// Token: 0x04003E44 RID: 15940
		public float gridSize = 0.02f;

		// Token: 0x04003E45 RID: 15941
		[Header("Layers")]
		public LayerMask allPiecesMask;

		// Token: 0x04003E46 RID: 15942
		[Header("Builder Options")]
		public bool useSnapRotation;

		// Token: 0x04003E47 RID: 15943
		public BuilderPlacementStyle usePlacementStyle;

		// Token: 0x04003E48 RID: 15944
		public bool buildInPlace;

		// Token: 0x04003E49 RID: 15945
		public BuilderOptionButton buttonSnapRotation;

		// Token: 0x04003E4A RID: 15946
		public BuilderOptionButton buttonSnapPosition;

		// Token: 0x04003E4B RID: 15947
		public BuilderOptionButton buttonSaveLayout;

		// Token: 0x04003E4C RID: 15948
		public BuilderOptionButton buttonClearLayout;

		// Token: 0x04003E4D RID: 15949
		[HideInInspector]
		public List<BuilderAttachGridPlane> baseGridPlanes;

		// Token: 0x04003E4E RID: 15950
		[Header("Piece Fabrication")]
		public List<GameObject> builtInPieceRoots;

		// Token: 0x04003E4F RID: 15951
		private List<BuilderPiece> basePieces;

		// Token: 0x04003E50 RID: 15952
		public List<BuilderPiecePrivatePlot> allPrivatePlots;

		// Token: 0x04003E51 RID: 15953
		public BuilderPiece armShelfPieceType;

		// Token: 0x04003E52 RID: 15954
		private int nextPieceId;

		// Token: 0x04003E53 RID: 15955
		[HideInInspector]
		public List<BuilderTable.BuildPieceSpawn> buildPieceSpawns;

		// Token: 0x04003E54 RID: 15956
		[HideInInspector]
		public List<BuilderShelf> shelves;

		// Token: 0x04003E55 RID: 15957
		[NonSerialized]
		public List<BuilderPiece> pieces = new List<BuilderPiece>(1024);

		// Token: 0x04003E56 RID: 15958
		private Dictionary<int, int> pieceIDToIndexCache = new Dictionary<int, int>(1024);

		// Token: 0x04003E57 RID: 15959
		[HideInInspector]
		public Dictionary<int, int> plotOwners;

		// Token: 0x04003E58 RID: 15960
		private bool doesLocalPlayerOwnPlot;

		// Token: 0x04003E59 RID: 15961
		public Dictionary<int, int> playerToArmShelfLeft;

		// Token: 0x04003E5A RID: 15962
		public Dictionary<int, int> playerToArmShelfRight;

		// Token: 0x04003E5B RID: 15963
		private HashSet<int> builderPiecesVisited = new HashSet<int>(128);

		// Token: 0x04003E5C RID: 15964
		public BuilderResources totalResources;

		// Token: 0x04003E5D RID: 15965
		[Tooltip("Resources reserved for conveyors and dispensers")]
		public BuilderResources totalReservedResources;

		// Token: 0x04003E5E RID: 15966
		public BuilderResources resourcesPerPrivatePlot;

		// Token: 0x04003E5F RID: 15967
		[NonSerialized]
		public int[] maxResources;

		// Token: 0x04003E60 RID: 15968
		private int[] plotMaxResources;

		// Token: 0x04003E61 RID: 15969
		[NonSerialized]
		public int[] usedResources;

		// Token: 0x04003E62 RID: 15970
		[NonSerialized]
		public int[] reservedResources;

		// Token: 0x04003E63 RID: 15971
		private List<int> playersInBuilder;

		// Token: 0x04003E64 RID: 15972
		private List<IBuilderPieceFunctional> activeFunctionalComponents = new List<IBuilderPieceFunctional>(128);

		// Token: 0x04003E65 RID: 15973
		private List<IBuilderPieceFunctional> funcComponentsToRegister = new List<IBuilderPieceFunctional>(10);

		// Token: 0x04003E66 RID: 15974
		private List<IBuilderPieceFunctional> funcComponentsToUnregister = new List<IBuilderPieceFunctional>(10);

		// Token: 0x04003E67 RID: 15975
		private List<IBuilderPieceFunctional> fixedUpdateFunctionalComponents = new List<IBuilderPieceFunctional>(128);

		// Token: 0x04003E68 RID: 15976
		private List<IBuilderPieceFunctional> funcComponentsToRegisterFixed = new List<IBuilderPieceFunctional>(10);

		// Token: 0x04003E69 RID: 15977
		private List<IBuilderPieceFunctional> funcComponentsToUnregisterFixed = new List<IBuilderPieceFunctional>(10);

		// Token: 0x04003E6A RID: 15978
		private const int MAX_SPHERE_CHECK_RESULTS = 1024;

		// Token: 0x04003E6B RID: 15979
		private NativeList<BuilderGridPlaneData> gridPlaneData;

		// Token: 0x04003E6C RID: 15980
		private NativeList<BuilderGridPlaneData> checkGridPlaneData;

		// Token: 0x04003E6D RID: 15981
		private NativeArray<ColliderHit> nearbyPiecesResults;

		// Token: 0x04003E6E RID: 15982
		private NativeArray<OverlapSphereCommand> nearbyPiecesCommands;

		// Token: 0x04003E6F RID: 15983
		private List<BuilderPotentialPlacement> allPotentialPlacements;

		// Token: 0x04003E70 RID: 15984
		private static HashSet<BuilderPiece> tempPieceSet = new HashSet<BuilderPiece>(512);

		// Token: 0x04003E71 RID: 15985
		private BuilderTable.TableState tableState;

		// Token: 0x04003E72 RID: 15986
		private bool inRoom;

		// Token: 0x04003E73 RID: 15987
		private bool inBuilderZone;

		// Token: 0x04003E74 RID: 15988
		private static int DROPPED_PIECE_LIMIT = 100;

		// Token: 0x04003E75 RID: 15989
		public static string nextUpdateOverride = string.Empty;

		// Token: 0x04003E76 RID: 15990
		private List<BuilderPiece> droppedPieces;

		// Token: 0x04003E77 RID: 15991
		private List<BuilderTable.DroppedPieceData> droppedPieceData;

		// Token: 0x04003E78 RID: 15992
		private HashSet<int>[] repelledPieceRoots;

		// Token: 0x04003E79 RID: 15993
		private int repelHistoryLength = 3;

		// Token: 0x04003E7A RID: 15994
		private int repelHistoryIndex;

		// Token: 0x04003E7B RID: 15995
		private bool hasRequestedConfig;

		// Token: 0x04003E7C RID: 15996
		private bool isDirty;

		// Token: 0x04003E7D RID: 15997
		private bool hasQueriedSaveTime;

		// Token: 0x04003E7E RID: 15998
		private DateTime[] saveDateTime = new DateTime[BuilderScanKiosk.NUM_SAVE_SLOTS];

		// Token: 0x04003E7F RID: 15999
		private static List<string> saveDateKeys = new List<string>(BuilderScanKiosk.NUM_SAVE_SLOTS);

		// Token: 0x04003E80 RID: 16000
		private bool saveInProgress;

		// Token: 0x04003E81 RID: 16001
		private int currentSaveSlot = -1;

		// Token: 0x04003E82 RID: 16002
		[HideInInspector]
		public UnityEvent OnSaveTimeUpdated;

		// Token: 0x04003E83 RID: 16003
		[HideInInspector]
		public UnityEvent<bool> OnSaveDirtyChanged;

		// Token: 0x04003E84 RID: 16004
		[HideInInspector]
		public UnityEvent<string> OnSaveFailure;

		// Token: 0x04003E85 RID: 16005
		[HideInInspector]
		public UnityEvent OnTableConfigurationUpdated;

		// Token: 0x04003E86 RID: 16006
		[HideInInspector]
		public UnityEvent<bool> OnLocalPlayerClaimedPlot;

		// Token: 0x04003E87 RID: 16007
		[HideInInspector]
		public UnityEvent<int, int> OnBuilderTimerStopped;

		// Token: 0x04003E88 RID: 16008
		[HideInInspector]
		public UnityEvent OnBuilderLocalTimerStarted;

		// Token: 0x04003E89 RID: 16009
		private List<BuilderTable.BuilderCommand> queuedBuildCommands;

		// Token: 0x04003E8A RID: 16010
		private List<BuilderAction> rollBackActions;

		// Token: 0x04003E8B RID: 16011
		private List<BuilderTable.BuilderCommand> rollBackBufferedCommands;

		// Token: 0x04003E8C RID: 16012
		private List<BuilderTable.BuilderCommand> rollForwardCommands;

		// Token: 0x04003E8D RID: 16013
		public static BuilderTable instance;

		// Token: 0x04003E8E RID: 16014
		private bool isSetup;

		// Token: 0x04003E8F RID: 16015
		[Header("Snap Params")]
		public BuilderTable.SnapParams pushAndEaseParams;

		// Token: 0x04003E90 RID: 16016
		public BuilderTable.SnapParams overlapParams;

		// Token: 0x04003E91 RID: 16017
		private BuilderTable.SnapParams currSnapParams;

		// Token: 0x04003E92 RID: 16018
		public int maxPlacementChildDepth = 5;

		// Token: 0x04003E93 RID: 16019
		private static List<BuilderPiece> tempPieces = new List<BuilderPiece>(256);

		// Token: 0x04003E94 RID: 16020
		private static List<BuilderConveyor> tempConveyors = new List<BuilderConveyor>(256);

		// Token: 0x04003E95 RID: 16021
		private static List<BuilderDispenserShelf> tempDispensers = new List<BuilderDispenserShelf>(256);

		// Token: 0x04003E96 RID: 16022
		private static List<BuilderRecycler> tempRecyclers = new List<BuilderRecycler>(5);

		// Token: 0x04003E97 RID: 16023
		private static List<BuilderTable.BuilderCommand> tempRollForwardCommands = new List<BuilderTable.BuilderCommand>(128);

		// Token: 0x04003E98 RID: 16024
		private static List<BuilderPiece> tempDeletePieces = new List<BuilderPiece>(1024);

		// Token: 0x04003E99 RID: 16025
		public const int MAX_PIECE_DATA = 2560;

		// Token: 0x04003E9A RID: 16026
		public const int MAX_GRID_PLANE_DATA = 10240;

		// Token: 0x04003E9B RID: 16027
		public const int MAX_PRIVATE_PLOT_DATA = 64;

		// Token: 0x04003E9C RID: 16028
		public const int MAX_PLAYER_DATA = 64;

		// Token: 0x04003E9D RID: 16029
		private BuilderTableData tableData;

		// Token: 0x04003E9E RID: 16030
		private static string personalBuildKey = "MyBuild";

		// Token: 0x04003E9F RID: 16031
		private static HashSet<BuilderTable.SnapOverlapKey> tempDuplicateOverlaps = new HashSet<BuilderTable.SnapOverlapKey>(16384);

		// Token: 0x04003EA0 RID: 16032
		private static List<BuilderPiece> childPieces = new List<BuilderPiece>(4096);

		// Token: 0x04003EA1 RID: 16033
		private static List<BuilderPiece> rootPieces = new List<BuilderPiece>(4096);

		// Token: 0x04003EA2 RID: 16034
		private static List<int> overlapPieces = new List<int>(4096);

		// Token: 0x04003EA3 RID: 16035
		private static List<int> overlapOtherPieces = new List<int>(4096);

		// Token: 0x04003EA4 RID: 16036
		private static List<long> overlapPacked = new List<long>(4096);

		// Token: 0x04003EA5 RID: 16037
		private static Dictionary<long, int> snapOverlapSanity = new Dictionary<long, int>(16384);

		// Token: 0x04003EA6 RID: 16038
		private static List<int> tempPeiceIds = new List<int>(4096);

		// Token: 0x04003EA7 RID: 16039
		private static List<int> tempParentPeiceIds = new List<int>(4096);

		// Token: 0x04003EA8 RID: 16040
		private static List<int> tempAttachIndexes = new List<int>(4096);

		// Token: 0x04003EA9 RID: 16041
		private static List<int> tempParentAttachIndexes = new List<int>(4096);

		// Token: 0x04003EAA RID: 16042
		private static List<int> tempParentActorNumbers = new List<int>(4096);

		// Token: 0x04003EAB RID: 16043
		private static List<bool> tempInLeftHand = new List<bool>(4096);

		// Token: 0x04003EAC RID: 16044
		private static List<int> tempPiecePlacement = new List<int>(4096);

		// Token: 0x020009B9 RID: 2489
		[Serializable]
		public class BuildPieceSpawn
		{
			// Token: 0x04003EAD RID: 16045
			public GameObject buildPiecePrefab;

			// Token: 0x04003EAE RID: 16046
			public int count = 1;
		}

		// Token: 0x020009BA RID: 2490
		public enum BuilderCommandType
		{
			// Token: 0x04003EB0 RID: 16048
			Create,
			// Token: 0x04003EB1 RID: 16049
			Place,
			// Token: 0x04003EB2 RID: 16050
			Grab,
			// Token: 0x04003EB3 RID: 16051
			Drop,
			// Token: 0x04003EB4 RID: 16052
			Remove,
			// Token: 0x04003EB5 RID: 16053
			Paint,
			// Token: 0x04003EB6 RID: 16054
			Recycle,
			// Token: 0x04003EB7 RID: 16055
			ClaimPlot,
			// Token: 0x04003EB8 RID: 16056
			FreePlot,
			// Token: 0x04003EB9 RID: 16057
			CreateArmShelf,
			// Token: 0x04003EBA RID: 16058
			PlayerLeftRoom,
			// Token: 0x04003EBB RID: 16059
			FunctionalStateChange,
			// Token: 0x04003EBC RID: 16060
			SetSelection,
			// Token: 0x04003EBD RID: 16061
			Repel
		}

		// Token: 0x020009BB RID: 2491
		public enum TableState
		{
			// Token: 0x04003EBF RID: 16063
			WaitingForZoneAndRoom,
			// Token: 0x04003EC0 RID: 16064
			WaitingForInitalBuild,
			// Token: 0x04003EC1 RID: 16065
			ReceivingInitialBuild,
			// Token: 0x04003EC2 RID: 16066
			WaitForInitialBuildMaster,
			// Token: 0x04003EC3 RID: 16067
			WaitForMasterResync,
			// Token: 0x04003EC4 RID: 16068
			ReceivingMasterResync,
			// Token: 0x04003EC5 RID: 16069
			InitialBuild,
			// Token: 0x04003EC6 RID: 16070
			ExecuteQueuedCommands,
			// Token: 0x04003EC7 RID: 16071
			Ready,
			// Token: 0x04003EC8 RID: 16072
			BadData
		}

		// Token: 0x020009BC RID: 2492
		public enum DroppedPieceState
		{
			// Token: 0x04003ECA RID: 16074
			None = -1,
			// Token: 0x04003ECB RID: 16075
			Light,
			// Token: 0x04003ECC RID: 16076
			Heavy
		}

		// Token: 0x020009BD RID: 2493
		private struct DroppedPieceData
		{
			// Token: 0x04003ECD RID: 16077
			public BuilderTable.DroppedPieceState droppedState;

			// Token: 0x04003ECE RID: 16078
			public float speedThreshCrossedTime;

			// Token: 0x04003ECF RID: 16079
			public float filteredSpeed;
		}

		// Token: 0x020009BE RID: 2494
		public struct BuilderCommand
		{
			// Token: 0x04003ED0 RID: 16080
			public BuilderTable.BuilderCommandType type;

			// Token: 0x04003ED1 RID: 16081
			public int pieceType;

			// Token: 0x04003ED2 RID: 16082
			public int pieceId;

			// Token: 0x04003ED3 RID: 16083
			public int attachPieceId;

			// Token: 0x04003ED4 RID: 16084
			public int parentPieceId;

			// Token: 0x04003ED5 RID: 16085
			public int parentAttachIndex;

			// Token: 0x04003ED6 RID: 16086
			public int attachIndex;

			// Token: 0x04003ED7 RID: 16087
			public Vector3 localPosition;

			// Token: 0x04003ED8 RID: 16088
			public Quaternion localRotation;

			// Token: 0x04003ED9 RID: 16089
			public byte twist;

			// Token: 0x04003EDA RID: 16090
			public sbyte bumpOffsetX;

			// Token: 0x04003EDB RID: 16091
			public sbyte bumpOffsetZ;

			// Token: 0x04003EDC RID: 16092
			public Vector3 velocity;

			// Token: 0x04003EDD RID: 16093
			public Vector3 angVelocity;

			// Token: 0x04003EDE RID: 16094
			public bool isLeft;

			// Token: 0x04003EDF RID: 16095
			public int materialType;

			// Token: 0x04003EE0 RID: 16096
			public NetPlayer player;

			// Token: 0x04003EE1 RID: 16097
			public BuilderPiece.State state;

			// Token: 0x04003EE2 RID: 16098
			public bool isQueued;

			// Token: 0x04003EE3 RID: 16099
			public bool canRollback;

			// Token: 0x04003EE4 RID: 16100
			public int localCommandId;

			// Token: 0x04003EE5 RID: 16101
			public int serverTimeStamp;
		}

		// Token: 0x020009BF RID: 2495
		[Serializable]
		public struct SnapParams
		{
			// Token: 0x04003EE6 RID: 16102
			public float minOffsetY;

			// Token: 0x04003EE7 RID: 16103
			public float maxOffsetY;

			// Token: 0x04003EE8 RID: 16104
			public float maxUpDotProduct;

			// Token: 0x04003EE9 RID: 16105
			public float maxTwistDotProduct;

			// Token: 0x04003EEA RID: 16106
			public float snapAttachDistance;

			// Token: 0x04003EEB RID: 16107
			public float snapDelayTime;

			// Token: 0x04003EEC RID: 16108
			public float snapDelayOffsetDist;

			// Token: 0x04003EED RID: 16109
			public float unSnapDelayTime;

			// Token: 0x04003EEE RID: 16110
			public float unSnapDelayDist;

			// Token: 0x04003EEF RID: 16111
			public float maxBlockSnapDist;
		}

		// Token: 0x020009C0 RID: 2496
		private struct SnapOverlapKey
		{
			// Token: 0x06003DF1 RID: 15857 RVA: 0x000585BF File Offset: 0x000567BF
			public override int GetHashCode()
			{
				return HashCode.Combine<int, int>(this.piece.GetHashCode(), this.otherPiece.GetHashCode());
			}

			// Token: 0x06003DF2 RID: 15858 RVA: 0x000585DC File Offset: 0x000567DC
			public bool Equals(BuilderTable.SnapOverlapKey other)
			{
				return this.piece == other.piece && this.otherPiece == other.otherPiece;
			}

			// Token: 0x06003DF3 RID: 15859 RVA: 0x000585FC File Offset: 0x000567FC
			public override bool Equals(object o)
			{
				return o is BuilderTable.SnapOverlapKey && this.Equals((BuilderTable.SnapOverlapKey)o);
			}

			// Token: 0x04003EF0 RID: 16112
			public long piece;

			// Token: 0x04003EF1 RID: 16113
			public long otherPiece;
		}
	}
}
