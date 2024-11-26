namespace TimeTraveler.Libary.Models;

public class Obstacle
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    private static int _obstacleIndex = 0;
    public int Appearances { get; private set; } = 0;  // 记录出现次数

    // 初始化时，障碍物的位置和大小应该是合理的
    public Obstacle(double x, double y, double width, double height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    // 更新障碍物的位置，向左移动
    public void UpdatePosition1(double speed)
    {
        X -= speed; // 每次更新时障碍物向左移动
       

        // 确保障碍物在屏幕左侧时重新生成
        if (X + Width < 0)
        {
            X = 1200; // 假设屏幕宽度为 800
            if (new Random().Next(0, 2) == 0)
            {
                Y = 0; // 上边贴紧屏幕
            }
            else
            {
                Y = 800 - Height; // 下边贴紧屏幕
            }
        }
    }
    
    public void UpdatePosition2(double speed)
    {
        X -= speed; // 每次更新时障碍物向左移动
       

        // 确保障碍物在屏幕左侧时重新生成
        if (X + Width < 0)
        {
            X = 800; // 假设屏幕宽度为 800
            Y = new Random().Next(0, 550);

        }
    }
}