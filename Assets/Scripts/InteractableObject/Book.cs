using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : InteractableObject
{
   
    public override void Interacted()
    {
        UIManager.instance.uiBook.Show();
    }


}
