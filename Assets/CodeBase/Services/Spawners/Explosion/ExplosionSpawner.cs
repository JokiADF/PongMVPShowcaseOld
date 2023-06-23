using System.Collections.Generic;
using CodeBase.Presenters;
using UnityEngine;

namespace CodeBase.Services.Spawners.Explosion
{
    public class ExplosionSpawner : IExplosionSpawner
    {
        private readonly ExplosionPresenter.Factory _factory;

        private readonly List<ExplosionPresenter> _explosions = new();

        public ExplosionSpawner(ExplosionPresenter.Factory factory) => 
            _factory = factory;

        public void Spawn(Vector3 position)
        {
            var explosion = _factory.Create(position);
            _explosions.Add(explosion);
        }

        public void Despawn(ExplosionPresenter explosion)
        {
            if (explosion == null || !_explosions.Contains(explosion))
                return;

            explosion.Dispose();
            _explosions.Remove(explosion);
        }

        public void DespawnAll()
        {
            foreach (var explosion in _explosions) 
                explosion.Dispose();

            _explosions.Clear();
        }
    }
}
