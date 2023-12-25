# The package provides 

ScriptableObjects scripts that are organised to have available a simple Event System based on ScriptObject logic.

# How to use?

In the system is quiet simple. You start by creating a SO instance by selecting the desired event in Unity menu **Assets > Create > Noovisphere Studio > Events**.

The list of events available are the followings:
- Void event channel
- Bool event channel
- Int event channel
- GameObject event channel
- Transform event channel

The event that is generated should be stored in a dedicated folder **"ScriptableObjects"** in Assets folder for example for a best organization of your code, but it is up to you to do as you want ;)

# What is a channel?

This event system creates a none coupled connection between your script. The channel is a then just an object that represents this connection. Then it is possible to listen or to broadcast information events. 

# Where to use?

In a script component that needs to receive information, you will put as parameter the event with the right type. In order word you will listen to this channel.

- Example:

```cs

[Header("Listening on channels")]
[SerializeField] private VoidEventChannelSO _OnSceneReady = default;

private void OnEnable()
	{
		if (_OnSceneReady != null)
		{
			_OnSceneReady.OnEventRaised += SpawnPlayer;
		}
	}

private void OnDisable()
	{
		if (_OnSceneReady != null)
		{
			_OnSceneReady.OnEventRaised -= SpawnPlayer;
		}
	}

```

When the event is raised this script is informed and then can proceed with a callback on the method defined. This event is registered then it must also be unregistered to avoid persistence inside the editor during development and so avoid some troubles.

In opposition, you script can to inform others about an event that occurs, it raises an event or broadcast information. Thus you must add a parameter to broadcast to the channel.

- Example:

```cs

[Header("BroadCasting on channels")]	
[SerializeField] private VoidEventChannelSO _onSceneReady = default;

/// <summary>
/// This function is called when all the scenes have been loaded
/// </summary>
private void SetActiveScene()
{
		[...]

		_onSceneReady.RaiseEvent(); //Spawn system will spawn the player
}

```

With simple lines of code, you have a simple system that permits to decouple the different parts of your application!

# How to custom?

Of course, you can simplify create a new event channel SO by following the same logic that for example **"GameObjectEventChannelSO"**.

- Example 1:

```cs
  
using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for Item interaction events.
/// Example: Pick up an item passed as paramater
/// </summary>

[CreateAssetMenu(menuName = "Events/UI/Item Event Channel")]
public class ItemEventChannelSO : ScriptableObject
{
	public UnityAction<Item> OnEventRaised;
	public void RaiseEvent(Item item)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(item);
	}
}

```

Or something more elaborated:

- Example 2:

```cs

using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// Event on which <c>AudioCue</c> components send a message to play SFX and music. <c>AudioManager</c> listens on these events, and actually plays the sound.
/// </summary>
[CreateAssetMenu(menuName = "Events/AudioCue Event Channel")]
public class AudioCueEventChannelSO : EventChannelBaseSO
{
	public AudioCuePlayAction OnAudioCuePlayRequested;
	public AudioCueStopAction OnAudioCueStopRequested;
	public AudioCueFinishAction OnAudioCueFinishRequested;

	public AudioCueKey RaisePlayEvent(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace = default)
	{
		AudioCueKey audioCueKey = AudioCueKey.Invalid;

		if (OnAudioCuePlayRequested != null)
		{
			audioCueKey = OnAudioCuePlayRequested.Invoke(audioCue, audioConfiguration, positionInSpace);
		}
		else
		{
			Debug.LogWarning("An AudioCue play event was requested, but nobody picked it up. " +
				"Check why there is no AudioManager already loaded, " +
				"and make sure it's listening on this AudioCue Event channel.");
		}

		return audioCueKey;
	}

	public bool RaiseStopEvent(AudioCueKey audioCueKey)
	{
		bool requestSucceed = false;

		if (OnAudioCueStopRequested != null)
		{
			requestSucceed = OnAudioCueStopRequested.Invoke(audioCueKey);
		}
		else
		{
			Debug.LogWarning("An AudioCue stop event was requested, but nobody picked it up. " +
				"Check why there is no AudioManager already loaded, " +
				"and make sure it's listening on this AudioCue Event channel.");
		}

		return requestSucceed;
	}

	public bool RaiseFinishEvent(AudioCueKey audioCueKey)
	{
		bool requestSucceed = false;

		if (OnAudioCueStopRequested != null)
		{
			requestSucceed = OnAudioCueFinishRequested.Invoke(audioCueKey);
		}
		else
		{
			Debug.LogWarning("An AudioCue finish event was requested, but nobody picked it up. " +
				"Check why there is no AudioManager already loaded, " +
				"and make sure it's listening on this AudioCue Event channel.");
		}

		return requestSucceed;
	}
}

public delegate AudioCueKey AudioCuePlayAction(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace);
public delegate bool AudioCueStopAction(AudioCueKey emitterKey);
public delegate bool AudioCueFinishAction(AudioCueKey emitterKey);

```

# What is the story of this package?

The ScriptableObject approach for Event System has been originally inspired by a talk of Ryan Hipple during a Unity Event called "Unite Austin 2017". 
His conference **"Game Architecture with Scriptable Objects"** is available on Youtube [here](https://youtu.be/raQ3iHhE_Kk)

The implementation proposed in this package is more a less the same.

Another inspiration for this package comes from the Unity Open Project in 2020 called Chop Chop that uses this approach. You can find source in github repository of this project [here](https://github.com/UnityTechnologies/open-project-1)

And the session 1 of the project on Youtube [here](https://youtu.be/O4N4s6BKNH0)


