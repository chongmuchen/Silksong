using TeamCherry.Localization;

namespace HutongGames.PlayMaker.Actions;

public class GetLocalisedString : FsmStateAction
{
	public LocalisedFsmString Cell;

	[UIHint(UIHint.Variable)]
	public FsmString StoreString;

	public override void Reset()
	{
		Cell = null;
		StoreString = null;
	}

	public override void OnEnter()
	{
		StoreString.Value = Cell;
		Finish();
	}
}
