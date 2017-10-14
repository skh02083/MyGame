using System;

public class DialogDataBudget : DialogData 
{
	public string Title { 
		get; 
		private set; 
	}

	public string Message { 
		get; 
		private set; 
	}

	public Action Callback {
		get; 
		private set; 
	}

	public DialogDataBudget(string title, string message, Action callback = null)
		: base( DialogType.Budget )
	{
		this.Title = title;
		this.Message = message;
		this.Callback = callback;
	}
}
