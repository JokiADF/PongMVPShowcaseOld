using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Helpers
{
    public class BoundItemsContainer<T> : IDisposable
    {
        private readonly CompositeDisposable _disposableList = new();
        
        public readonly Dictionary<T, GameObject> InstantiatedGameObjects = new();
        public readonly GameObject ItemPrefab;
        public readonly GameObject ItemHolder;

        private Subject<BoundItemsContainerEvent<T>> _add = null;
        private Subject<BoundItemsContainerEvent<T>> _remove = null;

        public bool IsCollectionInitialized { get; private set; }
        public bool DestroyOnRemove { get; set; } = true;
        
        public BoundItemsContainer(GameObject itemPrefab, GameObject itemHolder)
        {
            ItemPrefab = itemPrefab;
            ItemHolder = itemHolder;
        }

        public void Initialize(ReactiveCollection<T> collectionToBindTo)
        {
            if (IsCollectionInitialized)
                throw new InvalidOperationException($"'{nameof(Initialize)}' can only be called once.");

            collectionToBindTo.ObserveAdd().Subscribe(evt => AddHandler(evt.Value)).AddTo(_disposableList);
            collectionToBindTo.ObserveRemove().Subscribe(RemoveHandler).AddTo(_disposableList);
            collectionToBindTo.ObserveReset().Subscribe(ResetHandler).AddTo(_disposableList);

            foreach (var existingItem in collectionToBindTo) 
                AddHandler(existingItem);

            IsCollectionInitialized = true;
        }

        private void ResetHandler(Unit obj)
        {
            var modelList = InstantiatedGameObjects.Keys.ToList();

            foreach (var t in modelList)
            {
                RemoveItem(t);
            }
        }

        private void RemoveHandler(CollectionRemoveEvent<T> collectionRemoveEvent)
        {
            var removedModel = collectionRemoveEvent.Value;
            RemoveItem(removedModel);
        }

        private void RemoveItem(T modelToRemove)
        {
            var gameObjectToRemove = InstantiatedGameObjects[modelToRemove];

            if (DestroyOnRemove)
            {
                Object.Destroy(gameObjectToRemove);
                InstantiatedGameObjects.Remove(modelToRemove);
            }

            _remove?.OnNext(new BoundItemsContainerEvent<T>(gameObjectToRemove, modelToRemove));
        }

        private void AddHandler(T addedModel)
        {
            var newGameObject = Object.Instantiate(ItemPrefab, ItemHolder.transform);
            InstantiatedGameObjects.Add(addedModel, newGameObject);

            _add?.OnNext(new BoundItemsContainerEvent<T>(newGameObject, addedModel));
        }

        public IObservable<BoundItemsContainerEvent<T>> ObserveAdd()
        {
            return _add ??= new Subject<BoundItemsContainerEvent<T>>();
        }

        public IObservable<BoundItemsContainerEvent<T>> ObserveRemove()
        {
            return _remove ??= new Subject<BoundItemsContainerEvent<T>>();
        }

        public void Dispose()
        {
            _disposableList.Clear();
        }
    }

    public struct BoundItemsContainerEvent<T>
    {
        public GameObject GameObject { get; private set; }

        public T Model { get; private set; }

        public BoundItemsContainerEvent(GameObject gameObject, T model) : this()
        {
            GameObject = gameObject;
            Model = model;
        }
    }
}
