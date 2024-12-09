namespace TOZTestApp.SQLite
{
    public static class ManagerDB
    {
        private static ApplicationContext db = new ApplicationContext();
        private const int magicValue = 8; //Обрезаем на него, для того что бы получить дату в виде строки без времени
        public static void UpdateDB(List<MatchCalendarItem> tozMatches)
        {
            var dbList = db.MatchCalendarItemSQLite.ToList();
            foreach (var match  in tozMatches)
            {
                var matchSQl = match.ToSQLite();
                foreach (var item in dbList) 
                {
                    if (matchSQl.MatchDate.Substring(0, matchSQl.MatchDate.ToString().Length - magicValue) == item.MatchDate.Substring(0, item.MatchDate.ToString().Length - magicValue))
                    {
                        item.MatchType = matchSQl.MatchType;
                        item.MatchPlace = matchSQl.MatchPlace;
                        item.TeamName = matchSQl.TeamName;
                        item.TeamName1 = matchSQl.TeamName1;
                        item.TeamTown = matchSQl.TeamTown;
                        item.TeamTown1 = matchSQl.TeamTown1;
                        item.MatchDate = matchSQl.MatchDate.ToString();
                        //К сожалению я не нашел более нормального способа работать с EntitySQLite
                        db.MatchCalendarItemSQLite.Update(item);
                        db.SaveChanges();
                        goto SkipIfUpdate;
                    }
                }
                db.MatchCalendarItemSQLite.Add(matchSQl);
                db.SaveChanges();
                SkipIfUpdate:;
                CheckNearestMatches();
            }
        }
        public static void MatchIsEnd(MatchCalendarItem lastMatch)
        {
            var dbList = db.MatchCalendarItemSQLite.ToList();
            MatchCalendarItemSQLite lastMatchSQl = lastMatch.ToSQLite();
            foreach (var item in dbList)
            {
                if (lastMatchSQl.MatchDate == item.MatchDate)
                {
                    db.MatchCalendarItemSQLite.Remove(item);
                    db.SaveChanges();
                    Program.SendAlert(2, lastMatchSQl, lastMatch);
                }
            }
        }
        private static void CheckNearestMatches()
        {
            var dbList = db.MatchCalendarItemSQLite.ToList();
            foreach (var item in dbList)
            {
                string[] dates = item.MatchDate.Substring(0, item.MatchDate.Length - magicValue).Split(".");
                DateTime itemMatchDate = new DateTime(int.Parse(dates[(int)arrayDateIndexes.year]), int.Parse(dates[(int)arrayDateIndexes.month]), int.Parse(dates[(int)arrayDateIndexes.day]));
                if (itemMatchDate.Subtract(DateTime.Today).Days <= 1 && !item.isOneDayNotify)
                {
                    
                    item.isOneDayNotify = true;
                    db.MatchCalendarItemSQLite.Update(item);
                    db.SaveChanges();
                    Program.SendAlert(1, item);
                }
                else if(itemMatchDate.Subtract(DateTime.Today).Days <= 7 && !item.isSevenDayNotify)
                {
                    item.isSevenDayNotify = true;
                    db.MatchCalendarItemSQLite.Update(item);
                    db.SaveChanges();
                    Program.SendAlert(0, item);
                }
            }           
        }
    }
}
