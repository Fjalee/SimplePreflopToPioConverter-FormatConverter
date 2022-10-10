namespace FormatConverter
{
    public class Player
    {
        public string PositionName { get; set; }
        public PositionEnum Position { get; set; }

        public Player(string positionName, PositionEnum position)
        {
            if(String.IsNullOrEmpty(positionName))
            {
                throw new ArgumentNullException("Passed player PostionName is \"" + PositionName + "\". It must not be null or empty");
            }
            PositionName = positionName;
            Position = position;
        }
    }
}
