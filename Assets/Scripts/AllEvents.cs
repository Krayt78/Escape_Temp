using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

#region GameManager Events
public class GameMenuEvent : SDD.Events.Event
{
}
public class GamePlayEvent : SDD.Events.Event
{
}
public class GamePauseEvent : SDD.Events.Event
{
}
public class GameResumeEvent : SDD.Events.Event
{
}
public class GameOverEvent : SDD.Events.Event
{
}
public class GameVictoryEvent : SDD.Events.Event
{
}

public class GameStatisticsChangedEvent : SDD.Events.Event
{
	public float eBestScore { get; set; }
	public float eScore { get; set; }
	public int eNLives { get; set; }
}

#endregion

#region MenuManager Events
public class EscapeButtonClickedEvent : SDD.Events.Event
{
}
public class PlayButtonClickedEvent : SDD.Events.Event
{
}
public class ResumeButtonClickedEvent : SDD.Events.Event
{
}
public class MainMenuButtonClickedEvent : SDD.Events.Event
{
}

public class QuitButtonClickedEvent : SDD.Events.Event
{ }
public class OptionButtonClickedEvent : SDD.Events.Event
{ }
#endregion

#region Score Event
public class ScoreItemEvent : SDD.Events.Event
{
	public float eScore;
}
#endregion

#region Player Event

public class PlayerDieEvent : SDD.Events.Event
{

}

public class BallCollisionEvent : SDD.Events.Event
{

}

public class ChangingSceneEvent : SDD.Events.Event
{

}



#endregion

#region Ball Event
public class BallRespawnEvent : SDD.Events.Event
{

}

#endregion

public class LevelWonEvent : SDD.Events.Event
{

}

public class LevelWithdrawEvent : SDD.Events.Event
{

}
public class OnGameplaySceneLoadedEvent : SDD.Events.Event
{

}


public class PlayerRespawnEvent : SDD.Events.Event
{

}

public class ChangingSceneToMenuEvent : SDD.Events.Event
{

}

public class FuelAmountChangedEvent : SDD.Events.Event
{
    public float eFuelAmount { get; set; }
}
public class LifeAmountChangedEvent : SDD.Events.Event
{
    public int eLifeAmount { get; set; }
}

public class MainMenuLoadedEvent : SDD.Events.Event
{
}

public class OnHealthUpdatedEvent : SDD.Events.Event
{
    public float Health { get; set; }
    public float MaxHealth { get; set; }
}

public class OnTabletGrabEvent : SDD.Events.Event
{
    public NomTablet nom { get; set; }
}
