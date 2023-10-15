using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WindowCalculate
{
    public partial class MessageBox : Window
    {
        public MessageBox()
        {
            InitializeComponent();
        }
        void AddButtons(MessageBoxButton buttons)
        {
            switch (buttons)
            {
                
                case MessageBoxButton.OK:
                    AddButton("Да", MessageBoxResult.OK);
                    break;
                case MessageBoxButton.OKCancel:
                    AddButton("Да", MessageBoxResult.OK);
                    AddButton("Нет", MessageBoxResult.Cancel, isCancel: true);
                    break;
                case MessageBoxButton.YesNo:
                    AddButton("Да", MessageBoxResult.Yes);
                    AddButton("Нет", MessageBoxResult.No);
                    break;
                case MessageBoxButton.YesNoCancel:
                    AddButton("Да", MessageBoxResult.Yes);
                    AddButton("Нет", MessageBoxResult.No);
                    AddButton("Отмена", MessageBoxResult.Cancel, isCancel: true);
                    break;
                default:
                    throw new ArgumentException("Unknown button value", "buttons");
            }
        }

        void AddButton(string text, MessageBoxResult result, bool isCancel = false)
        {
            var button = new Button() { Content = text, IsCancel = isCancel };
            button.Click += (o, args) => { Result = result; DialogResult = true; };
            ButtonContainer.Children.Add(button);
            
        }

        MessageBoxResult Result = MessageBoxResult.None;

        public static bool Show(string message, MessageBoxButton buttons)
        {
            var dialog = new MessageBox();
            dialog.MessageContainer.Text = message;
            dialog.AddButtons(buttons);
            dialog.ShowDialog();
            var sdf = dialog.Result.ToString();
            if (dialog.Result.ToString() == "Yes")
            {
                return true;
            }
            return false;
        }
    }
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    class RightMenu : INotifyPropertyChanged
    {
        public string MyProperty { get; }
        private string _dataGridWidthPresented;
        public string DataGridWidthPresented
        {
            get { return _dataGridWidthPresented; }
            set
            {
                if (_dataGridWidthPresented == value)
                    return;
                _dataGridWidthPresented = value;
                OnPropertyChanged("MoneyOnDay");
            }
        }
        private string _dataGridWidth;
        public string DataGridWidth
        {
            get { return _dataGridWidth; }
            set
            {
                if (_dataGridWidth == value)
                    return;
                _dataGridWidth = value;
                OnPropertyChanged("DataGridWidth");
            }
        }
        private string _moneyOnDay;
        public string MoneyOnDay
        {
            get { return _moneyOnDay; }
            set
            {
                if (_moneyOnDay == value)
                    return;
                _moneyOnDay = value;
                OnPropertyChanged("MoneyOnDay");
            }
        }
        private string _limitMoney;
        public string LimitMoney
        {
            get { return _limitMoney; }
            set
            {
                if (_limitMoney == value)
                    return;
                _limitMoney = value;
                OnPropertyChanged("IsLimitMoney");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    class TodoModel : INotifyPropertyChanged
    {
        public string Day { get; set; }
        private string _transfer;
        public string Transfer 
        {
            get { return _transfer; }
            set {
                if (_transfer == value)
                    return;
                _transfer = value;
                OnPropertyChanged("IsTransfer");
            } 
        }
        private string _spend;
        public string Spend
        {
            get { return _spend; }
            set
            {
                if (_spend == value)
                    return;
                _spend = value;
                OnPropertyChanged("IsPrice");
            }
        }
        private string _rest;
        public string Rest
        {
            get { return _rest; }
            set
            {
                if (_rest == value)
                    return;
                _rest = value;
                OnPropertyChanged("IsPrice");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class MainKekw
    {
        public static bool InfoForAutoEnter() ///ctor generated msg
        {
            return MessageBox.Show("Хотите ли заполнить дату отсчёта от первого числа?", MessageBoxButton.YesNo);
        }
        
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void DGExpand_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGrid Dg = sender as DataGrid;
            if (Dg != null)
            {
                List<DataGridCellInfo> ldgci = new List<DataGridCellInfo>();
                if (e.AddedCells.Count > 0 && e.AddedCells[0].Column.DisplayIndex != 1)
                    Dg.SelectedCells.Clear();
            }
            

        }
        private BindingList<TodoModel> _tododatalist;
        private static DataGrid kekkk;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RightMenu rightMenu = new RightMenu();
            DataTimeTable dateNow = new DataTimeTable();
            ManageSqlCommand sqlCommand = new ManageSqlCommand();
            CalculateDay generatedTable = new CalculateDay();
            _tododatalist = new BindingList<TodoModel>();
            //загружается БД, перенести save и load в изменения
            List<string> fields = new List<string> { "Day", "Transfer", "Expenses" };
            main_grid.ItemsSource =  sqlCommand.Select(fields, dateNow.Mounth_year, ref _tododatalist);
            _tododatalist.ListChanged += _todoDataList_ListChanged;
            kekkk = main_grid;
            //Transfer.Width = DataGridLength.SizeToCells;
            //Expences.Width = DataGridLength.SizeToCells;
            /*var fsadfsdaf = Transfer.Width.Value;
            var sdg = main_grid.Columns[1].ActualWidth;
            var sdgdsfg = main_grid.DesiredSize.Width;
            var yusdf = rightMenu.MyProperty;
            rightMenu.DataGridWidth = (Data.Width.Value + Expences.Width.Value + Transfer.Width.Value + Rest.Width.Value).ToString();
            rightMenu.DataGridWidthPresented = (Data.Width.Value + Expences.Width.Value + Transfer.Width.Value + Rest.Width.Value - 2).ToString();
            var tiii = main_grid.ColumnWidth.Value;
            //main_grid.RowHeaderWidth = "Auto";
            main_grid.MinWidth = 701 /*Data.Width.Value + Expences.Width.Value + Transfer.Width.Value + Rest.Width.Value*/;
            main_grid.Height = main_grid.DesiredSize.Height * (_tododatalist.Count);
            

        }
        //возможно не понадобится в будущем
        private static void _todoDataList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.ItemChanged)
            {

            }

        }
        private void main_grid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            TodoModel valueCell = (TodoModel)e.Row.Item;
            if ((string)e.Column.Header == "Переводы")
            {
                if (valueCell.Transfer != null)
                    valueCell.Transfer = valueCell.Transfer.Trim('₽');
            }
            if ((string)e.Column.Header == "Расходы")
            {
                if (valueCell.Spend != null)
                    valueCell.Spend = valueCell.Spend.Trim('₽');
            }
        }
        private string ProcessingCorrectValues(string fieldTodoModel)
        {
            if (fieldTodoModel != null && fieldTodoModel != "")
            {
                var splitValueCell = fieldTodoModel.Split(new char[] { ',' }, 3);
                if (splitValueCell.Length >= 2)
                {
                    if (splitValueCell[1].Length == 1)
                        splitValueCell[1] += "0";
                    if (splitValueCell[0] == "")
                        splitValueCell[0] = "0";
                    string concatToDecimal = splitValueCell[0] + "," + splitValueCell[1];
                    fieldTodoModel = concatToDecimal;
                }
                if (fieldTodoModel.EndsWith(","))
                    fieldTodoModel += "00";
                else if (fieldTodoModel.Contains(",") == false)
                    fieldTodoModel += ",00";
                ///Logic decimal -> string
                decimal decimalFieldCell;
                try
                {
                    decimalFieldCell = Convert.ToDecimal(fieldTodoModel);
                }
                catch (Exception)
                {
                    decimalFieldCell = decimal.MaxValue;
                }
                fieldTodoModel = decimal.Round(decimalFieldCell, 2).ToString();
                fieldTodoModel += "₽";
            }
            return fieldTodoModel;

        }
        private void main_grid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            RightMenu rightMenu = new RightMenu();
            ManageSqlCommand sqlCommand = new ManageSqlCommand();
            DataTimeTable dataNow = new DataTimeTable();
            TodoModel valueCell = (TodoModel)e.Row.Item;
            if ((string)e.Column.Header == "Переводы")
            {
                valueCell.Transfer = ProcessingCorrectValues(valueCell.Transfer);
                sqlCommand.Update( "Transfer", dataNow.Mounth_year, valueCell.Transfer, e.Row.GetIndex() + 1);
            }
            if ((string)e.Column.Header == "Расходы")
            {
                valueCell.Spend = ProcessingCorrectValues(valueCell.Spend);
                sqlCommand.Update("Expenses", dataNow.Mounth_year, valueCell.Spend, e.Row.GetIndex() + 1);
            }
            _tododatalist = new BindingList<TodoModel>();
            DataTimeTable dateNow = new DataTimeTable();
            //загружается БД, перенести save и load в изменения
            List<string> fields = new List<string> { "Day", "Transfer", "Expenses" };
            main_grid.ItemsSource = sqlCommand.Select(fields, dateNow.Mounth_year, ref _tododatalist);
            Transfer.Width = DataGridLength.SizeToCells;
            Expences.Width = DataGridLength.SizeToCells;
            rightMenu.DataGridWidth = (Data.Width.Value + Expences.Width.Value + Transfer.Width.Value + Rest.Width.Value).ToString();
            rightMenu.DataGridWidthPresented = (Data.Width.Value + Expences.Width.Value + Transfer.Width.Value + Rest.Width.Value - 2).ToString();

        }

        private void main_grid_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == ",")
            {
                e.Handled = false;
            }
            else if (new Regex("^[0-9]+").IsMatch(e.Text) == false)
            {
                e.Handled = true;
            }
        }

        private void main_grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            //var textdfgBox = (TextBox)sender;
            //textdfgBox.Text = "234";
            
            TodoModel valueCell = new TodoModel();
            ManageSqlCommand sqlCommand = new ManageSqlCommand();
            RightMenu valueCellMenu = new RightMenu();
            valueCellMenu.LimitMoney = textBox.Text;
            valueCellMenu.LimitMoney = ProcessingCorrectValues(textBox.Text);
            textBox.Text = valueCellMenu.LimitMoney;
            string furuteTrend = textBox.Text.Trim('₽');
            
            sqlCommand.InsertInto("CountMoney", "UserInfo", valueCellMenu.LimitMoney);
            //TodoModel kekw = new TodoModel();
            valueCellMenu.MoneyOnDay = ModelTrend(textBox.Text.Trim('₽'), "default");
            DataContext = valueCellMenu;
            
            textBox.Text = valueCellMenu.LimitMoney;
        }
        private static BindingList<TodoModel> _htododatalist;
        public static string ModelTrend(string moneyMonth, string model)
        {
            if (model == "default")
            {
                DataTimeTable dateNow = new DataTimeTable();
                ManageSqlCommand sqlCommand = new ManageSqlCommand();
                CalculateDay generatedTable = new CalculateDay();
                List<string> fields = new List<string> { "Day", "Transfer", "Expenses" };
                decimal moneyDay = Convert.ToDecimal(moneyMonth) / Convert.ToDecimal(dateNow.CountDayMounth);
                string moneyString = decimal.Round(moneyDay, 2).ToString() + "₽";
                
                sqlCommand.UpdateManyDay(moneyString);
                //загружается БД, перенести save и load в изменения
                _htododatalist = new BindingList<TodoModel>();
                RightMenu kekw = new RightMenu();
                kekw.MoneyOnDay = moneyString;
                var t = sqlCommand.Select(fields, dateNow.Mounth_year, ref _htododatalist);
                
                kekkk.ItemsSource = t;
                return kekw.MoneyOnDay;
            }
            return null;
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == ",")
            {
                e.Handled = false;
            }
            else if (new Regex("^[0-9]+").IsMatch(e.Text) == false)
            {
                e.Handled = true;
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
            if (e.Key == Key.Enter)
            {
                TextBox textBox = sender as TextBox;
                if (e.Key == Key.Return)
                {
                    textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = textBox.Text.Trim('₽');
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void main_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}