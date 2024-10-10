using UnityEngine;

[CreateAssetMenu(fileName = "BikeData", menuName = "ShopItem/BikeData")]
public class BikeData : ShopItemData
{
    public Material ChasicsMaterial;
    public Material WheelsMaterial;
    public void ChangeMaterials(MeshRenderer chasicsMR, MeshRenderer wheelFrontMR, MeshRenderer wheelBackMR)
    {
        Material[] chasicsMaterials = chasicsMR.materials;
        Material[] frontWheelMaterials = wheelFrontMR.materials;
        Material[] backWheelMaterials = wheelBackMR.materials;

        chasicsMaterials[2] = ChasicsMaterial;
        frontWheelMaterials[1] = WheelsMaterial;
        backWheelMaterials[1] = WheelsMaterial;

        chasicsMR.materials = chasicsMaterials;
        wheelFrontMR.materials = frontWheelMaterials;
        wheelBackMR.materials = backWheelMaterials;
    }
}
