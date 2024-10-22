using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class PetInventory : ScriptableObject
{
    private List<PetInInventory> petInventory = new List<PetInInventory>();
    private List<PetsData> allPetsData; // ������ ���� ��������� PetsData

    public int MaxPetsInInventory { get; private set; } // ������������ ����� �������� � ���������
    public int MaxEquippedPets { get; private set; } // ������������ ����� ������������� ��������

    public event Action OnInventoryChanged;

    void Start()
    {
        // �������������� allPetsData, ��������, ��������� ���� PetsData �� ��������
        allPetsData = new List<PetsData>(Resources.LoadAll<PetsData>("ShopItems/Pets"));
        PetsData.OnPetDropped += AddPetToInventory;
    }
    private void OnDisable() => PetsData.OnPetDropped -= AddPetToInventory;
    // ������� ��� ���������� ������ ������� � ���������
    public void AddPetToInventory(PetsData petData)
    {
        if (petData == null)
        {
            Debug.LogError("PetData is null. Cannot add pet to inventory.");
            //return false;
        }

        if (petInventory.Count >= MaxPetsInInventory)
        {
            Debug.LogWarning("Cannot add pet: inventory is full.");
            //return false; // �� ������� �������� �������, ���� ��������� �����
        }

        // ������� ����� ������ PetInInventory
        PetInInventory newPet = new PetInInventory(petData);
        petInventory.Add(newPet);
        Debug.Log($"Added new pet to inventory: {petData.name}");
        OnInventoryChanged?.Invoke();
        //return true; // ������� �������� �������
    }


    // ������� ��� ���������� �������
    public bool EquipPet(int petId)
    {
        // ������� ���������� ��� ������������� ��������
        int equippedCount = petInventory.FindAll(p => p.IsEquiped).Count;

        if (equippedCount >= MaxEquippedPets)
        {
            Debug.LogWarning("Cannot equip pet: maximum equipped pets limit reached.");
            return false; // �� ������� ����������� �������, ���� �������� �����
        }

        PetInInventory petToEquip = petInventory.Find(p => p.Id == petId);
        if (petToEquip != null)
        {
            petToEquip.IsEquiped = true;
            Debug.Log($"Equipped pet: {petToEquip.PetData.name}");
            OnInventoryChanged?.Invoke();
            return true; // ������� ���������� �������
        }
        Debug.LogError($"Pet with ID {petId} not found in inventory.");
        return false; // �� ������� ����������� �������, ���� ������� �� ������
    }
    public bool DeletePet(int petId)
    {
        PetInInventory removablePet = petInventory.Find(p => p.Id == petId);
        if (removablePet != null)
        {
            petInventory.Remove(removablePet);
            Debug.Log($"Pet with ID {petId} has been removed from inventory");
            OnInventoryChanged?.Invoke();
            return true;
        }
        else
        {
            Debug.LogError($"Pet with ID {petId} not found in inventory.");
            return false;
        }
    }
    public void SavePetInventory()
    {
        // ����������� ������ � JSON
        string jsonData = JsonUtility.ToJson(new InventoryData { pets = petInventory.ToArray() });
        YandexGame.savesData.petInventoryData = jsonData;
        YandexGame.SaveProgress();

        Debug.Log("Pet inventory saved: " + jsonData);
    }

    public void LoadPetInventory()
    {
        // ��������� ������
        string jsonData = YandexGame.savesData.petInventoryData;

        if (!string.IsNullOrEmpty(jsonData))
        {
            // ������������� ������
            InventoryData loadedData = JsonUtility.FromJson<InventoryData>(jsonData);
            petInventory = new List<PetInInventory>(loadedData.pets);

            // ��������������� PetData ��� ������� �������
            foreach (var pet in petInventory)
            {
                pet.RestorePetData(allPetsData);
            }

            Debug.Log("Pet inventory loaded.");
        }
        else
        {
            Debug.Log("No saved pet inventory data found.");
        }
    }
    public List<PetInInventory> GetPets() => petInventory;
    public int GetPetsCount() => petInventory.Count;
    public int GetEquipedCount() => petInventory.FindAll(p => p.IsEquiped == true).Count;

    // ����� ��� ������� ������ ��������
    [System.Serializable]
    public class InventoryData
    {
        public PetInInventory[] pets; // ������ �������� ��� ������������
    }
}
[System.Serializable]
public class PetInInventory
{
    public int petDataId; // Id PetsData ��� ������������
    public bool IsEquiped;
    public int Id; // ���������� �������������

    [NonSerialized]
    public PetsData PetData; // ������ �� ScriptableObject

    private static int nextId = 1;

    public PetInInventory(PetsData petData)
    {
        PetData = petData;
        petDataId = petData.Id; // ��������� Id PetsData
        Id = nextId;
        nextId++;
    }

    // ����� ��� �������������� ������ �� PetsData ����� ��������������
    public void RestorePetData(List<PetsData> allPetsData)
    {
        // ���� PetsData �� Id
        PetData = allPetsData.Find(pet => pet.Id == petDataId);
    }
}
