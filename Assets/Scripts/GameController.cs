using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using UnityEngine.Advertisements;

public class GameController : MonoBehaviour {

#if UNITY_IOS
	private string gameId = "1547779";
#elif UNITY_ANDROID
    private string gameId = "1547778";
#endif

    public Text Question;
    public Text AnswerText1;
    public Text AnswerText2;
    public Text ApprovalText;
    public Text WelfareText;
    public Text EconomyText;
    public Text DiplomacyText;
    public Text ArmyText;
    public Button Answer1, Answer2;
    public Text ShowCharacter;
    public Text Timer;

    

    // Use this for initialization
    IEnumerator Start () {
		UpdateStat ();

        Debug.Log("Load Done");
        while (DataController.Instance.gameData == null || DataController.Instance.MetaDataLoaded == false) {
            yield return new WaitForSecondsRealtime(0.1f);
        } // metaData가 로드 되기 전까지 대기한다. 
        
        LoadQuestion (0, ref DataController.Instance.gameData.ScenarioNum);

        if (Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId);
        }
    }
	
	
    
    // Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            ShowRewardedVideo();
            Application.Quit();
    }

    // 화면의 텍스트에 표시되도록 연결
    void UpdateStat(){
        ApprovalText.text = DataController.Instance.gameData.Approval.ToString();
        WelfareText.text = DataController.Instance.gameData.Welfare.ToString();
        DiplomacyText.text = DataController.Instance.gameData.Diplomacy.ToString();
        EconomyText.text = DataController.Instance.gameData.Economy.ToString();
        ArmyText.text = DataController.Instance.gameData.Army.ToString();
        Timer.text = DataController.Instance.gameData.ScenarioNum.ToString();
        DebugFunction();
        DataController.Instance.SaveGameData();
    }

    void DebugFunction()
    {
        string ApprovalDebug = DataController.Instance.gameData.Approval.ToString();
        string WelfareDebug = DataController.Instance.gameData.Welfare.ToString();
        string EconomyDebug = DataController.Instance.gameData.Economy.ToString();
        string DiplomacyDebug = DataController.Instance.gameData.Diplomacy.ToString();
        string ArmyDebug = DataController.Instance.gameData.Army.ToString();
        string MoralDebug = DataController.Instance.gameData.Moral.ToString();
        string PowerDebug = DataController.Instance.gameData.Power.ToString();

        string DebugString = " Approval=" + ApprovalDebug 
            + " Welfare=" + WelfareDebug + " Economy=" + EconomyDebug + "Diplomacy=" + DiplomacyDebug
            + " Army=" + ArmyDebug + " Moral=" + MoralDebug + "Power=" + PowerDebug;
        Debug.Log(DebugString);
    }
    
    // Question 불러오기
    void LoadQuestion(int type, ref int ScenerioNum){
        List<ScenarioItem> list = DataController.Instance.GetGameScenario();
        if(DataController.Instance.gameData.ScenarioNum > list.Count)
        {
            DataController.Instance.gameData.ScenarioNum = 0;
        }

        if (type == 0) // 초기 타입, 버튼 선택 없음
        {
            Debug.Log(DataController.Instance.gameData.ScenarioNum);
            DataController.Instance.CurrentScenarioItem = list[DataController.Instance.gameData.ScenarioNum];
            //ScenarioNum++;
        }

        else if(type == 1) // answer1에 대하여
        {
            if (list[DataController.Instance.gameData.ScenarioNum].Next1 == 0)
            {
                DataController.Instance.gameData.ScenarioNum++;
                DataController.Instance.CurrentScenarioItem = list[DataController.Instance.gameData.ScenarioNum];
                
            } else
            {
                int NextID = list[DataController.Instance.gameData.ScenarioNum].Next1;
                Debug.Log(NextID);
                DataController.Instance.CurrentScenarioItem = DataController.Instance.ScenarioDic[NextID];
                DataController.Instance.gameData.ScenarioNum++;
            }
        }

        else if(type == 2) // answer2에 대하여
        {
            if (list[DataController.Instance.gameData.ScenarioNum].Next2 == 0)
            {
                DataController.Instance.gameData.ScenarioNum++;
                DataController.Instance.CurrentScenarioItem = list[DataController.Instance.gameData.ScenarioNum];
                
            } else
            {
                int NextID = list[DataController.Instance.gameData.ScenarioNum].Next2;
                Debug.Log(NextID);
                DataController.Instance.CurrentScenarioItem = DataController.Instance.ScenarioDic[NextID];
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
        DataController.Instance.gameData.Approval += DataController.Instance.CurrentScenarioItem.Approval1;
		DataController.Instance.gameData.Welfare += DataController.Instance.CurrentScenarioItem.Welfare1;
        DataController.Instance.gameData.Economy += DataController.Instance.CurrentScenarioItem.Economy1;
        DataController.Instance.gameData.Diplomacy += DataController.Instance.CurrentScenarioItem.Diplomacy1;
		DataController.Instance.gameData.Army += DataController.Instance.CurrentScenarioItem.Army1;
		DataController.Instance.gameData.Moral += DataController.Instance.CurrentScenarioItem.Moral1;
        DataController.Instance.gameData.Power += DataController.Instance.CurrentScenarioItem.Power1;


        ValueRange(ref DataController.Instance.gameData.Approval);
        ValueRange(ref DataController.Instance.gameData.Welfare);
        ValueRange(ref DataController.Instance.gameData.Economy);
        ValueRange(ref DataController.Instance.gameData.Army);
        ValueRange(ref DataController.Instance.gameData.Moral);
        ValueRange(ref DataController.Instance.gameData.Power);

        UpdateStat ();
        LoadQuestion(1, ref DataController.Instance.gameData.ScenarioNum);
    }

    
    
    // 버튼2이 눌렸을 때 기존의 수치에서 선택지 값에 해당하는 값 더하기
    public void OnClickAnswer2(){
		DataController.Instance.gameData.Approval += DataController.Instance.CurrentScenarioItem.Approval2;
		DataController.Instance.gameData.Welfare += DataController.Instance.CurrentScenarioItem.Welfare2;
        DataController.Instance.gameData.Economy += DataController.Instance.CurrentScenarioItem.Economy2;
        DataController.Instance.gameData.Diplomacy += DataController.Instance.CurrentScenarioItem.Diplomacy2;
        DataController.Instance.gameData.Army += DataController.Instance.CurrentScenarioItem.Army2;
		DataController.Instance.gameData.Moral += DataController.Instance.CurrentScenarioItem.Moral2;
        DataController.Instance.gameData.Power += DataController.Instance.CurrentScenarioItem.Power2;

        ValueRange(ref DataController.Instance.gameData.Approval);
        ValueRange(ref DataController.Instance.gameData.Welfare);
        ValueRange(ref DataController.Instance.gameData.Economy);
        ValueRange(ref DataController.Instance.gameData.Army);
        ValueRange(ref DataController.Instance.gameData.Moral);
        ValueRange(ref DataController.Instance.gameData.Power);

        UpdateStat();
        LoadQuestion(2, ref DataController.Instance.gameData.ScenarioNum);
    }


    public void GameReset()
    {
        DataController.Instance.gameData.ScenarioNum = 0;

        DataController.Instance.gameData.Approval = 50;

        DataController.Instance.gameData.Welfare = 50;

        DataController.Instance.gameData.Economy = 50;

        DataController.Instance.gameData.Diplomacy = 50;

        DataController.Instance.gameData.Army = 50;

        DataController.Instance.gameData.Moral = 50;

        DataController.Instance.gameData.Power = 50;

        UpdateStat();
}

    public void PurchaseComplete(Product p)
    {
        Debug.Log(p.metadata.localizedTitle + " purchase success!");
    }

    void ShowRewardedVideo()
    {
        var options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        Advertisement.Show("rewardedVideo", options);
    }

    void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("Video completed - Offer a reward to the player");

        }
        else if (result == ShowResult.Skipped)
        {
            Debug.LogWarning("Video was skipped - Do NOT reward the player");

        }
        else if (result == ShowResult.Failed)
        {
            Debug.LogError("Video failed to show");
        }
    }

}
