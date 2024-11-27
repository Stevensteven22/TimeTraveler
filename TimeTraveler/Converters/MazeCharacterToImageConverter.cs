using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

using System;
using System.Globalization;
using Avalonia.Data.Converters;

public class MazeCharacterToImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return null;

        string mazeChar = value.ToString();
        string imagePath = string.Empty;
        

        switch (mazeChar)
        {
            case "#":
                // 假设墙壁使用的图像
                imagePath = "avares://TimeTraveler/Assets/Wall.png";
                break;
            case " ":
                // 假设空格使用的图像
                imagePath = "/Assets/Floor.png";
                break;
            case "P":
                // 玩家位置使用图标
                imagePath = "/Assets/Player.png";
                break;
            default:
                imagePath = "/Assets/Floor.png"; // 默认图像路径
                break;
        }

      
        return imagePath; // 返回图像路径
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
