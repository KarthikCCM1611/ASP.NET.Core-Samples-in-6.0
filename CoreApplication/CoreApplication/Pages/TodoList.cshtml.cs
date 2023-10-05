using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Xml;
using static CoreApplication.Pages.TodoListModel;

namespace CoreApplication.Pages
{
    [IgnoreAntiforgeryToken]
    public class TodoListModel : PageModel
    {
        public List<Items> items = new List<Items>();
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TodoListModel(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public void OnGet()
        {
            AddExistingItemsInList();
        }

        private List<Items> AddExistingItemsInList()
        {
            // Get the path to the wwwroot folder
            string webRootPath = _webHostEnvironment.WebRootPath;

            // Combine the path with the file name
            string filePath = Path.Combine(webRootPath, "list.json");

            if (System.IO.File.Exists(filePath))
            {
                // Read the JSON from the file
                string json = System.IO.File.ReadAllText(filePath);

                // Deserialize the JSON into a list of items
                var newItems = JsonConvert.DeserializeObject<List<Items>>(json);
                foreach (var newItem in newItems)
                {
                    items.Add(newItem);
                }
            }
            return items;
        }

        public IActionResult OnPostAddItem(int id, string itemVal) {
            AddExistingItemsInList();
            int itemId = id;
            string itemValue = itemVal;
            var newItem = new Items(itemId, itemValue);
            items.Add(newItem);
            // Serialize the list to JSON
            string json = JsonConvert.SerializeObject(items, Newtonsoft.Json.Formatting.Indented);

            // Get the path to the wwwroot folder
            string webRootPath = _webHostEnvironment.WebRootPath;

            // Combine the path with the file name
            string filePath = Path.Combine(webRootPath, "list.json");

            // Write the JSON to the file
            System.IO.File.WriteAllText(filePath, json);

            return Content("Added the new item ");
        }

        public IActionResult OnPutUpdateItem(int id, string itemVal)
        {
            AddExistingItemsInList();
            foreach(var currentItem in items)
            {
                if(currentItem.id == id)
                {
                    currentItem.itemName = itemVal;
                }
            }

            // Serialize the list to JSON
            string json = JsonConvert.SerializeObject(items, Newtonsoft.Json.Formatting.Indented);

            // Get the path to the wwwroot folder
            string webRootPath = _webHostEnvironment.WebRootPath;

            // Combine the path with the file name
            string filePath = Path.Combine(webRootPath, "list.json");

            // Write the JSON to the file
            System.IO.File.WriteAllText(filePath, json);

            return Content("Updated the item :" + id);
        }

        public IActionResult OnDeleteItem(int id)
        {
            AddExistingItemsInList();
            var itemToRemove = items.FirstOrDefault(item => item.id == id);
            if (itemToRemove != null)
            {
                items.Remove(itemToRemove);
            }

            // Serialize the list to JSON
            string json = JsonConvert.SerializeObject(items, Newtonsoft.Json.Formatting.Indented);

            // Get the path to the wwwroot folder
            string webRootPath = _webHostEnvironment.WebRootPath;

            // Combine the path with the file name
            string filePath = Path.Combine(webRootPath, "list.json");

            // Write the JSON to the file
            System.IO.File.WriteAllText(filePath, json);

            return Content("Deleted item :" + id);
        }

        public IActionResult OnDeleteItems()
        {
            AddExistingItemsInList();

            items.Clear();

            // Serialize the list to JSON
            string json = JsonConvert.SerializeObject(items, Newtonsoft.Json.Formatting.Indented);

            // Get the path to the wwwroot folder
            string webRootPath = _webHostEnvironment.WebRootPath;

            // Combine the path with the file name
            string filePath = Path.Combine(webRootPath, "list.json");

            // Write the JSON to the file
            System.IO.File.WriteAllText(filePath, json);

            return Content("All the items are Deleted");
        }


        public class Items
        {
            public int id;
            public string itemName;

            public Items(int id, string itemName)
            {
                this.id = id;
                this.itemName = itemName;
            }
        }
    }
}
