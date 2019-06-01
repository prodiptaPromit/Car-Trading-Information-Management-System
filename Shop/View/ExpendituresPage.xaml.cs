using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Shop.ViewModel;

namespace Shop.View
{
    /// <summary>
    /// Interaction logic for ExpendituresPage.xaml
    /// </summary>
    public partial class ExpendituresPage : Page
    {
        ExpenseInfoManager expenseInfoManager;
        public ExpendituresPage()
        {
            InitializeComponent();
            Init();
        }

        internal Utilities.Notify Notify1
        {
            get => default(Utilities.Notify);
            set
            {
            }
        }

        internal ExpenseInfoManager ExpenseInfoManager
        {
            get => default(ExpenseInfoManager);
            set
            {
            }
        }

        private void Init()
        {
            expenseInfoManager = new ExpenseInfoManager();
            ExpenseDataGrid.ItemsSource = expenseInfoManager.GetExpenses();
        }

        private void ClearFields()
        {
            ETitleText.Text = AmountText.Text = DateText.Text = "";
            SETitleText.Text = SDateText.Text = "";
        }

        private void Refresh()
        {
            ClearFields();
            Init();
        }

        private void ExpensesEntryBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                expenseInfoManager.InsertExpenses(ETitleText.Text, AmountText.Text, DateText.Text);
                Notify(ETitleText.Text, AmountText.Text);                
            }
            catch (Exception exception)
            {
                MessageBox.Show("Something went wrong. \n" + exception.Message);
            }
            finally
            {
                Refresh();
            }
        }

        private void SalaryExpensesEntryBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                expenseInfoManager.InsertSalaryExpense(SDateText.Text,expenseInfoManager.CalculateMonthlySalary());
                Notify("Salary paid", expenseInfoManager.CalculateMonthlySalary());
                Refresh();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Something went wrong. \n" + exception.Message);
            }
        }

        private void Notify(string title, string amount)
        {
            Shop.Utilities.Notify notify = new Utilities.Notify("Expenditure Update");
            notify.SendNotificationViaEmail(title+", at the amount of "+amount+", has performed.");
        }

        private void ExpensePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsNumber(e.Text) == false)
            {
                e.Handled = true;
            }
        }

        private bool IsNumber(string Text)
        {
            int output;
            return int.TryParse(Text, out output);
        }
    }
}
