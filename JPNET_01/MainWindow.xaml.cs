using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

namespace JPNET_01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        class ComboBoxData
        {
            public ComboBoxData(string name, string population)
            {
                DisplayName = name + " (" + population + ")";
                Name = name;
            }
            public string DisplayName { get; private set; }
            public string Name { get; private set; }

            public override string ToString()
            {
                return DisplayName;
            }
        }

        XDocument doc;
        public MainWindow()
        {
            doc = XDocument.Load("mondial-3.0.xml");

            InitializeComponent();

            countryPopulation.Content = GetPopulation(countryName.Text);
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
            var countryName = ((ComboBox)sender).SelectedItem as ComboBoxData;
            var country = GetCountry(countryName.Name);
            var code = country.Attribute("datacode").Value;
            var population = country.Attribute("population").Value;
            var totalArea = country.Attribute("total_area").Value;

            countryInfo.Document.Blocks.Clear();
            AddTextToRichTextBox(countryInfo, "Kod: " + code);
            AddTextToRichTextBox(countryInfo, "Populacja: " + population);
            AddTextToRichTextBox(countryInfo, "Powierzchnia: " + totalArea);

            foreach (var city in country.Descendants("city"))
            {
                AddTextToRichTextBox(countryInfo, "Miasto: " + city.Element("name").Value.Trim());
            }
        }


        void AddTextToRichTextBox(RichTextBox textBox, string text)
        {
            textBox.Document.Blocks.Add(new Paragraph(new Run(text)));
        }

        string GetPopulation(string text)
        {
            var node = GetCountry(text);
            if (node == null)
                return "Country not found";
            return int.Parse(node.Attribute("population").Value).ToString("0,0");
        }

        XElement GetCountry(string name)
        {
            return doc.Descendants("country").FirstOrDefault(c => c.Attribute("name").Value == name);
        }

        private async void ComboBoxLoaded(object sender, RoutedEventArgs e)
        {
            countryList.ItemsSource = await Task.WhenAll(doc.Find(100000).Select(async c =>
                await Task.Run(() =>
                {
                    Thread.Sleep(100);
                    return new ComboBoxData(c.Attribute("name").Value, c.Attribute("population").Value);
                })
            ));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var maxLine = 20;
            var wholeTxt = doc.ToString();
            var blockList = new List<TextBlock>();
            for(int i = 0; i < wholeTxt.Length; i += maxLine)
            {
                var block = new TextBlock(new Run(wholeTxt.Substring(i, Math.Min(maxLine, wholeTxt.Length - i))));

            }
            countryInfo.Document.Blocks.Clear();
            AddTextToRichTextBox(countryInfo, wholeTxt);
        }
    }
}
