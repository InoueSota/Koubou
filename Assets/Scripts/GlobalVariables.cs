using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    // �t���O��
    public static bool isClear = false;
    public static bool isGetItem1 = false;
    public static bool isGetItem2 = false;

    // ���O��
    public static string retryStageName = "Stage1";
    public static string nextStageName = "Stage1";

    // �F
    public static int colorNum = 0;
    public static Color color1 = new(1f, 1f, 0.0f);
    public static Color color2 = new(1f, 0f, 1f);
}
