using UnityEngine;

public class BoxPointCode : MonoBehaviour
{
    [SerializeField] private GameObject[] _giftsPrefabs;
    [SerializeField] private float _spawnTime;
    private GiftBoxCode _currentBox;
    private void Start()
    {
        CreateNewBox();
    }
    private void CreateNewBox()
    {
        int randomIndex = Random.Range(0, _giftsPrefabs.Length);
        GameObject box = Instantiate(_giftsPrefabs[randomIndex],transform);
        _currentBox = box.GetComponent<GiftBoxCode>();
        _currentBox.OwnPoint = this;
    }
    public void InvokeSpawn() => Invoke(nameof(CreateNewBox), _spawnTime);
}
