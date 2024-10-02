using UnityEngine;

[CreateAssetMenu(fileName = "BikeData", menuName = "ShopItem/BikeData")]
public class BikeData : ScriptableObject
{
    public string Name = "Bike";
    public int Price = 100;
    public int Id;
    public bool IsBought;
    public bool IsEquiped;
    public Material ChasicsMaterial;
    public Material WheelsMaterial;
    public void Buy()
    {
        if (!IsBought)
        {
            IsBought = true;
            Debug.Log($"������� {Name} ��� ������ �� {Price} �����");
        }
        else Debug.LogError("������� ��� ������!");
    }
    public void ChangeMaterials(MeshRenderer chasicsMR, MeshRenderer wheelFrontMR, MeshRenderer wheelBackMR)
    {
        chasicsMR.materials[2] = ChasicsMaterial;
        wheelFrontMR.materials[1] = WheelsMaterial;
        wheelBackMR.materials[1] = WheelsMaterial;
    }
}
