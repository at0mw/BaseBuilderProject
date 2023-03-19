using System;
using System.Collections.Generic;
using BuildingSystem.Scripts.BuildingSystem;
using BuildingSystem.Scripts.Interfaces;
using UnityEngine;

namespace BuildingSystem.Scripts.UI_System {
    public class UIManager : MonoBehaviour, IButtonObserver, IUISubject {
        private readonly List<IUIObserver> _uiObservers = new();
        [SerializeField] private GameObject buildManager;

        private void Start() {
            // ReSharper disable once Unity.NoNullPropagation
            IUIObserver tempUIObserver = buildManager?.GetComponent<BuildManager>();
            if(tempUIObserver != null)
                Attach(tempUIObserver);
        }

        #region Observe BuildButtons
        public void NotifyPressed(ButtonClickMessage message_) {
            switch (message_.ButtonType)
            {
                case ButtonType.ConstructMode:
                    Debug.Log($"Notifying with {message_.MessageText}");
                    NotifyUIUpdate(new UpdateMessage {
                        ButtonType = message_.ButtonType
                    });
                    break;
                case ButtonType.DeconstructMode:
                    Debug.Log($"Notifying with {message_.MessageText}");
                    NotifyUIUpdate(new UpdateMessage {
                        ButtonType = message_.ButtonType
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Inform of UI Updates
        public void Attach(IUIObserver observer_) {
            _uiObservers.Add(observer_);
        }

        public void Detach(IUIObserver observer_) {
            _uiObservers.Remove(observer_);
        }

        public void NotifyUIUpdate(UpdateMessage message_) {
            if (_uiObservers.Count == 0) return;
            foreach (var observer in _uiObservers)
            {
                observer.NotifyUpdate(message_);
            }
        }
        #endregion

    }
}
