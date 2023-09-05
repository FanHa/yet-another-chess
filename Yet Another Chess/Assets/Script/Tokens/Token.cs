using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Token : MonoBehaviour
{
    public Vector2Int BoardPosition = Vector2Int.zero;
    public string Class = "Base Token";
    public string Info = " Low speed\n Low attack\n Low defense";
    public TextMeshProUGUI TokenNameText;
    public GameObject ChoosonRing;
    [SerializeField] private int _mobility = 5;
    private int _team;

    public int Team
    {
        get => _team;
        set
        {
            _team = value;
            Renderer renderer = GetComponent<Renderer>();
            if (value == 1)
            {
                renderer.material.color = Color.blue;
            }
            else
            {
                renderer.material.color = Color.red;
            }

        }
    }

    public string Name
    {
        get => name;
        set
        {
            name = value;
            TokenNameText.text = name;
        }
    }
    public float CurrentHealth;
    public float MaxHealth;

    public GameObject HealthBarUI;
    public Slider Slider;
    // Start is called before the first frame update
    public void Start()
    {
        Debug.Log("Base Start");
        CurrentHealth = MaxHealth;
        Slider.value = CalculateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveTo(Vector2Int targetCoordinate)
    {
        BoardPosition = targetCoordinate;
        var realPosition = BoardManager.GetRealPositionByCoordinate(targetCoordinate);
        transform.position = realPosition;
    }

    public List<Vector2> CanMoveToList()
    {
        return new List<Vector2> { };
    }


    public bool CanAttackTo(Token target)
    {

        return true;
    }

    public bool CanMoveTo(BoardUnit boardUnit)
    {
        var distance = GetDistance(boardUnit.XYZCoordinate, new Vector3Int(BoardPosition.x, BoardPosition.y, 0 - BoardPosition.x - BoardPosition.y));
        Debug.Log(distance);
        Debug.Log(boardUnit.XYCoordinate);
        if (distance <= _mobility)
        {
            return true;
        }
        return false;
    }
    public void AddHP(int hp)
    {
        CurrentHealth += hp;
        Slider.value = CalculateHealth();

        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void SetAttackTargetable(bool attackable)
    {
        // todo
    }

    int GetDistance(Vector3Int qrsA, Vector3Int qrsB)
    {
        int dQ = Mathf.Abs(qrsB.x - qrsA.x);
        int dR = Mathf.Abs(qrsB.y - qrsA.y);
        int dS = Mathf.Abs(qrsB.z - qrsA.z);

        return Mathf.Max(dQ, dR, dS);
    }

    float CalculateHealth()
    {
        return CurrentHealth / MaxHealth;
    }
}
