using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
/// <summary>
/// the class that is used in the beginning of the game to deployed forces 
/// </summary>
class Start_Game    
{
    private List<int> players;
    public int num_of_players;
    public Graph mp;
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="num_of_players">how many players are playing</param>
    /// <param name="graph">the map</param>
    public Start_Game(int num_of_players,Graph graph)
    {
        this.players = new List<int>();
        this.mp = graph;
        this.num_of_players = num_of_players;
        for (int i = 0; i < num_of_players; i++)
        {
            /*
             the size of the forces is determined:
             how many player     how many soldiers
             2                   40
             3                   35
             4                   30
             5                   25
             6                   20
            */
             this.players.Add(50 - (num_of_players * 5));
        }
    }
    /// <summary>
    /// get how many forces the player left to deploy
    /// </summary>
    /// <param name="current_player">the wanted player</param>
    /// <returns></returns>
    public int getNumberOfForces(int current_player)
    {
        return this.players[current_player];
    }
    /// <summary>
    /// deploy one soldier  in the wanted location(if the location is possible and there are soldiers left)
    /// </summary>
    /// <param name="idex_of_cou">which country to deploy</param>
    /// <param name="current_player">the current player</param>
    /// <returns></returns>
    public bool choose_forces(int idex_of_cou,int current_player)
    {
        int beforeDeploy;
        if (this.num_of_players > 0)
        {
            // there are still soldiers to be deployed
            if (this.players[current_player] > 0)
            {
                player_teritories turn_of_player_x = new player_teritories(current_player, this.players[current_player], true, this.mp);
                beforeDeploy = this.players[current_player];
                //change the number of soldiers that the player need to put
                this.players[current_player] = turn_of_player_x.choose_teritories(idex_of_cou, 1);
                //if he deployed all his soldirs
                if (this.players[current_player] == 0)
                {
                    this.num_of_players--;
                }
                //still soldiers
                else
                {
                    //not possible selection so try again
                    if(beforeDeploy== this.players[current_player])
                    {
                        return false;
                    }    
                }
                return true;

            }
      
        }
        return true;
    }
}
/// <summary>
/// the class that is used to deploy forces in the map for a player
/// </summary>
class player_teritories
{
    private int current_player;
    private int number_of_forces;
    private bool flag_start;
    private Graph mp;

    /// <summary>
    /// constructer
    /// </summary>
    /// <param name="current_player">the player</param>
    /// <param name="number_of_forces">how many soldiers to deploy</param>
    /// <param name="flag_start">true or false(true if it is the start stage false if it is the reinforcement)</param>
    /// <param name="graph"></param>
    public player_teritories(int current_player, int number_of_forces, bool flag_start, Graph graph)
    {
        this.mp = graph;
        this.current_player = current_player;
        this.number_of_forces = number_of_forces;
        this.flag_start = flag_start;
    }

    /// <summary>
    /// check if possbile(if yes deploy the forces in the wanted location)
    /// </summary>
    /// <param name="index_of_ter">which teritotiry to deploy</param>
    /// <param name="quantity_of_forces">how many soldiers to deploy</param>
    /// <returns></returns>
    public int choose_teritories(int index_of_ter, int quantity_of_forces)
    {
       
            if (teritotiry_is_ok(index_of_ter))
            {
                this.mp.map[index_of_ter].Info.Ruled_by = (Global.Control)(this.current_player + 1);//change the ruler of the teritory
                this.mp.map[index_of_ter].Info.Num_of_forces += quantity_of_forces;//change the number of forces in the teritory
                this.number_of_forces -= quantity_of_forces;
            }

        return this.number_of_forces;
    }
    /// <summary>
    ///     check if the player can put  forces in the given teritotiry
    /// </summary>
    /// <param name="index_of_ter">the chosen index</param>
    /// <returns>true or false</returns>
    private bool teritotiry_is_ok(int index_of_ter)
    {

        //is owned by the the player
        if ((Global.Control)(this.current_player + 1) == this.mp.map[index_of_ter].Info.Ruled_by)
            return true;
        //if it is start player can take free country
        if (this.flag_start == true)
        {
            if (this.mp.map[index_of_ter].Info.Ruled_by == Global.Control.Free)
                return true;
        }
        return false;
    }

}
/// <summary>
/// the class that is used to do battles
/// </summary>
class Attack
{
    public int current_player;
    public List<int> possible_sou;
    private System.Random rd = new System.Random();
    public Graph mp;
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="current_player">the player</param>
    /// <param name="graph">the map</param>
    public Attack(int current_player, Graph graph)
    {
        this.current_player = current_player;
        this.mp = graph;
    }
    /// <summary>
    /// check if the player has places to attack from
    /// </summary>
    /// <returns>true if possible</returns>
    public bool possibleToAttack()
    {
        if (this.possible_sou.Count > 0)
            return true;
        return false;
    }
    /// <summary>
    /// create list of all the possible locations to attack from
    /// </summary>
    public void possible_sources()
    {
        this.possible_sou = new List<int>();
        for (int i = 0; i < Global.MAX_INDEX_OF_TER; i++)
        {
            if (location_source_possbile(i) == true)
            {
                // addd all possible indexes of source ter
                this.possible_sou.Add(i);
            }
        }


    }
    /// <summary>
    /// check if the teritory is in the possible locations
    /// </summary>
    /// <param name="index_of_sou">the checked teritory</param>
    /// <returns>true if possible</returns>
    public bool in_possible_sources(int index_of_sou)
    {
        //find if the given index is in the possible list 
        foreach (int cou in this.possible_sou)
        {
            if (cou == index_of_sou)
                return true;
        }
        return false;
    }

    /// <summary>
    /// play attack one time(role the dices )
    /// </summary>
    /// <param name="indexOfSuo">index of attaker</param>
    /// <param name="indexOfDes"> index of defender</param>
    /// <returns>
    /// retrun 0- attacker won
    ///return 1- defender won
    /// return 2- check if countue battle
    /// </returns>

