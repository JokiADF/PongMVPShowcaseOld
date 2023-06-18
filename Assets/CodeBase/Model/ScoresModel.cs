using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Services.Storage;
using UniRx;

namespace CodeBase.Model
{
    [Serializable]
    public class ScoreItem
    {
        public int score;
        public string date;
    }

    [Serializable]
    public class Scoreboard
    {
        public ScoreItem[] items;
    }

    public class ScoresModel
    {
        private const string StorageKey = "Scoreboard";
        
        public ReactiveCollection<ScoreItem> Scoreboard { get; private set; }

        private readonly IStorageService _storageService;


        public ScoresModel(IStorageService storageService)
        {
            _storageService = storageService;
            
            Scoreboard = new ReactiveCollection<ScoreItem>();
        }

        public void Add(ScoreItem scoreItem)
        {
            var originalList = Scoreboard.ToList();
            originalList.Add(scoreItem);

            var orderedList = originalList
                .OrderByDescending(x => x.score)
                .Take(10).ToList();

            UpdateScoreboard(orderedList);
        }
        

        public void Save()
        {
            _storageService.Save(StorageKey, new Scoreboard()
            {
                items = Scoreboard.ToArray()
            });
        }

        public void Load()
        {
            var scoreboard = _storageService.Load<Scoreboard>(StorageKey);

            if (scoreboard != null) 
                UpdateScoreboard(scoreboard.items);
        }

        private void UpdateScoreboard(IList<ScoreItem> items)
        {
            if (items == null)
                return;

            Scoreboard.Clear();

            foreach (var item in items) 
                Scoreboard.Add(item);
        }
    }
}
