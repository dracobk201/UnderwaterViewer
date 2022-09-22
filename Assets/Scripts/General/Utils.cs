using UnityEngine;

public static class Utils
{
    #region Tags
    public const string PlayerTag = "Player";
    public const string CreatureTag = "Fish";
    #endregion

    #region ActionMaps
    public const string UnderwaterActionMap = "Underwater";
    public const string CameraActionMap = "Camera";
    #endregion

    #region Functions

    public static void ShowPanel(CanvasGroup targetCanvasGroup, bool value)
    {
        if (value)
        {
            targetCanvasGroup.alpha = 1;
        }
        else
        {
            targetCanvasGroup.alpha = 0;
        }
        targetCanvasGroup.blocksRaycasts = value;
    }

    public static int AxisCheck(bool value)
    {
        int invertedFactor = 1;

        if (value)
        {
            invertedFactor = -1;
        }

        return invertedFactor;
    }

    #endregion
}
