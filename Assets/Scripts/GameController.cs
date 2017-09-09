using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Text Question;
    public Text AnswerText1;
    public Text AnswerText2;
    public Text News;
    public Text MoneyStatus;
    public Text Opinion;
    public Text Congress;
    public Text Diplomacy;
    public Button Answer1, Answer2;
    public Text ShowCharacter;
    public Text Newspaper;
    public Text Timer;

    

    // Use this for initialization
    void Start () {
		UpdateStat ();

        DataController.Instance.Init();

        DataController.Instance.LoadScenario("GameNormalQuestion");
        DataController.Instance.LoadScenario("GameSpecialQuestion");
        Debug.Log("Load Done");

        LoadQuestion (0, ref DataController.Instance.gameData.ScenarioNum);
    }
	
	
    
    // Update is called once per frame
	void Update () {

    }


    string OpinionFunction()
    {
        if(DataController.Instance.gameData.Approval < 30)
        {
            return "Bad";
        } else if(DataController.Instance.gameData.Approval >= 30 && DataController.Instance.gameData.Approval < 70)
        {
            return "Normal";
        } else
        {
            return "good";
        }

    }

    string CongressFunction()
    {
        if (DataController.Instance.gameData.Power < 30)
        {
            return "Bad";
        }
        else if (DataController.Instance.gameData.Power >= 30 && DataController.Instance.gameData.Power < 70)
        {
            return "Normal";
        }
        else
        {
            return "good";
        }
    }
    

    string DiplomacyFunction()
    {
        float DiploScore = (DataController.Instance.gameData.North 
            + DataController.Instance.gameData.USA + DataController.Instance.gameData.China 
            + DataController.Instance.gameData.Japan + DataController.Instance.gameData.OtherNation) / 5;

        if (DiploScore < 30f)
        {
            return "Bad";
        }
        else if (DiploScore >= 30f && DiploScore < 70f)
        {
            return "Normal";
        }
        else
        {
            return "good";
        }
    }
    
    
    // 화면의 텍스트에 표시되도록 연결
    void UpdateStat(){
        MoneyStatus.text = DataController.Instance.gameData.Money.ToString();
        Opinion.text = OpinionFunction();
        Congress.text = CongressFunction();
        Diplomacy.text = DiplomacyFunction();
        Timer.text = DataController.Instance.gameData.ScenarioNum.ToString();
        DebugFunction();
        DataController.Instance.SaveGameData();
    }

    void DebugFunction()
    {
        string MoneyDebug = DataController.Instance.gameData.Money.ToString();
        string ApprovalDebug = DataController.Instance.gameData.Approval.ToString();
        string PowerDebug = DataController.Instance.gameData.Power.ToString();
        string NorthDebug = DataController.Instance.gameData.North.ToString();
        string USADebug = DataController.Instance.gameData.USA.ToString();
        string ChinaDebug = DataController.Instance.gameData.China.ToString();
        string JapanDebug = DataController.Instance.gameData.Japan.ToString();
        string OtherNationDebug = DataController.Instance.gameData.OtherNation.ToString();
        string EconomyDebug = DataController.Instance.gameData.Economy.ToString();
        string ArmyDebug = DataController.Instance.gameData.Army.ToString();
        string MoralDebug = DataController.Instance.gameData.Moral.ToString();

        string DebugString = "Money=" + MoneyDebug + " Approval=" + ApprovalDebug 
            + " Power=" + PowerDebug + " North=" + NorthDebug + " USA=" + USADebug 
            + " China=" + ChinaDebug + " Japan=" + JapanDebug + " OtherNation=" + OtherNationDebug 
            + " Economy" + EconomyDebug + " Army" + ArmyDebug + " Moral" + MoralDebug;
        Debug.Log(DebugString);
    }
    
    // Question 불러오기
    void LoadQuestion(int type, ref int ScenerioNum){
        List<ScenarioItem> list = DataController.Instance.GetGameScenario();
        //Debug.Log(list.Count);

        if (type == 0) // 초기 타입, 버튼 선택 없음
        {
            DataController.Instance.CurrentScenarioItem = list[DataController.Instance.gameData.ScenarioNum];
            //ScenarioNum++;
        }

        else if(type == 1) // answer1에 대하여
        {
            if (list[DataController.Instance.gameData.ScenarioNum].Next1 == 0)
            {
                //Debug.Log("Type1, null "+ list[ScenarioNum].Next1);
                DataController.Instance.gameData.ScenarioNum++;
                DataController.Instance.CurrentScenarioItem = list[DataController.Instance.gameData.ScenarioNum];
                
            } else
            {
                //Debug.Log("Type1, !null "+ list[ScenarioNum].Next1);
                int NextID = list[DataController.Instance.gameData.ScenarioNum].Next1;
                int IDFinder = 0;
                List<ScenarioItem> SpecialList = DataController.Instance.GetSpecialScenarioList();
                for (int index = 0; index < SpecialList.Count; index++)
                {
                    if (SpecialList[index].ID == NextID)
                    {
                        IDFinder = index;
                        break;
                    }
                }
                DataController.Instance.CurrentScenarioItem = SpecialList[IDFinder];
                DataController.Instance.gameData.ScenarioNum++;
            }
        }

        else if(type == 2) // answer2에 대하여
        {
            if (list[DataController.Instance.gameData.ScenarioNum].Next2 == 0)
            {
                //Debug.Log("Type2, null "+ list[ScenarioNum].Next2);
                DataController.Instance.gameData.ScenarioNum++;
                DataController.Instance.CurrentScenarioItem = list[DataController.Instance.gameData.ScenarioNum];
                
            } else
            {
                //Debug.Log("Type2, !null " + list[ScenarioNum].Next2);
                int NextID = list[DataController.Instance.gameData.ScenarioNum].Next2;
                int IDFinder = 0;
                List<ScenarioItem> SpecialList = DataController.Instance.GetSpecialScenarioList();
                for (int index = 0; index < SpecialList.Count; index++)
                {
                    Debug.Log(SpecialList.Count);
                    if (SpecialList[index].ID == NextID)
                    {
                        IDFinder = index;
                        //Debug.Log(IDFinder);
                        break;
                    }
                }
                DataController.Instance.CurrentScenarioItem = SpecialList[IDFinder];
                DataController.Instance.gameData.ScenarioNum++;
            }
        }
        ShowCharacter.text = DataController.Instance.CurrentScenarioItem.Character;
        Question.text = DataController.Instance.CurrentScenarioItem.Question;
        AnswerText1.text = DataController.Instance.CurrentScenarioItem.Answer1;
        AnswerText2.text = DataController.Instance.CurrentScenarioItem.Answer2;
	}




    
    
    // 지표 값의 범위 지정
    public void ValueRange(ref int Value)
    {
        if (Value < 0)
        {
            Value = 0;
        }

        if (Value > 100)
        {
            Value = 100;
        }
    }

    
    
    // 버튼1이 눌렸을 때 기존의 수치에서 선택지 값에 해당하는 값 더하기 
    public void OnClickAnswer1(){
        
        DataController.Instance.gameData.Money += DataController.Instance.CurrentScenarioItem.Money1;
        DataController.Instance.gameData.Approval += DataController.Instance.CurrentScenarioItem.Approval1;
		DataController.Instance.gameData.Power += DataController.Instance.CurrentScenarioItem.Power1;
        DataController.Instance.gameData.North += DataController.Instance.CurrentScenarioItem.North1;
        DataController.Instance.gameData.USA += DataController.Instance.CurrentScenarioItem.USA1;
        DataController.Instance.gameData.China += DataController.Instance.CurrentScenarioItem.China1;
        DataController.Instance.gameData.Japan += DataController.Instance.CurrentScenarioItem.Japan1;
        DataController.Instance.gameData.OtherNation += DataController.Instance.CurrentScenarioItem.OtherNation1;
        DataController.Instance.gameData.Economy += DataController.Instance.CurrentScenarioItem.Economy1;
		DataController.Instance.gameData.Army += DataController.Instance.CurrentScenarioItem.Army1;
		DataController.Instance.gameData.Moral += DataController.Instance.CurrentScenarioItem.Moral1;

        ValueRange(ref DataController.Instance.gameData.Money);
        ValueRange(ref DataController.Instance.gameData.Approval);
        ValueRange(ref DataController.Instance.gameData.Power);
        ValueRange(ref DataController.Instance.gameData.North);
        ValueRange(ref DataController.Instance.gameData.USA);
        ValueRange(ref DataController.Instance.gameData.China);
        ValueRange(ref DataController.Instance.gameData.Japan);
        ValueRange(ref DataController.Instance.gameData.OtherNation);
        ValueRange(ref DataController.Instance.gameData.Economy);
        ValueRange(ref DataController.Instance.gameData.Army);
        ValueRange(ref DataController.Instance.gameData.Moral);

        UpdateStat ();
        ShowNewspaper(1);
        LoadQuestion(1, ref DataController.Instance.gameData.ScenarioNum);
    }

    
    
    // 버튼2이 눌렸을 때 기존의 수치에서 선택지 값에 해당하는 값 더하기
    public void OnClickAnswer2(){
		DataController.Instance.gameData.Money += DataController.Instance.CurrentScenarioItem.Money2;
		DataController.Instance.gameData.Approval += DataController.Instance.CurrentScenarioItem.Approval2;
		DataController.Instance.gameData.Power += DataController.Instance.CurrentScenarioItem.Power2;
        DataController.Instance.gameData.North += DataController.Instance.CurrentScenarioItem.North2;
        DataController.Instance.gameData.USA += DataController.Instance.CurrentScenarioItem.USA2;
        DataController.Instance.gameData.China += DataController.Instance.CurrentScenarioItem.China2;
        DataController.Instance.gameData.Japan += DataController.Instance.CurrentScenarioItem.Japan2;
        DataController.Instance.gameData.OtherNation += DataController.Instance.CurrentScenarioItem.OtherNation2;
        DataController.Instance.gameData.Economy += DataController.Instance.CurrentScenarioItem.Economy2;
		DataController.Instance.gameData.Army += DataController.Instance.CurrentScenarioItem.Army2;
		DataController.Instance.gameData.Moral += DataController.Instance.CurrentScenarioItem.Moral2;

        ValueRange(ref DataController.Instance.gameData.Money);
        ValueRange(ref DataController.Instance.gameData.Approval);
        ValueRange(ref DataController.Instance.gameData.Power);
        ValueRange(ref DataController.Instance.gameData.North);
        ValueRange(ref DataController.Instance.gameData.USA);
        ValueRange(ref DataController.Instance.gameData.China);
        ValueRange(ref DataController.Instance.gameData.Japan);
        ValueRange(ref DataController.Instance.gameData.OtherNation);
        ValueRange(ref DataController.Instance.gameData.Economy);
        ValueRange(ref DataController.Instance.gameData.Army);
        ValueRange(ref DataController.Instance.gameData.Moral);

        UpdateStat();
        ShowNewspaper(2);
        LoadQuestion(2, ref DataController.Instance.gameData.ScenarioNum);
    }

    public void ShowNewspaper(int type)
    {
        string QuestionType = DataController.Instance.CurrentScenarioItem.Type;
        string Newspaper1 = DataController.Instance.CurrentScenarioItem.News1;
        string Newspaper2 = DataController.Instance.CurrentScenarioItem.News2;
        if (type == 1)
        {
            if(Newspaper1 != "null")
            {
                Newspaper.text = "[속보]" + Newspaper1;
            }
        } else if (type == 2)
        {
            if (Newspaper2 != "null")
            {
                Newspaper.text = "[속보]" + Newspaper2;
            }
        }
    }
    
    
}
