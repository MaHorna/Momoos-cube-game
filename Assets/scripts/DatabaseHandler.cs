using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;

public class DatabaseHandler : MonoBehaviour
{
    public Sprite[] cubeImage;
    public GameObject LBholder;
    int LeaderPage = 0;
    string[] Usernames;
    int[] CubeIndexes;
    string[] Dates;
    int[] Scores;
    public void SendData(int score)
    {
        string connectionString = "SERVER=sql11.freemysqlhosting.net; PORT = 3306;DATABASE=sql11435839;UID=sql11435839;PASSWORD=iGIC8W8yW1;SslMode=none;";
        MySqlConnection mycon = new MySqlConnection(connectionString);
        mycon.Open();
        var sql = "INSERT INTO `Leaderboard` (`ID`, `UserName`, `Cube`, `Score`, `Date`) VALUES (NULL, '" + DataHolder.Instance.Name + "', '" + DataHolder.Instance.CubeIndex + "', '" + score + "', '" + DateTime.Now.ToString("d.M.yyyy") + "');";
        var cmd = new MySqlCommand(sql, mycon);
        var reader = cmd.ExecuteReader();
        mycon.Close();
    }
    string duplicateName;
    int duplicateCubeIndex;
    int duplicateScore;
    string duplicateDate;
    int count;
    public void LBshow()
    {
        Debug.Log(LeaderPage);
        string connectionString = "SERVER=sql11.freemysqlhosting.net; PORT = 3306;DATABASE=sql11435839;UID=sql11435839;PASSWORD=iGIC8W8yW1;SslMode=none;";
        MySqlConnection mycon = new MySqlConnection(connectionString);
        mycon.Open();
        var sql = "SELECT COUNT(*) FROM Leaderboard";
        var cmd = new MySqlCommand(sql, mycon);
        count = Convert.ToInt32(cmd.ExecuteScalar());

        sql = "SELECT * FROM Leaderboard ORDER BY Score DESC LIMIT " + (0 + LeaderPage * 10) + ",10;";
        cmd = new MySqlCommand(sql, mycon);
        var reader = cmd.ExecuteReader();
        Usernames = new string[10];
        CubeIndexes = new int[10];
        Scores = new int[10];
        Dates = new string[10];
        for (int i = 0; i < 10; i++)
        {
            reader.Read();
            Usernames[i] = (string)reader["UserName"];
            CubeIndexes[i] = (int)reader["Cube"];
            Scores[i] = (int)reader["Score"];
            Dates[i] = (string)reader["Date"];
            if (i >= 1)
            {
                if (Usernames[i - 1] == Usernames[i] && CubeIndexes[i - 1] == CubeIndexes[i] && Scores[i - 1] == Scores[i] && Dates[i - 1] == Dates[i])
                {
                    duplicateName = Usernames[i];
                    duplicateCubeIndex = CubeIndexes[i];
                    duplicateScore = Scores[i];
                    duplicateDate = Dates[i];
                }
            }
            if (duplicateName == Usernames[i] && duplicateCubeIndex == CubeIndexes[i] && duplicateScore == Scores[i] && duplicateDate == Dates[i])
            {
                Usernames[i] = "";
                CubeIndexes[i] = -1;
                Scores[i] = -1;
                Dates[i] = "";
            }
        }
        UpdateLeaderBoardUI();
        mycon.Close();
    }
    public void LBnext()
    {
        if (count / 10 > LeaderPage)
        {
            LeaderPage++;
            LBshow();
        }
        
    }
    public void LBprevious()
    {
        if (LeaderPage != 0)
        {
            LeaderPage--;
            LBshow();
        }

    }

    public void UpdateLeaderBoardUI()
    {
        for (int i = 0; i < 10; i++)
        {
            Transform tmp = LBholder.transform.Find("Row" + (i + 1).ToString());
            Transform tmp1 = tmp.transform.Find("N1");
            tmp1.gameObject.GetComponent<Text>().text = Usernames[i];
            tmp1 = tmp.transform.Find("S1");
            if (Scores[i] == -1)
            {
                tmp1.gameObject.GetComponent<Text>().text = "";
            }
            else
            {
                tmp1.gameObject.GetComponent<Text>().text = Scores[i].ToString();
            }
            tmp1 = tmp.transform.Find("D1");
            tmp1.gameObject.GetComponent<Text>().text = Dates[i];
            tmp1 = tmp.transform.Find("R1");
            if (Scores[i] == -1)
            {
                tmp1.gameObject.GetComponent<Text>().text = "";
            }
            else
            {
                tmp1.gameObject.GetComponent<Text>().text = ((LeaderPage * 10) + i + 1).ToString();
            }

            tmp1 = tmp.transform.Find("C1");
            if (CubeIndexes[i] == -1)
            {
                tmp1.gameObject.GetComponent<Image>().sprite = null;
            }
            else
            {
                tmp1.gameObject.GetComponent<Image>().sprite = cubeImage[CubeIndexes[i]];
            }

        }
    }
}
