using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements; // For UIDocument, VisualElement, and related classes
using UnityEngine.EventSystems;


public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private UIDocument inventoryUIDocument;

    private VisualElement inventoryUI;
    private bool isInventoryOpen = false;

    private void Start()
    {
        inventoryUI = inventoryUIDocument.rootVisualElement.Q<VisualElement>("PLAYER_INVENTORY_UI");
        inventoryUI.style.display = DisplayStyle.None;
    }

    public void ToggleInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isInventoryOpen = !isInventoryOpen;
            inventoryUI.style.display = isInventoryOpen ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
    
}
