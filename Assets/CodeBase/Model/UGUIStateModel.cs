using CodeBase.Helpers;

namespace CodeBase.Model
{
    public enum UGUIState
    {
        Menu,
        Gameplay,
        Results,
        Scores,
    }

    public class UGUIStateModel : StateModel<UGUIState>
    {
        public UGUIStateModel() : base(UGUIState.Menu)
        {
        }
    }
}
