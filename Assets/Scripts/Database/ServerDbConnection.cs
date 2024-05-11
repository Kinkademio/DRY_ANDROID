using Duck.Http;
using UnityEngine;

public class ServerDbConnection
{
    /**
     * TODO тут нужно описывать запросы к апи согласно информации на серверной части 
     * ИНСТРУКЦИЯ ПО составлению запросов тут https://github.com/dubit/unity-http
     * ПРИХОДИТ СТРОКА JSON Нужно десериализовать её как в утке через класс 
     * JsonUtility.FromJson<User>(response.Text);
     */
    public void GetTestData(string slug)
    {
        var request = Http.Get("http://localhost:3000/media")
            .OnSuccess(response => Debug.Log(response.Text))
            .OnError(response => Debug.Log(response.StatusCode))
            .Send();
    }
}
