using System.Collections.Generic;
using UnityEngine;

public class ControllableCharacter : MonoBehaviour
{
  Queue<BaseCommand> commandQueue = new Queue<BaseCommand>();
  public float moveSpeed = 5.0f;
  public GameObject huntingMark;

  private void Awake()
  {
    huntingMark.SetActive(false);
  }

  void Update()
  {
    ExecuteCommands();
  }

  public void AddCommand(BaseCommand newCommand)
  {
    commandQueue.Enqueue(newCommand);
  }

  void ExecuteCommands()
  {
    if(commandQueue.Count != 0)
    {
      BaseCommand currentCommand = commandQueue.Peek();
      // If we have a current command
      if(currentCommand != null)
      {
        // If the current command has finished executing
        if (currentCommand.Execute(this))
          commandQueue.Dequeue();
      }
    }
  }

  //*****************************
  // Helper methods for commands.

  public bool MoveToLocation(Vector3 targetLocation)
  {
    Vector3 currentPosition = transform.position;
    Vector3 moveDelta = targetLocation - currentPosition;

    // If the distance we have to move is less than how far we want to move
    if(moveDelta.magnitude < moveSpeed * Time.deltaTime)
    {
      transform.position = targetLocation;
      return true;
    }
    else
    {
      transform.position += moveDelta.normalized * moveSpeed * Time.deltaTime;
      return false;
    }
  }

  public bool KillTargetCharacter(ControllableCharacter character, float killRadius)
  {
    Vector3 positionDifference = character.transform.position - transform.position;

    if (positionDifference.magnitude < killRadius)
    {
      if(character || character.gameObject)
        Destroy(character.gameObject);

      return true;
    }
    else
      return false;
  }

  public void MarkTarget(ControllableCharacter target, bool marked)
  {
    target.huntingMark.SetActive(marked);
  }

  public void MarkSelfAsTarget(bool marked)
  {
    this.huntingMark.SetActive(marked);
  }

  public bool DropItem(GameObject item, Vector3 location)
  {
    return Instantiate(item, location, Quaternion.identity);
  }

  public void PickUpItem(GameObject item)
  {
    Destroy(item);
  }
}
