public abstract class BaseCommand
{
  // Abstract class means that any child of baseCommand MUST override this function
  public abstract bool Execute(ControllableCharacter character);
}
