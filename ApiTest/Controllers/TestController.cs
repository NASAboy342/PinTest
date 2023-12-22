using ApiTest.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Reflection;

namespace ApiTest.Controllers
{
    [ApiController]
    [Route("Test")]
    public class TestController : ControllerBase
    {
        private  TestService _testService = new TestService();

        public TestController()
        {

        }

        [HttpGet]
        [Route("linqJoinTest")]
        public object TestJoinLinq()
        {
            var students = new List<Student>
            {
                new Student {Id = 1, FirstName = "Jack", LastName = "Sparrow"},
                new Student {Id = 2, FirstName = "Atwin", LastName = "Hobble"},
                new Student {Id = 3, FirstName = "Towny", LastName = "Stack"},
                new Student {Id = 4, FirstName = "Brus", LastName = "Wain"}
            };

            var studentPerformences = new List<StudentPerformence>
            {
                new StudentPerformence{StudentId = 1,Subject = "C#", Score = 80},
                new StudentPerformence{StudentId = 1,Subject = "Java", Score = 50},
                new StudentPerformence{StudentId = 2,Subject = "C#", Score = 40},
                new StudentPerformence{StudentId = 3,Subject = "C#", Score = 70},
                new StudentPerformence{StudentId = 2,Subject = "Java", Score = 80},
                new StudentPerformence{StudentId = 4,Subject = "C#", Score = 60},
                new StudentPerformence{StudentId = 3,Subject = "Java", Score = 80},
                new StudentPerformence{StudentId = 2,Subject = "HTML", Score = 50},
                new StudentPerformence{StudentId = 4,Subject = "Java", Score = 80},
                new StudentPerformence{StudentId = 5,Subject = "C#", Score = 50}
            };


            var result = from performence in studentPerformences
                         join student in students
                         on performence.StudentId equals student.Id
                         select new { FirstName = student.FirstName ??= "", LastName = student.LastName ??= "", performence.Subject,performence.Score};

            return result;
        }

        [HttpGet]
        [Route("test-tryget-error")]
        public string TestTryGetError()
        {
            string T = null;
            var Type = SportTypeDictionary.SportType.TryGetValue(T ??= "", out var type) ? type : T;
            return Type;
        }

        [HttpGet]
        [Route("test-ordering-with-ignore")]
        public object TestOrderingWithIgnore()
        {
            var companyGame = new List<CompanyGame>
            {
                new CompanyGame{DisplayOrder = 2,GameName = "Bacara", GameProviderId = 5, GameProviderName = "Prata", GameProviderType = EnumGameProviderType.Games, GameProviderTypeDisplayOrder = 2},
                new CompanyGame{DisplayOrder = 2,GameName = "Boto", GameProviderId = 5, GameProviderName = "Prata", GameProviderType = EnumGameProviderType.Games, GameProviderTypeDisplayOrder = 2},
                new CompanyGame{DisplayOrder = 2,GameName = "Backor", GameProviderId = 5, GameProviderName = "Prata", GameProviderType = EnumGameProviderType.Sports, GameProviderTypeDisplayOrder = 2},
                new CompanyGame{DisplayOrder = 2,GameName = "Loopa", GameProviderId = 5, GameProviderName = "Prata", GameProviderType = EnumGameProviderType.Casino, GameProviderTypeDisplayOrder = 2},
                new CompanyGame{DisplayOrder = 2,GameName = "Meeta", GameProviderId = 5, GameProviderName = "Prata", GameProviderType = EnumGameProviderType.Casino, GameProviderTypeDisplayOrder = 2},
                new CompanyGame{DisplayOrder = 2,GameName = "Yogert", GameProviderId = 5, GameProviderName = "Prata", GameProviderType = EnumGameProviderType.Casino, GameProviderTypeDisplayOrder = 2},
                new CompanyGame{DisplayOrder = 2,GameName = "Meetball", GameProviderId = 5, GameProviderName = "Prata", GameProviderType = EnumGameProviderType.Sports, GameProviderTypeDisplayOrder = 2},
                new CompanyGame{DisplayOrder = 2,GameName = "Lapricon", GameProviderId = 5, GameProviderName = "Prata", GameProviderType = EnumGameProviderType.Sports, GameProviderTypeDisplayOrder = 2},
                new CompanyGame{DisplayOrder = 2,GameName = "nooar", GameProviderId = 5, GameProviderName = "Prata", GameProviderType = EnumGameProviderType.VirtualSports, GameProviderTypeDisplayOrder = 2},
                new CompanyGame{DisplayOrder = 2,GameName = "Vaksial", GameProviderId = 5, GameProviderName = "Prata", GameProviderType = EnumGameProviderType.VirtualSports, GameProviderTypeDisplayOrder = 2}
            };

