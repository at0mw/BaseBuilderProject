using System;
using System.Collections.Generic;
using BuildingSystem.Scripts.Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace BuildingSystem.Scripts.UI_System.BuildControl {
    public class UIBuildControlsManager: MonoBehaviour, IButtonSubject {
        private readonly List<IButtonObserver> _buttonObservers = new();

        [SerializeField] private GameObject gameUi;
        
        [SerializeField]private GameObject constructModeButton;
        [SerializeField]private GameObject deconstructModeButton;

        private void Start() {
            // Add event handlers to the buttons
            InitialiseButtons();
        }

        private void InitialiseButtons() {
            ListenToConstructButton();
            ListenToDeconstructButton();
            
            SubscribeUiManager();
        }

        private void SubscribeUiManager() {
            // ReSharper disable once Unity.NoNullPropagation
            IButtonObserver tempButtonObserver = gameUi?.GetComponent<UIManager>();
            if (tempButtonObserver is null) return;

            Subscribe(tempButtonObserver);
        }

        private void ListenToDeconstructButton() {
            var deconstructModeBtn = deconstructModeButton.GetComponent<Button>();
            // ReSharper disable once Unity.NoNullPropagation
            deconstructModeBtn?.onClick.AddListener(() => {
                // Notify observers that the deconstruct mode button was clicked
                NotifyClickedButton(new ButtonClickMessage() {
                    MessageType = MessageType.Info,
                    ButtonType = ButtonType.DeconstructMode,
                    MessageText = "Deconstruct Clicked"
                });
            });
        }

        private void ListenToConstructButton() {
            var constructModeBtn = constructModeButton.GetComponent<Button>();
            // ReSharper disable once Unity.NoNullPropagation
            constructModeBtn?.onClick.AddListener(() => {
                // Notify observers that the construct mode button was clicked
                NotifyClickedButton(new ButtonClickMessage() {
                    MessageType = MessageType.Info,
                    ButtonType = ButtonType.ConstructMode,
                    MessageText = "Construct Clicked"
                });
            });
        }


        #region IButtonSubject Update and Handle Observers
        public void Subscribe(IButtonObserver observer_)
        {
            _buttonObservers.Add(observer_);
        }

        public void Unsubscribe(IButtonObserver observer_)
        {
            _buttonObservers.Remove(observer_);
        }

        public void NotifyClickedButton(ButtonClickMessage message_) {
            if (_buttonObservers.Count == 0) return;
            foreach (var observer in _buttonObservers)
            {
                observer.NotifyPressed(message_);
            }
        }
        #endregion
    }
}