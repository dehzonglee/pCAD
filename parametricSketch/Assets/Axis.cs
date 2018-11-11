public class Axis
{
    public Axis()
    {
        _origin = new Origin();
    }

    public Coordinate GetCoordiante(float position)
    {
        var coordinate = new Mue(_origin, position);
        return coordinate;
    }



    private Coordinate _origin;
    private Coordinate _selectedCoordinate;
}