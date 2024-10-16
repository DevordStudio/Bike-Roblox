using NaughtyAttributes;
using System;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Скрипт для управления скином персонажа")]
    [HorizontalLine(color: EColor.Orange)]
    [SerializeField] private CharacterData[] _characters;
    public CharacterData[] Characters
    {
        get
        {
            return _characters;
        }
        private set { }
    }
    [SerializeField] private bicycle_code _bike;
    public int ActiveCharacterId; //{ get; private set; }

    private void Start()
    {
        foreach (var character in _characters)
        {
            if (character.Info.IsEquiped)
            {
                ActiveCharacterId = character.Info.Id;
                print(character.Info.Id);
                break;
            }
        }
        CharacterInfo.OnCharacterChanged += ChangeCharacter;
        ActivateCurrentCharacter();
    }
    public void ChangeCharacter(int Id)
    {
        Debug.Log($"Персонаж изменён на {Id}");
        ActiveCharacterId = Id;
        ActivateCurrentCharacter();
    }
    private void ActivateCurrentCharacter()
    {
        foreach (var character in _characters)
        {
            if (character.Info.Id == ActiveCharacterId)
            {
                character.Character.SetActive(true);
                _bike.ChangeCharacter(character.Character);
                Debug.Log(character.Character);
            }
            else character.Character.SetActive(false);
        }
    }
    private void OnDestroy() => CharacterInfo.OnCharacterChanged -= ChangeCharacter;
}
