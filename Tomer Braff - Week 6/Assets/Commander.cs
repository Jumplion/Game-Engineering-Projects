using UnityEngine;

public class Commander : MonoBehaviour
{
  public ControllableCharacter currentSelection;
  public GameObject selectIndicator;
  public GameObject item;
  public float indicatorHeight = 1.0f;
  public float characterKillRadius = 1.0f;

	// Update is called once per frame
	void Update ()
  {
    // If left click, change/remove current selection
    CheckForCharacterSelection();

    // If character is selected, check for right click to enact command
    if (currentSelection)
      CheckForCommandIssue();

    UpdateSelectionIndicator();
	}

  void CheckForCharacterSelection()
  {
    // Check for left click
    if (Input.GetButtonDown("Fire1"))
    {
      RaycastHit hit;
      // If the component is null, it won't register as selected anyway
      if (MouseCast(out hit))
        currentSelection = hit.transform.GetComponent<ControllableCharacter>();
    }
  }

  void CheckForCommandIssue()
  {
    if (Input.GetButtonDown("Fire2"))
    {
      RaycastHit hit;
      if(MouseCast(out hit))
      {
        if (hit.transform.tag == "Character")
          currentSelection.AddCommand(new HuntCommand(hit, characterKillRadius));
        else if (hit.transform.tag == "Item")
          currentSelection.AddCommand(new PickUpItemCommand(hit.transform.gameObject));
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
          currentSelection.AddCommand(new DropItemCommand(hit.point, item));
        else
          currentSelection.AddCommand(new MoveCommand(hit.point));
      }
    }
  }

  bool MouseCast(out RaycastHit hit, float maxDistance = 9999f, int layerMask = -1)
  {
    return Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, maxDistance, layerMask);
  }

  void UpdateSelectionIndicator()
  {
    if (currentSelection)
    {
      selectIndicator.SetActive(true);
      Vector3 pos = currentSelection.transform.position;
      selectIndicator.transform.position = new Vector3(pos.x, pos.y + indicatorHeight, pos.z);
    }
    else
      selectIndicator.SetActive(false);
  }
}
