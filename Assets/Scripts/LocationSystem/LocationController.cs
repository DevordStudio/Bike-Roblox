using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationController : MonoBehaviour
{
    [Header("������ ��� ���������� ���������")]
    [HorizontalLine(color: EColor.Indigo)]
    [SerializeField] LocationData[] _locations;
    public LocationData[] Locations
    {
        get
        {
            return _locations;
        }
        private set { }
    }
    public int ActiveLocationId; //{ get; private set; }
    public void LoadLocation(int Id)
    {
        foreach (var location in _locations)
        {
            location.LocationInfo.IsEquiped = location.LocationInfo.Id == Id;
            ActiveLocationId = Id;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //�������� ������ ���������� � ������� ActiveLocationId
    private void Start()
    {
        LocationInfo.OnLocationChanged += LoadLocation;
        foreach (var location in _locations)
        {
            //location.Location.SetActive(location.LocationInfo.IsEquiped);
            if (location.LocationInfo.IsEquiped)
            {
                ActiveLocationId = location.LocationInfo.Id;
                if (location.LocationInfo.Skybox)
                    RenderSettings.skybox = location.LocationInfo.Skybox;
            }
        }
    }
    private void OnDestroy()
    {
        LocationInfo.OnLocationChanged -= LoadLocation;
    }
}
