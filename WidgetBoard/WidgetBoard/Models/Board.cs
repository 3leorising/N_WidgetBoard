using SQLite;

namespace WidgetBoard.Models;

public class Board
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; init; }
    public int NumberOfColumns { get; init; }
    public int NumberOfRows { get; init; }

    //this var wasnt in the book at all.
    //im sorry i had to look at the reference code on github,
    //but a lot of errors pointed at this class...
    public IList<BoardWidget> BoardWidgets { get; set; }
    //public BaseLayout Layout { get; init; }
}
