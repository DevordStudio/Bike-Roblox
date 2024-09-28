using UnityEngine;

public class BikeData : ShopItemData
{
    public Material ChasicsMaterial;
    public Material WheelsMaterial;

    public void ChangeMaterials(MeshRenderer chasicsMR, MeshRenderer wheelFrontMR, MeshRenderer wheelBackMR)
    {
        chasicsMR.materials[2] = ChasicsMaterial;
        wheelFrontMR.materials[1] = WheelsMaterial;
        wheelBackMR.materials[1] = WheelsMaterial;
    }
    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}
