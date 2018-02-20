using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMusicManager : MonoBehaviour {

    public PlayerLifePoints player;
    public GameObject[] pakus;
    public GameObject[] grounds;
    public AudioClip[] baseMusics;
    public AudioClip[] spawnMusics;
    public float spawnWaveWaitTime;
    public GameObject welcomeText;
    public Text scoreText;

    private AudioSource[] musicsAudioSources;
    // 0 -> Base Music
    // 1 -> Spawn Music

    private float timer, spawningTimer;
    private bool spawnStarted, spawning, playerIsReady;
    private int sequenceType;
    private int[] numberOrder;
    private bool[] isUsed;
    private int musicLoopCounter;
    private int lowBoundary;
    private int upperBoundary;

    private const float PERSPAWNWAIT = 4f;
    private const float MOVEWAIT = 2f;
    private const int NUMBEROFSEQUENCE = 2;
    private const int NUMBEROFGROUND = 9;

    //JUST IN CASE MOAL RANDOM
    private int[] OBSTACLESEQUENCE = new int[10] {
        1,2,3,4,5,6,7,8,9,0
    };

    private Color32 GROUNDMAT = new Color32(223, 226, 210, 255);
    private Color32 GROUNDMAT2 = new Color32(180, 130, 180, 255);
    private Color32 ATTENTIONMAT = new Color32(85, 85, 85, 255);

    void Start() {
        spawnStarted = false;
        spawning = false;
        playerIsReady = false;
        timer = 0f;
        spawningTimer = 0f;
        isUsed = new bool[NUMBEROFGROUND];
        for(int i=0; i<NUMBEROFGROUND; i++) {
            isUsed[i] = false;
        }
        musicLoopCounter = 0;
        lowBoundary = 0;
        lowBoundary = 2;
        musicsAudioSources = GetComponents<AudioSource>();
        musicsAudioSources[0].clip = baseMusics[0];
        musicsAudioSources[0].Play();
        musicsAudioSources[1].clip = spawnMusics[0];
        scoreText.text = "Score : 0";
    }

    void Update() {
        if(playerIsReady) {
            float deltaTime = Time.deltaTime;
            timer += deltaTime;
            if(spawnStarted) {
                if(timer >= PERSPAWNWAIT) {
                    if(player.lifePoint > 0) {
                        musicLoopCounter++;
                    }
                    timer -= PERSPAWNWAIT;
                    spawningTimer = 0f;
                    spawning = true;
                    sequenceType = 8;
                    sequenceType = Mathf.FloorToInt(Random.Range(lowBoundary, upperBoundary - 0.1f));
                    // Need to clean this later
                    // Sequence type:
                    // 0, 4 beat normal sequence
                    // 1, 4 beat
                    // 2, 5 beat
                    // 3, 5 beat
                    FillNumberOrder(sequenceType);
                    StartCoroutine(RhythmicAttentionGround(sequenceType, numberOrder));
                    musicsAudioSources[1].clip = spawnMusics[sequenceType];
                    musicsAudioSources[1].Play();
                }
            } else {
                if(timer >= spawnWaveWaitTime) {
                    spawnStarted = true;
                    timer -= spawnWaveWaitTime;
                }
            }

            if(spawning) {
                spawningTimer += deltaTime;
                if(spawningTimer >= MOVEWAIT) {
                    StartCoroutine(MovePaku());
                    spawning = false;
                    if(musicLoopCounter > 16) {
                        //8
                        lowBoundary = 8;
                        upperBoundary = 10;
                    } else if(musicLoopCounter > 14) {
                        //7,8
                        lowBoundary = 6;
                        upperBoundary = 10;
                    } else if(musicLoopCounter > 12) {
                        //6,7,8
                        upperBoundary = 10;
                    } else if(musicLoopCounter > 10) {
                        //6,7
                        lowBoundary = 4;
                        upperBoundary = 8;
                    } else if(musicLoopCounter > 8) {
                        //5,6,7
                        upperBoundary = 8;
                    } else if(musicLoopCounter > 6) {
                        //5,6
                        lowBoundary = 2;
                        upperBoundary = 6;
                    } else if(musicLoopCounter > 4) {
                        //4,5,6
                        upperBoundary = 6;
                    } else if(musicLoopCounter > 2) {
                        //4,5
                        upperBoundary = 4;
                    }
                }
            }
        } else {
            if(Input.GetButtonDown("Jump")) {
                playerIsReady = true;
                musicsAudioSources[0].Stop();
                musicsAudioSources[0].Play();
                welcomeText.SetActive(false);
            }
        }
    }

    IEnumerator MovePaku() {
        pakus[0].GetComponent<AudioSource>().Play();
        for(int i = 0; i<numberOrder.Length; i++) {
            pakus[numberOrder[i]].transform.localPosition += new Vector3(0,  1, 0);
        }
        yield return new WaitForSeconds(1);
        for(int i = 0; i < numberOrder.Length; i++) {
            pakus[numberOrder[i]].transform.localPosition += new Vector3(0, -1, 0);
        }
        ResetGroundColor(numberOrder);
        scoreText.text = "Score : " + musicLoopCounter;
    }

    IEnumerator RhythmicAttentionGround(int sequenceType, int[] groundSeq) {
        switch(sequenceType) {
            case 0:
                for(int j = 0; j < 4; j++) {
                    grounds[groundSeq[j]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                    yield return new WaitForSeconds(0.25f);
                }
                break;
            case 1:
                int i = 0;
                grounds[groundSeq[i]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.5f);
                for(i = 1; i < 4; i++) {
                    grounds[groundSeq[i]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                    yield return new WaitForSeconds(0.25f);
                }
                break;
            case 2:
                int k = 0;
                for(k = 0; k < 3; k++) {
                    grounds[groundSeq[k]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                    yield return new WaitForSeconds(0.25f);
                }
                for(k = 3; k < 5; k++) {
                    grounds[groundSeq[k]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                    yield return new WaitForSeconds(0.10f);
                }
                break;
            case 3:
                int l = 0;
                grounds[groundSeq[l]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.5f);
                for(l = 1; l < 5; l++) {
                    grounds[groundSeq[l]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                    yield return new WaitForSeconds(0.125f);
                }
                break;
            case 4:
                grounds[groundSeq[0]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.25f);
                grounds[groundSeq[1]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.125f);
                grounds[groundSeq[2]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.125f);
                grounds[groundSeq[3]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.25f);
                grounds[groundSeq[4]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.125f);
                grounds[groundSeq[5]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.125f);
                break;
            case 5:
                grounds[groundSeq[1]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.125f);
                grounds[groundSeq[2]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.125f);
                grounds[groundSeq[0]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.25f);
                grounds[groundSeq[4]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.125f);
                grounds[groundSeq[5]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.125f);
                grounds[groundSeq[3]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.25f);
                break;
            case 6:
                grounds[groundSeq[0]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.125f);
                grounds[groundSeq[1]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.125f);
                grounds[groundSeq[2]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.125f);
                grounds[groundSeq[3]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.125f);
                grounds[groundSeq[4]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.125f);
                grounds[groundSeq[5]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.125f);
                grounds[groundSeq[6]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.25f);
                break;
            case 7:
                grounds[groundSeq[0]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.125f);
                grounds[groundSeq[1]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.125f);
                grounds[groundSeq[2]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.25f);
                grounds[groundSeq[3]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.125f);
                grounds[groundSeq[4]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.125f);
                grounds[groundSeq[5]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.125f);
                grounds[groundSeq[6]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                yield return new WaitForSeconds(0.125f);
                break;
            case 8: case 9:
                for(int x = 0; x < 8; x++) {
                    grounds[groundSeq[x]].GetComponent<Renderer>().material.color = ATTENTIONMAT;
                    yield return new WaitForSeconds(0.125f);
                }
                break;
            default:
                break;
        }
    }

    private void ResetGroundColor(int[] groundSeq) {
        for(int i = 0; i < groundSeq.Length; i++) {
            if(groundSeq[i] % 2 == 0) {
                grounds[groundSeq[i]].GetComponent<Renderer>().material.color = GROUNDMAT;
            } else {
                grounds[groundSeq[i]].GetComponent<Renderer>().material.color = GROUNDMAT2;
            }
        }
    }

    private void RandomXNumber(int numberOfRandom) {
        numberOrder = new int[numberOfRandom];
        for(int i = 0; i < NUMBEROFGROUND; i++) {
            isUsed[i] = false;
        }
        for(int i=0; i<numberOrder.Length; i++) {
            numberOrder[i] = Mathf.FloorToInt(Random.Range(0, NUMBEROFGROUND - 0.5f));
            while(isUsed[numberOrder[i]]){
                numberOrder[i] = Mathf.FloorToInt(Random.Range(0, NUMBEROFGROUND - 0.5f));
            } 
            isUsed[numberOrder[i]] = true;
        }
    }

    private void FillNumberOrder(int sequenceTypeNumber) {
        switch(sequenceTypeNumber) {
            case 0: case 1:
                RandomXNumber(4);
                break;
            case 2: case 3:
                RandomXNumber(5);
                break;
            case 4: case 5:
                RandomXNumber(6);
                break;
            case 6: case 7:
                RandomXNumber(7);
                break;
            case 8: case 9:
                RandomXNumber(8);
                break;
        }
    }

}
