using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
/// <summary>
/// the main class that runs in the background and defines the game
/// </summary>
public class CountryManager : MonoBehaviour
{
    public static CountryManager insteance = null;
    public int getInputCountry;
    [SerializeField] private int counter = 0;
    public GameObject startPanel;
    public GameObject AttackPanel;
    public GameObject ReiPanel;
    public GameObject MovePanel;
    public GameObject WPanel;
    public GameObject forcesPanel;
    public int inputInexOfCou;
    public int inputInexOfCou2;


    private bool[] players;
    private bool[] playerAi;
    private int alivePlayers;
    private int currentPlayer;
    private int counterStart;
    /*
     0-reinforce
     1-attack
     2-move
     3-start the game
     */
    private int currentStage;
    //private bool start = true;
    private int numOfActivePlayer;

    Start_Game st;
    reinforcement rei;
    SecretMissions sm;
    Attack at;
    moving mo;
    Ai bot;
    /// <summary>
    /// create insteance of the singltone
    /// </summary>
    void Awake()
    {
        //create singltone of the class
        if (insteance != null && insteance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            insteance = this;
        }
    }
    /// <summary>
    /// one of the main functions, get the wanted territory from countryHnadler(the player) and play the game
    /// </summary>
    /// <param name="input">index of the country</param>
    public void getInput(int input)
    {
        //dont get input before the user declare the players
        if (this.forcesPanel != null)
        {
            if(this.forcesPanel.activeSelf ==false)
            {
                bool flag_next_player = false;
                //the player lost already
                if (players[this.currentPlayer] == false)
                {
                    //move to next player
                    nextPlayer();
                }
                else
                {
                    if (this.counter == 0)
                    {

                        this.inputInexOfCou = input;
                        //in the choose teritories stage
                        if (this.currentStage == 3)
                        {
                            bool flag_to_attack = false;
                            flag_next_player = chooseTer(this.inputInexOfCou);
                            //still need to be deployed
                            if (this.st.num_of_players > 0)
                            {
                                //the player picked good
                                if (flag_next_player == true)
                                {
                                    nextPlayer();
                                    //already moved so no need to move it
                                    flag_next_player = false;
                                    //while the player is bot
                                    while (this.st.num_of_players > 0 && this.playerAi[this.currentPlayer] == true)
                                    {
                                        //make a move as ai 
                                        this.bot = new Ai(this.currentPlayer);
                                        this.bot.makeMoveInStart(this.st);
                                        //all the soldiers werent deployed
                                        if (this.st.num_of_players > 0)
                                        {
                                            nextPlayer();
                                        }
                                        else
                                        {
                                            flag_to_attack = true;
                                        }
                                    }
                                }
                            }
                            //all the soldiers of all players were deployed
                            else
                            {
                                flag_to_attack = true;
                            }

                            if (flag_to_attack == true)
                            {
                                //hide the startPanel
                                if (this.startPanel != null)
                                {
                                    this.startPanel.SetActive(false);
                                }
                                this.currentStage = 1;
                                //move to next player
                                nextPlayer();
                                flag_next_player = false;
                                this.at = new Attack(this.currentPlayer, Global.map);
                                if (this.AttackPanel != null)
                                {
                                    this.AttackPanel.SetActive(true);
                                }
                            }
                        }
                        //in the choose reinfoesment stage
                        else if (this.currentStage == 0)
                        {
                            //check if all the soldiers were deployed
                            if (this.rei.getNumberOfForces() <= 0)
                            {
                                //hide the reinforsment Panel
                                if (this.ReiPanel != null)
                                {
                                    this.ReiPanel.SetActive(false);
                                }
                                //next stage
                                this.currentStage = 1;
                                this.at = new Attack(this.currentPlayer, Global.map);
                                //show the attackPanel
                                if (this.AttackPanel != null)
                                {
                                    this.AttackPanel.SetActive(true);
                                }
                            }
                            else
                            {
                                //allow  to reinforse
                                if (this.ReiPanel.transform.Find("conf") != null)
                                {
                                    this.ReiPanel.transform.Find("conf").gameObject.SetActive(true);
                                }
                                if (this.ReiPanel.transform.Find("SliderText") != null)
                                {
                                    this.ReiPanel.transform.Find("SliderText").gameObject.SetActive(true);
                                }
                                if (this.ReiPanel.transform.Find("Slider") != null)
                                {
                                    this.ReiPanel.transform.Find("Slider").gameObject.SetActive(true);
                                }
                                this.ReiPanel.transform.Find("Slider").GetComponent<Slider>().maxValue = this.rei.getNumberOfForces();
                                this.ReiPanel.transform.Find("Slider").GetComponent<Slider>().minValue = 1;
                            }

                        }
                        //on the attack stage
                        else if (this.currentStage == 1)
                        {
                            //create the list of possible location
                            at.possible_sources();
                            //can attack
                            if (at.possibleToAttack() == true)
                            {
                                //a possible location was selcted
                                if (at.in_possible_sources(this.inputInexOfCou))
                                {
                                    //show text
                                    if (this.AttackPanel.transform.Find("ataText") != null)
                                    {
                                        this.AttackPanel.transform.Find("ataText").gameObject.SetActive(true);
                                    }
                                    this.counter++;
                                }
                            }
                            else
                            {
                                //next stage
                                //hide the attackPanel
                                if (this.AttackPanel != null)
                                {
                                    this.AttackPanel.SetActive(false);
                                }
                                this.currentStage = 2;
                                this.mo = new moving(this.currentPlayer,Global.map);
                                //show the movekPanel
                                if (this.MovePanel != null)
                                {
                                    this.MovePanel.SetActive(true);
                                }

                            }

                        }
                        // on the move forces
                        else
                        {
                            //create all the possble source
                            this.mo.possible_source();
                            //can move 
                            if (this.mo.canMove() == true)
                            {
                                if (this.mo.canChooseSou(this.inputInexOfCou))
                                {
                                    this.mo.possible_des(this.inputInexOfCou);
                                    this.mo.removeSelf(this.inputInexOfCou);
                                    //show text
                                    if (this.MovePanel.transform.Find("first") != null)
                                    {
                                        this.MovePanel.transform.Find("first").gameObject.SetActive(true);
                                    }
                                    this.counter++;
                                }
                            }
                            else
                            {
                                moveNextStage();
                            }
                        }
                        //reinforsment
                        //attack
                        //check win
                        //move
                    }
                    //one teritory was already chosen
                    else if (this.counter == 1)
                    {
                        this.inputInexOfCou2 = input;
                        //on the attack stage
                        if (this.currentStage == 1)
                        {
                            //the selcted des can be attacked
                            if (at.destention_is_possible(this.inputInexOfCou2, this.inputInexOfCou))
                            {

                                //show text
                                if (this.AttackPanel.transform.Find("defText") != null)
                                {
                                    this.AttackPanel.transform.Find("defText").gameObject.SetActive(true);
                                }
                                //show buttons
                                if (this.AttackPanel.transform.Find("withdraw") != null)
                                {
                                    this.AttackPanel.transform.Find("withdraw").gameObject.SetActive(true);
                                }
                                if (this.AttackPanel.transform.Find("cont") != null)
                                {
                                    this.AttackPanel.transform.Find("cont").gameObject.SetActive(true);
                                }
                                this.counter++;
                            }

                        }
                        //the sourse location was chosen in the moving stage
                        else if (this.currentStage == 2)
                        {
                            if (this.mo.canChooseDes(this.inputInexOfCou2))
                            {
                                //show text
                                if (this.MovePanel.transform.Find("SliderText") != null)
                                {
                                    this.MovePanel.transform.Find("SliderText").gameObject.SetActive(true);
                                }
                                //show slider
                                if (this.MovePanel.transform.Find("Slider") != null)
                                {
                                    this.MovePanel.transform.Find("Slider").gameObject.SetActive(true);
                                }
                                //show button
                                if (this.MovePanel.transform.Find("Button") != null)
                                {
                                    this.MovePanel.transform.Find("Button").gameObject.SetActive(true);
                                }
                                this.counter++;
                                this.MovePanel.transform.Find("Slider").GetComponent<Slider>().maxValue = Global.map.map[this.inputInexOfCou].Info.Num_of_forces - 1;
                                this.MovePanel.transform.Find("Slider").GetComponent<Slider>().minValue = 1;
                            }
                        }
                    }
                    if (flag_next_player == true)
                    {
                        //move to next player
                        nextPlayer();
                    }

                }
            }

        }
 
    }

