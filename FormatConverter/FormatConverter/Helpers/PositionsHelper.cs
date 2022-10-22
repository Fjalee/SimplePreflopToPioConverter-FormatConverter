namespace FormatConverter.Helpers
{
    public static class PositionsHelper
    {
        public static Player GetPlayer(string positionString, PositionNamesOptions positionNames)
        {
            if (positionNames.SBName == positionString)
            {
                return new Player(positionNames.SBName, PositionEnum.SB);
            }
            else if (positionNames.BBName == positionString)
            {
                return new Player(positionNames.BBName, PositionEnum.BB);
            }
            else if (positionNames.UTGName == positionString)
            {
                return new Player(positionNames.UTGName, PositionEnum.UTG);
            }
            else if (positionNames.MP1Name == positionString)
            {
                return new Player(positionNames.MP1Name, PositionEnum.MP1);
            }
            else if (positionNames.MP2Name == positionString)
            {
                return new Player(positionNames.MP2Name, PositionEnum.MP2);
            }
            else if (positionNames.MP3Name == positionString)
            {
                return new Player(positionNames.MP3Name, PositionEnum.MP3);
            }
            else if (positionNames.HIJName == positionString)
            {
                return new Player(positionNames.HIJName, PositionEnum.HIJ);
            }
            else if (positionNames.COName == positionString)
            {
                return new Player(positionNames.COName, PositionEnum.CO);
            }
            else if (positionNames.BTNName == positionString)
            {
                return new Player(positionNames.BTNName, PositionEnum.BTN);
            }
            else
            {
                throw new ArgumentException("Position: " + positionString + ", was not declared in PositionHelper");
            }
        }

        public static Player GetPlayer(PositionEnum positionEnum, PositionNamesOptions positionNames)
        {
            switch (positionEnum)
            {
                case PositionEnum.SB:
                    return new Player(positionNames.SBName, PositionEnum.SB);
                case PositionEnum.BB:
                    return new Player(positionNames.BBName, PositionEnum.BB);
                case PositionEnum.UTG:
                    return new Player(positionNames.UTGName, PositionEnum.UTG);
                case PositionEnum.MP1:
                    return new Player(positionNames.MP1Name, PositionEnum.MP1);
                case PositionEnum.MP2:
                    return new Player(positionNames.MP2Name, PositionEnum.MP2);
                case PositionEnum.MP3:
                    return new Player(positionNames.MP3Name, PositionEnum.MP3);
                case PositionEnum.HIJ:
                    return new Player(positionNames.HIJName, PositionEnum.HIJ);
                case PositionEnum.CO:
                    return new Player(positionNames.COName, PositionEnum.CO);
                case PositionEnum.BTN:
                    return new Player(positionNames.BTNName, PositionEnum.BTN);
                default:
                    throw new ArgumentException("PositionEnum: " + positionEnum + ", was not declared in PositionHelper");
            }
        }
    }
}
