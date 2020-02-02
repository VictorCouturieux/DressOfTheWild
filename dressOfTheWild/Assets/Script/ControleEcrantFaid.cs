using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;
using System.Collections;

public class ControleEcrantFaid : MonoBehaviour {
    public Image imagetofaid;
    
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);
        
        // Detect if a button was pressed this frame
        if (prevState.Buttons.Start == ButtonState.Released && state.Buttons.Start == ButtonState.Pressed) {
            Debug.Log("toto");
            StartCoroutine("CinematicEndWin");
        }
    }

    IEnumerator CinematicEndWin() {
        Debug.Log("titi");
        float lerp = 0;
        while (lerp < 1) {
            imagetofaid.color *= 1-lerp;
            lerp += Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }
    }

}
