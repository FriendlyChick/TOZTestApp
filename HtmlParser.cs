using HtmlAgilityPack;
using TOZTestApp.SQLite;
using TOZTestApp.Selenium;
namespace TOZTestApp
{
    internal class HtmlParser
    {
        private HtmlDocument _htmlDock = new HtmlDocument();
        private readonly string[] _tozTeams = new string[] {"Оружейник","Оружейник - ЛН"};
        private Dictionary<string, int> Month = new Dictionary<string, int>()
        {

            {"января" , 1 },
            {"февраля" , 2 },
            {"марта" , 3 },
            {"апреля" , 4 },
            {"мая" , 5 },
            {"июня" , 6 },
            {"июля" , 7 },
            {"августа" , 8 },
            {"сентября" , 9 },
            {"октября" , 10 },
            {"ноября" , 11 },
            {"декабря" , 12 },
        };
        public void CheckMatches(string htmlLink)
        {
            SeleniumManager seleniumManager = new SeleniumManager();
            _htmlDock.LoadHtml(seleniumManager.GetHtmlPage(htmlLink));
            CheckMatches();
        }
        public void CheckResults(string htmlLink)
        {
            SeleniumManager seleniumManager = new SeleniumManager();
            _htmlDock.LoadHtml(seleniumManager.GetHtmlPage(htmlLink));
            CheckResults();
        }
        private void CheckMatches()
        {
                List<MatchCalendarItem> matchCalendarItems = new List<MatchCalendarItem>();
                foreach (HtmlNode div in _htmlDock.DocumentNode.SelectNodes("//div[contains(@class, 'match-calendar__item')]"))
                {
                    MatchCalendarItem item = new MatchCalendarItem();
                    try
                    {
                        //Информация о первой команде
                        var node = div.SelectSingleNode(".//div[contains(@class, 'match-calendar__team order-3 order-md-2')]//div[contains(@class, 'match-calendar__team-name')]//a");
                        item.TeamName = node.Attributes["title"].Value;
                        node = div.SelectSingleNode(".//div[contains(@class, 'match-calendar__team match-calendar__team_right order-4 order-md-6')]//div[contains(@class, 'match-calendar__team-name')]//a");
                        item.TeamName1 = node.Attributes["title"].Value;
                        if (!_tozTeams.Contains(item.TeamName) && !_tozTeams.Contains(item.TeamName1))
                        {
                            continue;
                        }
                        node = div.SelectSingleNode(".//div[contains(@class, 'match-calendar__team order-3 order-md-2')]//div[contains(@class, 'match-calendar__team-city')]");
                        item.TeamTown = node.InnerText;
                        //Информация о второй команде
                        node = div.SelectSingleNode(".//div[contains(@class, 'match-calendar__team match-calendar__team_right order-4 order-md-6')]//div[contains(@class, 'match-calendar__team-city')]");
                        item.TeamTown1 = node.InnerText;
                        //Информация о типе матча
                        string matchInfo;
                        node = div.SelectSingleNode(".//div[contains(@class, 'match-calendar__center order-5 order-md-4')]//div[contains(@class, 'match-calendar__info')]//span[contains(@class, 'match-calendar__info-part')]//a");
                        matchInfo = node.InnerText + " ";
                        if (node.NextSibling != null)
                        matchInfo += node.NextSibling.InnerText;
                        var i = div.SelectNodes(".//div[contains(@class, 'match-calendar__center order-5 order-md-4')]//div[contains(@class, 'match-calendar__info')]//span[contains(@class, 'match-calendar__info-part')]");
                        matchInfo += i[1].InnerText + " ";
                        matchInfo += i[2].InnerText + " ";
                        matchInfo += i[3].InnerText;
                        matchInfo = matchInfo.Replace("&nbsp;", " ");
                        item.MatchType = matchInfo;
                        //Получаем данные времени матча
                        i = div.SelectNodes(".//div[contains(@class, 'match-calendar__center order-5 order-md-4')]//div[contains(@class, 'match-calendar__time')]");
                        string matchDateandTime = i[1].InnerText;
                        matchDateandTime = matchDateandTime.Replace(",", "");
                        matchDateandTime = matchDateandTime.Replace(".", "");
                        string[] date = matchDateandTime.Split(" ");
                        string[] time = date[4].Split(":");
                        if (int.TryParse(time[0], out int result))
                        {
                            item.MatchDate = new DateTime(int.Parse(date[(int)arrayDateIndexes.year]), Month[date[(int)arrayDateIndexes.month]], int.Parse(date[(int)arrayDateIndexes.day]), int.Parse(time[(int)arrayTimeIndexes.hours]), int.Parse(time[(int)arrayTimeIndexes.minutes]), 0);
                        }
                        else
                        {
                            item.MatchDate = new DateTime(int.Parse(date[(int)arrayDateIndexes.year]), Month[date[(int)arrayDateIndexes.month]], int.Parse(date[(int)arrayDateIndexes.day]));
                        }
                        node = div.SelectSingleNode(".//div[contains(@class, 'match-calendar__center order-5 order-md-4')]//div[contains(@class, 'match-calendar__place')]");
                        item.MatchPlace = node.InnerText;
                        matchCalendarItems.Add(item);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                FilterMatches(matchCalendarItems);      
        }
        private void CheckResults()
        {
            MatchCalendarItem LastMatchCalendarItem = new MatchCalendarItem();
            foreach (HtmlNode div in _htmlDock.DocumentNode.SelectNodes("//div[contains(@class, 'match-calendar__item')]"))
            {
                MatchCalendarItem item = new MatchCalendarItem();
                try
                {
                    //Информация о первой команде
                    var node = div.SelectSingleNode(".//div[contains(@class, 'match-calendar__team order-3 order-md-2')]//div[contains(@class, 'match-calendar__team-name')]//a");
                    item.TeamName = node.Attributes["title"].Value;
                    node = div.SelectSingleNode(".//div[contains(@class, 'match-calendar__team match-calendar__team_right order-4 order-md-6')]//div[contains(@class, 'match-calendar__team-name')]//a");
                    item.TeamName1 = node.Attributes["title"].Value;
                    if (!_tozTeams.Contains(item.TeamName) && !_tozTeams.Contains(item.TeamName1))
                    {
                        continue;
                    }
                    node = div.SelectSingleNode(".//div[contains(@class, 'match-calendar__team order-3 order-md-2')]//div[contains(@class, 'match-calendar__team-city')]");
                    item.TeamTown = node.InnerText;
                    //Информация о второй команде
                    node = div.SelectSingleNode(".//div[contains(@class, 'match-calendar__team match-calendar__team_right order-4 order-md-6')]//div[contains(@class, 'match-calendar__team-city')]");
                    item.TeamTown1 = node.InnerText;
                    //Информация о типе матча
                    string matchInfo;
                    node = div.SelectSingleNode(".//div[contains(@class, 'match-calendar__center order-5 order-md-4')]//div[contains(@class, 'match-calendar__info')]//span[contains(@class, 'match-calendar__info-part')]//a");
                    matchInfo = node.InnerText + " ";
                    if (node.NextSibling != null)
                        matchInfo += node.NextSibling.InnerText;
                    var i = div.SelectNodes(".//div[contains(@class, 'match-calendar__center order-5 order-md-4')]//div[contains(@class, 'match-calendar__info')]//span[contains(@class, 'match-calendar__info-part')]");
                    matchInfo += i[1].InnerText + " ";
                    matchInfo += i[2].InnerText + " ";
                    matchInfo += i[3].InnerText;
                    matchInfo = matchInfo.Replace("&nbsp;", " ");
                    item.MatchType = matchInfo;
                    //Получаем данные времени матча
                    i = div.SelectNodes(".//div[contains(@class, 'match-calendar__center order-5 order-md-4')]//div[contains(@class, 'match-calendar__time')]");
                    string matchDateandTime = i[1].InnerText;
                    matchDateandTime = matchDateandTime.Replace(",", "");
                    matchDateandTime = matchDateandTime.Replace(".", "");
                    string[] date = matchDateandTime.Split(" ");
                    string[] time = date[4].Split(":");
                    if (int.TryParse(time[0], out int result))
                    {
                        item.MatchDate = new DateTime(int.Parse(date[(int)arrayDateIndexes.year]), Month[date[(int)arrayDateIndexes.month]], int.Parse(date[(int)arrayDateIndexes.day]), int.Parse(time[(int)arrayTimeIndexes.hours]), int.Parse(time[(int)arrayTimeIndexes.minutes]), 0);
                    }
                    else
                    {
                        item.MatchDate = new DateTime(int.Parse(date[(int)arrayDateIndexes.year]), Month[date[(int)arrayDateIndexes.month]], int.Parse(date[(int)arrayDateIndexes.day]));
                    }
                    node = div.SelectSingleNode(".//div[contains(@class, 'match-calendar__center order-5 order-md-4')]//div[contains(@class, 'match-calendar__place')]");
                    item.MatchPlace = node.InnerText;
                    //Тут берём результаты игры
                    node = div.SelectSingleNode(".//div[contains(@class, 'match-calendar__score d-none d-md-block order-md-3 match-calendar__score_wr')]//a");
                    item.TeamScore = int.Parse(node.InnerText);
                    node = div.SelectSingleNode(".//div[contains(@class,'match-calendar__score d-none d-md-block order-md-5 ')]//a");
                    item.TeamScore1 = int.Parse(node.InnerText);
                    item.HrefToResult = "https://tula.nhliga.org/" + node.Attributes["href"].Value;
                    Console.WriteLine(item.TeamScore);
                    Console.WriteLine(item.TeamScore1);
                    Console.WriteLine(item.HrefToResult);

                    LastMatchCalendarItem = item;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            ManagerDB.MatchIsEnd(LastMatchCalendarItem);
            Console.WriteLine(LastMatchCalendarItem.MatchDate);
        }
        private void FilterMatches(List<MatchCalendarItem> allMatches)
        {
            ManagerDB.UpdateDB(allMatches);
        }
    }
}
