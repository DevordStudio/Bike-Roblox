using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationController : MonoBehaviour
{
    [Header("Скрипт для управления локациями")]
    [HorizontalLine(color: EColor.Orange)]
    [SerializeField] LocationData[] _locations;
    public LocationData[] Locations
    {
        get
        {
            return _locations;
        }
        private set { }
    }
    public int ActiveLocationId { get; private set; }

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
        ActiveLocationId = Id;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void Start()
    {
        foreach (var location in _locations)
        {
            location.Location.SetActive(location.Id == ActiveLocationId);
        }
    }
}
