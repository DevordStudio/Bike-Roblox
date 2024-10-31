using UnityEngine;
using UnityEngine.UI;

public class TrailCell : MonoBehaviour
{
    public Image Icon;
    public GameObject IconEquiped;
    public GameObject IconSelected;
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
        //TrailController.lastCell = TrailController.currentCell;
        TrailController.currentCell = this;
        IconSelected.SetActive(true);
        if (TrailController.lastSelected)
            TrailController.lastSelected.IconSelected.SetActive(false);
        TrailController.lastSelected = this;
        TrailController.UpdateUI();
    }
    public void UpdateCell()
    {
        IconEquiped.gameObject.SetActive(TrailData.IsEquiped);
    }
}
