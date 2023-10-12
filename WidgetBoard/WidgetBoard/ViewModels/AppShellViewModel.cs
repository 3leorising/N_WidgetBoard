using System.Collections.ObjectModel;
using WidgetBoard.Models;

namespace WidgetBoard.ViewModels;

public class AppShellViewModel : BaseViewModel
{
    public ObservableCollection<Board> Boards { get; } = new ObservableCollection<Board>();

    public AppShellViewModel() 
    {
        Boards.Add(
            new Board
            {
                Name = "My first board",
                Layout = new FixedLayout
                {
                    NumberOfColumns = 3,
                    NumberOfRows = 2,
                }
            });
    }

    private Board currentBoard;
    public Board CurrentBoard
    {
        get => currentBoard;
        set
        {
            if(SetProperty(ref currentBoard, value)) 
            {
                BoardSelected(value);
            }
        }
    }

    private async void BoardSelected(Board board)
    {
        await Shell.Current.GoToAsync(
            "fixedboard",
            new Dictionary<string, object>
            { { "Board", board}});
    }
}
