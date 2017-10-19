using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class LobbyController : MonoBehaviour {

#if UNITY_IOS
	private string gameId = "1547779";
#elif UNITY_ANDROID
    private string gameId = "1547778";
#endif

    // Use this for initialization
    IEnumerator Start () {

        while (DataController.Instance.gameData == null || DataController.Instance.MetaDataLoaded == false)
        {
            yield return new WaitForSecondsRealtime(0.1f);
        } // metaData가 로드 되기 전까지 대기한다. 

        if (Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId);
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0))
        {
            Application.LoadLevel("Game");
        }
	}
}
