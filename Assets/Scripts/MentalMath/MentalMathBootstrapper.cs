using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class MentalMathBootstrapper
{
	private static void EnsureMentalMathHUD()
	{
		var canvas = Object.FindObjectOfType<Canvas>();
		if (canvas == null)
		{
			Debug.LogWarning("[MentalMathBootstrapper] No Canvas found in scene. Skipping HUD creation.");
			return;
		}

		Debug.Log("[MentalMathBootstrapper] Using COMMON_Canvas for HUD (no runtime HUD creation)");

		// Wire existing GameManager to existing TimerUI and LivesUI (from COMMON_Canvas)
		var gm = Object.FindObjectOfType<GameManager>();
		var timerUI = Object.FindObjectOfType<TimerUI>();
		var livesUI = Object.FindObjectOfType<LivesUI>();
		
		Debug.Log($"[MentalMathBootstrapper] Scene objects found - GameManager={(gm!=null)}, TimerUI={(timerUI!=null)}, LivesUI={(livesUI!=null)}");
		
		// If no GameManager exists, create one
		if (gm == null)
		{
			Debug.Log("[MentalMathBootstrapper] No GameManager found, creating one...");
			var gmGO = new GameObject("GameManager");
			gm = gmGO.AddComponent<GameManager>();
			Object.DontDestroyOnLoad(gmGO);
			Debug.Log("[MentalMathBootstrapper] Created GameManager");
		}
		
		if (gm != null)
		{
			Debug.Log($"[MentalMathBootstrapper] GameManager found: {gm.name}, active: {gm.gameObject.activeInHierarchy}");
			typeof(GameManager).GetField("timerUI", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
				?.SetValue(gm, timerUI);
			typeof(GameManager).GetField("livesUI", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
				?.SetValue(gm, livesUI);
			Debug.Log($"[MentalMathBootstrapper] Wired GameManager -> TimerUI={(timerUI!=null)}, LivesUI={(livesUI!=null)}");
		}

		// If COMMON_Canvas has specific children for time bar, wire them into TimerUI
		if (timerUI != null)
		{
			var labelGO = GameObject.Find("TimeLabel");
			var barGO = GameObject.Find("FillBar");
			var label = labelGO ? labelGO.GetComponent<TextMeshProUGUI>() : null;
			var bar = barGO ? barGO.GetComponent<Image>() : null;
			if (label != null && bar != null)
			{
				timerUI.SetRefs(label, bar);
				Debug.Log("[MentalMathBootstrapper] TimerUI refs set from COMMON_Canvas");
			}
		}

		// Ensure MentalMath components exist and are wired
		EnsureMentalMath(canvas);
	}

	private static void EnsureMentalMath(Canvas canvas)
	{
		var logic = Object.FindObjectOfType<CalculLogic>();
		var ui = Object.FindObjectOfType<CalculUIManager>();
		var mgr = Object.FindObjectOfType<MiniGame_CalculManager>();

		// Ensure gameplay components exist (safe to create these; we only avoid creating HUD)
		if (logic == null)
		{
			var go = new GameObject("CalculLogic");
			go.transform.SetParent(canvas.transform, false);
			logic = go.AddComponent<CalculLogic>();
			Debug.Log("[MentalMathBootstrapper] Created CalculLogic");
		}
		if (ui == null)
		{
			var go = new GameObject("CalculUIManager");
			go.transform.SetParent(canvas.transform, false);
			ui = go.AddComponent<CalculUIManager>();
			Debug.Log("[MentalMathBootstrapper] Created CalculUIManager");
		}
		if (mgr == null)
		{
			var go = new GameObject("MiniGame_CalculManager");
			go.transform.SetParent(canvas.transform, false);
			mgr = go.AddComponent<MiniGame_CalculManager>();
			Debug.Log("[MentalMathBootstrapper] Created MiniGame_CalculManager");
		}

		// Wire UI references by scene object names
		var calcTextObj = GameObject.Find("calculation-value");
		var propTextObj = GameObject.Find("proposition-value");
		var successImgObj = GameObject.Find("SucessImage");
		var failImgObj = GameObject.Find("FailImage");

		var calcText = calcTextObj ? calcTextObj.GetComponent<TMPro.TMP_Text>() : null;
		var propText = propTextObj ? propTextObj.GetComponent<TMPro.TMP_Text>() : null;
		var successImg = successImgObj ? successImgObj.GetComponent<Image>() : null;
		var failImg = failImgObj ? failImgObj.GetComponent<Image>() : null;

		// Hide feedback images immediately to match default state
		if (successImg != null) successImg.enabled = false;
		if (failImg != null) failImg.enabled = false;

		// Assign fields via reflection (private serialized)
		if (ui != null)
		{
			typeof(CalculUIManager).GetField("questionText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
				?.SetValue(ui, calcText);
			typeof(CalculUIManager).GetField("propositionsText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
				?.SetValue(ui, propText);
			typeof(CalculUIManager).GetField("successImage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
				?.SetValue(ui, successImg);
			typeof(CalculUIManager).GetField("failImage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
				?.SetValue(ui, failImg);
		}

		var gm = Object.FindObjectOfType<GameManager>();
		if (mgr != null)
		{
			Debug.Log($"[MentalMathBootstrapper] Wiring MiniGame_CalculManager with GameManager={(gm!=null)}");
			typeof(MiniGame_CalculManager).GetField("gameManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
				?.SetValue(mgr, gm);
			typeof(MiniGame_CalculManager).GetField("calculLogic", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
				?.SetValue(mgr, logic);
			typeof(MiniGame_CalculManager).GetField("calculUIManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
				?.SetValue(mgr, ui);
			Debug.Log($"[MentalMathBootstrapper] Wired MiniGame_CalculManager -> GameManager={(gm!=null)}, Logic={(logic!=null)}, UI={(ui!=null)}");
		}

		Debug.Log($"[MentalMathBootstrapper] MentalMath wired: Logic={(logic!=null)}, UI={(ui!=null)}, Mgr={(mgr!=null)}, Q={(calcText!=null)}, P={(propText!=null)}, S={(successImg!=null)}, F={(failImg!=null)}");
	}
}
