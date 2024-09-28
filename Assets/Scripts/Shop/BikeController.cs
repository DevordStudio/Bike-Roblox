using NaughtyAttributes;
using UnityEngine;

public class BikeController : MonoBehaviour
{
    [Header("Скрипт для управления скинами велосипеда")]
    [HorizontalLine(color: EColor.Orange)]
    [SerializeField] private BikeData[] _bikes;
    public BikeData[] Bikes
    {
        get
        {
            return _bikes;
        }
        private set { }
    }
    [Tooltip("Компонент Mesh renderer который висит на раме велосипеда")]
    [SerializeField] private MeshRenderer _chasicsMR;
    [Tooltip("Mesh Renderer переднего колеса")]
    [SerializeField] private MeshRenderer _wheelFrontMR;
    [Tooltip("Mesh Renderer заднего колеса")]
    [SerializeField] private MeshRenderer _wheelBackMR;
    [SerializeField] private GameObject _bikeModel;

    public int ActiveBikeId { get; private set; }
    public static BikeController Instance;
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
    private void Start()
    {
        ActivateCurrentBike();
    }
    public void ChangeBike(int Id)
    {
        ActiveBikeId = Id;
        ActivateCurrentBike();
    }
    public void ActivateCurrentBike()
    {
        foreach (var bike in _bikes)
        {
            if (bike.Id == ActiveBikeId)
            {
                bike.ChangeMaterials(_chasicsMR, _wheelFrontMR, _wheelBackMR);
                Debug.Log($"Выбран велосипед {bike.Name}");
                break;
            }
        }
    }
}
