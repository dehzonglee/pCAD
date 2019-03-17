namespace Model
{
    public class AnchorCoordinates
    {
        public Coordinate PrimaryCoordinate { get; private set; }

        public Coordinate SecondaryCoordinate { get; private set; }

        public bool AnchorsMatch => PrimaryCoordinate == SecondaryCoordinate;

        public AnchorCoordinates(Origin origin)
        {
            _origin = origin;
            PrimaryCoordinate = origin;
            SecondaryCoordinate = origin;
        }

        public void SetPrimaryCoordinate(Coordinate c)
        {
            SecondaryCoordinate = PrimaryCoordinate;
            PrimaryCoordinate = c;
        }

        public void ResetPrimaryCoordinate()
        {
            PrimaryCoordinate = _origin;
        }

        public void ResetSecondaryCoordinate()
        {
            SecondaryCoordinate = _origin;
        }

        private readonly Origin _origin;
    }
}