using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Quantity of the item in the inventory. 
public class ItemStack
{
    
    public byte id;
    public int amount;

    //Constructor
    public ItemStack(byte _id, int _amount)
    {

        id = _id;
        amount = _amount;

    }

}