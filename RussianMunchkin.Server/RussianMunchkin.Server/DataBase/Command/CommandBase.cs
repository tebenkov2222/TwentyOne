using Repository;

namespace RussianMunchkin.Server.DataBase.Command
{
    public abstract class CommandBase
    {
        protected readonly PostreSQL PostreSql;
        protected CommandBase(PostreSQL postreSql) => PostreSql = postreSql;
    }
}