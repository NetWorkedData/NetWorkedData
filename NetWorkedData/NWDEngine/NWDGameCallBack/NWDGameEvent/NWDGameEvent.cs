using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class NWDGameEvent : UnityEvent <Dictionary<string,object>>
	{

	}
	//-------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class NWDGameDatasUpdated : UnityEvent <Dictionary<string,object>>
	{
	}
	//-------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class NWDGameDatasStart : UnityEvent <Dictionary<string,object>>
	{
	}
	//-------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class NWDGameDatasError : UnityEvent <Dictionary<string,object>, NWDError>
	{
	}
	//-------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class NWDGameDatasSuccess : UnityEvent <Dictionary<string,object>>
	{
	}
	//-------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class NWDGameAccountStart : UnityEvent <Dictionary<string,object>, NWDAppEnvironmentPlayerStatut>
	{
	}
	//-------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class NWDGameAccountError : UnityEvent <Dictionary<string,object>, NWDAppEnvironmentPlayerStatut, NWDError>
	{
	}
	//-------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class NWDGameAccountSuccess : UnityEvent <Dictionary<string,object>, NWDAppEnvironmentPlayerStatut>
	{
	}
	//-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================