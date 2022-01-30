using UnityEngine;

public static class ColorPallete
{
    public readonly static Color32 Blue7  = new Color(0,0,1,0.7f);
    public readonly static Color32 Green7 = new Color(0,1,0,0.7f);
    public readonly static Color32 Red7   = new Color(1,0,0,0.7f);

    //Red
    public readonly static Color32 IndianRed = new Color32(205, 92, 92,255);

    public readonly static Color32 CrimsonRed = new Color32(220, 20, 60,255);
    public readonly static Color32 TomatoRed = new Color32(255, 99, 71,255);
    public readonly static Color32 FireBrickRed = new Color32(178, 34, 34,255);
    public readonly static Color32 Coral = new Color32(255, 127, 80,255);
    public readonly static Color32 VioletRed = new Color32(199, 21, 133,255);

    //Pink
    public readonly static Color32 Pink = new Color32(216, 191, 216,255);

    public readonly static Color32 HotPink = new Color32(255, 105, 180,255);
    public readonly static Color32 MistyRosePink = new Color32(255, 228, 225, 255);
    public readonly static Color32 DeepPink = new Color32(255, 20, 147,255);
    public readonly static Color32 Thistle = new Color32(216, 191, 216,255);

    //Voilet
    public readonly static Color32 OrchidPink = new Color32(218, 112, 214,255);

    public readonly static Color32 Violet = new Color32(238, 130, 238,255);
    public readonly static Color32 Plum = new Color32(221, 160, 221,255);

    //Purple
    public readonly static Color32 Purple = new Color32(128, 0, 128,255);

    public readonly static Color32 Indigo = new Color32(70, 0, 130, 255);
    public readonly static Color32 OrchidPurple = new Color32(185, 85, 211,255);
    public readonly static Color32 MediumPurple = new Color32(147, 112, 219,255);

    //Blue
    public readonly static Color32 DodgerBlue = new Color32(30, 144, 255,255);

    public readonly static Color32 RoyalBlue = new Color32(65, 105, 225,255);
    public readonly static Color32 SlateBlue = new Color32(106, 90, 205,255);
    public readonly static Color32 CornflowerBlue = new Color32(100, 149, 237,255);
    public readonly static Color32 MediumSlateBlue = new Color32(123,104,238,255);
    public readonly static Color32 Turquaise = new Color32(64,224,208,255);
    public readonly static Color32 MidnightBlue = new Color32(25,25,112,255);

    //Green
    public readonly static Color32 LightSeaGreen = new Color32(32, 178, 170,255);

    public readonly static Color32 LawnGreen = new Color32(124, 252, 0,255);
    public readonly static Color32 MediumSeaGreen = new Color32(60, 179, 113,255);
    public readonly static Color32 LeafGreen = new Color32(173, 255, 47,255);

    //Yellow
    public readonly static Color32 GoldenrodYellow = new Color32(218, 165, 32,255);

    public readonly static Color32 PeruYellow = new Color32(205, 133, 63,255);

    //Orange
    public readonly static Color32 Orange = new Color32(255, 165, 0,255);

    public readonly static Color32 DarkOrange = new Color32(255, 140, 0,255);

    //brown
    public readonly static Color32 Sienna = new Color32(160, 82, 45,255);

    public readonly static Color32 Chocolate = new Color32(210, 105, 30,255);
    public readonly static Color32 SaddleBrown = new Color32(139, 69, 19,255);
    public readonly static Color32 RosyBrown = new Color32(188, 143, 143,255);

    //Tan
    public readonly static Color32 Tan = new Color32(210, 180, 140, 255);

    public readonly static Color32 Wheat = new Color32(245, 222, 179,255);
    public readonly static Color32 Bisque = new Color32(255, 220, 196,255);
    public readonly static Color32 BlanchedAlmond = new Color32(255, 235, 205,255);

    //White
    public readonly static Color32 SilverWhite = new Color(0.752f,0.752f,0.752f,255);

    public readonly static Color32 NavajoWhite = new Color32(255, 222, 173,255);
    public readonly static Color32 SeashellWhite = new Color32(255, 245, 238,255);
    public readonly static Color32 Linen = new Color32(250, 240, 230,255);

    public readonly static Color[] Rarity =
    {
        new Color(0.5f, 0.5f, 0.5f, 1),//회
        new Color(0.9f, 0.9f, 0.9f, 1),//흰
        new Color(0.3f, 0.8f, 0.3f, 1),//녹
        new Color(0.3f, 0.3f, 0.8f, 1),//파
        new Color(0.6f, 0.3f, 0.9f, 1),//보
        new Color(0.9f, 0.6f, 0.3f, 1),//주
        new Color(0.8f, 0.8f, 0.3f, 1),//노
        new Color(0.8f, 0.3f, 0.3f, 1),//빨
    };

    public readonly static Color[] Tier =
    {
        Int(200,130,62),//동
        Int(245, 245, 220),//은
        Int(255, 215, 0),//금
        Int(32, 178, 170),//플
        Int(135, 206, 250),//다
        Int(147, 112, 219),//마
        DarkOrange,//첼
    };

    public static Color Int(int r, int g, int b)
    {
        return new Color(r / 255f, g / 255f, b / 255f);
    }
}