using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogControllerBudget : DialogController
{
	public Text LabelTitle;
	public Text LabelMessage;

	DialogDataBudget Data {
		get;
		set;
	}

	public override void Awake ()
	{
		base.Awake ();
	}

	public override void Start ()
	{
		base.Start ();

		DialogManager.Instance.Regist( DialogType.Budget, this );
	}

    public override void Build(DialogData data)
    {
		base.Build(data);
		
		if( ! (data is DialogDataBudget) ) {
			Debug.LogError("Invalid dialog data!");
			return;
		}
		
		Data = data as DialogDataBudget;
		LabelTitle.text = Data.Title;
		LabelMessage.text = Data.Message;
    }

    public void OnClickOK()
    {
        // calls child's callback
        if (Data!=null && Data.Callback != null)
            Data.Callback();

		DialogManager.Instance.Pop();
    }
}
