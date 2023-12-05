using System.Windows.Input;
using WidgetBoard.Controls;

namespace WidgetBoard.Layouts;

public class FixedLayoutManager : BindableObject, ILayoutManager
{
    public static readonly BindableProperty NumberOfColumnsProperty = BindableProperty.Create(
        nameof(NumberOfColumns), 
        typeof(int), typeof(FixedLayoutManager), 
        defaultBindingMode: BindingMode.OneWay, 
        propertyChanged: OnNumberOfColumnsChanged);

    public static readonly BindableProperty NumberOfRowsProperty = BindableProperty.Create(
        nameof(NumberOfRows), 
        typeof(int), 
        typeof(FixedLayoutManager), 
        defaultBindingMode: BindingMode.OneWay, 
        propertyChanged: OnNumberOfRowsChanged);

    public static readonly BindableProperty PlacerholderTappedCommandProperty = BindableProperty.Create(
        nameof(PlaceholderTappedCommand), 
        typeof(ICommand), 
        typeof(FixedLayoutManager));

    private BoardLayout board;
    private bool isInitialised;

    public int NumberOfColumns
    {
        get => (int)GetValue(NumberOfColumnsProperty);
        set => SetValue(NumberOfColumnsProperty, value);
    }

    static void OnNumberOfColumnsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var manager = (FixedLayoutManager)bindable;
        manager.InitialiseGrid();
    }

    public int NumberOfRows
    {
        get => (int)GetValue(NumberOfRowsProperty);
        set => SetValue(NumberOfRowsProperty, value);
    }

    static void OnNumberOfRowsChanged (BindableObject bindable, object oldValue, object newValue) 
    {
        var manager = (FixedLayoutManager)bindable;
        manager.InitialiseGrid();
    }

    public ICommand PlaceholderTappedCommand
    {
        get => (ICommand)GetValue(PlacerholderTappedCommandProperty);
        set => SetValue(PlacerholderTappedCommandProperty, value);
    }

    private void TapGestureRecognizer_Tapped (object sender, EventArgs e)
    {
        if (sender is Placeholder placeholder)
        {
            if (PlaceholderTappedCommand?.CanExecute(placeholder.Position) == true)
            {
                PlaceholderTappedCommand.Execute(placeholder.Position);
            }
        }
    }

    public BoardLayout Board
    {
        get => board;
        set
        {
            board = value;
            InitialiseGrid();
        }
    }

    private void InitialiseGrid () 
    { 
        if (Board is null || NumberOfColumns == 0 || NumberOfRows == 0 || isInitialised == true)
        { return; }

        isInitialised = true;

        for (int i = 0; i < NumberOfColumns; i++)
        {
            Board.AddColumn(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
        }

        for (int i = 0; i < NumberOfRows; i++)
        {
            Board.AddRow(new RowDefinition(new GridLength(1, GridUnitType.Star)));
        }

        for (int column = 0; column < NumberOfColumns; column++)
        {
            for (int row = 0; row < NumberOfRows; row++)
            {
                var placeholder = new Placeholder();
                placeholder.Position = row * NumberOfColumns + column;
                
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;

                placeholder.GestureRecognizers.Add(tapGestureRecognizer);

                Board.AddPlaceholder(placeholder);

                Grid.SetColumn(placeholder, column);
                Grid.SetRow(placeholder, row);
            }
        }
    }

    public void SetPosition (BindableObject bindableObject, int position)
    {
        if (NumberOfColumns == 0)
        {
            return;
        }

        int column = position % NumberOfColumns;
        int row = position / NumberOfRows;

        Grid.SetColumn (bindableObject, column);
        Grid.SetRow (bindableObject, row);
        var placeholder = Board.Placeholders.Where(p => p.Position ==  position).FirstOrDefault();

        if (placeholder is not null)
        {
            Board.RemovePlaceholder(placeholder);
        }
    }
}
