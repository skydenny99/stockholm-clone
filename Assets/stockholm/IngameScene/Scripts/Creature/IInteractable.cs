public interface IInteractable
{
	void Interact(InteractMessage message, params object[] param);
}

public enum InteractMessage
{
	EnterNPC,
	ExitNPC
};