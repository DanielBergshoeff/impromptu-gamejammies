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
				SetMissionPrompt(currentMission.Title);
			}
			else
			{
				SetMissionPrompt("None");
			}
		}
    }

	private void EvaluateMission()
	{
		switch (currentMission.MyType)
		{
			case MissionType.None:
				break;
			case MissionType.GoTo:
				if(currentMission.Evaluate(new GoToMissionEvaluateInfo(Player.transform.position)))
				{
					currentMission = null;
				}
				break;
			case MissionType.Fetch:
				if(currentMission.Evaluate(new FetchMissionEvaluateInfo()))
				{
					currentMission = null;
				}
				break;
			default:
				break;
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
}
