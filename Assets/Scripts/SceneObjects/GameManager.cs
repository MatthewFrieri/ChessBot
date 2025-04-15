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


        string pgn = "1. e4 c6 2. b3 d5 3. f3 Nf6 4. e5 Nh5 5. Bb2 Bf5 6. Bd3 Bxd3 7. cxd3 e6 8. Ne2 Bc5 9. Kf1 O-O 10. d4 Be7 11. Qe1 Nd7 12. a3 Nb6 13. d3 f6 14. a4 fxe5 15. dxe5 Bc5 16. Ba3 Bxa3 17. Nxa3 Qg5 18. Ng3 Nf4 19. Qe3 Nd7 20. Ne2 Nxe5 21. Nxf4 Rxf4";

        Game.Init(Piece.Black, pieceToGameObject, pgn);
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
}
