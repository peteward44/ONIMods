// FarmTile
public class WildFarmTile : StateMachineComponent<WildFarmTile.SMInstance>
{
	public class SMInstance : GameStateMachine<States, SMInstance, WildFarmTile, object>.GameInstance
	{
		public SMInstance(WildFarmTile master)
			: base(master)
		{
		}

		public bool HasWater()
		{
			PrimaryElement primaryElement = base.master.storage.FindPrimaryElement(SimHashes.Water);
			return primaryElement != null && primaryElement.Mass > 0f;
		}
	}

	public class States : GameStateMachine<States, SMInstance, WildFarmTile>
	{
		public class FarmStates : State
		{
			public State wet;

			public State dry;
		}

		public FarmStates empty;

		public FarmStates full;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = empty;
			empty.EventTransition(GameHashes.OccupantChanged, full, (SMInstance smi) => smi.master.plantablePlot.Occupant != null);
			empty.wet.EventTransition(GameHashes.OnStorageChange, empty.dry, (SMInstance smi) => !smi.HasWater());
			empty.dry.EventTransition(GameHashes.OnStorageChange, empty.wet, (SMInstance smi) => !smi.HasWater());
			full.EventTransition(GameHashes.OccupantChanged, empty, (SMInstance smi) => smi.master.plantablePlot.Occupant == null);
			full.wet.EventTransition(GameHashes.OnStorageChange, full.dry, (SMInstance smi) => !smi.HasWater());
			full.dry.EventTransition(GameHashes.OnStorageChange, full.wet, (SMInstance smi) => !smi.HasWater());
		}
	}

	[MyCmpReq]
	private PlantablePlot plantablePlot;

	[MyCmpReq]
	private Storage storage;

	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}
}

