using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
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
    public int ActiveCharacterId { get; private set; }

    public static CharacterController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    public void ChangeCharacter(int Id)
    {
        ActiveCharacterId = Id;
        foreach (var character in _characters)
        {
            character.gameObject.SetActive(character.Id == ActiveCharacterId);
        }
    }
}
