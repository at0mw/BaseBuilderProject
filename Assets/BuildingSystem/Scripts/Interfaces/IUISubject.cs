using UnityEditor;

namespace BuildingSystem.Scripts.Interfaces {
    public interface IUISubject {
        public void Attach(IUIObserver observer_);
        public void Detach(IUIObserver observer_);
        public void NotifyUIUpdate(UpdateMessage message_);
    }
    
    public struct UpdateMessage
    {
        public ButtonType ButtonType;
    }
    
}