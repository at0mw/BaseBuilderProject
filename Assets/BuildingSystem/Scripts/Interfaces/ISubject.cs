using UnityEditor;

namespace BuildingSystem.Scripts.Interfaces {
    public interface ISubject
    {
        public void Attach(IObserver observer_);
        public void Detach(IObserver observer_);
        public void Notify(Message message_);
    }

    public struct Message
    {
        public MessageType MessageType;
        // For testing
        public string MessageText;
    }
}