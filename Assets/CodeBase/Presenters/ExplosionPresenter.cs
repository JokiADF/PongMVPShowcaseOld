using System;
using CodeBase.Services.Spawners.Explosion;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace CodeBase.Presenters
{
    public class ExplosionPresenter : MonoBehaviour, IPoolable<Vector3, IMemoryPool>, IDisposable
    {
        [SerializeField] private ParticleSystem particle;

        private IExplosionSpawner _particleSpawner;

        private IMemoryPool _pool;

        [Inject]
        private void Construct(IExplosionSpawner particleSpawner) => 
            _particleSpawner = particleSpawner;

        public async void OnSpawned(Vector3 position, IMemoryPool pool)
        {
            _pool = pool;
            transform.position = position;
            particle.Play();

            await UniTask.Delay(Mathf.RoundToInt(particle.main.duration * 1000));
            
            _particleSpawner.Despawn(this);
        }

        public void OnDespawned()
        {
            _pool = null;
            particle.Stop();
            transform.position = Vector3.zero;
        }

        public void Dispose() => 
            _pool.Despawn(this);

        public class Factory : PlaceholderFactory<Vector3, ExplosionPresenter>
        {
        }
    }
}
