
public class ChristmasThemeResConfig : IThemeRes
{
    public string GetBingoSpinePath()
    {
        throw new System.NotImplementedException();
    }

    public string GetHomeSpinePath()
    {
        throw new System.NotImplementedException();
    }

    public string GetNumberCellBgDefaultTexturePath()
    {
        return "Textures/DefaultTheme/CellBg/Panel_Black";
    }

    public string GetNumberCellBgHighlightTexturePath()
    {
        return "Textures/DefaultTheme/CellBg/Panel_Yellow";
    }

    public string GetNumberTexturePath()
    {
        return "Textures/DefaultTheme/Numbers/Atari_No{0}";
    }

    public string GetRotateBgSpinePath()
    {
        throw new System.NotImplementedException();
    }

    public string GetThemeBgTexturePath()
    {
        return "Textures/ChristmasThemes/bg";
    }

    public string GetThemeBgDecorateTexturePath()
    {
        return "Textures/DefaultTheme/none";
    }

    public string GetThemeGameViewTopDecorateTexturePaht()
    {
        return "Textures/DefaultTheme/none";
    }
}