    /// <summary>
    /// move to next player
    /// </summary>
    private void nextPlayer()
    {
        if (this.currentPlayer >= this.numOfActivePlayer - 1)
        {
            this.currentPlayer = 0;
        }
        else
        {
            this.currentPlayer++;

        }
    }
    /// <summary>
    /// define all the variables 
    /// </summary>
    private void Start()
    {
        this.getInputCountry = int.MaxValue;
        this.currentPlayer = 0;
        this.currentStage = 3;

        this.forcesPanel = GameObject.Find("forcesPanel");
        //set the panels
        this.startPanel = GameObject.Find("StartPanel");
        if (this.startPanel != null)
        {
            this.startPanel.SetActive(false);
        }
        this.AttackPanel = GameObject.Find("AttackPanel");
        if (this.AttackPanel != null)
        {
            this.AttackPanel.SetActive(false);
        }
        this.ReiPanel = GameObject.Find("ReinforcementPanel");
        if (this.ReiPanel != null)
        {
            this.ReiPanel.SetActive(false);
        }
        this.MovePanel = GameObject.Find("MovingPanel");
        if (this.MovePanel != null)
        {
            this.MovePanel.SetActive(false);
        }
        this.WPanel = GameObject.Find("WinPanel");
        if (this.WPanel != null)
        {
            this.WPanel.SetActive(false);
        }
        this.st = null;
        this.rei = null;
        this.sm = null;
        this.at = null;
        this.mo = null;
        this.bot = null; 

    }
    /// <summary>
    /// send which country to choose territory function
    /// </summary>
    /// <param name="getInputCountry"></param>
    /// <returns>boolean value (if the input is good or the end of the stage return true)</returns>
    private bool chooseTer(int getInputCountry)
    {
        return this.st.choose_forces(getInputCountry, this.currentPlayer);
    }
    /// <summary>
    /// check if the current player lost
    /// </summary>
    /// <returns> boolean value (if lost return true)</returns>
    private bool lostCheck()
    {
        if (checkIfAive() == false)
        {
            players[this.currentPlayer] = false;
            this.alivePlayers--;
            //the player lost before he got to play
            if (this.counterStart > 0)
            {
                this.counterStart--;
            }
            return true;
        }
        return false;
    }
    /// <summary>
    /// check if player that is check lost
    /// </summary>
    /// <param name="enemyPlayer"> number of player</param>
    public void playerDefeatedDad(int enemyPlayer)
    {
        insteance.playerDefeated(enemyPlayer);
    }
    private void playerDefeated(int enemyPlayer)
    {
        //enemy was defeated 
        if (Global.map.how_many_forces((Global.Control)(enemyPlayer + 1)) <= 0)
        {
            this.alivePlayers--;
            this.players[enemyPlayer] = false;
        }
    }
    /// <summary>
    /// check if the current player still own territories
    /// </summary>
    /// <returns> boolean value (if he didnt lost already return true)</returns>
    private bool checkIfAive()
    {
        if (Global.map.how_many_forces((Global.Control)(this.currentPlayer + 1)) <= 0)
        {
            return false;
        }
        return true;



    }
    /// <summary>
    /// check if he is the only player left(he won) or if the player did his secret mission(he won)
    /// </summary>
    /// <returns> boolean value (if he won return true)</returns>
    public bool checkWinDad()
    {
        return insteance.checkWin();
    }
    private bool checkWin()
    {
        if (this.alivePlayers < 2 || this.sm.missionAccomplished(this.currentPlayer) == true)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// get from the user which players are chosen as bots and which are regular player.
    /// update the needed parts
    /// </summary>
    public void getWhoBotDad()
    {
        insteance.getWhoBot();
    }
    private void getWhoBot()
    {

        for (int i = 2; i <= this.numOfActivePlayer; i++)
        {
            string stringTog = "ToggleP" + i;
            if (this.forcesPanel.transform.Find(stringTog) != null)
            {
                this.playerAi[i - 1] = this.forcesPanel.transform.Find(stringTog).GetComponent<Toggle>().isOn;
            }
        }

        if (this.forcesPanel != null)
        {
            this.forcesPanel.SetActive(false);
        }

        if (this.startPanel != null)
        {
            this.startPanel.SetActive(true);
        }
        this.sm = new SecretMissions(this.numOfActivePlayer, Global.map);
        this.st = new Start_Game(this.numOfActivePlayer,Global.map);

    }

    /// <summary>
    /// get how many players from the user.
    /// updte the needed parts
    /// </summary>
    public void getNumOfPlayersDad()
    {
        insteance.getNumOfPlayers();
    }
    private void getNumOfPlayers()
    {

        //set number of players
        this.numOfActivePlayer = this.forcesPanel.transform.Find("numberP").GetComponent<Dropdown>().value + 1;
        this.counterStart = this.numOfActivePlayer - 1;
        this.alivePlayers = this.numOfActivePlayer;
        this.players = new bool[this.numOfActivePlayer];
        this.playerAi = new bool[this.numOfActivePlayer];
        for (int i = 0; i < this.players.Length; i++)
        {
            this.players[i] = true;
        }
        //choose who are bots
        if (this.forcesPanel.transform.Find("numberP") != null)
        {
            this.forcesPanel.transform.Find("numberP").gameObject.SetActive(false);
        }
        if (this.forcesPanel.transform.Find("numPText") != null)
        {
            this.forcesPanel.transform.Find("numPText").gameObject.SetActive(false);
        }
        if (this.forcesPanel.transform.Find("botsText") != null)
        {
            this.forcesPanel.transform.Find("botsText").gameObject.SetActive(true);
        }
        //depends on the selcted number of players

        for (int i = 2; i <= this.numOfActivePlayer; i++)
        {
            string stringtPlayer = "player" + i;
            string stringBot = "ToggleP" + i;
            if (this.forcesPanel.transform.Find(stringtPlayer) != null)
            {
                this.forcesPanel.transform.Find(stringtPlayer).gameObject.SetActive(true);
            }
            if (this.forcesPanel.transform.Find(stringBot) != null)
            {
                this.forcesPanel.transform.Find(stringBot).gameObject.SetActive(true);
            }
        }

        if (this.forcesPanel.transform.Find("selectBot") != null)
        {
            this.forcesPanel.transform.Find("selectBot").gameObject.SetActive(true);
        }

    }
    /// <summary>
    /// in the end of turn move the the next player.
    /// if the next player is bot play the moves of the bots
    /// </summary>
    private void moveNextStage()
    {

        //hide text
        //hide text
        if (this.MovePanel.transform.Find("first") != null)
        {
            this.MovePanel.transform.Find("first").gameObject.SetActive(false);
        }
        if (this.MovePanel.transform.Find("SliderText") != null)
        {
            this.MovePanel.transform.Find("SliderText").gameObject.SetActive(false);
        }
        //hide slider
        if (this.MovePanel.transform.Find("Slider") != null)
        {
            this.MovePanel.transform.Find("Slider").gameObject.SetActive(false);
        }
        //hide button
        if (this.MovePanel.transform.Find("Button") != null)
        {
            this.MovePanel.transform.Find("Button").gameObject.SetActive(false);
        }
        //hide the move Panel
        if (this.MovePanel != null)
        {
            this.MovePanel.SetActive(false);
        }


        nextPlayer();
        bool flag_find_player = false;
        while(flag_find_player == false)
        {
            //player is not alive
            if (players[this.currentPlayer] == false)
            {
                //if its after the beggining
                if (this.counterStart > 0)
                {
                    this.counterStart--;
                }
                    nextPlayer();
            }
            else
            {
                //player was elimnated
                if(lostCheck() == true)
                {
                    nextPlayer();
                }
                else
                {
                    //the player is bot

                    if (this.playerAi[this.currentPlayer] == true)
                    {
                        int won = 0;
                        //if its after the beggining and you should go straight to attack
                        if (this.counterStart > 0)
                        {

                            this.bot = new Ai(this.currentPlayer);
                            won = this.bot.playATurn(true,this);
                            //bot from attack
                            this.counterStart--;
                        }
                        else
                        {
                            this.bot = new Ai(this.currentPlayer);
                            won = this.bot.playATurn(false, this);
                            //bot from rein
                        }
                        //bot won
                        if (won == 1)
                        {
                            //show winner
                            if (this.WPanel != null)
                            {
                                this.WPanel.SetActive(true);
                            }
                            this.WPanel.transform.Find("winner").GetComponent<Text>().text = "the winner is " + whichPlayer();
                            flag_find_player = true;

                        }
                        else
                        {
                            nextPlayer();
                        }
                    }
                    //regular player
                    else
                    {
                        //if its after the beggining and you should go straight to attack
                        if (this.counterStart > 0)
                        {
                            //next stage
                            this.currentStage = 1;
                            this.at = new Attack(this.currentPlayer, Global.map);
                            //show the attackPanel
                            if (this.AttackPanel != null)
                            {
                                this.AttackPanel.SetActive(true);
                            }

                            this.counterStart--;
                        }
                        else
                        {
                            //next stage
                            this.currentStage = 0;
                            this.counter = 0;
                            this.rei = new reinforcement(this.currentPlayer, Global.map);
                            //show the reinforcement Panel
                            if (this.ReiPanel != null)
                            {
                                this.ReiPanel.SetActive(true);
                            }
                        }
                        //player founded play it
                        flag_find_player = true;

                    }
                }
            }
        }


    }
    /// <summary>
    /// get from user how many soldiers to reinforce
    /// </summary>
    public void onClickRein()
    {
        insteance.rein();
            
    }
    private void rein()
    {
        this.rei.choose_forces(this.inputInexOfCou, Mathf.RoundToInt(this.ReiPanel.transform.Find("Slider").GetComponent<Slider>().value));
        //hide
        if (this.ReiPanel.transform.Find("conf") != null)
        {
            this.ReiPanel.transform.Find("conf").gameObject.SetActive(false);
        }
        if (this.ReiPanel.transform.Find("SliderText") != null)
        {
            this.ReiPanel.transform.Find("SliderText").gameObject.SetActive(false);
        }
        if (this.ReiPanel.transform.Find("Slider") != null)
        {
            this.ReiPanel.transform.Find("Slider").gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// if player choose the skip move stage
    /// </summary>
    public void onClickMovingS()
    {
        insteance.MovingS();
    }
    private void MovingS()
    {
        moveNextStage();
    }
    /// <summary>
    /// get from player how many soldier to move to the wanted territory and go to next player
    /// </summary>
    public void onClickMoving()
    {
        insteance.moving();
    }
    private void moving()
    {
        int numbnerOfSoldiers = Mathf.RoundToInt(this.MovePanel.transform.Find("Slider").GetComponent<Slider>().value);
        mo.moveForces(this.inputInexOfCou2, this.inputInexOfCou, numbnerOfSoldiers);
        moveNextStage();
    }

    //after attacl confirm 
    /// <summary>
    /// get from player how many soldiers he want to move the the conquered territory
    /// </summary>
    public void onClickAfterAttack()
    {
        insteance.afterAtack();

    }
    private void afterAtack()
    {
        int numbnerOfSoldiers = Mathf.RoundToInt(this.AttackPanel.transform.Find("Slider").GetComponent<Slider>().value);
        Global.map.map[this.inputInexOfCou].Info.Num_of_forces -= numbnerOfSoldiers;
        Global.map.map[this.inputInexOfCou2].Info.Num_of_forces = numbnerOfSoldiers;
        hideAfterAttack();
    }
    /// <summary>
    /// player choose to try attacking, get the result and update
    /// </summary>
    public void onClickAttackA()
    {
        insteance.attackA();

    }
    private void attackA()
    {
        //attack
        int result;
        Global.Control defenderR = Global.map.map[this.inputInexOfCou2].Info.Ruled_by;
        result = this.at.attackOnce(this.inputInexOfCou, this.inputInexOfCou2);
        switch (result)
        {
            //won
            case 0:
                //check if a player lost 
                //not occupied free 
                if(defenderR != Global.Control.Free)
                {
                    if (Global.map.how_many_forces(defenderR) <= 0)
                    {
                        players[Global.indexOfControl(defenderR)] = false;
                        this.alivePlayers--;
                    }
                }
                
                if (checkWin()==true)
                {
                    //hide the attackPanel
                    if (this.AttackPanel != null)
                    {
                        this.AttackPanel.SetActive(false);
                    }
                    //show winner
                    if (this.WPanel != null)
                    {
                        this.WPanel.SetActive(true);
                    }
                    this.WPanel.transform.Find("winner").GetComponent<Text>().text = "the winner is " + whichPlayer();
                }
                attackClose();
                showAfterAttack();
                this.AttackPanel.transform.Find("Slider").GetComponent<Slider>().maxValue = Global.map.map[this.inputInexOfCou].Info.Num_of_forces - 1;
                this.AttackPanel.transform.Find("Slider").GetComponent<Slider>().minValue = 1;

                break;
            //lost
            case 1:
                attackClose();
                break;
                //allow to attack again

        }
    }
    /// <summary>
    /// player chose to withdraw from battle 
    /// </summary>
    public void onClickAttackW()
    {
        insteance.attackW();
    }
    private void attackW()
    {
        attackClose();
    }
    /// <summary>
    /// stop the attack stage and start the moving stage
    /// </summary>
    public void onClickAttackE()
    {
        insteance.attackE();
    }
    private void attackE()
    {
        attackClose();
        //hide the attackPanel
        if (this.AttackPanel != null)
        {
            this.AttackPanel.SetActive(false);
        }
        //next stage
        this.currentStage = 2;
        this.mo = new moving(this.currentPlayer, Global.map);
        //show the movekPanel
        if (this.MovePanel != null)
        {
            this.MovePanel.SetActive(true);
        }


    }
    /// <summary>
    /// close all the open objects from the attack stage
    /// </summary>
    public void attackClose()
    {
        //hide texts
        if (this.AttackPanel.transform.Find("ataText") != null)
        {
            this.AttackPanel.transform.Find("ataText").gameObject.SetActive(false);
        }
        if (this.AttackPanel.transform.Find("defText") != null)
        {
            this.AttackPanel.transform.Find("defText").gameObject.SetActive(false);
        }
        //show buttons
        if (this.AttackPanel.transform.Find("withdraw") != null)
        {
            this.AttackPanel.transform.Find("withdraw").gameObject.SetActive(false);
        }
        if (this.AttackPanel.transform.Find("cont") != null)
        {
            this.AttackPanel.transform.Find("cont").gameObject.SetActive(false);
        }
        this.counter = 0;
    }
    /// <summary>
    /// show the start game objects of the attack stage
    /// </summary>
    public void showAfterAttack()
    {
        //show
        if (this.AttackPanel.transform.Find("SliderText") != null)
        {
            this.AttackPanel.transform.Find("SliderText").gameObject.SetActive(true);

        }
        if (this.AttackPanel.transform.Find("Slider") != null)
        {
            this.AttackPanel.transform.Find("Slider").gameObject.SetActive(true);

        }
        if (this.AttackPanel.transform.Find("SliderText") != null)
        {
            this.AttackPanel.transform.Find("SliderText").gameObject.SetActive(true);

        }
        if (this.AttackPanel.transform.Find("confirmButton") != null)
        {
            this.AttackPanel.transform.Find("confirmButton").gameObject.SetActive(true);

        }
    }
    /// <summary>
    /// hide the start game objects of the attack stage
    /// </summary>
    public void hideAfterAttack()
    {
        //show
        if (this.AttackPanel.transform.Find("SliderText") != null)
        {
            this.AttackPanel.transform.Find("SliderText").gameObject.SetActive(false);

        }
        if (this.AttackPanel.transform.Find("Slider") != null)
        {
            this.AttackPanel.transform.Find("Slider").gameObject.SetActive(false);

        }
        if (this.AttackPanel.transform.Find("SliderText") != null)
        {
            this.AttackPanel.transform.Find("SliderText").gameObject.SetActive(false);

        }
        if (this.AttackPanel.transform.Find("confirmButton") != null)
        {
            this.AttackPanel.transform.Find("confirmButton").gameObject.SetActive(false);

        }
    }
    /// <summary>
    /// show during the game what is the current stage and the player
    /// show during the start stage and the reinforsment how many soldier left to deploy
    /// show dring the attack stage how many soldiers the attacker and the defender have
    /// </summary>
    private void Update()
    {
        GameObject.Find("curP").GetComponent<Text>().text = (whichPlayer() + " turn");
        GameObject.Find("curPart").GetComponent<Text>().text = whichPart();
        //start of the game stage

            if (this.startPanel != null && currentStage == 3)
            {

                if (this.startPanel.transform.Find("ForcesText") != null && this.st != null)
                {
                    this.startPanel.transform.Find("ForcesText").GetComponent<Text>().text = "number of forces left foe deploy:" + st.getNumberOfForces(this.currentPlayer);
                }

            }
            //reinforsment stage
            if (this.ReiPanel != null && currentStage == 0)
            {
                if (this.ReiPanel.transform.Find("numberOfS") != null)
                {
                    this.ReiPanel.transform.Find("numberOfS").GetComponent<Text>().text = "number of forces left foe deploy:" + rei.getNumberOfForces();
                }
            }
            //update the texts in the attack stage
            if (this.AttackPanel != null && currentStage == 1)
            {
                if (this.AttackPanel.transform.Find("ataText") != null)
                {
                    this.AttackPanel.transform.Find("ataText").GetComponent<Text>().text = "attacker have: " + Global.map.map[this.inputInexOfCou].Info.Num_of_forces + " soldiers left";
                }
                if (this.AttackPanel.transform.Find("defText") != null)
                {
                    this.AttackPanel.transform.Find("defText").GetComponent<Text>().text = "defender have: " + Global.map.map[this.inputInexOfCou2].Info.Num_of_forces + " soldiers left";
                }
            }

    }
    /// <summary>
    ///  return the text from the int
    /// </summary>
    /// <returns>a string of the name of the player</returns>
    private string whichPlayer()
    {
        switch (this.currentPlayer)
        {
            case 0: return "player 1";
            case 1: return "player 2";
            case 2: return "player 3";
            case 3: return "player 4";
            case 4: return "player 5";
            case 5: return "player 6";

        }
        return "";
    }
    /// <summary>
    /// return the text from the int
    /// </summary>
    /// <returns>a string of the name of the stage</returns>
    private string whichPart()
    {
        switch (this.currentStage)
        {
            case 0: return "reinforcment";
            case 1: return "attack";
            case 2: return "move forces";
            case 3: return "choose forces";
        }
        return "";
    }

}
