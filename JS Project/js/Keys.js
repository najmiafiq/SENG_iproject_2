export const keys = {
    // Player 1.
    a: {
        pressed: false
    },
    d: {
        pressed: false
    },
    w: {    // Since there is gravity, when P1 jumps it will slowly start to fall.
        pressed: false,
    },
    // Player 2/AI
    ArrowLeft: {
        pressed: false
    },
    ArrowRight: {
        pressed: false
    },
    ArrowUp: {    // Since there is gravity, when P2 jumps it will slowly start to fall.
        pressed: false,
    }
}

export function loadKeyDownEvents(player1, player2) {
    // Whenever a key is pressed.
    window.addEventListener('keydown', (event) => {
        event.preventDefault(); // Prevent any keys' default behaviour i.e using the arrow keys to navigate the page.
        switch (event.key) {
            // Player 1 keys.
            case 'd':
                player1.keys.d.pressed = true;
                player1.lastKey = 'd';
                break;
            case 'a':
                player1.keys.a.pressed = true;
                player1.lastKey = 'a';
                break;
            case 'w':
                if (!player1.inTheAir) {  // Can only jump if it's not in the air.
                    player1.velocity.y = -player1.moveFactor * 4;
                }
                break;
            case ' ':   // P1 attack with space bar.
                player1.isAttacking = true;
                player1.lastKey = ' ';
                break;


            // P2 keys.
            case 'ArrowRight':
                player2.keys.ArrowRight.pressed = true;
                player2.lastKey = 'ArrowRight';
                break;
            case 'ArrowLeft':
                player2.keys.ArrowLeft.pressed = true;
                player2.lastKey = 'ArrowLeft';
                break;
            case 'ArrowUp':
                if (!player2.inTheAir) {   // Only can jump if it's not in the air.
                    player2.velocity.y = -player2.moveFactor * 4;
                }
                break;
            case 'Alt': // P2 attack with control key.
                player2.isAttacking = true;
                player2.lastKey = 'Alt'
                break;
        }
    });
}

export function loadKeyUpEvents(player1, player2) {
    // Whenever a key is lifted.
    window.addEventListener('keyup', (event) => {
        switch (event.key) {
            // Player 1.
            case 'd':
                player1.keys.d.pressed = false;
                break;
            case 'a':
                player1.keys.a.pressed = false;
                break;
            // Player 2
            case 'ArrowRight':
                player2.keys.ArrowRight.pressed = false;
                break;
            case 'ArrowLeft':
                player2.keys.ArrowLeft.pressed = false;
                break;
        }
    });
}