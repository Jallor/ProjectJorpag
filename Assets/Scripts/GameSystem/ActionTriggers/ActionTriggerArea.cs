using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ActionTriggerArea : ActionTrigger
{
}


[System.Serializable] [SelectImplementationName("On Enter Area")]
public class OnEnterAreaActionTrigger : ActionTriggerArea
{
}

[System.Serializable] [SelectImplementationName("On Exit Area")]
public class OnExitAreaActionTrigger : ActionTriggerArea
{
}
