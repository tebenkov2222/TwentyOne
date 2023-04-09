using Repository;
using RussianMunchkin.Server.DataBase.Command;

namespace RussianMunchkin.Server.DataBase
{
    public class DataBaseController
    {
        public Select Select;
        public Insert Insert;
        public Update Update;

        public DataBaseController(PostreSQL dataBase)
        {
            Select = new Select(dataBase);
            Insert = new Insert(dataBase);
            Update = new Update(dataBase);
        }
    }
}