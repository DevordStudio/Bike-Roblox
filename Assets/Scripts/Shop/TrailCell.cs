using UnityEngine;
using UnityEngine.UI;

public class TrailCell : MonoBehaviour
{
    public Image Icon;
    public Image IconEquiped;
    [HideInInspector] public TrailData TrailData;
    [HideInInspector] public TrailController TrailController;
    public Button ButtonChoose;


    private void Start()
    {
        ButtonChoose.onClick.AddListener(SetTrail);
        UpdateCell();
    }
    public void SetTrail()
    {
        TrailController.lastCell = TrailController.currentCell;
        TrailController.currentCell = this;
        TrailController.UpdateUI();
    }
    public void UpdateCell()
    {
        if (TrailData.IsEquiped)
            IconEquiped.gameObject.SetActive(TrailData.IsEquiped);
    }
}
