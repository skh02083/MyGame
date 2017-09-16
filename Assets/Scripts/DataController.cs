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
    
    public bool MetaDataLoaded = false;


    public void Init()
    {
        GameScenario = new List<ScenarioItem>();

        LoadScenario("GameNormalQuestion");
        LoadScenario("GameSpecialQuestion");

        MetaDataLoaded = true;
    }


    public ScenarioItem CurrentScenarioItem;

    // csv파일에서 데이터 가져오기
    public void LoadScenario(string ScenarioFileName){
		TextAsset scenarioText = Resources.Load ("Scenario/" + ScenarioFileName) as TextAsset;
        /*파일 종류
         * - GameNormalQuestion : 일반 질문
         * - GameSpecialQuestion : 특별 질문
         */

        // \n을 기준으로 문자열을 쪼갠다.
        string[] list = scenarioText.text.Split ('\n');
		int lineNum = 0;

        // 시나리오리스트 객체 생성
        if(ScenarioFileName == "GameNormalQuestion")
        {
            NormalScenarioList = new List<ScenarioItem>();
        }

        if(ScenarioFileName == "GameSpecialQuestion")
        {
            SpecialScenarioList = new List<ScenarioItem>();
        }



		foreach (string line in list) {
            lineNum++;
			if (lineNum == 1) // 표의 맨 윗줄에 해당하는 첫번째 줄은 건너뛴다.
				continue;
            // 콤마를 기준으로 문자열을 쪼갠다.
			string[] values = line.Split (',');
            
            // 자료수 검사
            if (ScenarioFileName == "GameNormalQuestion" && values.Length != 32)
				continue;
            if (ScenarioFileName == "GameSpecialQuestion" && values.Length != 32)
                continue;

            ScenarioItem Item = null;
            
            Item = new ScenarioItem();

            Item.ID = int.Parse(values[0]);
            Item.Type = values[1];
            Item.Character = values[2];
            Item.Question = values[3];

            Item.Answer1 = values[4];
            Item.Money1 = int.Parse(values[5]);
            Item.Approval1 = int.Parse(values[6]);
            Item.Power1 = int.Parse(values[7]);
            Item.North1 = int.Parse(values[8]);
            Item.USA1 = int.Parse(values[9]);
            Item.China1 = int.Parse(values[10]);
            Item.Japan1 = int.Parse(values[11]);
            Item.OtherNation1 = int.Parse(values[12]);
            Item.Economy1 = int.Parse(values[13]);
            Item.Army1 = int.Parse(values[14]);
            Item.Moral1 = int.Parse(values[15]);
            Item.News1 = values[16];
            Item.Next1 = int.Parse(values[17]);

            Item.Answer2 = values[18];
            Item.Money2 = int.Parse(values[19]);
            Item.Approval2 = int.Parse(values[20]);
            Item.Power2 = int.Parse(values[21]);
            Item.North2 = int.Parse(values[22]);
            Item.USA2 = int.Parse(values[23]);
            Item.China2 = int.Parse(values[24]);
            Item.Japan2 = int.Parse(values[25]);
            Item.OtherNation2 = int.Parse(values[26]);
            Item.Economy2 = int.Parse(values[27]);
            Item.Army2 = int.Parse(values[28]);
            Item.Moral2 = int.Parse(values[29]);
            Item.News2 = values[30];
            Item.Next2 = int.Parse(values[31]);

                // 불러온 값을 시나리오리스트에 추가
                
            if(ScenarioFileName == "GameNormalQuestion")
            {
                NormalScenarioList.Add(Item);
                
            }

            if (ScenarioFileName == "GameSpecialQuestion")
            {
                SpecialScenarioList.Add(Item);
                
            }

            //Debug.Log(line);
        }
    }
	

	public List<ScenarioItem> GetNormalScenarioList(){

		if (NormalScenarioList == null) {
			LoadScenario ("GameNormalQuestion");
		}

		return NormalScenarioList;

	}

    public List<ScenarioItem> GetSpecialScenarioList()
    {
        if (SpecialScenarioList == null) {
            LoadScenario("GameSpecialQuestion");
        }

        return SpecialScenarioList;
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
        int IDFinder = 0;
        for (int index = 0; index < SpecialScenarioList.Count - 1; index++)
        {
            if (SpecialScenarioList[index].ID == QuestionID)
            {
                IDFinder = index;
                break;
            }
        }
        GameScenario.Add(SpecialScenarioList[IDFinder]);
        //Debug.Log(SpecialScenarioList[IDFinder].Question);
    }


    public List<ScenarioItem> GetGameScenario()
    {
        NormalRandom(5);
        SpecialFinder(1);
        NormalRandom(5);
        SpecialFinder(4);
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
            //_gameData.CollectGoldLevel = 1;
            

        }
    }

    public void SaveGameData()
    {

        string dataAsJson = JsonUtility.ToJson(gameData);

        string filePath = Application.persistentDataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);

    }
}
