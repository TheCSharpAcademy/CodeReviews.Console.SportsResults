using System.Text;

namespace HtmlParser;

public class Match_
{
    public Team Team1;  
    public Team Team2;  

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append(string.Format("{0,15} vs {1,-10}\n", Team1.Name, Team2.Name));
        sb.Append(string.Format("{0,15} <> {1,-10}\n", Team1.Points, Team2.Points) );


        return sb.ToString();
    }
}
