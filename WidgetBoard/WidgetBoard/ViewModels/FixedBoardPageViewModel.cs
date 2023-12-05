using System.Collections.ObjectModel;
using WidgetBoard.Views;
using WidgetBoard.Models;
using System.Windows.Input;
using System.Runtime.InteropServices;
using WidgetBoard.Data;

namespace WidgetBoard.ViewModels;

public class FixedBoardPageViewModel : BaseViewModel, IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var boardParameter = query["Board"] as Board;

        preferences.Set("LastUsedBoardId", board.Id);

        board = boardRepository.LoadBoard(boardParameter.Id);

        BoardName = board.Name;

        NumberOfColumns = ((FixedLayout)board.Layout).NumberOfColumns;
        NumberOfRows = ((FixedLayout)board.Layout).NumberOfRows;

        foreach (var boardWidget in board.BoardWidgets)
        {
            var widgetViewmodel = WidgetFactory.CreateWidgetViewModel(boardWidget.WidgetType);

            widgetViewmodel.Position = boardWidget.Position;
                
            Widgets.Add(widgetViewmodel);
        }
    }

    private string boardName;
    private int numberOfColumns;
    private int numberOfRows;

    private int addingPosition;
    private string selectedWidget;
    private bool isAddingWidget;
    private readonly WidgetFactory widgetFactory;

    private Board board;
    private readonly IBoardRepository boardRepository;
    private readonly IPreferences preferences;

    public string BoardName
    {
        get => boardName;
        set => SetProperty(ref boardName, value);
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

    public FixedBoardPageViewModel(WidgetTemplateSelector widgetTemplateSelector, IPreferences preferences, WidgetFactory widgetFactory, IBoardRepository boardRepository) 
    {
        WidgetTemplateSelector = widgetTemplateSelector;
        this.widgetFactory = widgetFactory;
        this.boardRepository = boardRepository;

        Widgets = new ObservableCollection<IWidgetViewModel>();

        AddWidgetCommand = new Command(OnAddWidget);

        AddNewWidgetCommand = new Command<int>(index =>
        {
            IsAddingWidget = true;
            addingPosition = index;
        });
    }
    public ObservableCollection<IWidgetViewModel> Widgets { get; }

    public WidgetTemplateSelector WidgetTemplateSelector { get; }

    public IList<string> AvailableWidgets => widgetFactory.AvailableWidgets;

    public ICommand AddWidgetCommand { get; }

    public ICommand AddNewWidgetCommand { get; }

    public bool IsAddingWidget
    {
        get => isAddingWidget;
        set => SetProperty(ref isAddingWidget, value);
    }

    public string SelectedWidget
    {
        get => selectedWidget;
        set => SetProperty(ref selectedWidget, value);
    }

    private void OnAddWidget()
    {
        if (SelectedWidget is null)
        { return; }

        var widgetViewModel = widgetFactory.CreateWidgetViewModel(SelectedWidget);

        widgetViewModel.Position = addingPosition;

        Widgets.Add(widgetViewModel);

        SaveWidget(widgetViewModel);

        isAddingWidget = false;
    }

    private void SaveWidget(IWidgetViewModel widgetViewModel)
    {
        var boardWidget = new BoardWidget
        {
            BoardId = board.Id,
            PositionChangedEventArgs = widgetViewModel.Position,
            WidgetType = widgetViewModel.Type
        };

        boardRepository.CreateBoardWidget(boardWidget);
    }
}
