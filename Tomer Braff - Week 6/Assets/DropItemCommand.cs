using UnityEngine;

public class DropItemCommand : BaseCommand
{
  Vector3 targetLocation;
  GameObject item;

  public DropItemCommand(Vector3 location, GameObject item)
  {
    this.targetLocation = location;
    this.item = item;
  }

  public override bool Execute(ControllableCharacter character)
  {
    if (character.MoveToLocation(targetLocation))
      return character.DropItem(item, targetLocation);

    return false;
  }
}
