using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class PetInventory : MonoBehaviour
{
    [SerializeField] private List<PetInInventory> petInventory = new List<PetInInventory>();
    private List<PetsData> allPetsData; // Список всех доступных PetsData

    public int MaxPetsInInventory; // Максимальное число питомцев в инвентаре
    public int MaxEquippedPets; // Максимальное число экипированных питомцев

    public event Action OnInventoryChanged;
    public event Action<PetInInventory> OnPetAdded;
    public event Action<PetInInventory, bool> OnPetEquipChanged;
    private void Awake()
    {
        allPetsData = new List<PetsData>(Resources.LoadAll<PetsData>("ShopItems/Pets"));

    }
    void Start()
    {
        // Инициализируем allPetsData, например, загрузкой всех PetsData из ресурсов
        PetsData.OnPetDropped += AddPetToInventory;
        print(allPetsData.Count);
        LoadPetInventory();
    }
    private void OnDisable() => PetsData.OnPetDropped -= AddPetToInventory;
    // Функция для добавления нового питомца в инвентарь
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
            return; // Не удалось добавить питомца, если инвентарь полон
        }

        // Создаем новый объект PetInInventory
        PetInInventory newPet = new PetInInventory(petData);
        petInventory.Add(newPet);
        Debug.Log($"Added new pet to inventory: {petData.name}");
        OnInventoryChanged?.Invoke();
        OnPetAdded?.Invoke(newPet);
        SavePetInventory(); // Успешно добавлен питомец
    }


    // Функция для экипировки питомца
    public bool EquipPet(int petId)
    {
        // Считаем количество уже экипированных питомцев
        int equippedCount = petInventory.FindAll(p => p.IsEquiped).Count;

        if (equippedCount >= MaxEquippedPets)
        {
            Debug.Log("Cannot equip pet: maximum equipped pets limit reached.");
            return false; // Не удалось экипировать питомца, если превышен лимит
        }

        PetInInventory petToEquip = petInventory.Find(p => p.Id == petId);
        if (petToEquip != null)
        {
            petToEquip.IsEquiped = true;
            Debug.Log($"Equipped pet: {petToEquip.PetData.name}");
            OnInventoryChanged?.Invoke();
            OnPetEquipChanged?.Invoke(petToEquip, petToEquip.IsEquiped);
            SavePetInventory();
            return true; // Успешно экипирован питомец
        }
        Debug.LogError($"Pet with ID {petId} not found in inventory.");
        return false; // Не удалось экипировать питомца, если питомец не найден
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
        // Преобразуем список в JSON, включая nextId
        string jsonData = JsonUtility.ToJson(new InventoryData { pets = petInventory.ToArray(), nextId = PetInInventory.nextId });
        YandexGame.savesData.petInventoryData = jsonData;
        YandexGame.SaveProgress();
        print("Данные сохранены");
        Debug.Log($"Pet inventory saved: {jsonData}");
    }

    public void LoadPetInventory()
    {
        string jsonData = YandexGame.savesData.petInventoryData;

        if (!string.IsNullOrEmpty(jsonData))
        {
            // Десериализуем данные
            InventoryData loadedData = JsonUtility.FromJson<InventoryData>(jsonData);
            petInventory = new List<PetInInventory>();

            // Создаем объекты PetInInventory из загруженных данных
            foreach (var petData in loadedData.pets)
            {
                // Находим соответствующий PetsData по petDataId
                PetsData petDataReference = allPetsData.Find(pet => pet.Id == petData.petDataId);
                if (petDataReference != null)
                {
                    // Создаем новый объект PetInInventory с найденным PetsData
                    PetInInventory pet = new PetInInventory(petDataReference)
                    {
                        Id = petData.Id,
                        IsEquiped = petData.IsEquiped
                    };
                    print("Пет дата - "+petData.IsEquiped);
                    pet.RestorePetData(allPetsData); // Восстанавливаем ссылку на PetsData
                    petInventory.Add(pet);
                }
                else
                {
                    Debug.LogWarning($"PetsData с ID {petData.petDataId} не найдено.");
                }
            }

            // Устанавливаем nextId из загруженных данных
            PetInInventory.nextId = loadedData.nextId > 0 ? loadedData.nextId : petInventory.Count + 1;
            Debug.Log(PetInInventory.nextId);
        }
        else
        {
            // Если данных нет, устанавливаем nextId на 1
            PetInInventory.nextId = 1;
            Debug.Log("nextId не найден");
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
        public int nextId; // Текущее значение nextId для корректной загрузки
    }
}
[System.Serializable]
public class PetInInventory
{
    public int petDataId; // Id PetsData для сериализации
    public bool IsEquiped; // Поле должно быть сериализуемым
    public int Id; // Уникальный идентификатор

    [NonSerialized]
    public PetsData PetData; // Ссылка на ScriptableObject

    public static int nextId;

    public PetInInventory(PetsData petData)
    {
        PetData = petData;
        petDataId = petData.Id; // Сохраняем Id PetsData

        // Устанавливаем уникальный Id
        Id = nextId;
        nextId++;

        Debug.Log($"Создан новый PetInInventory с Id: {Id}, nextId теперь: {nextId}");
    }

    // Метод для восстановления ссылки на PetsData после десериализации
    public void RestorePetData(List<PetsData> allPetsData)
    {
        // Ищем PetsData по Id
        PetData = allPetsData.Find(pet => pet.Id == petDataId);
    }
}
