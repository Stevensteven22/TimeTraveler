namespace TimeTraveler.Libary.Models;

public class Flyer
{
    public double X { get; set; }  // 小球的X坐标
    public double Y { get; set; }  // 小球的Y坐标
    public double Radius { get; set; }  // 小球的半径
    public double Velocity { get; set; } // 小球的速度

    private readonly double _gravity = 0.5;  // 重力
    private readonly double _flapStrength = -10; // 跳跃力度
    
    public Flyer(double initialX, double initialY, double radius)
    {
        X = initialX;
        Y = initialY;
        Radius = radius;
        Velocity = 0;
    }
    
    // 更新小球的速度和位置
    public void UpdatePosition()
    {
        Velocity += _gravity;  // 应用重力
        Y += Velocity;  // 更新小球的Y坐标
    }

    // 触发跳跃
    public void Flap()
    {
        Velocity = _flapStrength;
    }

    // 碰撞检测：检查小球是否触底
    public bool CheckCollision(double groundY)
    {
        if (Y + Radius * 2 > groundY)
        {
            Y = groundY - Radius * 2; // 限制小球位置不掉出底部
            Velocity = 0; // 停止下落
            return true;
        }
        return false;
    }
}