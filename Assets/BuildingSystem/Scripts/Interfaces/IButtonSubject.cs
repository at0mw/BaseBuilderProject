using UnityEditor;

namespace BuildingSystem.Scripts.Interfaces {
    public interface IButtonSubject {
        public void Subscribe(IButtonObserver observer_);
        public void Unsubscribe(IButtonObserver observer_);
        public void NotifyClickedButton(ButtonClickMessage message_);
    }
    
    public struct ButtonClickMessage
    {
        public MessageType MessageType;
        public ButtonType ButtonType;
        // For testing
        public string MessageText;
    }

    public enum ButtonType {
        ConstructMode = 1,
        DeconstructMode = 2
    }
}