            var games = companyGame.OrderBy(g => g.GameProviderTypeDisplayOrder)
                .ThenBy(g => g.SboBetCategoryDisplayOrder)
                .ThenBy(g => g.GameProviderId == 1029 ? 1 : 2)
                .ThenBy(g => g.DisplayOrder).ToList();

            games.ForEach((g) =>
            {
                if (g.GameProviderId == 5)
                {
                    switch (g.Category)
                    {
                        case GameCategory.SPORTS:
                        case GameCategory.Sbo568Sport:
                            g.GameProviderType = EnumGameProviderType.Sports;
                            break;
                        case GameCategory.LIVE_CASINO:
                            g.GameProviderType = EnumGameProviderType.Casino;
                            break;
                        case GameCategory.VIRTUAL_SPORTS:
                            g.GameProviderType = EnumGameProviderType.VirtualSports;
                            break;
                        default:
                            g.GameProviderType = EnumGameProviderType.Games;
                            break;
                    }
                }
            });

            return games;
        }

        [HttpPost]
        [Route("test-join-linq")]
        public List<Object1> TestJointLinq()
        {
            var object1 = new List<Object1>
            {
                new Object1{CustomerId = 1, Name = "Boyo", Description = "desc1"},
                new Object1{CustomerId = 2, Name = "Bany", Description = "desc2"},
                new Object1{CustomerId = 3, Name = "Boom", Description = "desc3"},
                new Object1{CustomerId = 4, Name = "Moya", Description = "desc4"},
                new Object1{CustomerId = 5, Name = "Lepe", Description = "desc5"}
            };

            var object2 = new List<Player>
            {
                new Player{CustmerId = 1, TagId = 2},
                new Player{CustmerId = 3, TagId = 6},
                new Player{CustmerId = 4, TagId = 5},
                new Player{CustmerId = 5, TagId = 3}
            };

            var query = from o1 in object1
                        join o2 in object2
                        on o1.CustomerId equals o2.CustmerId
                        into temp
                        from t in temp.DefaultIfEmpty()
                        select new { o1, t };

            // Update the TagId of object1 with the value from object2 or null
            foreach (var item in query)
            {
                item.o1.TagId = item.t?.TagId ?? 0;
            }

            return object1;


        }

        [HttpPost]
        [Route("test-empty-property")]
        public object TestEmptyProperty()
        {
            var objects = new TestEmptyPropertyResponse
            {
                CustomerId = 1,
                Description = "jfdlksa;",
                Name = "Name",
                TagId = null
            };

            return objects;
        }

        [HttpPost]
        [Route("test-ex")]
        public string TestEx()
        {
            var ex = ExceptionF();
            return "jfkdls;ajkl";
        }

        private string ExceptionF()
        {
            throw new Exception();
        }

