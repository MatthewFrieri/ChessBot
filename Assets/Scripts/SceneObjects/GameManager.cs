using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject pawnWhite;
    public GameObject rookWhite;
    public GameObject knightWhite;
    public GameObject bishopWhite;
    public GameObject queenWhite;
    public GameObject kingWhite;

    public GameObject pawnBlack;
    public GameObject rookBlack;
    public GameObject knightBlack;
    public GameObject bishopBlack;
    public GameObject queenBlack;
    public GameObject kingBlack;

    public Dictionary<int, GameObject> pieceToGameObject = new Dictionary<int, GameObject>();

    public GameObject captureIndicator;
    public GameObject moveIndicator;

    private AudioSource audioSource;
    public AudioClip moveAudioClip;
    public AudioClip captureAudioClip;
    public AudioClip checkAudioClip;
    public AudioClip castleAudioClip;
    public AudioClip gameEndAudioClip;

    private void Start()
    {

        audioSource = GetComponent<AudioSource>();

        pieceToGameObject[Piece.Pawn | Piece.White] = pawnWhite;
        pieceToGameObject[Piece.Rook | Piece.White] = rookWhite;
        pieceToGameObject[Piece.Knight | Piece.White] = knightWhite;
        pieceToGameObject[Piece.Bishop | Piece.White] = bishopWhite;
        pieceToGameObject[Piece.Queen | Piece.White] = queenWhite;
        pieceToGameObject[Piece.King | Piece.White] = kingWhite;
        pieceToGameObject[Piece.Pawn | Piece.Black] = pawnBlack;
        pieceToGameObject[Piece.Rook | Piece.Black] = rookBlack;
        pieceToGameObject[Piece.Knight | Piece.Black] = knightBlack;
        pieceToGameObject[Piece.Bishop | Piece.Black] = bishopBlack;
        pieceToGameObject[Piece.Queen | Piece.Black] = queenBlack;
        pieceToGameObject[Piece.King | Piece.Black] = kingBlack;

        Game.Init(180, Piece.Black, pieceToGameObject);
    }

    public void PlayMoveSound()
    {
        audioSource.clip = moveAudioClip;
        audioSource.Play();
    }
    public void PlayCaptureSound()
    {
        audioSource.clip = captureAudioClip;
        audioSource.Play();
    }
    public void PlayCheckSound()
    {
        audioSource.clip = checkAudioClip;
        audioSource.Play();
    }
    public void PlayCastleSound()
    {
        audioSource.clip = castleAudioClip;
        audioSource.Play();
    }
    public void PlayGameEndSound()
    {
        audioSource.clip = gameEndAudioClip;
        audioSource.Play();
    }
}
