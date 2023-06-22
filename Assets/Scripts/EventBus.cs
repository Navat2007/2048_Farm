using System;
using UnityEngine;

public static class EventBus 
{
    public static class GameEvents
    {
        public static Action OnGameStarted;
        public static Action<bool> OnGameEnded;
        public static Action OnPause;
        public static Action OnUnPause;
        public static Action<int> OnTileMerged;
        public static Action OnTileSpawned;
        public static Action OnTileMoved;
    }
    
    public static class ScoreEvents
    {
        public static Action<float> OnScoreChanged;
        public static Action<float> OnMaxScoreChanged;
    }
    
    public static class UIEvents
    {
        public static Action OnMainMenuWindowShow;
        public static Action OnPauseWindowShow;
        public static Action OnPauseWindowHide;
        public static Action OnSettingsWindowShow;
        public static Action OnLeaderBoardWindowShow;
        public static Action<float> OnTimerChanged;
    }
    
    public static class InputEvents
    {
        public static Action<Vector2> OnInputMoveChange;
    }
    
    public static class AdsEvents
    {
        public static Action OnAdsNeedToShow;
        public static Action OnAdsShown;
        public static Action OnAdsClose;
        public static Action OnAdsFailed;
    }
}
