using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WSForm.Models;

namespace WSForm
{
    /// <summary>
    /// Interaction logic for StatisticWindow.xaml
    /// </summary>
    public partial class StatisticWindow : Window
    {
        List<Book> books;
        public StatisticWindow(List<Book> books)
        {
            this.books = books;
            InitializeComponent();
            showColumnChart(processList());
        }

        private List<KeyValuePair<string, int>> processList()
        {
            List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();

            var bookTypeList = books.GroupBy(x => x.BookTypeName).Select(x => new { Name = x.Key, Count = x.Sum(y => int.Parse(y.Count)) }).ToList();
            foreach (var bookType in bookTypeList)
            {
                valueList.Add(new KeyValuePair<string, int>(bookType.Name, bookType.Count));
            }

            return valueList;
        }

        private void showColumnChart(List<KeyValuePair<string, int>> valueList)
        {

            //Setting data for column chart
            columnChart.DataContext = valueList;

            // Setting data for pie chart
            pieChart.DataContext = valueList;

            //Setting data for area chart
            areaChart.DataContext = valueList;

            //Setting data for bar chart
            barChart.DataContext = valueList;
        }
    }
}
