using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationController : MonoBehaviour
{
    [SerializeField] private LocationData[] _locations;
    [SerializeField] private int _activeLocationId;

    public static LocationController Instance;

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
    public void LoadLocation(int Id)
    {
        _activeLocationId = Id;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void Start()
    {
        foreach (var location in _locations)
        {
            location.Location.SetActive(location.Id == _activeLocationId);
        }
    }
}
