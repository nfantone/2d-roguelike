using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MovingObject
{
    const int DEFAULT_WALL_DAMAGE = 1;
    const int DEFAULT_FOOD_POINTS = 10;
    const int DEFAULT_SODA_POINTS = 10;
    const float DEFAULT_RESTART_LEVEL_DELAY = 1.0f;
    const string EXIT_TAG = "Exit";
    const string FOOD_TAG = "Food";
    const string SODA_TAG = "Soda";
    const string FOOD_GAIN_TEXT_FORMAT = "+ {0} Food: {1}";
    const string FOOD_LOSS_TEXT_FORMAT = "- {0} Food: {1}";
    const string FOOD_TEXT = "Food ";

    public int wallDamage = DEFAULT_WALL_DAMAGE;
    public int pointsPerFood = DEFAULT_FOOD_POINTS;
    public int pointsPerSoda = DEFAULT_SODA_POINTS;
    public float restartLevelDelay = DEFAULT_RESTART_LEVEL_DELAY;
    public Text foodText;
    public AudioClip moveSound1;
		public AudioClip moveSound2;
		public AudioClip eatSound1;
		public AudioClip eatSound2;
		public AudioClip drinkSound1;
		public AudioClip drinkSound2;
		public AudioClip gameOverSound;

    Animator animator;
    int food;

    // Start overrides the Start function of MovingObject
    protected override void Start()
    {
        //Get a component reference to the Player's animator component
        animator = GetComponent<Animator>();

        //Get the current food point total stored in GameManager.instance between levels.
        food = GameManager.instance.playerFoodPoints;
        foodText.text = FOOD_TEXT + food;
        //Call the Start function of the MovingObject base class.
        base.Start();
    }

    // This function is called when the behaviour becomes disabled or inactive.
    private void OnDisable()
    {
        // When Player object is disabled, store the current local food total in the GameManager so it can be re-loaded in next level.
        GameManager.instance.playerFoodPoints = food;
    }


    private void Update()
    {
        // If it's not the player's turn, exit the function.
        if (!GameManager.instance.playersTurn)
        {
            return;
        }

        int horizontal = 0;     //Used to store the horizontal move direction.
        int vertical = 0;       //Used to store the vertical move direction.

        // Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));

        // Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        //Check if moving horizontally, if so set vertical to zero.
        if (horizontal != 0)
        {
            vertical = 0;
        }

        //Check if we have a non-zero value for horizontal or vertical
        if (horizontal != 0 || vertical != 0)
        {
            // Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
            // Pass in horizontal and vertical as parameters to specify the direction to move Player in.
            AttemptMove<WallController>(horizontal, vertical);
        }
    }

    //AttemptMove overrides the AttemptMove function in the base class MovingObject
    //AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
    protected override void AttemptMove<T>(int x, int y)
    {
        // Every time player moves, subtract from food points total.
        food--;
        foodText.text = FOOD_TEXT + food;

        // Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
        base.AttemptMove<T>(x, y);

        // Hit allows us to reference the result of the Linecast done in Move.
        RaycastHit2D hit;

        // If Move returns true, meaning Player was able to move into an empty space.
        if (Move(x, y, out hit))
        {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }

        // Since the player has moved and lost food points, check if the game has ended.
        CheckIfGameOver();

        // Set the playersTurn boolean of GameManager to false now that players turn is over.
        GameManager.instance.playersTurn = false;
    }

    // OnCantMove overrides the abstract function OnCantMove in MovingObject.
    // It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
    protected override void OnCantMove<T>(T component)
    {
        // Set hitWall to equal the component passed in as a parameter.
        WallController hitWall = component as WallController;

        // Call the DamageWall function of the Wall we are hitting.
        hitWall.DamageWall(wallDamage);

        // Set the attack trigger of the player's animation controller in order to play the player's attack animation.
        animator.SetTrigger("playerChop");
    }

    // OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the tag of the trigger collided with is Exit.
        if (other.tag == EXIT_TAG)
        {
            // Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
            Invoke("Restart", restartLevelDelay);
            // Disable the player object since level is over.
            enabled = false;
        }
        else if (other.tag == FOOD_TAG)
        {
            // Add pointsPerFood to the players current food total.
            food += pointsPerFood;
            foodText.text = string.Format(FOOD_GAIN_TEXT_FORMAT, pointsPerFood, food);
						SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
            // Disable the food object the player collided with.
            other.gameObject.SetActive(false);
        }
        else if (other.tag == SODA_TAG)
        {
            // Add pointsPerSoda to players food points total
            food += pointsPerSoda;
            foodText.text = string.Format(FOOD_GAIN_TEXT_FORMAT, pointsPerSoda, food);
						SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
            //D isable the soda object the player collided with.
            other.gameObject.SetActive(false);
        }
    }

    //Restart reloads the scene when called.
    private void Restart()
    {
        // Load the last scene loaded, in this case Main, the only scene in the game.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // LoseFood is called when an enemy attacks the player.
    // It takes a parameter loss which specifies how many points to lose.
    public void LoseFood(int loss)
    {
        // Set the trigger for the player animator to transition to the playerHit animation.
        animator.SetTrigger("playerHit");

        // Subtract lost food points from the players total.
        food -= loss;
        foodText.text = string.Format(FOOD_LOSS_TEXT_FORMAT, loss, food);
        // Check to see if game has ended.
        CheckIfGameOver();
    }


    // CheckIfGameOver checks if the player is out of food points and if so, ends the game.
    private void CheckIfGameOver()
    {
        //Check if food point total is less than or equal to zero.
        if (food <= 0)
        {
            SoundManager.instance.musicSource.Stop();
						SoundManager.instance.PlaySingle(gameOverSound);
            //Call the GameOver function of GameManager.
            GameManager.instance.GameOver();
        }
    }
}
