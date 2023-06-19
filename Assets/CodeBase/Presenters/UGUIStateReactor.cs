using CodeBase.Model;
using SpaceInvaders.Helpers;
using Zenject;

namespace CodeBase.Presenters
{
    public class UGUIStateReactor : StateReactor<UGUIState>
    {
        private UGUIStateModel _uguiState;

        protected override StateModel<UGUIState> Model => _uguiState;

        [Inject]
        private void Construct(UGUIStateModel uguiState) =>
            _uguiState = uguiState;
    }
}
