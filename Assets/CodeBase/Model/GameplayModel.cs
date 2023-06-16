using UniRx;

namespace CodeBase.Model
{
    public class GameplayModel
    {
        public ReactiveProperty<int> CurrentPlayerScore { get; private set; }
        public ReactiveProperty<int> CurrentEnemyScore { get; private set; }

        public GameplayModel()
        {
            CurrentPlayerScore = new ReactiveProperty<int>(0);
            CurrentEnemyScore = new ReactiveProperty<int>(0);
        }

        public void Reset()
        {
            CurrentPlayerScore.Value = 0;
            CurrentEnemyScore.Value = 0;
        }
    }
}
