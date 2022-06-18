using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
	public GameObject Player;
	public Text MissionPromptText;

	private Queue<IMission> missions;
	private IMission currentMission;

	private void Awake()
	{
		missions = new Queue<IMission>();
	}

    // Update is called once per frame
    void Update()
    {
		if (currentMission != null)
		{
			EvaluateMission();
		}
		else
		{
			if(missions.Count > 0)
			{
				currentMission = missions.Dequeue();
				SetupMission();
			}
			else
			{
				SetMissionPrompt("None");
			}
		}
    }

	private void SetupMission()
	{
		switch (currentMission.MyType)
		{
			case MissionType.Combine:
				GameEvents.OnComboExecuted += EvaluateCombine;
				break;
			default:
				break;
		}

		SetMissionPrompt(currentMission.Title);
	}

	private void FinishMission()
	{
		switch (currentMission.MyType)
		{
			case MissionType.Combine:
				GameEvents.OnComboExecuted -= EvaluateCombine;
				break;
			default:
				break;
		}

		currentMission = null;
	}

	private void EvaluateCombine(InteractionCombo combo)
	{
		if(currentMission.Evaluate(new CombineMissionEvaluateInfo(combo.Result)))
		{
			FinishMission();
		}
	}

	private void EvaluateMission()
	{
		MissionEvaluateInfo info = null;

		switch (currentMission.MyType)
		{
			case MissionType.None:
				break;
			case MissionType.GoTo:
				info = new GoToMissionEvaluateInfo(Player.transform.position);
				break;
			case MissionType.Fetch:
				info = new FetchMissionEvaluateInfo();
				break;
			default:
				break;
		}
		
		if(info == null)
		{
			return;
		}	

		if(currentMission.Evaluate(info))
		{
			FinishMission();
		}
	}

	private void SetMissionPrompt(string prompt)
	{
		MissionPromptText.text = prompt;
	}

	public void CreateGoToMission(Vector3 target, float maxDistance, string title)
	{
		GoToMission mission = new GoToMission();
		mission.Create(new GoToMissionInfo(target, maxDistance));
		mission.Title = title;
		missions.Enqueue(mission);
	}

	public void CreateFetchMission(Transform fetchObject, Vector3 targetPosition, float maxDistance, string title)
	{
		FetchMission mission = new FetchMission();
		mission.Create(new FetchMissionInfo(targetPosition, maxDistance, fetchObject));
		mission.Title = title;
		missions.Enqueue(mission);
	}

	public void CreateCombineMission(InteractableData combinedObject, string title)
	{
		CombineMission mission = new CombineMission();
		mission.Create(new CombineMissionInfo(combinedObject));
		mission.Title = title;
		missions.Enqueue(mission);
	}
}
