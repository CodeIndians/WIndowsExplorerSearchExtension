using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WIndowsExplorerSearchExtension
{
  
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Read the config file
        /// Populate the list view with the file/folder names
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_Click(object sender, EventArgs e)
        {
            // search term
            string searchTerm = "folder";

            // TODO: API call, replace MockSearchApi with the actual API
            List<string> searchResults = MockSearchApi(searchTerm);

            // Update ListView with search results
            listView1.Items.Clear();
            foreach (var resultPath in searchResults)
            {
                listView1.Items.Add(new ListViewItem(resultPath));
            }
        }

        private RootObject ReadConfig()
        {
            string configFilePath = Path.Combine(Application.StartupPath, "config.json");
            string json = File.ReadAllText(configFilePath);
            RootObject config = JsonConvert.DeserializeObject<RootObject>(json);
            return config;
        }


        private List<string> MockSearchApi(string searchTerm)
        {
            // Read search results from the config.json file
            RootObject config = ReadConfig();

            List<string> filteredPaths = new List<string>();

            // Assuming you want to return all paths if searchTerm is empty, or paths that contain the searchTerm otherwise
            foreach (var result in config.SearchResults)
            {
                if (string.IsNullOrEmpty(searchTerm) || result.ResultPath.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    filteredPaths.Add(result.ResultPath);
                }
            }

            return filteredPaths;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            // Clear the search results and search term
            listView1.Items.Clear();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            // Check if an item is selected
            if (listView1.SelectedItems.Count == 1)
            {
                // Get the first selected item
                ListViewItem item = listView1.SelectedItems[0];

                // This will be the path to the file or folder, stored in the item's Text property.
                string path = item.Text;

                // Use the Process.Start method to open the file or folder
                try
                {
                    System.Diagnostics.Process.Start(path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to open file or folder: {ex.Message}");
                }
            }
        }
    }
    public class RootObject
    {
        public List<string> Drives { get; set; }
        public List<SearchResult> SearchResults { get; set; }
    }

    public class SearchResult
    {
        public string NodeType { get; set; }
        public string ResultPath { get; set; }
    }
}