        [HttpPost]
        [Route("test-get-player-tags")]
        public object TestGetPlayerTags()
        {
            var playerTagSettings = new List<PLayerTagSettng>
            {
                new PLayerTagSettng{Id = 3, Name ="Arbitrage",Color = "red", Priority = 1},
                new PLayerTagSettng{Id = 5, Name ="Risky",Color = "Yellow", Priority = 2},
                new PLayerTagSettng{Id = 10, Name ="Bonus Hunter",Color = "Blue", Priority = 4},
                new PLayerTagSettng{Id = 12, Name ="Pro Player",Color = "Green", Priority = 5},
                new PLayerTagSettng{Id = 13, Name ="Old Player",Color = "Brown", Priority = 7},
            };

            var targetPlayer = new List<Player>
            {
                new Player{CustmerId = 200, STagId  = "3,5" },
                new Player{CustmerId = 262, STagId  = "10,5" },
                new Player{CustmerId = 300, STagId  = "10,5" },
                new Player{CustmerId = 196, STagId  = "12,13" },
                new Player{CustmerId = 50, STagId   = "13" },
                new Player{CustmerId = 306, STagId  = "3" },
                new Player{CustmerId = 572, STagId  = "5" },
                new Player{CustmerId = 48, STagId   = "13" }
            };

            var result = new List<PlayerTagInfo>();

            foreach (var player in targetPlayer)
            {
                foreach (var pLayerTagSettng in playerTagSettings)
                {
                    result.Add(new PlayerTagInfo
                    {
                        CustomerId = player.CustmerId,
                        Id = pLayerTagSettng.Id,
                        Color = pLayerTagSettng.Color,
                        Name = pLayerTagSettng.Name,
                        Priority = pLayerTagSettng.Priority,
                        IsTagged = player.STagId.Split(',').Any(t => t == pLayerTagSettng.Id.ToString())
                    });
                }
            }

            return result;
        }

        [HttpPost]
        [Route("test-int.parse")]
        public object TestIntParse()
        {
            var GameProviders = "[45,46,71,76,1]";
            List<int> GameProviderList = GameProviders.Trim('[', ']').Split(',').Select((gameProvider) => Convert.ToInt32(gameProvider)).ToList();

            return GameProviderList;
        }

        [HttpPost]
        [Route("test-direct-object-from-dataBase")]
        public GetInsertScriptResponse GetInsertScript(GetInsertScriptRequest req)
        {
            if (!req.IsValidRequest(out var invalid))
                return new GetInsertScriptResponse
                {
                    ErrorMessage = invalid
                };

            var script = GetSelectScript(req);
            var sqlOutput = BaseRepositository.Insten().QuerySql<string>(script).ToList();
            var insertScript = ToInsertScript(req, sqlOutput);
            return insertScript;
        }

        [HttpPost]
        [Route("test-stpit-refno")]
        public bool TestStpitRefno()
        {
            var RefNo = "568winGames_2_8780791_2_1701830313080345018";
            var splitRefNo = RefNo.Split('_').ToList();
            return splitRefNo.Count == 5 && splitRefNo[3] == "1";
        }

        [HttpPost]
        [Route("is-chrismas")]
        public string IsChrismas()
        {
            return _testService.IsChrismas();
        }

        private GetInsertScriptResponse ToInsertScript(GetInsertScriptRequest req, List<string> sqlOutput)
        {
            return new GetInsertScriptResponse
            {
                InsertInto = InsertInto(req.Table) + ColumnsToInsert(req.Columns),
                Values = ValuesToInsert(req, sqlOutput)

            };
        }

        private List<string> ValuesToInsert(GetInsertScriptRequest req, List<string> sqlOutput)
        {
            var values = new List<string>();
            values.Add("VALUES ");

            foreach (var row in sqlOutput)
            {
                var columns = row.Split('^');
                var rowToInsert = string.Empty;
                for (int i = 0; i < columns.Length; i++)
                {
                    var dataType = req.Columns[i].DataType.Trim();
                    var column = columns[i].Trim();
                    switch (dataType.ToLower())
                    {
                        case "char":
                        case "varchar":
                        case "nchar":
                        case "nvarchar":
                        case "text":
                        case "ntext":
                        case "date":
                        case "time":
                        case "datetime":
                        case "datetime2":
                        case "smalldatetime":
                        case "datetimeoffset":
                            rowToInsert += string.IsNullOrEmpty(column) ? $"''," : $"'{column}', ";
                            break;
                        default:
                            rowToInsert += string.IsNullOrEmpty(column) ? $"0," : $"{column}, ";
                            break;
                    }
                }
                rowToInsert = rowToInsert.TrimEnd(',', ' ');
                values.Add($"({rowToInsert}), ");
            }

            return values;
        }

