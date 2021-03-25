// PlanterBox
[SkipSaveFileSerialization]
public class WildPlanterBox : StateMachineComponent<WildPlanterBox.SMInstance>
{
	public class SMInstance : GameStateMachine<States, SMInstance, WildPlanterBox, object>.GameInstance
	{
		public SMInstance(WildPlanterBox master)
			: base(master)
		{
		}
	}

	public class States : GameStateMachine<States, SMInstance, WildPlanterBox>
	{
		public State empty;

		public State full;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = empty;
			empty.EventTransition(GameHashes.OccupantChanged, full, (SMInstance smi) => smi.master.plantablePlot.Occupant != null).PlayAnim("off");
			full.EventTransition(GameHashes.OccupantChanged, empty, (SMInstance smi) => smi.master.plantablePlot.Occupant == null).PlayAnim("on");
		}
	}

	[MyCmpReq]
	private PlantablePlot plantablePlot;

	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}
}

