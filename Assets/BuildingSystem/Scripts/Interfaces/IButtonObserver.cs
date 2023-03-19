namespace BuildingSystem.Scripts.Interfaces {
    public interface IButtonObserver {
        public void NotifyPressed(ButtonClickMessage message_);
    }
}