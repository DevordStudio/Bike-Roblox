using System.Collections.Generic;
using UnityEngine;

public class PetsController : MonoBehaviour
{
    [SerializeField] private GameObject _parentPets;
    [SerializeField] private PetInventory _invent;
    [SerializeField] private Transform _targetToFollow;
    [SerializeField] private float _lerpSpeed;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _minDistanceFromOtherPets = 2f;

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

    //private void Update()
    //{
    //    if (_petGO.Count == 0) return;

    //    for (int i = 0; i < _petGO.Count; i++)
    //    {
    //        if (Vector3.Distance(_petGO[i].transform.position, _targetToFollow.transform.position) > 0.7)
    //        {
    //            Vector3 targetPosition = _targetToFollow.transform.position;

    //            if (i > 0)
    //            {
    //                Vector3 previousPetPosition = _petGO[i - 1].transform.position;
    //                Vector3 direction = (previousPetPosition - targetPosition).normalized;
    //                targetPosition = previousPetPosition + direction * _minDistanceFromOtherPets;
    //            }

    //            _petGO[i].transform.position = Vector3.Lerp(_petGO[i].transform.position, targetPosition, _lerpSpeed * Time.deltaTime);
    //        }
    //    }
    //}

    private void Controll(PetInInventory pet, bool isEquiped)
    {
        if (isEquiped) EquipPet(pet);
        else UnequipPet(pet);
    }

    private void EquipPet(PetInInventory pet)
    {
        if (pet == null) return;

        Vector3 spawnPosition = CalculateSpawnPositionInRearHemisphere();
        var petGO = Instantiate(pet.PetData.PetModel, spawnPosition, Quaternion.identity, _parentPets.transform);
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

    private Vector3 CalculateSpawnPositionInRearHemisphere()
    {
        float angle = Random.Range(90F, 270F);
        float radians = angle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians)) * _spawnRadius;

        return _targetToFollow.position + offset;
    }
}
