using UnityEngine;
using UnityEngine.UI;

public class TrailCell : MonoBehaviour
{
    public Image Icon;
    public Image IconEquiped;
    [HideInInspector] public TrailData TrailData;
    public Button ButtonChoose;


    private void Start()
    {
        ButtonChoose.onClick.AddListener(SetTrail);
        UpdateCell();
    }
    public void SetTrail()
    {
        TrailController.Instance.lastCell = TrailController.Instance.currentCell;
        TrailController.Instance.currentCell = this;
        TrailController.Instance.UpdateUI();
    }
    public void UpdateCell()
    {
        if (TrailData.IsEquiped)
            IconEquiped.gameObject.SetActive(TrailData.IsEquiped);
    }
}
