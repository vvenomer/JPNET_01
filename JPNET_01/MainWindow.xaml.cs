using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml;

namespace JPNET_01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        XmlDocument doc;
        public MainWindow()
        {
            doc = new XmlDocument();
            doc.Load("mondial-3.0.xml");

            InitializeComponent();

            countryPopulation.Content = GetPopulation(countryName.Text);
            countryList.ItemsSource = GetCountriesNames();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var textBox = (TextBox)sender;
                countryPopulation.Content = GetPopulation(textBox.Text);
            }
        }
        private void SelectedCountryInComboBox(object sender, SelectionChangedEventArgs e)
        {
            var countryName = ((ComboBox)sender).SelectedItem.ToString();
            var country = GetCountry(countryName);
            var code = country.Attributes["datacode"].Value;
            var population = country.Attributes["population"].Value;
            var totalArea = country.Attributes["total_area"].Value;
            countryInfo.Document.Blocks.Clear();
            countryInfo.Document.Blocks.Add(new Paragraph(new Run("Kod: " + code)));
            countryInfo.Document.Blocks.Add(new Paragraph(new Run("Populacja: " + population)));
            countryInfo.Document.Blocks.Add(new Paragraph(new Run("Powierzchnia: " + totalArea)));

            country.GetElementsByName("province").ToList().ForEach(c =>
            {
                countryInfo.Document.Blocks.Add(new Paragraph(new Run("Miasto: " + c.GetElementsByName("name").First().Value)));
            });
        }

        string GetPopulation(string text)
        {
            var node = GetCountry(text);
            if (node == null)
                return "Country not found";
            return int.Parse(node.Attributes["population"].Value).ToString("0,0");
        }

        IEnumerable<XmlNode> GetCountries()
        {
            return doc.GetElementsByTagName("country").ToEnumerable();
        }

        XmlNode GetCountry(string name)
        {
            return GetCountries().GetByAttribute("name", name);
        }

        IEnumerable<string> GetCountriesNames()
        {
            return GetCountries().Select(c => c.Attributes["name"].Value);
        }

    }
}
