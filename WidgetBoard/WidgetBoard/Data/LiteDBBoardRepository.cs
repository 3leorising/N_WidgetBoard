using LiteDB;
using WidgetBoard.Models;

namespace WidgetBoard.Data;

public class LiteDBBoardRepository : IBoardRepository
{
    private readonly LiteDatabase database;
    private readonly ILiteCollection<Board> boardCollection;
    private readonly ILiteCollection<BoardWidget> boardWidgetCollection;
    

    public LiteDBBoardRepository(IFileSystem fileSystem)
    {
        var dbPath = Path.Combine(fileSystem.AppDataDirectory, "widgetboard_litedb.db");

        database = new LiteDatabase(dbPath);

        boardCollection = database.GetCollection<Board>("Boards");
        boardWidgetCollection = database.GetCollection<BoardWidget>("BoardWidgets");

        boardCollection.EnsureIndex(b => b.Id, true);
        boardCollection.EnsureIndex(b => b.Name, false);

    }

    public void CreateBoard(Board board)
    {
        boardCollection.Insert(board);
    }

    public void CreateBoardWidget (BoardWidget boardWidget)
    {
        throw new NotImplementedException();
    }

    public void DeleteBoard (Board board)
    {
        boardCollection.Delete(board.Id);
    }

    public IReadOnlyList<Board>ListBoards()
    {
        return boardCollection.Query().OrderBy(b => b.Name).ToList();
    }

    public Board LoadBoard(int boardId)
    {
        var board = boardCollection.FindById(boardId);
        var baordWidgets = boardWidgetCollection.Find(w => w.BoardId == boardId).ToList;

        board.BoardWidgets = BoardWidgets;

        return board;
    }

    public void UpdateBoard(Board board)
    {
        boardCollection.Update(board);
    }
}
