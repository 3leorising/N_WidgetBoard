using WidgetBoard.Data;
using WidgetBoard.Models;

namespace WidgetBoard.ViewModels;

public class BoardDetailsPageViewModel : BaseViewModel
{
    private string boardName;

    private bool isFixed = true;
    private int numberOfColumns = 3;
    private int numberOfRows = 2;

    private readonly ISemanticScreenReader semanticScreenReader;
    
    private readonly IBoardRepository boardRepository;

    public string BoardName
    {
        get => boardName;
        set
        {
            SetProperty(ref boardName, value);
            SaveCommand.ChangeCanExecute();
        }
    }

    public bool IsFixed
    {
        get => isFixed;
        set => SetProperty(ref isFixed, value);
    }

    public int NumberOfColumns
    {
        get => numberOfColumns;
        set => SetProperty(ref numberOfColumns, value);
    }

    public int NumberOfRows
    {
        get => numberOfRows;
        set => SetProperty(ref numberOfRows, value);
    }

    public Command SaveCommand { get; }

    public BoardDetailsPageViewModel(ISemanticScreenReader semanticScreenReader, IBoardRepository boardRepository)
    {
        this.semanticScreenReader = semanticScreenReader;
        this.boardRepository = boardRepository;

        SaveCommand = new Command(
            () => Save(),
            () => !string.IsNullOrWhiteSpace(BoardName));
    }

    private async void Save()
    {
        var board = new Board
        {
            Name = BoardName,
            NumberOfColumns = NumberOfColumns,
            NumberOfRows = NumberOfRows,
        };

        this.boardRepository.CreateBoard(board);

        semanticScreenReader.Announce($"A new board with the name {BoardName} was created successfully.");

        await Shell.Current.GoToAsync("fixedboard", new Dictionary<string, object>
        {
            { "Board", board }, });
    }
}
