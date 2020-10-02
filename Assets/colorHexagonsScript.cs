using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class colorHexagonsScript : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;

    public KMSelectable[] hexes;
    public Material[] colors; //K,  B,  G,  C,  R,  M,  Y,  W
    public TextMesh[] texts;
    public Color[] textColors; //K, W
    public GameObject backing;
    public Material backWithTint;
    public Material[] colorChannels;
    public Renderer ren;
    public Renderer[] rens;

    private string diagramColors =
    ".,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,"+
    ",.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,."+
    ".,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,"+
    ",.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,."+
    ".,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,"+
    ",.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,."+
    ".,.,.,.,.,.,.,.,.,.,.,.,.,C,B,Y,W,R,K,G,R,K,B,M,M,C,Y,.,.,.,.,.,.,.,.,.,.,.,.,"+
    ",.,.,.,.,.,.,.,.,.,.,.,.,W,R,W,C,R,B,R,R,M,R,K,K,B,B,B,.,.,.,.,.,.,.,.,.,.,.,."+
    ".,.,.,.,.,.,.,.,.,.,.,.,R,M,K,Y,R,G,W,M,C,W,B,B,Y,G,W,K,.,.,.,.,.,.,.,.,.,.,.,"+
    ",.,.,.,.,.,.,.,.,.,.,.,R,G,B,B,Y,C,Y,G,Y,M,M,Y,R,G,K,R,R,.,.,.,.,.,.,.,.,.,.,."+
    ".,.,.,.,.,.,.,.,.,.,.,B,W,R,W,B,R,M,M,G,B,R,C,B,G,G,R,M,B,.,.,.,.,.,.,.,.,.,.,"+
    ",.,.,.,.,.,.,.,.,.,.,R,W,K,C,K,Y,W,B,G,K,M,B,Y,G,G,G,M,B,C,.,.,.,.,.,.,.,.,.,."+
    ".,.,.,.,.,.,.,.,.,.,C,G,W,R,C,M,M,C,Y,Y,C,B,C,W,G,K,R,Y,B,G,.,.,.,.,.,.,.,.,.,"+
    ",.,.,.,.,.,.,.,.,.,Y,M,B,R,G,Y,M,R,G,M,K,B,C,Y,W,Y,M,W,W,K,R,.,.,.,.,.,.,.,.,."+
    ".,.,.,.,.,.,.,.,.,M,Y,Y,G,M,K,B,W,K,B,W,W,Y,K,Y,M,C,K,G,B,G,G,.,.,.,.,.,.,.,.,"+
    ",.,.,.,.,.,.,.,.,R,R,M,K,M,R,C,G,M,C,W,G,K,B,G,B,R,B,G,R,B,K,M,.,.,.,.,.,.,.,."+
    ".,.,.,.,.,.,.,.,Y,K,W,G,K,W,K,G,C,Y,Y,W,M,W,K,B,R,K,W,B,G,B,K,M,.,.,.,.,.,.,.,"+
    ",.,.,.,.,.,.,.,M,R,W,R,Y,K,R,M,C,W,C,B,Y,Y,K,C,Y,M,M,C,G,G,M,C,C,.,.,.,.,.,.,."+
    ".,.,.,.,.,.,.,Y,W,R,K,R,G,R,R,G,Y,W,B,G,G,M,M,M,R,R,Y,M,R,C,B,C,W,.,.,.,.,.,.,"+
    ",.,.,.,.,.,.,Y,C,C,K,B,W,W,K,Y,C,G,Y,B,C,K,G,R,W,G,W,M,K,B,G,K,B,B,.,.,.,.,.,."+
    ".,.,.,.,.,.,.,W,M,W,R,G,Y,W,B,R,M,K,K,C,B,R,C,Y,W,C,C,Y,R,C,B,G,Y,.,.,.,.,.,.,"+
    ",.,.,.,.,.,.,.,Y,B,C,B,B,Y,Y,C,C,W,B,K,B,W,M,W,M,Y,C,W,B,K,G,R,M,.,.,.,.,.,.,."+
    ".,.,.,.,.,.,.,.,M,B,M,Y,K,K,M,Y,G,B,G,C,M,R,G,B,B,K,R,B,W,M,B,B,.,.,.,.,.,.,.,"+
    ",.,.,.,.,.,.,.,.,B,B,K,Y,B,W,B,G,M,K,R,R,G,C,G,K,C,Y,B,Y,W,C,M,.,.,.,.,.,.,.,."+
    ".,.,.,.,.,.,.,.,.,R,M,G,G,Y,Y,K,G,C,W,W,Y,G,K,C,M,K,Y,C,M,Y,R,.,.,.,.,.,.,.,.,"+
    ",.,.,.,.,.,.,.,.,.,K,G,Y,M,W,M,B,G,Y,K,M,C,R,W,Y,M,R,R,K,R,M,.,.,.,.,.,.,.,.,."+
    ".,.,.,.,.,.,.,.,.,.,G,Y,C,Y,M,W,K,Y,C,C,M,K,M,C,Y,B,R,M,B,M,.,.,.,.,.,.,.,.,.,"+
    ",.,.,.,.,.,.,.,.,.,.,G,M,G,Y,K,R,R,G,K,C,W,B,Y,W,M,R,G,R,K,.,.,.,.,.,.,.,.,.,."+
    ".,.,.,.,.,.,.,.,.,.,.,W,K,W,Y,C,B,G,Y,K,Y,R,C,W,G,G,B,K,R,.,.,.,.,.,.,.,.,.,.,"+
    ",.,.,.,.,.,.,.,.,.,.,.,K,Y,B,Y,M,B,C,W,R,Y,C,K,Y,G,R,C,Y,.,.,.,.,.,.,.,.,.,.,."+
    ".,.,.,.,.,.,.,.,.,.,.,.,Y,W,R,C,C,C,C,K,R,C,K,K,B,R,W,W,.,.,.,.,.,.,.,.,.,.,.,"+
    ",.,.,.,.,.,.,.,.,.,.,.,.,Y,Y,W,G,C,B,B,Y,C,W,Y,K,W,C,M,.,.,.,.,.,.,.,.,.,.,.,."+
    ".,.,.,.,.,.,.,.,.,.,.,.,.,M,Y,M,R,B,R,M,R,C,W,K,K,B,M,.,.,.,.,.,.,.,.,.,.,.,.,"+
    ".,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,"+
    ",.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,."+
    ".,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,"+
    ",.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,."+
    ".,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,"+
    ",.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.";

    private string diagramDigits =
    ".,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,"+
	",.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,."+
	".,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,"+
	",.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,."+
	".,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,"+
	",.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,."+
	".,.,.,.,.,.,.,.,.,.,.,.,.,F,7,E,D,0,D,F,6,7,A,F,E,3,6,.,.,.,.,.,.,.,.,.,.,.,.,"+
	",.,.,.,.,.,.,.,.,.,.,.,.,9,7,1,D,B,0,7,4,8,C,5,1,B,7,2,.,.,.,.,.,.,.,.,.,.,.,."+
	".,.,.,.,.,.,.,.,.,.,.,.,B,7,3,B,4,4,A,D,8,7,E,B,7,1,2,C,.,.,.,.,.,.,.,.,.,.,.,"+
	",.,.,.,.,.,.,.,.,.,.,.,A,7,7,F,9,8,E,F,9,C,2,3,F,B,5,E,6,.,.,.,.,.,.,.,.,.,.,."+
	".,.,.,.,.,.,.,.,.,.,.,0,2,6,A,C,5,9,6,E,8,3,0,D,2,B,1,3,0,.,.,.,.,.,.,.,.,.,.,"+
	",.,.,.,.,.,.,.,.,.,.,2,1,D,C,7,8,B,B,F,6,A,9,B,4,8,2,D,2,9,.,.,.,.,.,.,.,.,.,."+
	".,.,.,.,.,.,.,.,.,.,3,7,7,A,1,C,A,9,D,7,D,6,B,9,0,E,2,C,7,D,.,.,.,.,.,.,.,.,.,"+
	",.,.,.,.,.,.,.,.,.,D,A,9,0,7,E,8,7,3,5,F,4,2,7,2,7,7,F,9,5,0,.,.,.,.,.,.,.,.,."+
	".,.,.,.,.,.,.,.,.,2,7,5,3,6,D,1,4,1,6,3,5,3,3,4,A,B,1,E,D,F,7,.,.,.,.,.,.,.,.,"+
	",.,.,.,.,.,.,.,.,6,1,E,4,5,9,1,F,D,B,C,7,6,6,1,0,3,E,1,B,E,B,D,.,.,.,.,.,.,.,."+
	".,.,.,.,.,.,.,.,9,5,E,F,0,8,9,2,A,D,0,7,5,9,9,9,C,6,5,E,9,2,A,D,.,.,.,.,.,.,.,"+
	",.,.,.,.,.,.,.,A,5,9,C,F,E,C,1,B,E,8,5,9,5,B,E,6,6,C,6,7,4,6,1,1,.,.,.,.,.,.,."+
	".,.,.,.,.,.,.,D,6,4,9,0,0,B,B,2,3,E,A,5,5,6,8,9,A,B,8,1,6,5,7,6,9,.,.,.,.,.,.,"+
	",.,.,.,.,.,.,6,6,F,5,C,F,1,3,5,B,A,1,4,6,8,5,6,6,2,4,D,3,B,F,C,B,F,.,.,.,.,.,."+
	".,.,.,.,.,.,.,2,D,C,F,C,0,E,C,0,9,B,2,5,3,8,6,3,8,0,4,F,8,C,0,C,F,.,.,.,.,.,.,"+
	",.,.,.,.,.,.,.,C,D,C,0,B,A,0,C,1,0,8,8,7,D,8,D,2,6,9,B,0,8,8,4,2,.,.,.,.,.,.,."+
	".,.,.,.,.,.,.,.,4,7,9,9,B,2,5,6,B,0,C,E,F,B,7,8,8,D,B,4,C,5,F,7,.,.,.,.,.,.,.,"+
	",.,.,.,.,.,.,.,.,6,8,F,F,7,C,1,F,D,1,5,2,8,A,0,C,4,2,0,D,2,F,D,.,.,.,.,.,.,.,."+
	".,.,.,.,.,.,.,.,.,D,1,6,9,2,5,A,E,A,F,6,6,8,0,3,2,5,5,B,4,E,6,.,.,.,.,.,.,.,.,"+
	",.,.,.,.,.,.,.,.,.,9,A,7,6,7,0,5,7,0,4,9,E,5,9,7,E,1,6,E,D,D,.,.,.,.,.,.,.,.,."+
	".,.,.,.,.,.,.,.,.,.,1,8,D,4,1,2,3,6,0,2,1,C,B,0,9,C,D,4,8,6,.,.,.,.,.,.,.,.,.,"+
	",.,.,.,.,.,.,.,.,.,.,2,5,F,B,8,0,E,B,1,3,E,5,D,C,3,8,8,4,2,.,.,.,.,.,.,.,.,.,."+
	".,.,.,.,.,.,.,.,.,.,.,C,5,0,7,4,E,8,4,A,B,6,E,F,6,D,3,C,8,.,.,.,.,.,.,.,.,.,.,"+
	",.,.,.,.,.,.,.,.,.,.,.,5,8,5,1,9,B,3,C,3,E,C,B,6,1,9,1,7,.,.,.,.,.,.,.,.,.,.,."+
	".,.,.,.,.,.,.,.,.,.,.,.,D,5,4,3,5,C,3,A,1,4,2,A,4,1,8,B,.,.,.,.,.,.,.,.,.,.,.,"+
	",.,.,.,.,.,.,.,.,.,.,.,.,0,6,F,0,E,6,D,8,1,A,0,A,D,D,8,.,.,.,.,.,.,.,.,.,.,.,."+
	".,.,.,.,.,.,.,.,.,.,.,.,.,8,E,E,C,F,D,2,1,0,3,0,3,B,5,.,.,.,.,.,.,.,.,.,.,.,.,"+
	".,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,"+
	",.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,."+
	".,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,"+
	",.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,."+
	".,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,"+
	",.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.";

    private int startingLocation = -1;
    private List<int> directions = new List<int> { -79, -77, 2, 79, 77, -2 }; //NW, NE, E, SE, SW, W
    private int chosenDirection = -1;
    private List<int> allLocations = new List<int>{};
    private List<int> colorsOnHexes = new List<int>{};
    private int correctHex = -1;
    private int randomColor = -1;
    private List<string> colorNames = new List<string>{ "Black", "Blue", "Green", "Cyan", "Red", "Magenta", "Yellow", "White" };
    private List<string> dirNames = new List<string>{ "North-west", "North-east", "East", "South-east", "South-west", "West" };

    private string hexadecimal = "0123456789ABCDEF";
    private int red = 0;
    private int green = 0;
    private int blue = 0;

    //Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake () {
        moduleId = moduleIdCounter++;

        foreach (KMSelectable hex in hexes) {
            hex.OnInteract += delegate () { hexPress(hex); return false; };
        }
    }

    // Use this for initialization
    void Start () {
        PickNewStart:
        startingLocation = UnityEngine.Random.Range(0, diagramColors.Length);
        if (diagramColors[startingLocation] == '.' || diagramColors[startingLocation] == ',') {
            goto PickNewStart;
        } else {
            PickNewDirection:
            chosenDirection = UnityEngine.Random.Range(0, 6);
            allLocations.Add(startingLocation);
            allLocations.Add(startingLocation + directions[chosenDirection]);
            allLocations.Add(allLocations[1] + directions[chosenDirection]);
            allLocations.Add(allLocations[2] + directions[chosenDirection]);
            allLocations.Add(allLocations[3] + directions[chosenDirection]);
            allLocations.Add(allLocations[4] + directions[chosenDirection]);
            if (diagramColors[allLocations[5]] == '.' || diagramColors[allLocations[5]] == ',') {
                allLocations.Clear();
                goto PickNewDirection;
            } else {
                correctHex = UnityEngine.Random.Range(0, 6);
                for (int i = 0; i < 6; i++) {
                    texts[i].text = diagramDigits[allLocations[i]].ToString();
                    if (i == correctHex) {
                        switch (diagramColors[allLocations[i]]) {
                            case 'K': colorsOnHexes.Add(0); break;
                            case 'B': colorsOnHexes.Add(1); break;
                            case 'G': colorsOnHexes.Add(2); break;
                            case 'C': colorsOnHexes.Add(3); break;
                            case 'R': colorsOnHexes.Add(4); break;
                            case 'M': colorsOnHexes.Add(5); break;
                            case 'Y': colorsOnHexes.Add(6); break;
                            case 'W': colorsOnHexes.Add(7); break;
                            default: Debug.Log("Things screwed up " + diagramColors[allLocations[i]]); break;
                        }
                    } else {
                        randomColor = UnityEngine.Random.Range(0, 8);
                        switch (diagramColors[allLocations[i]]) {
                            case 'K': while (randomColor == 0) {randomColor = UnityEngine.Random.Range(0, 8);} colorsOnHexes.Add(randomColor); break;
                            case 'B': while (randomColor == 1) {randomColor = UnityEngine.Random.Range(0, 8);} colorsOnHexes.Add(randomColor); break;
                            case 'G': while (randomColor == 2) {randomColor = UnityEngine.Random.Range(0, 8);} colorsOnHexes.Add(randomColor); break;
                            case 'C': while (randomColor == 3) {randomColor = UnityEngine.Random.Range(0, 8);} colorsOnHexes.Add(randomColor); break;
                            case 'R': while (randomColor == 4) {randomColor = UnityEngine.Random.Range(0, 8);} colorsOnHexes.Add(randomColor); break;
                            case 'M': while (randomColor == 5) {randomColor = UnityEngine.Random.Range(0, 8);} colorsOnHexes.Add(randomColor); break;
                            case 'Y': while (randomColor == 6) {randomColor = UnityEngine.Random.Range(0, 8);} colorsOnHexes.Add(randomColor); break;
                            case 'W': while (randomColor == 7) {randomColor = UnityEngine.Random.Range(0, 8);} colorsOnHexes.Add(randomColor); break;
                            default: Debug.Log("Things screwed up " + diagramColors[allLocations[i]]); break;
                        }
                    }
                    hexes[i].GetComponent<MeshRenderer>().material = colors[colorsOnHexes[i]];
                    if (colorsOnHexes[i] == 0 || colorsOnHexes[i] == 1) {
                        texts[i].color = textColors[1];
                    } else {
                        texts[i].color = textColors[0];
                    }
                    Debug.LogFormat("[Color Hexagons #{0}] Hex {1} is {2} on {3}.{4}", moduleId, i + 1, diagramDigits[allLocations[i]], colorNames[colorsOnHexes[i]], correctHex == i ? " ← ✓" : "");
                }
            }
            Debug.LogFormat("[Color Hexagons #{0}] Line starts at \"Column\":{1} Row:{2} and goes {3}", moduleId, ((startingLocation%78)-12)/2, (startingLocation/78)-5, dirNames[chosenDirection]);
        }
    }

    void hexPress(KMSelectable pressedHex) {
        pressedHex.AddInteractionPunch();
        for (int i = 0; i < 6; i++) {
            if (!moduleSolved) {
                if (pressedHex == hexes[i]) {
                    if (i == correctHex) {
                        Debug.LogFormat("[Color Hexagons #{0}] You pressed Hex {1}, which is correct. Module solved.", moduleId, i);
                        GetComponent<KMBombModule>().HandlePass();
                        moduleSolved = true;
                        backing.GetComponent<MeshRenderer>().material = backWithTint;
                        hexes[0].GetComponent<MeshRenderer>().material = colorChannels[0];
                        hexes[1].GetComponent<MeshRenderer>().material = colorChannels[0];
                        hexes[2].GetComponent<MeshRenderer>().material = colorChannels[1];
                        hexes[3].GetComponent<MeshRenderer>().material = colorChannels[1];
                        hexes[4].GetComponent<MeshRenderer>().material = colorChannels[2];
                        hexes[5].GetComponent<MeshRenderer>().material = colorChannels[2];
                        red = hexadecimal.IndexOf(diagramDigits[allLocations[0]]) * 16 + hexadecimal.IndexOf(diagramDigits[allLocations[1]]);
                        green = hexadecimal.IndexOf(diagramDigits[allLocations[2]]) * 16 + hexadecimal.IndexOf(diagramDigits[allLocations[3]]);
                        blue = hexadecimal.IndexOf(diagramDigits[allLocations[4]]) * 16 + hexadecimal.IndexOf(diagramDigits[allLocations[5]]);
                        ren.material.color = new Color((float) red/255, (float) green/255, (float) blue/255);
                        rens[0].material.color = new Color((float) red/255, 0f, 0f);
                        rens[1].material.color = new Color((float) red/255, 0f, 0f);
                        rens[2].material.color = new Color(0f, (float) green/255, 0f);
                        rens[3].material.color = new Color(0f, (float) green/255, 0f);
                        rens[4].material.color = new Color(0f, 0f, (float) blue/255);
                        rens[5].material.color = new Color(0f, 0f, (float) blue/255);
                        for (int j = 0; j < 6; j++) {
                            texts[j].color = textColors[1];
                        }
                    } else {
                        Debug.LogFormat("[Color Hexagons #{0}] You pressed Hex {1}, which is incorrect. Strike!", moduleId, i);
                        GetComponent<KMBombModule>().HandleStrike();
                    }
                }
            } else {
                if (pressedHex == hexes[i]) {
                    switch (i) {
                        case 0: red = (red + 16) % 256; break;
                        case 1: red = (red/16)*16 + ((red + 1) % 16); break;
                        case 2: green = (green + 16) % 256; break;
                        case 3: green = (green/16)*16 + ((green + 1) % 16); break;
                        case 4: blue = (blue + 16) % 256; break;
                        case 5: blue = (blue/16)*16 + ((blue + 1) % 16); break;
                        default: break;
                    }
                    ren.material.color = new Color((float) red/255, (float) green/255, (float) blue/255);
                    rens[0].material.color = new Color((float) red/255, 0f, 0f);
                    rens[1].material.color = new Color((float) red/255, 0f, 0f);
                    rens[2].material.color = new Color(0f, (float) green/255, 0f);
                    rens[3].material.color = new Color(0f, (float) green/255, 0f);
                    rens[4].material.color = new Color(0f, 0f, (float) blue/255);
                    rens[5].material.color = new Color(0f, 0f, (float) blue/255);
                    texts[0].text = hexadecimal[red/16].ToString();
                    texts[1].text = hexadecimal[red%16].ToString();
                    texts[2].text = hexadecimal[green/16].ToString();
                    texts[3].text = hexadecimal[green%16].ToString();
                    texts[4].text = hexadecimal[blue/16].ToString();
                    texts[5].text = hexadecimal[blue%16].ToString();
                }
            }
        }
    }

}
