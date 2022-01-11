using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for Events that have one int argument.
/// Example: An item unlock event, where the int is the Item ID.
/// </summary>

[CreateAssetMenu(menuName = "Noovisphere Studio/Events/Int Event Channel")]
public class IntEventChannelSO : EventChannelBaseSO
{
	public UnityAction<int> OnEventRaised;
	public void RaiseEvent(int value)
	{
		OnEventRaised.Invoke(value);
	}
}
