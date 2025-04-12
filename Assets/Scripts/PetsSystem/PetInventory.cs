using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using YG;

public class PetInventory : MonoBehaviour
{
    [SerializeField] private List<PetInInventory> petInventory = new List<PetInInventory>();
    [SerializeField] private List<PetsData> allPetsData; // ������ ���� ��������� PetsData

    public int MaxPetsInInventory; // ������������ ����� �������� � ���������
    public int MaxEquippedPets; // ������������ ����� ������������� ��������

    public event Action OnInventoryChanged;
    public static Action OnInventoryLoaded;
    public event Action<PetInInventory> OnPetAdded;
    public event Action<PetInInventory, bool> OnPetEquipChanged;

    void Start()
    {
        // �������������� allPetsData, ��������, ��������� ���� PetsData �� ��������
        PetsData.OnPetDropped += AddPetToInventory;
        print(allPetsData.Count);
        LoadPetInventory();
        //LoadPetInventory();
    }
    private void OnDisable() => PetsData.OnPetDropped -= AddPetToInventory;
    // ������� ��� ���������� ������ ������� � ���������
    public void AddPetToInventory(PetsData petData)
    {
        if (petData == null)
        {
            Debug.LogError("PetData is null. Cannot add pet to inventory.");
            return;
        }

        if (petInventory.Count >= MaxPetsInInventory)
        {
            Debug.LogWarning("Cannot add pet: inventory is full.");
            return; // �� ������� �������� �������, ���� ��������� �����
        }

        // ������� ����� ������ PetInInventory
        PetInInventory newPet = new PetInInventory(petData);
        petInventory.Add(newPet);
        Debug.Log($"Added new pet to inventory: {petData.name}");
        OnInventoryChanged?.Invoke();
        OnPetAdded?.Invoke(newPet);
        SavePetInventory(); // ������� �������� �������
    }
    // ������� ��� ���������� �������
    public bool EquipPet(int petId)
    {
        // ������� ���������� ��� ������������� ��������
        int equippedCount = petInventory.FindAll(p => p.IsEquiped).Count;

        if (equippedCount >= MaxEquippedPets)
        {
            Debug.Log("Cannot equip pet: maximum equipped pets limit reached.");
            return false; // �� ������� ����������� �������, ���� �������� �����
        }

        PetInInventory petToEquip = petInventory.Find(p => p.Id == petId);
        if (petToEquip != null)
        {
            petToEquip.IsEquiped = true;
            Debug.Log($"Equipped pet: {petToEquip.PetData.name}");
            OnInventoryChanged?.Invoke();
            OnPetEquipChanged?.Invoke(petToEquip, petToEquip.IsEquiped);
            SavePetInventory();
            return true; // ������� ���������� �������
        }
        Debug.LogError($"Pet with ID {petId} not found in inventory.");
        return false; // �� ������� ����������� �������, ���� ������� �� ������
    }
    public bool UnEquip(int petId)
    {
        PetInInventory petToUnequip = petInventory.Find(p => p.Id == petId);
        if (!petToUnequip.IsEquiped)
            return false;
        if (petToUnequip != null)
        {
            petToUnequip.IsEquiped = false;
            OnInventoryChanged?.Invoke();
            OnPetEquipChanged?.Invoke(petToUnequip, petToUnequip.IsEquiped);
            Debug.Log($"Unequipped pet: {petToUnequip.PetData.name}");
            SavePetInventory();
            return true;
        }
        Debug.LogError($"Pet with ID {petId} not found in inventory.");
        return false;
    }
    public bool DeletePet(int petId)
    {
        PetInInventory removablePet = petInventory.Find(p => p.Id == petId);
        if (removablePet != null)
        {
            petInventory.Remove(removablePet);
            Debug.Log($"Pet with ID {petId} has been removed from inventory");
            OnInventoryChanged?.Invoke();
            OnPetEquipChanged?.Invoke(removablePet, false);
            SavePetInventory();
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
        // ����������� ������ � JSON, ������� nextId
        string jsonData = JsonUtility.ToJson(new InventoryData { pets = petInventory.ToArray(), nextId = PetInInventory.nextId });
        YandexGame.savesData.petInventoryData = jsonData;
        YandexGame.SaveProgress();
        print("������ ���������");
        Debug.Log($"Pet inventory saved: {jsonData}");
    }

    public void LoadPetInventory()
    {
        string jsonData = YandexGame.savesData.petInventoryData;

        if (!string.IsNullOrEmpty(jsonData))
        {
            // ������������� ������
            InventoryData loadedData = JsonUtility.FromJson<InventoryData>(jsonData);
            petInventory = new List<PetInInventory>();

            // ������� ������� PetInInventory �� ����������� ������
            foreach (var petData in loadedData.pets)
            {
                // ������� ��������������� PetsData �� petDataId
                PetsData petDataReference = allPetsData.Find(pet => pet.Id == petData.petDataId);
                if (petDataReference != null)
                {
                    // ������� ����� ������ PetInInventory � ��������� PetsData
                    PetInInventory pet = new PetInInventory(petDataReference)
                    {
                        Id = petData.Id,
                        IsEquiped = petData.IsEquiped
                    };
                    print("��� ���� - " + petData.IsEquiped);
                    pet.RestorePetData(allPetsData); // ��������������� ������ �� PetsData
                    petInventory.Add(pet);
                }
                else
                {
                    Debug.LogWarning($"PetsData � ID {petData.petDataId} �� �������.");
                }
            }

            // ������������� nextId �� ����������� ������
            PetInInventory.nextId = loadedData.nextId > 0 ? loadedData.nextId : petInventory.Count + 1;
            Debug.Log(PetInInventory.nextId);
        }
        else
        {
            // ���� ������ ���, ������������� nextId �� 1
            PetInInventory.nextId = 1;
            Debug.Log("nextId �� ������");
        }

        OnInventoryLoaded?.Invoke();
    }
    public List<PetInInventory> GetPets() => petInventory;
    public int GetPetsCount() => petInventory.Count;
    public int GetEquipedCount() => petInventory.FindAll(p => p.IsEquiped == true).Count;

    // ����� ��� ������� ������ ��������
    [System.Serializable]
    public class InventoryData
    {
        public PetInInventory[] pets; // ������ �������� ��� ������������
        public int nextId; // ������� �������� nextId ��� ���������� ��������
    }
}
[System.Serializable]
public class PetInInventory
{
    public int petDataId; // Id PetsData ��� ������������
    public bool IsEquiped; // ���� ������ ���� �������������
    public int Id; // ���������� �������������

    [NonSerialized]
    public PetsData PetData; // ������ �� ScriptableObject

    public static int nextId;

    public PetInInventory(PetsData petData)
    {
        PetData = petData;
        petDataId = petData.Id; // ��������� Id PetsData

        // ������������� ���������� Id
        Id = nextId;
        nextId++;

        Debug.Log($"������ ����� PetInInventory � Id: {Id}, nextId ������: {nextId}");
    }

    // ����� ��� �������������� ������ �� PetsData ����� ��������������
    public void RestorePetData(List<PetsData> allPetsData)
    {
        // ���� PetsData �� Id
        PetData = allPetsData.Find(pet => pet.Id == petDataId);
    }
}
