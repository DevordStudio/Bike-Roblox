using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class PetInventory : ScriptableObject
{
    private List<PetInInventory> petInventory = new List<PetInInventory>();
    private List<PetsData> allPetsData; // Список всех доступных PetsData

    public int MaxPetsInInventory { get; private set; } // Максимальное число питомцев в инвентаре
    public int MaxEquippedPets { get; private set; } // Максимальное число экипированных питомцев

    public event Action OnInventoryChanged;

    void Start()
    {
        // Инициализируем allPetsData, например, загрузкой всех PetsData из ресурсов
        allPetsData = new List<PetsData>(Resources.LoadAll<PetsData>("ShopItems/Pets"));
        PetsData.OnPetDropped += AddPetToInventory;
    }
    private void OnDisable() => PetsData.OnPetDropped -= AddPetToInventory;
    // Функция для добавления нового питомца в инвентарь
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
            //return false; // Не удалось добавить питомца, если инвентарь полон
        }

        // Создаем новый объект PetInInventory
        PetInInventory newPet = new PetInInventory(petData);
        petInventory.Add(newPet);
        Debug.Log($"Added new pet to inventory: {petData.name}");
        OnInventoryChanged?.Invoke();
        //return true; // Успешно добавлен питомец
    }


    // Функция для экипировки питомца
    public bool EquipPet(int petId)
    {
        // Считаем количество уже экипированных питомцев
        int equippedCount = petInventory.FindAll(p => p.IsEquiped).Count;

        if (equippedCount >= MaxEquippedPets)
        {
            Debug.LogWarning("Cannot equip pet: maximum equipped pets limit reached.");
            return false; // Не удалось экипировать питомца, если превышен лимит
        }

        PetInInventory petToEquip = petInventory.Find(p => p.Id == petId);
        if (petToEquip != null)
        {
            petToEquip.IsEquiped = true;
            Debug.Log($"Equipped pet: {petToEquip.PetData.name}");
            OnInventoryChanged?.Invoke();
            return true; // Успешно экипирован питомец
        }
        Debug.LogError($"Pet with ID {petId} not found in inventory.");
        return false; // Не удалось экипировать питомца, если питомец не найден
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
        // Преобразуем список в JSON
        string jsonData = JsonUtility.ToJson(new InventoryData { pets = petInventory.ToArray() });
        YandexGame.savesData.petInventoryData = jsonData;
        YandexGame.SaveProgress();

        Debug.Log("Pet inventory saved: " + jsonData);
    }

    public void LoadPetInventory()
    {
        // Загружаем данные
        string jsonData = YandexGame.savesData.petInventoryData;

        if (!string.IsNullOrEmpty(jsonData))
        {
            // Десериализуем данные
            InventoryData loadedData = JsonUtility.FromJson<InventoryData>(jsonData);
            petInventory = new List<PetInInventory>(loadedData.pets);

            // Восстанавливаем PetData для каждого объекта
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

    // Класс для обертки списка питомцев
    [System.Serializable]
    public class InventoryData
    {
        public PetInInventory[] pets; // Массив питомцев для сериализации
    }
}
[System.Serializable]
public class PetInInventory
{
    public int petDataId; // Id PetsData для сериализации
    public bool IsEquiped;
    public int Id; // Уникальный идентификатор

    [NonSerialized]
    public PetsData PetData; // Ссылка на ScriptableObject

    private static int nextId = 1;

    public PetInInventory(PetsData petData)
    {
        PetData = petData;
        petDataId = petData.Id; // Сохраняем Id PetsData
        Id = nextId;
        nextId++;
    }

    // Метод для восстановления ссылки на PetsData после десериализации
    public void RestorePetData(List<PetsData> allPetsData)
    {
        // Ищем PetsData по Id
        PetData = allPetsData.Find(pet => pet.Id == petDataId);
    }
}
