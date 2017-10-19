using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataController : MonoBehaviour {

	// Singleton class start
	static GameObject _container;
	static GameObject Container {
		get {
			return _container;
		}
	}

	static DataController _instance;
	public static DataController Instance {
		get {
			if( ! _instance ) {
				_container = new GameObject();
				_container.name = "DataController";
				_instance = _container.AddComponent( typeof(DataController) ) as DataController;
				DontDestroyOnLoad (_container);
                _instance.Init();
			}

			return _instance;
		}
	}
    // Singleton class end

    // 세이브 파일 로드
    GameData _gameData;
    public GameData gameData
    {
        get
        {
            if (_gameData == null)
            {
                LoadGameData();
            }
            return _gameData;
        }
    }

    // 모든 시나리오 내용을 담기 위해 ScenarioItem 리스트 선언
    public List<ScenarioItem> NormalScenarioList;
    public List<ScenarioItem> SpecialScenarioList;

    public Dictionary<int, ScenarioItem> ScenarioDic;
    
    public bool MetaDataLoaded = false;


    public void Init()
    {
        GameScenario = new List<ScenarioItem>();

        LoadScenario();

        MetaDataLoaded = true;
    }


    public ScenarioItem CurrentScenarioItem;

    // csv파일에서 데이터 가져오기
    public void LoadScenario(){
        TextAsset scenarioText = Resources.Load ("Scenario/GameNormalQuestion") as TextAsset;
        /*파일 종류
         * - GameNormalQuestion : 일반 질문
         * - GameSpecialQuestion : 특별 질문
         */

        // \n을 기준으로 문자열을 쪼갠다.
        string[] list = scenarioText.text.Split ('\n');
		int lineNum = 0;

        // 시나리오리스트 객체 생성
        NormalScenarioList = new List<ScenarioItem>();
        SpecialScenarioList = new List<ScenarioItem>();
        ScenarioDic = new Dictionary<int, ScenarioItem>();


        
        foreach (string line in list) {
            lineNum++;
			if (lineNum == 1) // 표의 맨 윗줄에 해당하는 첫번째 줄은 건너뛴다.
				continue;
            // 콤마를 기준으로 문자열을 쪼갠다.
			string[] values = line.Split (',');

            // 자료수 검사
            if (values.Length != 22)
				continue;
            
            ScenarioItem Item = null;
            
            Item = new ScenarioItem();

            Item.ID = int.Parse(values[0]);
            Item.Type = values[1];
            Item.Character = values[2];
            Item.Question = values[3];

            Item.Answer1 = values[4];
            Item.Approval1 = int.Parse(values[5]);
            Item.Welfare1 = int.Parse(values[6]);
            Item.Economy1 = int.Parse(values[7]);
            Item.Diplomacy1 = int.Parse(values[8]);
            Item.Army1 = int.Parse(values[9]);
            Item.Moral1 = int.Parse(values[10]);
            Item.Power1 = int.Parse(values[11]);
            Item.Next1 = int.Parse(values[12]);

            Item.Answer2 = values[13];
            Item.Approval2 = int.Parse(values[14]);
            Item.Welfare2 = int.Parse(values[15]);
            Item.Economy2 = int.Parse(values[16]);
            Item.Diplomacy2 = int.Parse(values[17]);
            Item.Army2 = int.Parse(values[18]);
            Item.Moral2 = int.Parse(values[19]);
            Item.Power2 = int.Parse(values[20]);
            Item.Next2 = int.Parse(values[21]);

                // 불러온 값을 시나리오리스트에 추가
            if(Item.Type == "N")
            {
                NormalScenarioList.Add(Item);

            }
            else if(Item.Type == "S")
            {
                SpecialScenarioList.Add(Item);

            }
            ScenarioDic.Add(Item.ID, Item);

        }
    }
	

	public List<ScenarioItem> GetNormalScenarioList(){

        LoadScenario();

		return NormalScenarioList;

	}
    
    public List<ScenarioItem> GameScenario;

    public void NormalRandom(int number)
    {
        for (int i = 0; i < number; i++)
        {
            List<ScenarioItem> list = GetNormalScenarioList();
            int idx = Random.Range(0, list.Count - 1);
            GameScenario.Add(list[idx]);
        }
    }

    public void SpecialFinder(int QuestionID)
    {
        //Debug.Log(IDFinder);
        ScenarioItem item = ScenarioDic[QuestionID];
        GameScenario.Add(item);
        //Debug.Log(SpecialScenarioList[IDFinder].Question);
    }


    public List<ScenarioItem> GetGameScenario()
    {
        NormalRandom(5);
        SpecialFinder(1);
        NormalRandom(5);
        SpecialFinder(1);
        return GameScenario;
    }

    void Start(){

        //GetGameScenario();
        Debug.Log("Scenario Done");
    }


    

    public string gameDataProjectFilePath = "/game.json";

    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + gameDataProjectFilePath;

        if (File.Exists(filePath))
        {
            Debug.Log("loaded!");
            string dataAsJson = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(dataAsJson);
        }
        else
        {
            Debug.Log("Create new");

            _gameData = new GameData();

            _gameData.ScenarioNum = 0;
            _gameData.Approval = 50;
            _gameData.Welfare = 50;
            _gameData.Economy = 50;
            _gameData.Diplomacy = 50;
            _gameData.Army = 50;
            _gameData.Moral = 50;

}
    }

    public void SaveGameData()
    {

        string dataAsJson = JsonUtility.ToJson(gameData);

        string filePath = Application.persistentDataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);
    }
}
