using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationController : MonoBehaviour
{
    [Header("������ ��� ���������� ���������")]
    [HorizontalLine(color: EColor.Indigo)]
    [SerializeField] LocationInfo[] _locations;
    public LocationInfo[] Locations
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
            location.IsEquiped = location.Id == Id;
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
            if (location.IsEquiped)
            {
                ActiveLocationId = location.Id;
                if (location.Skybox)
                    RenderSettings.skybox = location.Skybox;
            }
        }
    }
    private void OnDestroy()
    {
        LocationInfo.OnLocationChanged -= LoadLocation;
    }
}
