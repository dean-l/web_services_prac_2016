using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using prac4.Models;

namespace prac4.Controllers
{
    public class PlayerController : ApiController
    {

        List<Player> players = new List<Player>();

        public PlayerController()
        {
            string[] playerList = File.ReadAllLines(@"C:\Users\Dean\Desktop\INFS3204\prac4\prac4\players.txt");
            foreach (string playerLine in playerList)
            {
                int playerYear;
                int playerMonth;
                int playerDay;
                Player newPlayer = new Player();
                string[] playerLineSplit = playerLine.Split(',');

                newPlayer.Registration_ID = playerLineSplit[0];
                newPlayer.Player_name = playerLineSplit[1];
                newPlayer.Team_name = playerLineSplit[2];
                string[] playerDob = playerLineSplit[3].Split('-');
                Int32.TryParse(playerDob[0], out playerYear);
                Int32.TryParse(playerDob[1], out playerMonth);
                Int32.TryParse(playerDob[2], out playerDay);
                newPlayer.Date_of_birth = new DateTime(playerYear, playerMonth, playerDay);
                players.Add(newPlayer);
            }
        }

        public IHttpActionResult GetAllPlayers()
        {
            
            return Ok(players);
        }

        public IHttpActionResult GetPlayer(string id)
        {
            var player = players.FirstOrDefault((p) => p.Registration_ID == id);
            if (player == null)
            {
                return NotFound();
            }
            return Ok(player);
        }
        public IHttpActionResult PostPlayer(Player newPlayer)
        {
            var player = players.FirstOrDefault((p) => p.Registration_ID == newPlayer.Registration_ID);
            if(newPlayer.Registration_ID == null || newPlayer.Player_name == null || newPlayer.Team_name == null || newPlayer.Date_of_birth == null)
            {
                return BadRequest("Invalid Input");
            }
            if(player != null)
            {
                players.Remove(player);
                players.Add(newPlayer);
            }
            else
            {
                players.Add(newPlayer);
            }
            return writeToFile();
        }
        public IHttpActionResult DeletePlayer(string id)
        {
            var player = players.FirstOrDefault((p) => p.Registration_ID == id);
            if (player == null)
            {
                return NotFound();
            }
            players.Remove(player);
            return writeToFile();
        }
        
        private IHttpActionResult writeToFile()
        {
            using (StreamWriter writer = new StreamWriter(@"C:\Users\Dean\Desktop\INFS3204\prac4\prac4\players.txt"))
            {
                foreach (Player player in players)
                {
                    string line = player.Registration_ID + "," + player.Player_name + "," + player.Team_name + "," + player.Date_of_birth.Year + '-'
                        + player.Date_of_birth.Month + '-' + player.Date_of_birth.Day;
                    writer.WriteLine(line);
                }
            }
            return Ok(players);
        }
        
    }
}
