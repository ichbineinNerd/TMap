namespace TMap.Data
{
    public class Tile
    {
        public Tile()
        {
            Active = true;
            Id = 0;
            FrameX = -1;
            FrameY = -1;
            Color = 0;
            Wall = 0;
            WallColor = 0;
            LiquidAmount = 0;
            Lava = false;
            Honey = false;
            RedWire = false;
            BlueWire = false;
            GreenWire = false;
            YellowWire = false;
            HalfBrick = false;
            Slope = 0;
            Actuator = false;
        }

        public Tile(Tile t)
        {
            Active = t.Active;
            Id = t.Id;
            FrameX = t.FrameX;
            FrameY = t.FrameY;
            Color = t.Color;
            Wall = t.Wall;
            WallColor = t.WallColor;
            LiquidAmount = t.LiquidAmount;
            Lava = t.Lava;
            Honey = t.Honey;
            RedWire = t.RedWire;
            BlueWire = t.BlueWire;
            GreenWire = t.GreenWire;
            YellowWire = t.YellowWire;
            HalfBrick = t.HalfBrick;
            Slope = t.Slope;
            Actuator = t.Actuator;
        }

        public static bool operator ==(Tile t, Tile t2)
        {
            if (t is null && t2 is null)
                return true;
            if (t is null || t2 is null)
                return false;
            return t.Active == t2.Active && t.Id == t2.Id && t.FrameX == t2.FrameX && t.FrameY == t2.FrameY &&
                   t.Color == t2.Color && t.Wall == t2.Wall && t.WallColor == t2.WallColor &&
                   t.LiquidAmount == t2.LiquidAmount && t.Lava == t2.Lava && t.Honey == t2.Honey &&
                   t.RedWire == t2.RedWire && t.BlueWire == t2.BlueWire && t.GreenWire == t2.GreenWire &&
                   t.YellowWire == t2.YellowWire && t.HalfBrick == t2.HalfBrick && t.Slope == t2.Slope &&
                   t.Actuator == t2.Actuator;
        }

        public static bool operator !=(Tile t, Tile t2)
        {
            return !(t == t2);
        }

        public bool Active;
        public int Id;
        public short FrameX;
        public short FrameY;
        public byte Color;
        public ushort Wall;
        public byte WallColor;
        public byte LiquidAmount;
        public bool Lava;
        public bool Honey;
        public bool RedWire;
        public bool BlueWire;
        public bool GreenWire;
        public bool YellowWire;
        public bool HalfBrick;
        public byte Slope;
        public bool Actuator;
    }
}
