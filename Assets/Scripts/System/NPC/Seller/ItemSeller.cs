using UnityEngine;

public class ItemSeller : MonoBehaviour
{
    public static bool isMenuOpen = false;

    [SerializeField]
    private GameObject _itemsMenu;

    private bool isInRange = false;
    // Used to indicate that the player is outside the seller range after closing the item menu
    private bool _isOut = true; 

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.tag + " : " + _isOut);
        if (collider.transform.CompareTag("Player") && _isOut)
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.transform.CompareTag("Player"))
        {
            Debug.Log("Out");
            isInRange = false;
            _isOut = true;
        }
    }

    private void LateUpdate()
    {
        if (isInRange && !isMenuOpen)
        {// The player is in the range
            OpenItemsMenu();
            isInRange = false;
            _isOut = false;
        }
        else if (Input.GetKey(KeyCode.Escape) && isMenuOpen)
        {
            CloseItemsMenu();
        }
    }

    public void OpenItemsMenu()
    {
        PlayerInfo.isStopped = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _itemsMenu.SetActive(true);
        isMenuOpen = true;
    }

    public void CloseItemsMenu()
    {
        PlayerInfo.isStopped = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _itemsMenu.SetActive(false);
        isMenuOpen = false;
    }
}
