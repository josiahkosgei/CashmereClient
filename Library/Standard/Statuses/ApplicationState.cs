// Decompiled with JetBrains decompiler
// Type: Cashmere.Library.Standard.Statuses.ApplicationState


namespace Cashmere.Library.Standard.Statuses
{
  public enum ApplicationState
  {
    NONE,
    IDLE,
    STARTUP,
    STARTUP_COMPLETE,
    APP_ERROR,
    DEVICE_ERROR,
    SPLASH,
    GATHER_REFERENCES,
    COUNT_STARTED,
    COUNTING,
    STORING,
    COUNT_ENDING,
    PRINTING,
    CIT_START,
    CIT_BAG_CLOSED,
    CIT_DOOR_OPENED,
    CIT_BAG_REMOVED,
    CIT_BAG_REPLACED,
    CIT_DOOR_CLOSED,
    CIT_BAG_OPENED,
    CIT_END,
    ESCROWJAM_START,
    ESCROWJAM_OPEN_REQUEST,
    ESCROWJAM_CLEAR_WAIT,
    ESCROWJAM_END_REQUEST,
    ESCROWJAM_END,
  }
}
