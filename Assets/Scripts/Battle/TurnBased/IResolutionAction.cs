
namespace FirstWave.Niot.Battle.TurnBased
{
	public interface IResolutionAction
	{
		void Execute(BattleCommand command, int? damage);
	}
}
