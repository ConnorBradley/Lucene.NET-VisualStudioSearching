//------------------------------------------------------------------------------
// <copyright file="RapidSearchWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Data;
using System.Text;

namespace RapidSearch
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for RapidSearchWindowControl.
    /// </summary>
    public partial class RapidSearchWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RapidSearchWindowControl"/> class.
        /// </summary>
        public RapidSearchWindowControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            FileSearcher f = new FileSearcher();
            StringBuilder sb = new StringBuilder();
            var dt = f.Search(textBox.Text);

            foreach (DataRow r in dt.Rows)
            {
                foreach (DataColumn c in dt.Columns)
                {
                    sb.Append(c.ColumnName.ToString() + ":" + r[c.ColumnName].ToString());

                }
            }

            MessageBox.Show(sb.ToString());
          
        }
    }
}