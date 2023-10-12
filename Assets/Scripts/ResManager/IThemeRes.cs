

public interface IThemeRes
{
    // 游戏的主题背景
    public string GetThemeBgTexturePath();

    // 首页的背景装饰
    public string GetThemeBgDecorateTexturePath();

    // 游戏页的顶部装饰
    public string GetThemeGameViewTopDecorateTexturePaht();

    // 数字贴图
    public string GetNumberTexturePath();

    // 数字默认背景贴图
    public string GetNumberCellBgDefaultTexturePath();

    // 数字选中效果背景贴图
    public string GetNumberCellBgHighlightTexturePath();

    // 首页的spine动画
    public string GetHomeSpinePath();

    // bingo和reach的spine动画
    public string GetBingoSpinePath();

    // 转盘的背景spine动画
    public string GetRotateBgSpinePath();
}