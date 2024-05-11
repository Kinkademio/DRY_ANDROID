public static class DBConnecor
{
    /**
     * Соединение с локальной базой
     */
    public static SQLiteDbConnection LocalBaseConnection()
    {
        return new SQLiteDbConnection();
    }

    /**
     * Соединение с удаленной базой
     */
    public static ServerDbConnection ServerDbConnection()
    {
        return new ServerDbConnection();
    }

}