        private string ColumnsToInsert(List<Column> columns)
        {
            var targetColumns = "(";
            foreach (var column in columns)
            {
                targetColumns += $" [{column.Name}],";
            }
            targetColumns = targetColumns.Remove(targetColumns.Length - 1, 1) + ")";
            return targetColumns;
        }

        private string InsertInto(string table)
        {
            return $"INSERT INTO {table}";
        }

        private string GetSelectScript(GetInsertScriptRequest req)
        {
            var script = "SELECT";
            var targetColumn = string.Empty;
            var isFirstColumn = true;

            if (req.Rows > 0)
                script += $" TOP {req.Rows}";
            foreach (var column in req.Columns)
            {
                targetColumn += (AppendColumn(column.Name,isFirstColumn));
                if (isFirstColumn)
                    isFirstColumn = false;
            }
            script += $"{targetColumn} AS SqlInsertStatment FROM {req.Table}";
            
            return script;
        }

        private string AppendColumn(string column, bool isFirstColumn)
        {

            return isFirstColumn? $" CAST(ISNULL([{column}],'') AS NVARCHAR(MAX))" : $" + '^' +CAST(ISNULL([{column}],'') AS NVARCHAR(MAX))";
        }
    }
    public class GetInsertScriptResponse
    {
        public string InsertInto { get; set; }
        public List<string> Values { get; set; }
        public string ErrorMessage { get; set; } = "Success";
    }

    public class GetInsertScriptRequest
    {
        [JsonProperty("columns")]
        public List<Column> Columns { get; set; }
        public string Table { get; set; }
        public int Rows { get; set; } = 0;
        public bool IsValidRequest(out string invalid)
        {
            if (Columns == null || Columns.Count() == 0 || Columns.Any(c => c.Name == "*" || c.Name == "" || c.DataType == ""))
            {
                invalid = "Invalid Columns";
                return false;
            }
            if (Table == null || Table == "")
            {
                invalid = "Invalid Table";
                return false;
            }
            if (Columns.Any(c => c.DataType.Contains('(') || c.DataType.Contains(')') || c.DataType.Contains('[') || c.DataType.Contains(']')))
            {
                invalid = "Invalid DataType";
                return false;
            }
            if (Table.Split('.').Length != 3)
            {
                invalid = "Invalid Table";
                return false;
            }
            if (Rows < 0)
            {
                invalid = "Invalid RowNumber";
                return false;
            }
            invalid = "";
            return true;
        }
    }

