namespace TOZTestApp
{
    public class MatchCalendarItem
    {
        private protected DateTime _matchDate;
        public DateTime MatchDate { get { return _matchDate; } set { _matchDate = value; } }
        private protected string _matchPlace;
        public string MatchPlace { get { return _matchPlace; } set { _matchPlace = value; } }
        private protected string _matchType;
        public string MatchType { get { return _matchType; } set { _matchType = value; } }
        private string _teamName;
        public string TeamName { get { return _teamName; } set { _teamName = value; } }
        private int _teamScore;
        public int TeamScore { get { return _teamScore; }set{ _teamScore = value; } }

        private string _teamTown;
        public string TeamTown { get { return _teamTown; } set { _teamTown = value; } }
        private string _teamName1;
        public string TeamName1 { get { return _teamName1; } set { _teamName1 = value; } }
        private int _teamScore1;
        public int TeamScore1 { get { return _teamScore1; } set { _teamScore1 = value; } }
        private string _teamTown1;
        public string TeamTown1 { get { return _teamTown1; } set { _teamTown1 = value; } }
        private string _hrefToResult;
        public string HrefToResult { get { return _hrefToResult; }set { _hrefToResult = value; } }
        public MatchCalendarItemSQLite ToSQLite()
        {
            MatchCalendarItemSQLite matchCalendarItemSQLite = new MatchCalendarItemSQLite();
            matchCalendarItemSQLite.MatchType = _matchType;
            matchCalendarItemSQLite.MatchPlace = _matchPlace;
            matchCalendarItemSQLite.TeamName = _teamName;
            matchCalendarItemSQLite.TeamName1 = _teamName1;
            matchCalendarItemSQLite.TeamTown = _teamTown;
            matchCalendarItemSQLite.TeamTown1 = _teamTown1;
            matchCalendarItemSQLite.MatchDate = _matchDate.ToString();
            return matchCalendarItemSQLite;
        }
    }

    public class MatchCalendarItemSQLite // Нужно что бы создать в SQL нормальную бд
    {
        public int? id { get; set; }
        public string MatchPlace { get; set; }
        public string MatchType { get; set; }
        public string TeamName { get; set; }
        public string TeamName1 { get; set; }
        public string TeamTown { get; set; }
        public string TeamTown1 {get; set; }
        public string MatchDate { get; set; }
        public bool isSevenDayNotify {  get; set; }
        public bool isOneDayNotify { get; set; }
    }
}
