using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineMission : IMission
{
	private CombineMissionInfo createInfo;

	public MissionType MyType { get { return MissionType.Combine; } set { } }
	public string Title { get; set; }

	public void Create(MissionCreateInfo info)
	{
		createInfo = (CombineMissionInfo)info;
	}

	public bool Evaluate(MissionEvaluateInfo info)
	{
		CombineMissionEvaluateInfo evaluateInfo = (CombineMissionEvaluateInfo)info;

		if(evaluateInfo.CombinedObject == createInfo.CombinedObject)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}

public class CombineMissionInfo : MissionCreateInfo
{
	public InteractableData CombinedObject;

	public CombineMissionInfo(InteractableData combinedObject)
	{
		CombinedObject = combinedObject;
	}
}

public class CombineMissionEvaluateInfo : MissionEvaluateInfo
{
	public InteractableData CombinedObject;

	public CombineMissionEvaluateInfo(InteractableData combinedObject)
	{
		CombinedObject = combinedObject;
	}
}