    public class Column
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("dataType")]
        public string DataType { get; set; }
    }

    public enum EnumDataType
    {
        INT,
        NVARCHAR,
        VARCHAR,
        CHAR,
        DATETIME,
        BIT,
        DECIMAL
    }

    public class PLayerTagSettng
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int Priority { get; set; }
    }
    public class TestEmptyPropertyResponse
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TagId { get; set; } = string.Empty;
    }
    public class Object1
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TagId { get; set; }
    }
    public class PlayerTagInfo
    {
        public int CustomerId { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
        public int Priority { get; set; }
        public bool IsTagged { get; set; }
    }
    public class Player
    {
        public int CustmerId { get; set; }
        public int TagId { get; set; }
        public string STagId { get; set; }
    }

    public class SportTypeDictionary
    {
        public static Dictionary<string, string> SportType => new Dictionary<string, string>()
        {
            {"1","Soccer"},
            {"2","BasketBall"},
            {"3","Football"},
            {"5","Tennis"},
            {"8","Baseball"},
            {"10","Golf"},
            {"11","Motorsports"},
            {"43","E-Sports"},
            {"99","Other Sports"},
            {"180","Virtual Sports"},
            {"190","Virtual Sports"},
            {"9901","Saba soccer"},
            {"9902","Saba basketball"},
            {"1MP","Soccer Mix Parlay"},
            {"99MP","Mix Parlay"}
        };
    }

    public class CompanyGame : Game
    {
        public EnumGameProviderType GameProviderType { get; set; }
        public string GameProviderTypeName => GameProviderType.ToString();
        public int SboBetCategoryDisplayOrder => GameProviderId == 5 ? SboBetCategoryDisplayOrde.DisplayOrder[GameProviderType] : 8;
    }

    public class SboBetCategoryDisplayOrde
    {
        public static Dictionary<EnumGameProviderType, int> DisplayOrder = new Dictionary<EnumGameProviderType, int>
        {
            {EnumGameProviderType.Sports, 1},
            {EnumGameProviderType.VirtualSports, 2},
            {EnumGameProviderType.Casino, 3},
            {EnumGameProviderType.Games, 4},
            {EnumGameProviderType.Poker, 5},
            {EnumGameProviderType.Lottery, 6},
            {EnumGameProviderType.CockFighting, 7}
        };
    }

    public enum EnumGameProviderType
    {
        Sports = 1,
        Casino = 2,
        Games = 3,
        VirtualSports = 4,
        Lottery = 5,
        CockFighting = 6,
        Poker = 7
    }
    public enum GameCategory
    {
        REFERRAL_BONUS = -3,
        REGISTER_BONUS = -2,
        DEPOSIT_BONUS = -1,
        ALL = 0,

        /// <summary>
        /// Include Third Party
        /// </summary>
        SPORTS = 1,

        /// <summary>
        /// Include Third Party
        /// </summary>
        LIVE_CASINO = 2,

        /// <summary>
        /// Include Third Party
        /// </summary>
        GAMES = 3,

        /// <summary>
        /// Include Third Party
        /// </summary>
        VIRTUAL_SPORTS = 4,

        /// <summary>
        /// Only use when call sw (not store in db)
        /// </summary>
        SEAMLESS_GAMEPROVIDER = 5,

        /// <summary>
        /// Only use when call sw (not store in db)
        /// </summary>
        THIRDPARTY_SPORTS = 6,

        LIVE_COIN = 7,
        TRANSFER_GAMEPROVIDER_TRANSFER = 8,
        Sbo568Sport = 13,
        P2PSport = 14,
        KYSport = 15
    }

    public enum GameType
    {
        REGISTER_BONUS = -2,
        DEPOSIT_BONUS = -1,
        Unknown = 0,

        //Casino
        CasinoLobby = 100,
        Baccarat = 101,
        Blackjack = 102,
        Roulette = 103,
        DragonTiger = 104,
        Sicbo = 105,
        BullBull = 106,
        Poker = 107,
        Dice = 108,
        GameShow = 109,

        //Games
        GamesLobby = 200,
        Slots = 201,
        ArcadeGames = 202,
        FishingGames = 203,
        TableGames = 204,
        Scratchcards = 205,
        VirtualGames = 206,
        LotteryGames = 207,
        OthersGames = 208,

        //Sports
        SportsBook = 300
    }

    public class Game
    {
        public string GameProviderName { get; set; }
        public int GameProviderId { get; set; }
        public int GameProviderTypeDisplayOrder { get; set; }
        public string GameName { get; set; }
        public int DisplayOrder { get; set; }
        public GameCategory Category { get; set; }
    }

    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set;}
        public string LastName { get; set;}
    }

    public class StudentPerformence
    {
        public int StudentId { get; set; }
        public string Subject { get; set; }
        public int Score { get; set; }
    }
}
