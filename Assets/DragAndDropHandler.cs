using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndDropHandler : MonoBehaviour
{

    [SerializeField] private UIItemSlot cursorSlot = null;
    private ItemSlot cursorItemSlot;

    //For actual clicking of things. 
    [SerializeField] private GraphicRaycaster m_Raycaster = null;
    private PointerEventData m_PointerEventData;
    [SerializeField] private EventSystem m_EventSystem = null;

    World world;

    private void Start()
    {

        world = GameObject.Find("World").GetComponent<World>();

        cursorItemSlot = new ItemSlot(cursorSlot);

    }

    private void Update()
    {

        if (!world.inUI)
            return;

        cursorSlot.transform.position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {

            HandleSlotClick(CheckForSlot());

        }

    }

    private void HandleSlotClick(UIItemSlot clickedSlot)
    {

        if (clickedSlot == null)
            return;

        //if the slot the we clicked has nothing on it and we are not current holding anything.
        if (!cursorSlot.HasItem && !clickedSlot.HasItem)
            return;

        if (clickedSlot.itemSlot.isCreative)
        {

            cursorItemSlot.EmptySlot();
            cursorItemSlot.InsertStack(clickedSlot.itemSlot.stack);

        }

        if (!cursorSlot.HasItem && clickedSlot.HasItem)
        {

            cursorItemSlot.InsertStack(clickedSlot.itemSlot.TakeAll());
            return;

        }

        if (cursorSlot.HasItem && !clickedSlot.HasItem)
        {
            //Pick an item up and place it on the slot. 
            clickedSlot.itemSlot.InsertStack(cursorItemSlot.TakeAll());
            return;

        }

        //If both slot have items
        if (cursorSlot.HasItem && clickedSlot.HasItem)
        {

            if (cursorSlot.itemSlot.stack.id != clickedSlot.itemSlot.stack.id)
            {

                ItemStack oldCursorSlot = cursorSlot.itemSlot.TakeAll();
                ItemStack oldSlot = clickedSlot.itemSlot.TakeAll();

                //Swapped an item in the slot. 
                clickedSlot.itemSlot.InsertStack(oldCursorSlot);
                cursorSlot.itemSlot.InsertStack(oldSlot);

            }

        }


    }

    private UIItemSlot CheckForSlot()
    {
        //Cast a ray at the mouse position and return a list containing all of the UI elements that are on the mouse position. 
        m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        m_Raycaster.Raycast(m_PointerEventData, results);

        //Loop through all of the raycast results that we got. 
        foreach (RaycastResult result in results)
        {

            if (result.gameObject.tag == "UIItemSlot")
                return result.gameObject.GetComponent<UIItemSlot>();

        }

        return null;

    }

}