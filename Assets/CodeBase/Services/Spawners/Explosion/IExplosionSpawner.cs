using CodeBase.Presenters;
using UnityEngine;

namespace CodeBase.Services.Spawners.Explosion
{
    public interface IExplosionSpawner
    {
        void Despawn(ExplosionPresenter explosion);
        void DespawnAll();
        void Spawn(Vector3 position);
    }
}