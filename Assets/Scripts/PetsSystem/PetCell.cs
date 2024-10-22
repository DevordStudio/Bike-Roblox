using System;
using UnityEngine;
using UnityEngine.UI;

public class PetCell : MonoBehaviour
{
    public Image IconPet;
    public GameObject IconEquiped;
    public GameObject IconSelected;
    public PetInInventory Pet;

    public static event Action<PetCell> OnCellSelected;

    public void OnClick()
    {
        OnCellSelected?.Invoke(this);
    }
}
