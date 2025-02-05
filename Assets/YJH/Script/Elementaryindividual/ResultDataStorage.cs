using UnityEngine;

public class ResultDataStorage : MonoBehaviour
{
    private static ResultDataStorage instance;
    public static ResultDataStorage Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = new GameObject("ResultDataStorage");
                instance = obj.AddComponent<ResultDataStorage>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // ----------------------------------------------------
    // Game1 관련 데이터
    // ----------------------------------------------------
    [Header("Game1")]
    [SerializeField] private string game1ContData = "기본 더미 내용";
    [SerializeField] private int game1ImgTypeData = -1;
    [SerializeField] private int game1StatusData = 0;

    public string Game1ContData
    {
        get => game1ContData;
        set => game1ContData = value;
    }

    public int Game1ImgTypeData
    {
        get => game1ImgTypeData;
        set => game1ImgTypeData = value;
    }

    public int Game1StatusData
    {
        get => game1StatusData;
        set => game1StatusData = value;
    }

    // ----------------------------------------------------
    // Game2 관련 데이터
    // ----------------------------------------------------
    [Header("Game2")]
    [SerializeField] private string game2ContData = "Game2 기본 더미 내용";
    [SerializeField] private int game2ImgTypeData = -1;
    [SerializeField] private int game2StatusData = 0;

    public string Game2ContData
    {
        get => game2ContData;
        set => game2ContData = value;
    }

    public int Game2ImgTypeData
    {
        get => game2ImgTypeData;
        set => game2ImgTypeData = value;
    }

    public int Game2StatusData
    {
        get => game2StatusData;
        set => game2StatusData = value;
    }
}
