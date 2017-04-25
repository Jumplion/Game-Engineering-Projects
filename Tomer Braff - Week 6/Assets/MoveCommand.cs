using UnityEngine;

public class MoveCommand : BaseCommand
{
  Vector3 targetLocation;

  public MoveCommand(Vector3 targetLocation)
  {
    this.targetLocation = targetLocation;
  }

  public override bool Execute(ControllableCharacter character)
  {
    return character.MoveToLocation(targetLocation);
  }
}
