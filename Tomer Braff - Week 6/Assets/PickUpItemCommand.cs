using UnityEngine;

public class PickUpItemCommand : BaseCommand
{
  GameObject item;

  public PickUpItemCommand(GameObject item)
  {
    this.item = item;
  }

  public override bool Execute(ControllableCharacter character)
  {
    if (character.MoveToLocation(item.transform.position))
    {
      character.PickUpItem(item);
      return true;
    }

    return false;
  }

}