    public int attackOnce(int indexOfSuo,int indexOfDes)
    {
        //if you are attacking unrolled teritory
        if (this.mp.map[indexOfDes].Info.Ruled_by == Global.Control.Free)
        {
            //change the ruler
            this.mp.map[indexOfDes].Info.Ruled_by = (Global.Control)(this.current_player + 1);
            //choose how many forces to send and how many to stay
            return 0;
           
        }
        else
        {
           the_battle(indexOfSuo, indexOfDes);
            //lost attack
            if (this.mp.map[indexOfSuo].Info.Num_of_forces <= 1)
            {
                return 1;
            }
            //won battle
            else if (this.mp.map[indexOfDes].Info.Num_of_forces <= 0)
            {
                //change the ruler
                this.mp.map[indexOfDes].Info.Ruled_by = (Global.Control)(this.current_player + 1);
                return 0;

            }
            //not lost and not won
            else
            {
                return 2;
            }
        }
    }
    /// <summary>
    /// update the map after one attack
    /// </summary>
    /// <param name="index_of_sou">index of attaker</param>
    /// <param name="index_of_des">ndex of defender</param>
    private void the_battle(int index_of_sou, int index_of_des)
    {
        //het the rolled dices of the attacker and defender
        List<int> attacker = dice_rolled(index_of_sou);
        List<int> defender = dice_rolled(index_of_des);
        int counter = 0;
        int numOfRounds = Math.Min(attacker.Count, defender.Count);
        // up until 3,attacker most have at least 2 soldier,defender lost when he has 0 soldiers
        while (counter < numOfRounds && counter <= attacker.Count && counter <= defender.Count && this.mp.map[index_of_sou].Info.Num_of_forces > 1 && this.mp.map[index_of_des].Info.Num_of_forces > 0)
         {
          
            //attaker won
            if (attacker[counter] > defender[counter])
            {
                this.mp.map[index_of_des].Info.Num_of_forces--;
            }
            //defender won
            else
            {
                this.mp.map[index_of_sou].Info.Num_of_forces--;
            }
            counter++;
        }
    }
    /// <summary>
    /// check if  teritory has enough soldiers and has emeny near
    /// </summary>
    /// <param name="index_of_ter">the checked teritory</param>
    /// <returns>true if possbile to attack from</returns>
    private bool location_source_possbile(int index_of_ter)
    {
        //owned by the current player
        if ((Global.Control)(this.current_player + 1) == this.mp.map[index_of_ter].Info.Ruled_by)
        {
            //not enough forces to atack
            if (this.mp.map[index_of_ter].Info.Num_of_forces < 2)
            {
                return false;
            }
            //if the neigbors can be attacked
            foreach (State s in this.mp.map[index_of_ter].Edges)
            {
                if (s.Ruled_by != (Global.Control)(this.current_player + 1))
                    return true;
            }

        }
        return false;
    }


    /// <summary>
    /// check if the destention is possbile
    /// </summary>
    /// <param name="index_of_des">the attcked index</param>
    /// <param name="index_of_sou">the attacker index </param>
    /// <returns>true if can be attacked</returns>
    public bool destention_is_possible(int index_of_des, int index_of_sou)
    {
        //is a neighbor of the chosen index
        if (this.mp.vertex_is_neighbor(index_of_sou, index_of_des))
        {
            //is not owned by attacker
            if ((Global.Control)(this.current_player + 1) != this.mp.map[index_of_des].Info.Ruled_by)
            {
                return true;
            }
           
        }    
        return false;
    }
    /// <summary>
    /// get the rolled dices(from biggest outcome to the least)
    /// </summary>
    /// <param name="inex_of_ter">index of the teritory </param>
    /// <returns>list of roled dices</returns>
    private List<int> dice_rolled(int inex_of_ter)
    {
        List<int> all_rolled = new List<int>();
        int rand_num;
        int counter = 0;
        //top is 3 dices
        while (counter < 3 && this.mp.map[inex_of_ter].Info.Num_of_forces - counter > 0)
        {

            rand_num = rd.Next(1, 7);
            //add the dice
            all_rolled.Add(rand_num);
            counter++;
        }
        //from the largest number to the smallest
        all_rolled.Sort();
        all_rolled.Reverse();
        return all_rolled;
    }
    /// <summary>
    ///  update the quantity in the teritories
    /// </summary>
    /// <param name="des">attacker</param>
    /// <param name="sou">defender</param>
    /// <param name="qun">how many to send</param>
    public void moveForces(int des, int sou, int qun)
    {
        this.mp.map[des].Info.Num_of_forces += qun;
        this.mp.map[sou].Info.Num_of_forces -= qun;
    }
}
/// <summary>
/// the class that is used to reinforce the territories
/// </summary>
class reinforcement
{
    /*
    Asia: 7
    Nort America: 5
    Europe: 5
    Africa: 3
    South America: 2
    Australia: 2 

    numofforces/3  
      */
    private int current_player;
    private int number_of_forces;
    public Graph mp;
    public reinforcement(int current_player, Graph graph) 
    {
        this.current_player = current_player;
        this.mp = graph;
        how_many_rein(); 
    }
    /// <summary>
    /// get the number of forces
    /// </summary>
    /// <returns>how many forces left</returns>
    public int getNumberOfForces()
    {
        return this.number_of_forces;
    }
    /// <summary>
    /// deploy the forces in the wanted location
    /// </summary>
    /// <param name="idex_of_cou">which territory to put</param>
    /// <param name="wantedNumber">how many forces to put</param>
    /// <returns></returns>
    public bool choose_forces(int idex_of_cou, int wantedNumber)
    {
        int beforeDeploy;
        if (this.number_of_forces > 0)
        {

            player_teritories turn_of_player_x = new player_teritories(this.current_player, this.number_of_forces, false, this.mp);
            beforeDeploy = this.number_of_forces;
            //change the number of soldiers that the player need to put
            this.number_of_forces = turn_of_player_x.choose_teritories(idex_of_cou, wantedNumber);


            //not possible selection so try again
            if (beforeDeploy == this.number_of_forces)
            {
                return false;
            }

            return true;

        }

        return true;
    }
    /// <summary>
    /// get how many forces to deploy
    /// </summary>
    private void how_many_rein()
    {
        int count = 0;
        //how many forces in map
        count = this.mp.how_many_forces((Global.Control)(this.current_player + 1));
        count /= 3;
        //Africa
        if (this.mp.has_continent(this.mp.create_list_of_Africa(), (Global.Control)(this.current_player + 1)))
        {
            count += 3;
        }
        //Asia
        if (this.mp.has_continent(this.mp.create_list_of_Asia(), (Global.Control)(this.current_player + 1)))
        {
            count += 7;
        }
        //Australia
        if (this.mp.has_continent(this.mp.create_list_of_Australia(), (Global.Control)(this.current_player + 1)))
        {
            count += 2;
        }
        //South America
        if (this.mp.has_continent(this.mp.create_list_of_SouthAmerica(), (Global.Control)(this.current_player + 1)))
        {
            count += 2;
        }
        //Nort America
        if (this.mp.has_continent(this.mp.create_list_of_NorthAmerica(), (Global.Control)(this.current_player + 1)))
        {
            count += 5;
        }
        //Europe
        if (this.mp.has_continent(this.mp.create_list_of_Europe(), (Global.Control)(this.current_player + 1)))
        {
            count += 5;
        }
        this.number_of_forces = count;

    }

}
/// <summary>
///  the class that is used to move forces between two territories
/// </summary>
class moving
{
    private Graph temp_map;
    public Graph mp;
    private int current_player;
    private List<int> possible_sou;
    public moving(int current_player, Graph graph)
    {
        this.temp_map = new Graph();
        this.current_player = current_player;
        this.mp = graph;
    }


