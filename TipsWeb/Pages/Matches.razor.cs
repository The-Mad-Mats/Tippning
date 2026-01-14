namespace TipsWeb.Pages
{
    public partial class Matches
    {
        private List<ItemModel> Items { get; set; } = new();

        protected override void OnInitialized()
        {
            Items.Add(new ItemModel
            {
                StartTime = DateTime.Now.AddHours(1),
                Team1Flag = "images/sweden.png",
                Team1 = "Sverige",
                Team2Flag = "images/ukraina.png",
                Team2 = "Ukraina",
                Team1Score = 0,
                Team2Score = 0
            });

            Items.Add(new ItemModel
            {
                StartTime = DateTime.Now.AddDays(1),
                Team1Flag = "images/polen.png",
                Team1 = "Polen",
                Team2Flag = "images/albanien.png",
                Team2 = "Albanien",
                Team1Score = 0,
                Team2Score = 0
            });
        }

        private void Spara()
        {
            Items.Add(new ItemModel
            {
                Team1Flag = "images/default.png",
                Team1 = $"Item {Items.Count + 1}",
                Team2Flag = "images/default.png",
                Team2 = "New item",
                Team1Score = 0,
                Team2Score = 0
            });
        }

        public class ItemModel
        {
            public DateTime StartTime = DateTime.MinValue;
            public string Team1Flag { get; set; } = "";
            public string Team1 { get; set; } = "";
            public string Team2Flag { get; set; } = "";
            public string Team2 { get; set; } = "";
            public int Team1Score { get; set; } = 0;
            public int Team2Score { get; set; } = 0;
        }
    }
}
