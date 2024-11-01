using System.Collections.Generic;
using UnityEngine;

public class PetsController : MonoBehaviour
{
    //[SerializeField] private GameObject _parentPets;
    [SerializeField] private PetInventory _invent;
    [SerializeField] private Transform[] _spawnPoints;

    private List<GameObject> _petGO = new List<GameObject>();

    private void Start()
    {
        _invent.OnPetEquipChanged += Controll;
        foreach (var pet in _invent.GetPets())
        {
            if (pet.IsEquiped) EquipPet(pet);
        }
    }

    private void OnDisable()
    {
        _invent.OnPetEquipChanged -= Controll;
    }

    private void Controll(PetInInventory pet, bool isEquiped)
    {
        if (isEquiped) EquipPet(pet);
        else UnequipPet(pet);
    }

    private void EquipPet(PetInInventory pet)
    {
        if (pet == null) return;

        Transform spawnPoint = FindAvailableSpawnPoint();
        if (spawnPoint == null)
        {
            Debug.LogWarning("Нет доступных точек для спавна питомца.");
            return;
        }

        var petGO = Instantiate(pet.PetData.PetModel, /*spawnPoint.position, spawnPoint.rotation,*/ spawnPoint.transform);
        petGO.name = pet.Id.ToString();
        _petGO.Add(petGO);
        Debug.Log($"Создана модель питомца с Id {pet.Id}");
    }

    private void UnequipPet(PetInInventory pet)
    {
        if (pet == null) return;

        GameObject petGO = _petGO.Find(p => p.name == pet.Id.ToString());
        _petGO.Remove(petGO);
        Destroy(petGO);
        Debug.Log($"Удалена модель питомца с Id {pet.Id}");
    }

    private Transform FindAvailableSpawnPoint()
    {
        foreach (var point in _spawnPoints)
        {
            if (!_petGO.Exists(pet => pet.transform.position == point.position))
                return point;
        }
        return null;
    }
}
