using UnityEngine;

public class HuntCommand : BaseCommand
{
  ControllableCharacter target;
  float killRadius;

  public HuntCommand(RaycastHit h, float killRadius)
  {
    this.target = h.transform.GetComponent<ControllableCharacter>();
    this.killRadius = killRadius;

    target.MarkSelfAsTarget(true);
  }

  public override bool Execute(ControllableCharacter character)
  {
    character.MoveToLocation(target.transform.position);

    if((character.transform.position - target.transform.position).magnitude < killRadius)
      return character.KillTargetCharacter(target, killRadius);

    return false;
  }
}
