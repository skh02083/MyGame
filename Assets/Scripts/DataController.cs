using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			}

			return _instance;
		}
	}
    // Singleton class end



    // 초기값 설정
    public int Money = 50;
	public int Approval = 50;
	public int Power = 50;
    public int North = 50;
    public int USA = 50;
    public int China = 50;
    public int Japan = 50;
    public int OtherNation = 50;
    public int Economy = 50;
	public int Army = 50;
	public int Moral = 50;



    // 모든 시나리오 내용을 담기 위해 ScenarioItem 리스트 선언
    public List<ScenarioItem> NormalScenarioList;
    public List<ScenarioItem> SpecialScenarioList;
    public List<ScenarioItem> GroupScenarioList;



    public ScenarioItem CurrentScenarioItem;

    // csv파일에서 데이터 가져오기
    public void LoadScenario(string ScenarioFileName){
		TextAsset scenarioText = Resources.Load ("Scenario/" + ScenarioFileName) as TextAsset;
        /*파일 종류
         * - GameNormalQuestion : 일반 질문
         * - GameSpecialQuestion : 분기점 질문
         * - GameGroupQuestion : 묶음 질문
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

        if(ScenarioFileName == "GameGroupQuestion")
        {
            GroupScenarioList = new List<ScenarioItem>();
        }


		foreach (string line in list) {
            lineNum++;
			if (lineNum == 1) // 표의 맨 윗줄에 해당하는 첫번째 줄은 건너뛴다.
				continue;
            // 콤마를 기준으로 문자열을 쪼갠다.
			string[] values = line.Split (',');
            
            // 자료수 검사
            if (ScenarioFileName == "GameNormalQuestion" && values.Length != 31)
				continue;
            if (ScenarioFileName == "GameSpecialQuestion" && values.Length != 31)
                continue;
            if (ScenarioFileName == "GameGroupQuestion" && values.Length != 31)
                continue;

            ScenarioItem Item = null;
            
            Item = new ScenarioItem();

            Item.ID = values[0];
            Item.Character = values[1];
            Item.Question = values[2];

            Item.Answer1 = values[3];
            Item.Money1 = int.Parse(values[4]);
            Item.Approval1 = int.Parse(values[5]);
            Item.Power1 = int.Parse(values[6]);
            Item.North1 = int.Parse(values[7]);
            Item.USA1 = int.Parse(values[8]);
            Item.China1 = int.Parse(values[9]);
            Item.Japan1 = int.Parse(values[10]);
            Item.OtherNation1 = int.Parse(values[11]);
            Item.Economy1 = int.Parse(values[12]);
            Item.Army1 = int.Parse(values[13]);
            Item.Moral1 = int.Parse(values[14]);
            Item.News1 = values[15];
            Item.Next1 = values[16];

            Item.Answer2 = values[17];
            Item.Money2 = int.Parse(values[18]);
            Item.Approval2 = int.Parse(values[19]);
            Item.Power2 = int.Parse(values[20]);
            Item.North2 = int.Parse(values[21]);
            Item.USA2 = int.Parse(values[22]);
            Item.China2 = int.Parse(values[23]);
            Item.Japan2 = int.Parse(values[24]);
            Item.OtherNation2 = int.Parse(values[25]);
            Item.Economy2 = int.Parse(values[26]);
            Item.Army2 = int.Parse(values[27]);
            Item.Moral2 = int.Parse(values[28]);
            Item.News2 = values[29];
            Item.Next2 = values[30];

                // 불러온 값을 시나리오리스트에 추가
                
            if(ScenarioFileName == "GameNormalQuestion")
            {
                NormalScenarioList.Add(Item);
            }

            if (ScenarioFileName == "GameSpecialQuestion")
            {
                SpecialScenarioList.Add(Item);
            }

            if(ScenarioFileName == "GameGroupQuestion")
            {
                GroupScenarioList.Add(Item);
            }
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

    public List<ScenarioItem> GetGroupScenarioList()
    {
        if (GroupScenarioList == null)
        {
            LoadScenario("GameGroupQuestion");
        }

        return GroupScenarioList;
    }





    public List<ScenarioItem> GameScenario;

    public void NormalRandom(int number)
    {
        for (int i = 0; i < number; i++)
        {
            List<ScenarioItem> list = GetNormalScenarioList();
            int idx = Random.Range(0, list.Count - 1);
            if(list[idx] == null)
            {
                Debug.Log("Empty");
            }
            GameScenario.Add(list[idx]);
        }
    }

    public void SpecialFinder(string QuestionID)
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
    }

    public void GroupFinder(string QuestionID)
    {
        int IDFinder = 0;
        for (int index = 0; index < GroupScenarioList.Count - 1; index++)
        {
            if (GroupScenarioList[index].ID == QuestionID)
            {
                IDFinder = index;
                break;
            }
        }
        GameScenario.Add(GroupScenarioList[IDFinder]);
    }

    public List<ScenarioItem> GetGameScenario()
    {
        if(GameScenario == null)
        {
            NormalRandom(10);
        }
        return GameScenario;
    }

    void Start(){
		LoadScenario ("GameNormalQuestion");
        LoadScenario("GameSpecialQuestion");
        LoadScenario("GameGroupQuestion");
        Debug.Log("Load Done");
        GetGameScenario();
        Debug.Log("Scenario Done");
    }

}
