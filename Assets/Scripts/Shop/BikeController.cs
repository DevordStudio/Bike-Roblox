using NaughtyAttributes;
using UnityEngine;

public class BikeController : MonoBehaviour
{
    [Header("������ ��� ���������� ������� ����������")]
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
    [Tooltip("��������� Mesh renderer ������� ����� �� ���� ����������")]
    [SerializeField] private MeshRenderer _chasicsMR;
    [Tooltip("Mesh Renderer ��������� ������")]
    [SerializeField] private MeshRenderer _wheelFrontMR;
    [Tooltip("Mesh Renderer ������� ������")]
    [SerializeField] private MeshRenderer _wheelBackMR;
    //[SerializeField] private GameObject _bikeModel;

    public int ActiveBikeId { get; private set; }

    private void Start()
    {
        foreach (var bike in _bikes)//�������� ��������
        {
            if (bike.IsEquiped)
            {
                ActiveBikeId = bike.Id;
                break;
            }
        }
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
                Debug.Log($"������ ��������� {bike.Name}");
                break;
            }
        }
    }
}