    /// <summary>
    /// get all the possbile sources
    /// </summary>
    public void possible_source()
    {
        this.possible_sou = new List<int>();
        for (int i = 0; i < Global.MAX_INDEX_OF_TER; i++)
        {
            if (location_source_possbile(i))
                this.possible_sou.Add(i);
        }
    }
    /// <summary>
    /// player can move if he wants
    /// </summary>
    /// <returns>true if possbile </returns>
    public bool canMove()
    {
        if(this.possible_sou.Count>0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// check if location can send forces
    /// </summary>
    /// <param name="index_of_ter">index to check</param>
    /// <returns>true if possbile</returns>
    public bool location_source_possbile(int index_of_ter)
    {
        //owned by the current player
        if ((Global.Control)(this.current_player + 1) == this.mp.map[index_of_ter].Info.Ruled_by)
        {
            //not enough forces
            if (this.mp.map[index_of_ter].Info.Num_of_forces < 2)
            {
                return false;
            }
            //if the neigbors is in the same control
            foreach (State s in this.mp.map[index_of_ter].Edges)
            {
                if (s.Ruled_by == (Global.Control)(this.current_player + 1))
                    return true;
            }

        }
        return false;
    }



    /// <summary>
    /// recursion function that create map of all the possible location to move to(connect teritories)
    /// </summary>
    /// <param name="index_of_ter"> index to move form</param>
    public void possible_des(int index_of_ter)
    {
        this.temp_map.map[index_of_ter].Info.Ruled_by = (Global.Control)(this.current_player + 1);//change the ruler of the teritory
                                                                                                  //go through all the neigbors
        foreach (State s in this.mp.map[index_of_ter].Edges)
        {
            //if the the neigbor country belongs to the same ruler
            if (s.Ruled_by == (Global.Control)(this.current_player + 1))
            {
                //if the the neigbor country wasnt already added
                if (this.temp_map.map[s.Index - 1].Info.Ruled_by != (Global.Control)(this.current_player + 1))
                {
                    possible_des(s.Index - 1);
                }
            }

        }

    }
    /// <summary>
    /// cant choose to move forces to the source location(remove from the temp map)
    /// </summary>
    /// <param name="index_of_ter">the index </param>
    public void removeSelf(int index_of_ter)
    {
        this.temp_map.map[index_of_ter].Info.Ruled_by = Global.Control.Free;//change the ruler of the teritory
                                                                                         
    }

    /// <summary>
    /// rather  the chosen source loaction is one of the possbile
    /// </summary>
    /// <param name="index_of_ter"> the index</param>
    /// <returns>true if possible </returns>
    public bool canChooseSou(int index_of_ter)
    {
        foreach (int cou in this.possible_sou)
        {
            if(cou== index_of_ter)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// check if chosen destention loaction is one of the possible
    /// </summary>
    /// <param name="index_of_ter">index of territory</param>
    /// <returns>true if possible</returns>
    public bool canChooseDes(int index_of_ter)
    {
        if (this.temp_map.map[index_of_ter].Info.Ruled_by == (Global.Control)(this.current_player + 1))
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// get the list of possible sources
    /// </summary>
    /// <returns>list</returns>
    public List<int> getTheSources()
    {
        return this.possible_sou;
    }
    /// <summary>
    /// move the forces in the map
    /// </summary>
    /// <param name="des">index of destention</param>
    /// <param name="sou">index of source</param>
    /// <param name="qun">how many to move</param>
    public void moveForces(int des,int sou,int qun)
    {
        this.mp.map[des].Info.Num_of_forces += qun;
        this.mp.map[sou].Info.Num_of_forces -= qun;
    }
}
/// <summary>
/// the class that is used to give each player a secret mission
/// </summary>
class SecretMissions
{
    /*
   1) Capture Europe, Australia and one other continent
2)Capture Europe, South America and one other continent
3)Capture North America and Africa
4)Capture North America and Australia
5)Capture Asia and South America
6)Capture Asia and Africa
7)Capture 24 territories
8)Capture 18 territories and occupy each with two troops
    * */
    public List<int> playersMissions;
    public Graph mp;
    /// <summary>
    /// give to each player a diffrent random mission
    /// </summary>
    /// <param name="numberOfPlayers">how many players</param>
    /// <param name="graph">the map</param>
    public SecretMissions(int numberOfPlayers, Graph graph)
    {
        this.mp = graph;
        System.Random rnd = new System.Random();
        int index;
        this.playersMissions = new List<int>();
        List<int> numberList = new List<int>();
        for (int i = 1; i < 9; i++)
        {
            numberList.Add(i);
        }
        //add a secret mission for each player
        for (int j = 0; j < numberOfPlayers; j++)
        {
            index = rnd.Next(0, numberList.Count); //pick a random item from the master list
            this.playersMissions.Add(numberList[index]); //place it at the end of the randomized list
            numberList.RemoveAt(index);

        }
    }
    /// <summary>
    /// check if player did the mission
    /// </summary>
    /// <param name="currentPlayer">checked player</param>
    /// <returns>true if player won</returns>
    public bool missionAccomplished(int currentPlayer)
    {

       if (whichMission(currentPlayer) ==true)
        {
            return true;
        }
        return false;
    }
    //check which mission
    /// <summary>
    /// check if mission accomplished by player
    /// </summary>
    /// <param name="currentPlayer">the wanted player</param>
    /// <returns></returns>
    private bool whichMission(int currentPlayer)
    {
        Global.Control player = (Global.Control)(currentPlayer + 1);
        switch (this.playersMissions[currentPlayer])
        {
            case 1: return mission1(player);
            case 2: return mission2(player);
            case 3: return mission3(player);
            case 4: return mission4(player);
            case 5: return mission5(player);
            case 6: return mission6(player);
            case 7: return mission7(player);
            case 8: return mission8(player);

        }
        return false;

    }
    /// <summary>
    /// chck which continets the player own
    /// </summary>
    /// <param name="currentPlayer">the checked player</param>
    /// <returns>list of bool(player own the continet or not)</returns>
    private bool[] continets(Global.Control currentPlayer)
    {
        bool[] cont = new bool[6];
        cont[0] = this.mp.has_continent(this.mp.create_list_of_NorthAmerica(), currentPlayer);
        cont[1] = this.mp.has_continent(this.mp.create_list_of_SouthAmerica(), currentPlayer); ;
        cont[2] = this.mp.has_continent(this.mp.create_list_of_Africa(), currentPlayer); ;
        cont[3] = this.mp.has_continent(this.mp.create_list_of_Europe(), currentPlayer); ;
        cont[4] = this.mp.has_continent(this.mp.create_list_of_Asia(), currentPlayer); ;
        cont[5] = this.mp.has_continent(this.mp.create_list_of_Australia(), currentPlayer);


        return cont;
    }
    /*=================================
    | check the missions              |
    ==================================*/
    /// <summary>
    /// Capture Europe, Australia and one other continent
    /// </summary>
    /// <param name="currentPlayer"></param>
    /// <returns></returns>
    private bool mission1(Global.Control currentPlayer)
    {
        bool[] cont = continets(currentPlayer);
        int counter = 0;
        foreach (bool contime in cont)
        {
            if (contime == true)
                counter++;
        }
        if (counter > 2 && cont[3] == true && cont[5] == true)
            return true;
        return false;
    }
    /// <summary>
    /// Capture Europe, South America and one other continent
    /// </summary>
    /// <param name="currentPlayer"></param>
    /// <returns></returns>
    private bool mission2(Global.Control currentPlayer)
    {
        bool[] cont = continets(currentPlayer);
        int counter = 0;
        foreach (bool contime in cont)
        {
            if (contime == true)
                counter++;
        }
        if (counter > 2 && cont[3] == true && cont[1] == true)
            return true;
        return false;
    }
    /// <summary>
    /// Capture North America and Africa
    /// </summary>
    /// <param name="currentPlayer"></param>
    /// <returns></returns>
    private bool mission3(Global.Control currentPlayer)
    {
        bool[] cont = continets(currentPlayer);
       
        if (cont[0] == true && cont[2] == true)
            return true;
        return false;
    }
    /// <summary>
    /// Capture North America and Australia
    /// </summary>
    /// <param name="currentPlayer"></param>
    /// <returns></returns>
    private bool mission4(Global.Control currentPlayer)
    {
        bool[] cont = continets(currentPlayer);

        if (cont[0] == true && cont[5] == true)
            return true;
        return false;
    }
    /// <summary>
    /// Capture Asia and South America
    /// </summary>
    /// <param name="currentPlayer"></param>
    /// <returns></returns>
    private bool mission5(Global.Control currentPlayer)
    {
        bool[] cont = continets(currentPlayer);

        if (cont[1] == true && cont[4] == true)
            return true;
        return false;
    }
    /// <summary>
    /// Capture Asia and Africa
    /// </summary>
    /// <param name="currentPlayer"></param>
    /// <returns></returns>
    private bool mission6(Global.Control currentPlayer)
    {
        bool[] cont = continets(currentPlayer);

        if (cont[2] == true && cont[4] == true)
            return true;
        return false;
    }
    /// <summary>
    /// Capture 24 territories
    /// </summary>
    /// <param name="currentPlayer"></param>
    /// <returns></returns>
    private bool mission7(Global.Control currentPlayer)
    {
        int counter = this.mp.number_of_forces(currentPlayer)[0];
        if(23< counter)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Capture 18 territories and occupy each with two troops
    /// </summary>
    /// <param name="currentPlayer"></param>
    /// <returns></returns>
    private bool mission8(Global.Control currentPlayer)
    {
        int counter = this.mp.number_of_forces(currentPlayer)[1];
        if (17 < counter)
        {
            return true;
        }
        return false;
    }
}
/// <summary>
/// the class that is used to play as a bot
/// </summary>
class Ai
{
    private int whichPlayer;
    private List<List<int>> world_map;
    public Ai(int current_player)
    {
        world_map = list_of_continents();
        this.whichPlayer = current_player;
    }
    /// <summary>
    /// which continet the map is made of(for more generic program)
    /// </summary>
    /// <returns> list of lists of indexes(each list represent a continet)</returns>
    private List<List<int>> list_of_continents()
    {
        /*
         wolrd:
         north america
         South America
         Europe
         Asia
         Africa
         Australia

         */
        List<List<int>> world = new List<List<int>>();
        world.Add(Global.map.create_list_of_NorthAmerica());
        world.Add(Global.map.create_list_of_SouthAmerica());
        world.Add(Global.map.create_list_of_Europe());
        world.Add(Global.map.create_list_of_Asia());
        world.Add(Global.map.create_list_of_Africa());
        world.Add(Global.map.create_list_of_Australia());
        return world;
    }
    /// <summary>
    /// info about the continets in the list
    /// </summary>
    /// <param name="indexOfC">which continet from the list</param>
    /// <returns>the info about the continet:
    ///     [0]-bonus
    ///     [1]-borders
    ///     [2]-ter
    /// </returns>
    private int[] continentBR(int indexOfC)
    {

        int[] infoOfCon = new int[3];
        switch (indexOfC)
        {
            //north
            case 0:
                infoOfCon[0] = 5;
                infoOfCon[1] = 3;
                infoOfCon[2] = 9;
                break;
            //south
            case 1:
                infoOfCon[0] = 2;
                infoOfCon[1] = 2;
                infoOfCon[2] = 4;
                break;
            //eu
            case 2:
                infoOfCon[0] = 5;
                infoOfCon[1] = 4;
                infoOfCon[2] = 7;
                break;
            //as
            case 3:
                infoOfCon[0] = 7;
                infoOfCon[1] = 5;
                infoOfCon[2] = 12;
                break;
            //af
            case 4:
                infoOfCon[0] = 3;
                infoOfCon[1] = 3;
                infoOfCon[2] = 6;
                break;
            //au
            case 5:
                infoOfCon[0] = 2;
                infoOfCon[1] = 1;
                infoOfCon[2] = 4;
                break;
            default:
                //prob
                Debug.LogError("prob");
                infoOfCon[0] = 0;
                infoOfCon[1] = 0;
                infoOfCon[2] = 0;
                break;
        }
        return infoOfCon;
    }


    /************
     *choose    *
     *teritories*
     ************/
    /// <summary>
    /// choose the best territory in the most worth continet
    /// </summary>
    /// <param name="st">the start class</param>
    public void makeMoveInStart(Start_Game st)
    {
        /*
         score (of continent) can be negitive or positive 
          positive if the continent is yet to be conquered by any state/only by your own soldirs
          negitve if the continent has enemy inside 
          SCORE is from 0 to 100 or 0 to -inf
           i caculate for pos:
           number_of_occupied*(100/number_of_counteries)
           for neg:
            -number_of_occupied_enemy_soldiers     
           
         */
        List<int> score_for_continet = new List<int>();
        int number_of_occupied;
        int number_of_counteries;
        int number_of_occupied_enemy_soldiers;
        bool flag_whole;
        int score;
        int index_of_con = 0;

        bool selectTheCon = false;
        List<int> notNeed = new List<int>();
        //go through each continet
        foreach (List<int> cont in this.world_map)
        {
            flag_whole = true;
            number_of_counteries = cont.Count;
            number_of_occupied = 0;
            number_of_occupied_enemy_soldiers = 0;
            //go through all the country 
            foreach (int count in cont)
            {
                //the country is owned by the player
                if (Global.map.map[count].Info.Ruled_by == (Global.Control)(this.whichPlayer + 1))
                {
                    //the player has the country
                    number_of_occupied++;
                }
                else
                {
                    //the country is not  free
                    if (Global.map.map[count].Info.Ruled_by != Global.Control.Free)
                    {
                        flag_whole = false;
                        //number of enmy soldiers in the continet
                        number_of_occupied_enemy_soldiers += Global.map.map[count].Info.Num_of_forces;
                    }
                }
            }
            //positive score
            if (flag_whole == true)
            {
                score = number_of_occupied * (100 / number_of_counteries);
            }
            else
            {
                score = -number_of_occupied_enemy_soldiers;
            }
            score_for_continet.Add(score);
        }
        //find the index of the continet to add to
        while (selectTheCon == false)
        {
            //not checked every option
            if (notNeed.Count < score_for_continet.Count)
            {
                index_of_con = findMaxCon(score_for_continet, notNeed);
                //the contintet need to be stronger
                if (checkIfNeedCon(this.world_map[index_of_con],false))
                {
                    selectTheCon = true;
                }
                else
                {
                    notNeed.Add(index_of_con);
                }

            }
            //no need fore back up but make the most important contintet stronger
            else
            {
                selectTheCon = true;
                notNeed.Clear();
                index_of_con = findMaxCon(score_for_continet, notNeed);

            }


        }
        //put in most needed country
        st.choose_forces(findMostStart(this.world_map[index_of_con]), this.whichPlayer);
    }
    /// <summary>
    /// check if the territory need more soldiers
    /// </summary>
    /// <param name="list_con">the continet</param>
    /// <param name="reinF">if it is the reinforces stage</param>
    /// <returns>true if back up is needed</returns>
    private bool checkIfNeedCon(List<int> list_con,bool reinF)
    {
        bool needed = false;
        bool flagHasFree = false;
        for (int i = 0; i < list_con.Count && needed == false; i++)
        {
            //belong to the player
            if (Global.map.map[list_con[i]].Info.Ruled_by == (Global.Control)(this.whichPlayer + 1))
            {
                //if its not the reinfocment stage(the rate is diffrent)
                if(reinF == false)
                {
                    //the back up is needed 
                    if (rateCou(Global.map.map[list_con[i]]) >= 0)
                    {
                        needed = true;
                    }
                }
                else
                {
                    //the back up is needed 
                    if (rateCouRein(Global.map.map[list_con[i]]) >= 0)
                    {
                        needed = true;
                    }
                }
               
            }
            //free
            if (Global.map.map[list_con[i]].Info.Ruled_by == Global.Control.Free)
            {
                flagHasFree = true;
            }
        }
        //if there is still free countries
        if (flagHasFree == true)
        {
            needed = true;
        }
        return needed;
    }
    /// <summary>
    /// find the continet where reinforcement is most needed
    /// </summary>
    /// <param name="list_con">all the continets</param>
    /// <param name="not_needed_con">continets that not need back up(skip them)</param>
    /// <returns>index of the continet in the list </returns>
    private int findMaxCon(List<int> list_con, List<int> not_needed_con)
    {
        int max_index = 0;
        int Maxscore = int.MinValue;
        //find the max score in the continets
        for (int i = 0; i < list_con.Count; i++)
        {
            //if the continet is one that need back up
            if (!not_needed_con.Contains(i))
            {
                if (Maxscore < list_con[i])
                {
                    Maxscore = list_con[i];
                    max_index = i;
                }
            }

        }
        return max_index;
    }
    /// <summary>
    /// find the best territory to put the force in the continet
    /// </summary>
    /// <param name="list_con">indexes of the continests in the map</param>
    /// <returns>index of the territory </returns>
    private int findMostStart(List<int> list_con)
    {
        int counterIndex = 0;
        int rate;
        int Maxrate = int.MinValue;
        bool flagEmpty = false;
        for (int i = 0; i < list_con.Count && flagEmpty == false; i++)
        {
            //empty
            if (Global.map.map[list_con[i]].Info.Ruled_by == Global.Control.Free)
            {
                flagEmpty = true;
                counterIndex = list_con[i];
            }
            //belong to the player
            else if (Global.map.map[list_con[i]].Info.Ruled_by == (Global.Control)(this.whichPlayer + 1))
            {
                //if the back up is more needed in this index
                rate = rateCou(Global.map.map[list_con[i]]);
                if (Maxrate < rate)
                {
                    Maxrate = rate;
                    counterIndex = list_con[i];
                }
            }
        }

        return counterIndex;
    }
    /************
     *the ai    *
     *move      *
     ************/
    /// <summary>
    /// play one round as ai
    /// </summary>
    /// <param name="inStart">bool if the move is in the start of the game(so skip reinforce)</param>
    /// <param name="instean">the country manger(to use its functions) </param>
    /// <returns>result of attack</returns>
    public int playATurn(bool inStart, CountryManager instean)
    {
        if (inStart == false)
        {
            //choose the reinforce
            try
            {
                playRien();
            }
            catch (Exception)
            {

                Debug.LogError("in rein");
            }
           


        }

        //attack
        int sta = 0;

        try
        {
            sta = playAttack(instean);
        }
        catch (Exception)
        {

            Debug.LogError("in attack");
        }



        //if ai didnt win
        if (sta != 1)
        {
            //move
            try
            {
                playMove();
            }
            catch (Exception)
            {

                Debug.LogError("in move");
            }
        }
        return sta;


    }
    /************
     *attack    *
     *stage     *
     ************/

    /// <summary>
    ///   play attack(attack the best locations)  
    /// </summary>
    /// <param name="instean">game mager</param>
    /// <returns>result of battles</returns>
    public int playAttack(CountryManager instean)
    {
        Attack att = new Attack(whichPlayer, Global.map);
        bool stopAttack = false;
        bool stopBattle = false;
        List<int> allPossibleSour;
        int indexOfAttacer;
        int indexOfDefender;
        int outcome;
        int rulerOfDefender;
        //tha attack is not stopped
        while (stopAttack == false)
        {
            att.possible_sources();
            //if possible to attack
            if (att.possibleToAttack() == true)
            {
                allPossibleSour = att.possible_sou;
                indexOfAttacer = bestToattackFrom(allPossibleSour);
                indexOfDefender = weakestEnemy(Global.map.map[indexOfAttacer]);
                rulerOfDefender = Global.indexOfControl(Global.map.map[indexOfDefender].Info.Ruled_by);
                //attacker has lower chance to win (dont attack) (you need at least 2 more soldiers)
                if (betterDef(indexOfAttacer, indexOfDefender))
                {
                    stopAttack = true;
                }
                else
                {
                    //attack till end//better backoff
                    stopBattle = false;
                    do
                    {
                        outcome = att.attackOnce(indexOfAttacer, indexOfDefender);
                        //not lost or won
                        if(outcome==2)
                        {
                            //if you lost more soldiers that the defender
                            if (Global.map.map[indexOfAttacer].Info.Num_of_forces < Global.map.map[indexOfDefender].Info.Num_of_forces)
                            {
                                stopBattle = true;

                            }
                        }
                    } while (outcome == 2 && stopBattle == false);
                    //won
                    if (outcome == 0)
                    {
                        //defender lost the game
                        //the attacked land wasnt free country
                        if (rulerOfDefender != -1)
                        {
                            instean.playerDefeatedDad(rulerOfDefender);
                        }

                        //bot won
                        if (instean.checkWinDad() == true)
                        {
                            return 1;
                        }
                        //check win


                        //how many soldiers to send
                        att.moveForces(indexOfDefender, indexOfAttacer, howManyToSend(indexOfDefender, indexOfAttacer));
                    }
                }
                //find if have more chances to win
            }
            else
            {
                stopAttack = true;
            }
        }

        return 0;
    }
    /// <summary>
    /// if the attacker won the battle choose how to divide the forces 
    /// </summary>
    /// <param name="indexOfDefender">index of the place that was attaked</param>
    /// <param name="indexOfAttacer">index of the place that attaked</param>
    /// <returns>how many forces to move from the attackerto the defender country</returns>
    private int howManyToSend(int indexOfDefender, int indexOfAttacer)
    {
        int qua = 0;
        int eneFrom = enemiesQua(Global.map.map[indexOfAttacer]);
        int eneTo = enemiesQua(Global.map.map[indexOfDefender]);
        //there are enemy near the attacker
        if(eneFrom>0)
        {
            //in danger
            if (eneFrom - Global.map.map[indexOfAttacer].Info.Num_of_forces > 0)
            {
                //the defender country need back up
                if(eneTo > 0)
                {
                    //check if less than half is possible
                    if(eneTo< (int)Global.map.map[indexOfAttacer].Info.Num_of_forces / 2)
                    {
                        qua = eneTo;
                    }
                    else
                    {
                        //both need so half half with each other
                        qua = (int)Global.map.map[indexOfAttacer].Info.Num_of_forces / 2;
                    }
                }
                else
                {
                    //minimum 
                    qua = 1;
                }
            }
            else
            {
                if(eneTo > 0)
                {
                    //the leftover from habing as soldires as the neighbors is not enough
                    if(eneTo> eneFrom - Global.map.map[indexOfAttacer].Info.Num_of_forces)
                    {
                        qua = (int)Global.map.map[indexOfAttacer].Info.Num_of_forces/2;
                    }

                    else
                    {
                        //give the left over
                        qua = eneFrom - Global.map.map[indexOfAttacer].Info.Num_of_forces;
                    }
                }
            }
        }
        else
        {
            //there are enemy near the defender
            if (eneTo > 0)
            {
                //all the soldiers 
                qua = (int)Global.map.map[indexOfAttacer].Info.Num_of_forces-1;
            } 
            //no need to deliver
            else
            {

                if (Global.map.map[indexOfAttacer].Info.Num_of_forces > 2)
                {
                    // more than half(for more soldier to deliver in the moving stage)
                    qua = (int)Global.map.map[indexOfAttacer].Info.Num_of_forces * (2 / 5);
                }
                else
                {
                    qua = 1;
                }
            }
        }
        return qua;


    }
    /// <summary>
    /// choose if the attacker should attack or not 
    /// </summary>
    /// <param name="indexOfAttacer">index of the place that attaked</param>
    /// <param name="indexOfDefender">index of the place that was attaked</param>
    /// <returns>true if bot better not attack</returns>
    private bool betterDef(int indexOfAttacer,int indexOfDefender)
    {
        bool defendF = false;
        //if you have less than 3(you wont get any reinforcement)
        if (Global.map.how_many_forces((Global.Control)(this.whichPlayer + 1)) < 3)
        {
            // if you are weaker that the weakest neigbor (wait meybe he will be weaker next rounds)
            if (Global.map.map[indexOfAttacer].Info.Num_of_forces < Global.map.map[indexOfDefender].Info.Num_of_forces)
            {
                defendF = true;

            }
        }
        else
        { 
            //if the defender is not free
            if(Global.map.map[indexOfDefender].Info.Ruled_by != Global.Control.Free)
            {
                //you are surrounded by more enemies(so better wait for reinfocement)
                if (rateCou(Global.map.map[indexOfAttacer]) >= 0)
                {
                    defendF = true;
                }
            }
        }
        return defendF;


    }
    /// <summary>
    /// choose which country is best to attack from the selected territory
    /// </summary>
    /// <param name="listOfSources">all possbile places to attack from</param>
    /// <returns>index of the best location to attack from</returns>
    private int bestToattackFrom(List<int> listOfSources)
    {
        int min = int.MaxValue;
        int score;
        int indexChose = -1;
        foreach (int indexOfCou in listOfSources)
        {
            //has the lower score (strongest) 
            score = rateCou(Global.map.map[indexOfCou]);
            if (min > score)
            {
                min = score;
                indexChose = indexOfCou;
            }
        }
        return indexChose;
    }
    /// <summary>
    /// find which territort is best to attack
    /// </summary>
    /// <param name="count">the country that the attacker is at</param>
    /// <returns>the index of the the best lecation to attack</returns>
    private int weakestEnemy(Vertex count)
    {
        int indexOfEnemy = -1;
        int numOfSoldirs = int.MaxValue;
        foreach (State enem in count.Edges)
        {
            //not same ruler
            if (enem.Ruled_by != (Global.Control)(this.whichPlayer + 1))
            {
                if (enem.Num_of_forces < numOfSoldirs)
                {
                    indexOfEnemy = enem.Index - 1;
                    numOfSoldirs = enem.Num_of_forces;
                }
            }
        }
        return indexOfEnemy;
    }
    /************
     *reinforce *
     *stage     *
     ************/
    /// <summary>
    /// deploy all the soldiers the bot has in reinforcement
    /// </summary>
    public void playRien()
    {
        reinforcement rie = new reinforcement(whichPlayer, Global.map);
        bool reinforceFlag = true;
        int indexOfCountry;
        int numberOfNeededBackUp;
        List<int> score_for_continet = new List<int>();
        bool selectTheCon = false;
        List<int> notNeed;
        int index_of_con = 0;
        //until all the soldiers were deployed
        
        while (reinforceFlag == true)
        {
            if (rie.getNumberOfForces() > 0)
            {

                //go through each continet and add the score of con
                for (int i = 0; i < this.world_map.Count; i++)
                {
                    score_for_continet.Add(targetContinent(i));
                }
                //each time reset
                notNeed = new List<int>();
                //find the index of the continet to add to
                while (selectTheCon == false)
                {
                    //not checked every option
                    if (notNeed.Count < score_for_continet.Count)
                    {
                        index_of_con = findMaxCon(score_for_continet, notNeed);
                        //the contintet need to be stronger
                        if (checkIfNeedCon(this.world_map[index_of_con],true))
                        {
                            selectTheCon = true;
                        }
                        else
                        {
                            notNeed.Add(index_of_con);
                        }

                    }
                    //no need fore back up but make the most important contintet stronger
                    else
                    {
                        selectTheCon = true;
                        notNeed.Clear();
                        index_of_con = findMaxCon(score_for_continet, notNeed);

                    }

                }
                indexOfCountry = findMostRie(this.world_map[index_of_con]);
                numberOfNeededBackUp = rateCouRein(Global.map.map[indexOfCountry]);
                // less than 0(more than neighbours) equal to  0(as number as enemies) more than o( weaker than neighbores)
                //you already have enogh soldiers or you need to deploy all the rienforsemnt lefy
                if (numberOfNeededBackUp < 1 || numberOfNeededBackUp >= rie.getNumberOfForces())
                {
                    //put all the rein in this country
                    rie.choose_forces(indexOfCountry, rie.getNumberOfForces());
                    reinforceFlag = false;
                }
                else
                {
                    //become stronger than nighbors but not all the rein
                    rie.choose_forces(indexOfCountry, numberOfNeededBackUp + 1);
                }

            }
            else
            {
                reinforceFlag = false;
            }
        }
    }
    /// <summary>
    /// find the most needed location for reinforcement in the continet
    /// </summary>
    /// <param name="ter_in_continet">the continet to choose from</param>
    /// <returns>the index of the territory</returns>
    public int findMostRie(List<int> ter_in_continet)
    {
        int counterIndex = 0;
        int rate;
        int Maxrate = int.MinValue;
        for (int i = 0; i < ter_in_continet.Count; i++)
        {
            //belong to the player
            if (Global.map.map[ter_in_continet[i]].Info.Ruled_by == (Global.Control)(this.whichPlayer + 1))
            {
                //if the reinforce is more needed in this index
                rate = rateCou(Global.map.map[ter_in_continet[i]]);
                if (Maxrate < rate)
                {
                    Maxrate = rate;
                    counterIndex = ter_in_continet[i];
                }
            }
        }


        return counterIndex; 
    }
    /// <summary>
    /// get the score of the country(lower is stronger)
    /// </summary>
    /// <param name="countr">the territory that is checked</param>
    /// <returns>the score of the terrory</returns>
    private int rateCouRein(Vertex countr)
    {
        int rate = 0;
        //get the score of the  territory
        rate += enemiesQuaRein(countr);
        rate -= countr.Info.Num_of_forces;
        return rate;
    }
    /// <summary>
    /// get how many soldiers the enemies have near the country(multiply by 1.5 for extra strength)
    /// </summary>
    /// <param name="countr">the territory that is checked</param>
    /// <returns>the number of forces</returns>
    private int enemiesQuaRein(Vertex countr)
    {
        /*
         F = 1.5E + T. For example, if enemy has 20 armies in 4 territories that you want to conquer in this turn, then place F = 1.5 * 20 + 4 = 34 armies and start to attack.
          */
        int numOfEn = 0;
        int numOfEnCout = 0;
        foreach (State enem in countr.Edges)
        {
            //not same ruler or unrolled
            if (enem.Ruled_by != (Global.Control)(this.whichPlayer + 1) && enem.Ruled_by != Global.Control.Free)
            {
                numOfEn += enem.Num_of_forces;
                numOfEnCout++;
            }
        }
        return (int)1.5*numOfEn+ numOfEnCout;
    }
    /************
     *move      *
     *stage     *
     ************/
    /// <summary>
    /// move the soldiers from not treatned territory for the most needed
    /// </summary>
    public void playMove()
    {
        moving mov = new moving(this.whichPlayer, Global.map);
        List<int> allPossibleOptions;
        int source;
        int destention;
        mov.possible_source();
        //have possible location
        if (mov.canMove() == true)
        {
            allPossibleOptions = mov.getTheSources();
            source = bestToMoveFrom(allPossibleOptions);
            //has a possible location that not neighbor with enemy
            if (source != -1)
            {
                //get possble des
                mov.possible_des(source);
                mov.removeSelf(source);
                //choose where to send the soldiers to
                destention = bestToMovTo(mov);
                //mov all the soldiers to there
                mov.moveForces(destention, source, Global.map.map[source].Info.Num_of_forces - 1);
            }
        }
    }
    /// <summary>
    /// find the best territory to send forces from
    /// </summary>
    /// <param name="listOfSources">list of possbile indexes</param>
    /// <returns>index of trrriotry</returns>
    private int bestToMoveFrom(List<int> listOfSources)
    {
        int max = int.MinValue;
        int indexChose = -1;
        foreach (int indexOfCou in listOfSources)
        {
            bool ok = true;
            foreach (State coun in Global.map.map[indexOfCou].Edges)
            {
                //not same ruler or unrolled
                if (Global.map.map[indexOfCou].Info.Ruled_by != (Global.Control)(this.whichPlayer + 1) && Global.map.map[indexOfCou].Info.Ruled_by != Global.Control.Free)
                {
                    ok = false;
                }
            }
            //not have enemies near him
            if (ok == true)
            {
                //has the most soldiers
                if (Global.map.map[indexOfCou].Info.Num_of_forces > max)
                {
                    indexChose = indexOfCou;
                }
            }
        }
        return indexChose;
    }
    /// <summary>
    /// find the location that need the back up the most(and connected to the chosen source)
    /// </summary>
    /// <param name="mov">moving class for it's functions</param>
    /// <returns>the index of the terrotey </returns>
    private int bestToMovTo(moving mov)
    {
        int maxRate = int.MinValue;
        int index = 0;
        int rate;
        // check all countries
        foreach (Vertex countries in Global.map.map)
        {
            //can move to
            if (mov.canChooseDes(countries.Info.Index - 1))
            {
                rate = rateCou(countries);
                //need more the rein
                if (maxRate < rate)
                {
                    maxRate = rate;
                    index = countries.Info.Index - 1;
                }
            }

        }
        return index;
    }

    /************
    *for        *
    *all stages *
     ************/

    /// <summary>
    /// get score of the territry(score is (sum of neibors enemies)-(num of soldiers))
    /// </summary>
    /// <param name="countr">index of country</param>
    /// <returns>score</returns>
    private int rateCou(Vertex countr)
    {
        int rate = 0;
        //get the score of the  territory
        rate += enemiesQua(countr);
        rate -= countr.Info.Num_of_forces;
        return rate;
    }
    /// <summary>
    /// how many forces neighbors enemies have
    /// </summary>
    /// <param name="countr">the check territory</param>
    /// <returns>sum/returns>
    private int enemiesQua(Vertex countr)
    {
        int numOfEn = 0;
        foreach (State enem in countr.Edges)
        {
            //not same ruler or unrolled
            if (enem.Ruled_by != (Global.Control)(this.whichPlayer + 1) && enem.Ruled_by != Global.Control.Free)
            {
                numOfEn += enem.Num_of_forces;
            }
        }
        return numOfEn;
    }
    /// <summary>
    /// rating of the continet
    /// </summary>
    /// <param name="index_con">index of continet in the continets list</param>
    /// <returns>score</returns>
    private int targetContinent(int index_con)
    {
        int[] fullCon = calcNumOf(this.world_map[index_con], false);
        int[] forPlayer = calcNumOf(this.world_map[index_con], true);
        //number of soldiers of the player in the continet
        int ap = forPlayer[1];
        //number of soldiers in the continet
        int ta = fullCon[1];
        //number of countries in the continet
        int ter = fullCon[0];
        //number of countries owned by the player in the continet
        int aTer =  forPlayer[0];
        int cr= continentRating(index_con);//rating of the continet
        return (((ap / ta) + (aTer / ter)) / 2) * cr;
    }
    /// <summary>
    /// get info about number of soldiers and countries in continet
    /// </summary>
    /// <param name="con">the check continet</param>
    /// <param name="playerF">check of spacific player(false if not)</param>
    /// <returns>
    /// [0]number of territories
    /// [1]number of forces
    /// </returns>
    private int[] calcNumOf(List<int> con, bool playerF)
    {
        int[] numOf = new int[2];
        //initilize
        //num of ter
        numOf[0] = 0;
        //num of armies
        numOf[1] = 0;

        for (int i = 0; i < con.Count; i++)
        {
            //belong to the player
            if (Global.map.map[con[i]].Info.Ruled_by == (Global.Control)(this.whichPlayer + 1))
            {
                numOf[0] += 1;
                numOf[1] += Global.map.map[con[i]].Info.Num_of_forces;
            }
            //not belong
            else
            {
                //not calc for spacsific player
                if(playerF == false)
                {
                    numOf[0] += 1;
                    numOf[1] += Global.map.map[con[i]].Info.Num_of_forces;
                }
            }
        }

        return numOf;
    }
    /// <summary>
    ///permanent rating of continet 
    /// </summary>
    /// <param name="index_con">index of continet in the continets list</param>
    /// <returns>score</returns>
    private int continentRating(int index_con)
    {
        int[] infoC = continentBR(index_con);
        //number of bonus for getting full continet
        int bonus= infoC[0];
        //number of bordes for neighbors continets
        int borders= infoC[1];
        //numbe of countries in the con
        int terNum= infoC[2];
        return (15+ bonus -4* borders) / (terNum);
    }

}
