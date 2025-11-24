import { player1, player2 } from './Fighter.js';
import { background, shop } from './Sprite.js';
import { loadKeyDownEvents, loadKeyUpEvents } from './Keys.js';

const canvas = document.querySelector('canvas');
const c = canvas.getContext('2d');
let timer = 30; //Game timer - TODO: adjustable timer
let timerID;   //Use to clearTimeout.
let gameEnded = false; //Flag to determine whether game ended or not.
//TODO: Proper Restart Button

loadKeyDownEvents(player1, player2); //Load P1 and P2 KeyDown events.
loadKeyUpEvents(player1, player2);  //Load P1 and P2 KeyUp events.

const onePlayer = document.getElementById("1P");
const twoPlayers = document.getElementById("2P");

onePlayer.addEventListener("click", () => {
    intervalBot();
    startGame();
});

twoPlayers.addEventListener("click", () => {
    startGame();
});


//Main function to start the game after the menu
function startGame() {
    document.getElementById('menu').style.display = "none"; //Hide menu
    c.fillRect(0, 0, canvas.width, canvas.height);  //Simulate loading screen with black screen
    setTimeout(() => {
        animate();      //Start the animation function
        decreaseTimer();//Start timer countdown
        document.getElementById('hud').style.display = "flex"; //Show HUD
    }, 1000) //Simulate 1 sec loading time
}


//Timer function. 
function decreaseTimer() {
    if (timer > 0) {
        timerID = setTimeout(decreaseTimer, 1000); //Call this function every 1 second
        timer--;
        document.querySelector('#timebox').innerHTML = timer; //Update the timer box
    } else  //timer runs out, determine winner by health
        determineWinner({ player1, player2, timerID });
}

//Animate the sprites
function animate() {
    window.requestAnimationFrame(animate); //Recursive function
    background.update();
    shop.update();
    update(player1);
    update(player2);
    //Reset the x velocity for both players to make sure this is the default state.
    player1.velocity.x = 0;
    player2.velocity.x = 0;

    //default state 
    if (!player1.movement() && !player1.isAttacking && !player1.isHit) {
        player1.switchSprite('idle');
    }

    if (!player2.movement() && !player2.isAttacking && !player2.isHit) {
        player2.switchSprite('idle');
    }

    player1.attack(player2);
    player2.attack(player1);

    if (!gameEnded) {
        if (player2.health <= 0 || player1.health <= 0)
            determineWinner({ player1, player2, timerID });
    }
}

//Bot Behaviors Management - Completely random
function intervalBot() {
    setInterval(botMove, 500); //Evaluate for moving every 0.5 sec
    setInterval(botAttack, 500); //Evalute for attacking every 0.5 sec
}

//Bot moves randomly: 45% chance of forward or backwards, 10% idle.
//The period of time for the movement is also random.
function botMove() {
    let randomFloat = Math.random();

    if (randomFloat < 0.45) {
        window.dispatchEvent(new KeyboardEvent('keydown', { 'key': 'ArrowLeft' }))
        setTimeout(() => {
            window.dispatchEvent(new KeyboardEvent('keyup', { 'key': 'ArrowLeft' }))
        }, randomFloat * 3000); //Random time up to 1.5 sec
    }else if (randomFloat < 0.85) {
        window.dispatchEvent(new KeyboardEvent('keydown', { 'key': 'ArrowRight' }))
        setTimeout(() => {
            window.dispatchEvent(new KeyboardEvent('keyup', { 'key': 'ArrowRight' }))
        }, (randomFloat - 0.45) * 3000); //Random time up to 1.5 sec
    }
}

//Bot attacks the player if in range and his cooldown is available
function botAttack() {
    //Check if in range
    if (player2.attackCooldown && player2.isHitting(player1)) {
        player2.isAttacking = true;
        player2.attack(player1);
        setTimeout(() => { player2.isAttacking = false; }, 1000)
    }
}

//Make key presses work only when fighter is alive
function update(fighter) {
    if (fighter.health > 0) { //Allow movement and attacks only if alive
        fighter.update();
    } else {                //If dead, only draw on screen, movement is restricted
        fighter.animateFrames();
        fighter.draw();
    }
}

function determineWinner({ player1, player2, timerID }) {
    clearTimeout(timerID); //Stop the timer
    gameEnded = true;
    document.querySelector('#result').style.display = 'flex' //make the result visible
    if (player1.health === player2.health){
        document.querySelector('#result').innerHTML = 'DRAW';
    } else if (player1.health > player2.health) {
        document.querySelector('#result').innerHTML = 'PLAYER 1 WIN';
        player2.switchSprite('death');
    } else {
        document.querySelector('#result').innerHTML = 'PLAYER 2 WIN';
        player1.switchSprite('death');
    }

    setTimeout(() => {
        location.reload();
    }, 2000)
}