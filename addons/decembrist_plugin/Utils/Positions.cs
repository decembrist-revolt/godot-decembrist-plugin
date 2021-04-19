using Godot;

namespace Decembrist.Utils
{
    public static class Positions
    {
        public static Vector2 RandomOutOfZonePoint(Rect2 zone)
        {
            var onLeftSide = Random.RandomBool();
            Vector2 result;

            var newZoneX = onLeftSide ? zone.Position.x - zone.Size.x : zone.Position.x + zone.Size.x;
            
            var newZonePosition = new Vector2(newZoneX, zone.Position.y);
            var newZone = new Rect2(newZonePosition, zone.Size);
            return RandomZonePoint(newZone);
        }

        public static Vector2 RandomZonePoint(Rect2 zone)
        {
            return Random.RandomVector(zone.Position, zone.End);
        }
    }
}
